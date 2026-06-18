--Below Scripts are part of Generic Utility
-- Tables:
--1. DataFixUtility_log
--2. peril_DataFix
--3. Rating_Section_DataFix
--Stored Procedures :
--1. spu_DataFixUtility_log_add
--2. spu_CopyReversalTransdetail
--3. spu_RI_Arrangement_refresh_RI2007_ForUtility
--4. spu_add_stats_folder_reverse
--5. spu_add_stats_details_Reverse
--6. spu_CopyReversalDocument
--7. spu_update_TransDetail_Comment
--8. spu_Copy_Stats_Reversal_Claim_By_Document_Ref
--9. spu_CLM_Finalise_stats_Reversal
--10. spu_allocate_transactions_by_docref
--11. spu_claim_stats_reverse
--12. spu_CopyReversalTransExportDetail
--13. spu_reverse_SRP
--14. spu_ACT_Sel_TransDetail_By_Doc_FACAlloc_DataFix
--15. spu_Claim_Recalculate_Reinsurance
--16. spu_ICCS_6129_Add_Missing_Task
--17. spu_Update_TaxCalculation_DataFix
--18. spu_update_insurance_file_type_datafix
--19. spu_Update_Insurance_File_Premium_DataFix
--20. spu_ValidateRisk_DataFix
--21. spu_InsertRatingSectionPeril_DataFix
--22. spu_sir_rating_section_peril_DataFix
--23. spu_select_Insurance_FileCnt_DataFix
--24. spu_add_stats_folder_Datafix
--25. spu_CLM_Get_Transaction_Code
--26. spu_Copy_Stats_for_Cloned_Reversal_DataFix
--27. spu_Copy_TransExportDetail_for_Cloned_Reversal_DataFix
--28. Spu_ri_arrangement_copy_ForUtility
--29. Spu_claim_recalculate_reinsurance_NOT2007 
--30. spu_RI_Arrangement_make_RI2007_ForUtility
--31. spu_CLM_Finalise_stats_For_Utility
--32 spu_Copy_Reinsurance_Details_To_Claim_Utility
--33 spu_Refresh_Tax_DataFix
--***************************************************************************************************************
IF Isnull(Objectproperty(Object_id('dbo.DataFixUtility_log'), 'IsTable'), 0) = 0
  BEGIN
      CREATE TABLE [dbo].[DataFixUtility_log]
        (
           [DataFixUtility_log_id]      [INT] IDENTITY(1, 1) NOT NULL,
           [PMNumber]                   [VARCHAR](50) NULL,
           [Created_by]                 [VARCHAR](50) NULL,
           [insurance_file_cnt]         [INT] NULL,
           [old_document_id]            [INT] NULL,
           [new_document_id]            [INT] NULL,
           [created_date]               [DATETIME] NULL,
           [comment]                    [VARCHAR](100) NULL,
           [Claim_id]                   [INT] NULL,
           [AllocationID]               [INT] NULL,
           [BordereauReference]         [NVARCHAR](100) NULL,
           [DepositNumber]              [NVARCHAR](100) NULL,
           [DomainAaccount]             [NVARCHAR](100) NULL,
           [ApplicationLoggedInAccount] [NVARCHAR](100) NULL
        )
      ON [PRIMARY]
  END

GO

EXEC Ddladdcolumn
  'DataFixUtility_log',
  'BordereauReference',
  'NVARCHAR(100)'

GO

EXEC Ddladdcolumn
  'DataFixUtility_log',
  'DepositNumber',
  'NVARCHAR(100)'

GO

EXEC Ddladdcolumn
  'DataFixUtility_log',
  'DomainAaccount',
  'NVARCHAR(100)'

GO

EXEC Ddladdcolumn
  'DataFixUtility_log',
  'ApplicationLoggedInAccount',
  'NVARCHAR(100)'

GO

EXEC Ddladdcolumn
  'DataFixUtility_log',
  'AllocationID',
  'INT'

GO

--******************************************************************************************************************

IF Isnull(Objectproperty(Object_id('dbo.peril_DataFix'), 'IsTable'), 0) = 0
BEGIN

CREATE TABLE dbo.peril_DataFix(
	risk_cnt int NOT NULL,
	rating_section_id int NOT NULL,
	peril_id int NOT NULL,
	peril_type_id int NOT NULL,
	class_of_business_id int NOT NULL,
	sequence_number int NOT NULL,
	description varchar(30) NULL,
	sum_insured numeric(21, 6) NOT NULL,
	rating_sum_insured numeric(21, 6) NOT NULL,
	rate_type_id int NULL,
	annual_rate numeric(21, 6) NULL,
	annual_premium numeric(21, 6) NOT NULL,
	this_premium numeric(21, 6) NOT NULL,
	coinsured_this_premium numeric(21, 6) NULL,
	coinsured_sum_insured numeric(21, 6) NULL,
	coinsured_commission numeric(21, 6) NULL,
	retained_this_premium numeric(21, 6) NULL,
	retained_sum_insured numeric(21, 6) NULL,
	lead_commission_band int NULL,
	sub_commission_band int NULL,
	lead_commission_value numeric(21, 6) NULL,
	sub_commission_value numeric(21, 6) NULL,
	tax_group int NULL,
	tax_value numeric(21, 6) NULL,
	ri_band int NULL,
	xl_band tinyint NULL,
	is_premium tinyint NULL,
	is_sum_insured tinyint NULL,
	is_levy_tax tinyint NULL,
	is_taxed tinyint NULL
	)

END

GO

--******************************************************************************************************************


IF Isnull(Objectproperty(Object_id('dbo.Rating_Section_DataFix'), 'IsTable'), 0) = 0
BEGIN



CREATE TABLE dbo.Rating_Section_DataFix(
	risk_cnt int NOT NULL,
	rating_section_id int NOT NULL,
	rating_section_type_id int NOT NULL,
	policy_section_type_id int NULL,
	sequence_number int NOT NULL,
	description varchar(30) NULL,
	rate_type_id int NULL,
	annual_rate numeric(21, 6) NULL,
	sum_insured numeric(19, 4) NULL,
	annual_premium numeric(19, 4) NULL,
	this_premium numeric(19, 4) NULL,
	original_flag tinyint NULL,
	currency_id smallint NULL,
	country_id int NULL,
	state_id int NULL,
	this_discount numeric(19, 4) NULL,
	applied_discount numeric(19, 4) NULL,
	adjusted_discount numeric(4, 4) NULL,
	is_amended tinyint NULL,
	calculated_premium money NULL,
	override_reason varchar(255) NULL,
	discount_original_this_premium money NULL,
	auto_calculated tinyint NULL,
	Earning_Pattern_id int NOT NULL
	)

END

GO

--******************************************************************************************************************
EXEC Ddldropprocedure
  'spu_DataFixUtility_log_add'

GO

CREATE PROCEDURE Spu_datafixutility_log_add @PMNumber                   VARCHAR(50) = '',
                                            @Created_by                 VARCHAR(50),
                                            @insurance_file_cnt         INT=0,
                                            @old_document_ref           VARCHAR(20) = '',
                                            @new_document_id            INT = 0,
                                            @is_reversal                BIT=0,
                                            @Claim_id                   INT=0,
                                            @BordereauReference         NVARCHAR(100) = '',
                                            @DepositNumber              NVARCHAR(100) = '',
                                            @DomainAaccount             NVARCHAR(100) = '',
                                            @ApplicationLoggedInAccount NVARCHAR(100) = '',
                                            @AllocationID               INT = 0,
                                            @AssociatedDocRef           NVARCHAR(20) =  '',
											@IsOnlyGenerate             BIT = 0
AS
  BEGIN
      DECLARE @old_document_id INT,
              @comment         VARCHAR(100),
			  @newDocRef VARCHAR(20)

      SELECT @old_document_id = document_id
      FROM   document
      WHERE  document_ref = @old_document_ref

      IF @is_reversal = 1
        SELECT @comment = 'Reversal of document_ref - '
                          + @old_document_ref
      ELSE
        SELECT @comment = 'Reposting of document_ref -'
                          + @old_document_ref

      IF @AllocationID > 0
        SELECT @comment = @old_document_ref
                          + ' has been reversed and now allocated to '
                          + @AssociatedDocRef

      IF Isnull(@new_document_id, 0) = 0
        BEGIN
            IF @insurance_file_cnt = 0
              BEGIN
                  SELECT TOP 1 @new_document_id = D.document_id,
                               @insurance_file_cnt = D.insurance_file_cnt
                  FROM   Stats_Folder SF
                         INNER JOIN Document D
                                 ON SF.document_ref = D.document_ref
                  WHERE  loss_id = @Claim_id
                  ORDER  BY stats_folder_cnt DESC
              END
            ELSE
              SELECT @new_document_id = Max(document_id)
              FROM   document
              WHERE  insurance_file_cnt = @insurance_file_cnt
        END

      IF @claim_id = 0
        SET @claim_id = NULL

	IF @IsOnlyGenerate = 1 
 BEGIN
select @newDocRef = document_ref from Document where document_id = @new_document_id
SELECT @comment='Posting of document_ref - ' +  @newDocRef 
END

      INSERT INTO DataFixUtility_log
                  (PMNumber,
                   Created_by,
                   insurance_file_cnt,
                   old_document_id,
                   new_document_id,
                   created_date,
                   comment,
                   Claim_id,
                   BordereauReference,
                   DepositNumber,
                   DomainAaccount,
                   ApplicationLoggedInAccount,
                   AllocationID)
      VALUES      ( @PMNumber,
                    @Created_by,
                    @insurance_file_cnt,
                    @old_document_id,
                    @new_document_id,
                    Getdate(),
                    @comment,
                    @Claim_id,
                    @BordereauReference,
                    @DepositNumber,
                    @DomainAaccount,
                    @ApplicationLoggedInAccount,
                    @AllocationID )
  END

GO

--***************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_CopyReversalTransdetail'

GO

CREATE PROCEDURE Spu_copyreversaltransdetail @nNewDocumentId  INT,
                                             @sOldDocumentRef VARCHAR(25),
                                             @user_id         INT
AS
    DECLARE @posting_period_number INT

    SELECT @posting_period_number = posting_period_number
    FROM   Stats_Folder sd
           JOIN Document d
             ON sd.document_ref = d.document_ref
    WHERE  d.document_id = @nNewDocumentId

    INSERT INTO transdetail
                (account_id,
                 postingstatus_id,
                 company_id,
                 sub_branch_id,
                 currency_id,
                 period_id,
                 document_id,
                 document_sequence,
                 accounting_date,
                 amount,
                 base_amount_unrounded,
                 fully_matched,
                 currency_amount,
                 currency_amount_unrounded,
                 currency_base_xrate,
                 euro_currency_id,
                 euro_amount,
                 euro_base_xrate,
                 euro_ccy_xrate,
                 comment,
                 insurance_ref,
                 operator_id,
                 purchase_order_no,
                 purchase_invoice_no,
                 department,
                 spare,
                 ref_date,
                 ref_amount,
                 ref_quantity,
                 ref_units,
                 insurance_ref_index,
                 department_id,
                 not_reported,
                 underwriting_year_id,
                 amount_currency_id,
                 account_currency_id,
                 account_amount,
                 account_amount_unrounded,
                 account_base_xrate,
                 system_currency_id,
                 system_amount,
                 system_amount_unrounded,
                 system_base_xrate,
                 outstanding_amount,
                 outstanding_currency_amount,
                 outstanding_account_amount,
                 outstanding_system_amount,
                 amount_updated,
                 reference,
                 type_code,
                 transdetail_type_id,
                 tax_group_id,
                 tax_band_id,
                 batch_id,
				 fee_type)
    SELECT account_id,
           postingstatus_id,
           company_id,
           sub_branch_id,
           currency_id,
           @posting_period_number,
           @nNewDocumentId,
           document_sequence,
           Getdate(),
           -1 * amount,
           -1 * base_amount_unrounded,
           0,
           -1 * currency_amount,
           -1 * currency_amount_unrounded,
           currency_base_xrate,
           euro_currency_id,
           euro_amount,
           euro_base_xrate,
           euro_ccy_xrate,
           '',
           insurance_ref,
           @user_id,
           purchase_order_no,
           purchase_invoice_no,
           department,
           spare,
           ref_date,
           ref_amount,
           ref_quantity,
           ref_units,
           insurance_ref_index,
           department_id,
           not_reported,
           underwriting_year_id,
           amount_currency_id,
           account_currency_id,
           -1 * account_amount,
           -1 * account_amount_unrounded,
           account_base_xrate,
           system_currency_id,
           -1 * system_amount,
           -1 * system_amount_unrounded,
           system_base_xrate,
           -1 * amount,
           -1 * currency_amount,
           -1 * account_amount,
           -1 * system_amount,
           amount_updated,
           reference,
           type_code,
           transdetail_type_id,
           tax_group_id,
           tax_band_id,
           batch_id,
		   fee_type
    FROM   transdetail
    WHERE  document_id = (SELECT document_id
                          FROM   document
                          WHERE  document_ref = @sOldDocumentRef)

GO

--*****************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_RI_Arrangement_refresh_RI2007_ForUtility'

GO

CREATE PROCEDURE Spu_ri_arrangement_refresh_ri2007_forutility @insurance_file_cnt        INTEGER,
                                                              @risk_cnt                  INTEGER,
                                                              @Trans_type                VARCHAR(5) = '',
                                                              @TMPRisk_cnt_under_renewal INTEGER = NULL,
                                                              @copy_fac_risk_cnt         INTEGER = NULL,
                                                              @version_id                INT = 1,
                                                              @transferdate              DATETIME = NULL
