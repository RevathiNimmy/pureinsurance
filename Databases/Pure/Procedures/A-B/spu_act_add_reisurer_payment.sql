SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON

GO

EXECUTE DDLDROPPROCEDURE 'spu_act_add_reisurer_payment'

GO

CREATE PROCEDURE spu_act_add_reisurer_payment 
	@nCompanyID         INT,
	@nTaxBandID        INT,    
	@nCurrencyID        INT,
	@crAmount             MONEY,
	@nInsuranceFileCnt INT,
	@nRIPartyCnt       INT,
	@sDocumentRef       VARCHAR(50)

AS
  DECLARE @tax_rate_is_value INT
  DECLARE @tax_rate MONEY
  DECLARE @tax_currency_id INT
  DECLARE @effective_date DATETIME
  DECLARE @individual_tax_amount MONEY
  DECLARE @tax_type_id INT
  DECLARE @tax_calculation_cnt INT
  DECLARE @tax_sequence INT
  DECLARE @tax_rate_allow_tax_credit INT
  DECLARE @tax_rate_country_id INT
  DECLARE @tax_rate_state_id INT
  DECLARE @tax_rate_class_of_business_id INT
  DECLARE @tax_band_rate_id INT
  DECLARE @system_option VARCHAR(20)
  DECLARE @tax_currency_amount MONEY
  DECLARE @tax_base_amount MONEY
  DECLARE @tax_group_id INT
  DECLARE @stats_folder_cnt INT
  DECLARE @tax_band VARCHAR(50)
  DECLARE @stats_version INT
  DECLARE @earning_pattern_id INT

  SELECT @tax_currency_amount = 0,
         @tax_base_amount = 0

  -- cannot proceed if @tax_band_id is NULL so just exit......       
  IF Isnull(@nTaxBandID, 0) = 0
    RETURN
      SELECT @system_option = '0'

  SELECT @system_option = VALUE
  FROM   System_Options
  WHERE  option_number = 5019
         AND branch_id = 1

  IF @system_option = '1'
    BEGIN
        -- Get details from insurance file    
        SELECT @effective_date = ifi.cover_start_date
        FROM   insurance_file ifi
        WHERE  ifi.insurance_file_cnt = @nInsuranceFileCnt
    END
  ELSE
    IF @system_option = '0'
      BEGIN
          -- Get system_base_date from insurance file    
          SELECT @effective_date = ifi.system_base_date
          FROM   insurance_file ifi
          WHERE  ifi.insurance_file_cnt = @nInsuranceFileCnt
      END
    ELSE
      BEGIN
          SELECT @effective_date = Getdate()
      END

  --get tax band and tax band rate details details
  SELECT @tax_type_id = tt.tax_type_id,
         @nTaxBandID = tb.tax_band_id,
         @tax_rate_is_value = tbr.is_value,
         @tax_rate = tbr.rate,
         @tax_currency_id = tbr.currency_id,
         @tax_sequence = tgtb.SEQUENCE,
         @tax_rate_allow_tax_credit = tbr.allow_tax_credit,
         @tax_rate_country_id = tbr.country_id,
         @tax_rate_state_id = tbr.state_id,
         @tax_rate_class_of_business_id = tbr.class_of_business_id,
         @tax_band_rate_id = tbr.tax_band_rate_id,
         @tax_group_id = tgtb.tax_group_id,
         @tax_band = tb.code
  FROM   tax_type tt
         JOIN tax_band tb
           ON tb.tax_type_id = tt.tax_type_id
         JOIN tax_band_rate tbr
           ON tbr.tax_band_id = tb.tax_band_id
         JOIN tax_group_tax_band tgtb
           ON tgtb.tax_band_id = tb.tax_band_id
  WHERE  tt.is_deleted = 0
         AND tb.tax_band_id = @nTaxBandID
         AND tt.effective_date <= @effective_date
         AND tb.is_deleted = 0
         AND tb.effective_date <= @effective_date
         AND tbr.tax_band_rate_id = -- Ensure we only get one rate for the band!!!      
             (SELECT TOP 1 tax_band_rate_id
              FROM   tax_band_rate tbr2
              WHERE  tbr2.tax_band_id = tb.tax_band_id
                     AND tbr2.is_deleted = 0
                     AND tbr2.effective_date <= @effective_date
                     AND Isnull(tbr2.TTRIPR, 0) = 1
              ORDER  BY tbr2.effective_date DESC)
  ORDER  BY tgtb.SEQUENCE

  -- the routine needs to convert the tax value        
  -- into the same currency as the other transactions        
  -- before it is added to the tax_calculation table       
  EXEC Spu_act_do_currency_to_currency_conversion
    @currency_id_from = @tax_currency_id,
    @currency_amount_from = @crAmount,
    @company_id = @nCompanyID,
    @currency_id_to= @nCurrencyID,
    @currency_amount_to = @individual_tax_amount OUTPUT

  IF @individual_tax_amount <> 0
    BEGIN
        -- insert an entry into tax_calculation for applied 
		-- tax_band_rate and insurance_file_cnt
        INSERT INTO tax_calculation
                    (insurance_file_cnt,
                     risk_cnt,
                     tax_band_id,
                     premium,
                     percentage,
                     VALUE,
                     is_value,
                     is_manually_changed,
                     Calc_Basis,
                     Basis_Value,
                     Sum_Insured,
                     Sum_Insured_Rounded,
                     currency_id,
                     allow_tax_credit,
                     original_sum_insured,
                     country_id,
                     state_id,
                     class_of_business_id,
                     tax_group_id,
                     SEQUENCE,
                     transtype,
                     policy_fee_u_id,
                     ri_party_cnt,
                     tax_band_rate_id)
        VALUES      ( @nInsuranceFileCnt,
                      NULL,
                      @nTaxBandID,
                      Round(@individual_tax_amount, 2),
                      CASE
                        WHEN @tax_rate_is_value = 0 THEN @tax_rate
                        ELSE 0
                      END,
                      Round(@individual_tax_amount, 2),
                      @tax_rate_is_value,
                      0,
                      NULL,
                      NULL,
                      NULL,
                      NULL,
                      @nCurrencyID,
                      @tax_rate_allow_tax_credit,
                      NULL,
                      @tax_rate_country_id,
                      @tax_rate_state_id,
                      @tax_rate_class_of_business_id,
                      @tax_group_id,
                      @tax_sequence,
                      'TTRIPR', --new transaction type for RI Payment/Reciept
                      NULL,
                      @nRIPartyCnt,--@associated_key_id
                      @tax_band_rate_id)

        --insert an entry in stats detail
        DECLARE @stats_detail_id          INT,
                @currency_rate            DECIMAL,
                @currency_code            VARCHAR(50),
                @risk_type_id             INT,
                @risk_id                  INT,
                @system_rate              DECIMAL,
                @risk_type_code           VARCHAR(50),
                @peril_id                 INT,
                @peril_description        VARCHAR(50),
                @peril_type_id            INT,
                @peril_type_code          VARCHAR(50),
                @policy_section_type_id   INT,
                @policy_section_type_code VARCHAR(50),
                @class_of_business_id     INT,
                @class_of_business_code   VARCHAR(50),
                @ri_share_percent         DECIMAL

        SELECT @stats_folder_cnt = stats_folder_cnt
        FROM   stats_folder
        WHERE  insurance_file_cnt = @nInsuranceFileCnt

        SELECT @stats_detail_id = MAX(stats_detail_id) + 1
        FROM   Stats_Detail
        WHERE  stats_folder_cnt = @stats_folder_cnt

        IF @stats_detail_id IS NULL
          SELECT @stats_detail_id = 1

        SELECT @nCompanyID = source_id,
               @nCurrencyID = currency_id,
               @currency_rate = currency_base_xrate,
               @system_rate = system_base_xrate
        FROM   insurance_file
        WHERE  insurance_file_cnt = @nInsuranceFileCnt

        SELECT @currency_code = code
        FROM   currency
        WHERE  currency_id = @nCurrencyID
		--Pick any existing stats_detail for the given stats_folder_cnt and add
		--a new entry in stats_detail with changed values
        SELECT TOP 1 @risk_id = risk_id,
                     @risk_type_id = risk_type_id,
                     @risk_type_code = risk_type_code,
                     @peril_id = peril_id,
                     @peril_description = peril_description,
                     @peril_type_id = peril_type_id,
                     @peril_type_code = peril_type_code,
                     @policy_section_type_id = policy_section_type_id,
                     @policy_section_type_code = policy_section_type_code,
                     @class_of_business_id = class_of_business_id,
                     @class_of_business_code = class_of_business_code,
                     @stats_version = stats_version,
                     @earning_pattern_id = earning_pattern_id
        FROM   Stats_Detail
        WHERE  stats_folder_cnt = @stats_folder_cnt
               AND stats_detail_type = 'GRS'

        /* Insert the Stats Detail for -ve amount and ri_party_cnt*/
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
                     ri_share_percent,
                     currency_code,
                     currency_rate,
                     this_premium_original,
                     this_premium_home,
                     this_premium_system)
        VALUES      ( @stats_folder_cnt,
                      @stats_detail_id,
                      'TTP',
                      @risk_id,
                      @risk_type_id,
                      @risk_type_code,
                      @peril_id,
                      @peril_description,
                      @peril_type_id,
                      @peril_type_code,
                      @policy_section_type_id,
                      @policy_section_type_code,
                      @class_of_business_id,
                      @class_of_business_code,
                      @nTaxBandID,
                      @tax_band,
                      -@individual_tax_amount,
                      @nRIPartyCnt,
                      NULL,
                      @ri_share_percent,
                      @currency_code,
                      @currency_rate,
                      -@individual_tax_amount,
                      -@individual_tax_amount,
                      -@individual_tax_amount )

        
        SELECT @stats_detail_id = MAX(stats_detail_id) + 1
        FROM   Stats_Detail
        WHERE  stats_folder_cnt = @stats_folder_cnt
		/* Insert the Stats Detail for +ve amount and tax account*/
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
                     ri_share_percent,
                     currency_code,
                     currency_rate,
                     this_premium_original,
                     this_premium_home,
                     this_premium_system)
        VALUES      ( @stats_folder_cnt,
                      @stats_detail_id,
                      'TAN',
                      @risk_id,
                      @risk_type_id,
                      @risk_type_code,
                      @peril_id,
                      @peril_description,
                      @peril_type_id,
                      @peril_type_code,
                      @policy_section_type_id,
                      @policy_section_type_code,
                      @class_of_business_id,
                      @class_of_business_code,
                      @nTaxBandID,
                      @tax_band,
                      @individual_tax_amount,
                      NULL,
                      'NOTA' + @tax_band,
                      @ri_share_percent,
                      @currency_code,
                      @currency_rate,
                      @individual_tax_amount,
                      @individual_tax_amount,
                      @individual_tax_amount )

		--add a new document as Journal
        DECLARE @documenttype_id INT
		DECLARE @doctype VARCHAR(5)
		DECLARE @Document_id INT
		DECLARE @currenct_date datetime
		
		SELECT @currenct_date=getdate()
		SELECT @doctype=Substring(@sDocumentRef, 1, 2)
        SELECT @documenttype_id = documenttype_id
        FROM   documenttype
        WHERE  code = @doctype
		
        EXEC spu_act_add_document
          @Document_id=@Document_id,
          @company_id=@nCompanyID,
          @postingstatus_id=3,
          @documenttype_id=@documenttype_id,
          @auditset_id=NULL,
          @batch_id=NULL,
          @document_ref=@sDocumentRef,
          @document_date=@currenct_date,
          @created_date=@currenct_date,
          @authorised_date=@currenct_date,
          @comment='Reinsurance Tax',
          @write_off_reason_id=NULL,
          @insurance_file_cnt=@nInsuranceFileCnt,
          @reason='',
          @sub_branch_id=NULL,
          @claim_id=NULL,
          @terms_of_payment_id=0

    END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO