SET Quoted_IdentIfier  Off
GO
SET Ansi_Nulls  ON
GO

EXECUTE ddlDropProcedure 'spu_copy_reinsurance_details_to_claim'
GO

CREATE PROCEDURE spu_Copy_Reinsurance_Details_To_Claim
@Claim_id             INT,
  @is_balance_and_close TINYINT  = NULL,
  @bOpenClaimNoTrans TINYINT  = NULL,
  @is_created   TINYINT  = 0
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
	@claim_has_xol         TINYINT,
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
  @ri_model_type	  INT,
  @cover_start_date_ForRi datetime  

 DECLARE  @claim_ri_arrangement_id INT

   SELECT @product_option='0'
   SELECT @product_option=ISNULL(value,0) FROM Hidden_Options WITH (NOLOCK) WHERE option_number=88
   Select @Effective_date=loss_from_date from claim where claim_id=@claim_id
   Set @Reapply_TX=0

   SELECT @insurance_file_cnt = Policy_Id
	FROM   Claim WITH (NOLOCK)
	WHERE  Claim_Id = @Claim_id
    
	SELECT         
      @cover_start_date_ForRi = inception_date_tpi  
    FROM insurance_file  (NOLOCK)        
    WHERE Insurance_file_cnt = @Insurance_file_cnt
	
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
  SELECT @old_claim_id = Claim_Id
  FROM   Claim WITH (NOLOCK)
  WHERE  Version_Id = @prev_version_id
  AND Base_Claim_Id = @base_id
 END
 ELSE
 -- if the version is 1 this is is open claim (C_CO)
 SELECT @old_claim_id = NULL

 SELECT @insurance_file_cnt = Policy_Id,
   @risk_cnt = Risk_Type_Id,
   @catastrophe_code_id = Catastrophe_Code_Id
 FROM   Claim WITH (NOLOCK)
 WHERE  Claim_Id = @Claim_id
 -- Get the percentage not covered by coinsurance
 SELECT @retained_share = (100 - Isnull(SUM(Share),0)) / 100
 FROM   Claim_Party WITH (NOLOCK)
 WHERE  Claim_Id = @claim_id
 AND Insurer_Type = 0

 -- We're here because either we've just entered the reinsurance screen, in which this table has no record,
 -- or we've clicked Apply, in which case we definitely don't want to lose our changes.
 IF EXISTS (SELECT *
	FROM   Claim_ri_Arrangement WITH (NOLOCK)
	WHERE  Claim_Id = @Claim_id)
 BEGIN
 -- Peter Finney 04/07/2003
  -- That was a bit naff and wouldn't allow us to recalculate if the reserve had changed.
  -- Now check if the any of the band's reserves have changed as well...
  IF NOT EXISTS (SELECT   p.ri_Band
					FROM     Claim_Peril p WITH (NOLOCK)
							JOIN Reserve r WITH (NOLOCK)
							ON p.Claim_Peril_Id = r.Claim_Peril_Id
							WHERE    p.Claim_Id = @Claim_id
							GROUP BY p.ri_Band
							HAVING   SUM(Isnull(r.Initial_Reserve + r.Revised_Reserve,0)) <> (SELECT ra.Reserve + ra.This_Reserve
							FROM   Claim_ri_Arrangement ra WITH (NOLOCK)
							WHERE  ra.Claim_Id = @Claim_id
							AND ra.ri_Band_Id = p.ri_Band)
							OR SUM(Isnull(r.Paid_To_Date,0)) <> (SELECT ra.Payment + ra.This_Payment
							FROM   Claim_ri_Arrangement ra WITH (NOLOCK)
							WHERE  ra.Claim_Id = @Claim_id
							AND ra.ri_Band_Id = p.ri_Band))
		RETURN
END

 -- Check if we should be creating the ri for the first time
 IF @old_claim_id IS NULL OR @is_created = 1
 SELECT @is_create = 1
 ELSE
 IF NOT EXISTS (SELECT *
	 FROM   Claim_ri_Arrangement WITH (NOLOCK)
	 WHERE  Claim_Id = @Claim_id)
 SELECT @is_create = 1,
   -- set deferred reinsurance flag to indicate that there could be existing reserves / payments that require
   -- allocation - not just those added in this session
   @is_deferred = 1