AS
    DECLARE @effective_date                             DATETIME,
            @is_coinsured                               TINYINT,
            @retained_percent                           FLOAT,
            @source_id                                  INT,
            @policy_currency_id                         SMALLINT,
            @policy_currency_rate                       FLOAT,
            @is_auto_reinsured                          TINYINT,
            @risk_type_id                               INT,
            @eml_percent                                FLOAT,
            @allow_deferred                             TINYINT,
            @line_limit                                 MONEY,
            @ri_band                                    INT,
            @premium                                    MONEY,
            @sum_insured                                MONEY,
            @ri_arrangement_id                          INT,
            @ri_arrangement_line_id                     INT,
            @is_modified                                TINYINT,
            @Date_for_Treaty_XOL_Calculation            INT,
            @Transaction_type_id                        INT,
            @RI_sum_insured                             FLOAT,
            @RI_premium                                 FLOAT,
            @RIDetails_Changed                          INT,
            @Original_Risk_cnt                          INTEGER,
            @Recalculate_prorata_reinsurance_during_MTA TINYINT,
            @treaty_id                                  INT,
            @party_cnt                                  INT,
            @default_share_percent                      FLOAT,
            @obligatory_sum_insured                     MONEY,--PN 71440
            @NBExtended_Is_Enabled                      TINYINT,
            @prop_effective_date                        DATETIME,
            @Date_for_Prop_Calculation                  INT,
            @Extended_Limits_Enabled                    TINYINT,
            @original_flag                              INT,
            @is_ON_Accounting_Year                      INT,
            @new_copy_fac_risk_cnt                      INT,
            @original_ri_arrangement_id                 INT,
            @original_sum_insured                       MONEY,
            @original_premium                           MONEY,
            @original_ri_model_id                       INT,
            @Original_Extended_Limits_Amount            MONEY,
            @Original_Extended_Limits_Enabled           TINYINT,
            @RI_Effective_Date                          DATETIME,
            @RI_Version_Type_id                         INT = 1,
            @Limit_Effective_Date                       DATETIME,
            @pro_rata_rate                              FLOAT,
            @old_pro_rata_rate                          FLOAT,
            @temp_prop_Effective_Date                   DATETIME,
            @temp_XOL_Effective_Date                    DATETIME,
            @DocumentDate                               DATETIME,
            @is_PT_DRI_Amended                          BIT,
            @Extended_Limits_EnabledSystemOption        TINYINT

    SET @new_copy_fac_risk_cnt =@copy_fac_risk_cnt

    -- Don't use the supplied effective date.
    -- Note: For an MTA this call may return an MTA date or today's date
    -- depending on the system option "Use MTA date for reinsurance"
    EXECUTE Spu_get_effective_date
      @insurance_file_cnt = @insurance_file_cnt,
      @risk_cnt = @risk_cnt,
      @effective_date = @effective_date OUTPUT,
      @prop_effective_date = @prop_effective_date OUTPUT

    SELECT @Extended_Limits_EnabledSystemOption = Isnull(VALUE, 0)
    FROM   system_options
    WHERE  option_number = 5260
           AND branch_id = 1

    IF @TransferDate IS NULL
      SET @TransferDate = CONVERT(DATE, Getdate())

    SET @Limit_Effective_Date = @effective_date

    DECLARE @option INT

    -- Check "Use MTA Date for Reinsurance"    
    SELECT @option = CONVERT(INT, value)
    FROM   system_options
    WHERE  branch_id = 1
           AND option_number = 1023

    IF @Trans_type IN ( 'DRI', 'PT' )
       AND @version_id = 1
      BEGIN
          SELECT @version_id = Max(version_id)
          FROM   RI_Arrangement
          WHERE  risk_cnt = @risk_cnt
                 AND original_flag = 0

          IF @version_id > 1
            BEGIN
                SET @is_PT_DRI_Amended = 1

                SELECT @pro_rata_rate = pro_rata_rate
                FROM   RI_Arrangement
                WHERE  risk_cnt = @risk_cnt
                       AND original_flag = 0
                       AND version_id = @version_id
            END
          ELSE
            SET @version_id=1
      END

    IF @Trans_type = 'NB'
        OR @Trans_type = 'REN'
      SELECT @option = 0

    --SET @prop_effective_date=@effective_date    
    -- Check coinsurance and rate
    EXECUTE Spu_insurance_file_is_coinsured
      @insurance_file_cnt,
      @is_coinsured OUTPUT,
      @retained_percent OUTPUT

    -- Get currency of policy, and therefore the currency of new ri_arrangement
    SELECT @policy_currency_id = currency_id,
           @policy_currency_rate = currency_base_xrate,
           @source_id = source_id
    FROM   insurance_file
    WHERE  insurance_file_cnt = @insurance_file_cnt

    -- If policy rate wasn't overridden then get the rate from currencyrate table
    IF Isnull(@policy_currency_rate, 0) = 0
      EXECUTE Spu_act_get_currency_rate
        @policy_currency_id,
        @source_id,
        @effective_date,
        @policy_currency_rate OUTPUT

    -- Get config values from risk and risk_type
    SELECT @is_auto_reinsured = rt.is_auto_reinsured,
           @risk_type_id = rt.risk_type_id,
           @eml_percent = Isnull(r.eml_percentage, 100) / 100,
           @allow_deferred = rt.is_deferred_ri_permitted
    FROM   risk r
           JOIN risk_type rt
             ON r.risk_type_id = rt.risk_type_id
    WHERE  r.risk_cnt = @risk_cnt

    -- Copy any original reinsurance
    IF EXISTS(SELECT NULL FROM RI_Arrangement_Line RIAL 
				INNER JOIN RI_Arrangement RI 
					ON RIAL.Ri_Arrangement_id = RI.Ri_Arrangement_id 
				AND RI.risk_cnt =@risk_cnt 
				AND type IN('F','FX'))
	BEGIN
		EXECUTE Spu_ri_arrangement_copy_ForUtility
		  @insurance_file_cnt = @insurance_file_cnt,
		  @risk_cnt = @risk_cnt,
		  @effective_date = @effective_date,
		  @Trans_type = @Trans_type,
		  @version_id = @version_id,
		  @RI_Effective_Date = @TransferDate
	END
	ELSE
	BEGIN
    EXECUTE Spu_ri_arrangement_copy
      @insurance_file_cnt = @insurance_file_cnt,
      @risk_cnt = @risk_cnt,
      @effective_date = @effective_date,
      @Trans_type = @Trans_type,
      @version_id = @version_id,
      @RI_Effective_Date = @TransferDate
	END

    IF @Trans_type = 'NB'
        OR @Trans_type = 'REN'
      SELECT @Extended_Limits_Enabled = Isnull(VALUE, 0)
      FROM   system_options
      WHERE  option_number = 5260
             AND branch_id = 1

    IF @version_id > 1
      BEGIN
          SELECT @RI_Effective_Date = @transferdate,
                 @RI_Version_Type_id = 2

          SET @Limit_Effective_Date = @transferdate
      END
    ELSE
      BEGIN
          SELECT @RI_Effective_Date = @effective_date
      END

    -- Declare active ri_band cursor and get premiums
    DECLARE ri_band_cursor CURSOR FAST_FORWARD FOR
      SELECT p.ri_band,
             sum_insured = Isnull((SELECT Sum(rs2.sum_insured)
                                   FROM   rating_section rs2
                                   WHERE  rs2.risk_cnt = @risk_cnt
                                          AND rs2.rating_section_id IN (SELECT rating_section_id
                                                                        FROM   peril p2
                                                                        WHERE  p2.risk_cnt = @risk_cnt
                                                                               AND p2.is_sum_insured = 1
                                                                               AND p2.ri_band = p.ri_band)
                                          AND rs2.original_flag = rs.original_flag), 0),
             premium = Isnull(Sum(CASE
                                    WHEN p.is_premium = 1 THEN p.this_premium
                                    ELSE 0
                                  END), 0),
             rs.original_flag
      FROM   peril p WITH (nolock)
             JOIN rating_section rs WITH (nolock)
               ON rs.risk_cnt = p.risk_cnt
                  AND rs.rating_section_id = p.rating_section_id
             LEFT JOIN peril_type pt
                    ON p.peril_type_id = pt.peril_type_id
      -- PN:61917 : Added By Upendra : The Premimum should not reflect on RI-Insurance Screen when the Levy Tax is selected.
      WHERE  p.risk_cnt = @risk_cnt
             AND ( p.is_premium = 1
                    OR p.is_sum_insured = 1 )
             AND Isnull(pt.is_levy_tax, 0) = 0
             AND ( rs.original_flag = 0
                    OR @version_id = 1 )
      -- PN:61917 : Added By Upendra : The Premimum should not reflect on RI-Insurance Screen when the Levy Tax is selected.
      GROUP  BY p.ri_band,
                rs.original_flag

    -- Open the RI Bands Cursor and get first row
    OPEN ri_band_cursor

    FETCH NEXT FROM ri_band_cursor INTO @ri_band, @sum_insured, @premium, @original_flag

    -- Start processing
    WHILE ( @@FETCH_STATUS = 0 )
      BEGIN
          -- Apply the EML Percentage and Coinsurance
          SELECT @sum_insured = @sum_insured * @retained_percent * @eml_percent,
                 @premium = @premium * @retained_percent

          -- Check for existing arrangement
          SELECT @ri_arrangement_id = NULL,
                 @temp_prop_Effective_Date = @prop_effective_date,
                 @temp_XOL_Effective_Date = @effective_date

          SELECT @ri_arrangement_id = ri_arrangement_id,
                 @is_modified = Isnull(is_modified, 1),
                 @Date_for_Treaty_XOL_Calculation = xol_calc_method_id,
                 @Date_for_Prop_Calculation = prop_calc_method_id
          FROM   ri_arrangement
          WHERE  risk_cnt = @risk_cnt
                 AND ri_band_id = @ri_band
                 AND original_flag = @original_flag
                 AND version_id = @version_id

          -- Execute the SP to update premium percent in accordance to
          -- Premium distributed
          EXEC Spu_ri_arrangement_line_premiumpercent_upd
            @ri_arrangement_id= @ri_arrangement_id

          -- Either insert or update our arrangement
          SET @RIDetails_Changed=0

          SELECT @Date_for_Treaty_XOL_Calculation = Isnull(@Date_for_Treaty_XOL_Calculation, date_for_treaty_xol_calculation_id),
                 @Date_for_Prop_Calculation = Isnull(@Date_for_Prop_Calculation, Proportional_RI_Cal_Method)
          FROM   ri_band
          WHERE  ri_band_id = @ri_band

          IF @Date_for_Prop_Calculation = 2
             AND @option <> 1
            BEGIN
                SET @is_ON_Accounting_Year =1

                --SELECT @temp_prop_Effective_Date = CASE WHEN cover_start_date> @TransferDate  THEN cover_start_date ELSE @TransferDate  END    
                --FROM   insurance_file WHERE  insurance_file_cnt = @insurance_file_cnt    
                SELECT TOP 1 @temp_prop_Effective_Date = Cast(cast(document_date as date) as datetime)
                FROM   Document
                WHERE  insurance_file_cnt = @insurance_file_cnt
                       AND ( document_ref LIKE 'I%'
                              OR document_ref LIKE 'S%' )
                ORDER  BY document_date ASC

                IF @original_flag <> 1
                  BEGIN
                      SET @Limit_Effective_Date = @temp_prop_Effective_Date
                  END
            END

          -- Check for ri_limits from DM    
          -- Note: This call returns an single row of Null
          IF @is_auto_reinsured = 1
            BEGIN
                IF @original_flag = 1
                  BEGIN
                      SELECT @Original_Risk_cnt = original_risk_cnt
                      FROM   insurance_file_risk_link
                      WHERE  insurance_file_cnt = @insurance_file_cnt
                             AND risk_cnt = @risk_cnt

                      IF @Extended_Limits_EnabledSystemOption = 1
                        BEGIN
                            EXECUTE Spu_get_ri_values
                              @insurance_file_cnt = @insurance_file_cnt,
                              @risk_cnt = @Original_Risk_cnt,
                              @effective_date = @Limit_Effective_Date,
                              @value = @line_limit OUTPUT
                        END

                      IF @Trans_type = 'DRI'
                         AND EXISTS(SELECT NULL
                                    FROM   RI_Arrangement
                                    WHERE  ri_band_id = @ri_band
                                           AND risk_cnt = @risk_cnt
                                           AND version_id = @version_id
                                    GROUP  BY ri_model_id
                                    HAVING Count(ri_arrangement_id) > 1)
                        SET @is_ON_Accounting_Year = 1
                  END
                ELSE IF @Extended_Limits_EnabledSystemOption = 1
                  BEGIN
                      EXECUTE Spu_get_ri_values
                        @insurance_file_cnt = @insurance_file_cnt,
                        @risk_cnt = @risk_cnt,
                        @effective_date = @Limit_Effective_Date,
                        @value = @line_limit OUTPUT
                  END
            END

          IF Isnull(@ri_arrangement_id, 0) > 0
            BEGIN
                -- Update totals on current arrangement
                SELECT @RI_sum_insured = sum_insured,
                       @RI_premium = premium
                FROM   ri_arrangement
                WHERE  ri_arrangement_id = @ri_arrangement_id

                IF @original_flag = 1
                  SET @sum_insured = @RI_sum_insured

                IF @is_modified = 1
                   AND @original_flag <> 1
                  BEGIN
                      UPDATE ri_arrangement
                      SET    extended_limit_amount = @line_limit
                      WHERE  ri_arrangement_id = @ri_arrangement_id

                      IF @Extended_Limits_Enabled = 1
                         AND Isnull(@line_limit, 0) > 0
                         AND @original_flag <> 1
                        UPDATE RI_Arrangement_Line
                        SET    line_limit = Isnull(@line_limit, 0)
                        WHERE  type IN ( 'TFS', 'T', 'R' )
                               AND ri_arrangement_id = @ri_arrangement_id
                  END

                IF @RI_sum_insured <> @sum_insured
                    OR @RI_premium <> @premium
                    OR @Trans_type = 'DRI'
                  SET @RIDetails_Changed=1

                IF @original_flag <> 1
                  UPDATE ri_arrangement
                  SET    sum_insured = @sum_insured,
                         premium = @premium
                  WHERE  ri_arrangement_id = @ri_arrangement_id
            END
          ELSE
            BEGIN
                -- Insert new arrangement
                INSERT INTO ri_arrangement
                            (risk_cnt,
                             ri_band_id,
                             sum_insured,
                             premium,
                             original_flag,
                             is_modified,
                             version_id,
                             RI_Version_Type_id,
                             Effective_Date)
                VALUES      (@risk_cnt,
                             @ri_band,
                             @sum_insured,
                             @premium,
                             @original_flag,
                             1^@is_auto_reinsured,
                             @version_id,
                             @RI_Version_Type_id,
                             @RI_Effective_Date)

                -- Get new id and assume we have not modified
                SELECT @ri_arrangement_id = @@IDENTITY,
                       @is_modified = 0
            END

          IF @Trans_type = 'MTA'
              OR @Trans_type = 'MTC'
              OR @Trans_type = 'MTR'
              OR @Trans_type = 'MTCR'
              OR @Trans_type IN ( 'PT', 'DRI' )
            BEGIN
                IF EXISTS(SELECT NULL
                          FROM   insurance_file ifl1
                                 JOIN insurance_file ifl2
                                   ON ifl1.insurance_folder_cnt = ifl2.insurance_folder_cnt
                                 JOIN insurance_file_risk_link ifrl
                                   ON ifl1.insurance_file_cnt = ifrl.insurance_file_cnt
                                 JOIN ri_arrangement ra
                                   ON ifrl.risk_cnt = ra.risk_cnt
                          WHERE  ifl2.insurance_file_cnt = @insurance_file_cnt
                                 AND ifl1.insurance_file_type_id = 2
                                 AND ra.is_extended_limit_applied = 1)
                  SELECT @NBExtended_Is_Enabled = 1
                ELSE
                  SELECT @NBExtended_Is_Enabled = 0
            END

          IF @Date_for_Treaty_XOL_Calculation = 2
             AND @option <> 1 --AND @Trans_type NOT IN ('PT')    
            BEGIN
                SELECT @temp_XOL_Effective_Date = @TransferDate
                FROM   insurance_file
                WHERE  insurance_file_cnt = @insurance_file_cnt
            END
          ELSE IF @Date_for_Treaty_XOL_Calculation = 3
             AND @option <> 1 --AND  @Trans_type <> 'PT'    
            BEGIN
                SET @is_ON_Accounting_Year =1

                SELECT TOP 1 @DocumentDate = document_date
                FROM   Document
                WHERE  insurance_file_cnt = @insurance_file_cnt
                       AND ( document_ref LIKE 'I%'
                              OR document_ref LIKE 'S%' )
                ORDER  BY document_date ASC

                SELECT @temp_XOL_Effective_Date = CASE
                                                    WHEN cover_start_date > @DocumentDate THEN cover_start_date
                                                    ELSE @DocumentDate
                                                  END
                FROM   insurance_file
                WHERE  insurance_file_cnt = @insurance_file_cnt
            END

          -- Check if the band has been modified
          IF ( @is_modified = 0 )
              OR @Trans_type = 'DRI'
            BEGIN
                IF @Trans_type = 'REN'
                   AND @TMPRisk_cnt_under_renewal IS NOT NULL
                  BEGIN
                      EXECUTE Spu_ri_arrangement_copy_tmpfac
                        @TMPRisk_cnt_under_renewal,
                        @ri_arrangement_id
                  END

                IF ( @new_copy_fac_risk_cnt IS NOT NULL )
                  BEGIN
                      EXECUTE Spu_ri_arrangement_copy_tmpfac
                        @new_copy_fac_risk_cnt,
                        @ri_arrangement_id
                  END

                -- It hasn't, or it's new so refresh it
                EXECUTE spu_RI_Arrangement_make_RI2007_ForUtility
                  @ri_arrangement_id = @ri_arrangement_id,
                  @risk_type_id = @risk_type_id,
                  @ri_band_id = @ri_band,
                  @effective_date = @temp_XOL_Effective_Date,
                  @allow_deferred = @allow_deferred,
                  @sum_insured = @sum_insured,
                  @premium = @premium,
                  @line_limit = @line_limit,
                  @is_auto_reinsured = @is_auto_reinsured,
                  @source_id = @source_id,
                  @policy_currency_id = @policy_currency_id,
                  @policy_currency_rate = @policy_currency_rate,
                  @Trans_type =@Trans_type,
                  @NBExtended_Is_Enabled = @NBExtended_Is_Enabled,
                  @prop_effective_date=@temp_prop_Effective_Date,
                  @Original_Risk_Cnt = @Original_Risk_cnt
            END
          ELSE IF @RIDetails_Changed = 1
            BEGIN
                DECLARE @RI_Model_id INT

                EXECUTE Spu_getrimodel
                  @Effective_Date = @temp_XOL_Effective_Date,
                  @risk_type_id = @risk_type_id,
                  @ri_band_id =@ri_band,
                  @allow_deferred = @allow_deferred,
                  @RI_Model_id =@RI_Model_id OUTPUT

                EXECUTE Spu_ri_arrangement_calc_ri2007
                  @ri_arrangement_id = @ri_arrangement_id,
                  @band_si = @sum_insured,
                  @band_premium = @premium,
                  @ri_model_id = @ri_model_id,
                  @Trans_type = @Trans_type
            END

          IF @version_id > 1
            BEGIN
                SELECT @original_ri_arrangement_id = RI_Arrangement_id,
                       @original_sum_insured = sum_insured,
                       @original_premium = premium,
                       @original_ri_model_id = ri_model_id,
                       @Original_Extended_Limits_Enabled = Is_extended_limit_applied,
                       @Original_Extended_Limits_Amount = Extended_limit_amount
                FROM   RI_Arrangement
                WHERE  risk_cnt = @risk_cnt
                       AND version_id = @version_id
                       AND original_flag = 1
                       AND ri_band_id = @ri_band

                EXECUTE Spu_ri_arrangement_calc_ri2007
                  @ri_arrangement_id = @original_ri_arrangement_id,
                  @band_si = @original_sum_insured,
                  @band_premium = @original_premium,
                  @ri_model_id = @original_ri_model_id,
                  @Trans_type = @Trans_type,
                  @Extended_Limits_Enabled = @Original_Extended_Limits_Enabled,
                  @Extended_Limits_Amount = @Original_Extended_Limits_Amount
            END

          --ReCalc this_share_percent & premium_percent
          --Gaurav
          --Start PN 71440
          SELECT @obligatory_sum_insured = @sum_insured

          IF EXISTS(SELECT Count(*)
                    FROM   ri_arrangement_line
                    WHERE  is_obligatory = 1
                           AND ri_arrangement_id = @ri_arrangement_id)
            BEGIN
                UPDATE ri_arrangement_line
                SET    this_share_percent = CASE
                                              WHEN ( CONVERT(FLOAT, @sum_insured) = 0
                                                      OR sum_insured = 0 ) THEN
                                                CASE
                                                  WHEN premium_value = 0 THEN 0
                                                  ELSE default_share_percent
                                                END
                                              ELSE ( sum_insured / CONVERT(FLOAT, @sum_insured) ) * 100.0000
                                            END
                WHERE  ri_arrangement_id = @ri_arrangement_id
                       AND is_obligatory = 1

                SELECT @obligatory_sum_insured = @obligatory_sum_insured - sum_insured
                FROM   ri_arrangement_line
                WHERE  ri_arrangement_id = @ri_arrangement_id
                       AND is_obligatory = 1
            END

      /*
        UPDATE ri_arrangement_line
           SET this_share_percent = CASE WHEN (convert(float, @obligatory_sum_insured) =0 or sum_insured=0)
                                     THEN CASE WHEN premium_value=0 then 0 else default_share_percent end
                           else (sum_insured / convert(float, @obligatory_sum_insured)) * 100.0000 END--,
      --        premium_percent = (premium_value / convert(float, @premium)) * 100.0000
                 WHERE ri_arrangement_id = @ri_arrangement_id AND Is_Obligatory=0
      */
          --End PN 71440
          -- We should recalc all taxes, just to be safe
          -- Note, this will also refresh the premium & comm shares
          -- just in case the si/premium ratio has changed
          EXECUTE Spu_ri_arrangement_taxes
            @insurance_file_cnt = @insurance_file_cnt,
            @risk_cnt = @risk_cnt,
            @ri_arrangement_id = @ri_arrangement_id,
            @band_premium = @premium

          SELECT @Date_for_Prop_Calculation = NULL,
                 @Date_for_Treaty_XOL_Calculation = NULL

          -- Get next record
          FETCH NEXT FROM ri_band_cursor INTO @ri_band, @sum_insured, @premium, @original_flag
      END

    -- Close the cursor
    CLOSE ri_band_cursor

    DEALLOCATE ri_band_cursor

    IF Isnull(@is_PT_DRI_Amended, 0) = 1
      BEGIN
          SELECT @old_pro_rata_rate = Isnull(pro_rata_rate, 1)
          FROM   risk
          WHERE  risk_cnt = @risk_cnt

          UPDATE RI_Arrangement
          SET    premium = @pro_rata_rate * premium / @old_pro_rata_rate,
                 pro_rata_rate = @pro_rata_rate
          WHERE  risk_cnt = @risk_cnt
                 AND version_id = @version_id
                 AND original_flag = 0

          UPDATE RI_Arrangement_Line
          SET    premium_value = @pro_rata_rate * premium_value / @old_pro_rata_rate,
                 premium_tax = @pro_rata_rate * premium_tax / @old_pro_rata_rate,
                 commission_tax = @pro_rata_rate * commission_tax / @old_pro_rata_rate,
                 commission_value = @pro_rata_rate * commission_value / @old_pro_rata_rate
          WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id
                                       FROM   RI_Arrangement
                                       WHERE  risk_cnt = @risk_cnt
                                              AND version_id = @version_id
                                              AND original_flag = 0)

          UPDATE Tax_Calculation
          SET    premium = premium * @pro_rata_rate / @old_pro_rata_rate,
                 value = value * @pro_rata_rate / @old_pro_rata_rate
          WHERE  ri_arrangement_line_id IN (SELECT ral.ri_arrangement_line_id
                                            FROM   RI_Arrangement ra
                                                   JOIN RI_Arrangement_Line ral
                                                     ON ra.ri_arrangement_id = ral.ri_arrangement_id
                                            WHERE  risk_cnt = @risk_cnt
                                                   AND version_id = @version_id
                                                   AND original_flag = 0)
      END

    -- Delete any tax on arrangements on bands that are no longer in use
    DELETE tax_calculation
    WHERE  ri_arrangement_line_id IN (SELECT rl.ri_arrangement_line_id
                                      FROM   ri_arrangement_line rl
                                             INNER JOIN ri_arrangement ri
                                                     ON ri.ri_arrangement_id = rl.ri_arrangement_id
                                      WHERE  ri.risk_cnt = @risk_cnt
                                             AND ri.ri_band_id NOT IN (SELECT p.ri_band
                                                                       FROM   peril p
                                                                              JOIN rating_section rs
                                                                                ON rs.risk_cnt = p.risk_cnt
                                                                                   AND rs.rating_section_id = p.rating_section_id
                                                                       WHERE  p.risk_cnt = @risk_cnt
                                                                              AND ( p.is_premium = 1
                                                                                     OR p.is_sum_insured = 1 )
                                                                              AND rs.original_flag = 0)
                                             AND ri.original_flag = 0)

    -- Delete any arrangements on bands that are no longer in use
    IF NOT EXISTS(SELECT *
                  FROM   RI_Arrangement_line_Broker_Participants_Archive RIBrAr
                         INNER JOIN ri_arrangement_line ril
                                 ON ril.ri_arrangement_line_id = RIBrAr.ri_arrangement_line_id
                  WHERE  ril.ri_arrangement_id IN (SELECT ri_arrangement_id
                                                   FROM   ri_arrangement RI
                                                          INNER JOIN insurance_file_risk_link ifrl
                                                                  ON RI.risk_cnt = ifrl.risk_cnt
                                                          INNER JOIN Insurance_File IFL
                                                                  ON ifrl.insurance_file_cnt = ifl.insurance_file_cnt
                                                   WHERE  ri.risk_cnt = @risk_cnt
                                                          AND ri.version_id = @version_id
                                                          AND ifl.insurance_file_type_id IN ( 2, 5, 8, 9 )))
      BEGIN
          INSERT INTO RI_Arrangement_line_Broker_Participants_Archive
                      (ri_arrangement_line_id,
                       ri_party_cnt,
                       participation_percent)
          SELECT ri_arrangement_line_id,
                 ri_party_cnt,
                 participation_percent
          FROM   RI_Arrangement_line_Broker_Participants
          WHERE  ri_arrangement_line_id IN (SELECT ri_arrangement_line_id
                                            FROM   ri_arrangement_line
                                            WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id
                                                                         FROM   ri_arrangement
                                                                         WHERE  risk_cnt = @risk_cnt
                                                                                AND ri_band_id NOT IN (SELECT p.ri_band
                                                                                                       FROM   peril p
                                                                                                              JOIN rating_section rs
                                                                                                                ON rs.risk_cnt = p.risk_cnt
                                                                                                                   AND rs.rating_section_id = p.rating_section_id
                                                                                                       WHERE  p.risk_cnt = @risk_cnt
                                                                                                              AND ( p.is_premium = 1
                                                                                                                     OR p.is_sum_insured = 1 )
                                                                                                              AND rs.original_flag = 0)
                                                                                AND original_flag = 0))
      END

    DELETE RI_Arrangement_line_Broker_Participants
    WHERE  ri_arrangement_line_id IN (SELECT ri_arrangement_line_id
                                      FROM   ri_arrangement_line
                                      WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id
                                                                   FROM   ri_arrangement
                                                                   WHERE  risk_cnt = @risk_cnt
                                                                          AND ri_band_id NOT IN (SELECT p.ri_band
                                                                                                 FROM   peril p
                                                                                                        JOIN rating_section rs
                                                                                                          ON rs.risk_cnt = p.risk_cnt
                                                                                                             AND rs.rating_section_id = p.rating_section_id
                                                                                                 WHERE  p.risk_cnt = @risk_cnt
                                                                                                        AND ( p.is_premium = 1
                                                                                                               OR p.is_sum_insured = 1 )
                                                                                                        AND rs.original_flag = 0)
                                                                          AND original_flag = 0))

    -- Delete any arrangements on bands that are no longer in use
    IF NOT EXISTS(SELECT *
                  FROM   ri_arrangement_Line_Archive RIALAr
                         INNER JOIN ri_arrangement ria
                                 ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id
                  WHERE  ria.ri_arrangement_id IN (SELECT ri_arrangement_id
                                                   FROM   ri_arrangement RI
                                                          INNER JOIN insurance_file_risk_link ifrl
                                                                  ON RI.risk_cnt = ifrl.risk_cnt
                                                          INNER JOIN Insurance_File IFL
                                                                  ON ifrl.insurance_file_cnt = ifl.insurance_file_cnt
                                                   WHERE  ri.risk_cnt = @risk_cnt
                                                          AND ri.version_id = @version_id
                                                          AND ifl.insurance_file_type_id IN ( 2, 5, 8, 9 )))
      BEGIN
          INSERT INTO RI_Arrangement_Line_Archive
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
          FROM   RI_Arrangement_Line
          WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id
                                       FROM   ri_arrangement
                                       WHERE  risk_cnt = @risk_cnt
                                              AND ri_band_id NOT IN (SELECT p.ri_band
                                                                     FROM   peril p
                                                                            JOIN rating_section rs
                                                                              ON rs.risk_cnt = p.risk_cnt
                                                                                 AND rs.rating_section_id = p.rating_section_id
                                                                     WHERE  p.risk_cnt = @risk_cnt
                                                                            AND ( p.is_premium = 1
                                                                                   OR p.is_sum_insured = 1 )
                                                                            AND rs.original_flag = 0)
                                              AND original_flag = 0)
      END

    DELETE ri_arrangement_line
    WHERE  ri_arrangement_id IN (SELECT ri_arrangement_id
                                 FROM   ri_arrangement
                                 WHERE  risk_cnt = @risk_cnt
                                        AND ri_band_id NOT IN (SELECT p.ri_band
                                                               FROM   peril p
                                                                      JOIN rating_section rs
                                                                        ON rs.risk_cnt = p.risk_cnt
                                                                           AND rs.rating_section_id = p.rating_section_id
                                                               WHERE  p.risk_cnt = @risk_cnt
                                                                      AND ( p.is_premium = 1
                                                                             OR p.is_sum_insured = 1 )
                                                                      AND rs.original_flag = 0)
                                        AND original_flag = 0)

    DELETE ri_arrangement
    WHERE  risk_cnt = @risk_cnt
           AND ri_band_id NOT IN (SELECT p.ri_band
                                  FROM   peril p
                                         JOIN rating_section rs
                                           ON rs.risk_cnt = p.risk_cnt
                                              AND rs.rating_section_id = p.rating_section_id
                                  WHERE  p.risk_cnt = @risk_cnt
                                         AND ( p.is_premium = 1
                                                OR p.is_sum_insured = 1 )
                                         AND rs.original_flag = 0)
           AND original_flag = 0

    -- PN 52372 - START
    DECLARE @isEdited INT

    SELECT @Recalculate_prorata_reinsurance_during_MTA = Isnull(VALUE, 0)
    FROM   system_options
    WHERE  option_number = 5070
           AND branch_id = 1

    IF @Recalculate_prorata_reinsurance_during_MTA = 1
       AND @is_auto_reinsured = 1
      -- recalculate original reinsurance
      BEGIN
          -- Arrangement Line cursor
          DECLARE arrangement_line_cursor CURSOR FAST_FORWARD FOR
            SELECT ra.ri_arrangement_id,
                   ral.ri_arrangement_line_id,
                   ral.treaty_id,
                   party_cnt
            FROM   ri_arrangement_line ral
                   INNER JOIN ri_arrangement ra
                           ON ra.ri_arrangement_id = ral.ri_arrangement_id
                   INNER JOIN ri_model_line rml
                           ON rml.ri_model_id = ra.ri_model_id
                   LEFT JOIN treaty t
                          ON t.replaced_by_treaty_id = ral.treaty_id
                             AND t.treaty_id = rml.treaty_id
            WHERE  ra.risk_cnt = @risk_cnt
                   AND ra.original_flag = 1
                   AND ral.treaty_id IS NOT NULL
                   AND t.treaty_id IS NOT NULL
                   AND version_id = @version_id
            ORDER  BY ral.ri_arrangement_line_id

          OPEN arrangement_line_cursor

          FETCH NEXT FROM arrangement_line_cursor INTO @ri_arrangement_id, @ri_arrangement_line_id, @treaty_id, @party_cnt

          -- For each of the old arrangements
          WHILE ( @@FETCH_STATUS = 0 )
            BEGIN
                -- Copy Default percent value from New RI to Original on the basis of Treaty ID
                -- here make sure Replaced by treaty should exact with new ri model else this cannot be update
                SELECT @default_share_percent = default_share_percent
                FROM   ri_arrangement_line ral
                       INNER JOIN ri_arrangement ra
                               ON ra.ri_arrangement_id = ral.ri_arrangement_id
                WHERE  ra.risk_cnt = @risk_cnt
                       AND ra.original_flag = 0
                       AND treaty_id = @treaty_id
                       AND version_id = @version_id

                IF @default_share_percent IS NOT NULL
                  BEGIN
                      UPDATE ri_arrangement_line
                      SET    default_share_percent = @default_share_percent
                      WHERE  ri_arrangement_line_id = @ri_arrangement_line_id

                      SELECT @isEdited = 1
                  END

                -- Get next arrangement line
                FETCH NEXT FROM arrangement_line_cursor INTO @ri_arrangement_id, @ri_arrangement_line_id, @treaty_id, @party_cnt
            END

          -- Close and release cursor
          CLOSE arrangement_line_cursor

          DEALLOCATE arrangement_line_cursor

          IF @isEdited = 1
            BEGIN
                DECLARE arrangement_band_cursor CURSOR FAST_FORWARD FOR
                  SELECT ri_arrangement_id,
                         xol_ri_model_id,
                         sum_insured,
                         premium
                  FROM   ri_arrangement
                  WHERE  risk_cnt = @risk_cnt
                         AND original_flag = 1
                         AND version_id = @version_id

                OPEN arrangement_band_cursor

                FETCH NEXT FROM arrangement_band_cursor INTO @ri_arrangement_id, @ri_model_id, @sum_insured, @premium

                -- For each of the old arrangements
                WHILE ( @@FETCH_STATUS = 0 )
                  BEGIN
                      EXECUTE Spu_ri_arrangement_calc_ri2007
                        @ri_arrangement_id = @ri_arrangement_id,
                        @band_si = @sum_insured,
                        @band_premium = @premium,
                        @ri_model_id = @ri_model_id,
                        @Trans_type = @Trans_type,
                        @calc_original = 1

                      --Send this 1 to calculate original RI Model
                      -- Get next arrangement line
                      FETCH NEXT FROM arrangement_band_cursor INTO @ri_arrangement_id, @ri_model_id, @sum_insured, @premium
                  END

                CLOSE arrangement_band_cursor

                DEALLOCATE arrangement_band_cursor
            END
      END

