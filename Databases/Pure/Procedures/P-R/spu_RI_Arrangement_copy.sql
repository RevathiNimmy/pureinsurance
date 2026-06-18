SET QUOTED_IDENTIfIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_RI_Arrangement_copy'
GO
CREATE PROCEDURE spu_RI_Arrangement_copy  
    @insurance_file_cnt int,  
    @risk_cnt int,  
    @effective_date datetime,  
    @Trans_type varchar(5) = '',  
    @version_id INT=1,  
    @RI_effective_date DATETIME= NULL  
AS  
  
    Declare  
        @original_risk_cnt int,  
        @original_insurance_file_cnt int,  
        @pro_rata_rate float,  
        @new_pro_rata_rate float,  
        -- RI Arrangement Fields  
        @old_ri_arrangement_id int,  
        @new_ri_arrangement_id int,  
        @has_fac tinyint,
        @original_flag tinyint,  
        -- RI Arrangement Line Fields  
        @old_ri_arrangement_line_id int,  
        @new_ri_arrangement_line_id int,  
        -- Currency Fields  
        @original_currency_id smallint,  
        @original_source_id smallint,  
        @original_currency_rate float,  
        @original_date datetime,  
        @new_currency_id smallint,  
        @new_source_id smallint,  
        @new_currency_rate float,  
        @new_date datetime,  
        @combined_rate float,  
        @old_expiry_date datetime,  
        @new_expiry_date datetime,  
        @Date_for_Treaty_XOL_Calculation  int,  
        @old_treaty_id int,  
        @Replaced_by_treaty_id int,  
        @Replaced_by_effective_date datetime,  
        @RI2007Enabled int,  
        @old_grouping_id int,  
        @new_grouping_id int,  
        @IsTempMTA tinyint,  
		@oldFac_Premium Numeric(19, 4),  
		@old_SumInsured Numeric(19, 4),  
		@old_LineLimit Numeric(19, 4),  
		@oos_mta_cancelled_policy_id int,  
		@oos_mta_cancelled_risk_id int,  
		@oos_mta_cancelled_riarrangement_id int,  
		@this_share_percent float,  
		@has_original_fac tinyint, 
		@original_version_id int,
		@ri_pro_rata_rate float,
		@transaction_type varchar(5),
		@Cover_Start_date DATETIME,
		@Original_RI_Effective_Date DATETIME,
		@NoOfDecilamPlaces INT,
		@bIsSuppressDecimal TINYINT,
		@ri_band_id int,
        @business_type_id INT,
		@cover_start_date_ForRi DATETIME  

   SET @transaction_type = @Trans_type
      
	IF @RI_effective_date IS NULL
	SELECT @RI_effective_date = GETDATE()
            
	Select @RI2007Enabled=ISNull(value,0) from hidden_options where option_number=88  

	SELECT  @cover_start_date_ForRi = inception_date_tpi      
    FROM insurance_file  (NOLOCK)      
    WHERE Insurance_file_cnt = @Insurance_file_cnt  
    
	DECLARE @SuppressDecimalOption AS INT=112 
	SELECT @bIsSuppressDecimal=ISNULL(Value,0) from Hidden_options WHERE option_number=@SuppressDecimalOption
	
	SELECT @NoOfDecilamPlaces = CASE WHEN @bIsSuppressDecimal = 1 THEN 0 ELSE 2 END;
	        
    -- Select and validate original risk  
      
    Select  @original_risk_cnt = Null, @original_insurance_file_cnt = NULL  

    IF @Trans_type ='DRI' 
		BEGIN
				Declare  @insurance_folder_cnt int ,  @Risk_Folder_cnt int , @processed_Insurance_file_cnt int,@Insurance_file_cnt_prev int, @Insurance_file_type_id int

				SELECT @processed_Insurance_file_cnt =@Insurance_file_cnt

				SELECT	@insurance_folder_cnt = Insurance_folder_cnt,
				@Cover_start_date = Cover_start_date,@business_type_id=business_type_id,
				@Insurance_file_type_id = insurance_file_type_id
				FROM	insurance_file  (NOLOCK)
				WHERE	Insurance_file_cnt = @Insurance_file_cnt

				IF @Insurance_file_type_id = 2 
					SELECT	@Insurance_file_cnt_prev = MAX(insurance_file_cnt)
					FROM	insurance_file (NOLOCK)
					WHERE	Insurance_folder_cnt = @insurance_folder_cnt
					AND		cover_start_date <= @Cover_start_date
					AND		Insurance_file_cnt not in (@Insurance_file_cnt  ,@processed_Insurance_file_cnt)
					AND		insurance_file_type_id IN (2)
					AND  insurance_file_cnt < @processed_Insurance_file_cnt
				ELSE
					SELECT	@Insurance_file_cnt_prev = MAX(insurance_file_cnt)
					FROM	insurance_file (NOLOCK)
					WHERE	Insurance_folder_cnt = @insurance_folder_cnt
					AND		cover_start_date <= @Cover_start_date
					AND		Insurance_file_cnt not in (@Insurance_file_cnt  ,@processed_Insurance_file_cnt)
					AND		insurance_file_type_id IN (2,5,6,8,9)
					AND  insurance_file_cnt < @processed_Insurance_file_cnt

				SELECT	@Risk_Folder_cnt = Risk_Folder_cnt
				FROM	Risk(NOLOCK)
				where	risk_cnt= @Risk_cnt

				SELECT @original_risk_cnt=Insurance_file_risk_link.Risk_cnt
				FROM	Insurance_file_risk_link  (NOLOCK)
						INNER JOIN Risk (NOLOCK)
							ON Insurance_file_risk_link.risk_cnt=Risk.risk_cnt
				WHERE  insurance_file_cnt = @Insurance_file_cnt_prev  AND Risk.risk_folder_cnt=@Risk_Folder_cnt

 		END 
   ELSE
	BEGIN  
    Select  @original_risk_cnt = case @version_id  when 1 then ifrl.original_risk_cnt else ifrl.risk_cnt end  
    From    insurance_file_risk_link ifrl  
    Join    insurance_file ifi  
            On ifrl.insurance_file_cnt = ifi.insurance_file_cnt  
    Join    risk r  
            On r.risk_cnt = ifrl.risk_cnt  
    Where   ifrl.insurance_file_cnt = @insurance_file_cnt  
    And     ifrl.risk_cnt = @risk_cnt  
            -- Don't copy original ri on an mta reinstatement  
