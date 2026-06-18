
Execute DDLDropProcedure 'spu_SIR_Recalculate_Risk_Fees'
GO

CREATE PROCEDURE spu_SIR_Recalculate_Risk_Fees
 @transaction_type_id int,
 @risk_cnt int,
 @insurance_file_cnt int,
 @use_existing_fee_details INT = 0,
 @sTransactiontype VARCHAR(10) =NULL
AS

    -- ************************************************
    -- NB: FEE_AMOUNT IS JUST THE VALUE CARRIED THROUGH
    -- FROM THE FEE_AMOUNTS CONFIG TABLE, THE
    -- CURRENCY_AMOUNT IS THE ACTUAL FEE AMOUNT
    -- ************************************************

    DECLARE @base_currency_id SMALLINT
    DECLARE @currency_id SMALLINT
    DECLARE @currency_base_xrate FLOAT
    DECLARE @source_id INT
    DECLARE @TypeOfRates TINYINT
    DECLARE @effective_date DATETIME
    DECLARE @effective_fee_date DATETIME
    DECLARE @premium MONEY
    DECLARE @currency_desc VARCHAR(255)
    DECLARE @currency_isocode varchar(10)
    DECLARE @transaction_sub_type_id int
    DECLARE @crProratarate NUMERIC(19,8)
    -- policy discount / loading requires that any manual (user led) changes
    -- to fees ( e.g edits / deletes ) are retained when a discount is applied
    -- and rolled out. For this reason we use the existing fee detail but
    -- recalculate the fee amount and tax amount as the underlying premiums will
    -- have changed
    IF (@use_existing_fee_details=0)
      IF exists(SELECT pfu.fee_rate_percentage FROM
      policy_fee_u pfu
      WHERE pfu.Insurance_file_cnt = @insurance_file_cnt and pfu.risk_cnt=@risk_cnt ) SET @use_existing_fee_details=1

       IF @use_existing_fee_details <> 0 BEGIN
              DECLARE @ExistingFee TABLE
              (
              party_cnt INT,
              fee_rate_percentage NUMERIC(7, 4),
              fee_rate_amount NUMERIC(19, 4),
              transaction_type_id INT,
			  peril_group_id INT,
              tax_group_id INT,
              transaction_sub_type INT,
              fee_rate_currency_id SMALLINT,
              MakeLiveOptions_id INT,
              DoPaymentTerms_id INT,
              Calculation_Basis TINYINT,
              Is_Prorated TINYINT,
              Is_Override TINYINT,
              Is_Percent  TINYINT
              )

              -- Keep all existing fee lines before deleting
         INSERT INTO @ExistingFee (	party_cnt,
									fee_rate_percentage,
									fee_rate_amount,
									transaction_type_id,
									peril_group_id,
									tax_group_id,
									transaction_sub_type,
									fee_rate_currency_id,
									MakeLiveOptions_id,
									DoPaymentTerms_id,
									Calculation_Basis,
									Is_Prorated,
									Is_Override,
									Is_Percent)
			SELECT					party_cnt,
									fee_rate_percentage,
									fee_rate_amount,
									transaction_type_id,
									peril_group_id,
									tax_group_id,
									transaction_sub_type,
									fee_rate_currency_id,
									MakeLiveOptions_id,
									DoPaymentTerms_id,
									Calculation_Basis,
									Is_Prorated,
									Is_Override,
									FeeTypePercent
			FROM policy_fee_u WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt = @risk_cnt
			SELECT @use_existing_fee_details = 0
       End

	   