GO

--********************************************************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_add_stats_folder_reverse'

GO

CREATE PROCEDURE Spu_add_stats_folder_reverse @stats_folder_cnt   INT OUTPUT,
                                              @insurance_file_cnt INT,
                                              @user_id            INT,
                                              @user_name          VARCHAR(255),
                                              @next_orion_doc_ref VARCHAR(25),
                                              @transfer_date      DATETIME = NULL,
                                              @is_cloned_reverse  BIT = 0,
                                              @DocumentRef        VARCHAR(50)=''
AS
  BEGIN
      DECLARE @source_id                  INT,
              @debit_credit               CHAR(1),
              @document_prefix            CHAR(3),
              @document_ref               VARCHAR(25),
              @document_comment           VARCHAR(60),
              @document_date              DATETIME,
              @accounting_date            DATETIME,
              @posting_period_year        INT,
              @posting_period_number      SMALLINT,
              @premium_total              NUMERIC(19, 4),
              @transaction_type_id        INT,
              @transaction_type_code      CHAR(10),
              @transaction_date           DATETIME,
              @insurance_ref              VARCHAR(30),
              @effective_date             DATETIME,
              @cover_start_date           DATETIME,
              @expiry_date                DATETIME,
              @insurance_holder_cnt       INT,
              @insurance_holder_shortname VARCHAR(20),
              @insurance_holder_name      VARCHAR(60),
              @product_id                 INT,
              @product_code               CHAR(10),
              @business_type_id           SMALLINT,
              @business_type_code         CHAR(10),
              @account_handler_cnt        INT,
              @account_handler_shortname  CHAR(20),
              @branch_id                  SMALLINT,
              @branch_code                CHAR(10),
              @currency_code              CHAR(10),
              @agent_cnt                  INT,
              @agent_shortname            VARCHAR(20),
              @loss_id                    INT,
              @loss_code                  VARCHAR(30),
              @loss_date                  DATETIME,
              @created_by_user_id         SMALLINT,
              @created_by_username        VARCHAR(12),
              --@key_suffix_int int,  
              @currency_id                SMALLINT,
              --@retrieved_prefix varchar(10),  
              --@max_orion_ref varchar(20),  
              --@numbering_prefix char(3),  
              @sub_branch_id              INT,
              @multi_tree                 VARCHAR(20),
              @IsInstalments              CHAR(1),
              @underwriting_year_id       INT,
              @post_by_effective_date     TINYINT, --(RC) PLICO 9-10  
			  @Original_premium_total     NUMERIC(19, 4)

      /* Set temporary default values */
      SELECT @debit_credit = 'D',
             @document_prefix = 'SND',
             @document_ref = 'Doc Ref',
             @document_comment = 'New Business Premium',
             @document_date = Getdate(),
             @accounting_date = Getdate(),
             -- JMK 03/05/2001  
             --    @posting_period_year = GetDate(),  
             --    @posting_period_number = 5,  
             @premium_total = 0,
             @transaction_type_id = 1,
             @transaction_type_code = 'NB',
             @effective_date = Getdate(),
             @loss_id = NULL,
             @loss_code = NULL,
             @loss_date = NULL

      /* TF180598 - transaction_type details retrieved from Insurance_File_System */
      IF @DocumentRef = ''
        SELECT TOP 1 @transaction_type_id = T.transaction_type_id,
     @transaction_type_code = T.code,
					 @Original_premium_total = SF.premium_total
        FROM   stats_folder sf,
               Transaction_Type T
        WHERE  sf.insurance_file_cnt = @insurance_file_cnt
               AND T.transaction_type_id = sf.transaction_type_id
        ORDER  BY 1 DESC
      ELSE
        SELECT TOP 1 @transaction_type_id = SF.transaction_type_id,
                     @transaction_type_code = SF.transaction_type_code,
                     @branch_code = sf.branch_code,
					 @Original_premium_total = SF.premium_total
        FROM   stats_folder sf
        WHERE  sf.insurance_file_cnt = @insurance_file_cnt
               AND sf.document_ref = @DocumentRef

      IF @transfer_date IS NOT NULL
         AND @transaction_type_code <> 'DRI'
        SELECT @transaction_type_code = 'PT',
               @transaction_type_id = 22

      IF @is_cloned_reverse = 1
         AND @transaction_type_code = 'DRI'
        SELECT @transaction_type_code = 'DRIC',
               @transaction_type_id = 22

      -- RWH (08/08/01) Set document comment dependant on transaction type.  
      SELECT @document_comment = CASE @transaction_type_code
                                   WHEN 'MTA' THEN 'MTA Premium'
                                   WHEN 'MTC' THEN 'Mid-term Cancellation'
                                   WHEN 'MTR' THEN 'Policy Reinstatement'
                                   WHEN 'REN' THEN 'Renewal Premium'
                                   WHEN 'DRI' THEN 'Deferred Reinsurance'
                                   WHEN 'PT' THEN 'Portfolio Transfer'
                                   WHEN 'DRIC' THEN 'Deferred Reinsurance Reverse'
                                 END

      SELECT @IsInstalments = 'S'

      -- Determine the correct credit / debit status  
      SELECT @debit_credit = CASE
                               WHEN @Original_premium_total <= 0 THEN 'D'
                               ELSE 'C'
                             END

      SELECT @document_prefix = CASE @transaction_type_code
                                  WHEN 'MTA' THEN @IsInstalments + 'E' + @debit_credit -- i.e. SED  
                                  WHEN 'MTC' THEN @IsInstalments + 'E' + @debit_credit -- i.e. SEC  
                                  WHEN 'MTR' THEN @IsInstalments + 'I' + @debit_credit -- i.e. SID  
                                  WHEN 'REN' THEN @IsInstalments + 'R' + @debit_credit -- i.e. SRD  
                                  WHEN 'DRI' THEN 'SDD'
                                  WHEN 'PT' THEN 'SPD'
                                  WHEN 'DRIC' THEN 'SDR'
                                  ELSE @IsInstalments + 'N' + @debit_credit -- i.e. SND  
                                END

      SELECT @document_ref = @document_prefix + @next_orion_doc_ref

      SELECT @source_id = source_id,
             @insurance_ref = insurance_ref,
             @cover_start_date = cover_start_date,
             @expiry_date = expiry_date,
             @insurance_holder_cnt = insured_cnt,
             @product_id = product_id,
             @business_type_id = business_type_id,
             @account_handler_cnt = account_handler_cnt,
             @branch_id = branch_id,
             --@currency_id = currency_id,
             @agent_cnt = lead_agent_cnt,
             @sub_branch_id = branch_id,-- IFIBCR  
             @underwriting_year_id = underwriting_year_id
      FROM   Insurance_File
      WHERE  insurance_file_cnt = @insurance_file_cnt

	  select top 1 @currency_id = currency_id from Currency C
	  INNER JOIN Stats_folder SF ON C.code=SF.currency_code
	  where SF.insurance_file_cnt=@insurance_file_cnt
	  ORDER BY SF.stats_folder_cnt asc


      SELECT @multi_tree = value
      FROM   hidden_options
      WHERE  branch_id = 1
             AND option_number = 16

      IF ( Isnull(@multi_tree, '0') <> '1' )
        SELECT @sub_branch_id = 1

      SELECT @posting_period_number = posting_period_id
      FROM   Insurance_File
      WHERE  insurance_file_cnt = @insurance_file_cnt

      IF @posting_period_number IS NULL
        BEGIN
            --(RC) PLICO 9-10 START  
            SELECT @post_by_effective_date = value
            FROM   system_options
            WHERE  branch_id = 1
                   AND option_number = 5038

            IF Isnull(@post_by_effective_date, 0) = 1
              BEGIN
                  IF @cover_start_date > Getdate()
                    BEGIN
                        SELECT @effective_date = @cover_start_date
                    END

                  IF @cover_start_date <= Getdate()
                    BEGIN
                        SELECT @effective_date = Getdate()
                    END

                  -- Get posting period number  
                  CREATE TABLE #tempTable
                    (
                       period_id   INT,
                       year_name   VARCHAR(20),
                       period_name VARCHAR(15)
                    )

                  INSERT INTO #tempTable
                  EXEC Spu_act_do_getperiodfordate
                    @source_id,
                    @effective_date,
                    @sub_branch_id

                  SELECT @posting_period_number = period_id
                  FROM   #tempTable

                  DROP TABLE #tempTable

                  SELECT @posting_period_number
              END
            ELSE
              BEGIN
                  SELECT @posting_period_number = current_period_id
                  FROM   ledger
                  WHERE  ledger_short_name = 'SA'
                         AND sub_branch_id = @sub_branch_id
              END
        END

      SELECT @posting_period_year = Datepart(year, Min(period_end_date))
      FROM   period
      WHERE  year_name = (SELECT year_name
                          FROM   period
                          WHERE  Period_id = @posting_period_number)
             AND sub_branch_id = @sub_branch_id

      SELECT @insurance_holder_shortname = shortname,
             @insurance_holder_name = NAME
      FROM   Party
      WHERE  party_cnt = @insurance_holder_cnt

      SELECT @account_handler_shortname = shortname
      FROM   Party
      WHERE  party_cnt = @account_handler_cnt

      SELECT @product_code = code
      FROM   Product
      WHERE  product_id = @product_id

      SELECT @business_type_code = code
      FROM   Business_Type
      WHERE  business_type_id = @business_type_id

      --SELECT  @branch_code = code  
      --FROM    source  
      --WHERE   source_id = @source_id  
      SELECT @currency_code = iso_code
      FROM   Currency
      WHERE  currency_id = @currency_id

      SELECT @agent_shortname = shortname
      FROM   Party
      WHERE  party_cnt = @agent_cnt

      /* Set transaction date */
      SELECT @transaction_date = Getdate()

      /* Insert the Stats Folder */
      INSERT INTO Stats_Folder
                  (source_id,
                   debit_credit,
                   document_ref,
                   document_comment,
                   document_date,
                   accounting_date,
                   posting_period_year,
                   posting_period_number,
                   premium_total,
                   transaction_type_id,
                   transaction_type_code,
                   transaction_date,
                   insurance_file_cnt,
                   insurance_ref,
                   effective_date,
                   cover_start_date,
                   expiry_date,
                   insurance_holder_cnt,
                   insurance_holder_shortname,
                   insurance_holder_name,
                   product_id,
                   product_code,
                   business_type_id,
                   business_type_code,
                   account_handler_cnt,
                   account_handler_shortname,
                   branch_id,
 branch_code,
                   currency_code,
                   agent_cnt,
                   agent_shortname,
                   loss_id,
                   loss_code,
                   loss_date,
                   created_by_user_id,
                   created_by_username,
                   underwriting_year_id)
      VALUES      ( @source_id,
                    @debit_credit,
                    @document_ref,
                    @document_comment,
                    @document_date,
                    @accounting_date,
                    @posting_period_year,
                    @posting_period_number,
                    @premium_total,
                    @transaction_type_id,
                    @transaction_type_code,
                    @transaction_date,
                    @insurance_file_cnt,
                    @insurance_ref,
                    @effective_date,
                    @cover_start_date,
                    @expiry_date,
                    @insurance_holder_cnt,
                    @insurance_holder_shortname,
                    @insurance_holder_name,
                    @product_id,
                    @product_code,
                    @business_type_id,
                    @business_type_code,
                    @account_handler_cnt,
                    @account_handler_shortname,
                    @branch_id,
                    @branch_code,
                    @currency_code,
                    @agent_cnt,
                    @agent_shortname,
                    @loss_id,
                    @loss_code,
                    @loss_date,
                    @user_id,
                    @user_name,
                    @underwriting_year_id)

      /* Return the Count of the Record Added */
      SELECT @stats_folder_cnt = @@IDENTITY
  END

GO

--************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_add_stats_details_Reverse'

GO

CREATE PROCEDURE Spu_add_stats_details_reverse @nNewStatsFolderCnt INT,
                                               @nInsuranceFileCnt  INT
AS
  BEGIN
      DECLARE @nOldStatsFolderCnt INT

      SELECT TOP 1 @nOldStatsFolderCnt = stats_folder_cnt
      FROM   stats_folder
      WHERE  insurance_file_cnt = @nInsuranceFileCnt
             AND document_ref LIKE 'S%'
      ORDER  BY stats_folder_cnt

      INSERT INTO stats_detail
                  (stats_folder_cnt,
                   stats_detail_id,
                   stats_detail_type,
                   risk_id,
                   risk_type_id,
                   risk_type_code,
                   peril_id,
                   peril_description,
                   peril_type_id,
                   peril_type_code,
                   policy_section_type_id,
                   policy_section_type_code,
                   class_of_business_id,
                   class_of_business_code,
                   tax_type_id,
                   tax_type_code,
                   tax_value,
                   ri_party_cnt,
                   ri_shortname,
                   ri_party_type,
                   ri_share_percent,
                   ri_agreement_code,
                   annual_premium,
                   currency_code,
                   currency_rate,
                   this_premium_original,
                   this_premium_home,
                   commission_percent,
                   lead_commission_value_home,
                   sub_commission_value_home,
                   sum_insured_home,
                   sum_insured_currency_code,
                   sum_insured_change,
                   transaction_ledger_id,
                   transaction_account_id,
                   account_type_code,
                   ceded_ref,
                   cover_share_percent,
                   sum_insured_total,
                   charges_total,
                   taxes_total,
                   recoveries_total,
                   commission_excluded,
                   withholding_tax_excluded,
                   purchase_order_no,
                   purchase_invoice_no,
                   stats_version,
                   this_premium_system,
                   lead_commission_value_system,
                   sub_commission_value_system,
                   sum_insured_system,
                   is_commission_modified,
                   original_flag,
                   cover_to_date,
                   Claim_RI_Only_Amendment,
                   Earning_Pattern_id,
                   ri_arrangement_line_Id)
      SELECT @nNewStatsFolderCnt,
             stats_detail_id,
             stats_detail_type,
             risk_id,
             risk_type_id,
             risk_type_code,
             peril_id,
             peril_description,
             peril_type_id,
             peril_type_code,
             policy_section_type_id,
             policy_section_type_code,
             class_of_business_id,
             class_of_business_code,
             tax_type_id,
             tax_type_code,
             -1 * tax_value,
             ri_party_cnt,
             ri_shortname,
             ri_party_type,
             ri_share_percent,
             ri_agreement_code,
             -1 * annual_premium,
             currency_code,
             currency_rate,
             -1 * this_premium_original,
             -1 * this_premium_home,
             commission_percent,
             -1 * lead_commission_value_home,
             -1 * sub_commission_value_home,
             -1 * sum_insured_home,
             sum_insured_currency_code,
             -1 * sum_insured_change,
             transaction_ledger_id,
             transaction_account_id,
             account_type_code,
             ceded_ref,
             cover_share_percent,
             -1 * sum_insured_total,
             charges_total,
             taxes_total,
             recoveries_total,
             commission_excluded,
             withholding_tax_excluded,
             purchase_order_no,
             purchase_invoice_no,
             stats_version,
             -1 * this_premium_system,
             -1 * lead_commission_value_system,
             -1 * sub_commission_value_system,
             -1 * sum_insured_system,
             is_commission_modified,
             original_flag,
             cover_to_date,
             Claim_RI_Only_Amendment,
             Earning_Pattern_id,
             ri_arrangement_line_Id
      FROM   stats_detail
      WHERE  stats_folder_cnt = @nOldStatsfolderCnt
  END

GO

--***************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_CopyReversalDocument'

GO

CREATE PROCEDURE Spu_copyreversaldocument @document_id     INT OUTPUT,
                                          @oldDocRef       VARCHAR(25),
                                          @nStatsFolderCnt INT
AS
    DECLARE @documenttype_id INT
    DECLARE @sNewDocRef VARCHAR(25)

    SELECT @sNewDocRef = document_ref
    FROM   Stats_Folder
    WHERE  stats_folder_cnt = @nStatsFolderCnt

    SELECT @documenttype_id = documenttype_id
    FROM   DocumentType
    WHERE  code = Substring(@sNewDocRef, 1, 3)

    INSERT INTO Document
                (company_id,
                 sub_branch_id,
                 postingstatus_id,
                 documenttype_id,
                 auditset_id,
                 batch_id,
                 document_ref,
                 document_date,
                 created_date,
                 authorised_date,
                 comment,
                 write_off_reason_id,
                 insurance_file_cnt,
                 reason,
                 claim_id,
                 terms_of_payment_id,
                 payment_due_date)
    SELECT company_id,
           sub_branch_id,
           postingstatus_id,
           @documenttype_id,
           auditset_id,
           NULL,
           @sNewDocRef,
           Getdate(),
           Getdate(),
           authorised_date,
           'Reverse of Document' + @oldDocRef,
           write_off_reason_id,
           insurance_file_cnt,
           reason,
           claim_id,
           terms_of_payment_id,
           payment_due_date
    FROM   Document
    WHERE  document_ref = @oldDocRef

    SELECT @document_id = @@IDENTITY

GO

--**********************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_update_TransDetail_Comment'

GO

CREATE PROCEDURE Spu_update_transdetail_comment @NewDocument_ID INT,
                                                @OldDocumentRef VARCHAR(40)
AS
    DECLARE @sCommentForReversalDocument VARCHAR(100),
            @sCommentForOriginalDocument VARCHAR(100),
            @oldDocument_ID              INT,
            @NewDocumentRef              VARCHAR(40)

    SELECT @sCommentForReversalDocument = 'Reverse of Document ' + @OldDocumentRef

    SELECT TOP 1 @oldDocument_ID = document_id
    FROM   document
    WHERE  document_ref = @OldDocumentRef

    SELECT TOP 1 @NewDocumentRef = document_ref
    FROM   Document
    WHERE  document_id = @NewDocument_ID

    SELECT @sCommentForOriginalDocument = 'Reversed by ' + @NewDocumentRef

    UPDATE TransDetail
    SET    comment = @sCommentForReversalDocument
    WHERE  document_id = @NewDocument_ID

    UPDATE TransDetail
    SET    comment = @sCommentForOriginalDocument
    WHERE  document_id = @oldDocument_ID

GO

--************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_Copy_Stats_Reversal_Claim_By_Document_Ref'

GO

CREATE PROCEDURE Spu_copy_stats_reversal_claim_by_document_ref @OldDocument_ref      VARCHAR(20),
                                                               @New_Stats_Folder_Cnt INT
