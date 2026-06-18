SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure spu_sir_agent_commission_calc
GO

CREATE PROCEDURE spu_sir_agent_commission_calc  
    @insurance_file_cnt int,  
    @transaction_type varchar(10)  
AS  
  
BEGIN  
   DECLARE  @gis_object_commission_rate NUMERIC(19, 10)
	Declare @Count int,
		@party_type_id int,
		@party_cnt int,
		@Rate numeric(19, 10),
		@Commission_value numeric(19, 4),
		@is_value tinyint,
		@Lead_Commission_band int,
		@Sub_Commission_band int,
		@Product_id int,
		@Risk_Type_id int,
		@premium numeric(19, 4),
		@effective_date datetime,
		@annual_premium numeric(19, 4),
		@transaction_type_id int,
		@maximum_rate numeric(19, 4),
		@is_amended TINYINT,
		@use_override_commission_rate Tinyint,
        @use_override_commission_rate_at_renewal Tinyint,
		@override_reason varchar(255) ,
		@temprate numeric(19, 10),
		@tempIs_Value tinyint,
		@temptax_group_id INT,
		@use_policy_inception_date tinyint= 0,
		@class_of_business_id INT,
		@peril_type_id INT 
     

	DECLARE @currency_id SMALLINT,  
		@base_currency_id SMALLINT,  
		@base_commission_value MONEY,  
		@tax_group_id INT  
  
 DECLARE @is_true_monthly_policy TINYINT,  
  @lead_allow_consolidated_commission TINYINT,  
  @sub_allow_consolidated_commission TINYINT,  
  @lead_month_in_cycle TINYINT,  
  @sub_month_in_cycle TINYINT,  
  @month_in_cycle TINYINT,  
  @renewal_count TINYINT,  
  @lead_multiplier FLOAT,  
  @sub_multiplier FLOAT,  
  @lead_clawback_commission TINYINT,  
  @sub_clawback_commission TINYINT,  
  @subagent_consolidated TINYINT,  
  @temp_multiplier FLOAT ,  
  @commission_level_id int  ,  
  @Insurance_folder_cnt INT,  
  @insurance_file_type_id TINYINT,
  @cover_start_date datetime ,
  @CurrencybaseXrate FLOAT  ,  
  @return_status   INT  ,
  @company_id int,
  @account_id int
  
    --Same as portal where GetAgentCommission updates MaximumRate using spu_upd_commission_max_rate
	  DECLARE @RiskTypeID INT
	  DECLARE @CommissionDandID INT
	  DECLARE @RiskTypeCode AS VARCHAR(10)
	  DECLARE @CommisionBandCode AS VARCHAR(10)
  select @cover_start_date = cover_start_date from insurance_file where Insurance_File_cnt = @insurance_file_cnt      

    -- Read the commission display level option (5264: Display Commission at Commission Band Level)
    DECLARE @display_band_level BIT = 0
    SELECT @display_band_level = ISNULL(
        (SELECT CAST(so.value AS BIT) 
         FROM system_options so
         INNER JOIN insurance_file ifile ON ifile.insurance_file_cnt = @insurance_file_cnt
         INNER JOIN source s ON s.source_id = ifile.source_id
         WHERE so.branch_id = s.source_id 
         AND so.option_number = 5264
         AND so.value = '1'), 0)
                  
         
         
  if @display_band_level = 1
         BEGIN
            -- If we are displaying at band level, then we don't want to group by class of business
           -- Declare the cursor to return the Sub agents for the insurance  
        Declare Sub_agent_cursor cursor FAST_FORWARD for  
		Select P.Party_agent_type_id,  
		IFA.Party_cnt,  
		P.allow_consolidated_commission ,  
		p.commission_level_id  
		From Insurance_file_agent IFA,  
		Party_agent P  
		Where IFA.Insurance_file_cnt = @insurance_file_cnt  
		And IFA.Party_cnt = P.Party_cnt  
  
    -- Declare the cursor to get the premiums for each commission band  
    Declare Lead_Peril_Cursor Cursor FAST_FORWARD For  
        Select Lead_commission_band,  
        r.Risk_type_id,  
        Convert(numeric(19, 4), Sum(this_Premium)),  
        Convert(numeric(19, 4), Sum(annual_Premium))  
        from Peril p,  
        Risk r,  
        insurance_file_risk_link ifr  
        Where ifr.Insurance_file_cnt = @insurance_file_cnt  
        -- And ifr.status_flag = 'C'  
        And ifr.status_flag <> 'U'  
        And r.risk_cnt = ifr.risk_cnt  
        And p.risk_cnt = r.risk_cnt  
        And IsNull(p.is_levy_tax,0)=0   -- Thinh Nguyen (16/07/2002) ignore levy tax  
        And r.is_risk_selected=1        -- Alix - 10/12/2002 - Issue 1575  
        group by lead_commission_band, Risk_type_id  
  
    Declare Sub_Peril_Cursor Cursor FAST_FORWARD For  
        Select Sub_commission_band,  
        r.Risk_Type_id,  
        Convert(numeric(19, 4), Sum(this_Premium)),  
        Convert(numeric(19, 4), Sum(annual_Premium))  
        from Peril p,  
        Risk r,  
        insurance_file_risk_link ifr  
        Where ifr.Insurance_file_cnt = @insurance_file_cnt  
        -- And ifr.status_flag = 'C'  
        And ifr.status_flag <> 'U'  
        And r.risk_cnt = ifr.risk_cnt  
        And p.Risk_cnt = r.risk_cnt  
        And IsNull(p.is_levy_tax,0)=0   
        And r.is_risk_selected=1        
        And Sub_commission_band IS NOT NULL  
        group by Sub_commission_band, Risk_type_id  
  
        SELECT @Effective_date = cover_start_date  
        FROM insurance_file  
        WHERE insurance_file_cnt = @insurance_file_cnt  
    
    Select @Effective_date = isnull(@Effective_date, Getdate())  
  
    --Get the insurance folder cnt associated to current insurance file cnt  
      SELECT @Insurance_folder_cnt=Insurance_Folder_cnt  
      FROM Insurance_File  
      WHERE insurance_file_cnt =@insurance_file_cnt  

	   --Same as portal where GetAgentCommission updates MaximumRate using spu_upd_commission_max_rate    
 SELECT @RiskTypeID = ac.Risk_type_id, @CommissionDandID = ac.Commission_band_id
   FROM insurance_file ifi  
   JOIN insurance_file_risk_link ifrl  
   ON ifi.insurance_file_cnt=ifrl.insurance_file_cnt  
        JOIN Agent_commission ac ON  
      ifi.insurance_file_cnt = ac.insurance_file_cnt  AND ifi.lead_agent_cnt = ac.party_cnt  
      AND ac.Insurance_file_cnt = @insurance_file_cnt  

	  SELECT @RiskTypeCode = RTRIM(Code) from Risk_Type where risk_type_id = @RiskTypeID
	  SELECT @CommisionBandCode = RTRIM(code) from Commission_Band where commission_band_id = @CommissionDandID
	  exec spu_upd_commission_max_rate @insurance_file_cnt,@RiskTypeCode ,@CommisionBandCode

   
  -- SELECT THE LATEST LIVE POLICY PRIOR TO CURRENT INSURANCE FILE CNT        
    SELECT Risk_type_id, Commission_band_id ,ac.commission_percentage,ac.is_amended, ac.tax_group_id, ac.Is_Value, ac.Maximum_rate,ac.override_reason        
    INTO #Percentage_BAND        
    FROM insurance_file ifi        
        JOIN Agent_commission ac ON        
        ifi.insurance_file_cnt = ac.insurance_file_cnt  AND ifi.lead_agent_cnt = ac.party_cnt        
        AND ac.Insurance_file_cnt = @insurance_file_cnt
  
  IF @is_amended IS NULL  
  SET @is_amended=0  
 
  EXEC spu_sir_agent_commission_del @insurance_file_cnt
  
 -- Calculate multiplier for Indemnity Commission  
 EXEC spu_get_true_monthly_policy_details @insurance_file_cnt,  
  @is_true_monthly_policy=@is_true_monthly_policy OUTPUT,  
  @lead_month_in_cycle=@lead_month_in_cycle OUTPUT,  
  @sub_month_in_cycle=@sub_month_in_cycle OUTPUT,  
  @lead_allow_consolidated_commission=@lead_allow_consolidated_commission OUTPUT,  
  @sub_allow_consolidated_commission=@sub_allow_consolidated_commission OUTPUT,  
  @transaction_type_code=NULL,  
  @renewal_count=@renewal_count OUTPUT  
  
  SELECT @lead_multiplier=1  
  SELECT @sub_multiplier=1  
  SELECT @lead_clawback_commission=0  
  SELECT @sub_clawback_commission=0  
  
 -- We only do this logic within the first year  
 If @is_true_monthly_policy=1 BEGIN  
  IF @transaction_type='MTC' AND @renewal_count<12 BEGIN  
   IF @lead_allow_consolidated_commission=1 BEGIN  
    IF @renewal_count>=@lead_month_in_cycle BEGIN  
     SELECT @lead_clawback_commission=1  
       SELECT @lead_multiplier=(12-CAST(@renewal_count AS FLOAT)-1)/12  
    END  
    ELSE BEGIN  
     SELECT @lead_clawback_commission=2  
     SELECT @lead_multiplier=0  
    END  
   END  
   ELSE  
   SELECT @lead_multiplier=1  
  
   IF @sub_allow_consolidated_commission=1 BEGIN  
    IF @renewal_count>=@sub_month_in_cycle BEGIN  
     SELECT @sub_clawback_commission=1  
     SELECT @sub_multiplier=(12-CAST(@renewal_count AS FLOAT)-1)/12  
    END  
    ELSE BEGIN  
     SELECT @sub_clawback_commission=2  
     SELECT @sub_multiplier=0  
    END  
   END  
   ELSE  
    SELECT @sub_multiplier=1  
  END  
  ELSE IF @renewal_count=0 AND @transaction_type<>'REN' AND @transaction_type<>'MTA' BEGIN  
   IF @lead_allow_consolidated_commission=1  
       SELECT @lead_multiplier=12  
  
   IF @sub_allow_consolidated_commission=1  
       SELECT @sub_multiplier=12  
  END  
  ELSE IF @renewal_count < 11 BEGIN  
   IF @lead_allow_consolidated_commission=1  
       SELECT @lead_multiplier=0  
  
   IF @sub_allow_consolidated_commission=1  
       SELECT @sub_multiplier=0  
  
  END  
 END  
  
    Select @transaction_type_id = transaction_type_id  
    From Transaction_type  
    Where code = @transaction_type  
  
    -- Process for lead_commission_Band  
    Open Lead_Peril_Cursor  
    Fetch Next From Lead_Peril_Cursor Into @Lead_Commission_Band, @Risk_Type_id, @Premium, @annual_premium  
  
    While @@Fetch_Status = 0 Begin  
        -- Get the Lead Agent details from the Insurance File  
        Select @Party_Type_id = P.party_agent_type_id,  
     @Party_cnt = lead_agent_cnt,  
     @Product_id = Product_id,  
		@commission_level_id = (SELECT TOP 1 commission_level_id      
								 FROM Agent_Commission_level ACL       
								 WHERE ACL.party_agent_cnt = P.Party_cnt       
								 AND ACL.effective_date <= ifile.cover_start_date      
                              and ACL.Is_deleted = 0
								 ORDER BY ACL.effective_date desc,
								 ACL.Agent_Commission_Level_id desc),  
     @use_override_commission_rate = use_override_commission_rate,  
     @use_override_commission_rate_at_renewal = use_override_commission_renewal   
     from Insurance_file ifile,  
     Party_agent P  
     Where Insurance_file_cnt = @insurance_file_cnt  
     And lead_agent_cnt = P.Party_cnt  
  
   SET @Rate = NULL  
   SET @is_amended = 0  
     SET @gis_object_commission_rate = NULL 
   SELECT @rate=commission_percentage,@is_amended=is_amended, @tax_group_id = tax_group_id, @Is_Value = is_value, @maximum_rate = maximum_rate,@override_reason=override_reason  
    FROM #Percentage_BAND  
   WHERE  Risk_type_id =  @Risk_Type_id AND Commission_band_id = @Lead_Commission_Band  
  
   IF @party_cnt is not null 
   BEGIN 
   EXEC spu_sir_get_agent_commission_from_output @insurance_file_cnt =@insurance_file_cnt,     
		@party_id =@Party_cnt,
		@commission_band_id =@Lead_Commission_band, 
		@commission_rate =@gis_object_commission_rate OUTPUT
		 If  @gis_object_commission_rate IS NOT NULL
		  set @rate=@gis_object_commission_rate 
 
           Set @gis_object_commission_rate=0.0 