IF @use_existing_fee_details <> 1 or (@transaction_type_id=10 and @risk_cnt is null)
    BEGIN
    --Get policy details
    SELECT  @effective_date = i.currency_base_date,
            @effective_Fee_date = i.cover_start_date,
            @base_currency_id = s.base_currency_id,
            @currency_id = i.currency_id,
            @currency_base_xrate = i.currency_base_xrate,
            @source_id = i.source_id,
            --@premium = i.this_premium,  -- PN 78361 & PN 78366 , calculating below
            @currency_desc = c.description,
            @currency_isocode = c.iso_code
    FROM    insurance_file i
    JOIN    source s
            ON s.source_id = i.source_id
    JOIN    currency c
            ON c.currency_id = i.currency_id
    WHERE   i.insurance_file_cnt = @insurance_file_cnt

	
	Select @premium =  Sum(this_premium) from Rating_Section where risk_cnt = @risk_cnt

    -- set transaction sub_type so that by default it is ignored by the main select
    SET @transaction_sub_type_id = 0

    -- if this is an MTA with a positive premium this is transaction_sub_type : Additional MTA
    IF @premium >= 0 AND @transaction_type_id = 9
        SET @transaction_sub_type_id = 1

    -- if this is an MTA with a negative premium this is transaction_sub_type : Return MTA
    IF @premium < 0 AND @transaction_type_id = 9
        SET @transaction_sub_type_id = 2

    -- if a valid effective date cannot be found then use todays date
    Select @effective_date = ISNULL(@effective_date, GetDAte())

    --Get source_id where rates are stored
    EXEC spu_ACT_GetTypeOfRates @TypeOfRates OUTPUT
    IF @TypeOfRates = 1
        SELECT @source_id = 1

	EXEC spu_get_policy_pro_rata_rate @nInsurancefilecnt=@insurance_file_cnt,@sTransactionType=@sTransactiontype, @crProrataRate =  @crProratarate OUTPUT
    IF ISNULL(@crProratarate,0) =0
        SELECT @crProratarate = 1

    -- clear down any taxes based on fees
    DELETE  Tax_Calculation
    WHERE   policy_fee_u_id IN (SELECT  policy_fee_u_id
                                FROM    policy_fee_u
                                WHERE   transaction_type_id = @Transaction_Type_id
                                AND     insurance_file_cnt = @insurance_file_cnt
                                AND     risk_cnt = @risk_cnt)

    -- clear down any entries that have already been calculated
    DELETE  policy_fee_u
    WHERE   transaction_type_id = @Transaction_Type_id
    AND     insurance_file_cnt = @insurance_file_cnt
    AND     risk_cnt = @risk_cnt

    -- NB. When no currency is specified against the fee
    -- then the fee automatically adopts the same currency id
    -- as the insurance_file

    -- create policy_fee_u entries
    INSERT INTO Policy_Fee_U (
            insurance_file_cnt,
            fee_rate_percentage,
            fee_rate_amount,
            fee_rate_currency_id,
            currency_id,
            branch_id,
            base_currency_id,
            risk_type_group_id,
            peril_group_id,
            transaction_sub_type,
            tax_group_id,
            risk_cnt,
            is_fee_applied_to_cr,
            party_cnt,
            product_id,
            transaction_type_id,
            include_fee_in_instalments,
	    spread_fee_across_instalments, FeeTypePercent,
			MakeLiveOptions_id ,
			DoPaymentTerms_id,
			Calculation_Basis,
			Is_Prorated,
			Pro_rata_rate ,
			is_override,
			fee_amount_id)
    SELECT  @insurance_file_cnt,
            fa.fee_percentage,
            CASE  WHEN FA.Is_Prorated=1 THEN @crProratarate*(fa.fee_amount)ELSE fa.fee_amount END ,
            CASE WHEN fa.currency_id IS NULL THEN @currency_id ELSE fa.currency_id END,
            @currency_id,
            @source_id,
            @base_currency_id,
            fa.risk_type_group_id,
            fa.peril_group_id,
            fa.transaction_sub_type,
            fa.tax_group_id,
            @risk_cnt,
            fa.is_fee_applied_to_cr,
            fa.party_cnt,
            fa.product_id,
            fa.transaction_type_id,
       	    fa.include_fee_in_instalments,
	    fa.spread_fee_across_instalments,
		Case when ISNULL(fa.fee_amount,0) = 0 and fa.currency_id IS NULL Then 1
	    ELSE 0
	    END ,
		NULL,
			NULL,
			NULL ,
			FA.Is_Prorated,
			CASE
			WHEN  fa.fee_percentage <> 0 THEN  1
			WHEN  (ISNULL(fa.is_Prorated,0) <> 0) THEN @crProratarate
			ELSE  1
			END,
			fa.Is_Override,
			fa.fee_amount_id
    FROM    Fee_Amounts fa
    WHERE   fa.transaction_type_id = @transaction_type_id
    AND    (@transaction_sub_type_id = 0 OR fa.transaction_sub_type = @transaction_sub_type_id)
    AND     fa.product_id IS NULL
    AND     fa.is_deleted = 0
    AND    (fa.risk_type_group_id
            IN (SELECT  risk_type_group_id
                FROM    risk_type_usage rtu
                JOIN    risk r
                        ON r.risk_type_id = rtu.risk_type_id
                WHERE   r.risk_cnt = @risk_cnt)
        OR  fa.peril_group_id
            IN (SELECT  peril_group_id
                FROM    rating_section_type rst
                JOIN    rating_section rs
                        ON rs.rating_section_type_id = rst.rating_section_type_id
                WHERE   rs.risk_cnt = @risk_cnt))
    AND     fa.effective_date = (SELECT  MAX(effective_date)
                                 FROM    fee_amounts fa2
                                 WHERE   fa2.transaction_type_id = @Transaction_Type_id
                                 AND    (@transaction_sub_type_id = 0 OR fa2.transaction_sub_type = @transaction_sub_type_id)
                                 AND     fa2.product_id IS NULL
                                 AND     fa2.is_deleted = 0
                                 AND    (fa2.risk_type_group_id
                                         IN (SELECT  risk_type_group_id
                                             FROM    risk_type_usage rtu
                                             JOIN    risk r
                                                     ON r.risk_type_id = rtu.risk_type_id
                                             WHERE   r.risk_cnt = @risk_cnt)
                                     OR  fa2.peril_group_id
                                         IN (SELECT  peril_group_id
                                             FROM    rating_section_type rst
                                             JOIN    rating_section rs
                                                     ON rs.rating_section_type_id = rst.rating_section_type_id
                                             WHERE   rs.risk_cnt = @risk_cnt))
                                 AND     fa2.effective_date <= @effective_fee_date
                                 AND     fa2.party_cnt = fa.party_cnt)

								 -- ReCalculate Peril Fees , PN 78361 & PN 78366
    -- Calculation of Peril Fees has become redundant in this SP.
    -- All Peril Fees being calculated/inserted by prevoius steps are deleted in the Next SP call.
    -- The Peril Fees are recalculated in spu_SIR_Recalculate_Peril_Fees

    EXEC spu_SIR_Recalculate_Peril_Fees @transaction_type_id,@risk_cnt,@insurance_file_cnt,@use_existing_fee_details,@sTransactiontype

	 IF Exists(Select Null From @ExistingFee) BEGIN

		  UPDATE policy_fee_u
			Set fee_rate_amount = exFee.fee_rate_amount,
			fee_rate_percentage = exFee.fee_rate_percentage,
			FeeTypePercent = ISNULL(exFee.Is_Percent, 0)
			From policy_fee_u pfu
			Inner Join @ExistingFee exFee
			On exFee.party_cnt = pfu.party_cnt
			And ISNULL(exFee.transaction_type_id, 0) = ISNULL(pfu.transaction_type_id, 0)
			And ISNULL(exFee.tax_group_id, 0) = ISNULL(pfu.tax_group_id, 0)
			And ISNULL(exFee.transaction_sub_type, 0) = ISNULL(pfu.transaction_sub_type, 0)
			And ISNULL(exFee.fee_rate_currency_id, 0) = ISNULL(pfu.fee_rate_currency_id, 0)
			And ISNULL(exFee.MakeLiveOptions_id, 0) = ISNULL(pfu.MakeLiveOptions_id, 0)
			And ISNULL(exFee.DoPaymentTerms_id, 0) = ISNULL(pfu.DoPaymentTerms_id, 0)
			And ISNULL(exFee.Calculation_Basis, 0) = ISNULL(pfu.Calculation_Basis, 0)
			And ISNULL(exFee.Is_Prorated, 0) = ISNULL(pfu.Is_Prorated, 0)
			And ISNULL(exFee.Is_Override, 0) = ISNULL(pfu.Is_Override, 0)
			AND ISNULL(exFee.peril_group_id,0) = ISNULL(pfu.peril_group_id,0)
			WHERE insurance_file_cnt = @insurance_file_cnt AND risk_cnt =@risk_cnt

	 END


	 
    END -- use existing fee details

    -- calculate the fee amounts
    EXEC spu_SIR_Calculate_Fee_Amounts_Wrapper @insurance_file_cnt, @risk_cnt

    -- calculate the tax amounts
    EXEC spu_SIR_Calculate_Fee_Tax_Amounts_Wrapper @insurance_file_cnt, @risk_cnt

	
GO