AS
    DECLARE @oldStatsFolderCnt INT

    SELECT @oldStatsFolderCnt = stats_folder_cnt
    FROM   Stats_folder
    WHERE  document_ref = @OldDocument_ref

    INSERT INTO Stats_Detail
                (stats_folder_cnt,
                 stats_detail_id,
                 stats_detail_type,
                 risk_id,
                 risk_type_id,
                 risk_type_code,
                 peril_id,
                 peril_description,
                 peril_type_id,
                 peril_type_code,
                 policy_section_type_id,
                 policy_section_type_code,
                 class_of_business_id,
                 class_of_business_code,
                 tax_type_id,
                 tax_type_code,
                 tax_value,
                 ri_party_cnt,
                 ri_shortname,
                 ri_party_type,
                 ri_share_percent,
                 ri_agreement_code,
                 annual_premium,
                 currency_code,
                 currency_rate,
                 this_premium_original,
                 this_premium_home,
                 commission_percent,
                 lead_commission_value_home,
                 sub_commission_value_home,
                 sum_insured_home,
                 sum_insured_currency_code,
                 sum_insured_change,
                 transaction_ledger_id,
                 transaction_account_id,
                 account_type_code,
                 ceded_ref,
                 cover_share_percent,
                 sum_insured_total,
                 charges_total,
                 taxes_total,
                 recoveries_total,
                 commission_excluded,
                 withholding_tax_excluded,
                 purchase_order_no,
                 purchase_invoice_no,
                 stats_version,
                 this_premium_system,
                 lead_commission_value_system,
                 sub_commission_value_system,
                 sum_insured_system,
                 is_commission_modified,
                 original_flag,
                 cover_to_date,
                 Claim_RI_Only_Amendment,
                 Earning_Pattern_id,
                 ri_arrangement_line_Id)
    SELECT @New_Stats_Folder_Cnt,
           stats_detail_id,
           stats_detail_type,
           risk_id,
           risk_type_id,
           risk_type_code,
           peril_id,
           peril_description,
           peril_type_id,
           peril_type_code,
           policy_section_type_id,
           policy_section_type_code,
           class_of_business_id,
           class_of_business_code,
           tax_type_id,
           tax_type_code,
           -tax_value,
           ri_party_cnt,
           ri_shortname,
           ri_party_type,
           ri_share_percent,
           ri_agreement_code,
           -annual_premium,
           currency_code,
           currency_rate,
           -this_premium_original,
           -this_premium_home,
           commission_percent,
           -lead_commission_value_home,
           -sub_commission_value_home,
           -sum_insured_home,
           sum_insured_currency_code,
           -sum_insured_change,
           transaction_ledger_id,
           transaction_account_id,
           account_type_code,
           ceded_ref,
           cover_share_percent,
           -sum_insured_total,
           -charges_total,
           -taxes_total,
           -recoveries_total,
           commission_excluded,
           withholding_tax_excluded,
           purchase_order_no,
           purchase_invoice_no,
           stats_version,
           -this_premium_system,
           -lead_commission_value_system,
           -sub_commission_value_system,
           -sum_insured_system,
           is_commission_modified,
           original_flag,
           cover_to_date,
           Claim_RI_Only_Amendment,
           Earning_Pattern_id,
           ri_arrangement_line_Id
    FROM   Stats_Detail
    WHERE  Stats_Folder_cnt = @oldStatsFolderCnt

GO

--****************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_CLM_Finalise_stats_Reversal'

GO

CREATE PROCEDURE Spu_clm_finalise_stats_reversal @claim_id              INT,
                                                 @transaction_type_id   INT,
                                                 @transaction_type_code VARCHAR(10),
                                                 @stats_folder_cnt      INT,
                                                 @bstatssuppressed      TINYINT output,
                                                 @is_cloned             TINYINT =0,
                                                 @is_cloned_reversal    TINYINT =0,
                                                 @is_pt                 TINYINT =0
AS
    DECLARE @source_id                     INT,
            @sub_branch_id                 INT,
            @stats_detail_id               INT,
            @transaction_export_folder_cnt INT,
            @transaction_export_detail_id  INT,
            @document_prefix               CHAR(3),
            @retrieved_prefix              VARCHAR(10),
            @max_orion_ref                 VARCHAR(20),
            @document_ref                  VARCHAR(25),
            @posting_period_year           INT,
            @posting_period_number         SMALLINT,
            @key_suffix_int                INT,
            @transaction_amount            NUMERIC(19, 4),
            @NumberRangeID                 INT,
            @DocumentRefNumber             VARCHAR(25),
            @UniqueDocumentRef             INTEGER,
            @user_id                       INT

    SELECT @transaction_type_code = transaction_type_code
    FROM   stats_folder
    WHERE  stats_folder_cnt = @stats_folder_cnt

    SELECT @transaction_type_code = Ltrim(Rtrim(@transaction_type_code))

    DECLARE @suppress INT

    EXEC Spu_clm_suppress_stats
      @claim_id,
      @transaction_type_code,
      @suppress OUTPUT

    IF @suppress = 1
      BEGIN
          DELETE FROM stats_detail
          WHERE  stats_folder_cnt = @stats_folder_cnt

          DELETE FROM stats_folder
          WHERE  stats_folder_cnt = @stats_folder_cnt

          SET @bstatssuppressed = 1

          RETURN
      END

    IF @is_cloned = 1
      BEGIN
          UPDATE Stats_Detail
          SET    tax_value = 0,
                 annual_premium = 0,
                 this_premium_original = 0,
                 this_premium_home = 0,
                 lead_commission_value_home = 0,
                 sub_commission_value_home = 0,
                 sum_insured_home = 0,
                 sum_insured_total = 0,
                 charges_total = 0,
                 taxes_total = 0,
                 recoveries_total = 0,
                 commission_excluded = 0,
                 withholding_tax_excluded = 0,
                 this_premium_system = 0,
                 lead_commission_value_system = 0,
                 sub_commission_value_system = 0,
                 sum_insured_system = 0
          WHERE  stats_folder_cnt = @stats_folder_cnt
                 AND Stats_detail_type = 'GRS'
      END

    SELECT @UniqueDocumentRef = Isnull(value, 0)
    FROM   Hidden_options
    WHERE  branch_id = 1
           AND option_number = 102

    SELECT @document_prefix = CASE @transaction_type_code
                                WHEN 'C_CO' THEN 'CLO'
                                WHEN 'C_CP' THEN 'CLP'
                                WHEN 'C_CR' THEN 'CLA'
                                WHEN 'C_SA' THEN 'CLR'
                                WHEN 'C_RV' THEN 'CLR'
                              END

    IF @is_cloned = 1
      SELECT @document_prefix = 'CLD'

    IF @is_cloned_reversal = 1
      SELECT @document_prefix = 'CLC'

    IF @is_pt = 1
      SELECT @document_prefix = 'CPA'

    IF ( @UniqueDocumentRef = 1 )
      BEGIN
          SELECT @retrieved_prefix = prefix,
                 @key_suffix_int = next_number
          FROM   Next_Orion_Doc_Ref
          WHERE  prefix = @document_prefix

          IF @Key_Suffix_Int IS NULL
            BEGIN
                SELECT @max_orion_ref = Max(document_ref)
                FROM   Document
                WHERE  document_ref LIKE @document_prefix + '%'
                       AND Len (Ltrim (Rtrim (document_ref))) - Len (@document_prefix) = 8

                IF @max_orion_ref IS NOT NULL
                  SELECT @Key_Suffix_Int = Substring ( @max_orion_ref, Len ( @document_prefix ) + 1, 8 )
                                           + 1
                ELSE
                  SELECT @Key_Suffix_Int = 10000001
            END

          EXEC Spu_act_get_number_range_from_code
            @document_prefix,
            @NumberRangeID OUTPUT

          EXEC Spu_act_generate_next_unique_document_reference
            @NumberRangeID,
            1,
            1,
            @DocumentRefNumber OUTPUT

          SELECT @document_ref = @document_prefix + @DocumentRefNumber

          IF ( @retrieved_prefix IS NULL )
              OR ( @retrieved_prefix = '' )
            INSERT INTO Next_Orion_Doc_Ref
            VALUES      (@document_prefix,
                         @Key_Suffix_Int + 1)
          ELSE
            UPDATE Next_Orion_Doc_Ref
            SET    next_number = @Key_Suffix_Int + 1
            WHERE  prefix = @document_prefix
      END
    ELSE
      BEGIN
          SELECT @source_id = source_id,
                 @user_id = created_by_user_id
          FROM   stats_folder
          WHERE  stats_folder_cnt = @stats_folder_cnt

          SELECT @NumberRangeID = CASE @document_prefix
                                    WHEN 'CLO' THEN 40
                                    WHEN 'CLP' THEN 28
                                    WHEN 'CLA' THEN 41
                                    WHEN 'CLR' THEN 29
                                    WHEN 'CLD' THEN 59
                                    WHEN 'CPA' THEN 60
                                    WHEN 'CLC' THEN 58
                                  END

          EXEC Spe_actnumber_add
            @Key_Suffix_Int OUTPUT,
            @NumberRangeID,
            @user_id,
            @source_id

          SELECT @document_ref = Rtrim(@document_prefix)
                                 + CONVERT(VARCHAR, Replicate ( 0, 10-Len(@Key_Suffix_Int)))
                                 + CONVERT(VARCHAR, @Key_Suffix_Int)
      END

    DECLARE @ProductOption INT

    SELECT @ProductOption = value
    FROM   Hidden_Options
    WHERE  branch_id = 1
           AND option_number = 16

    SELECT @sub_branch_id = branch_id
    FROM   insurance_file
           INNER JOIN claim
                   ON insurance_file.insurance_file_cnt = claim.policy_id
    WHERE  claim.claim_id = @claim_id

    IF @ProductOption = 1
      BEGIN
          SELECT @posting_period_number = current_period_id
          FROM   ledger
          WHERE  ledger_short_name = 'SA'
                 AND sub_branch_id = @sub_branch_id

          SELECT @posting_period_year = Datepart(year, Min(period_end_date))
          FROM   period
          WHERE  year_name = (SELECT year_name
                              FROM   period
                              WHERE  Period_id = @posting_period_number)
                 AND sub_branch_id = @sub_branch_id
      END
    ELSE
      BEGIN
          SELECT @posting_period_number = current_period_id
          FROM   ledger
          WHERE  ledger_short_name = 'SA'

          SELECT @posting_period_year = Datepart(year, Min(period_end_date))
          FROM   period
          WHERE  year_name = (SELECT year_name
                              FROM   period
                              WHERE  Period_id = @posting_period_number)
      END

  BEGIN
      UPDATE Stats_Folder
      SET    document_ref = @document_ref,
             posting_period_year = @posting_period_year,
             posting_period_number = @posting_period_number,
             transaction_type_id = @transaction_type_id,
             transaction_type_code = @transaction_type_code
      WHERE  stats_folder_cnt = @stats_folder_cnt
  END

GO

--**************************************************************************************************************************
Ddldropprocedure 'spu_allocate_transactions_by_docref'

GO

CREATE PROCEDURE Spu_allocate_transactions_by_docref @old_document_ref VARCHAR(50),
                                                     @new_document_id  VARCHAR(50)
AS
    DECLARE @old_document_id     INT,
            @allocation_batch_id INT,
            @account_id          INT,
            @Today               DATETIME,
            @Allocation_id       INT,
            @company_id          INT

    SET @Today = Getdate()

    SELECT @old_document_id = document_id
    FROM   document
    WHERE  document_ref = @old_document_ref

    CREATE TABLE #OutStandingTransactions
      (
         account_id                  INT,
         transdetail_id              INT,
         outstanding_amount          NUMERIC(17, 2),
         outstanding_currency_amount NUMERIC(17, 2),
         total_outstanding           NUMERIC(17, 2),
         rank                        INT,
         fully_matched               BIT,
         is_primary                  BIT,
         currency_id                 SMALLINT,
         amount                      NUMERIC(19, 4),
         base_amount_unrounded       NUMERIC(19, 4),
         currency_amount             NUMERIC(19, 4),
         currency_amount_unrounded   NUMERIC(19, 4),
         currency_base_xrate         NUMERIC(19, 4),
         document_id                 INT,
         document_type_id            INT,
         document_ref                VARCHAR(25),
         document_date               DATETIME,
         document_sequence           SMALLINT,
         spare                       VARCHAR(30),
         account_amount              NUMERIC(19, 4),
         account_amount_unrounded    NUMERIC(19, 4),
         system_amount               NUMERIC(19, 4),
         system_amount_unrounded     NUMERIC(19, 4),
         account_base_xrate          NUMERIC(19, 4),
         system_base_xrate           NUMERIC(19, 4),
         AllocBaseAmount             NUMERIC(19, 4),
         AllocCCyAmount              NUMERIC(19, 4),
         AllocAccountAmount          NUMERIC(19, 4),
         AllocSystemAmount           NUMERIC(19, 4),
         cBaseMatchAmount            NUMERIC(19, 4),
         cCurrencyMatchAmount        NUMERIC(19, 4),
         OSBaseAmount                NUMERIC(19, 4),
         OSCcyAmount                 NUMERIC(19, 4),
         ID                          INT IDENTITY(1, 1),
         IsOriginalDocRef            BIT,
         company_id                  INT
      )

    INSERT INTO #OutStandingTransactions
                (account_id,
                 transdetail_id,
                 outstanding_amount,
                 outstanding_currency_amount,
                 total_outstanding,
                 rank,
                 fully_matched,
                 currency_id,
                 amount,
                 base_amount_unrounded,
                 currency_amount,
                 currency_amount_unrounded,
                 currency_base_xrate,
                 document_id,
                 document_sequence,
                 spare,
                 account_amount,
                 account_amount_unrounded,
                 system_amount,
                 system_amount_unrounded,
                 account_base_xrate,
                 system_base_xrate,
                 document_type_id,
                 document_ref,
                 document_date,
                 cBaseMatchAmount,
                 cCurrencyMatchAmount,
                 AllocBaseAmount,
                 AllocAccountAmount,
                 AllocSystemAmount,
                 OSBaseAmount,
                 OSCcyAmount,
                 AllocCcyAmount,
                 IsOriginalDocRef,
                 company_id)
    SELECT td.account_id,
           td.transdetail_id,
           td.outstanding_amount,
           td.outstanding_currency_amount,
           0.0,
           1,
           TD.fully_matched,
           TD.currency_id,
           TD.amount,
           TD.amount,
           TD.currency_amount,
           TD.currency_amount,
           TD.currency_base_xrate,
           TD.document_id,
           TD.document_sequence,
           TD.spare,
           TD.account_amount,
           TD.account_amount_unrounded,
           TD.system_amount,
           TD.system_amount_unrounded,
           TD.account_base_xrate,
           TD.system_base_xrate,
           D.documentType_id,
           D.Document_ref,
           d.document_date,
           0,
           0,
           td.outstanding_amount,
           td.outstanding_amount / td.account_base_xrate,
           td.outstanding_amount / td.system_base_xrate,
           td.amount,
           td.currency_amount,
           td.outstanding_amount,
           1,
           td.company_id
    FROM   Transdetail td
           JOIN Document D
             ON TD.Document_id = D.Document_id
    WHERE  TD.Document_id = @old_document_id

    INSERT INTO #OutStandingTransactions
                (account_id,
                 transdetail_id,
                 outstanding_amount,
                 outstanding_currency_amount,
                 total_outstanding,
                 rank,
                 fully_matched,
                 currency_id,
                 amount,
                 base_amount_unrounded,
                 currency_amount,
                 currency_amount_unrounded,
                 currency_base_xrate,
                 document_id,
                 document_sequence,
                 spare,
                 account_amount,
                 account_amount_unrounded,
                 system_amount,
                 system_amount_unrounded,
                 account_base_xrate,
                 system_base_xrate,
                 document_type_id,
                 document_ref,
                 document_date,
                 cBaseMatchAmount,
                 cCurrencyMatchAmount,
                 AllocBaseAmount,
                 AllocAccountAmount,
                 AllocSystemAmount,
                 OSBaseAmount,
                 OSCcyAmount,
                 AllocCcyAmount,
                 IsOriginalDocRef,
                 company_id)
    SELECT td.account_id,
           td.transdetail_id,
           td.outstanding_amount,
           td.outstanding_currency_amount,
           0.0,
           1,
           TD.fully_matched,
           TD.currency_id,
           TD.amount,
           TD.amount,
           TD.currency_amount,
           TD.currency_amount,
           TD.currency_base_xrate,
           TD.document_id,
           TD.document_sequence,
           TD.spare,
           TD.account_amount,
           TD.account_amount_unrounded,
           TD.system_amount,
           TD.system_amount_unrounded,
           TD.account_base_xrate,
           TD.system_base_xrate,
           D.documentType_id,
           D.Document_ref,
           d.document_date,
           0,
           0,
           td.outstanding_amount,
           td.outstanding_amount / td.account_base_xrate,
           td.outstanding_amount / td.system_base_xrate,
           td.amount,
           td.currency_amount,
           td.outstanding_amount,
           0,
           td.company_id
    FROM   Transdetail td
           JOIN Document D
             ON TD.Document_id = D.Document_id
    WHERE  TD.Document_id = @new_document_id

    DELETE #OutStandingTransactions
    WHERE  outstanding_amount = 0

    EXEC Spu_act_add_allocationbatch
      @allocation_batch_id OUTPUT

    DECLARE allocation_cursor CURSOR FAST_FORWARD FOR
      SELECT DISTINCT account_id,
                      company_id
      FROM   #OutStandingTransactions

    OPEN allocation_cursor

    FETCH NEXT FROM allocation_cursor INTO @account_id, @company_id

    WHILE @@FETCH_STATUS <> -1
      BEGIN
          IF (SELECT Sum(outstanding_amount)
              FROM   #OutStandingTransactions
              WHERE  account_id = @account_id
                     AND IsOriginalDocRef = 0) = -1 * (SELECT Sum(outstanding_amount)
                                                       FROM   #OutStandingTransactions
                                                       WHERE  account_id = @account_id
                                                              AND IsOriginalDocRef = 1)
            BEGIN
                EXEC Spu_act_add_allocation
                  @company_id=@company_id,
                  @account_id=@Account_id,
                  @user_id=1,
                  @allocation_date=@Today,
                  @allocationstatus_id=3,
                  @nAllocationbatch_id=@allocation_batch_id,
                  @Allocation_id=@Allocation_id OUTPUT

                INSERT INTO AllocationDetail
                            (allocation_id,
                             original_currency,
                             transdetail_id,
                             documenttype_id,
                             document_ref,
                             accounting_date,
                             original_date,
                             allocate_to_base,
                             orig_base_amount,
                             orig_base_amount_unrounded,
                             orig_ccy_amount,
                             orig_ccy_amount_unrounded,
                             orig_xrate,
                             effective_xrate,
                             os_base_amount,
                             os_ccy_amount,
                             alloc_base_amount,
                             alloc_ccy_amount,
                             fully_matched,
                             new_os_ccy_amount,
                             new_os_base_amount,
                             loss_gain_amount,
                             is_primary,
                             euro_currency_id,
                             euro_amount,
                             euro_base_xrate,
                             euro_ccy_xrate,
                             alloc_account_amount,
                             alloc_system_amount)
                SELECT @Allocation_id,
                       T.Currency_id,
                       T.transdetail_id,
                       T.Document_type_id,
                       T.Document_ref,
                       @Today,
                       T.document_date,
                       0,
                       T.amount,
                       T.amount,
                       currency_amount,
                       currency_amount_unrounded,
                       currency_base_xrate,
                       currency_base_xrate,
                       OSBaseAmount,
                       OSCcyAmount,
                       AllocBaseAmount,
                       AllocCCyAmount,
                       fully_matched,
                       OSCcyAmount - AllocCCyAmount,
                       OSBaseAmount - AllocBaseAmount,
                       0,
                       Isnull(is_primary, 0),
                       0,
                       0.0,
                       1,
                       1,
                       AllocAccountAmount,
                       AllocSystemAmount
                FROM   #OutStandingTransactions T
                WHERE  account_id = @account_id

                UPDATE T
                SET    T.outstanding_amount = 0,
                       T.outstanding_currency_amount = 0,
                       T.outstanding_account_amount = 0,
                       T.outstanding_system_amount = 0,
                       T.amount_updated = Getdate(),
                       T.Fully_matched = 1
                FROM   TransDetail T
                       JOIN #OutStandingTransactions O
                         ON T.Transdetail_id = O.TransDetail_id
                WHERE  t.account_id = @account_id
            END

          FETCH NEXT FROM allocation_cursor INTO @account_id, @company_id
      END

    CLOSE allocation_cursor

    DEALLOCATE allocation_cursor

    DROP TABLE #OutStandingTransactions

GO

--**********************************************************************************************************************
Ddldropprocedure 'spu_claim_stats_reverse'

GO

CREATE PROCEDURE Spu_claim_stats_reverse @claim_id             INT,
                                         @New_Stats_Folder_Cnt INT,
                                         @oldDocRef            VARCHAR(15)=''