--    And     ifi.insurance_file_type_id <> 10  
            -- Don't pick up an original count where this is just a copied risk  
            -- i.e. the original version of this risk was created on this policy  
    And     Not Exists (  
            Select  NULL  
            From    insurance_file_risk_link ifrl2  
            Where   ifrl2.insurance_file_cnt = @insurance_file_cnt  
            And     ifrl2.risk_cnt = ifrl.original_risk_cnt)  
  END
    -- If we have no original risk we have nothing to copy  
    If @original_risk_cnt Is Null  
        Return  
  
    DECLARE @is_oos_reversal INT  
  
    IF @Trans_type ='DRI' AND EXISTS (select  null from mta_insurance_file_link where cancelled_linked_insurance_file_cnt=@insurance_file_cnt) BEGIN  
  SET @is_oos_reversal =1  
 END  
 ELSE  
  SET @is_oos_reversal =0  
  
 IF @Trans_type ='DRI' AND EXISTS (select  null from mta_insurance_file_link where new_linked_insurance_file_cnt=@insurance_file_cnt AND cancelled_linked_insurance_file_cnt IS NOT NULL)  
         SET @Trans_type ='MTA'  
  
 IF EXISTS (SELECT version_id from ri_arrangement where risk_cnt=@original_risk_cnt AND version_id>1)  
 BEGIN  
  Select @Cover_Start_date=cover_start_date from Insurance_File where insurance_file_cnt=@insurance_file_cnt  
  Select DISTINCT @Original_RI_Effective_Date=Effective_Date from RI_Arrangement where risk_cnt=@original_risk_cnt and version_id=2  
  If (@Cover_Start_date>=@Original_RI_Effective_Date)  
   SELECT @original_version_id = CASE  
          WHEN ( @Trans_type = 'MTCR'  
            OR ( @Trans_type = 'DRI'  
              AND @is_oos_reversal = 1 ) ) THEN @version_id  
          ELSE Max(version_id)  
           END  
   FROM   ri_arrangement  
   WHERE  risk_cnt = @original_risk_cnt  
      AND original_flag = 0  
  ELSE  
   SET @original_version_id=1  
 END  
 ELSE  
  SELECT @original_version_id = CASE  
          WHEN ( @Trans_type = 'MTCR'  
            OR ( @Trans_type = 'DRI'  
              AND @is_oos_reversal = 1 ) ) THEN @version_id  
          ELSE Max(version_id)  
           END  
  FROM   ri_arrangement  
  WHERE  risk_cnt = @original_risk_cnt  
      AND original_flag = 0  
  
    IF @original_version_id is null  SET @original_version_id=1  
  
 Select @original_insurance_file_cnt = insurance_file_cnt  
  From    insurance_file_risk_link  
  Where   risk_cnt = @original_risk_cnt  
 AND status_flag NOT IN ('U') -- must be a copied risk  
  
 Set @oos_mta_cancelled_policy_id = 0  
 Set @oos_mta_cancelled_risk_id = 0  
 IF @transaction_type <> 'DRI'
BEGIN
 IF @version_id<>2 BEGIN
    Select @oos_mta_cancelled_policy_id = ISNull(cancelled_linked_insurance_file_cnt, 0)  
     From mta_insurance_file_link  with (nolock)  
      Where new_linked_insurance_file_cnt = @insurance_file_cnt AND ISNull(cancelled_linked_insurance_file_cnt, 0) <> 0  
  
     END  
 If (@oos_mta_cancelled_policy_id > 0)  
 Begin  
  --figure out corresponding risk from version being cancelled\regenerated  
  Select @oos_mta_cancelled_risk_id = ISNull(ifrl.original_risk_cnt, 0) From insurance_file_risk_link ifrl  
   Join risk r ON r.risk_cnt = ifrl.risk_cnt  
    Where ifrl.insurance_file_cnt = @oos_mta_cancelled_policy_id  
     AND r.risk_folder_cnt = (Select risk_folder_cnt From risk Where risk_cnt = @risk_cnt)  
 End  
 END           
 If ((@oos_mta_cancelled_risk_id > 0) OR (@oos_mta_cancelled_policy_id > 0)) AND @version_id<>2  
  Begin  
   -- This is a version being re-generated; Due to interactive mode we have to be up-to-date with original RI and new RI FAC Placements  
  if not exists(select * From RI_Arrangement_line_Broker_Participants_Archive RIBrAr  
   Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id  
   Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
                 inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
                 inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
                 Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
   Begin  
   Insert Into RI_Arrangement_line_Broker_Participants_Archive  
    (ri_arrangement_line_id,  
    ri_party_cnt,  
    participation_percent)  
   SELECT ri_arrangement_line_id,  
    ri_party_cnt,  
    participation_percent FROM RI_Arrangement_line_Broker_Participants  
    WHERE ri_arrangement_line_id IN (SELECT ri_arrangement_line_id from RI_Arrangement_Line  
       where ri_arrangement_id in  
        (select ri_arrangement_id from RI_Arrangement where risk_cnt =@risk_cnt and version_id =@version_id  ))  
   End  
   Delete From RI_Arrangement_line_Broker_Participants  
     From RI_Arrangement_line_Broker_Participants RIBr  
      Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBr.ri_arrangement_line_id  
    Where ril.ri_arrangement_id IN  
       (Select ri_arrangement_id From RI_Arrangement Where risk_cnt = @risk_cnt and version_id=@version_id)  
  
  if not exists(select * From ri_arrangement_Line_Archive RIALAr  
      Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id  
      Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
                 inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
                 inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
                 Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
  Begin  
   Insert Into  RI_Arrangement_Line_Archive  
    (ri_arrangement_line_id,  
    ri_arrangement_id,  
    type,  
    treaty_id,  
    party_cnt,  
    default_share_percent,  
    this_share_percent,  
    premium_percent,  
    commission_percent,  
    agreement_code,  
    priority,  
    number_of_lines,  
    line_limit,  
    sum_insured,  
    premium_value,  
    commission_value,  
    premium_tax,  
    commission_tax,  
    is_commission_modified,  
    retained,  
    lower_limit,  
    participation_percent,  
    grouping,  
    ri_model_line_id,  
    Is_Obligatory)  
    SELECT ri_arrangement_line_id,  
    ri_arrangement_id,  
    type,  
    treaty_id,  
    party_cnt,  
    default_share_percent,  
    this_share_percent,  
    premium_percent,  
    commission_percent,  
    agreement_code,  
    priority,  
    number_of_lines,  
    line_limit,  
    sum_insured,  
    premium_value,  
    commission_value,  
    premium_tax,  
    commission_tax,  
    is_commission_modified,  
    retained,  
    lower_limit,  
    participation_percent,  
    grouping,  
    ri_model_line_id,  
    Is_Obligatory  
    FROM RI_Arrangement_Line  
     Where ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id)  
  End  
   Delete From ri_arrangement_Line  
     Where ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id)  
   Delete From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id  
  End  
 Else If @trans_type ='DRI' AND ((Exists (Select null from Insurance_File_Cloned_RI_Usage where insurance_file_cnt =@insurance_file_cnt and status =1 ) And @version_id<>2)  or @is_oos_reversal=1 )  
 BEGIN  
  if not exists(select * From RI_Arrangement_line_Broker_Participants_Archive RIBrAr  
       Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id  
       Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
                inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
                inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
                Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
  Begin  
   Insert Into RI_Arrangement_line_Broker_Participants_Archive  
     (ri_arrangement_line_id,  
     ri_party_cnt,  
     participation_percent)  
    SELECT ri_arrangement_line_id,  
     ri_party_cnt,  
     participation_percent FROM RI_Arrangement_line_Broker_Participants  
     WHERE ri_arrangement_line_id IN (SELECT ri_arrangement_line_id from RI_Arrangement_Line  
               where ri_arrangement_id in  
                 (select ri_arrangement_id from RI_Arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1))  
  End  
   Delete From RI_Arrangement_line_Broker_Participants  
     From RI_Arrangement_line_Broker_Participants RIBr  
      Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBr.ri_arrangement_line_id  
     Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1)  
  
  if not exists(select * From ri_arrangement_Line_Archive RIALAr  
                  Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id  
                  Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
               inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
               inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
               Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
  Begin  
   Insert Into  RI_Arrangement_Line_Archive  
    (ri_arrangement_line_id,  
    ri_arrangement_id,  
    type,  
    treaty_id,  
    party_cnt,  
    default_share_percent,  
    this_share_percent,  
    premium_percent,  
    commission_percent,  
    agreement_code,  
    priority,  
    number_of_lines,  
    line_limit,  
    sum_insured,  
    premium_value,  
    commission_value,  
    premium_tax,  
    commission_tax,  
    is_commission_modified,  
    retained,  
    lower_limit,  
    participation_percent,  
    grouping,  
    ri_model_line_id,  
    Is_Obligatory, manually_added)  
    SELECT ri_arrangement_line_id,  
    ri_arrangement_id,  
    type,  
    treaty_id,  
    party_cnt,  
    default_share_percent,  
    this_share_percent,  
    premium_percent,  
    commission_percent,  
    agreement_code,  
    priority,  
    number_of_lines,  
    line_limit,  
    sum_insured,  
    premium_value,  
    commission_value,  
    premium_tax,  
    commission_tax,  
    is_commission_modified,  
    retained,  
    lower_limit,  
    participation_percent,  
    grouping,  
    ri_model_line_id,  
    Is_Obligatory, manually_added  
    FROM RI_Arrangement_Line  
    Where ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1)  
  End  
   Delete From ri_arrangement_Line  
     Where ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id )  
   Delete From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id   
 END  
 Else  
  If @RI2007Enabled = 0 OR ISNULL(@RI2007Enabled,0) = 0  
    Begin  
     Delete From RI_Arrangement_line_Broker_Participants      
     From RI_Arrangement_line_Broker_Participants RIBr      
      Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBr.ri_arrangement_line_id      
     Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1)      
      
     Delete From ri_arrangement_Line      
     Where ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id  and original_flag =1)      
     Delete From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1      
     
  End  
  -- Check if we have already copied  
  If Exists (Select NULL From ri_arrangement Where risk_cnt = @risk_cnt And original_flag = 1 and version_id=@version_id)  
  Return  
    -- Get new policy details  
    Select  @new_currency_id = currency_id,  
            @new_source_id = source_id,  
            @new_currency_rate = currency_base_xrate,  
            @new_date = cover_start_date,  
          @new_expiry_date = expiry_date  
    From    insurance_file  
    Where   insurance_file_cnt = @insurance_file_cnt  
  
    -- Get original policy details  
    Select  @original_currency_id = currency_id,  
            @original_source_id = source_id,  
            @original_currency_rate = currency_base_xrate,  
            @original_date = cover_start_date,  
            @old_expiry_date = expiry_date  
    From    insurance_file  
    Where   insurance_file_cnt = (  
            Select  Max(ifi.insurance_file_cnt)  
            From    insurance_file_risk_link ifrl  
            Join    insurance_file ifi  
                    On ifi.insurance_file_cnt = ifrl.insurance_file_cnt  
            Where   ifrl.risk_cnt = @original_risk_cnt AND ifrl.status_flag NOT IN ('U'))  
  
    -- Calculate single rate to go From old currency to new currency  
    If @new_currency_id = @original_currency_id  
        -- Don't convert amounts if both policy versions are in they same currency, even if they have different rates.  
        Select @combined_rate = 1  
    Else Begin  
        -- If new rate wasn't overridden then get the rate From currencyrate table  
        If IsNull(@new_currency_rate,0) = 0  
            Execute spu_ACT_Get_Currency_Rate  
				@new_currency_id,  
				@new_source_id,  
                @new_date,  
                @new_currency_rate Output  
  
        -- If original rate wasn't overridden then get the rate From currencyrate table  
        If IsNull(@original_currency_rate,0) = 0  
            Execute spu_ACT_Get_Currency_Rate  
                @original_currency_id,  
                @original_source_id,  
                @original_date,  
                @original_currency_rate Output  
  
        Select @combined_rate = @original_currency_rate / @new_currency_rate  
    End  
  
    -- Ensure non null  
    Select @combined_rate = IsNull(@combined_rate, 1)  
  
    -- Get prorata rate  
    Execute spu_get_pro_rata_rate  
        @insurance_file_cnt = @insurance_file_cnt,  
        @risk_cnt = @risk_cnt,  
        @original_risk_cnt = @original_risk_cnt,  
        @pro_rata_rate = @pro_rata_rate output  
  