select @use_policy_inception_date = ISNULL(use_policy_inception_date,0) from Product p inner join Insurance_File inf on p.product_id = inf.product_id where inf.insurance_file_cnt= @insurance_file_cnt 
  

 -- Get the commission_Rate  
-- Fix: Only recalculate if rate is 0 AND not manually amended
IF ISNULL(@rate,0) = 0 
   AND (ISNULL(@is_amended,0) = 0 
        OR (((@is_amended = 0 AND @transaction_type = 'REN') 
              OR (ISNULL(@use_override_commission_rate, 0) = 0 AND @transaction_type = 'REN') 
              OR (ISNULL(@use_override_commission_rate_at_renewal, 0) = 0 AND @transaction_type = 'REN')) 
             AND (@use_policy_inception_date = 0 AND @transaction_type = 'REN')))
BEGIN  

	--Set @temprate=@rate
	--Set @tempIs_Value =@is_value
	--Set @temptax_group_id= @tax_group_id
 Execute spu_Sir_Calc_Commission_Rate  
  @Party_Type_id,  
     @Party_cnt,  
     @Product_id,  
     @Risk_Type_id,  
     @transaction_type_id,  
     @Lead_Commission_band,  
     @effective_date,  
     @rate OUT,  
     @Is_Value OUT,  
     @tax_group_id OUT,  
     @transaction_type,  
     @insurance_file_cnt,  
     1,  
     @maximum_rate OUT ,  
	 @commission_level_id 

	--IF @temprate IS NOT NULL AND (@is_amended<>0)
	--	BEGIN
	--	Set @rate=@temprate
	--	Set @Is_Value =@tempis_value
	--	Set @tax_group_id= @temptax_group_id
	-- END  

	 
 END
             if @lead_clawback_commission = 1 begin  
                Select @Commission_value = (ROUND(ROUND((@annual_premium * @Rate),4) / 100,4)*12) * @lead_multiplier  
                Select @rate = 0  
            End Else if @lead_clawback_commission = 2 begin  
                Select @Commission_value =ROUND(ROUND((@annual_premium * @Rate),4) / 100,4) * @lead_multiplier  
                Select @rate = 0  
          End Else if @is_true_monthly_policy=1 and @lead_allow_consolidated_commission=1 begin  
                Select @commission_value = ROUND(ROUND((@annual_premium * @Rate),4) / 100,4) * @lead_multiplier  
          End Else if ISNULL(@is_value,0) = 0 begin  
                Select @commission_value = ROUND(ROUND((@Premium * @Rate),4) / 100,4) * @lead_multiplier  
          End Else if @is_value = 1 begin  
                Select @commission_value = @Rate * @lead_multiplier  
            end  
  
             EXEC spu_sir_agent_commission_add  
                 @insurance_file_cnt = @insurance_file_cnt,  
                 @is_lead_agent = 1,  
                 @party_cnt = @party_cnt,  
                 @risk_type_id = @risk_type_id,  
                 @commission_band_id = @Lead_Commission_band,  
                 @premium = @Premium,  
                 @Commission_percentage = @rate,  
                 @commission_value = @commission_value,  
                 @is_amended = @is_amended,  
                 @tax_group_id = @tax_group_id,  
                 @calculated_commission_value=@commission_value,  
                 @override_reason=@override_reason,  
				 @maximum_rate=@maximum_rate,  
				 @is_value=@is_value,  
				 @commission_level_id = @commission_level_id,  
				 @ViaCalculateAgentCommission=1  
  END  
  
        -- Fetch the next record  
        Fetch Next From Lead_Peril_Cursor Into @Lead_Commission_Band, @Risk_Type_id, @Premium, @annual_premium  
    End  
  
    -- Close and Deallocate  
    Close Lead_Peril_Cursor  
    Deallocate Lead_Peril_Cursor  
  
    -- Process for lead_commission_Band  
    Open Sub_Peril_Cursor  
    Fetch Next From Sub_Peril_Cursor Into @Sub_Commission_Band, @Risk_Type_id, @Premium, @annual_premium  
  
    While @@Fetch_Status = 0 Begin  
  
        -- Get the Product Id from the Insurance file  
        Select @Product_id = Product_Id  
        From Insurance_file  
        Where Insurance_file_cnt = @insurance_file_cnt  
  
        -- Open the cursor to Get the subagents  
        Open Sub_agent_cursor  
        Fetch Next From Sub_Agent_Cursor into @Party_type_id, @Party_cnt, @subagent_consolidated ,@commission_level_id  
  
        While @@Fetch_Status = 0 Begin  
            -- Get the commission_Rate  
            Execute spu_Sir_Calc_Commission_Rate  
                @Party_Type_id,  
                @Party_cnt,  
                @Product_id,  
                @Risk_Type_id,  
                @transaction_type_id,  
                @Sub_Commission_Band,  
                @effective_date,  
                @rate OUT,  
                @Is_Value OUT,  
                @tax_group_id OUT,  
				@transaction_type,  
				@insurance_file_cnt,  
				0,  
				@maximum_rate OUT ,  
				@commission_level_id  
  
   IF @subagent_consolidated=1  
    SELECT @temp_multiplier = @sub_multiplier  
   ELSE  
    SELECT @temp_multiplier = 1  
  
            -- Check the rate and calculate the commission value accordingly  
            if @sub_clawback_commission = 1 begin  
                Select @Commission_value = (((@annual_premium * @Rate) / 100)*12) * @temp_multiplier  
         Select @rate = 0  
            End Else if @sub_clawback_commission = 2 begin  
                Select @Commission_value = ((@annual_premium * @Rate) / 100) * @temp_multiplier  
                Select @rate = 0  
     End Else if @is_true_monthly_policy=1 and @sub_allow_consolidated_commission=1 begin  
                Select @commission_value = ((@annual_premium * @Rate) / 100) * @temp_multiplier  
            End Else if @Is_Value = 0 begin  
                Select @commission_value = ((@Premium * @Rate) / 100) * @temp_multiplier  
   End Else if @Is_Value = 1 begin  
                Select @commission_value =  @Rate * @temp_multiplier  
            end
  
             EXEC spu_sir_agent_commission_add  
                 @insurance_file_cnt = @insurance_file_cnt,  
                 @is_lead_agent = 0,  
                 @party_cnt = @party_cnt,  
                 @risk_type_id = @risk_type_id,  
                 @commission_band_id = @Sub_Commission_Band,  
			     @premium = @Premium,  
                 @Commission_percentage = @rate,  
                 @commission_value = @commission_value,  
                 @is_amended = 0,  
                 @tax_group_id = @tax_group_id,  
                 @calculated_commission_value=@commission_value,  
                 @override_reason=@override_reason,  
				 @maximum_rate=@maximum_rate,  
				 @is_value=@is_value ,  
				 @commission_level_id = @commission_level_id,  
				 @ViaCalculateAgentCommission=1  
  
            -- Select the next record from the sub agent cursor  
            Fetch Next From Sub_Agent_Cursor into @Party_type_id, @Party_cnt, @subagent_consolidated ,@commission_level_id  
        End  
  
        -- Close the Sub agent Cursor  
        Close Sub_Agent_Cursor  
  
        Fetch Next From Sub_Peril_Cursor Into @Sub_Commission_Band, @Risk_Type_id, @Premium, @annual_premium  
    End  
  
    -- Deallocate the Sub agent Cursor  
    Deallocate Sub_Agent_Cursor  
  
    -- Close and Deallocate the cursors  
    Close Sub_Peril_Cursor  
    Deallocate Sub_Peril_Cursor  
  END
  ELSE
  BEGIN
       -- Declare the cursor to return the Sub agents for the insurance  
    Declare Sub_agent_cursor cursor FAST_FORWARD for  
  Select P.Party_agent_type_id,  
  IFA.Party_cnt,  
  P.allow_consolidated_commission ,  
  commission_level_id   = ( SELECT TOP 1 commission_level_id      
							FROM Agent_Commission_level ACL       
							WHERE ACL.party_agent_cnt = P.party_cnt       
							and ACL.effective_date <= @cover_start_date      
                            and ACL.Is_deleted = 0
							ORDER BY ACL.effective_date desc,
							ACL.Agent_Commission_Level_id desc)   
  From Insurance_file_agent IFA,  
  Party_agent P  
  Where IFA.Insurance_file_cnt = @insurance_file_cnt  
  And IFA.Party_cnt = P.Party_cnt  
  
  -- Declare the cursor to get the premiums for each commission band  
    Declare Lead_Peril_Cursor Cursor FAST_FORWARD For  
        Select Lead_commission_band,  
        r.Risk_type_id,  
        Convert(numeric(19, 4), Sum(this_Premium)),  
        Convert(numeric(19, 4), Sum(annual_Premium)),  
  p.class_of_business_id,  
  p.peril_type_id  
        from Peril p,  
        Risk r,  
        insurance_file_risk_link ifr  
        Where ifr.Insurance_file_cnt = @insurance_file_cnt  
        -- And ifr.status_flag = 'C'  
        And ifr.status_flag <> 'U'  
        And r.risk_cnt = ifr.risk_cnt  
        And p.risk_cnt = r.risk_cnt  
        And IsNull(p.is_levy_tax,0)=0   -- Thinh Nguyen (16/07/2002) ignore levy tax  
        And r.is_risk_selected=1        -- Alix - 10/12/2002 - Issue 1575  
        group by lead_commission_band, Risk_type_id, p.class_of_business_id, p.peril_type_id  
  
    Declare Sub_Peril_Cursor Cursor FAST_FORWARD For  
        Select Sub_commission_band,  
        r.Risk_Type_id,  
        Convert(numeric(19, 4), Sum(this_Premium)),  
        Convert(numeric(19, 4), Sum(annual_Premium)),  
  p.class_of_business_id,  
  p.peril_type_id  
        from Peril p,  
        Risk r,  
        insurance_file_risk_link ifr  
        Where ifr.Insurance_file_cnt = @insurance_file_cnt  
        -- And ifr.status_flag = 'C'  
        And ifr.status_flag <> 'U'  
        And r.risk_cnt = ifr.risk_cnt  
        And p.Risk_cnt = r.risk_cnt  
        And IsNull(p.is_levy_tax,0)=0  
        And r.is_risk_selected=1  
        And Sub_commission_band IS NOT NULL  
        group by Sub_commission_band, Risk_type_id, p.class_of_business_id, p.peril_type_id  
  
        SELECT @Effective_date = cover_start_date  
        FROM insurance_file  
        WHERE insurance_file_cnt = @insurance_file_cnt  
  
    Select @Effective_date = isnull(@Effective_date, Getdate())  
  
    --Get the insurance folder cnt associated to current insurance file cnt  
      SELECT @Insurance_folder_cnt=Insurance_Folder_cnt  
      FROM Insurance_File  
      WHERE insurance_file_cnt =@insurance_file_cnt  
  
   --Same as portal where GetAgentCommission updates MaximumRate using spu_upd_commission_max_rate    
  
   SELECT @RiskTypeID = ac.Risk_type_id, @CommissionDandID = ac.Commission_band_id, @class_of_business_id = p.class_of_business_id, @peril_type_id = p.peril_type_id  
   FROM insurance_file ifi  
   JOIN insurance_file_risk_link ifrl  
   ON ifi.insurance_file_cnt=ifrl.insurance_file_cnt  
   JOIN Risk r  
   ON r.risk_cnt = ifrl.risk_cnt  
   JOIN Peril p  
   ON r.risk_cnt = p.risk_cnt  
      JOIN Agent_commission ac ON  
      ifi.insurance_file_cnt = ac.insurance_file_cnt  AND ifi.lead_agent_cnt = ac.party_cnt  
      AND ac.Insurance_file_cnt = @insurance_file_cnt  
  
   SELECT @RiskTypeCode = RTRIM(Code) from Risk_Type where risk_type_id = @RiskTypeID  
   SELECT @CommisionBandCode = RTRIM(code) from Commission_Band where commission_band_id = @CommissionDandID  
   exec spu_upd_commission_max_rate @insurance_file_cnt,@RiskTypeCode ,@CommisionBandCode  

  -- SELECT THE LATEST LIVE POLICY PRIOR TO CURRENT INSURANCE FILE CNT  
    SELECT Risk_type_id, Commission_band_id ,ac.commission_percentage,ac.is_amended, ac.tax_group_id, ac.Is_Value, ac.Maximum_rate,ac.override_reason  
    INTO #Percentage_Peril  
    FROM insurance_file ifi  
        JOIN Agent_commission ac ON  
        ifi.insurance_file_cnt = ac.insurance_file_cnt  AND ifi.lead_agent_cnt = ac.party_cnt  
        AND ac.Insurance_file_cnt = @insurance_file_cnt  
  
  IF @is_amended IS NULL  
  SET @is_amended=0  
  
  EXEC spu_sir_agent_commission_del @insurance_file_cnt  
  
 -- Calculate multiplier for Indemnity Commission  
 EXEC spu_get_true_monthly_policy_details @insurance_file_cnt,  
  @is_true_monthly_policy=@is_true_monthly_policy OUTPUT,  
  @lead_month_in_cycle=@lead_month_in_cycle OUTPUT,  
  @sub_month_in_cycle=@sub_month_in_cycle OUTPUT,  
  @lead_allow_consolidated_commission=@lead_allow_consolidated_commission OUTPUT,  
  @sub_allow_consolidated_commission=@sub_allow_consolidated_commission OUTPUT,  
  @transaction_type_code=NULL,  
  @renewal_count=@renewal_count OUTPUT  
  
  SELECT @lead_multiplier=1  
  SELECT @sub_multiplier=1  
  SELECT @lead_clawback_commission=0  
  SELECT @sub_clawback_commission=0  
  
 -- We only do this logic within the first year  
 If @is_true_monthly_policy=1 BEGIN  
  IF @transaction_type='MTC' AND @renewal_count<12 BEGIN  
   IF @lead_allow_consolidated_commission=1 BEGIN  
    IF @renewal_count>=@lead_month_in_cycle BEGIN  
     SELECT @lead_clawback_commission=1  
       SELECT @lead_multiplier=(12-CAST(@renewal_count AS FLOAT)-1)/12  
    END  
    ELSE BEGIN  
     SELECT @lead_clawback_commission=2  
     SELECT @lead_multiplier=0  
    END  
   END  
   ELSE  
   SELECT @lead_multiplier=1  
  
   IF @sub_allow_consolidated_commission=1 BEGIN  
    IF @renewal_count>=@sub_month_in_cycle BEGIN  
     SELECT @sub_clawback_commission=1  
     SELECT @sub_multiplier=(12-CAST(@renewal_count AS FLOAT)-1)/12  
    END  
    ELSE BEGIN  
     SELECT @sub_clawback_commission=2  
     SELECT @sub_multiplier=0  
    END  
   END  
   ELSE  
    SELECT @sub_multiplier=1  
  END  
  ELSE IF @renewal_count=0 AND @transaction_type<>'REN' AND @transaction_type<>'MTA' BEGIN  
   IF @lead_allow_consolidated_commission=1  
       SELECT @lead_multiplier=12  
  
   IF @sub_allow_consolidated_commission=1  
       SELECT @sub_multiplier=12  
  END  
  ELSE IF @renewal_count < 11 BEGIN  
   IF @lead_allow_consolidated_commission=1  
       SELECT @lead_multiplier=0  
  
   IF @sub_allow_consolidated_commission=1  
       SELECT @sub_multiplier=0  
  
  END  
 END  
  
    Select @transaction_type_id = transaction_type_id  
    From Transaction_type  
    Where code = @transaction_type  
  
    -- Process for lead_commission_Band  
    Open Lead_Peril_Cursor  
    Fetch Next From Lead_Peril_Cursor Into @Lead_Commission_Band, @Risk_Type_id, @Premium, @annual_premium, @class_of_business_id, @peril_type_id  
  