AS
    DECLARE @ri_arrangement_version INT
    DECLARE @Stats_folder_cnt INT,
            @stats_detail_id  INT
    DECLARE @Base_claim_id INT
    DECLARE @Old_Stats_folder_cnt INT
    DECLARE @insurance_file_cnt    INT,
            @Document_comment      VARCHAR(100),
            @transaction_type_code VARCHAR(10),
            @created_by_user_id    INT,
            @Debit_Credit          VARCHAR(1),
            @transaction_type_id   INT,
            @user_id               INT,
            @user_name             VARCHAR(25)
    DECLARE stats_details_cur CURSOR Static FOR
      SELECT sd.stats_detail_id,
             sd.stats_folder_cnt
      FROM   stats_detail sd
             JOIN stats_folder sf
               ON sd.stats_folder_cnt = sf.stats_folder_cnt
      WHERE  sf.loss_id = @claim_id
             AND sf.stats_folder_cnt <> @New_Stats_Folder_Cnt
			 AND sf.document_ref = @oldDocRef
             AND sd.Stats_detail_type IN( 'FAC', 'FAX', 'TTY', 'TYX',
                                          'TFS', 'NET', 'GRS', 'XOL','TAG','TAN' ,'TR')

    OPEN stats_details_cur

    FETCH next FROM stats_details_cur INTO @stats_detail_id, @Stats_folder_cnt

    WHILE @@FETCH_STATUS = 0
      BEGIN
          DECLARE @sd_id INT

          SELECT @sd_id = Isnull(Max(stats_detail_id), 0) + 1
          FROM   stats_detail
          WHERE  Stats_folder_cnt = @New_Stats_Folder_Cnt

          INSERT INTO stats_detail
                      (stats_folder_cnt,
                       stats_detail_id,
                       stats_detail_type,
                       risk_id,
                       risk_type_id,
                       risk_type_code,
                       peril_id,
                       peril_description,
                       peril_type_id,
                       peril_type_code,
                       policy_section_type_id,
                       policy_section_type_code,
                       class_of_business_id,
                       class_of_business_code,
                       tax_type_id,
                       tax_type_code,
                       tax_value,
                       ri_party_cnt,
                       ri_shortname,
                       ri_party_type,
                       ri_share_percent,
                       ri_agreement_code,
                       annual_premium,
                       currency_code,
                       currency_rate,
                       this_premium_original,
                       this_premium_home,
                       commission_percent,
                       lead_commission_value_home,
                       sub_commission_value_home,
                       sum_insured_home,
                       sum_insured_currency_code,
                       sum_insured_change,
                       transaction_ledger_id,
                       transaction_account_id,
                       account_type_code,
                       ceded_ref,
                       cover_share_percent,
                       sum_insured_total,
                       charges_total,
                       taxes_total,
                       recoveries_total,
                       commission_excluded,
                       withholding_tax_excluded,
                       purchase_order_no,
                       purchase_invoice_no,
                       stats_version,
                       this_premium_system,
                       lead_commission_value_system,
                       sub_commission_value_system,
                       sum_insured_system,
                       is_commission_modified,
                       original_flag,
                       cover_to_date,
                       Claim_RI_Only_Amendment)
          SELECT @New_Stats_Folder_Cnt,
                 @sd_id,
                 stats_detail_type,
                 risk_id,
                 risk_type_id,
                 risk_type_code,
                 peril_id,
                 peril_description,
                 peril_type_id,
                 peril_type_code,
                 policy_section_type_id,
                 policy_section_type_code,
                 class_of_business_id,
                 class_of_business_code,
                 tax_type_id,
                 tax_type_code,
                 tax_value * -1,
                 ri_party_cnt,
                 ri_shortname,
                 ri_party_type,
                 ri_share_percent,
                 ri_agreement_code,
                 annual_premium * -1,
                 currency_code,
                 currency_rate,
                 this_premium_original * -1,
                 this_premium_home * -1,
                 commission_percent,
                 lead_commission_value_home * -1,
                 sub_commission_value_home * -1,
                 sum_insured_home * -1,
                 sum_insured_currency_code,
                 sum_insured_change,
                 transaction_ledger_id,
                 transaction_account_id,
                 account_type_code,
                 ceded_ref,
                 cover_share_percent,
                 sum_insured_total * -1,
                 charges_total * -1,
                 taxes_total * -1,
                 recoveries_total * -1,
                 commission_excluded * -1,
                 withholding_tax_excluded * -1,
                 purchase_order_no,
                 purchase_invoice_no,
                 stats_version,
                 this_premium_system * -1,
                 lead_commission_value_system * -1,
                 sub_commission_value_system * -1,
                 sum_insured_system * -1,
                 is_commission_modified,
                 original_flag,
                 cover_to_date,
                 1
          FROM   Stats_detail
          WHERE  Stats_folder_cnt = @Stats_folder_cnt
                 AND stats_detail_id = @stats_detail_id

          FETCH next FROM stats_details_cur INTO @stats_detail_id, @Stats_folder_cnt
      END

    CLOSE stats_details_cur

    DEALLOCATE stats_details_cur

    IF ( @oldDocRef <> '' )
      BEGIN
          SELECT @Old_Stats_folder_cnt = stats_folder_cnt
          FROM   Stats_Folder
          WHERE  document_ref = @oldDocRef

          UPDATE Stats_Folder
          SET    premium_total = (SELECT -premium_total
                                  FROM   stats_folder
                                  WHERE  stats_folder_cnt = @Old_Stats_folder_cnt)
          WHERE  stats_folder_cnt = @New_Stats_Folder_Cnt
      END

GO

--*************************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_CopyReversalTransExportDetail'

GO

CREATE PROCEDURE Spu_copyreversaltransexportdetail @nNewTransactionExportFolderCnt INT,
                                                   @sOldDocumentRef                VARCHAR(25)
AS
    INSERT INTO Transaction_Export_detail
                (transaction_export_folder_cnt,
                 transaction_export_detail_id,
                 transaction_amount,
                 transaction_ledger_code,
                 account_type_code,
                 sum_insured_total,
                 mapping_code,
                 transdetail_type_code,
                 spare)
    (SELECT @nNewTransactionExportFolderCnt,
            transaction_export_detail_id,
            -1 * transaction_amount,
            transaction_ledger_code,
            account_type_code,
            -1 * sum_insured_total,
            mapping_code,
            transdetail_type_code,
            spare
     FROM   Transaction_Export_Detail
     WHERE  transaction_export_folder_cnt IN(SELECT transaction_export_folder_cnt
                                             FROM   Transaction_Export_Folder
                                             WHERE  document_ref = @sOldDocumentRef))

GO

--*******************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_reverse_SRP'

GO

CREATE PROCEDURE Spu_reverse_srp @transdetail_id INT
AS
    DECLARE @allocationid INT

    SELECT allocation_id
    INTO   #temp
    FROM   AllocationDetail
    WHERE  transdetail_id = @transdetail_id

    DECLARE data_fix CURSOR FOR
      SELECT allocation_id
      FROM   #temp

    OPEN data_fix

    FETCH next FROM data_fix INTO @allocationid

    WHILE @@FETCH_STATUS = 0
      BEGIN
          EXEC Spu_act_do_allocation_reversal_srp
            @allocationid

          PRINT @allocationid

          FETCH NEXT FROM data_fix INTO @allocationid
      END

    CLOSE data_fix

    DEALLOCATE data_fix

GO

--***********************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_ACT_Sel_TransDetail_By_Doc_FACAlloc_DataFix'

GO

CREATE PROCEDURE Spu_act_sel_transdetail_by_doc_facalloc_datafix @document_id   INT,
                                                                 @iAllocationId INT=0,
                                                                 @iAccountId    INT=0,
                                                                 @nAmount       NUMERIC(19, 4)=0
AS
    DECLARE @DocumentType_id INT

    IF ( @iAllocationId = 0 )
      BEGIN
          SELECT @DocumentType_id = documenttype_id
          FROM   Document
          WHERE  document_id = @document_id

          IF ( @DocumentType_id IN ( 50, 59 ) )
            SELECT transdetail_id,
                   account_id,
                   amount
            FROM   TransDetail
            WHERE  document_id = @document_id
                   AND account_id = @iAccountId
                   AND amount = -@nAmount
      END
    ELSE
      SELECT TD.transdetail_id,
             TD.account_id,
             TD.amount
      FROM   AllocationDetail AD
             INNER JOIN Document D
                     ON AD.document_ref = D.document_ref
             INNER JOIN TransDetail TD
                     ON AD.transdetail_id = TD.transdetail_id
      WHERE  allocation_id = @iAllocationId
             AND D.document_id = @document_id

GO

--**************************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_Claim_Recalculate_Reinsurance'

GO

CREATE PROCEDURE Spu_claim_recalculate_reinsurance @Claim_id INT
AS
    DECLARE @original_claim_id          INT,
            @Reserve                    MONEY,
            @Payment                    MONEY,
            @ri_arrangement_line_id     INT,
            @max_ri_arrangement_line_id INT,
            @Claim_RI_Arrangement_Id    INT,
            @version_id                 INT,
			@Recovery					TINYINT,
			@Transaction_Type_Code		VARCHAR(10)

	SELECT TOP 1 @Transaction_Type_Code = TP.code FROM Claim C iNNER JOIN Transaction_Type TP ON C.transaction_type_id = TP.transaction_type_id  WHERE Claim_id = @Claim_id

	IF @Transaction_Type_Code = 'C_SA'
		SET @Recovery = 1
	ELSE IF @Transaction_Type_Code = 'C_RV'
		SET @Recovery = 0
	ELSE
		SET @Recovery = 2

    EXEC Spu_copy_reinsurance_details_to_claim_ri2007 @Claim_id= @claim_id, @is_balance_and_close = NULL, @bOpenClaimNoTrans = NULL, @Recovery = @Recovery

   UPDATE claim_ri_arrangement_line
    SET    reserve = Isnull(reserve, 0) + Isnull(this_reserve, 0),
           payment = Isnull(payment, 0) + Isnull(this_payment, 0),
           salvage = Isnull(salvage, 0) + Isnull(this_salvage, 0),
           recovery = Isnull(recovery, 0) + Isnull(this_recovery, 0),
		   reserve_to_date =ISNULL(reserve_to_date,0) + ISNULL(this_reserve,0),
		   payment_to_date = ISNULL(payment_to_date,0) + ISNULL(this_payment,0),
		   salvage_to_date =ISNULL(salvage_to_date,0) + ISNULL(this_salvage,0),
		   recovery_to_date = ISNULL(recovery_to_date,0) + ISNULL(this_recovery,0)
    WHERE  claim_id = @claim_id

    UPDATE claim_ri_arrangement
    SET    reserve = Isnull(reserve, 0) + Isnull(this_reserve, 0),
           payment = Isnull(payment, 0) + Isnull(this_payment, 0),
           salvage = Isnull(salvage, 0) + Isnull(this_salvage, 0),
           recovery = Isnull(recovery, 0) + Isnull(this_recovery, 0),
		   reserve_to_date =ISNULL(reserve_to_date,0) + ISNULL(this_reserve,0),
		   payment_to_date = ISNULL(payment_to_date,0) + ISNULL(this_payment,0),
		   salvage_to_date =ISNULL(salvage_to_date,0) + ISNULL(this_salvage,0),
		   recovery_to_date = ISNULL(recovery_to_date,0) + ISNULL(this_recovery,0)
    WHERE  claim_id = @claim_id

GO

--*************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_ICCS_6129_Add_Missing_Task'

GO

CREATE PROCEDURE Spu_iccs_6129_add_missing_task @BordereauTransactionId INT
AS
    DECLARE @CashListID AS INT
    DECLARE @CashListItemID AS INT
    DECLARE @BordereauStatusID AS INT -- 4: Recommendation, 5: Authorisation
    DECLARE @SourceID INT
    DECLARE @CreatedByUserID INT

    SELECT @CashListID = CashListKey,
           @CashListItemID = CashListItemKey,
           @BordereauStatusID = BordereauStatusID,
           @SourceID = Branch_source_id,
           @CreatedByUserID = CreatedBy_user_id
    FROM   [etana].[BordereauTransaction]
    WHERE  BordereauTransactionId = @BordereauTransactionId

    DECLARE @CashListTypeID AS INT
    DECLARE @AccountID AS INT
    DECLARE @ShortCode AS CHAR(30)
    DECLARE @OurRef AS VARCHAR(30)
    DECLARE @Description VARCHAR(255)
    DECLARE @Amount NUMERIC(19, 4)
    DECLARE @Date DATETIME

    SELECT @AccountID = CLI.account_id,
           @OurRef = CLI.our_ref,
           @Amount = CLI.amount
    FROM   CashListItem CLI
    WHERE  CLI.cashlistitem_id = @CashListItemID

    SELECT @CashListTypeID = cashlisttype_id
    FROM   CashList
    WHERE  cashlist_id = @CashListID

    SELECT @ShortCode = short_code
    FROM   Account
    WHERE  account_id = @AccountID

    SELECT @Description = 'Payments - Cash / Cheque  - Reference:   - The Amount: '
                          + CONVERT(VARCHAR, @Amount)

    SELECT @Date = Cast(CONVERT(CHAR(8), Getdate(), 112)
                        + ' 23:59:59.99' AS DATETIME)

    DECLARE @p1 INT

    EXEC Spe_pmwrk_task_instance_add
      @pmwrk_task_instance_cnt=@p1 OUTPUT,
      @pmwrk_task_group_id=1,
      @pmwrk_task_id=206,
      @customer=@ShortCode,
      @task_due_date=@Date,
      @pmuser_group_id=1,
      @user_id=NULL,
      @description=@Description,
      @task_status=0,
      @is_urgent=0,
      @date_created=@Date,
      @created_by_id=@CreatedByUserID,
      @last_modified=NULL,
      @modified_by_id=NULL,
      @is_visible=1,
      @workflow_information=NULL,
      @source_id=@SourceID,
      @Is_task_review=0

    EXEC Spu_pmwrk_task_inst_key_add
      @pmwrk_task_instance_cnt=@p1,
      @key_name=N'cashlistitem_id',
      @key_value=@CashListItemID

    EXEC Spu_pmwrk_task_inst_key_add
      @pmwrk_task_instance_cnt=@p1,
      @key_name=N'cashlist_id',
      @key_value=@CashListID

    EXEC Spu_pmwrk_task_inst_key_add
      @pmwrk_task_instance_cnt=@p1,
      @key_name=N'cashlisttype_id',
      @key_value=N'1'

    EXEC Spu_pmwrk_task_inst_key_add
      @pmwrk_task_instance_cnt=@p1,
      @key_name=N'actionkey',
      @key_value=N'approve'

    EXEC Spu_pmwrk_task_inst_key_add
      @pmwrk_task_instance_cnt=@p1,
      @key_name=N'payment_id',
      @key_value=N'0'

GO
--**************************************************************************************************************************


--*************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_Update_TaxCalculation_DataFix'

GO

Create Procedure spu_Update_TaxCalculation_DataFix
(
@insurance_file_cnt INT,
@transaction_type VARCHAR(5)
)
 AS

 BEGIN 

DECLARE @risk_cnt INT ,
@tax_amount money,
@policy_fee_premium numeric(11,4)

select distinct ifi.insurance_file_cnt ,ifi.insurance_ref ,ifi.insurance_file_type_id ,ifi.cover_start_date ,
RS.risk_cnt,D.document_ref 'Document Ref', D.document_date , sum(RS.this_premium) as Vatable_Premium ,ift.description 
INTO #RatingSectionData
from rating_section 
RS JOIN Insurance_file_risk_link IFRL on IFRL.risk_cnt=RS.risk_cnt
JOIN Insurance_file IFI on IFI.insurance_file_cnt=IFRL.insurance_file_cnt
JOIN Document D on IFI.insurance_file_cnt = D.insurance_file_cnt
JOIN Insurance_file_Type ift on ift.insurance_file_type_id = ifi.insurance_file_type_id
 where RS.risk_cnt in(select
risk_id from gis_policy_link GPL 
JOIN EMARCAR_policy_binder EPB on EPB.gis_policy_link_id=GPL.gis_policy_link_id 
JOIN EMARCAR_EMARCAR ECAR on ECAR.EMARCAR_policy_binder_id= EPB.EMARCAR_policy_binder_id)
and IFI.insurance_file_type_id in (2,5,8,9) 
and ifrl.status_flag = 'C'
and IFI.product_id = 9 
and IFI.cover_start_date>'2014-12-31 00:00:00.000'
and RS.country_id = 1
and D.document_ref like 'S%'
and ifi.insurance_file_cnt = @insurance_file_cnt
group by  ifi.insurance_file_cnt, ifi.insurance_ref,ifi.insurance_file_type_id,ifi.cover_start_date, RS.risk_cnt,D.document_ref, D.document_date,ift.description
order by RS.risk_cnt,ifi.insurance_ref

select distinct @risk_cnt =  rsd.risk_cnt from #RatingSectionData rsd
 inner join (select sum(premium) VAT_Premium_Charged,risk_cnt from tax_calculation group by risk_cnt) tc
on rsd.risk_cnt = tc.risk_cnt
where tc.VAT_Premium_Charged <> rsd.Vatable_Premium


  drop table #RatingSectionData

  delete from tax_calculation where risk_cnt = @risk_cnt

  execute spu_Risk_Tax_Select @risk_cnt,0,@transaction_type,@insurance_file_cnt

  update Tax_Calculation set value = (premium * percentage)/100 where risk_cnt = @risk_cnt  

  select @tax_amount = sum(value) from tax_calculation with (nolock) where insurance_file_cnt = @insurance_file_cnt and risk_cnt is not null 

  update insurance_file set tax_amount = @tax_amount where insurance_file_cnt = @insurance_file_cnt

  select @policy_fee_premium = (this_premium + tax_amount) from insurance_file where insurance_file_cnt = @insurance_file_cnt

  update policy_fee_U set fee_premium = @policy_fee_premium where insurance_file_cnt = @insurance_file_cnt
End

GO
--**************************************************************************************************************************

EXECUTE Ddldropprocedure
  'spu_update_insurance_file_type_datafix'

  GO

create procedure spu_update_insurance_file_type_datafix
@insuranceFilecnt int,
@insuranceFileTypeId int

AS
BEGIN

UPDATE IFS
SET IFS.last_trans_type_id= CASE WHEN i.insurance_file_type_id = 4 AND i.insurance_file_status_id = 1 THEN 7
								WHEN i.insurance_file_type_id = 4 AND i.insurance_file_status_id IS NULL THEN 9
								WHEN i.insurance_file_type_id = 3  THEN 10
								WHEN i.insurance_file_type_id = 11  THEN 7
								WHEN i.insurance_file_type_id = 10  THEN 20
							END
FROM Insurance_File_System IFS
inner join insurance_file I
on i.insurance_file_cnt = ifs.insurance_file_cnt
where ifs.insurance_file_cnt = @insuranceFilecnt

UPDATE Insurance_File  
SET     insurance_file_type_id =  CASE  
                        WHEN @insuranceFileTypeId = 3 THEN 2 
                        WHEN @insuranceFileTypeId = 4 THEN 5 
						WHEN @insuranceFileTypeId = 11 THEN 8 
						WHEN @insuranceFileTypeId = 10 THEN 9                         
                    END 
WHERE   insurance_file_cnt = @insuranceFilecnt


END

GO

--**************************************************************************************************************************

EXECUTE DDLDROPPROCEDURE 'spu_Update_Insurance_File_Premium_DataFix'

GO


CREATE proc spu_Update_Insurance_File_Premium_DataFix
@insurance_file_cnt INT
AS
BEGIN

DECLARE @PremiumTotal NUMERIC(19,4)
SELECT @PremiumTotal=SUM(total_this_premium)  from insurance_file_risk_link ifrl INNER JOIN risk r ON r.risk_cnt =ifrl.risk_cnt WHERE insurance_file_cnt =@insurance_file_cnt
and ifrl.status_flag in ('C','D')
UPDATE insurance_file SET this_premium=@PremiumTotal,net_premium=@PremiumTotal WHERE insurance_file_cnt = @insurance_file_cnt

UPDATE insurance_file_risk_link set is_risk_edited=1 where insurance_file_cnt = @insurance_file_cnt and original_risk_cnt IS NOT NULL AND status_flag IN ('C','D')

END




GO

--**************************************************************************************************************************

EXECUTE DDLDROPPROCEDURE 'spu_ValidateRisk_DataFix'

GO


CREATE PROCEDURE dbo.spu_ValidateRisk_DataFix
    @risk_cnt int,
	@insurance_file_cnt int
  
AS

declare @StatusFlag char(1)
declare @originalLineExist int

IF EXISTS
    (
        select NULL from rating_section where risk_cnt = @risk_cnt and original_flag = 1
    )
BEGIN

Set @originalLineExist = 1
END
Else
Set @originalLineExist = 0

Select @StatusFlag = status_flag from insurance_file_risk_link where risk_cnt = @risk_cnt and insurance_file_cnt = @insurance_file_cnt
   
if (@StatusFlag in ('C','D') and @originalLineExist = 0)
BEGIN
Select 1
END
Else
 Select 0

GO

--**************************************************************************************************************************

EXECUTE DDLDROPPROCEDURE 'spu_InsertRatingSectionPeril_DataFix'

GO

CREATE PROCEDURE dbo.spu_InsertRatingSectionPeril_DataFix
(
    @risk_cnt int
 )
AS

BEGIN 

DELETE from rating_section_DataFix WHERE risk_cnt = @risk_cnt 

DELETE from peril_DataFix WHERE risk_cnt = @risk_cnt 


Insert into rating_section_DataFix SELECT * FROM rating_section
WHERE risk_cnt = @risk_cnt and original_flag = 0

Insert into peril_DataFix SELECT * FROM peril WHERE risk_cnt = @risk_cnt 

END

GO

--**************************************************************************************************************************

EXECUTE DDLDROPPROCEDURE 'spu_sir_rating_section_peril_DataFix'

GO


CREATE PROCEDURE spu_sir_rating_section_peril_DataFix
(
@risk_cnt int
)

AS

BEGIN

delete from peril where risk_cnt = @risk_cnt
and rating_section_id in (select rating_section_id from rating_section where risk_cnt = @risk_cnt and original_flag = 0)

delete from rating_section where risk_cnt = @risk_cnt and original_flag = 0

declare @rating_section_id int
declare @totalRecords int
DECLARE @i int = 1

SELECT  @rating_section_id = MAX(rating_section_id) + 1
    FROM    rating_section 
    WHERE   risk_cnt = @risk_cnt

if @rating_section_id is null or @rating_section_id = ''
BEGIN
SET @rating_section_id = 0
END

SELECT  @totalRecords = count(*) from peril_DataFix where risk_cnt = @risk_cnt

WHILE @i <= @totalRecords
BEGIN


 /* Add Rating_Section */
    INSERT INTO Rating_Section
    (
        risk_cnt,
        rating_section_id,
        rating_section_type_id,
        policy_section_type_id,
        sequence_number,
        description,
        rate_type_id,
        annual_rate,
        sum_insured,
        annual_premium,
        this_premium,
        original_flag,
        currency_id,
        country_id,
        state_id,
        is_amended,
        calculated_premium,
        override_reason,
        auto_calculated,
        Earning_Pattern_id
    )
    SELECT    
        risk_cnt,
        @rating_section_id,
        rating_section_type_id,
        policy_section_type_id,
        @rating_section_id,
        description,
        rate_type_id,
        annual_rate,
        sum_insured,
        annual_premium,
        this_premium,
        original_flag,
        currency_id,
        country_id,
        state_id,
        is_amended,
        calculated_premium,
        override_reason,
        auto_calculated,
        Earning_Pattern_id    
	FROM  rating_section_DataFix 
	where risk_cnt = @risk_cnt
	and rating_section_id = @i

INSERT INTO Peril (
        risk_cnt,
        rating_section_id,
        peril_id,
        peril_type_id,
        class_of_business_id,
        sequence_number,
        description,
        sum_insured,
        rating_sum_insured,
        rate_type_id,
        annual_rate,
        annual_premium,
        this_premium,
        coinsured_this_premium,
        coinsured_sum_insured,
        coinsured_commission,
        retained_this_premium,
        retained_sum_insured,
        lead_commission_band,
        sub_commission_band,
        lead_commission_value,
        sub_commission_value,
        tax_group,
        tax_value,
        ri_band,
        xl_band,
        is_premium,
        is_sum_insured,
        is_levy_tax     
            )
    SELECT    risk_cnt,
        @rating_section_id, 
        peril_id,
        peril_type_id,
        class_of_business_id,
        sequence_number,
        description,
        sum_insured,
        rating_sum_insured,
        rate_type_id,
        annual_rate,
        annual_premium,
        this_premium,
        coinsured_this_premium,
        coinsured_sum_insured,
        coinsured_commission,
        retained_this_premium,
        retained_sum_insured,
        lead_commission_band,
        sub_commission_band,
        lead_commission_value,
        sub_commission_value,
        tax_group,
        tax_value,
        ri_band,
        xl_band,
        is_premium,
        is_sum_insured,
        is_levy_tax 
    FROM  peril_DataFix 
	where risk_cnt = @risk_cnt
	and rating_section_id = @i

	SET @i = @i + 1
	SET @rating_section_id = @rating_section_id + 1

	END

	DELETE from peril_DataFix where risk_cnt = @risk_cnt
	DELETE from rating_section_DataFix where risk_cnt = @risk_cnt