IF ISNULL(@pro_rata_rate,0) = 0 BEGIN  
    SET @pro_rata_rate = 1  
END  
  
Set @IsTempMTA = 0  
If Exists ( select NULL from insurance_file where insurance_file_cnt = @insurance_file_cnt and insurance_file_type_id in (6,7))  
Set @IsTempMTA = 1  
  
 -- Store and apply on FACs on new tab  
 Set @new_pro_rata_rate = @pro_rata_rate  
  
IF (@new_expiry_date <> @old_expiry_date) And @IsTempMTA = 0  
Begin  
     -- Get prorata rate  
    Execute spu_get_extension_pro_rata_rate  
        @insurance_file_cnt = @original_insurance_file_cnt,  
        @risk_cnt = @risk_cnt,  
        @original_risk_cnt = @original_risk_cnt,  
        @pro_rata_rate = @pro_rata_rate output  
End  
  
IF EXISTS(SELECT NULL FROM product p INNER JOIN insurance_file i ON i.product_id=p.product_id  
   WHERE ISNULL(enable_mtc_rating_rule,0)=1 AND insurance_file_type_id=12 AND insurance_file_cnt=@insurance_file_cnt)  
SELECT @pro_rata_rate=1,@new_pro_rata_rate=1  
  
    -- Reset the reinsurance flags on the risk  
    Update  r  
    Set     is_ri_at_risk_level = rt.is_ri_at_risk_level,  
            is_auto_reinsured = rt.is_auto_reinsured  
    From    risk r  
    Join    risk_type rt On rt.risk_type_id = r.risk_type_id  
    Where   r.risk_cnt = @risk_cnt  
  -- Set original premium when insurance_file_status_id = 6
IF @Insurance_file_type_id = 6 AND @Trans_type = 'DRI'
BEGIN
    SELECT @pro_rata_rate = pro_rata_rate
    FROM Risk
    WHERE risk_cnt = (
        SELECT risk_cnt 
        FROM insurance_file_risk_link (NOLOCK)
        WHERE insurance_file_cnt = (
            SELECT TOP 1 insurance_file_cnt
            FROM Insurance_File (NOLOCK)
            WHERE insurance_folder_cnt = @insurance_folder_cnt AND insurance_file_type_id = 6 AND insurance_file_status_id = 5
        )
    );