While @@Fetch_Status = 0 Begin  
        -- Get the Lead Agent details from the Insurance File  
        Select @Party_Type_id = P.party_agent_type_id,  
            @Party_cnt = lead_agent_cnt,  
            @Product_id = Product_id,  
			@commission_level_id = (SELECT TOP 1 commission_level_id      
									 FROM Agent_Commission_level ACL       
									 WHERE ACL.party_agent_cnt = P.Party_cnt       
									 AND ACL.effective_date <= ifile.cover_start_date      
                                     and ACL.Is_deleted = 0
									 ORDER BY ACL.effective_date desc,
									 ACL.Agent_Commission_Level_id desc),  
            @use_override_commission_rate = use_override_commission_rate,  
            @use_override_commission_rate_at_renewal = use_override_commission_renewal   
            from Insurance_file ifile,  
            Party_agent P  
            Where Insurance_file_cnt = @insurance_file_cnt  
            And lead_agent_cnt = P.Party_cnt  
  
   SET @Rate = NULL  
   SET @is_amended = 0  
   SET @gis_object_commission_rate = NULL 
    
   SELECT @rate=commission_percentage,@is_amended=is_amended, @tax_group_id = tax_group_id, @Is_Value = is_value, @maximum_rate = maximum_rate,@override_reason=override_reason  
    FROM #Percentage_Peril  
   WHERE  Risk_type_id =  @Risk_Type_id AND Commission_band_id = @Lead_Commission_Band  
  
   IF @party_cnt is not null  
   BEGIN  
   EXEC spu_sir_get_agent_commission_from_output @insurance_file_cnt =@insurance_file_cnt,  
  @party_id =@Party_cnt,  
  @commission_band_id =@Lead_Commission_band,  
  @commission_rate =@gis_object_commission_rate OUTPUT  
   If  @gis_object_commission_rate IS NOT NULL  
    set @rate=@gis_object_commission_rate  
  
 Set @gis_object_commission_rate=0.0 