END

GO

--*****************************************************************************************************************

EXECUTE DDLDropProcedure 'spu_select_Insurance_FileCnt_DataFix'
GO

CREATE PROCEDURE spu_select_Insurance_FileCnt_DataFix
    @risk_cnt int,
    @cover_start_date date
AS

SELECT
ifi.insurance_file_cnt,
ifi.cover_start_date,
ifi.expiry_date
FROM insurance_file_risk_link ifrl
JOIN insurance_file ifi
ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt
INNER JOIN Insurance_File_Type ift
ON ift.insurance_file_type_id = ifi.insurance_file_type_id 
WHERE ifrl.risk_cnt = @risk_cnt
AND ISNULL(ifi.insurance_file_status_id, 3) in (1,2,3,4,5,6,309)
AND ift.code IN ('POLICY', 'MTA PERM', 'MTAREINS', 'MTAQREINS', 'MTAQUOTE', 'RENEWAL', 'MTACAN')
AND ifi.cover_start_date <=@cover_start_date
ORDER BY insurance_file_cnt DESC

GO

--*************************************************************************************************************

EXECUTE DDLDropProcedure 'spu_add_stats_folder_Datafix'
GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_add_stats_folder_Datafix
    @stats_folder_cnt int OUTPUT,
    @insurance_file_cnt int,
    @user_id int,
    @user_name varchar(255),
    @next_orion_doc_ref varchar(25),  
    @is_out_of_sequence bit = 0,  
    @transfer_date datetime = null,  
    @is_cloned_reverse bit = 0,
	@Original_Doc_Ref VARCHAR(25) = ''
AS  
  
BEGIN  
  
DECLARE  
    @source_id int,  
    @debit_credit char(1),  
    @document_prefix char(3),  
    @document_ref varchar(25),  
    @document_comment varchar(60),  
    @document_date datetime,  
    @accounting_date datetime,  
    @posting_period_year int,  
    @posting_period_number smallint,  
    @premium_total numeric(19, 4),  
    @transaction_type_id int,  
    @transaction_type_code char(10),  
    @transaction_date datetime,  
    @insurance_ref varchar(30),  
    @effective_date datetime,  
    @cover_start_date datetime,  
    @expiry_date datetime,  
    @insurance_holder_cnt int,  
    @insurance_holder_shortname varchar(20),  
    @insurance_holder_name varchar(60),  
    @product_id int,  
    @product_code char(10),  
    @business_type_id smallint,  
    @business_type_code char(10),  
    @account_handler_cnt int,  
    @account_handler_shortname char(20),  
    @branch_id smallint,  
    @branch_code char(10),  
    @currency_code char(10),  
    @agent_cnt int,  
    @agent_shortname varchar(20),  
    @loss_id int,  
    @loss_code varchar(30),  
    @loss_date datetime,  
    @created_by_user_id smallint,  
    @created_by_username varchar(12),  
    @currency_id smallint,  
    @sub_branch_id int,  
    @multi_tree varchar(20),  
    @IsInstalments CHAR(1),  
    @underwriting_year_id int,  
    @post_by_effective_date TINYINT --(RC) PLICO 9-10  
  
/* Set temporary default values */  
SELECT  
    @debit_credit = 'D',  
    @document_prefix = 'SND',  
    @document_ref = 'Doc Ref',  
    @document_comment = 'New Business Premium',  
    @document_date = ISNULL(@transfer_date,Getdate()),  
    @accounting_date = ISNULL(@transfer_date,GetDate()),  
    @premium_total = 0,  
    @transaction_type_id = 1,  
    @transaction_type_code = 'NB',  
    @effective_date = ISNULL(@transfer_date,GetDate()),  
    @loss_id = NULL,  
    @loss_code = NULL,  
    @loss_date = NULL  
  
/* TF180598 - transaction_type details retrieved from Insurance_File_System */
IF @Original_Doc_Ref = ''
SELECT @transaction_type_id = T.transaction_type_id,  
       @transaction_type_code = T.code  
FROM   Insurance_File_System I,  
       Transaction_Type T  
WHERE  I.insurance_file_cnt = @insurance_file_cnt  
AND    T.transaction_type_id = I.last_trans_type_id
ELSE
SELECT @transaction_type_id = transaction_type_id,
	   @transaction_type_code = transaction_type_code
FROM   Stats_Folder
WHERE  document_ref = @Original_Doc_Ref
  
IF @transfer_date is not null  and @transaction_type_code<>'DRI' 
 SELECT @transaction_type_code = 'PT',@transaction_type_id =22  
  
IF @is_cloned_reverse =1  
 SELECT @transaction_type_code = 'DRIC',@transaction_type_id =22  
  
-- RWH (08/08/01) Set document comment dependant on transaction type.  
SELECT @document_comment = ''  
  
If @is_out_of_sequence = 1  
BEGIN  
 IF EXISTS(SELECT NULL FROM mta_insurance_file_link WHERE cancelled_linked_insurance_file_cnt = @insurance_file_cnt) BEGIN  
  SELECT @document_comment =  'Out of Sequence Reversal'  
 END  
 ELSE  
 IF EXISTS(SELECT NULL FROM mta_insurance_file_link WHERE new_linked_insurance_file_cnt = @insurance_file_cnt) BEGIN  
  SELECT @document_comment =  'Out of Sequence Endorsement'
 END  
END  

If @document_comment =  ''  
BEGIN  
SELECT @document_comment =  
    CASE @transaction_type_code  
        WHEN 'MTA' THEN 'MTA Premium'  
        WHEN 'MTC' THEN 'Mid-term Cancellation'  
        WHEN 'MTR' THEN 'Policy Reinstatement'  
        WHEN 'REN' THEN 'Renewal Premium'  
        WHEN 'DRI' THEN 'Deferred Reinsurance'  
        WHEN 'PT' THEN 'Portfolio Transfer'  
  WHEN 'DRIC' THEN 'Deferred Reinsurance Reverse'  
END  
END    
  
SELECT @IsInstalments = 'S'  
  
-- Determine the correct credit / debit status  
SELECT  @debit_credit = CASE WHEN this_premium >= 0 THEN 'D' ELSE 'C' END  
FROM    insurance_file  
WHERE   insurance_file_cnt = @insurance_file_cnt  
  
-- RWH (31/08/01) Set document type dependant on transaction type.  
SELECT @document_prefix =  
    CASE @transaction_type_code  
        WHEN 'MTA' THEN @IsInstalments + 'E' + @debit_credit -- i.e. SED  
        WHEN 'MTC' THEN @IsInstalments + 'E' + @debit_credit -- i.e. SEC  
        WHEN 'MTR' THEN @IsInstalments + 'I' + @debit_credit -- i.e. SID  
        WHEN 'REN' THEN @IsInstalments + 'R' + @debit_credit -- i.e. SRD  
        WHEN 'DRI' THEN 'SDD'  
        WHEN 'PT' THEN 'SPD'  
        WHEN 'DRIC' THEN 'SDR'  
        ELSE @IsInstalments + 'N' + @debit_credit -- i.e. SND  
    END  
  
-- Build the document ref  
SELECT  @document_ref = @document_prefix +  @next_orion_doc_ref  

-- overrided doc comment, transtype to depict OOS; doc ref need to remain as is.  
If @is_out_of_sequence = 1  
BEGIN  
 IF EXISTS(SELECT NULL FROM mta_insurance_file_link WHERE cancelled_linked_insurance_file_cnt = @insurance_file_cnt) BEGIN  
  SELECT @document_comment =  'Out of Sequence Reversal', @transaction_type_code = 'MTA', @transaction_type_id = 9  
 END  
 ELSE  
 IF EXISTS(SELECT NULL FROM mta_insurance_file_link WHERE new_linked_insurance_file_cnt = @insurance_file_cnt) BEGIN  
  SELECT @document_comment =  'Out of Sequence Endorsement', @transaction_type_code = 'MTA', @transaction_type_id = 9  
 END  
END 

-- PWF 31/07/2002 patch the current period to restrict to sub_branch_id  
-- Get details from insurance file  
SELECT @source_id = source_id,  
       @insurance_ref = insurance_ref,  
       @cover_start_date = isnull(@transfer_date,cover_start_date),  
       @expiry_date = expiry_date,  
       @insurance_holder_cnt = insured_cnt,  
       @product_id = product_id,  
       @business_type_id = business_type_id,  
       @account_handler_cnt = account_handler_cnt,  
       @branch_id = branch_id,  
       @currency_id = currency_id,  
       @agent_cnt = lead_agent_cnt,  
       @sub_branch_id = branch_id, -- IFIBCR  
     @underwriting_year_id = underwriting_year_id  
FROM   Insurance_File  
WHERE  insurance_file_cnt = @insurance_file_cnt  
  
-- PWF 03/12/2002 - Reset to sub_branch 1 where multi-tree accounts is not used  
SELECT @multi_tree = value  
FROM   hidden_options  
WHERE  branch_id = 1 AND option_number = 16  
  
IF (ISNULL(@multi_tree, '0') <> '1')  
    SELECT @sub_branch_id = 1  
-- PWF 03/12/2002 END  
  
--(RC) 15 Dec 2006 IH - User Defined Period Posting - START  
SELECT @posting_period_number=posting_period_id FROM Insurance_File WHERE insurance_file_cnt = @insurance_file_cnt  
  
IF @posting_period_number IS NULL BEGIN  
  
  --(RC) PLICO 9-10 START  
  SELECT  @post_by_effective_date = value  
  FROM    system_options  
  WHERE   branch_id = 1  
  AND     option_number = 5038  
  
  IF ISNULL(@post_by_effective_date, 0) = 1 BEGIN  
  
    IF @cover_start_date > getdate() BEGIN  
      SELECT @effective_date = @cover_start_date  
    END  
    IF @cover_start_date <= getdate() BEGIN  
      SELECT @effective_date = getdate()  
    END  
  
 -- Get posting period number  
 CREATE TABLE #tempTable (period_id int, year_name varchar(20), period_name varchar(15))  
 INSERT INTO #tempTable EXEC spu_ACT_Do_GetPeriodForDate @source_id, @effective_date, @sub_branch_id  
 SELECT @posting_period_number = period_id FROM #tempTable  
 DROP TABLE #tempTable  
 SELECT @posting_period_number  
  
  END  
  ELSE  
  BEGIN  
    SELECT @posting_period_number = current_period_id  
    FROM   ledger  
    WHERE  ledger_short_name = 'SA'  
    AND    sub_branch_id = @sub_branch_id  
  END  
  --(RC) PLICO 9-10 END  
  
END  
--(RC) 15 Dec 2006 IH - User Defined Period Posting - END  
  
SELECT @posting_period_year = datepart(year, min(period_end_date))  
FROM   period  
WHERE  year_name = (SELECT year_name FROM period WHERE Period_id = @posting_period_number)  
AND    sub_branch_id = @sub_branch_id  
-- JMK END  
-- PWF 31/07/2002 END  
  
SELECT  @insurance_holder_shortname = shortname,  
    @insurance_holder_name = name  
FROM    Party  
WHERE   party_cnt = @insurance_holder_cnt  
  
SELECT  @account_handler_shortname = shortname  
FROM    Party  
WHERE   party_cnt = @account_handler_cnt  
  
SELECT  @product_code = code  
FROM    Product  
WHERE   product_id = @product_id  
  
SELECT  @business_type_code = code  
FROM    Business_Type  
WHERE   business_type_id = @business_type_id  
  
SELECT  @branch_code = code  
FROM    Branch  
WHERE   branch_id = @branch_id  
  
SELECT  @currency_code = iso_code  
FROM    Currency  
WHERE   currency_id = @currency_id  
  
SELECT  @agent_shortname = shortname  
FROM    Party  
WHERE   party_cnt = @agent_cnt  
  
/* Set transaction date */  
SELECT @transaction_date = ISNULL(@transfer_date,GetDate())  
  
/* Insert the Stats Folder */  
INSERT INTO Stats_Folder (  
              source_id,  
              debit_credit,  
              document_ref,  
              document_comment,  
              document_date,  
              accounting_date,  
              posting_period_year,  
              posting_period_number,  
              premium_total,  
              transaction_type_id,  
              transaction_type_code,  
              transaction_date,  
              insurance_file_cnt,  
              insurance_ref,  
              effective_date,  
              cover_start_date,  
              expiry_date,  
              insurance_holder_cnt,  
              insurance_holder_shortname,  
              insurance_holder_name,  
              product_id,  
              product_code,  
              business_type_id,  
              business_type_code,  
              account_handler_cnt,  
              account_handler_shortname,  
              branch_id,  
              branch_code,  
              currency_code,  
         agent_cnt,  
              agent_shortname,  
              loss_id,  
              loss_code,  
              loss_date,  
              created_by_user_id,  
              created_by_username,  
        underwriting_year_id)  
VALUES       ( 
              @source_id,  
              @debit_credit,  
              @document_ref,  
              @document_comment,  
              @document_date,  
              @accounting_date,  
              @posting_period_year,  
              @posting_period_number,  
              @premium_total,  
              @transaction_type_id,  
              @transaction_type_code,  
              @transaction_date,  
              @insurance_file_cnt,  
              @insurance_ref,  
              @effective_date,  
              @cover_start_date,  
              @expiry_date,  
              @insurance_holder_cnt,  
              @insurance_holder_shortname,  
              @insurance_holder_name,  
              @product_id,  
              @product_code,  
              @business_type_id,  
              @business_type_code,  
              @account_handler_cnt,  
              @account_handler_shortname,  
              @branch_id,  
              @branch_code,  
              @currency_code,  
              @agent_cnt,  
              @agent_shortname,  
              @loss_id,  
              @loss_code,  
              @loss_date,  
              @user_id,  
              @user_name,  
        @underwriting_year_id)  
  
/* Return the Count of the Record Added */  
SELECT @stats_folder_cnt = @@IDENTITY  
  
  
END  
GO
--**************************************************************************************************************************
EXECUTE Ddldropprocedure
  'spu_CLM_Get_Transaction_Code'
  GO

  CREATE PROCEDURE spu_CLM_Get_Transaction_Code  
  
@claim_id int  
  
AS  
BEGIN  

 SELECT code from transaction_type t inner join CLaim c
 on t.transaction_type_id = c.transaction_type_id
 where c.claim_id =  @claim_id

 END  

  GO

--**************************************************************************************************************************
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Copy_Stats_for_Cloned_Reversal_DataFix'
GO

CREATE PROCEDURE spu_Copy_Stats_for_Cloned_Reversal_DataFix
@ClonedInsuranceFileCnt  INT,
@StatsFolderCnt INT
AS  
Declare @oldStatsFolderCnt INT
SELECT @oldStatsFolderCnt=MAX(Stats_Folder_cnt) from Stats_Folder WHERE insurance_file_cnt=@ClonedInsuranceFileCnt  
and Stats_Folder_cnt NOT IN (@StatsFolderCnt) and loss_id is null and transaction_type_code not like 'DRI%'
  


Insert into stats_detail (stats_folder_cnt,
stats_detail_id,
stats_detail_type,
risk_id,
risk_type_id,
risk_type_code,
peril_id,
peril_description,
peril_type_id,
peril_type_code,
policy_section_type_id,
policy_section_type_code,
class_of_business_id,
class_of_business_code,
tax_type_id,
tax_type_code,
tax_value,
ri_party_cnt,
ri_shortname,
ri_party_type,
ri_share_percent,
ri_agreement_code,
annual_premium,
currency_code,
currency_rate,
this_premium_original,
this_premium_home,
commission_percent,
lead_commission_value_home,
sub_commission_value_home,
sum_insured_home,
sum_insured_currency_code,
sum_insured_change,
transaction_ledger_id,
transaction_account_id,
account_type_code,
ceded_ref,
cover_share_percent,
sum_insured_total,
charges_total,
taxes_total,
recoveries_total,
commission_excluded,
withholding_tax_excluded,
purchase_order_no,
purchase_invoice_no,
stats_version,
this_premium_system,
lead_commission_value_system,
sub_commission_value_system,
sum_insured_system,
is_commission_modified,
original_flag,
cover_to_date,
Claim_RI_Only_Amendment,
Earning_Pattern_id,
ri_arrangement_line_Id)

Select @StatsFolderCnt, stats_detail_id, stats_detail_type, risk_id, risk_type_id, risk_type_code, peril_id, peril_description, 
peril_type_id, peril_type_code, policy_section_type_id, policy_section_type_code, 
class_of_business_id, class_of_business_code, tax_type_id, tax_type_code, 0, ri_party_cnt, ri_shortname, 
ri_party_type, ri_share_percent, ri_agreement_code, annual_premium, currency_code, currency_rate, 0, 0, 0, 0, 0,
0, sum_insured_currency_code, 0, transaction_ledger_id, transaction_account_id, account_type_code, 
ceded_ref, cover_share_percent, null, null, null, null, null, null, null, null, stats_version, 0, 0, 
0, 0, is_commission_modified,  
original_flag,cover_to_date,Claim_RI_Only_Amendment,Earning_Pattern_id,ri_arrangement_line_Id   
FROM stats_detail 
Where stats_folder_cnt = @oldStatsFolderCnt and stats_detail_type in ('GRS')
  
Insert Into Stats_Detail
([stats_folder_cnt]
           ,[stats_detail_id]
           ,[stats_detail_type]
           ,[risk_id]
           ,[risk_type_id]
           ,[risk_type_code]
           ,[peril_id]
           ,[peril_description]
           ,[peril_type_id]
           ,[peril_type_code]
           ,[policy_section_type_id]
           ,[policy_section_type_code]
           ,[class_of_business_id]
           ,[class_of_business_code]
           ,[tax_type_id]
           ,[tax_type_code]
           ,[tax_value]
           ,[ri_party_cnt]
           ,[ri_shortname]
           ,[ri_party_type]
           ,[ri_share_percent]
           ,[ri_agreement_code]
           ,[annual_premium]
           ,[currency_code]
           ,[currency_rate]
           ,[this_premium_original]
           ,[this_premium_home]
           ,[commission_percent]
           ,[lead_commission_value_home]
           ,[sub_commission_value_home]
           ,[sum_insured_home]
           ,[sum_insured_currency_code]
           ,[sum_insured_change]
           ,[transaction_ledger_id]
           ,[transaction_account_id]
           ,[account_type_code]
           ,[ceded_ref]
           ,[cover_share_percent]
           ,[sum_insured_total]
           ,[charges_total]
           ,[taxes_total]
           ,[recoveries_total]
           ,[commission_excluded]
           ,[withholding_tax_excluded]
           ,[purchase_order_no]
           ,[purchase_invoice_no]
           ,[stats_version]
           ,[this_premium_system]
           ,[lead_commission_value_system]
           ,[sub_commission_value_system]
           ,[sum_insured_system]
           ,[is_commission_modified]
           ,[original_flag]
           ,[cover_to_date]
           ,[Claim_RI_Only_Amendment]
           ,[Earning_Pattern_id]
           ,[ri_arrangement_line_Id])
Select @StatsFolderCnt,stats_detail_id,stats_detail_type,risk_id,risk_type_id,risk_type_code,
		peril_id,peril_description,peril_type_id,peril_type_code,policy_section_type_id,policy_section_type_code,
		class_of_business_id,class_of_business_code,tax_type_id,tax_type_code,-tax_value,ri_party_cnt,ri_shortname,
		ri_party_type,ri_share_percent,ri_agreement_code,-annual_premium,currency_code,currency_rate,
		-this_premium_original,-this_premium_home,commission_percent,-lead_commission_value_home,-sub_commission_value_home,
		-sum_insured_home,sum_insured_currency_code,-sum_insured_change,transaction_ledger_id,transaction_account_id,account_type_code,ceded_ref,
		cover_share_percent,-sum_insured_total,-charges_total,-taxes_total,-recoveries_total,commission_excluded,withholding_tax_excluded,purchase_order_no,
		purchase_invoice_no,stats_version,-this_premium_system,-lead_commission_value_system,-sub_commission_value_system,-sum_insured_system,is_commission_modified,
		original_flag,cover_to_date,Claim_RI_Only_Amendment,Earning_Pattern_id,ri_arrangement_line_Id
 from Stats_Detail  WHERE Stats_Folder_cnt=@oldStatsFolderCnt and (ri_arrangement_line_Id is not null or stats_detail_type='NET')
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

--**************************************************************************************************************************
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Copy_TransExportDetail_for_Cloned_Reversal_DataFix'
GO

Create Procedure spu_Copy_TransExportDetail_for_Cloned_Reversal_DataFix
@ClonedInsuranceFileCnt  INT,  
@transaction_export_folder_cnt INT,
@Stats_folder_cnt INT
AS

Declare @Old_transaction_export_folder_cnt INT

Select @Old_transaction_export_folder_cnt = MAX(transaction_export_folder_cnt) from Transaction_Export_Folder where insurance_file_cnt = @ClonedInsuranceFileCnt
And transaction_export_folder_cnt Not In (@transaction_export_folder_cnt) and loss_id Is Null And transaction_type_code not like 'DRI%'

Insert Into Transaction_Export_Detail
Select @transaction_export_folder_cnt,
      transaction_export_detail_id,
      Case When transdetail_type_code ='GROSS' Then 0 Else -transaction_amount END,
      transaction_ledger_code,
      account_type_code,
      transaction_account_key,
      ceded_ref,
      cover_share_percent,
      Case When transdetail_type_code ='GROSS' Then 0 Else -sum_insured_total END,
      Case When transdetail_type_code ='GROSS' Then 0 Else -charges_total END,
      Case When transdetail_type_code ='GROSS' Then 0 Else -taxes_total END,
      -recoveries_total,
      -commission_excluded,
      -withholding_tax_excluded,
      mapping_code,
      spare,
      purchase_order_no,
      purchase_invoice_no,
      base_transaction_amount,
      base_taxes_amount,
      suspended,
      release_to_income,
      release_account_code,
      transdetail_type_code,
      tax_group_id,
      tax_band_id,
      manually_released,
      released_on_full_settlement,
      released_for_whole_posting,
      released_on_policy_effective ,
     fee_type
From Transaction_Export_Detail 
Where transaction_export_folder_cnt = @Old_transaction_export_folder_cnt 
and (transdetail_type_code Like 'REIN%' Or mapping_code in (SELECT ri_shortname
															FROM stats_detail
															WHERE stats_folder_cnt = @Stats_folder_cnt
															AND Stats_detail_type IN ('TAN', 'TAX')
															AND tax_type_code IS NOT NULL
															GROUP BY tax_type_code, ri_shortname, stats_detail_type)
		or transdetail_type_code Like ('GROSS'))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