IF (ISNULL(@bOpenClaimNoTrans, 0) = 1)
   SELECT @is_deferred = 1

 -- Clear down existing data
 DELETE Claim_ri_Arrangement_Line WITH (ROWLOCK)
 WHERE  Claim_Id = @Claim_id

 DELETE Claim_xol_Arrangement WITH (ROWLOCK)
 WHERE  Claim_Id = @Claim_id

 DELETE Claim_ri_Arrangement WITH (ROWLOCK)
 WHERE  Claim_Id = @Claim_id

 -- Should we create or copy?
 IF @is_create = 1
 BEGIN
 -- If original claim is null we're in open claim and so we copy the existing RI structure
  -- If the original claim is not null but there is no ri structure then this could be
  -- defered reinsurance; attempt to add the ri details; worst case is nothing will be done
  -- NotE:
  --    If we were implementing a different RI model for claims this is where you should
  --    insert the code.

  Select @ribandid=ri_band_id from ri_arrangement WITH (NOLOCK) where risk_cnt=@risk_cnt

  Select TOP 1  @XOL_Treaty_to_Recover_From=XOL_Treaty_to_Recover_From_id   
  from RI_Band_Version WITH (NOLOCK) where ri_band_id=@ribandid    
  AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)      
   ORDER BY effective_date DESC 

  Select @risk_type_id=risk.risk_type_id from risk WITH (NOLOCK),claim WITH (NOLOCK) where risk_cnt=claim.risk_type_id and claim_id=@claim_id

  if @product_option=1 and @XOL_Treaty_to_Recover_From = 2
  BEGIN

	  Select  @ri_model_id = rmu.ri_model_id,
			  @model_currency_id = currency_id,
			  @ri_model_type = rm.ri_model_type
	  From    risk_type_ri_model_usage rmu WITH (NOLOCK)
	  Join    ri_model rm WITH (NOLOCK)
			  On rm.ri_model_id = rmu.ri_model_id
	  Where   rmu.risk_type_id = @risk_type_id
	  And     rmu.ri_band = @ribandid
	  And     rmu.is_deleted = 0
	  And     rmu.effective_date <= @effective_date
	  And    (rmu.expiry_date >= @effective_date or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')
	  And     rm.ri_model_type = 0
	  And     rm.is_deleted = 0
	  And     rm.effective_date <= @effective_date
	  And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')
	  Order By
			  rm.ri_model_type Desc,    -- give priority to none-deferred models
			  rmu.effective_date Asc   -- give priority to newer models
  END

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
	Ri_arrangement_version)
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
  1
  FROM   ri_Arrangement ra WITH (NOLOCK)
	LEFT JOIN ri_Model rm WITH (NOLOCK)
	 ON rm.ri_Model_Id = ra.ri_Model_Id
  WHERE  ra.Risk_cnt = @risk_cnt
  AND ra.Original_Flag = 0

  SET @claim_ri_arrangement_id = @@IDENTITY

  -- set the base_claim_ri_arrangement_id to indicate this is the initial version
  UPDATE Claim_ri_Arrangement WITH (ROWLOCK)
  SET    Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement_Id,
	ri_Arrangement_Id = Claim_ri_Arrangement_Id
  WHERE  Claim_Id = @claim_id

  DECLARE Upd_clm_ri_Lines CURSOR  FOR
  SELECT Claim_ri_Arrangement_Id,
	Risk_cnt,
	ri_Band_Id,
	ri_model_id
  FROM   Claim_ri_Arrangement WITH (NOLOCK)
  WHERE  Claim_Id = @CLAIM_ID

  OPEN Upd_clm_ri_Lines

  FETCH NEXT FROM Upd_clm_ri_Lines
  INTO @Clm_ri_arngmt_id,
	@rsk_cnt,
	@ribandID,
  @rimodelid

  WHILE @@FETCH_STATUS = 0
  BEGIN
  -- Insert the lines
  IF @product_option=1 and @XOL_Treaty_to_Recover_From = 2 and @rimodelid <> @ri_model_id
	  BEGIN
		 SET @Reapply_TX = 1
		exec Spu_copy_Reinsurance_TreatyXOl_Details @claim_id=@claim_id ,
							  @risk_cnt= @risk_cnt,
							  @ri_band_id=@ribandID ,
							  @ri_model_id=@ri_model_id,
							  @claim_ri_arrangement_id=@Clm_ri_arngmt_id,
							  @version_id=@version_id
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
			 --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
			 [Grouping],
			 is_obligatory)
			 --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
		 SELECT @claim_id,
		   ral.ri_Arrangement_Line_Id,
		   @Clm_ri_arngmt_id,
		   ral.TYPE,
		   ral.Treaty_Id,
		   ral.Party_cnt,
		   NULL,
		   CASE
			WHEN ((ral.This_Share_Percent = NULL
				OR ral.This_Share_Percent = 0)
			   AND ral.Premium_Value <> 0) THEN ral.Default_Share_Percent
			ELSE ral.This_Share_Percent
		   END, -- Default percent is the share From NB
		   CASE
			   WHEN ral.type='F' then
					ral.This_Share_Percent
				WHEN ((ral.This_Share_Percent = NULL
					 )
					 AND ral.Premium_Value = 0) THEN ral.Default_Share_Percent
			   ELSE
				   ral.this_share_percent
		   END,
		   ral.Agreement_Code,
		   ral.Priority,
		   ral.Number_Of_Lines,
		   ral.Line_Limit,
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
		   ral.Lower_limit,
		   ral.retained,
		   ral.participation_percent,
		   --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
		   ral.Grouping,
		   ral.is_Obligatory
		   --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
		 FROM   ri_Arrangement_Line ral WITH (NOLOCK)
		   JOIN ri_Arrangement ra WITH (NOLOCK)
			ON ra.ri_Arrangement_Id = ral.ri_Arrangement_Id
		 WHERE  ra.Risk_cnt = @rsk_cnt
		 AND ra.ri_Band_Id = @ribandID
		 AND ra.Original_Flag = 0
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
  UPDATE Claim_ri_Arrangement_Line WITH (ROWLOCK)
  SET    Base_Claim_ri_Arrangement_Line_Id = Claim_ri_Arrangement_Line_Id,
	ri_Arrangement_Line_Id = Claim_ri_Arrangement_Line_Id
  WHERE  Claim_Id = @claim_id

 END
 ELSE
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
	Ri_Arrangement_version)
  SELECT @claim_id,
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
	0,
	0,
	0,
	0, -- Only zero the new 'this' amounts
	0,
	Base_Claim_ri_Arrangement_Id,
	@version_id,
	Original_ri_Arrangement_Id,
  ri_arrangement_version
  FROM   Claim_ri_Arrangement WITH (NOLOCK)
  WHERE  Claim_Id = @old_claim_id

  UPDATE Claim_ri_Arrangement WITH (ROWLOCK)
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
  FROM   Claim_xol_Arrangement WITH (NOLOCK)
	LEFT JOIN Claim_ri_Arrangement WITH (NOLOCK)
	 ON Claim_xol_Arrangement.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id
	LEFT JOIN (SELECT ri_Arrangement_Id,
		 Version_Id,
		 Base_Claim_ri_Arrangement_Id
		FROM   Claim_ri_Arrangement WITH (NOLOCK)
		WHERE  Version_Id = @version_id
		AND Claim_Id = @claim_id) Copy_Claim_ri_Arrangement
	 ON Copy_Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id
  WHERE  Claim_xol_Arrangement.Claim_Id = @old_claim_id

  UPDATE Claim_xol_Arrangement WITH (ROWLOCK)
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
	 --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
	 [Grouping],
	 is_obligatory)
	 --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
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
	Claim_ri_Arrangement_Line.Line_Limit,
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
	Claim_ri_Arrangement_Line.lower_limit,
				Claim_ri_Arrangement_Line.Retained,
				Claim_ri_Arrangement_Line.participation_percent,
			 --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
			   Claim_ri_Arrangement_Line.Grouping,
			   Claim_ri_Arrangement_Line.Is_Obligatory
			 --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)
  FROM   Claim_ri_Arrangement_Line WITH (NOLOCK)
	LEFT JOIN Claim_ri_Arrangement WITH (NOLOCK)
	 ON Claim_ri_Arrangement_Line.ri_Arrangement_Id = Claim_ri_Arrangement.ri_Arrangement_Id
	AND Claim_ri_Arrangement_Line.claim_Id = Claim_ri_Arrangement.claim_Id
	LEFT JOIN (SELECT ri_Arrangement_Id,
		 Version_Id,
		 Base_Claim_ri_Arrangement_Id
		FROM   Claim_ri_Arrangement WITH (NOLOCK)
		WHERE  Version_Id = @version_id
		AND Claim_Id = @claim_id) Copy_Claim_ri_Arrangement
	 ON Copy_Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id = Claim_ri_Arrangement.Base_Claim_ri_Arrangement_Id
	LEFT JOIN Claim_xol_Arrangement WITH (NOLOCK)
	 ON Claim_ri_Arrangement_Line.xol_Arrangement_Id = Claim_xol_Arrangement.xol_Arrangement_Id
	LEFT JOIN (SELECT xol_Arrangement_Id,
		 Version_Id,
		 Base_Claim_xol_Arrangement_Id
		FROM   Claim_xol_Arrangement WITH (NOLOCK)
		WHERE  Version_Id = @version_id
		AND Claim_Id = @claim_id) Copy_Claim_xol_Arrangement
	 ON Copy_Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id = Claim_xol_Arrangement.Base_Claim_xol_Arrangement_Id
  WHERE  Claim_ri_Arrangement_Line.Claim_Id = @old_claim_id

  UPDATE Claim_ri_Arrangement_Line WITH (ROWLOCK)
  SET    ri_Arrangement_Line_Id = Claim_ri_Arrangement_Line_Id
  WHERE  Claim_Id = @claim_id

  Select @claim_ri_arrangement_id = ri_arrangement_id From Claim_ri_Arrangement_Line where Claim_id=@claim_id
 END -- @is_create = 1

 --Add broker participants if any
 exec Spu_CLM_Copy_Broker_Details_to_Claim @claim_ri_arrangement_id

 -- Declare the cursor
 IF @is_deferred = 0
 BEGIN
 -- Just get reserve and payment for this session
  DECLARE Reserve_CurSor CURSOR  FOR
  SELECT   p.ri_Band,
	 SUM(r.This_Revision),
	 SUM(r.This_Payment)
  FROM     Claim_Peril p WITH (NOLOCK)
	 JOIN Reserve r WITH (NOLOCK)
	 ON r.Claim_Peril_Id = p.Claim_Peril_Id
  WHERE    p.Claim_Id = @claim_id
  GROUP BY p.ri_Band
  ORDER BY p.ri_Band
 END
 ELSE
 BEGIN
 -- Get totals for reserve and payments, this will allow deferred ri
  -- to catch up once we have a proper ri model
  DECLARE Reserve_CurSor CURSOR  FOR
  SELECT   p.ri_Band,
	 SUM(r.Initial_Reserve + r.Revised_Reserve),
	 SUM(r.Paid_To_Date)
  FROM     Claim_Peril p WITH (NOLOCK)
	 JOIN Reserve r WITH (NOLOCK)
	 ON r.Claim_Peril_Id = p.Claim_Peril_Id
  WHERE    p.Claim_Id = @claim_id
  GROUP BY p.ri_Band
  ORDER BY p.ri_Band
 END

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
   FROM   Claim_ri_Arrangement WITH (NOLOCK)
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

	-- E007
	SELECT @ri_model_type = ri_model_type FROM RI_Model where ri_model_id = @ri_model_id
	IF @ri_model_type =4
	BEGIN
		UPDATE  Claim_ri_arrangement
		SET     Cloned = 1
		WHERE   claim_ri_arrangement_id = @claim_ri_arrangement_id
	END
	ELSE
	BEGIN
		UPDATE  Claim_ri_arrangement
		SET     Cloned = 0
		WHERE   claim_ri_arrangement_id = @claim_ri_arrangement_id
	END
   -- Update reserve and payment for this band and get id's and calculation routine to use.
   UPDATE Claim_ri_Arrangement WITH (ROWLOCK)
   SET    This_Reserve = @this_reserve,
	 This_Payment = @this_payment,
	 @ri_arrangement_id = ri_Arrangement_Id,
	 @claim_allocation_type = Claim_Allocation_Type,
	 @is_modified = Isnull(Is_modIfied,0),
	 @total_reserve = Reserve + @this_reserve,
	 @total_payment = Payment + @this_payment
   WHERE  Claim_Id = @claim_id
   AND ri_Band_Id = @ri_band

   -- Check if the ri model has xol...the rules change then
   IF EXISTS (SELECT *
	  FROM   Claim_ri_Arrangement_Line WITH (NOLOCK)
	  WHERE  Claim_Id = @claim_id
	   AND ri_Arrangement_Id = @ri_arrangement_id
	   AND TYPE IN ('X', 'FX', 'TX'))
	SELECT @model_has_xol = 1,@claim_has_xol=1
   ELSE
	SELECT @model_has_xol = 1
   FROM   ri_Model rm WITH (NOLOCK)
	 JOIN Claim_ri_Arrangement ra WITH (NOLOCK)
	  ON ra.ri_Model_Id = rm.ri_Model_Id
   WHERE  ra.Claim_Id = @claim_id
   AND ra.ri_Arrangement_Id = @ri_arrangement_id
   AND (xol_clm_ri_Model_Id IS NOT NULL
	 OR xol_Cat_ri_Model_Id IS NOT NULL)

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

	 IF (@claim_allocation_type = 2) 	                     -- Non proportional
	 BEGIN
	 IF @product_option = 0
		 EXECUTE spu_Calculate_Claims_ri_Method_2_Full
		  @claim_id ,
		  @ri_arrangement_id ,
		  @total_reserve ,
		  @total_payment,
		  @Reapply_TX
	  ELSE
		 EXECUTE spu_Calculate_Claims_ri_Method_2_Full_RI2007
		  @claim_id ,
		  @ri_arrangement_id ,
		  @total_reserve ,
		  @total_payment,
		  @Reapply_TX
	  END
	 -- Next find out how much was allocated to retained
	 IF (IsNull(@claim_allocation_type,0) = 0)
	 Begin
	   SELECT @retained_reserve = SUM(This_Reserve),
	   @retained_payment = SUM(This_Payment)
	   FROM   Claim_ri_Arrangement_Line ral
	   WHERE  Claim_Id = @claim_id
	   AND ri_Arrangement_Id = @ri_arrangement_id
	  AND TYPE = 'R'
	 ENd
	 else
	 BEGIN
	   SELECT @retained_reserve = SUM(This_Reserve)+SUM(Reserve),
	   @retained_payment = SUM(This_Payment)+Sum(payment)
	   FROM   Claim_ri_Arrangement_Line ral WITH (NOLOCK)
	   WHERE  Claim_Id = @claim_id
	   AND ri_Arrangement_Id = @ri_arrangement_id
	 AND TYPE = 'R'
	 END

	 -- Store original values, we'll need them
	 SELECT @original_reserve = @retained_reserve,
	   @original_payment = @retained_payment
	
	DECLARE @xol_limit money, @cat_limit money
	Select  @xol_limit = ISNULL(rim.xol_clm_limit,0),@cat_limit=ISNULL(rim.xol_cat_limit,0)
		From    claim_ri_arrangement ra
		Left Join
				ri_model rim On rim.ri_model_id = ra.ri_model_id
		Where   claim_id = @claim_id
		And     ri_arrangement_id = @ri_arrangement_id


	IF @xol_limit<@cat_limit 
	BEGIN
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

	END
	ELSE
	BEGIN
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
		 -- Check for and allocate any per claim xol
		 -- Note: Output values return the reduced reserve after xol allocation
		 EXECUTE spu_Calculate_Claims_ri_xol_Claim
		  @claim_id ,
		  @ri_arrangement_id ,
		  @total_reserve ,
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

	 IF @product_option <>1 AND (ISNULL(@claim_has_xol,0)=1 OR @claim_allocation_type <> 2)
	 -- Now we have recreated the allocation rebalance the this_xxx columns
	 EXECUTE spu_Calculate_Claims_ri_Balance_Full_original
	  @claim_id ,
	  @ri_arrangement_id,
	  @claim_allocation_type
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
	 @this_payment

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