select @use_policy_inception_date = ISNULL(use_policy_inception_date,0) from Product p inner join Insurance_File inf on p.product_id = inf.product_id where inf.insurance_file_cnt= @insurance_file_cnt 
  
 -- Get the commission_Rate  
-- Fix: Only recalculate if rate is 0 AND not manually amended
IF ISNULL(@rate,0) = 0 
   AND (ISNULL(@is_amended,0) = 0 
        OR (((@is_amended = 0 AND @transaction_type = 'REN') 
              OR (ISNULL(@use_override_commission_rate, 0) = 0 AND @transaction_type = 'REN') 
              OR (ISNULL(@use_override_commission_rate_at_renewal, 0) = 0 AND @transaction_type = 'REN')) 
             AND (@use_policy_inception_date = 0 AND @transaction_type = 'REN')))
BEGIN  
 --Set @temprate=@rate  
 --Set @tempIs_Value =@is_value  
 --Set @temptax_group_id= @tax_group_id  
 Execute spu_Sir_Calc_Commission_Rate  
  @Party_Type_id,  
     @Party_cnt,  
     @Product_id,  
     @Risk_Type_id,  
     @transaction_type_id,  
     @Lead_Commission_band,  
     @effective_date,  
     @rate OUT,  
     @Is_Value OUT,  
     @tax_group_id OUT,  
     @transaction_type,  
     @insurance_file_cnt,  
     1,  
     @maximum_rate OUT ,  
  @commission_level_id,  
  @is_amended OUT  
  
 --IF @temprate IS NOT NULL AND (@is_amended<>0)  
 -- BEGIN  
 -- Set @rate=@temprate  
 -- Set @Is_Value =@tempis_value  
 -- Set @tax_group_id= @temptax_group_id  
 -- END  
  
 END  