--**************************************************************************************************************************

SET QUOTED_IDENTIfIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_RI_Arrangement_copy_ForUtility'
GO
CREATE PROCEDURE spu_RI_Arrangement_copy_ForUtility  
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
  @ri_band_id int
  
  SET @transaction_type = @Trans_type  
  
 IF @RI_effective_date IS NULL  
 SELECT @RI_effective_date = GETDATE()  
  
 Select @RI2007Enabled=ISNull(value,0) from hidden_options where option_number=88  
  
    -- Select and validate original risk  
    Select  @original_risk_cnt = Null, @original_insurance_file_cnt = NULL  
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
  
    IF @original_version_id is null  
     SET @original_version_id=1  
  
 Select @original_insurance_file_cnt = insurance_file_cnt  
  From    insurance_file_risk_link  
  Where   risk_cnt = @original_risk_cnt  
 AND status_flag NOT IN ('U') -- must be a copied risk  
  
 Set @oos_mta_cancelled_policy_id = 0  
 Set @oos_mta_cancelled_risk_id = 0  
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
	 AND Type NOT IN('F','FX')
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
    Where ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1)  
  End  
   Delete From ri_arrangement_Line  
     Where ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1)  
   Delete From ri_arrangement Where risk_cnt = @risk_cnt and version_id=@version_id and original_flag =1  
  
 END  
 Else  
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
  
    -- Arrangement cursor  
    Declare Arrangement_Cursor Cursor Fast_Forward For  
        Select  ri_arrangement_id,  
                Case When Exists (  
                    Select  NULL  
                    From    ri_arrangement_line ral  
                    Where   ral.type IN ('F','FX')  
                    And     ral.ri_arrangement_id = ra.ri_arrangement_id) Then 1 Else 0 End has_fac,  
                1 original_flag,  
    @original_risk_cnt,
	ri_band_id
        From    ri_arrangement ra  
        Where   risk_cnt = @original_risk_cnt  
        And    ( original_flag = 0 Or ( Not Exists (SELECT NULL from RI_Arrangement where risk_cnt=@original_risk_cnt and ri_band_id=ra.ri_band_id and original_flag=0) And  (@Trans_type = 'MTCR' OR @is_oos_reversal =1) ))  
        And     version_id=  @original_version_id  
  
    Open Arrangement_Cursor  
    Fetch Next From Arrangement_Cursor Into @old_ri_arrangement_id, @has_fac, @original_flag, @original_risk_cnt,@ri_band_id  
  
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
			
		IF NOT EXISTS (SELECT NULL from RI_Arrangement where risk_cnt = @risk_cnt)
		BEGIN
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
                Case When @original_flag = 0 Then 0 Else Round(sum_insured * @combined_rate, 2) End,  
                Case When @original_flag = 0 Then 0 Else Round(premium * @combined_rate * @pro_rata_rate, 2) End,  
                @original_flag,  
                Case When @original_flag = 0 Then 0 Else is_modified End,  
                extended_limit_amount,  
    is_extended_limit_applied,  
    prop_calc_method_id ,  
    xol_calc_method_id, @version_id,  
    Case When @trans_type ='PT' Then 2 Else Case When @trans_type ='DRI' THEN RI_Version_Type_id Else 1 End END,  
                Case When @trans_type ='PT' Then @RI_Effective_Date Else @effective_date End,  
    xol_ri_model_id ,  
 Case When @original_flag = 1 Then isnull(ri_override_reason_id,0)  Else 0 End  
        From    ri_arrangement  
        Where   risk_cnt = @original_risk_cnt  
        And     ri_arrangement_id = @old_ri_arrangement_id  
        And     ((original_flag = 0) Or ( (@Trans_type = 'MTCR' OR @is_oos_reversal =1) AND original_flag = 1))  
  
        -- Get new id  
        Select  @new_ri_arrangement_id = @@Identity  
		
		END
		
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
   Where ri_arrangement_id =@new_ri_arrangement_id  
    END  
   Delete from ri_arrangement_line where ri_arrangement_id=@new_ri_arrangement_id  
  
   SELECT @Date_for_Treaty_XOL_Calculation=Date_for_Treaty_XOL_Calculation_id  
   From ri_band where ri_band_id=(Select ri_band_id From ri_arrangement Where ri_arrangement_id = @old_ri_arrangement_id)  
  
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
  
      If Exists(Select NULL from ri_arrangement_line  
     Where ri_arrangement_line_id=@old_ri_arrangement_line_id  
      And type = 'TX') And @Date_for_Treaty_XOL_Calculation = 2 And @RI2007Enabled=1  
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
         is_obligatory,grouping)  
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
                           Round(line_limit * Abs(@combined_rate), 2),  
                           Round(lower_limit * Abs(@combined_rate), 2),  
                           Round(sum_insured * @combined_rate, 2),  
                           Round(premium_value * @combined_rate * @pro_rata_rate, 2),  
                     Round(commission_value * @combined_rate * @pro_rata_rate, 4),  
                           Case When @original_flag = 0 Then 0 Else Round(premium_tax * @combined_rate * @pro_rata_rate, 2) End,  
                           Case When @original_flag = 0 Then 0 Else Round(commission_tax * @combined_rate * @pro_rata_rate, 4) End,  
                        is_commission_modified,Retained,  
               is_obligatory,grouping  
                   From    ri_arrangement_line  
                   Where   ri_arrangement_line_id = @old_ri_arrangement_line_id  
                   And    (@original_flag = 1 Or type = 'F')  
  
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
         is_obligatory,grouping)  
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
                           Round(line_limit * Abs(@combined_rate), 2),  
                           Round(lower_limit * Abs(@combined_rate), 2),  
             Round(sum_insured * @combined_rate, 2),  
                           Round(premium_value * @combined_rate * @pro_rata_rate, 2),  
                           Round(commission_value * @combined_rate * @pro_rata_rate, 4),  
                           Case When @original_flag = 0 Then 0 Else Round(premium_tax * @combined_rate * @pro_rata_rate, 2) End,  
                           Case When @original_flag = 0 Then 0 Else Round(commission_tax * @combined_rate * @pro_rata_rate, 4) End,  
                           is_commission_modified,Retained,  
         is_obligatory,grouping  
                   From    ri_arrangement_line  
                   Where   ri_arrangement_line_id = @old_ri_arrangement_line_id  
                   And    (@original_flag = 1 Or type = 'F')  
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
       is_obligatory)  
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
                      Round(line_limit * Abs(@combined_rate), 2),  
                      Round(lower_limit * Abs(@combined_rate), 2),  
                      Round(sum_insured * @combined_rate, 2),  
       Case When (@original_flag = 0 and Rtrim(@trans_type) ='MTC') Then  
       0 Else  
        Case When (@original_flag = 0 and @oos_mta_cancelled_risk_id > 0) Then  
        premium_value Else  
         Case When (@original_flag = 0) Then (premium_value * @combined_rate * @new_pro_rata_rate)  
                Else (premium_value * @combined_rate * @pro_rata_rate) End End End,  
       Case When (@original_flag = 0 and Rtrim(@trans_type) ='MTC') Then  
       0 Else  
        Case When (@original_flag = 0 and @oos_mta_cancelled_risk_id > 0) Then  
        commission_value Else  
         Case When (@original_flag = 0) Then Round(commission_value * @combined_rate * @new_pro_rata_rate, 2)  
                Else Round(commission_value * @combined_rate * @pro_rata_rate, 4) End End End,  
                      Case When @original_flag = 0 Then 0 Else Round(premium_tax * @combined_rate * @pro_rata_rate, 2) End,  
                      Case When @original_flag = 0 Then 0 Else Round(commission_tax * @combined_rate * @pro_rata_rate, 4) End,  
                      is_commission_modified,Retained,participation_percent,grouping,  
       is_obligatory  
              From    ri_arrangement_line  ril  
        Where   ri_arrangement_line_id = @old_ri_arrangement_line_id  
                And    (@original_flag = 1 Or type IN ('F','FX')  Or (type = 'T' and ISNULL(line_limit,0) = 0 )  )    --If T manually added then it does not have line_limit  
  
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
       Round(premium * @combined_rate * @pro_rata_rate, 2)  
      Else  
          Round(-premium * @combined_rate * @pro_rata_rate, 2)  
      END,  
                        percentage,  
      Case  When transtype='TTRIFP' OR transtype='TTRIFC' Then  
        Round(value * @combined_rate * @pro_rata_rate, 2)  
      Else  
           Round(-value * @combined_rate * @pro_rata_rate, 2)  
      END,  
                        is_value,  
                        is_manually_changed,  
                        calc_basis,  
                        Round(-basis_value * @combined_rate, 2),  
                        Round(-sum_insured * @combined_rate, 2),  
                        sum_insured_rounded,  
                        allow_tax_credit,  
                        Round(-original_sum_insured * @combined_rate, 2),  
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
       Where ral.type IN ('F','FX')  
        And ral.ri_arrangement_id = @oos_mta_cancelled_riarrangement_id) AND  @transaction_type<>'DRI'  
    Begin  
     Set @has_fac = 1  
     Set @old_ri_arrangement_id = @oos_mta_cancelled_riarrangement_id  
     If @oos_mta_cancelled_risk_id > 0  
      Set @original_risk_cnt = @oos_mta_cancelled_risk_id  
 End  
 ELSE IF EXISTS (Select NULL From ri_arrangement_line ral  
       Where ral.type IN ('F','FX')  
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
        End Else Begin  
            -- If this is the last record we'll go into an infinite loop if  
            -- we don't manually reset the original_flag  
            Select @original_flag = 1  
		/*this is to cater for disabled pro rata option for output tables*/
		if @RI2007Enabled=1 AND @pro_rata_rate=1 AND @Trans_type in ('MTA','MTC','MTR')
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

GO
--**************************************************************************************************************************


EXECUTE Ddldropprocedure
  'Spu_claim_recalculate_reinsurance_NOT2007'

GO

CREATE PROCEDURE Spu_claim_recalculate_reinsurance_NOT2007 @Claim_id INT
AS
    DECLARE @original_claim_id          INT,
            @Reserve                    MONEY,
            @Payment                    MONEY,
            @ri_arrangement_line_id     INT,
            @max_ri_arrangement_line_id INT,
            @Claim_RI_Arrangement_Id    INT,
            @version_id                 INT

    UPDATE claim_ri_arrangement
    SET    reserve = Isnull(reserve, 0) - Isnull(this_reserve, 0),
           payment = Isnull(payment, 0) - Isnull(this_payment, 0),
           salvage = Isnull(salvage, 0) - Isnull(this_salvage, 0),
           recovery = Isnull(recovery, 0) - Isnull(this_recovery, 0)
    WHERE  claim_id = @claim_id

    DELETE FROM Claim_RI_Arrangement_line_Broker_Participants
    WHERE  claim_ri_arrangement_line_id IN (SELECT claim_ri_arrangement_line_id
                                            FROM   Claim_Ri_arrangement_Line
                                            WHERE  claim_id = @Claim_id)

    DELETE FROM Claim_Ri_arrangement_Line
    WHERE  claim_id = @Claim_id

    DELETE FROM Claim_Ri_arrangement
    WHERE  claim_id = @Claim_id

    EXEC spu_Copy_Reinsurance_Details_To_Claim_Utility  
      @claim_id

    EXEC spu_Copy_Reinsurance_Details_To_Claim_Utility  
      @claim_id

    UPDATE claim_ri_arrangement_line
    SET    reserve = Isnull(reserve, 0) + Isnull(this_reserve, 0),
           payment = Isnull(payment, 0) + Isnull(this_payment, 0),
           salvage = Isnull(salvage, 0) + Isnull(this_salvage, 0),
           recovery = Isnull(recovery, 0)
                      + Isnull(this_recovery, 0)
    WHERE  claim_id = @claim_id

    UPDATE claim_ri_arrangement
    SET    reserve = Isnull(reserve, 0) + Isnull(this_reserve, 0),
           payment = Isnull(payment, 0) + Isnull(this_payment, 0),
           salvage = Isnull(salvage, 0) + Isnull(this_salvage, 0),
           recovery = Isnull(recovery, 0)
                      + Isnull(this_recovery, 0)
    WHERE  claim_id = @claim_id

GO
 
--**************************************************************************************************************************

SET quoted_identifier ON 

GO 

SET ansi_nulls ON 

GO 

EXECUTE Ddldropprocedure 'spu_RI_Arrangement_make_RI2007_ForUtility' 

GO 

CREATE PROCEDURE spu_RI_Arrangement_make_RI2007_ForUtility @ri_arrangement_id     INT, 
                                                @risk_type_id          INTEGER, 
                                                @ri_band_id            INT, 
                                                @effective_date        DATETIME, 
                                                @allow_deferred        TINYINT, 
                                                @sum_insured           MONEY, 
                                                @premium               MONEY, 
                                                @line_limit            MONEY, 
                                                @is_auto_reinsured     TINYINT, 
                                                @source_id             INT, 
                                                @policy_currency_id    SMALLINT, 
                                                @policy_currency_rate  FLOAT, 
                                                @Trans_type            VARCHAR(5) = '', 
                                                @NBExtended_Is_Enabled TINYINT = 0 ,
                                                @prop_effective_date   DATETIME = NULL,
												@Original_Risk_Cnt	   INT = 0
AS 
  DECLARE @model_currency_id       SMALLINT,  
          @model_currency_rate     FLOAT,  
          @ri_model_id             INT,  
          @reinsurance_type        VARCHAR(3),  
          @Extended_Limits_Enabled TINYINT,  
          @ri_model_type    TINYINT,
          
          @xol_model_currency_id       SMALLINT,  
          @xol_model_currency_rate     FLOAT,  
          @xol_ri_model_id             INT,
   	  @xol_ri_model_type    TINYINT,
   	  @Date_for_Treaty_XOL_Calculation INT,
   	  @Date_for_Prop_Calculation INT,
   	  @Is_original INT
  
  SET @reinsurance_type = 'T'  
  
  -- If this Risk is automatically reinsured  
  IF @is_auto_reinsured = 1  
    BEGIN  
        -- This risk is auto-reinsured, so delete all lines, new and original,  
        -- except the new facultative ri as that is never automatic.  
        DELETE tax_calculation  
        WHERE  ri_arrangement_line_id IN (SELECT ri_arrangement_line_id  
                                          FROM   ri_arrangement_line  
                                          WHERE  
               ri_arrangement_id = @ri_arrangement_id  
               AND TYPE <> 'F')  
  
		--if not exists(select * From ri_arrangement_Line_Archive RIALAr
		--			Inner Join ri_arrangement ria ON ria.ri_arrangement_id = RIALAr.ri_arrangement_id
		--			Where ria.ri_arrangement_id IN (Select ri_arrangement_id From ri_arrangement RI
		--											inner join insurance_file_risk_link ifrl on RI.risk_cnt=ifrl.risk_cnt
		--											inner join Insurance_File IFL on ifrl.insurance_file_cnt=ifl.insurance_file_cnt
		--											Where ifl.insurance_file_type_id in (2,5,8,9) and RI.ri_arrangement_id=@ri_arrangement_id))
		--Begin
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
			 Is_Obligatory,
			 ri_arrangement_line_Version_id,
			 created_date)  
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
			 Is_Obligatory,
			 (SELECT MAX(ISNULL(ri_arrangement_line_Version_id,0))+1 FROM RI_Arrangement_Line_Archive WHERE  ri_arrangement_id = @ri_arrangement_id),
			 GETDATE() 
			 FROM RI_Arrangement_Line 
			 Where ri_arrangement_id = @ri_arrangement_id AND TYPE NOT IN( 'F', 'FX' )
		--End
		DELETE ri_arrangement_line   WHERE  ri_arrangement_id = @ri_arrangement_id   AND TYPE NOT IN( 'F', 'FX' )

	END
  IF EXISTS (SELECT NULL  
             FROM   ri_arrangement_line  
             WHERE  ri_arrangement_id = @ri_arrangement_id  
                    AND TYPE NOT IN ( 'F', 'FX' ))  
    RETURN  
      --Rohit Code change start for Generic Utility
DECLARE @Original_Flag TINYINT
SELECT @Original_Flag = Original_Flag FROM ri_arrangement WHERE ri_arrangement_id = @ri_arrangement_id 

      -- Get the best RI Model for this ri_band and risk_type  
      SELECT @ri_model_id = NULL  
	  SELECT @xol_ri_model_id = NULL  	

IF @Original_Flag = 1
BEGIN

SELECT 
	@ri_model_id = RIA.ri_model_id,
	@xol_ri_model_id = RIA.xol_ri_model_id   
FROM RI_Arrangement RIA
WHERE Risk_Cnt = @Original_Risk_Cnt
AND Original_Flag = 0
AND ri_band_id = @ri_band_id  

SELECT @model_currency_id = currency_id,@ri_model_type = ISNULL(ri_model_type,0) from RI_Model where ri_model_id = @ri_model_id
SELECT @xol_model_currency_id = currency_id,@xol_ri_model_type = ISNULL(ri_model_type,0) from RI_Model where ri_model_id = @xol_ri_model_id
END
ELSE
BEGIN
  SELECT @ri_model_id = rmu.ri_model_id,  
         @model_currency_id = currency_id,  
         @ri_model_type = ISNULL(rm.ri_model_type,0)  
  FROM   risk_type_ri_model_usage rmu  
         JOIN ri_model rm  
           ON rm.ri_model_id = rmu.ri_model_id  
  WHERE  rmu.risk_type_id = @risk_type_id  
         AND rmu.ri_band = @ri_band_id  
         AND rmu.is_deleted = 0  
         AND rmu.effective_date <= @prop_effective_date  
         AND ( rmu.expiry_date >= @prop_effective_date  
                OR Isnull(rmu.expiry_date, '1899.12.29') = '1899.12.29' )  
         AND ( rm.ri_model_type = 0  
                OR ( rm.ri_model_type = 2  
                     AND @allow_deferred = 1 ) OR (rm.ri_model_type = 4) )  
         AND rm.is_deleted = 0  
         AND rm.effective_date <= @prop_effective_date  
         AND ( rm.expiry_date >= @prop_effective_date  
                OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )  
  ORDER  BY rm.ri_model_type DESC,  
            -- give priority to none-deferred models  
            rmu.effective_date ASC -- give priority to newer models  
  
 SELECT @xol_ri_model_id = rmu.ri_model_id,  
         @xol_model_currency_id = currency_id,  
         @xol_ri_model_type = ISNULL(rm.ri_model_type,0)  
  FROM   risk_type_ri_model_usage rmu  
         JOIN ri_model rm  
           ON rm.ri_model_id = rmu.ri_model_id  
  WHERE  rmu.risk_type_id = @risk_type_id  
         AND rmu.ri_band = @ri_band_id  
         AND rmu.is_deleted = 0  
         AND rmu.effective_date <= @effective_date  
         AND ( rmu.expiry_date >= @effective_date  
                OR Isnull(rmu.expiry_date, '1899.12.29') = '1899.12.29' )  
         AND ( rm.ri_model_type = 0  
                OR ( rm.ri_model_type = 2  
                     AND @allow_deferred = 1 ) OR (rm.ri_model_type = 4) )  
         AND rm.is_deleted = 0  
         AND rm.effective_date <= @effective_date  
         AND ( rm.expiry_date >= @effective_date  
                OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )  
  ORDER  BY rm.ri_model_type DESC,  
            -- give priority to none-deferred models  
            rmu.effective_date ASC -- give priority to newer models  
 END

-- If model is not specified for band, check for a system default model  
  IF @ri_model_id IS NULL  
    SELECT @ri_model_id = rm.ri_model_id,  
           @model_currency_id = currency_id  
    FROM   ri_model rm  
    WHERE  rm.ri_model_type = 1 -- Default  
           AND rm.is_deleted = 0  
           AND rm.effective_date <= @prop_effective_date  
           AND ( rm.expiry_date >= @prop_effective_date  
                  OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )  
    ORDER  BY rm.effective_date -- give priority to newer models  

	-- If model is not specified for band, check for a system default model  
  IF @xol_ri_model_id IS NULL  
    SELECT @xol_ri_model_id = rm.ri_model_id,  
           @xol_model_currency_id = currency_id  
    FROM   ri_model rm  
    WHERE  rm.ri_model_type = 1 -- Default  
           AND rm.is_deleted = 0  
           AND rm.effective_date <= @effective_date  
           AND ( rm.expiry_date >= @effective_date  
                  OR Isnull(rm.expiry_date, '1899.12.29') = '1899.12.29' )  
    ORDER  BY rm.effective_date -- give priority to newer models  
  -- Update the arrangement  
  UPDATE ri_arrangement  
  SET    xol_ri_model_id = @xol_ri_model_id , 
		 ri_model_id = @ri_model_id  
  WHERE  ri_arrangement_id = @ri_arrangement_id  
   
   SELECT @Date_for_Treaty_XOL_Calculation =
               ISNULL(@Date_for_Treaty_XOL_Calculation ,date_for_treaty_xol_calculation_id),
               @Date_for_Prop_Calculation=
      ISNULL(@Date_for_Prop_Calculation ,Proportional_RI_Cal_Method )
        FROM   ri_band
        WHERE  ri_band_id = @ri_band_id

   UPDATE RI_Arrangement
   SET xol_calc_method_id  = @Date_for_Treaty_XOL_Calculation,
   prop_calc_method_id = @Date_for_Prop_Calculation
   WHERE  ri_arrangement_id = @ri_arrangement_id
 -- E007  
 IF @xol_ri_model_type =4  OR @ri_model_type = 4
 BEGIN  
  UPDATE  ri_arrangement  
  SET     Cloned = 1  
  WHERE   ri_arrangement_id = @ri_arrangement_id  
 END  
 ELSE  
 BEGIN  
  UPDATE  ri_arrangement  
  SET     Cloned = 0  
  WHERE   ri_arrangement_id = @ri_arrangement_id  
 END  
  
  -- WPR55? Read the options only in case of NB or Ren else it will be copied by Copy SP  
  IF @Trans_type = 'NB'  
      OR @Trans_type = 'REN'  
    SELECT @Extended_Limits_Enabled = Isnull(VALUE, 0)  
    FROM   system_options  
    WHERE  option_number = 5260
           AND branch_id = 1  
  
  
  -- If Switched On Then
  --If @Extended_Limits_Enabled=1
  IF @Trans_type = 'MTA'
      OR @Trans_type = 'MTC'
      OR @Trans_type = 'MTR'
      OR @Trans_type IN ('MTCR', 'PT', 'DRI')
    BEGIN
        UPDATE ri_arrangement
        SET    is_extended_limit_applied = Isnull(@NBExtended_Is_Enabled, 0),
               extended_limit_amount = Isnull(@line_limit, 0)
        WHERE  ri_arrangement_id = @ri_arrangement_id  and original_flag=0

        SET @Extended_Limits_Enabled=@NBExtended_Is_Enabled  
    END  
  ELSE  
    IF Isnull(@Extended_Limits_Enabled, 0) = 1  
      UPDATE ri_arrangement  
      SET    extended_limit_amount = Isnull(@line_limit, 0),  
             is_extended_limit_applied = Isnull(@Extended_Limits_Enabled, 0)  
      WHERE  ri_arrangement_id = @ri_arrangement_id  
    ELSE  
      IF Isnull(@Extended_Limits_Enabled, 0) = 0  
        UPDATE ri_arrangement  
        SET    extended_limit_amount = 0,  
               is_extended_limit_applied = 0  
        WHERE  ri_arrangement_id = @ri_arrangement_id  
  
  -- If different get combined rate, else set rate as 1  
  IF @model_currency_id <> @policy_currency_id  
    BEGIN  
        EXECUTE Spu_act_get_currency_rate  
          @model_currency_id,  
          @source_id,  
          @prop_effective_date,  
          @model_currency_rate OUTPUT  
  
        SELECT @model_currency_rate = @model_currency_rate /  
                                      @policy_currency_rate  
    END  
  ELSE  
    SELECT @model_currency_rate = 1  
  
  -- Insert the arrangement lines  
  INSERT INTO ri_arrangement_line  
              (ri_arrangement_id,  
               TYPE,  
               treaty_id,  
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
               is_commission_modified,  
               retained,  
               --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
               ri_model_line_id,  
               is_obligatory)  
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
  SELECT @ri_arrangement_id,  
         CASE  
           WHEN rt.code = 'RET' THEN 'R'  
           WHEN rt.code = 'XOL'  
                 OR rt.code = 'FAX' THEN 'TX'  
           --When rt.code = 'FAX'  Then  'FX'  
           WHEN rt.code = 'CAT' THEN 'TC'  
           WHEN rt.code = '001' THEN 'TFS'  
           ELSE 'T'  
         END,  
         rml.treaty_id,  
         rml.share_percent,  
         0,  
         0,  
         tc.commission_percent,  
         t.agreement_code,  
         rml.priority,  