END;
    -- Arrangement cursor  
    Declare Arrangement_Cursor Cursor Fast_Forward For  
        Select  ri_arrangement_id,  
                Case When Exists (  
                    Select  NULL  
                    From    ri_arrangement_line ral  
                    Where   (ral.type IN ('F','FX') OR ISNULL(ral.manually_added,0) = 1)
                    And     ral.ri_arrangement_id = ra.ri_arrangement_id) Then 1 Else 0 End has_fac,
					1 original_flag,  
					@original_risk_cnt,
					ri_band_id
        From    ri_arrangement ra  
        Where   risk_cnt = @original_risk_cnt  
        And    (original_flag = 0 Or ( Not Exists (SELECT NULL from RI_Arrangement where risk_cnt=@original_risk_cnt and ri_band_id=ra.ri_band_id and original_flag=0) And  (@Trans_type = 'MTCR' OR @is_oos_reversal =1) ))  
        And     ISNULL(version_id, 1) =  @original_version_id  
 
    Open Arrangement_Cursor  
    Fetch Next From Arrangement_Cursor Into @old_ri_arrangement_id, @has_fac,@original_flag, @original_risk_cnt,@ri_band_id  
  
 If (@@Fetch_Status = -1 AND (@Trans_type = 'MTCR' OR @is_oos_reversal =1 ))  
  BEGIN  
   -- Check if deleted risk is being reinstated then still need to pull fac  
   Select  @old_ri_arrangement_id = ri_arrangement_id, @original_flag = 0  
   From    ri_arrangement ra  
   Where   risk_cnt = @original_risk_cnt  
   And     original_flag = 1  
   AND    version_id = (select MAX(version_id) from ri_arrangement where risk_cnt = @original_risk_cnt  
   And     original_flag = 1)  
  END  
  
    -- For each of the old arrangements  
    While (@@Fetch_Status = 0) Or (@original_flag = 0 AND @trans_type <> 'DRI') Begin  
        -- If this is the original flag then combined_rate is negative  
  
        If NOT (@trans_type ='DRI' AND Exists (Select null from Insurance_File_Cloned_RI_Usage where insurance_file_cnt =@insurance_file_cnt and status =1 ) AND @original_flag =0)  
        BEGIN  
  
        If @original_flag = 1  
			Select  @combined_rate = -Abs(@combined_rate)  
        Else  
            Select  @combined_rate = Abs(@combined_rate)  
  
        -- Copy arrangement  
        Insert Into ri_arrangement (  
                risk_cnt,  
                ri_band_id,  
                ri_model_id,  
                sum_insured,  
                premium,  
                original_flag,  
                is_modified,  
                extended_limit_amount,  
                is_extended_limit_applied,  
                prop_calc_method_id ,  
                xol_calc_method_id,  
                version_id,  
                RI_Version_Type_id,  
                Effective_Date,xol_ri_model_id,  
				ri_override_reason_id)  
        Select  @risk_cnt,  
                ri_band_id,  
                ri_model_id,  
                Case When @original_flag = 0 Then 0 Else Round(sum_insured * @combined_rate, @NoOfDecilamPlaces) End,  
                Case When @original_flag = 0 Then 0 Else Round(premium * @combined_rate, @NoOfDecilamPlaces) End,  
                @original_flag,  
                Case When @original_flag = 0 Then 0 Else is_modified End,  
                extended_limit_amount,  
    is_extended_limit_applied,  
    prop_calc_method_id ,  
    xol_calc_method_id, @version_id,  
    Case When @trans_type ='PT' Then 2 Else Case When @trans_type ='DRI' THEN RI_Version_Type_id Else 1 End END,  
                Case When @trans_type ='PT' Then @RI_Effective_Date Else @effective_date End,  
    xol_ri_model_id ,  
 -- PBI 35359: Copy ri_override_reason_id for both current (original_flag=0) and original snapshot (original_flag=1)
 -- so the override reason is preserved on MTA and displayed on the reinsurance screen
 isnull(ri_override_reason_id, 0)  
        From    ri_arrangement  
        Where   risk_cnt = @original_risk_cnt  
        And     ri_arrangement_id = @old_ri_arrangement_id  
        And     ((original_flag = 0) Or ( (@Trans_type = 'MTCR' OR @is_oos_reversal =1) AND original_flag = 1))  
  
        -- Get new id  
        Select  @new_ri_arrangement_id = @@Identity  
  
         UPDATE ra SET Cloned =1 FROM RI_Arrangement ra join RI_Model rm  
   ON (ra.ri_model_id=rm.ri_model_id or ra.xol_ri_model_id=rm.ri_model_id) WHERE ri_model_type=4 and ra.ri_arrangement_id=@new_ri_arrangement_id  
  
 if not exists(select * From ri_arrangement_Line_Archive RIALAr  
                  Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id  
                  Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
               inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
               inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
               Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
 Begin  
  Insert Into  RI_Arrangement_Line_Archive  
   (ri_arrangement_line_id,  
   ri_arrangement_id,  
   type,  
   treaty_id,  
   party_cnt,  
   default_share_percent,  
   this_share_percent,  
   premium_percent, 
   commission_percent,  
   agreement_code,  
   priority,  
   number_of_lines,  
   line_limit,  
   sum_insured,  
   premium_value,  
   commission_value,  
   premium_tax,  
   commission_tax,  
   is_commission_modified,  
   retained,  
   lower_limit,  
   participation_percent,  
   grouping,  
   ri_model_line_id,  
   Is_Obligatory, manually_added)  
   SELECT ri_arrangement_line_id,  
   ri_arrangement_id,  
   type,  
   treaty_id,  
   party_cnt,  
   default_share_percent,  
   this_share_percent,  
   premium_percent,  
   commission_percent,  
   agreement_code,  
   priority,  
   number_of_lines,  
   line_limit,  
   sum_insured,  
   premium_value,  
   commission_value,  
   premium_tax,  
   commission_tax,  
   is_commission_modified,  
   retained,  
   lower_limit,  
   participation_percent,  
   grouping,  
   ri_model_line_id,  
   Is_Obligatory , manually_added 
   FROM RI_Arrangement_Line  
   Where ri_arrangement_id =@new_ri_arrangement_id  
    END  
   Delete from ri_arrangement_line where ri_arrangement_id=@new_ri_arrangement_id  
  
   SELECT top 1 @Date_for_Treaty_XOL_Calculation=Date_for_Treaty_XOL_Calculation_id      
   FROM RI_Band_Version     
   WHERE ri_band_id=(Select ri_band_id From ri_arrangement Where ri_arrangement_id = @old_ri_arrangement_id)      
   AND CONVERT(DATE, effective_date, 23 )<= CONVERT(DATE, @cover_start_date_ForRi, 23)  
   ORDER BY effective_date DESC  
  
   If @Date_for_Treaty_XOL_Calculation = 2  
   Select @effective_date=system_base_date from insurance_file where insurance_file_cnt=@insurance_file_cnt  
  
SET @new_grouping_id = NULL  
        -- Arrangement Line cursor  
        Declare Arrangement_Line_Cursor Cursor Fast_Forward For  
            Select  ri_arrangement_line_id,grouping  
            From    ri_arrangement_line  
            Where   ri_arrangement_id = @old_ri_arrangement_id  
            Order by grouping, ri_arrangement_line_id  
  
        Open Arrangement_Line_Cursor  
        Fetch Next From Arrangement_Line_Cursor Into @old_ri_arrangement_line_id,@old_grouping_id  
  
        -- For each of the old arrangements  
        While (@@Fetch_Status = 0) Begin  
        -- Copy arrangement line first  
  
      If Exists(Select NULL from ri_arrangement_line  Where ri_arrangement_line_id=@old_ri_arrangement_line_id  And type = 'TX') 
	  And @Date_for_Treaty_XOL_Calculation = 2 And @RI2007Enabled=1  
      Begin  
			Select @old_treaty_id=Treaty_id from ri_arrangement_line  
            Where  ri_arrangement_line_id=@old_ri_arrangement_line_id  
          Select @Replaced_by_treaty_id = ISNULL(Replaced_by_treaty_id,0),  
            @Replaced_by_effective_date = Replaced_by_effective_date  
            From Treaty  
            Where Treaty_id=@old_treaty_id  
         if (@Replaced_by_treaty_id<>0 And @Replaced_by_effective_date <= @effective_date)  
                   Insert Into ri_arrangement_line (  
                           ri_arrangement_id,  
                           type,  
                           treaty_id,  
                           party_cnt,  
                           default_share_percent,  
                           this_share_percent,  
                           premium_percent,  
                           commission_percent,  
                           agreement_code,  
                           priority,  
					       number_of_lines,  
                           line_limit,  
                           lower_limit,  
                           sum_insured,  
                           premium_value,  
                           commission_value,  
                           premium_tax,  
                           commission_tax,  
                           is_commission_modified,  
                           Retained,  
						   is_obligatory,grouping,manually_added,is_edited,is_premium_edited)  
                   Select  @new_ri_arrangement_id,  
                           type,  
                           @Replaced_by_treaty_id,  
                           party_cnt,  
                           default_share_percent,  
                           this_share_percent,  
                           premium_percent,  
                           commission_percent,  
                           agreement_code,  
                           priority,  
						number_of_lines,  
                           Round(line_limit * Abs(@combined_rate), @NoOfDecilamPlaces),  
                           Round(lower_limit * Abs(@combined_rate), @NoOfDecilamPlaces),  
                           Round(sum_insured * @combined_rate, @NoOfDecilamPlaces),  
                           Round(premium_value * @combined_rate * @pro_rata_rate, @NoOfDecilamPlaces),  
						  Round(commission_value * @combined_rate * @pro_rata_rate, 4),  
                           Case When @original_flag = 0 Then 0 Else Round(premium_tax * @combined_rate * @pro_rata_rate, @NoOfDecilamPlaces) End,  
                           Case When @original_flag = 0 Then 0 Else Round(commission_tax * @combined_rate * @pro_rata_rate, 4) End,  
                        is_commission_modified,Retained,  
                     is_obligatory,grouping,ISNULL(manually_added,0),
                     -- PBI 35359: carry is_edited for current arrangement; reset to 0 for original snapshot
                     ISNULL(is_edited, 0),
                     ISNULL(is_premium_edited, 0)
                   From    ri_arrangement_line  
                   Where   ri_arrangement_line_id = @old_ri_arrangement_line_id  
                   And    (@original_flag = 1 Or type = 'F' or ISNULL(manually_added,0) = 1)  
  
         Else  
                   Insert Into ri_arrangement_line (  
                           ri_arrangement_id,  
                           type,  
                           treaty_id,  
                           party_cnt,  
                           default_share_percent,  
                           this_share_percent,  
                           premium_percent,  
                           commission_percent,  
                           agreement_code,  
                           priority,  
                           number_of_lines,  
                           line_limit,  
                           lower_limit,  
           sum_insured,  
                           premium_value,  
                           commission_value,  
                           premium_tax,  
                           commission_tax,  
                           is_commission_modified,  
                         Retained,  
         is_obligatory,grouping,manually_added,is_edited,is_premium_edited)  
                   Select  @new_ri_arrangement_id,  
                           type,  
                           treaty_id,  
                           party_cnt,  
                           default_share_percent,  
                           this_share_percent,  
                           premium_percent,  
                           commission_percent,  
                           agreement_code,  
                           priority,  
                           number_of_lines,  
                           Round(line_limit * Abs(@combined_rate), @NoOfDecilamPlaces),  
                           Round(lower_limit * Abs(@combined_rate), @NoOfDecilamPlaces),  
             Round(sum_insured * @combined_rate, @NoOfDecilamPlaces),  
                           Round(premium_value * @combined_rate * @pro_rata_rate, @NoOfDecilamPlaces),  
                           Round(commission_value * @combined_rate * @pro_rata_rate, 4),  
                           Case When @original_flag = 0 Then 0 Else Round(premium_tax * @combined_rate * @pro_rata_rate,@NoOfDecilamPlaces) End,  
                           Case When @original_flag = 0 Then 0 Else Round(commission_tax * @combined_rate * @pro_rata_rate, 4) End,  
                           is_commission_modified,Retained,  
         is_obligatory,grouping,ISNULL(manually_added,0),
                     -- PBI 35359: carry is_edited for current arrangement; reset to 0 for original snapshot
                     ISNULL(is_edited, 0),
                     ISNULL(is_premium_edited, 0)
                   From    ri_arrangement_line  
                   Where   ri_arrangement_line_id = @old_ri_arrangement_line_id  
                   And    (@original_flag = 1 Or type = 'F' or ISNULL(manually_added,0) = 1)  
          End  
  
     ELSE  
    Insert Into ri_arrangement_line (  
                      ri_arrangement_id,  
                      type,  
                      treaty_id,  
                      party_cnt,  
                      default_share_percent,  
                      this_share_percent,  
                      premium_percent,  
                      commission_percent,  
                      agreement_code,  
                      priority,  
                      number_of_lines,  
                      line_limit,  
                      lower_limit,  
                      sum_insured,  
                      premium_value,  
                      commission_value,  
                      premium_tax,  
                      commission_tax,  
                      is_commission_modified,  
                      Retained,participation_percent,grouping,  
       is_obligatory,manually_added,FACPropPremiumPerc,is_edited,is_premium_edited)  
              Select  @new_ri_arrangement_id,  
       type,  
                      treaty_id,  
                      party_cnt,  
                      default_share_percent,  
                      this_share_percent,  
                      premium_percent,  
                      commission_percent,  
                      agreement_code,  
       priority,  
                      number_of_lines,  
                      Round(line_limit * Abs(@combined_rate), @NoOfDecilamPlaces),  
                      Round(lower_limit * Abs(@combined_rate), @NoOfDecilamPlaces),  
                      -- PBI 35359: Preserve sum_insured as-is for user-edited lines (is_edited=1)
                      Case When (@original_flag = 0 And ISNULL(is_edited,0) = 1) Then sum_insured
                           Else Round(sum_insured * @combined_rate, @NoOfDecilamPlaces) End,  
       Case When (@original_flag = 0 and Rtrim(@trans_type) ='MTC') Then  
       0 Else  
        Case When (@original_flag = 0 and @oos_mta_cancelled_risk_id > 0) Then  
        premium_value Else  
         -- PBI 35359: Preserve premium as-is for user-edited lines (is_edited=1)
         Case When (@original_flag = 0 and ISNULL(is_edited,0) = 1) Then premium_value Else
         Case When (@original_flag = 0 and ISNULL(manually_added,0) = 1) Then Round(premium_value * @combined_rate * @new_pro_rata_rate, @NoOfDecilamPlaces) Else
         Case When (@original_flag = 0) Then (premium_value * @combined_rate * @new_pro_rata_rate)  
                Else (premium_value * @combined_rate) End End End End End,  
       Case When (@original_flag = 0 and Rtrim(@trans_type) ='MTC') Then  
       0 Else  
        Case When (@original_flag = 0 and @oos_mta_cancelled_risk_id > 0) Then  
        commission_value Else  
         -- PBI 35359: Preserve commission as-is for user-edited lines (is_edited=1)
         Case When (@original_flag = 0 and ISNULL(is_edited,0) = 1) Then commission_value Else
         Case When (@original_flag = 0 and ISNULL(manually_added,0) = 1) Then Round(commission_value * @combined_rate * @new_pro_rata_rate, @NoOfDecilamPlaces) Else
         Case When (@original_flag = 0) Then Round(commission_value * @combined_rate * @new_pro_rata_rate, @NoOfDecilamPlaces)  
                Else Round(commission_value * @combined_rate, 4) End End End End End,  
                      Case When @original_flag = 0 Then 0 Else Round(premium_tax * @combined_rate, @NoOfDecilamPlaces) End,  
                      Case When @original_flag = 0 Then 0 Else Round(commission_tax * @combined_rate, 4) End,  
                      is_commission_modified,Retained,participation_percent,grouping,  
       is_obligatory,ISNULL(manually_added,0),FACPropPremiumPerc,
       -- PBI 35359: Carry forward is_edited so calc proc preserves edited values
       -- Do NOT carry is_edited for FAC (F/FX) on original snapshot - FAC must be recalculated by calc proc
       CASE WHEN @original_flag = 1 AND type IN ('F','FX') THEN 0 ELSE ISNULL(is_edited, 0) END,
       ISNULL(is_premium_edited, 0)  
              From    ri_arrangement_line  ril  
        Where   ri_arrangement_line_id = @old_ri_arrangement_line_id  
                And    (@original_flag = 1 Or ISNULL(manually_added,0) = 1 OR    type IN ('F','FX')  Or (type = 'T' and ISNULL(line_limit,0) = 0 ) Or (@original_flag = 0 And ISNULL(is_edited,0) = 1) )    --If T manually added then it does not have line_limit; also carry forward user-edited lines  
  
      -- Get new id  
   Declare @RowEffected int  
   Select @RowEffected = @@RowCount  
   IF @RowEffected > 0  
            Select  @new_ri_arrangement_line_id = @@Identity  
   ELSE  
    Select @new_ri_arrangement_line_id = 0  
  
  -- Initialise with Default value  
 Set @has_original_fac = 1  
  
  If @Trans_type = 'MTR' AND @original_flag = 0  
  BEGIN  
   Select @oldFac_Premium = premium_value * -1 From ri_arrangement_line ril  
   Inner Join ri_arrangement ria  
    ON ria.ri_arrangement_id = ril.ri_arrangement_id  
   Where risk_cnt = @original_risk_cnt and original_flag = 1 and version_id=@original_version_id  
   AND type = 'FX' AND lower_limit = (Select lower_limit From ri_arrangement_line Where ri_arrangement_line_id = @old_ri_arrangement_line_id)  
   AND Party_Cnt = (Select party_cnt From ri_arrangement_line Where ri_arrangement_line_id = @old_ri_arrangement_line_id)  
  
   --Update from original line of cancelled version  
 If ISNULL(@oldFac_Premium, 0) <> 0  
  Update ri_arrangement_line Set premium_value = @oldFac_Premium  
     Where ri_arrangement_line_id = @new_ri_arrangement_line_id  
  
  END  
 ELSE IF (@Trans_type = 'MTCR' OR @is_oos_reversal =1) AND @original_flag = 0 AND @new_ri_arrangement_line_id > 0  
 BEGIN  
  
  Select @oldFac_Premium = premium_value * -1, @old_LineLimit = RIL.line_limit,  
    @Old_SumInsured = RIL.sum_insured * -1,  
      @this_share_percent = RIL.this_share_percent  
   From ri_arrangement_line ril  
   Inner Join ri_arrangement ria ON ria.ri_arrangement_id = ril.ri_arrangement_id  
    Where risk_cnt = @original_risk_cnt and original_flag = 1 AND type = 'FX'  and version_id=@original_version_id  
     AND lower_limit = (Select lower_limit From ri_arrangement_line Where ri_arrangement_line_id = @old_ri_arrangement_line_id)  
     AND Party_Cnt = (Select party_cnt From ri_arrangement_line Where ri_arrangement_line_id = @old_ri_arrangement_line_id)  
  
  --Update from original line of cancelled version  
  If ISNULL(@oldFac_Premium, 0) <> 0 OR ISNULL(@Old_SumInsured, 0) <> 0  
   Update ri_arrangement_line Set premium_value = @oldFac_Premium, line_limit = @old_LineLimit,  
         sum_insured = @Old_SumInsured, this_share_percent = @this_share_percent  
   Where ri_arrangement_line_id = @new_ri_arrangement_line_id   AND type = 'FX'  
  ELSE  
 Begin  
  
  if not exists(select * From RI_Arrangement_line_Broker_Participants_Archive RIBrAr  
       Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id  
       Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
                inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
                inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
                Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
 Begin  
    Insert Into RI_Arrangement_line_Broker_Participants_Archive  
     (ri_arrangement_line_id,  
     ri_party_cnt,  
     participation_percent)  
    SELECT RIBrAr.ri_arrangement_line_id,  
    RIBrAr.ri_party_cnt,  
    RIBrAr.participation_percent FROM RI_Arrangement_line_Broker_Participants RIBrAr  
    Inner Join RI_Arrangement_Line  ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id  
    WHERE RIBrAr.ri_arrangement_line_id =@new_ri_arrangement_line_id  AND ril.type = 'FX'  
   End  
    Delete RI_Arrangement_line_Broker_Participants  
     From RI_Arrangement_line_Broker_Participants RIBr  
     Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBr.ri_arrangement_line_id  
      Where ril.ri_arrangement_line_id = @new_ri_arrangement_line_id AND type = 'FX'  
    Delete ri_arrangement_line Where ri_arrangement_line_id = @new_ri_arrangement_line_id  AND type = 'FX'  
  if not exists(select * From ri_arrangement_Line_Archive RIALAr  
                  Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id  
                  Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
               inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
               inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
               Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
   Begin  
    Insert Into  RI_Arrangement_Line_Archive  
     (ri_arrangement_line_id,  
     ri_arrangement_id,  
     type,  
     treaty_id,  
     party_cnt,  
     default_share_percent,  
     this_share_percent,  
     premium_percent,  
     commission_percent,  
     agreement_code,  
     priority,  
     number_of_lines,  
     line_limit,  
     sum_insured,  
     premium_value,  
     commission_value,  
     premium_tax,  
     commission_tax,  
     is_commission_modified,  
     retained,  
     lower_limit,  
     participation_percent,  
     grouping,  
     ri_model_line_id,  
     Is_Obligatory)  
     SELECT ri_arrangement_line_id,  
     ri_arrangement_id,  
     type,  
     treaty_id,  
     party_cnt,  
     default_share_percent,  
     this_share_percent,  
     premium_percent,  
     commission_percent,  
     agreement_code,  
     priority,  
     number_of_lines,  
     line_limit,  
     sum_insured,  
     premium_value,  
     commission_value,  
     premium_tax,  
     commission_tax,  
     is_commission_modified,  
     retained,  
     lower_limit,  
     participation_percent,  
     grouping,  
     ri_model_line_id,  
     Is_Obligatory  
     FROM RI_Arrangement_Line  
     Where ri_arrangement_line_id = @new_ri_arrangement_line_id  AND type = 'FX'  
   End  
   Delete ri_arrangement_line Where ri_arrangement_line_id = @new_ri_arrangement_line_id  AND type = 'FX'  
 End  
 Set @oldFac_Premium = 0  
 Set @Old_SumInsured = 0  
  
 Select @oldFac_Premium = premium_value * -1, @old_LineLimit = RIL.premium_percent,  
  @Old_SumInsured = RIL.sum_insured * -1, @this_share_percent = RIL.this_share_percent  
 From ri_arrangement_line ril  
  Inner Join ri_arrangement ria ON ria.ri_arrangement_id = ril.ri_arrangement_id  
   Where risk_cnt = @original_risk_cnt and original_flag = 1 AND type = 'F' and version_id=@original_version_id  
    AND Party_Cnt = (Select party_cnt From ri_arrangement_line Where ri_arrangement_line_id = @old_ri_arrangement_line_id)  
    AND priority = (Select priority From ri_arrangement_line Where ri_arrangement_line_id = @old_ri_arrangement_line_id)  
  
       --Update from original line of cancelled version or remove if nothing is there  
       If ISNULL(@oldFac_Premium, 0) <> 0 OR ISNULL(@Old_SumInsured, 0) <> 0  
   Begin  
    Update ri_arrangement_line Set premium_value = @oldFac_Premium, premium_percent = @old_LineLimit,  
           this_share_percent = @this_share_percent, sum_insured = @Old_SumInsured  
     Where ri_arrangement_line_id = @new_ri_arrangement_line_id   AND type = 'F'  
   End  
    Else  
   Begin  
    if not exists(select * From RI_Arrangement_line_Broker_Participants_Archive RIBrAr  
         Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id  
               Where ril.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
                inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
                inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
                Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
    Begin  
     Insert Into RI_Arrangement_line_Broker_Participants_Archive  
       (ri_arrangement_line_id,  
       ri_party_cnt,  
       participation_percent)  
       SELECT RIBrAr.ri_arrangement_line_id,  
       RIBrAr.ri_party_cnt,  
       RIBrAr.participation_percent FROM RI_Arrangement_line_Broker_Participants RIBrAr  
       Inner Join RI_Arrangement_Line  ril ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id  
       Where ril.ri_arrangement_line_id = @new_ri_arrangement_line_id AND type = 'F'  
    End  
    Delete RI_Arrangement_line_Broker_Participants  
     From RI_Arrangement_line_Broker_Participants RIBr  
     Inner Join ri_arrangement_line ril ON ril.ri_arrangement_line_id = RIBr.ri_arrangement_line_id  
      Where ril.ri_arrangement_line_id = @new_ri_arrangement_line_id AND type = 'F'  
    if not exists(select * From ri_arrangement_Line_Archive RIALAr  
         Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id  
         Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI  
               inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt  
               inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt  
               Where ri.risk_cnt = @risk_cnt and ri.version_id=@version_id and ifl.insurance_file_type_id in (2,5,8,9)))  
     Begin  
      Insert Into  RI_Arrangement_Line_Archive  
       (ri_arrangement_line_id,  
       ri_arrangement_id,  
       type,  
       treaty_id,  
       party_cnt,  
       default_share_percent,  
       this_share_percent,  
       premium_percent,  
       commission_percent,  
       agreement_code,  
       priority,  
       number_of_lines,  
       line_limit,  
       sum_insured,  
       premium_value,  
       commission_value,  
       premium_tax,  
       commission_tax,  
       is_commission_modified,  
       retained,  
       lower_limit,  
       participation_percent,  
       grouping,  
       ri_model_line_id,  
       Is_Obligatory)  
       SELECT ri_arrangement_line_id,  
       ri_arrangement_id,  
       type,  
       treaty_id,  
       party_cnt,  
       default_share_percent,  
       this_share_percent,  
       premium_percent,  
       commission_percent,  
       agreement_code,  
       priority,  
       number_of_lines,  
       line_limit,  
       sum_insured,  
       premium_value,  
       commission_value,  
       premium_tax,  
       commission_tax,  
       is_commission_modified,  
       retained,  
       lower_limit,  
       participation_percent,  
       grouping,  
       ri_model_line_id,  
       Is_Obligatory  
       FROM RI_Arrangement_Line  
       Where ri_arrangement_line_id = @new_ri_arrangement_line_id  AND type = 'F'  
     End  
     Delete ri_arrangement_line Where ri_arrangement_line_id = @new_ri_arrangement_line_id  AND type = 'F'  
     Set @has_original_fac = 0  
   End  
 End  
   IF EXISTS (Select NULL From Ri_Arrangement_line_Broker_Participants Where ri_arrangement_line_id=@old_ri_arrangement_line_id) AND @has_original_fac = 1  
   BEGIN  
         DECLARE Brokers_FAC CURSOR FOR  
           Select ri_party_cnt,participation_percent From  
            Ri_Arrangement_line_Broker_Participants  
            Where ri_arrangement_line_id=@old_ri_arrangement_line_id  
  
     DECLARE @Party_Cnt INT,  
       @Part_percent FLOAT  
  
     OPEN Brokers_FAC  
  
        FETCH NEXT FROM Brokers_FAC  
          INTO @Party_Cnt,@Part_percent  
  
           WHILE @@FETCH_STATUS = 0  
           BEGIN  
            Insert Into Ri_Arrangement_line_Broker_Participants  
      (ri_arrangement_line_id,  
               ri_party_cnt,  
               participation_percent)  
            Values  
      (@new_ri_arrangement_line_id,  
               @Party_Cnt,  
               @Part_percent)  
  
       FETCH NEXT FROM Brokers_FAC  
             INTO @Party_Cnt,@Part_percent  
           END  
           CLOSE Brokers_FAC  
           DEALLOCATE Brokers_FAC  
   END  
   IF  @old_ri_arrangement_line_id = @old_grouping_id  
     Set @new_grouping_id = @new_ri_arrangement_line_id  
  
   IF @old_grouping_id IS NOT NULL  
    Update ri_arrangement_line set grouping = @new_grouping_id  
        Where ri_arrangement_line_id = @new_ri_arrangement_line_id  
   ELSE  
    SET @new_grouping_id = NULL  
  
            -- Copy taxes (only for originals)  
            If @original_flag = 1 Begin  
                Insert Into tax_calculation (  
                        risk_cnt,  
                        tax_band_id,  
                        premium,  
                        percentage,  
                        value,  
                        is_value,  
                        is_manually_changed,  
                        calc_basis,  
                        basis_value,  
                        sum_insured,  
                        sum_insured_rounded,  
                        allow_tax_credit,  
                        original_sum_insured,  
                        currency_id,  
                        country_id,  
                        state_id,  
                        class_of_business_id,  
                        tax_group_id,  
                        sequence,  
        insurance_file_cnt,  
                        transtype,  
                        ri_party_cnt,  
                        ri_arrangement_line_id)  
                Select  @risk_cnt,  
                        tax_band_id,  
                        Case  When transtype='TTRIFP' OR transtype='TTRIFC' Then 
							Round(premium * @combined_rate, @NoOfDecilamPlaces)
						Else
						    Round(-premium * @combined_rate, @NoOfDecilamPlaces)
						END,
                        percentage,  
						Case  When transtype='TTRIFP' OR transtype='TTRIFC' Then 
							 Round(value * @combined_rate, @NoOfDecilamPlaces)
						Else
						     Round(-value * @combined_rate, @NoOfDecilamPlaces)
						END,            
                        is_value,  
                        is_manually_changed,  
                        calc_basis,  
                        Round(-basis_value * @combined_rate, @NoOfDecilamPlaces),  
                        Round(-sum_insured * @combined_rate, @NoOfDecilamPlaces),  
                        sum_insured_rounded,  
                        allow_tax_credit,  
                        Round(-original_sum_insured * @combined_rate, @NoOfDecilamPlaces),  
                        @new_currency_id,  
                        country_id,  
                        state_id,  
                        class_of_business_id,  
                        tax_group_id,  
                        sequence,  
                        @insurance_file_cnt,  
                        transtype,  
                        ri_party_cnt,  
                        @new_ri_arrangement_line_id  
                From    tax_calculation  
                Where   risk_cnt = @original_risk_cnt  
                And     ri_arrangement_line_id = @old_ri_arrangement_line_id  
            End Else Begin  
                -- We should recalc all taxes, just to be safe  
                -- Note, this will also refresh the premium & comm shares  
                -- just in case the si/premium ratio has changed  
                Execute spu_RI_Arrangement_taxes  
      @insurance_file_cnt = @insurance_file_cnt,  
                 @risk_cnt = @risk_cnt,  
                    @ri_arrangement_id = @new_ri_arrangement_id,  
                    @band_premium = 0  
            End  
  
            -- Get next arrangement line  
            Fetch Next From Arrangement_Line_Cursor Into @old_ri_arrangement_line_id,@old_grouping_id  
        End  
  
        -- Close and release cursor  
        Close Arrangement_Line_Cursor  
        Deallocate Arrangement_Line_Cursor  
  
  If ((@oos_mta_cancelled_risk_id > 0 OR (@Trans_type = 'MTCR' OR @is_oos_reversal =1) ) AND @original_flag = 1)  
  Begin  
   Set @oos_mta_cancelled_riarrangement_id = 0  
   -- check fac and place it  
   If (@Trans_type = 'MTCR'  OR @is_oos_reversal =1)  
    -- fetch ri arrangement to pull FAC from  
    Select @oos_mta_cancelled_riarrangement_id = ri_arrangement_id From ri_arrangement  
     Where risk_cnt = @original_risk_cnt and original_flag = 1  
      AND ri_band_id = (Select ri_band_id From ri_arrangement Where ri_arrangement_id = @old_ri_arrangement_id)  
   Else  
    -- fetch ri arrangement to pull FAC from  
    Select @oos_mta_cancelled_riarrangement_id = ri_arrangement_id From ri_arrangement  
     Where risk_cnt = @oos_mta_cancelled_risk_id and original_flag = 0  
      AND ri_band_id = (Select ri_band_id From ri_arrangement Where ri_arrangement_id = @old_ri_arrangement_id)  
  
   If Exists (Select NULL From ri_arrangement_line ral  
       Where (ral.type IN ('F','FX')  or isnull(ral.manually_added,0) = 1 )
        And ral.ri_arrangement_id = @oos_mta_cancelled_riarrangement_id) AND  @transaction_type<>'DRI'  
    Begin  
     Set @has_fac = 1  
     Set @old_ri_arrangement_id = @oos_mta_cancelled_riarrangement_id  
     If @oos_mta_cancelled_risk_id > 0  
      Set @original_risk_cnt = @oos_mta_cancelled_risk_id  
 End  
 ELSE IF EXISTS (Select NULL From ri_arrangement_line ral  
       Where (ral.type IN ('F','FX')  or isnull(manually_added,0) = 1 )
        AND ral.ri_arrangement_id = @old_ri_arrangement_id )  
    Begin  
     SET @has_fac = 1  
    End  
   Else  
    SET @has_fac = 0  
  End  
  End  

        -- If we have fac we need to copy it to new arrangements  
        If @original_flag = 1 And @has_fac = 1 Begin  
            -- Set original flag to 0 and reprocess arrangement for fac  
            Select @original_flag = 0  
			If @Trans_type = 'DRI'
				Fetch Next From Arrangement_Cursor Into @old_ri_arrangement_id, @has_fac, @original_flag, @original_risk_cnt, @ri_band_id 
        End Else Begin  
            -- If this is the last record we'll go into an infinite loop if  
            -- we don't manually reset the original_flag  
            Select @original_flag = 1  
		/*this is to cater for disabled pro rata option for output tables*/
		if @RI2007Enabled=1 AND @pro_rata_rate=1 AND @Trans_type in ('MTA','MTC','MTR') AND @business_type_id NOT IN(3,4)
		BEGIN
			DECLARE @band_original_premium Numeric(19, 4)
			SELECT @band_original_premium =sum(this_premium) from peril where risk_cnt=@risk_cnt and ri_band=@ri_band_id and rating_section_id in
			(select rating_section_id from Rating_Section where risk_cnt=@risk_cnt and original_flag=1)
			UPDATE RI_Arrangement set premium=@band_original_premium WHERE ri_arrangement_id=@new_ri_arrangement_id
		END

            -- Get next arrangement  
            Fetch Next From Arrangement_Cursor Into @old_ri_arrangement_id, @has_fac, @original_flag, @original_risk_cnt, @ri_band_id  
        End  
    End  
  
    -- Close and release cursor  
    Close Arrangement_Cursor  
    Deallocate Arrangement_Cursor    

-- PBI 35359: Reset is_edited for accounting year treaty lines on portfolio transfer
IF @Trans_type = 'PT'
BEGIN
    UPDATE ral
    SET    ral.is_edited = 0
    FROM   ri_arrangement_line ral
    JOIN   ri_arrangement ra ON ra.ri_arrangement_id = ral.ri_arrangement_id
    WHERE  ra.risk_cnt = @risk_cnt
    AND    ra.version_id = @version_id
    AND    ra.Cloned = 1
END


GO