IF @Is_Value = 1  
BEGIN  

SELECT @account_id = account_id
		FROM account
			WHERE account_key = @party_cnt

	SELECT
		@company_id = source_id,
		@currency_id = currency_id
	FROM insurance_file
	WHERE insurance_file_cnt = @insurance_file_cnt

    EXECUTE spu_ACT_Do_Currency_Conversion
		@account_id = @account_id,
		@company_id = @company_id,
		@currency_id = @currency_id,
		@currency_amount_unrounded = 0,
		@mode = 'ALL',
		@currency_base_xrate  = @CurrencybaseXrate OUTPUT,
		@return_status = @return_status OUTPUT 

	  
	IF ISNULL(@CurrencybaseXrate,0) =0    SELECT @CurrencybaseXrate=1
	SET @Commission_value = ROUND(@rate/@CurrencybaseXrate,4)
	 

	if @lead_clawback_commission = 1 OR @lead_clawback_commission = 2 OR @premium=0    
		Select @rate = 0 
	ELSE if @is_true_monthly_policy=1 and @lead_allow_consolidated_commission=1
		SET @Rate = (@commission_value / @lead_multiplier ) * 100 / @annual_premium
	ELSE
		SET @Rate = (@commission_value / @lead_multiplier ) * 100 / @Premium

