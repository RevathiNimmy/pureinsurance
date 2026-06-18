	SET QUOTED_IDENTIFIER OFF
	GO
	SET ANSI_NULLS ON
	GO
	Execute DDLDropProcedure 'spu_Copy_Reinsurance_Details_To_Claim_RI2007'
	GO

	CREATE PROCEDURE spu_Copy_Reinsurance_Details_To_Claim_RI2007  
  
	  @Claim_id             INT,  
	  @is_balance_and_close TINYINT  = NULL,  
	  @bOpenClaimNoTrans    TINYINT  = NULL,  
	  @Recovery             TINYINT  = 2,  
	  @is_created   TINYINT  = 0  
  
	--This is an new stored procedure and it will gets executed only if the RI2007 is enabled.  
	AS  
	  --A few assumptions:  
	 --1. Reinsurance is Underwriting only  
	 --2. Only risk-level reinsurance is used  
	 DECLARE  -- Working id's  
	 
		@old_claim_id INT,  
		@insurance_file_cnt    INT,  
		@risk_cnt              INT,  
		@catastrophe_code_id   INT,  
		-- Working vars  
		@is_create BIT,  
		@is_deferred           BIT,  
		@retained_share        FLOAT,  
		-- New reserve and payments,  
		@ri_band INT,  
		@this_reserve          MONEY,  
		@this_payment          MONEY,  
		@total_reserve         MONEY,  
		@total_payment         MONEY,  
		@retained_reserve      MONEY,  
		@retained_payment      MONEY,  
		@original_reserve      MONEY,  
		@original_payment      MONEY,  
		-- Ri arrangement details  
		@ri_arrangement_id INT,  
		@claim_allocation_type TINYINT,  
		@is_modified           TINYINT,  
		@model_has_xol         TINYINT,  
		@version_id            INT,  
		@base_id               INT,  
		@Clm_ri_arngmt_id      INT,  
		@rsk_cnt               INT,  
		@ribandID              INT,  
		@product_option        INT,  
		@XOL_Treaty_to_Recover_From int,  
		@Effective_date       datetime,  
		@ri_model_id       int,  
		@model_currency_id     int,  
		@rimodelId          int,  
		@risk_type_id       int,  
		@Reapply_TX         int,  
		@Reapply_Treaty         int,  
		@This_Receipt   Money,  
		@Total_Receipt  Money,  
		@This_Salvage Money,  
		@This_Recovery Money,  
		@ri_model_type INT,  
		@xol_ri_model_type INT,  
	  @RIRegen varchar(20),  
	  @prop_ri_model_id       int,  
	  @prop_model_currency_id int,  
	  @Date_for_Prop_Calculation INT  ,
		@Claim_Payment_id INT,
		@is_ex_gratia TINYINT  = 0,
		@prev_Claim_Status_id TINYINT =0
	  DECLARE  @claim_ri_arrangement_id INT  
	  DECLARE @claim_ri_arrangement_id_for_Broker INT
	  DECLARE @source_id                                  INT
	  DECLARE @policy_currency_id                         SMALLINT
	  DECLARE @policy_currency_rate                       FLOAT
	  DECLARE @model_currency_rate     FLOAT

	  DECLARE @cover_start_date_ForRi DATETIME
	   -- Get currency of policy, and therefore the currency of new ri_arrangement
	  SELECT @insurance_file_cnt = Policy_Id
	  FROM   Claim  
	  WHERE  Claim_Id = @Claim_id  

	  SELECT @policy_currency_id = currency_id,
			 @policy_currency_rate = currency_base_xrate,
			 @source_id = source_id,
			 @cover_start_date_ForRi = inception_date_tpi
	  FROM   insurance_file
	  WHERE  insurance_file_cnt = @insurance_file_cnt

	  -- If policy rate wasn't overridden then get the rate from currencyrate table
	  IF Isnull(@policy_currency_rate, 0) = 0
		EXECUTE Spu_act_get_currency_rate
		  @policy_currency_id,
		  @source_id,
		  @effective_date,
		  @policy_currency_rate OUTPUT

		SELECT @model_currency_rate=1
	   
	   SELECT @product_option='0'  
	   SELECT @product_option=ISNULL(value,0) FROM Hidden_Options WHERE option_number=88  
		Select @Effective_date=CONVERT(VARCHAR(13), loss_from_date, 106) from claim where claim_id=@claim_id  
	   Set @Reapply_TX=0  
	   SET @Reapply_Treaty=0
	 SELECT @RIRegen=value  FROM Hidden_options WHERE option_number=105  
   
	 IF EXISTS(select null from Claim_pt_log where claim_id=@Claim_id)  
	 SELECT @is_created=1  
  
	 EXEC spu_clm_Get_Claim_Version  
	  @claim_id = @claim_id ,  
	  @version_id = @version_id OUTPUT ,  
	  @base_id = @base_id OUTPUT  
  
	 -- Initialise deferred reinsurance flag  
	 SELECT @is_deferred = 0  
  
	 -- Get some working id's  
	 -- if the version is not 1 then this is is not open claim (C_CO)  
	 IF IsNull(@version_id,1) <> 1  
	 BEGIN  
	 DECLARE  @prev_version_id INT  
  
	  -- set previous version number  
	  SET @prev_version_id = @version_id - 1  
  
	  -- get the last version of the claim  
	   SELECT @old_claim_id = Claim_Id , @prev_Claim_Status_id =Claim_Status_id 
	   FROM   Claim  
	   WHERE  Version_Id = @prev_version_id  
	   AND Base_Claim_Id = @base_id AND is_dirty = 0 
	 END  
	 ELSE  
	 -- if the version is 1 this is is open claim (C_CO)  
	 SELECT @old_claim_id = NULL  
  
	 SELECT @insurance_file_cnt = Policy_Id,  
	   @risk_cnt = Risk_Type_Id,  
	   @catastrophe_code_id = Catastrophe_Code_Id  
	 FROM   Claim  
	 WHERE  Claim_Id = @Claim_id  
  
  
	 -- Get the percentage not covered by coinsurance  
	 SELECT @retained_share = (100 - Isnull(SUM(Share),0)) / 100  
	 FROM   Claim_Party  
	 WHERE  Claim_Id = @claim_id  
	 AND Insurer_Type = 0  
 
	 SELECT @Claim_Payment_id = MAX(Claim_Payment_id) 
	 FROM  Claim_Payment 
	 WHERE claim_id = @claim_id
 
	 SELECT @is_ex_gratia = is_ex_gratia 
	 FROM Claim_Payment 
	 WHERE claim_payment_id = @Claim_Payment_id
 
	  IF @is_ex_gratia <> 1 
	 BEGIN
	 -- Check if we should be creating the ri for the first time  
	 IF @old_claim_id IS NULL  OR @prev_Claim_Status_id =1
	 SELECT @is_create = 1  
	 ELSE  
	 IF NOT EXISTS (SELECT *  
		 FROM   Claim_ri_Arrangement  
		 WHERE  Claim_Id = @Claim_id)  
		 BEGIN
					 SELECT @is_create = 1
				   -- set deferred reinsurance flag to indicate that there could be existing reserves / payments that require
				   -- allocation - not just those added in this session and omit this for regeneration process
					  IF (ISNULL(@RIRegen,0)!='1') SELECT @is_deferred = 1
		 END
	IF (ISNULL(@bOpenClaimNoTrans, 0) = 1)  
	   SELECT @is_deferred = 1  
  
	 -- Clear down existing data  
	 DELETE FROM Claim_RI_Arrangement_line_Broker_Participants WHERE claim_ri_arrangement_line_id in  
	 (SELECT ri_arrangement_line_id FROM  Claim_ri_Arrangement_Line  
	 WHERE  Claim_Id  = @Claim_id)  
  
	 DELETE Claim_ri_Arrangement_Line  
	 WHERE  Claim_Id = @Claim_id  
  
	 DELETE Claim_xol_Arrangement  
	 WHERE  Claim_Id = @Claim_id  
  
	 DELETE Claim_ri_Arrangement  
	 WHERE  Claim_Id = @Claim_id  
  
	-- IF no lines where created during policies when nothing should be done on claims
	IF NOT EXISTS (SELECT NULL FROM RI_Arrangement_Line WHERE ri_arrangement_id in  
	  (SELECT ri_arrangement_id FROM RI_Arrangement WHERE risk_cnt=@risk_cnt))
	RETURN

	 -- Should we create or copy?  
	 IF @is_create = 1  
	 BEGIN  
  
	  INSERT INTO Claim_ri_Arrangement  
		 (Claim_Id,  
		  ri_Arrangement_Id,  
		  Risk_cnt,  
		  ri_Band_Id,  
		  ri_Model_Id,  
		  Claim_Allocation_Type,  
		  Sum_Insured,  
		  Reserve,  
		  Payment,  
		  Salvage,  
		  Recovery,  
		  This_Reserve,  
		  This_Payment,  
		  This_Salvage,  
		  This_Recovery,  
		  Is_modIfied,  
		  Version_Id,  
		  Original_ri_Arrangement_Id,  
		Ri_arrangement_version, xol_ri_model_id)  
	  SELECT @claim_id,  
		ra.ri_Arrangement_Id,  
		ra.Risk_cnt,  
		ra.ri_Band_Id,  
		ra.ri_Model_Id,  
		Isnull(rm.Claim_Allocation_Type,0),  
		ra.Sum_Insured,  
		0,  
		0,  
		0,  
		0,  
		0,  
		0,  
		0,  
		0,  
		0,  -- All zero for new ri  
		@version_id,  
		ra.ri_Arrangement_Id,  
	  1,xol_ri_model_id  
	  FROM   ri_Arrangement ra  
		LEFT JOIN ri_Model rm  
		ON rm.ri_Model_Id = ra.ri_Model_Id  
		WHERE  ra.Risk_cnt = @risk_cnt  
		AND ra.Original_Flag = 0  
		AND ri_band_id is not null
		AND ra.version_id = @version_id

	  SET @claim_ri_arrangement_id = @@IDENTITY  
  
	  -- set the base_claim_ri_arrangement_id to indicate this is the initial version  
	  UPDATE Claim_ri_Arrangement  
	  SET    Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement_Id,  
		ri_Arrangement_Id = Claim_ri_Arrangement_Id  
	  WHERE  Claim_Id = @claim_id  
  
	  DECLARE Upd_clm_ri_Lines CURSOR  FAST_FORWARD FOR  
	  SELECT Claim_ri_Arrangement_Id,  
		Risk_cnt,  
	 ri_Band_Id,  
		ri_model_id  
	  FROM   Claim_ri_Arrangement  
	  WHERE  Claim_Id = @CLAIM_ID  
  
	  OPEN Upd_clm_ri_Lines  
  
	  FETCH NEXT FROM Upd_clm_ri_Lines  
	  INTO @Clm_ri_arngmt_id,  
		@rsk_cnt,  
		@ribandID,  
	  @rimodelid  
  
	  WHILE @@FETCH_STATUS = 0  
	  BEGIN  
	  -- If original claim is null we're in open claim and so we copy the existing RI structure  
	   -- If the original claim is not null but there is no ri structure then this could be  
	   -- defered reinsurance; attempt to add the ri details; worst case is nothing will be done  
	   -- NotE:  
	   --    If we were implementing a different RI model for claims this is where you should  
	   --    insert the code.  

	   -- Select @ribandid=ri_band_id from ri_arrangement where risk_cnt=@risk_cnt  
      SELECT TOP 1  @XOL_Treaty_to_Recover_From=XOL_Treaty_to_Recover_From_id     
      FROM RI_Band_Version      
      WHERE ri_band_id=@ribandid        
      AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)      
       ORDER BY effective_date DESC        
	
	   Select @risk_type_id=risk.risk_type_id from risk,claim where risk_cnt=claim.risk_type_id and claim_id=@claim_id  
    
		SELECT TOP 1 @Date_for_Prop_Calculation= Proportional_RI_Cal_Method        
       FROM   RI_Band_Version        
       WHERE  ri_band_id = @ribandid        
       AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)          
       ORDER BY effective_date DESC        

		IF @Date_for_Prop_Calculation =2 Or @XOL_Treaty_to_Recover_From = 2  
		SET @RIRegen =1  
	   if @product_option=1 and @Date_for_Prop_Calculation = 2 and ISNULL(@RIRegen,0)='1'  
	   BEGIN  
			   Select  @prop_ri_model_id = rmu.ri_model_id,  
					   @prop_model_currency_id = currency_id  
			   From    risk_type_ri_model_usage rmu  
			   Join    ri_model rm  
					   On rm.ri_model_id = rmu.ri_model_id  
			   Where   rmu.risk_type_id = @risk_type_id  
			   And     rmu.ri_band = @ribandid  
			   And     rmu.is_deleted = 0  
		 And     rmu.effective_date <= CONVERT(varchar(13), GETDATE(), 106)  
		 And    (rmu.expiry_date >= CONVERT(varchar(13), GETDATE(), 106) or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')  
			   And     rm.ri_model_type IN (0,4)  
			   And     rm.is_deleted = 0  
		 And     rm.effective_date <= CONVERT(varchar(13), GETDATE(), 106)  
			   And    (rm.expiry_date >= CONVERT(varchar(13), GETDATE(), 106) or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')  
			   Order By  
					   rm.ri_model_type Desc,    -- give priority to none-deferred models  
					   rmu.effective_date ASC   -- give priority to newer models  
	   END  
  
	   if @product_option=1 and @XOL_Treaty_to_Recover_From = 2  
	   BEGIN  
  
		   Select  @ri_model_id = rmu.ri_model_id,  
				   @model_currency_id = currency_id  
		   From    risk_type_ri_model_usage rmu  
		   Join    ri_model rm  
				   On rm.ri_model_id = rmu.ri_model_id  
		   Where   rmu.risk_type_id = @risk_type_id  
		   And     rmu.ri_band = @ribandid  
		   And     rmu.is_deleted = 0  
		   And     rmu.effective_date <= @effective_date  
		   And    (rmu.expiry_date >= @effective_date or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')  
		   And     rm.ri_model_type IN (0,4)  
		   And     rm.is_deleted = 0  
		   And     rm.effective_date <= @effective_date  
		   And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')  
		   Order By  
				   rm.ri_model_type Desc,    -- give priority to none-deferred models  
				   rmu.effective_date ASC   -- give priority to newer models  
	   END  
	 -- E007  
		   Select  @ri_model_type = rm.ri_model_type  
		   From    risk_type_ri_model_usage rmu  
		   Join    ri_model rm  
				   On rm.ri_model_id = rmu.ri_model_id  
		   Where   rmu.risk_type_id = @risk_type_id  
		   And     rmu.ri_band = @ribandid  
		   And     rmu.is_deleted = 0  
		   And     rmu.effective_date <= @effective_date  
		   And    (rmu.expiry_date >= @effective_date or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')  
		   And     rm.is_deleted = 0  
		   And     rm.effective_date <= @effective_date  
		   And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')  
		   Order By  
				   rm.ri_model_type Desc,  
				   rmu.effective_date ASC 
				   
		---Get currency rate from ri model currency table
		SELECT @policy_currency_rate =   
    ISNULL((  
        SELECT conversion_rate  
        FROM RIModelCurrencyRates  
        WHERE currency_id = @policy_currency_id AND ri_model_id = @ri_model_id  
    ), @policy_currency_rate)  
  

				   IF @model_currency_id <> @policy_currency_id
		BEGIN
		   EXECUTE Spu_act_get_currency_rate
			  @model_currency_id,
			  @source_id,
			  @effective_date,
			  @model_currency_rate OUTPUT

					   SELECT @model_currency_rate = @model_currency_rate /
										  @policy_currency_rate
		END
	  ELSE
		SELECT @model_currency_rate = 1

	  -- Insert the lines  
	  IF  ISNULL(@RIRegen,0)='1'  
	  BEGIN  
	  IF @product_option=1 and @Date_for_Prop_Calculation = 2  
		  BEGIN  
	   SET @Reapply_Treaty=1  
	   -- set the ri_model_id if Recovery Type is Loss date and old RI MODEL IS diffenent from CURRENT.  
		UPDATE Claim_ri_Arrangement  
		SET    ri_model_id = @prop_ri_model_id  
		WHERE  Claim_ri_arrangement_Id = @Clm_ri_arngmt_id  AND Claim_Id = @claim_id
			exec Spu_copy_Reinsurance_Treaty_Details @claim_id=@claim_id ,  
								  @risk_cnt= @risk_cnt,  
								  @ri_band_id=@ribandID ,  
								  @ri_model_id=@prop_ri_model_id,  
								  @claim_ri_arrangement_id=@Clm_ri_arngmt_id,  
								  @version_id=@version_id,
								  @model_currency_rate=@model_currency_rate  
		  END  
      

	  ELSE  
		  BEGIN  
			 INSERT INTO Claim_ri_Arrangement_Line  
				(Claim_Id,  
				 ri_Arrangement_Line_Id,  
				 ri_Arrangement_Id,  
				 TYPE,  
				 Treaty_Id,  
				 Party_cnt,  
				 xol_Arrangement_Id,  
				 Default_Share_Percent,  
				 This_Share_Percent,  
				 Agreement_Code,  
				 Priority,  
				 Number_Of_Lines,  
				 Line_Limit,  
				 Sum_Insured,  
				 Reserve,  
				 Payment,  
				 Salvage,  
				 Recovery,  
				 This_Reserve,  
				 This_Payment,  
				 This_Salvage,  
				 This_Recovery,  
				 Version_Id,  
				 Original_ri_Arrangement_Line_Id,  
									  lower_limit,  
									  Retained,  
				 participation_percent,  
									  Grouping, is_obligatory , ri_model_line_id, manually_added, is_edited)  
			 SELECT @claim_id,  
			   ral.ri_Arrangement_Line_Id,  
			   @Clm_ri_arngmt_id,  
			   ral.TYPE,  
			   ral.Treaty_Id,  
			   ral.Party_cnt,  
			   NULL,  
		 Case  
			   WHEN ((ral.This_Share_Percent = NULL  
					OR ral.This_Share_Percent = 0
					OR ral.type ='FX')  
				   AND ral.Premium_Value <> 0) THEN ral.Default_Share_Percent  
		--Start Girija - PN55676  
		--WHEN ral.type='T' THEN ral.Default_Share_Percent  
		--End Girija - PN55676  
			ELSE ral.This_Share_Percent  
		END, -- Default percent is the share From NB  
	   ral.This_Share_Percent,  
			   ral.Agreement_Code,  
			   ral.Priority,  
			   ral.Number_Of_Lines,  
			   ral.Line_Limit *@model_currency_rate,  
			   ral.Sum_Insured,  
			   0,  
			   0,  
			   0,  
			   0,  
	 0,  
			   0,  
			   0,  
			   0,  -- All zero for new ri  
			   @version_id,  
			   ral.ri_Arrangement_Line_Id,  
			   ral.Lower_limit *@model_currency_rate,  
			   ral.retained,  
			   ral.participation_percent,  
			   ral.Grouping,ral.is_obligatory , ral.ri_model_line_id,ral.manually_added, ISNULL(ral.is_edited, 0)
			 FROM   ri_Arrangement_Line ral  
			   JOIN ri_Arrangement ra  
				ON ra.ri_Arrangement_Id = ral.ri_Arrangement_Id  
	   WHERE  ra.Risk_cnt = @rsk_cnt  
			 AND ra.ri_Band_Id = @ribandID  
			 AND ra.Original_Flag = 0 AND ral.type not in ('TX','TC','R')  
			 AND ra.version_id= @version_id  
		  END  
	   END  
	   IF @product_option=1 and @XOL_Treaty_to_Recover_From = 2  
		  BEGIN  
			 SET @Reapply_TX = 1  
	   -- set the ri_model_id if Recovery Type is Loss date and old RI MODEL IS diffenent from CURRENT.  
		UPDATE Claim_ri_Arrangement  
		SET    xol_ri_model_id = @ri_model_id  
		WHERE  Claim_ri_arrangement_Id = @Clm_ri_arngmt_id   AND Claim_Id = @claim_id
    
			exec Spu_copy_Reinsurance_TreatyXOl_Details @claim_id=@claim_id ,  
								  @risk_cnt= @risk_cnt,  
								  @ri_band_id=@ribandID ,  
								  @ri_model_id=@ri_model_id,  
								  @claim_ri_arrangement_id=@Clm_ri_arngmt_id,  
								  @version_id=@version_id ,
								  @model_currency_rate=@model_currency_rate 
		  END  
	  ELSE  
		  BEGIN  
			 INSERT INTO Claim_ri_Arrangement_Line  
				(Claim_Id,  
				 ri_Arrangement_Line_Id,  
				 ri_Arrangement_Id,  
				 TYPE,  
				 Treaty_Id,  
				 Party_cnt,  
				 xol_Arrangement_Id,  
				 Default_Share_Percent,  
				 This_Share_Percent,  
				 Agreement_Code,  
				 Priority,  
				 Number_Of_Lines,  
				 Line_Limit,  
				 Sum_Insured,  
		  Reserve,  
				 Payment,  
				 Salvage,  
				 Recovery,  
				 This_Reserve,  
				 This_Payment,  
				 This_Salvage,  
				 This_Recovery,  
				 Version_Id,  
				 Original_ri_Arrangement_Line_Id,  
									  lower_limit,  
									  Retained,  
				 participation_percent,  
									  Grouping, is_obligatory , ri_model_line_id, manually_added, is_edited)  
			 SELECT @claim_id,  
			   ral.ri_Arrangement_Line_Id,  
			   @Clm_ri_arngmt_id,  
			   ral.TYPE,  
			   ral.Treaty_Id,  
			   ral.Party_cnt,  
			   NULL,  
		 Case  
			   WHEN ((ral.This_Share_Percent = NULL  
					OR ral.This_Share_Percent = 0)  
				   AND ral.Premium_Value <> 0) THEN ral.Default_Share_Percent  
		--Start Girija - PN55676  
		--WHEN ral.type='T' THEN ral.Default_Share_Percent  
		--End Girija - PN55676  
			ELSE ral.This_Share_Percent  
		END, -- Default percent is the share From NB  
	   ral.This_Share_Percent,  
			   ral.Agreement_Code,  
			   ral.Priority,  
			   ral.Number_Of_Lines,  
			   ral.Line_Limit * @model_currency_rate,  
			   ral.Sum_Insured,  
			   0,  
			   0,  
			   0,  
			   0,  
	 0,  
			   0,  
			   0,  
			   0,  -- All zero for new ri  
			   @version_id,  
			   ral.ri_Arrangement_Line_Id,  
			   ral.Lower_limit * @model_currency_rate,  
			   ral.retained,  
			   ral.participation_percent,  
			   ral.Grouping,  
			   ral.is_obligatory,  
			   ral.ri_model_line_id,  
			   ISNULL(ral.manually_added,0),  
			   ISNULL(ral.is_edited, 0)  
			 FROM   ri_Arrangement_Line ral  
			   JOIN ri_Arrangement ra  
				ON ra.ri_Arrangement_Id = ral.ri_Arrangement_Id  
			 WHERE  ra.Risk_cnt = @rsk_cnt  
			 AND ra.ri_Band_Id = @ribandID  
			 AND ra.Original_Flag = 0  AND (ral.type in ('TX','TC','R') OR (NOT (ISNULL(@RIRegen,0)='1')))  
			 AND ra.version_id= @version_id  
  
		END  
    
	  SELECT @ri_model_type=ri_model_type from RI_Model WHERE ri_model_id = (SELECT ISNULL(ri_model_id,0) from Claim_ri_arrangement WHERE claim_ri_arrangement_id = @Clm_ri_arngmt_id)  
	  SELECT @xol_ri_model_type=ri_model_type from RI_Model WHERE ri_model_id = (SELECT ISNULL(xol_ri_model_id,0) from Claim_ri_arrangement WHERE claim_ri_arrangement_id = @Clm_ri_arngmt_id)  
  
	  IF @ri_model_type = 4 OR @xol_ri_model_type = 4  
	 BEGIN  
	  UPDATE  Claim_ri_arrangement  
	  SET     Cloned = 1  
	  WHERE   claim_ri_arrangement_id = @Clm_ri_arngmt_id   AND Claim_Id = @claim_id
		END  
		ELSE  
		BEGIN  
		 UPDATE  Claim_ri_arrangement  
	 SET     Cloned = 0  
	  WHERE   claim_ri_arrangement_id = @Clm_ri_arngmt_id   AND Claim_Id = @claim_id
    
		  END  
  
	   FETCH NEXT FROM Upd_clm_ri_Lines  
	   INTO @Clm_ri_arngmt_id,  
		 @rsk_cnt,  
		 @ribandID,  
	   @rimodelid  
	  END  
  
	  CLOSE Upd_clm_ri_Lines  
	  DEALLOCATE Upd_clm_ri_Lines  
  
	  -- set the base_claim_ri_arrangement_line_id to indicate this is the initial version  
	  UPDATE Claim_ri_Arrangement_Line  
	  SET    Base_Claim_ri_Arrangement_Line_Id = Claim_ri_Arrangement_Line_Id,  
		ri_Arrangement_Line_Id = Claim_ri_Arrangement_Line_Id  
	  WHERE  Claim_Id = @claim_id  
    
        
		END  
	 ELSE  --When is_create<>1  
	 BEGIN  
	 INSERT INTO Claim_ri_Arrangement  
		 (Claim_Id,  
		  ri_Arrangement_Id,  
		  Risk_cnt,  
		  ri_Band_Id,  
		  ri_Model_Id,  
		  Claim_Allocation_Type,  
		  Sum_Insured,  
		  Reserve,  
		  Payment,  
		  Salvage,  
		Recovery,  
		  This_Reserve,  
		  This_Payment,  
		  This_Salvage,  
		  This_Recovery,  
		  Is_modIfied,  
		  Base_Claim_ri_Arrangement_Id,  
		  Version_Id,  
		  Original_ri_Arrangement_Id,  
		Ri_Arrangement_version, xol_ri_model_id,cloned,
		incurred_to_date ,
		reserve_to_date ,
		payment_to_date ,
		salvage_to_date,
		recovery_to_date )  
	  SELECT @claim_id,  
		ri_Arrangement_Id,  
		Risk_cnt,  
		ri_Band_Id,  
		ri_Model_Id,  
		Claim_Allocation_Type,  
		Sum_Insured,  
		case @is_created when 1 then 0 else Reserve  end,  
		case @is_created when 1 then 0 else Payment end,  
		case @is_created when 1 then 0 else Salvage end,  
		case @is_created when 1 then 0 else Recovery end,  
		0,  
		0,  
		0,  
		0, -- Only zero the new 'this' amounts  
		0,  
		Base_Claim_ri_Arrangement_Id,  
		@version_id,  
		Original_ri_Arrangement_Id,  
	  ri_arrangement_version, xol_ri_model_id,cloned,
	  incurred_to_date,
		 reserve_to_date   ,  
		 payment_to_date  ,  
		salvage_to_date  ,  
		recovery_to_date    
	  FROM   Claim_ri_Arrangement  
	  WHERE  Claim_Id = @old_claim_id  
  
	  UPDATE Claim_ri_Arrangement  
	  SET    ri_Arrangement_Id = Claim_ri_Arrangement_Id  
	  WHERE  Claim_Id = @claim_id  
  
	  -----------------------------------------------------  
	  -- Copy Claim_xol_arrangement table - reinsurance  
	  -----------------------------------------------------  
	  INSERT INTO Claim_xol_Arrangement  
		 (xol_Arrangement_Id,  
		  Claim_Id,  
		  ri_Arrangement_Id,  
		  Catastrophe_Code_Id,  
		  Layer,  
		  ri_Model_Id,  
		  Trigger_Limit,  
		  Base_Claim_xol_Arrangement_Id,  
		  Version_Id)  
	  SELECT xol_Arrangement_Id,  
		@claim_id,  
		Copy_Claim_ri_Arrangement.ri_Arrangement_Id,  
		Claim_xol_Arrangement.Catastrophe_Code_Id,  
		Claim_xol_Arrangement.Layer,  
		Claim_xol_Arrangement.ri_Model_Id,  
		Claim_xol_Arrangement.Trigger_Limit,  
		Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id,  
		@version_id  
	  FROM   Claim_xol_Arrangement  
		LEFT JOIN Claim_ri_Arrangement  
		 ON Claim_xol_Arrangement.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
		LEFT JOIN (SELECT ri_Arrangement_Id,  
			 Version_Id,  
			 Base_Claim_ri_Arrangement_Id  
			FROM   Claim_ri_Arrangement  
			WHERE  Version_Id = @version_id  
	   AND Claim_Id = @claim_id) Copy_Claim_ri_Arrangement  
		 ON Copy_Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id  
	  WHERE  Claim_xol_Arrangement.Claim_Id = @old_claim_id  
  
	  UPDATE Claim_xol_Arrangement  
	  SET    xol_Arrangement_Id = Claim_xol_Arrangement_Id  
	  WHERE  Claim_Id = @claim_id  
	  -----------------------------------------------------  
	  -- Copy Claim_ri_arrangement_line table - reinsurance  
	  -----------------------------------------------------  
	  INSERT INTO Claim_ri_Arrangement_Line  
		 (Claim_Id,  
		  ri_Arrangement_Line_Id,  
		  ri_Arrangement_Id,  
		  TYPE,  
		  Treaty_Id,  
		  Party_cnt,  
		  xol_Arrangement_Id,  
		  Default_Share_Percent,  
	 This_Share_Percent,  
		  Agreement_Code,  
		  Priority,  
		  Number_Of_Lines,  
		  Line_Limit,  
		  Sum_Insured,  
		  Reserve,  
		  Payment,  
		  Salvage,  
		  Recovery,  
		  This_Reserve,  
		  This_Payment,  
		  This_Salvage,  
		  This_Recovery,  
		  Base_Claim_ri_Arrangement_Line_Id,  
		  Version_Id,  
		  Original_ri_Arrangement_Line_Id,  
							lower_limit,  
							Retained,  
	   participation_percent,  
		  Grouping,
		  is_obligatory ,
		  ri_model_line_id ,
		  manually_added,
		  reserve_to_date ,
		  payment_to_date ,
		  salvage_to_date ,
		  recovery_to_date,
		  claim_incurred_to_date,
		  is_pt_archive,
		  is_edited  )  
	  SELECT @claim_id,  
		Claim_ri_Arrangement_Line.ri_Arrangement_Line_Id,  
		Copy_Claim_ri_Arrangement.ri_Arrangement_Id,  
		Claim_ri_Arrangement_Line.TYPE,  
		Claim_ri_Arrangement_Line.Treaty_Id,  
		Claim_ri_Arrangement_Line.Party_cnt,  
		Copy_Claim_xol_Arrangement.xol_Arrangement_Id,  
		Claim_ri_Arrangement_Line.Default_Share_Percent,  
	Claim_ri_Arrangement_Line.This_Share_Percent,  
	  Claim_ri_Arrangement_Line.Agreement_Code,  
		Claim_ri_Arrangement_Line.Priority,  
		Claim_ri_Arrangement_Line.Number_Of_Lines,  
		Claim_ri_Arrangement_Line.Line_Limit * @model_currency_rate,  
		Claim_ri_Arrangement_Line.Sum_Insured,  
		case when type in ('F') and @is_created=1 then 0 else (Claim_ri_Arrangement_Line.Reserve) end,  
		case when type in ('F') and @is_created=1 then 0 else Claim_ri_Arrangement_Line.Payment end,  
		case when type in ('F') and @is_created=1 then 0 else Claim_ri_Arrangement_Line.Salvage end,  
		case when type in ('F') and @is_created=1 then 0 else Claim_ri_Arrangement_Line.Recovery end,  
	case when type in ('FX','TX','TC','R') and @is_created=1 then -Claim_ri_Arrangement_Line.this_reserve else 0 end,  
		0,  
		0,  
		0, -- Only zero the new 'this' amounts  
		Claim_ri_Arrangement_Line.Base_Claim_ri_Arrangement_Line_Id,  
		@version_id,  
		Original_ri_Arrangement_Line_Id,  
		Claim_ri_Arrangement_Line.lower_limit * @model_currency_rate,  
					Claim_ri_Arrangement_Line.Retained,  
					Claim_ri_Arrangement_Line.participation_percent,  
					Claim_ri_Arrangement_Line.Grouping,Claim_ri_Arrangement_Line.is_obligatory ,  
					Claim_ri_Arrangement_Line.ri_model_line_id,
					Claim_ri_Arrangement_Line.manually_added,
		 case when type in ('F') and @is_created=1 then 0 else            Claim_ri_Arrangement_Line.reserve_to_date end,
		 case when type in ('F') and @is_created=1 then 0 else           Claim_ri_Arrangement_Line.payment_to_date end,
		 case when type in ('F') and @is_created=1 then 0 else           Claim_ri_Arrangement_Line.salvage_to_date end,
		 case when type in ('F') and @is_created=1 then 0 else           Claim_ri_Arrangement_Line.recovery_to_date end,
					 Claim_ri_Arrangement_Line.claim_incurred_to_date ,
					0,
					Claim_ri_Arrangement_Line.is_edited  
	  FROM   Claim_ri_Arrangement_Line  
		LEFT JOIN Claim_ri_Arrangement  
		 ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
		LEFT JOIN (SELECT ri_Arrangement_Id,  
			 Version_Id,  
			 Base_Claim_ri_Arrangement_Id  
			FROM   Claim_ri_Arrangement  
			WHERE  Version_Id = @version_id  
			AND Claim_Id = @claim_id) Copy_Claim_ri_Arrangement  
		 ON Copy_Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id  
		LEFT JOIN Claim_xol_Arrangement  
		 ON Claim_ri_Arrangement_Line.xol_Arrangement_Id = Claim_xol_Arrangement.xol_Arrangement_Id  
		LEFT JOIN (SELECT xol_Arrangement_Id,  
			 Version_Id,  
			 Base_Claim_xol_Arrangement_Id  
			FROM   Claim_xol_Arrangement  
			WHERE  Version_Id = @version_id  
			AND Claim_Id = @claim_id) Copy_Claim_xol_Arrangement  
		 ON Copy_Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id = Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id  
	  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id  AND (type in ('F','FX','TX','TC','R')   or @is_created=2 ) 
		AND  Claim_ri_Arrangement_Line.is_pt_archive =0
  
	  IF @is_created<>2   BEGIN  
	  DECLARE Upd_clm_ri_Lines CURSOR FAST_FORWARD FOR  
	  SELECT Claim_ri_Arrangement_Id,  
		Risk_cnt,  
		ri_Band_Id,  
		ri_model_id  
	  FROM   Claim_ri_Arrangement  
	  WHERE  Claim_Id = @CLAIM_ID  
  
	 OPEN Upd_clm_ri_Lines  
  
	 FETCH NEXT FROM Upd_clm_ri_Lines  
	   INTO @Clm_ri_arngmt_id,  
	  @rsk_cnt,  
	  @ribandID,  
	   @rimodelid  
  
	 WHILE @@FETCH_STATUS = 0  
	 BEGIN  
  
		 SELECT top 1  @Date_for_Prop_Calculation=Proportional_RI_Cal_Method  
	 	 FROM   RI_Band_Version  
		 WHERE  ri_band_id = @ribandid 
		 AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)      
         ORDER BY effective_date DESC        
	 
  
	  Select @risk_type_id=risk.risk_type_id from risk,claim where risk_cnt=claim.risk_type_id and claim_id=@claim_id  
  
	  IF @product_option=1 and @Date_for_Prop_Calculation = 2  
	  BEGIN  
		SET @Reapply_Treaty=1  
		SET @Reapply_TX=1  
  
		Select  @prop_ri_model_id = rmu.ri_model_id,  
		  @prop_model_currency_id = currency_id,  
		  @ri_model_type=ri_model_type  
		From    risk_type_ri_model_usage rmu  
		Join    ri_model rm  
		  On rm.ri_model_id = rmu.ri_model_id  
		Where   rmu.risk_type_id = @risk_type_id  
		And     rmu.ri_band = @ribandid  
		And     rmu.is_deleted = 0  
		And     rmu.effective_date <= CONVERT(varchar(13), GETDATE(), 106)  
		And    (rmu.expiry_date >= CONVERT(varchar(13), GETDATE(), 106) or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')  
		And     rm.ri_model_type IN (0,4)  
		And     rm.is_deleted = 0  
		And     rm.effective_date <= CONVERT(varchar(13), GETDATE(), 106)  
		And    (rm.expiry_date >= CONVERT(varchar(13), GETDATE(), 106) or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')  
		Order By  
		  rm.ri_model_type Desc,    -- give priority to none-deferred models  
		  rmu.effective_date ASC   -- give priority to newer models  
  
		   UPDATE Claim_ri_Arrangement  
		SET    ri_model_id = @prop_ri_model_id  
		WHERE  Claim_ri_arrangement_Id = @Clm_ri_arngmt_id   AND Claim_Id = @claim_id
        
		EXEC spu_copy_claim_ri_line_for_pt @claim_id,@version_id ,@old_claim_id ,@ribandID , @is_created      
     
	 IF @ri_model_type = 4  
	   BEGIN  
		UPDATE  Claim_ri_arrangement  
		SET     Cloned = 1  
		WHERE   claim_ri_arrangement_id = @Clm_ri_arngmt_id   AND Claim_Id = @claim_id
	   END  
	 ELSE  
		BEGIN  
		 UPDATE  Claim_ri_arrangement  
	  SET     Cloned = 0  
		 WHERE   claim_ri_arrangement_id = @Clm_ri_arngmt_id   AND Claim_Id = @claim_id
		END  
  
      
		declare @counter int  
		SELECT @counter=isnull(max(claim_ri_arrangement_line_id),0) + 1  
	   FROM claim_ri_arrangement_line  
  
		 INSERT INTO Claim_ri_Arrangement_Line (  
			Claim_Id,  
			ri_Arrangement_Line_Id,  
			ri_Arrangement_Id,  
			TYPE,  
			Treaty_Id,  
			Default_Share_Percent,  
	  This_Share_Percent,  
		 Agreement_Code,  
	 Priority,  
	  Number_Of_Lines,  
			Line_Limit,  
			lower_limit,  
			Sum_Insured,  
			Reserve,  
			Payment,  
			Salvage,  
			Recovery,  
			This_Reserve,  
			This_Payment,  
			This_Salvage,  
			This_Recovery,  
			Version_Id,  
			Original_ri_Arrangement_Line_Id,  
			Retained,  
			participation_percent,  
			Grouping,ri_model_line_id,
			is_obligatory)  
  
	   Select distinct @claim_id,@counter+rml.ri_model_line_id,@Clm_ri_arngmt_id,  
		Case  
	  When rt.code = 'RET' Then 'R'  
	  When rt.code = 'XOL'  Then  'TX'  
	  When rt.code = 'CAT' Then 'TC'  
	  When rt.code = '001' Then 'TFS'  
	  Else 'T'  
		End,  
	   rml.treaty_id,  
	   rml.share_percent,  
	   isnull(rml.ceding_rate,0),  
	   t.agreement_code,  
	   rml.priority,  
	   rml.number_of_lines,  
	   rml.line_limit * @model_currency_rate,  
	   rml.lower_limit * @model_currency_rate,
	   RAl.sum_insured,
	   0,
	   0,
	   0,
	   0,
	   0,
	   0,
	   0,
	   0,
	   @version_id,null,null,null,null,rml.ri_model_line_id  ,RAL.Is_Obligatory
	   From    ri_model_line rml  
		Join    Treaty t  
	   On t.treaty_id = rml.treaty_id  
		Join    Reinsurance_type rt  
	   On rt.reinsurance_type_id = t.reinsurance_type_id  
	   Left Join  
	   (Select  treaty_id,  
		  Sum(commission_percent * (share_percent / 100)) commission_percent  
		From    treaty_party  
		Group By  
		  treaty_id) tc  
		On tc.treaty_id = t.treaty_id  
	   Join ri_arrangement_line RAL ON RAL.ri_model_line_id = rml.ri_model_line_id 
			join RI_Arrangement RA ON RA.ri_arrangement_id = RAL.ri_arrangement_id and RA.risk_cnt =@risk_cnt and ra.original_flag = 0
	   Where   ra.ri_model_id = @prop_ri_model_id  
	   AND RTRIM(rt.code) NOT IN ('XOL','CAT','RET') AND ra.original_flag = 0
	   AND ra.ri_band_id = @ribandid    
  
	  DECLARE  @reserve money, @recover_todate money, @salvage money, @payment money  
	  if @is_created <>1 BEGIN  
  
  
  SELECT @reserve=SUM(isnull(Claim_ri_Arrangement_Line.reserve,0)),@payment=SUM(isnull(Claim_ri_Arrangement_Line.payment,0)),  
  @salvage=SUM(isnull(Claim_ri_Arrangement_Line.salvage,0)),@recover_todate=SUM(isnull(Claim_ri_Arrangement_Line.recovery,0))  
  FROM   Claim_ri_Arrangement_Line  
  JOIN Claim_ri_Arrangement  
  ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id  AND type ='T' AND ri_band_id=@ribandID AND ISNULL(Is_Obligatory,0) = 1 
  
  Update Claim_ri_Arrangement_Line set reserve = isnull(@reserve,0), payment=isnull(@payment,0),  
    salvage=isnull(@salvage,0),recovery=isnull(@recover_todate,0) WHERE ri_arrangement_id=@Clm_ri_arngmt_id AND type='T' 
	AND is_pt_archive =0  AND ISNULL(Is_Obligatory,0) = 1

  SELECT @reserve=SUM(ISNULL(Claim_ri_Arrangement_Line.reserve,0)),@payment=SUM(ISNULL(Claim_ri_Arrangement_Line.payment,0)),  
  @salvage=SUM(ISNULL(Claim_ri_Arrangement_Line.salvage,0)),@recover_todate=SUM(ISNULL(Claim_ri_Arrangement_Line.recovery,0))  
  FROM   Claim_ri_Arrangement_Line  
  JOIN Claim_ri_Arrangement  
  ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id AND type ='T' AND ri_band_id=@ribandID AND ISNULL(Is_Obligatory,0) = 0 
  
  Update Claim_ri_Arrangement_Line set reserve = ISNULL(@reserve,0), payment=isnull(@payment,0),  
    salvage=ISNULL(@salvage,0),recovery=ISNULL(@recover_todate,0) WHERE ri_arrangement_id=@Clm_ri_arngmt_id AND type='T' 
	AND is_pt_archive = 0 AND ISNULL(Is_Obligatory,0) = 0
  
	  SELECT @reserve=SUM(isnull(Claim_ri_Arrangement_Line.reserve,0)),@payment=SUM(isnull(Claim_ri_Arrangement_Line.payment,0)),  
	  @salvage=SUM(isnull(Claim_ri_Arrangement_Line.salvage,0)),@recover_todate=SUM(isnull(Claim_ri_Arrangement_Line.recovery,0))  
	  FROM   Claim_ri_Arrangement_Line  
	  JOIN Claim_ri_Arrangement  
	  ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
	  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id  AND type ='TFS' and ri_band_id=@ribandID  
  
	  Update Claim_ri_Arrangement_Line set reserve = isnull(@reserve,0), payment=isnull(@payment,0),  
		salvage=isnull(@salvage,0),recovery=isnull(@recover_todate,0) WHERE ri_arrangement_id=@Clm_ri_arngmt_id and type='TFS' and is_pt_archive =0 
    
	  Update Claim_RI_Arrangement_Line set claim_incurred_to_date =ISNULL (reserve,0)+ISNULL(salvage,0) +ISNULL(recovery,0)  where claim_id =@Claim_id and ri_arrangement_id=@Clm_ri_arngmt_id and type in ('TFS','T') and is_pt_archive =0 
    
	  END  
   
	ELSE  
	BEGIN  
           
	  SELECT @reserve=SUM(isnull(Claim_ri_Arrangement_Line.reserve,0)),@payment=SUM(isnull(Claim_ri_Arrangement_Line.payment,0)),  
	  @salvage=SUM(isnull(Claim_ri_Arrangement_Line.salvage,0)),@recover_todate=SUM(isnull(Claim_ri_Arrangement_Line.recovery,0))  
	  FROM   Claim_ri_Arrangement_Line  
	  WHERE  ri_arrangement_id=@Clm_ri_arngmt_id  and type in ('FX','TX','TC','R')  
	  Update Claim_ri_Arrangement set reserve = isnull(@reserve,0), payment=isnull(@payment,0),  
		salvage=isnull(@salvage,0),recovery=isnull(@recover_todate,0) WHERE ri_arrangement_id=@Clm_ri_arngmt_id  
  
	  END  
	  END  
	 ELSE  
	 BEGIN
	 IF @is_created =1
		EXEC spu_copy_claim_ri_line_for_pt @claim_id,@version_id ,@old_claim_id ,@ribandID ,@is_created
        
	   INSERT INTO Claim_ri_Arrangement_Line  
		 (Claim_Id,  
		  ri_Arrangement_Line_Id,  
		  ri_Arrangement_Id,  
		  TYPE,  
		  Treaty_Id,  
		  Party_cnt,  
		  xol_Arrangement_Id,  
		  Default_Share_Percent,  
	 This_Share_Percent,  
		  Agreement_Code,  
		  Priority,  
		  Number_Of_Lines,  
		  Line_Limit,  
		 Sum_Insured,  
		  Reserve,  
		  Payment,  
		  Salvage,  
		  Recovery,  
		  This_Reserve,  
		  This_Payment,  
		  This_Salvage,  
		  This_Recovery,  
		  Base_Claim_ri_Arrangement_Line_Id,  
		  Version_Id,  
		  Original_ri_Arrangement_Line_Id,  
							lower_limit,  
							Retained,  
	   participation_percent,  
							Grouping,is_obligatory , ri_model_line_id,
				  manually_added,
				  reserve_to_date ,
				  payment_to_date ,
				  salvage_to_date ,
				  recovery_to_date ,
				  claim_incurred_to_date ,
				  is_pt_archive   )  
	  SELECT @claim_id,  
		Claim_ri_Arrangement_Line.ri_Arrangement_Line_Id,  
		Copy_Claim_ri_Arrangement.ri_Arrangement_Id,  
		Claim_ri_Arrangement_Line.TYPE,  
		Claim_ri_Arrangement_Line.Treaty_Id,  
		Claim_ri_Arrangement_Line.Party_cnt,  
		Copy_Claim_xol_Arrangement.xol_Arrangement_Id,  
		Claim_ri_Arrangement_Line.Default_Share_Percent,  
		Claim_ri_Arrangement_Line.This_Share_Percent,  
		Claim_ri_Arrangement_Line.Agreement_Code,  
		Claim_ri_Arrangement_Line.Priority,  
		Claim_ri_Arrangement_Line.Number_Of_Lines,  
		Claim_ri_Arrangement_Line.Line_Limit *@model_currency_rate,  
		Claim_ri_Arrangement_Line.Sum_Insured,  
		Claim_ri_Arrangement_Line.Reserve,  
		Claim_ri_Arrangement_Line.Payment,  
		Claim_ri_Arrangement_Line.Salvage,  
		Claim_ri_Arrangement_Line.Recovery,  
		0,  
		0,  
		0,  
		0, -- Only zero the new 'this' amounts  
		Claim_ri_Arrangement_Line.Base_Claim_ri_Arrangement_Line_Id,  
		@version_id,  
		Original_ri_Arrangement_Line_Id,  
		Claim_ri_Arrangement_Line.lower_limit * @model_currency_rate,  
					Claim_ri_Arrangement_Line.Retained,  
					Claim_ri_Arrangement_Line.participation_percent,  
					Claim_ri_Arrangement_Line.Grouping,Claim_ri_Arrangement_Line.is_obligatory ,  
					Claim_ri_Arrangement_Line.ri_model_line_id,
					Claim_ri_Arrangement_Line.manually_added,
					Claim_ri_Arrangement_Line.reserve_to_date ,
					Claim_ri_Arrangement_Line.payment_to_date ,
					Claim_ri_Arrangement_Line.salvage_to_date ,
					Claim_ri_Arrangement_Line.recovery_to_date ,
					Claim_ri_Arrangement_Line.claim_incurred_to_date ,
					0    
	  FROM   Claim_ri_Arrangement_Line  
		LEFT JOIN Claim_ri_Arrangement  
		 ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id  
		LEFT JOIN (SELECT ri_Arrangement_Id,  
			 Version_Id,  
			 Base_Claim_ri_Arrangement_Id  
			FROM   Claim_ri_Arrangement  
			WHERE  Version_Id = @version_id  
			AND Claim_Id = @claim_id) Copy_Claim_ri_Arrangement  
		 ON Copy_Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id  
		LEFT JOIN Claim_xol_Arrangement  
		 ON Claim_ri_Arrangement_Line.xol_Arrangement_Id = Claim_xol_Arrangement.xol_Arrangement_Id  
		LEFT JOIN (SELECT xol_Arrangement_Id,  
			 Version_Id,  
			 Base_Claim_xol_Arrangement_Id  
			FROM   Claim_xol_Arrangement  
			WHERE  Version_Id = @version_id  
			AND Claim_Id = @claim_id) Copy_Claim_xol_Arrangement  
		 ON Copy_Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id = Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id  
	  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id  AND type not in ('F','FX','TX','TC','R') and ri_band_id=@ribandID 
		AND Claim_ri_Arrangement_Line.is_pt_archive =0 
	   IF @is_created =1 BEGIN 
		  SELECT @reserve=SUM(isnull(Claim_ri_Arrangement_Line.reserve,0)),@payment=SUM(isnull(Claim_ri_Arrangement_Line.payment,0)),  
	  @salvage=SUM(isnull(Claim_ri_Arrangement_Line.salvage,0)),@recover_todate=SUM(isnull(Claim_ri_Arrangement_Line.recovery,0))  
	  FROM   Claim_ri_Arrangement_Line  
	  WHERE  ri_arrangement_id=@Clm_ri_arngmt_id  and type in ('FX','TX','TC','R')  
	  Update Claim_ri_Arrangement set reserve = isnull(@reserve,0), payment=isnull(@payment,0),  
		salvage=isnull(@salvage,0),recovery=isnull(@recover_todate,0) WHERE ri_arrangement_id=@Clm_ri_arngmt_id  
		END
	 END  
  
	  FETCH NEXT FROM Upd_clm_ri_Lines  
		INTO @Clm_ri_arngmt_id,  
		@rsk_cnt,  
		@ribandID,  
		@rimodelid  
  
	  END  
	  CLOSE Upd_clm_ri_Lines  
	  DEALLOCATE Upd_clm_ri_Lines  
  
	  END  
  
		UPDATE Claim_ri_Arrangement_Line  
		SET    ri_Arrangement_Line_Id = Claim_ri_Arrangement_Line_Id  
		WHERE  Claim_Id = @claim_id  
  
		Select @claim_ri_arrangement_id = ri_arrangement_id From Claim_ri_Arrangement_Line where Claim_id=@claim_id  
	 END -- @is_create = 1  
  
	  --Add broker participants if any  
	 EXEC Spu_CLM_Copy_Broker_Details_to_Claim @claim_ri_arrangement_id  
 
	 -- Declare the cursor  
	 IF @is_deferred = 0  
	 BEGIN  
	   if(@Recovery!=2)  
	  BEGIN  
		-- Just get reserve and payment for this session  
			DECLARE Reserve_CurSor CURSOR FAST_FORWARD FOR  
  
	--Arul Stephen  
	 SELECT   p.ri_Band,  
				   0,  
				 ISNULL(SUM(R.this_receipt_net) * (-1),0)  
			FROM      Claim_Peril p  
	  INNER JOIN Claim_Receipt CR  
	   ON CR.Claim_peril_id = p.Claim_Peril_Id  
	  INNER JOIN Claim_Receipt_Item CRI  
	   ON CRI.Claim_receipt_id = CR.Claim_receipt_id  
	  INNER JOIN Recovery R  
	   ON R.Recovery_id = CRI.Recovery_Id  
	  INNER JOIN Recovery_Type RT  
	   ON RT.Recovery_Type_id = R.Recovery_Type_Id  
			WHERE    p.Claim_Id = @Claim_Id and cri.claim_receipt_id in(select max(claim_receipt_id) from claim_receipt where Claim_Id = @Claim_Id)  
	 AND (  
	  (@Recovery = 0 AND RT.is_salvage = 0)  
	  OR  
	  (@Recovery = 1 AND RT.is_salvage = 1 )  
		 )  
			GROUP BY p.ri_Band  
			ORDER BY p.ri_Band  
  
	--End Arul Stephen  
			  END  
		  Else  
	 Begin  
  
	 -- Just get reserve and payment for this session  
	  DECLARE Reserve_CurSor CURSOR FAST_FORWARD FOR  
	  SELECT   p.ri_Band,  
		 ISNULL(SUM(r.This_Revision),0),  
		 ISNULL(SUM(r.This_Payment),0)  
	  FROM     Claim_Peril p  
		 LEFT JOIN Reserve r  
		 ON r.Claim_Peril_Id = p.Claim_Peril_Id  
		 LEFT JOIN claim_ri_arrangement a  
		 ON a.ri_band_id = p.ri_band  
		 AND a.claim_id = p.claim_id  
	  WHERE    p.Claim_Id = @claim_id  
	  GROUP BY p.ri_Band  
	  ORDER BY p.ri_Band  
	 END  
	End  
	--Start Arul Stephen  
		Else  
		 Begin  
		  if(@Recovery!=2)  
			BEGIN  
			 -- Get totals for reserve and payments, this will allow deferred ri  
			 -- to catch up once we have a proper ri model  
			 -- GAURAV -  Join with Recover to get the this_salvage and this_recovery amount  
			 DECLARE Reserve_CurSor CURSOR  FAST_FORWARD FOR  
			 SELECT   p.ri_Band,  
			 SUM(r.Initial_Reserve + r.Revised_Reserve),  
			 SUM(r.Received_To_Date)  
  
		  --Arul Stephen  
			 FROM     Claim_Peril p  
			 --Arul Stephen  
			 --      JOIN Reserve r  
			 --      ON r.Claim_Peril_Id = p.Claim_Peril_Id  
			 Join Recovery r ON r.Claim_Peril_Id=p.Claim_Peril_Id  
			 --End Arul Stephen  
			 WHERE    p.Claim_Id = @claim_id  
			 GROUP BY p.ri_Band  
			 ORDER BY p.ri_Band  
		   END  
  
	Else  
  
	 BEGIN  
	 -- Get totals for reserve and payments, this will allow deferred ri  
	  -- to catch up once we have a proper ri model  
	  DECLARE Reserve_CurSor CURSOR  FAST_FORWARD FOR  
	  SELECT   p.ri_Band,  
		 ISNULL(SUM(r.Initial_Reserve + r.Revised_Reserve),0),  
		 ISNULL(SUM(r.Paid_To_Date),0)  
	  FROM     Claim_Peril p  
		 LEFT JOIN Reserve r  
		 ON r.Claim_Peril_Id = p.Claim_Peril_Id  
		 LEFT JOIN claim_ri_arrangement a  
		 ON a.ri_band_id = p.ri_band  
		 AND a.claim_id = p.claim_id  
	  WHERE  p.Claim_Id = @claim_id  
	  GROUP BY p.ri_Band  
	  ORDER BY p.ri_Band  
	 END  
	End  
	 -- Open the cursor and get first row  
	 OPEN Reserve_CurSor  
  
	 FETCH NEXT FROM Reserve_CurSor  
	 INTO @ri_band,  
	   @this_reserve,  
		@this_payment  
  
	 WHILE @@FETCH_STATUS = 0  
	 BEGIN  
	  IF IsNull(@is_balance_and_close,0) = 1  
	  BEGIN  
	  -- Get arrangement  
	   SELECT @ri_arrangement_id = ri_Arrangement_Id  
	   FROM   Claim_ri_Arrangement  
	   WHERE  Claim_Id = @claim_id  
	   AND ri_Band_Id = @ri_band  
  
	   -- We are balancing and closing this claim, just balance the RI don't calculate  
	   EXECUTE spu_Calculate_Claims_ri_Balance_Reserves  
		@claim_id ,  
		@ri_arrangement_id  
	  END  
	  ELSE  
	  BEGIN  
	  -- Reduce reserve and payment amounts by the ri share  
	   SELECT @this_reserve = @this_reserve * @retained_share,  
  
		 @this_payment = @this_payment * @retained_share  
  
	   -- Update reserve and payment for this band and get id's and calculation routine to use.  

	  if(@Recovery=2)  
		 Begin  
  
		UPDATE Claim_ri_Arrangement  
	   SET    This_Reserve = @this_reserve,  
		  This_Payment = @this_payment,  
		  @ri_arrangement_id = ri_Arrangement_Id,  
		  @claim_allocation_type = Claim_Allocation_Type,  
		  @is_modified = Isnull(Is_modIfied,0),  
		  @total_reserve = Reserve + @this_reserve,  
		  --@total_payment = Payment + @this_Payment  
		  @total_payment = Payment + (salvage + Recovery) + @this_Payment
		WHERE  Claim_Id = @claim_id  
		AND ri_Band_Id = @ri_band  
  
		  End  
	 Else if(@Recovery=1 )  
		 Begin  
		UPDATE Claim_ri_Arrangement  
	   SET    This_Reserve = @this_reserve,  
		  --This_Payment = @this_payment,  
		  @ri_arrangement_id = ri_Arrangement_Id,  
		  @claim_allocation_type = Claim_Allocation_Type,  
		  @is_modified = Isnull(Is_modIfied,0),
		 @total_reserve = Reserve + @this_reserve,  
  
		   @total_payment = Payment + (salvage + Recovery) + @this_Payment,  
		  This_salvage=@this_payment  
  
		WHERE  Claim_Id = @claim_id  
		AND ri_Band_Id = @ri_band  
  
		  End  
  
	Else  
		Begin  
  
	  UPDATE Claim_ri_Arrangement  
	  SET    This_Reserve = @this_reserve,  
	  --This_Payment = @this_payment,  
	  @ri_arrangement_id = ri_Arrangement_Id,  
	  @claim_allocation_type = Claim_Allocation_Type,  
	  @is_modified = Isnull(Is_modIfied,0),  
	  @total_reserve = Reserve + @this_reserve,  
  
	  @total_Payment=Payment +(salvage+Recovery)+@this_Payment,  
	  This_recovery=@this_payment  
  
	 WHERE  Claim_Id = @claim_id  
	 AND ri_Band_Id = @ri_band  
	   End  
  
	  If @is_created =0
	  BEGIN
	  IF @Recovery =2
		 UPDATE Claim_ri_Arrangement  
	   SET  incurred_to_date =ISNULL(incurred_to_date,0) + @this_reserve
		WHERE  Claim_Id = @claim_id  
		AND ri_Band_Id = @ri_band  
	  ELSE
	  UPDATE Claim_ri_Arrangement  
	   SET  incurred_to_date =ISNULL(incurred_to_date,0) + @this_reserve+ @this_Payment   
		WHERE  Claim_Id = @claim_id  
		AND ri_Band_Id = @ri_band  
	  END
	   -- Check if the ri model has xol...the rules change then  
	   IF EXISTS (SELECT *  
		  FROM   Claim_ri_Arrangement_Line  
		  WHERE  Claim_Id = @claim_id  
		   AND ri_Arrangement_Id = @ri_arrangement_id  
		   AND TYPE IN ('X', 'FX', 'TX'))  
		SELECT @model_has_xol = 1  
	   ELSE  
		SELECT @model_has_xol = 1  
	  FROM   ri_Model rm  
		JOIN Claim_ri_Arrangement ra  
		  ON ra.ri_Model_Id = rm.ri_Model_Id  
	   WHERE  ra.Claim_Id = @claim_id  
	 AND ra.ri_Arrangement_Id = @ri_arrangement_id  
	   AND (xol_clm_ri_Model_Id IS NOT NULL  
		 OR xol_Cat_ri_Model_Id IS NOT NULL)  
  
	  --Start-(Arul)-(PN Fixing-PN 58323)  
	  IF @product_option = 1  
	  SELECT @model_has_xol=1  
	  --End-(Arul)-(PN Fixing-PN 58323)  
  
	   -- We process the claim in a new way if XOL is present  
	   IF (@model_has_xol = 1)  
	   BEGIN  
	   -- If model has been modified we don't perform automatic allocation  
		-- Note: Even if we don't have XOL yet!!!  
				  IF @product_option = 1  
				  BEGIN  
					   SET @claim_allocation_type = 2  
				  END  
		IF @is_modified = 0  
		BEGIN  
		-- First run appropriate calculation routine for base ri  
		 IF (IsNull(@claim_allocation_type,0) = 0)          -- Standard proportional  
		 EXECUTE spu_Calculate_Claims_ri_Method_0_Full  
	   @claim_id ,  
		  @ri_arrangement_id ,  
		  @total_reserve ,  
		  @total_payment  
  
		IF (@claim_allocation_type = 1)                     -- Proportional by priority (NOT SUPPORTED AT THIS TIME)  
			EXECUTE spu_Calculate_Claims_ri_Method_1_Full  
			@claim_id ,  
			@ri_arrangement_id ,  
			@total_reserve ,  
			@total_payment  
  
		IF (@claim_allocation_type = 2)                     -- Non proportional  
		BEGIN
			IF @product_option = 0 
				EXECUTE spu_Calculate_Claims_ri_Method_2_Full  
				@claim_id ,  
				@ri_arrangement_id ,  
				@total_reserve ,  
				@total_payment,  
				@Reapply_TX,  
				@Recovery,  
				@Reapply_Treaty, @is_created  
			ELSE
				EXECUTE spu_Calculate_Claims_ri_Method_2_Full_RI2007
				@claim_id ,  
				@ri_arrangement_id ,  
				@total_reserve ,  
				@total_payment,  
				@Reapply_TX,  
				@Recovery,  
				@Reapply_Treaty  
		END

		 -- Next find out how much was allocated to retained  
		 SELECT @retained_reserve = SUM(This_Reserve),  
		   @retained_payment = SUM(This_Payment)  
		 FROM   Claim_ri_Arrangement_Line ral  
		 WHERE  Claim_Id = @claim_id  
		 AND ri_Arrangement_Id = @ri_arrangement_id  
		 AND TYPE = 'R'  
  
		 -- Store original values, we'll need them  
		 SELECT @original_reserve = @retained_reserve,  
		   @original_payment = @retained_payment  
  
		 -- Check for and allocate any per claim xol  
		 -- Note: Output values return the reduced reserve after xol allocation  
		 EXECUTE spu_Calculate_Claims_ri_xol_Claim  
		  @claim_id ,  
		  @ri_arrangement_id ,  
		  @total_reserve ,  
		  @retained_reserve OUTPUT ,  
		  @retained_payment OUTPUT  
  
		 -- Check for and allocate any catastrophe xol  
		 -- Notes:  
		 --    Cat xol is based on raw retention so pass in original values  
		 --    Output values return the reduced reserve after xol allocation  
		 IF IsNull(@catastrophe_code_id,0) <> 0  
		 BEGIN  
		 EXECUTE spu_Calculate_Claims_ri_xol_Cat  
		   @claim_id ,  
		   @ri_arrangement_id ,    
		   @total_reserve ,  
		   @catastrophe_code_id ,  
		   @old_claim_id ,  
		   @original_reserve ,  
		   @original_payment ,  
		   @retained_reserve OUTPUT ,  
		   @retained_payment OUTPUT  
		 END  
  
		 -- Check if the our retention amounts have changed due to XOL  
		 IF (@original_reserve <> @retained_reserve)  
		  OR (@original_payment <> @retained_payment)  
		 BEGIN  
		 -- They have so update our retention values  
		  IF (IsNull(@claim_allocation_type,0) = 0)          -- Standard proportional  
		  EXECUTE spu_Calculate_Claims_ri_xol_Return_0  
		   @claim_id ,  
		   @ri_arrangement_id ,  
		   @total_reserve ,  
		   @original_reserve ,  
		   @original_payment ,  
		   @retained_reserve ,  
		   @retained_payment  
  
		  IF (@claim_allocation_type = 1)                     -- Proportional by priority (NOT SUPPORTED AT THIS TIME)  
		  EXECUTE spu_Calculate_Claims_ri_xol_Return_1  
		   @claim_id ,  
		   @ri_arrangement_id ,  
		   @total_reserve ,  
		   @original_reserve ,  
		   @original_payment ,  
		   @retained_reserve ,  
		   @retained_payment  
  
		  IF (@claim_allocation_type = 2)                     -- Non proportional  
		  EXECUTE spu_Calculate_Claims_ri_xol_Return_2  
		   @claim_id ,  
		   @ri_arrangement_id ,  
		   @total_reserve ,  
		   @original_reserve ,  
		 @original_payment ,  
		   @retained_reserve ,  
		   @retained_payment  
		 END  
  
		 IF @product_option <>1  
		 -- Now we have recreated the allocation rebalance the this_xxx columns  
		 EXECUTE spu_Calculate_Claims_ri_Balance_Full  
		  @claim_id ,  
		  @ri_arrangement_id  
		END  
	   END  
	   ELSE  
	   BEGIN  
	   -- No XOL fallback to original routines  
		-- Run appropriate calculation routine  
		IF (IsNull(@claim_allocation_type,0) = 0)          -- Standard proportional  
		EXECUTE spu_Calculate_Claims_ri_Method_0  
		 @claim_id ,  
		 @ri_arrangement_id ,  
		 @this_reserve ,  
		 @this_payment,  
		 @Recovery  
  
		IF (@claim_allocation_type = 1)                     -- Proportional by priority (NOT SUPPORTED AT THIS TIME)  
		EXECUTE spu_Calculate_Claims_ri_Method_1  
		 @claim_id ,  
		 @ri_arrangement_id ,  
		 @this_reserve ,  
		 @this_payment  
  
		IF (@claim_allocation_type = 2)                     -- Non proportional  
		EXECUTE spu_Calculate_Claims_ri_Method_2  
		 @claim_id ,  
		 @ri_arrangement_id ,  
		 @this_reserve ,  
		 @this_payment  
	   END  
	  END  
  
	  -- Get next record  
	  FETCH NEXT FROM Reserve_CurSor  
	  INTO @ri_band,  
		@this_reserve,  
  
		@this_payment  
  
	 END  
  
	 -- Shutdown the cursor  
	 CLOSE Reserve_CurSor  
  
	 DEALLOCATE Reserve_CurSor  

	 END