rml.number_of_lines,  
         CASE  
           WHEN Isnull(@line_limit, 0) <> 0  
                AND ( rt.code NOT IN ( 'XOL', 'CAT' ) )  
                 OR ( @Extended_Limits_Enabled = 1  
                      AND rt.code IN ( 'TFS', 'R', 'T' ) )THEN Isnull(@line_limit, 0)  
           ELSE ( rml.line_limit ) * @model_currency_rate  
         END,  
         rml.lower_limit * @model_currency_rate,  
         0,  
         0,  
         0,  
         0,  
         CASE  
           WHEN rt.code = 'RET' THEN 100  
           ELSE NULL  
         END retained,  
         --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
         rml.ri_model_line_id,  
         rml.is_obligatory  
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
  FROM   ri_model_line rml  
         JOIN treaty t  
           ON t.treaty_id = rml.treaty_id  
         JOIN reinsurance_type rt  
           ON rt.reinsurance_type_id = t.reinsurance_type_id  
         LEFT JOIN  
         -- Calculate a summary commission rate for each treaty  
         (SELECT treaty_id,  
                 SUM(commission_percent * ( share_percent / 100 ))  
                 commission_percent  
          FROM   treaty_party  
          GROUP  BY treaty_id) tc  
           ON tc.treaty_id = t.treaty_id  
  WHERE  ri_model_id = @ri_model_id  and rt.code NOT IN ( 'XOL', 'CAT','RET' )
  
  
IF @xol_model_currency_id <> @policy_currency_id  
    BEGIN  
        EXECUTE Spu_act_get_currency_rate  
          @xol_model_currency_id,  
          @source_id,  
          @effective_date,  
          @xol_model_currency_rate OUTPUT  
  
        SELECT @xol_model_currency_rate = @xol_model_currency_rate /  
                                      @policy_currency_rate  
    END  
  ELSE  
    SELECT @xol_model_currency_rate = 1  
  
  -- Insert the arrangement lines  
  INSERT INTO ri_arrangement_line  
              (ri_arrangement_id,  
               TYPE,  
               treaty_id,  
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
               is_commission_modified,  
               retained,  
               --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
               ri_model_line_id,  
               is_obligatory)  
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
  SELECT @ri_arrangement_id,  
         CASE  
           WHEN rt.code = 'RET' THEN 'R'  
           WHEN rt.code = 'XOL'  
                 OR rt.code = 'FAX' THEN 'TX'  
           --When rt.code = 'FAX'  Then  'FX'  
           WHEN rt.code = 'CAT' THEN 'TC'  
           WHEN rt.code = '001' THEN 'TFS'  
           ELSE 'T'  
         END,  
         rml.treaty_id,  
         rml.share_percent,  
         0,  
         0,  
         tc.commission_percent,  
         t.agreement_code,  
         rml.priority,  
rml.number_of_lines,  
         CASE  
           WHEN Isnull(@line_limit, 0) <> 0  
                AND ( rt.code NOT IN ( 'XOL', 'CAT' ) )  
                 OR ( @Extended_Limits_Enabled = 1  
                      AND rt.code IN ( 'TFS', 'R' , 'T') )THEN Isnull(@line_limit, 0)
           ELSE ( rml.line_limit ) * @xol_model_currency_rate  
         END,  
         rml.lower_limit * @xol_model_currency_rate,  
         0,  
         0,  
         0,  
         0,  
         CASE  
           WHEN rt.code = 'RET' THEN 100  
           ELSE NULL  
         END retained,  
         --Start-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
         rml.ri_model_line_id,  
         rml.is_obligatory  
  --End-(Arul Stephen)-(WPR2 - Reinsurance Obligatory)  
  FROM   ri_model_line rml  
         JOIN treaty t  
           ON t.treaty_id = rml.treaty_id  
         JOIN reinsurance_type rt  
           ON rt.reinsurance_type_id = t.reinsurance_type_id  
         LEFT JOIN  
         -- Calculate a summary commission rate for each treaty  
         (SELECT treaty_id,  
                 SUM(commission_percent * ( share_percent / 100 ))  
                 commission_percent  
          FROM   treaty_party  
          GROUP  BY treaty_id) tc  
           ON tc.treaty_id = t.treaty_id  
  WHERE  ri_model_id = @xol_ri_model_id and rt.code IN ( 'XOL', 'CAT','RET' )
  -- Recalculate the RI Arrangement Lines
  Select @Is_original = original_flag from RI_Arrangement where ri_arrangement_id =@ri_arrangement_id
  IF @Extended_Limits_Enabled=1 And @Is_original <>1 And ISNULL(@line_limit,0) >0
      Update RI_Arrangement_Line set line_limit=ISNULL(@line_limit,0) where type in ('TFS','T','R')   and ri_arrangement_id = @ri_arrangement_id
  
  IF @is_auto_reinsured = 1  
    IF @reinsurance_type = 'R'  
        OR @reinsurance_type = 'T'  
      EXECUTE Spu_ri_arrangement_calc_ri2007  
        @ri_arrangement_id = @ri_arrangement_id,  
        @band_si = @sum_insured,  
        @band_premium = @premium,  
        @ri_model_id = @xol_ri_model_id,  
        @Trans_type = @Trans_type,  
        @Extended_Limits_Enabled = @Extended_Limits_Enabled,  
        @Extended_Limits_Amount = @line_limit,
		@Original_Risk_Cnt = @Original_Risk_Cnt  
GO
  
--**************************************************************************************************************************

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Finalise_stats_For_Utility'
GO

CREATE PROCEDURE spu_CLM_Finalise_stats_For_Utility  
    @claim_id int,  
    @transaction_type_id int,  
    @transaction_type_code varchar(10),  
    @stats_folder_cnt int,  
    @bstatssuppressed  tinyint output,    
    @nIsCloned tinyint =0,   
    @nIsCloned_reversal tinyint =0 ,  
    @is_pt tinyint =0    
AS  
  
--  @transaction_export_folder_cnt int OUTPUT  
/*********************************************************************************************  
 1.1    Updated to create with correct Document Ref.    RWH 06/07/01  
  
 1.2    Increase numeric element of Doc Ref to 8 digits RWH 27/07/01  
 1.3    Pass in @stats_folder_cnt as parameter.    RWH 14/09/01  
  
**********************************************************************************************/  
  
DECLARE  
    @source_id int,  
    @sub_branch_id int, -- PWF 03/07/2002  
    @stats_detail_id int,  
    @transaction_export_folder_cnt int,  
    @transaction_export_detail_id int,  
    @document_prefix char(3),  
    @retrieved_prefix varchar(10),  
    @max_orion_ref varchar(20),  
    @document_ref varchar(25),  
    @posting_period_year int,  
    @posting_period_number smallint,  
    @key_suffix_int int,  
    @transaction_amount numeric(19, 4),  
    @NumberRangeID int,  
    @DocumentRefNumber Varchar(25),  
    @UniqueDocumentRef Integer,
    @user_id int,
	@nPayment_id int,
	@nReceipt_id int  
--Get the real data  
  
-- Get transaction_type_code from stats_folder rather than roadmap.  
SELECT @transaction_type_code = transaction_type_code  
FROM    stats_folder  
WHERE   stats_folder_cnt = @stats_folder_cnt  
  
--  Check the transaction type to set the Document Type.  
SELECT @transaction_type_code = LTRIM(RTRIM(@transaction_type_code))  
  
-- determine if this copy work to claim call should be suppressed  
DECLARE @suppress int  
EXEC spu_CLM_Suppress_Stats @claim_id, @transaction_type_code, @suppress OUTPUT  
IF @suppress = 1  
BEGIN  
 -- remove the suppressed stats details  
 DELETE FROM stats_detail where stats_folder_cnt = @stats_folder_cnt  
 DELETE FROM stats_folder where stats_folder_cnt = @stats_folder_cnt  
 Set @bstatssuppressed = 1  
 RETURN  
END  

  If @nIsCloned =1 
BEGIN
	 Update Stats_Detail   
      SET tax_value = 0,  
      annual_premium=0,  
      this_premium_original =0,  
      this_premium_home=0,  
      lead_commission_value_home=0,  
      sub_commission_value_home=0,  
      sum_insured_home=0,  
      sum_insured_total=0,  
      charges_total=0,  
      taxes_total =0,        
      recoveries_total =0,        
      commission_excluded =0,        
      withholding_tax_excluded =0,    
      this_premium_system =0,        
      lead_commission_value_system =0,        
      sub_commission_value_system =0,        
      sum_insured_system =0  
      Where stats_folder_cnt = @stats_folder_cnt and Stats_detail_type ='GRS'   
END

    
Select @UniqueDocumentRef = ISNull(value,0) 
				From Hidden_options Where branch_id = 1 And option_number= 100
  
SELECT @document_prefix =  
    CASE @transaction_type_code  
        WHEN 'C_CO' THEN 'CLO'  
        WHEN 'C_CP' THEN 'CLP'  
        WHEN 'C_CR' THEN 'CLA'  
        WHEN 'C_SA' THEN 'CLR'  
        WHEN 'C_RV' THEN 'CLR'  
    END  
  
    if @nIsCloned = 1    
  SELECT @document_prefix='CLD'    
    
  if @nIsCloned_reversal = 1  
  SELECT @document_prefix = 'CLC'  
    
    if @is_pt = 1    
  SELECT @document_prefix='CPA'    
    
--We need to check the sign of a payment/receipt to finalise the document type.  
  
-- Adjust the Doc Ref for payment or receipt depending on sign of amount.  
IF (@document_prefix = 'CLR') OR (@document_prefix = 'CLP')  
BEGIN  
  
    SELECT  @transaction_amount = sum_insured_total  
    FROM    stats_detail  
    WHERE   stats_folder_cnt  = @stats_folder_cnt  
    AND  stats_detail_type = 'GRS' -- there should be only one GRS line  
  
    IF @transaction_amount < 0  
    BEGIN  
        IF @document_prefix = 'CLR'  
            SELECT @document_prefix = 'CLP'  
        ELSE  
            SELECT @document_prefix = 'CLR'  
    END  
  
END
IF ISNULL(@nIsCloned_reversal,0) <> 1 
BEGIN 
	IF ISNULL(@nIsCloned,0) = 1
	BEGIN

	SELECT @document_ref=document_ref FROM Stats_Folder 
	WHERE loss_id = @claim_id and document_ref like 'CLC%' and transaction_type_code=@transaction_type_code

	END
END
IF ((ISNULL(@nIsCloned,0) <> 1 OR @document_ref IS NULL) OR (ISNULL(@nIsCloned_reversal,0) = 1 OR @document_ref IS NULL) )
BEGIN  

IF (@UniqueDocumentRef = 1)
    BEGIN
	SELECT  
	        @retrieved_prefix = prefix,  
	        @key_suffix_int = next_number  
	FROM    Next_Orion_Doc_Ref  
	WHERE   prefix = @document_prefix  
	  
	IF @Key_Suffix_Int is NULL  
	BEGIN  
	    SELECT  @max_orion_ref = MAX(document_ref)  
	    FROM    Document  
	    WHERE   document_ref like @document_prefix +'%'  
	    AND     LEN ( LTRIM ( RTRIM ( document_ref ))) - LEN ( @document_prefix ) = 8  
	  
	    IF @max_orion_ref IS NOT NULL  
	        SELECT  @Key_Suffix_Int = SUBSTRING ( @max_orion_ref, LEN ( @document_prefix ) + 1, 8 ) + 1  
	    ELSE  
	        SELECT  @Key_Suffix_Int = 10000001  
	  
	END  
	--Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
	  
	--Note:- DocumentRef will be calculated with the following SPs as per the new development "WPR78"  
	EXEC spu_ACT_Get_Number_Range_From_Code @document_prefix,  @NumberRangeID  OUTPUT  
	  
	EXEC spu_ACT_Generate_Next_Unique_Document_Reference @NumberRangeID,1,1,@DocumentRefNumber OUTPUT  
	  
	SELECT  @document_ref = @document_prefix + @DocumentRefNumber  
	  
	--End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
	  
	IF (@retrieved_prefix is null) OR (@retrieved_prefix = '')  
	    INSERT INTO Next_Orion_Doc_Ref  
	    VALUES  (@document_prefix, @Key_Suffix_Int + 1)  
	ELSE  
	    UPDATE  Next_Orion_Doc_Ref  
	    SET next_number = @Key_Suffix_Int + 1  
	    WHERE   prefix = @document_prefix  
    END
Else
    BEGIN
	SELECT @source_id = source_id,
		@user_id = created_by_user_id
	FROM   stats_folder  
	WHERE  stats_folder_cnt = @stats_folder_cnt  
	  
	SELECT @NumberRangeID =  
	    CASE @document_prefix  
	        WHEN 'CLO' THEN 40  
	        WHEN 'CLP' THEN 28 
	        WHEN 'CLA' THEN 41
	        WHEN 'CLR' THEN 29 
         WHEN 'CLD' THEN 59    
         WHEN 'CPA' THEN 60  
         WHEN 'CLC' THEN 58   
	    END  
	
	EXEC spe_ACTnumber_add @Key_Suffix_Int OUTPUT , @NumberRangeID, @user_id, @source_id
	SELECT @document_ref = RTRIM(@document_prefix) 
					+ CONVERT(VARCHAR, REPLICATE ( 0 , 10-LEN(@Key_Suffix_Int)))+ CONVERT(VARCHAR,@Key_Suffix_Int)
	
    END  

END
ELSE
BEGIN
IF ISNULL(@nIsCloned_reversal,0) <> 1
BEGIN
	SELECT @document_ref=REPLACE(@document_ref,'CLC','CLD')
END
END
--*************  
-- MEvans : 06-03-2003 : Issue 2728  
DECLARE @ProductOption int  
  
SELECT  @ProductOption = value  
FROM    Hidden_Options  
WHERE   branch_id = 1 and option_number = 16  
--*************  
  
-- PWF 30/07/2002 - get sub branch id  
  
        SELECT  @sub_branch_id = branch_id -- IFIBCR  
 FROM    insurance_file  
        INNER JOIN  
                claim ON insurance_file.insurance_file_cnt = claim.policy_id  
        WHERE   claim.claim_id = @claim_id  
  
--*************  
-- MEvans : 06-03-2003 : Issue 2728  
IF @ProductOption = 1  
    BEGIN  
  
        -- use sub branch ids  
        SELECT  @posting_period_number = current_period_id  
        FROM    ledger  
        WHERE   ledger_short_name = 'SA'  
        AND     sub_branch_id = @sub_branch_id -- PWF 30/07/2002  
  
        SELECT  @posting_period_year = datepart(year, min(period_end_date))  
        FROM    period  
        WHERE   year_name = (  
            SELECT  year_name  
            FROM    period  
            WHERE   Period_id = @posting_period_number)  
        AND     sub_branch_id = @sub_branch_id -- PWF 30/07/2002  
  
    END  
ELSE  
    BEGIN  
  
        -- use default ledger  
        SELECT  @posting_period_number = current_period_id  
                FROM    ledger  
                WHERE   ledger_short_name = 'SA'  
  
        SELECT  @posting_period_year = datepart(year, min(period_end_date))  
        FROM    period  
        WHERE   year_name = (  
            SELECT  year_name  
            FROM    period  
            WHERE   Period_id = @posting_period_number)  
  
    END  
--*************  
  
-- Now for the stats.  Every folder should be the same, so let's use the first one.  
-- Then we just take the details for each of the folders and write it out  

  
BEGIN  
  
    UPDATE Stats_Folder  
    SET   document_ref = @document_ref,  
          posting_period_year = @posting_period_year,  
       posting_period_number = @posting_period_number,  
          transaction_type_id = @transaction_type_id,  
          transaction_type_code = @transaction_type_code  
    WHERE stats_folder_cnt = @stats_folder_cnt  
  
		if (@nIsCloned_reversal = 1 or @nIsCloned=1)
		BEGIN
			select @nPayment_id=payment_id,@nReceipt_id=Receipt_Id from stats_folder where loss_id=@claim_id and document_ref not like 'CLC%' and document_ref not like 'CLD%'
			UPDATE Stats_Folder
			SET   payment_id=@nPayment_id,
				  Receipt_Id=@nReceipt_id
			WHERE stats_folder_cnt = @stats_folder_cnt
		END
  
END  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
--**************************************************************************************************************************

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Copy_Reinsurance_Details_To_Claim_Utility'
GO

--sp_helptext Spu_copy_reinsurance_details_to_claim
Create PROCEDURE spu_Copy_Reinsurance_Details_To_Claim_Utility
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
  @ri_model_type	  INT

 DECLARE  @claim_ri_arrangement_id INT

   SELECT @product_option='0'
   SELECT @product_option=ISNULL(value,0) FROM Hidden_Options WITH (NOLOCK) WHERE option_number=88
   Select @Effective_date=loss_from_date from claim where claim_id=@claim_id
   Set @Reapply_TX=0

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
  Select @XOL_Treaty_to_Recover_From=XOL_Treaty_to_Recover_From_id from ri_band WITH (NOLOCK) where ri_band_id=@ribandid
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
               ELSE
                   NULL
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
 --IF @is_deferred = 0
 --BEGIN
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
 --END
 --ELSE
 --BEGIN
 ---- Get totals for reserve and payments, this will allow deferred ri
 -- -- to catch up once we have a proper ri model
 -- DECLARE Reserve_CurSor CURSOR  FOR
 -- SELECT   p.ri_Band,
 --    SUM(r.Initial_Reserve + r.Revised_Reserve),
 --    SUM(r.Paid_To_Date)
 -- FROM     Claim_Peril p WITH (NOLOCK)
 --    JOIN Reserve r WITH (NOLOCK)
 --    ON r.Claim_Peril_Id = p.Claim_Peril_Id
 -- WHERE    p.Claim_Id = @claim_id
 -- GROUP BY p.ri_Band
 -- ORDER BY p.ri_Band
 --END

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


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE Ddldropprocedure
  'spu_Refresh_Tax_DataFix'

GO
Create Procedure spu_Refresh_Tax_DataFix
(
@insurance_file_cnt INT,
@transaction_type VARCHAR(5)
)
 AS
 BEGIN 


Declare @tax_amount Money
Declare @RecursivePremium Money
Declare @CalcBasis int
Declare @Premium Money
Declare @TaxCalculationCnt int
Declare @TaxPercentage float


DELETE FROM tax_calculation WHERE insurance_file_cnt= @insurance_file_cnt
exec spu_Insurance_File_Tax_Select @insurance_file_cnt, 0,@transaction_type,1        

 DECLARE INSCUR CURSOR FOR 
		select  premium, calc_basis, tax_calculation_cnt,percentage value from Tax_Calculation 
		where insurance_file_cnt= @insurance_file_cnt order by tax_calculation_cnt



OPEN INSCUR 
FETCH NEXT FROM INSCUR INTO @Premium,@CalcBasis,@TaxCalculationCnt,@TaxPercentage

WHILE @@FETCH_STATUS = 0
BEGIN
 
 
IF @CalcBasis=3
	begin
		select  @RecursivePremium=SUM(VALUE)  from Tax_Calculation 
		where insurance_file_cnt= @insurance_file_cnt  AND  tax_calculation_cnt < @TaxCalculationCnt
		SET @tax_amount= ROUND((@Premium+@RecursivePremium)*@TaxPercentage/100,4)
		UPDATE  tax_calculation set value =@tax_amount WHERE  tax_calculation_cnt =@TaxCalculationCnt

		print @TaxCalculationCnt
	end
	ELSE
	begin
		  SET @tax_amount= ROUND(@Premium*@TaxPercentage/100,4)
		  UPDATE  tax_calculation set value =@tax_amount WHERE  tax_calculation_cnt =@TaxCalculationCnt
		  
	end

 
FETCH NEXT FROM INSCUR INTO @Premium,@CalcBasis,@TaxCalculationCnt ,@TaxPercentage 


END

CLOSE INSCUR 
DEALLOCATE INSCUR
 
SET @tax_amount=0
select @tax_amount = isnull(sum(value),0)from tax_calculation with (nolock) where insurance_file_cnt = @insurance_file_cnt-- and risk_cnt is not null 
update insurance_file set tax_amount = @tax_amount where insurance_file_cnt = @insurance_file_cnt

DELETE from Agent_Commission where insurance_file_cnt=@insurance_file_cnt
EXEC spu_sir_agent_commission_calc @insurance_file_cnt,@transaction_type

END

GO