END
ELSE
BEGIN
            -- Check the rate and calculate the commission value accordingly  
            if @lead_clawback_commission = 1 begin  
                Select @Commission_value = (ROUND(ROUND((@annual_premium * @Rate),4) / 100,4)*12) * @lead_multiplier  
                Select @rate = 0  
            End Else if @lead_clawback_commission = 2 begin  
                Select @Commission_value =ROUND(ROUND((@annual_premium * @Rate),4) / 100,4) * @lead_multiplier  
                Select @rate = 0  
          End Else if @is_true_monthly_policy=1 and @lead_allow_consolidated_commission=1 begin  
                Select @commission_value = ROUND(ROUND((@annual_premium * @Rate),4) / 100,4) * @lead_multiplier  
          End Else if ISNULL(@is_value,0) = 0 begin  
                Select @commission_value = ROUND(ROUND((@Premium * @Rate),4) / 100,4) * @lead_multiplier  
          End Else if @is_value = 1 begin  
                Select @commission_value = @Rate * @lead_multiplier  
            end  
END

              EXEC spu_sir_agent_commission_add  
                 @insurance_file_cnt = @insurance_file_cnt,  
                 @is_lead_agent = 1,  
                 @party_cnt = @party_cnt,  
                 @risk_type_id = @risk_type_id,  
                 @commission_band_id = @Lead_Commission_band,  
                 @premium = @Premium,  
                 @Commission_percentage = @rate,  
                 @commission_value =  @commission_value,  
                 @is_amended = @is_amended,  
                 @tax_group_id = @tax_group_id,  
                 @calculated_commission_value=@commission_value,  
                 @override_reason=@override_reason,  
     @maximum_rate=@maximum_rate,  
     @is_value=@is_value,  
     @commission_level_id = @commission_level_id,  
     @ViaCalculateAgentCommission=1,  
            @class_of_business_id =@class_of_business_id,  
     @peril_type_id = @peril_type_id  
  END  
  
    -- Fetch the next record  
        Fetch Next From Lead_Peril_Cursor Into @Lead_Commission_Band, @Risk_Type_id, @Premium, @annual_premium, @class_of_business_id, @peril_type_id  
    End  
  
    -- Close and Deallocate  
    Close Lead_Peril_Cursor  
    Deallocate Lead_Peril_Cursor  
  
    -- Process for lead_commission_Band  
    Open Sub_Peril_Cursor  
    Fetch Next From Sub_Peril_Cursor Into @Sub_Commission_Band, @Risk_Type_id, @Premium, @annual_premium, @class_of_business_id, @peril_type_id  
  
    While @@Fetch_Status = 0 Begin  
  
        -- Get the Product Id from the Insurance file  
        Select @Product_id = Product_Id  
        From Insurance_file  
        Where Insurance_file_cnt = @insurance_file_cnt  
  
        -- Open the cursor to Get the subagents  
        Open Sub_agent_cursor  
        Fetch Next From Sub_Agent_Cursor into @Party_type_id, @Party_cnt, @subagent_consolidated ,@commission_level_id  
  
        While @@Fetch_Status = 0 Begin  
            -- Get the commission_Rate  
            Execute spu_Sir_Calc_Commission_Rate  
                @Party_Type_id,  
                @Party_cnt,  
                @Product_id,  
                @Risk_Type_id,  
                @transaction_type_id,  
                @Sub_Commission_Band,  
                @effective_date,  
                @rate OUT,  
                @Is_Value OUT,  
                @tax_group_id OUT,  
    @transaction_type,  
    @insurance_file_cnt,  
    0,  
    @maximum_rate OUT ,  
    @commission_level_id  
  
   IF @subagent_consolidated=1  
    SELECT @temp_multiplier = @sub_multiplier  
   ELSE  
    SELECT @temp_multiplier = 1  
  
            -- Check the rate and calculate the commission value accordingly  
            if @sub_clawback_commission = 1 begin  
                Select @Commission_value = (((@annual_premium * @Rate) / 100)*12) * @temp_multiplier  
         Select @rate = 0  
            End Else if @sub_clawback_commission = 2 begin  
                Select @Commission_value = ((@annual_premium * @Rate) / 100) * @temp_multiplier  
                Select @rate = 0  
     End Else if @is_true_monthly_policy=1 and @sub_allow_consolidated_commission=1 begin  
                Select @commission_value = ((@annual_premium * @Rate) / 100) * @temp_multiplier  
            End Else if @Is_Value = 0 begin  
                Select @commission_value = ((@Premium * @Rate) / 100) * @temp_multiplier  
   End Else if @Is_Value = 1 begin  
                Select @commission_value =  @Rate * @temp_multiplier  
            end  
 
             EXEC spu_sir_agent_commission_add  
                 @insurance_file_cnt = @insurance_file_cnt,  
                 @is_lead_agent = 0,  
                 @party_cnt = @party_cnt,  
                 @risk_type_id = @risk_type_id,  
                 @commission_band_id = @Sub_Commission_Band,  
        @premium = @Premium,  
                 @Commission_percentage = @rate,  
                 @commission_value = @commission_value,  
                 @is_amended = 0,  
                 @tax_group_id = @tax_group_id,  
                 @calculated_commission_value=@commission_value,  
                 @override_reason=@override_reason,  
     @maximum_rate=@maximum_rate,  
     @is_value=@is_value ,  
     @commission_level_id = @commission_level_id,  
     @ViaCalculateAgentCommission=1,  
         @class_of_business_id =@class_of_business_id,  
     @peril_type_id = @peril_type_id    
  
            -- Select the next record from the sub agent cursor  
            Fetch Next From Sub_Agent_Cursor into @Party_type_id, @Party_cnt, @subagent_consolidated ,@commission_level_id  
        End  
  
        -- Close the Sub agent Cursor  
        Close Sub_Agent_Cursor  
  
        Fetch Next From Sub_Peril_Cursor Into @Sub_Commission_Band, @Risk_Type_id, @Premium, @annual_premium, @class_of_business_id, @peril_type_id  
    End  
  
    -- Deallocate the Sub agent Cursor  
    Deallocate Sub_Agent_Cursor  
  
    -- Close and Deallocate the cursors  
    Close Sub_Peril_Cursor  
    Deallocate Sub_Peril_Cursor  
  END
    -- Select the Data from the table  
    SELECT  
        P.shortname,  
        PAT.Description,  
        RT.Code,  
        CB.Code,  
        AC.premium,  
        AC.commission_percentage,  
        AC.commission_value,  
        AC.is_lead_agent,  
        AC.is_amended,  
        AC.party_cnt,  
        PAT.Party_Agent_Type_id,  
        AC.risk_type_id,  
        AC.commission_band_id,  
        C.description,  
        AC.tax_group_id,  
       TG.description,  
        AC.tax_amount,  
        AC.calculated_commission_value,  
        AC.override_reason,  
  AC.maximum_rate,  
  AC.Is_Value ,  
  AC.peril_type_id,  
  AC.class_of_business_id  
  
    FROM Agent_Commission AC  
    JOIN Risk_Type RT  
        ON RT.Risk_Type_id = AC.Risk_Type_id  
    JOIN commission_band CB  
        ON CB.commission_band_id = AC.commission_band_id  
    JOIN insurance_file I  
        ON I.insurance_file_cnt = AC.insurance_file_cnt  
    JOIN currency C  
        ON C.currency_id = I.currency_id  
    JOIN  Party P  
        ON P.Party_cnt = AC.party_cnt  
    JOIN Party_Agent PA  
        ON  PA.Party_cnt = P.Party_cnt  
    JOIN Party_Agent_Type PAT  
        ON PAT.Party_agent_Type_id = PA.Party_agent_type_id  
    LEFT JOIN Tax_Group TG  
        ON TG.tax_group_id = AC.tax_group_id  
    WHERE AC.insurance_file_cnt=@insurance_file_cnt  
    AND AC.risk_type_id IN (SELECT r.risk_type_id FROM risk r with(nolock) LEFT JOIN insurance_file_risk_link l ON r.risk_cnt=l.risk_cnt WHERE l.insurance_file_cnt=ac.insurance_file_cnt)  
END  