SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_det_shares'
GO

CREATE PROCEDURE spu_pmb_trans_det_shares
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @total_extras_gross MONEY,
    @total_fees_calc MONEY,
    @total_discount_calc MONEY,
    @total_insurer_fee_calc MONEY
AS

DECLARE 
    @transaction_export_detail_id INT,
    @transaction_ledger_code CHAR(2),
    @account_type_code VARCHAR(10),
    @ceded_ref VARCHAR(10),
    @cover_share_percent FLOAT,
    @sum_insured_total MONEY,
    @charges_total MONEY,
    @taxes_total MONEY,
    @recoveries_total MONEY,
    @commission_excluded MONEY,
    @withholding_tax_excluded MONEY,
    @transaction_amount MONEY,
    @mapping_code VARCHAR(20),
    @transaction_account_key INT,
    @insurance_file_cnt INT,
    @ipt_amount MONEY,
    @vat_amount MONEY,
    @policy_holder_cnt INT,
    @this_premium MONEY,
    @policy_holder_source INT,
    @policy_holder_shortname VARCHAR(255),
    @party_cnt INT,
    @shortname VARCHAR(255),
    @split_percentage FLOAT,
    @split_value MONEY,
    @split_ipt MONEY,
    @split_vat MONEY,
    @split_extras MONEY,
    @split_fees MONEY,
    @split_discount MONEY,
    @split_insurer_fees MONEY,
    @policy_holder_contra MONEY,
    @policy_holder_ipt MONEY,
    @policy_holder_vat MONEY

SELECT  
    @insurance_file_cnt = ei.insurance_file_cnt,
    @policy_holder_cnt = ei.insured_cnt,
    @this_premium = ROUND(ei.this_premium, 2),
    @ipt_amount = ROUND(ei.tax_amount, 2),
    @vat_amount = ROUND(ei.vat_amount, 2)
FROM transaction_export_folder tef
JOIN event_insurance_file ei
    ON ei.insurance_file_cnt = tef.insurance_file_cnt
WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt

/* Get Policy Holder party details */
SELECT @policy_holder_contra = 0
SELECT @policy_holder_ipt = 0
SELECT @policy_holder_vat = 0

SELECT     
    @policy_holder_source = source_id,
    @policy_holder_shortname = shortname
FROM Party
WHERE party_cnt = @Policy_holder_cnt

/* Post the full debit to the policy holder */
SELECT @transaction_ledger_code = 'SL'
SELECT @account_type_code = 'SALESLEDGR'
SELECT @ceded_ref = NULL
SELECT @cover_share_percent = 0
SELECT @sum_insured_total = 0
SELECT @charges_total = @vat_amount
SELECT @taxes_total = @ipt_amount
SELECT @recoveries_total = 0
SELECT @commission_excluded = 0
SELECT @withholding_tax_excluded = 0
SELECT @mapping_code = @policy_holder_shortname
SELECT @transaction_account_key = @policy_holder_cnt

SELECT @transaction_amount = @this_premium + @total_extras_gross - @total_discount_calc + @total_insurer_fee_calc
    
IF @transaction_amount < 0 AND @transaction_type = 'D'
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1
END

IF @transaction_amount > 0 AND @transaction_type = 'C'
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1
END
        
SELECT @transaction_amount = @transaction_amount + @total_fees_calc

/*Get next export record*/
SELECT 
    @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
FROM Transaction_Export_Detail
WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

/*Insert full amount client record*/
INSERT INTO transaction_export_detail 
(
    transaction_export_folder_cnt,
    transaction_export_detail_id,
    transaction_amount,
    transaction_ledger_code,
    account_type_code,
    ceded_ref,
    cover_share_percent,
    sum_insured_total,
    charges_total,
    taxes_total,
    recoveries_total,
    commission_excluded,
    withholding_tax_excluded,
    mapping_code,
    transaction_account_key,
    transdetail_type_code
)
VALUES 
(
    @transaction_export_folder_cnt,
    @transaction_export_detail_id,
    @transaction_amount,
    @transaction_ledger_code,
    @account_type_code,
    @ceded_ref,
    @cover_share_percent,
    @sum_insured_total,
    @charges_total,
    @taxes_total,
    @recoveries_total,
    @commission_excluded,
    @withholding_tax_excluded,
    @mapping_code,
    @transaction_account_key,
    'NET'
)

DECLARE s_shares CURSOR FAST_FORWARD FOR
    SELECT 
        party_cnt = s.party_cnt,
        shortname = p.shortname,
        split_value = ROUND(s.split_value, 2)
    FROM event_policy_shared_premiums s
    JOIN party p
        ON p.party_cnt = s.party_cnt
    WHERE s.insurance_file_cnt = @insurance_file_cnt

/*Open the Shares Cursor*/
OPEN s_shares
FETCH NEXT FROM s_shares INTO
    @party_cnt,
    @shortname,
    @split_value

WHILE @@FETCH_STATUS = 0
BEGIN

    /*Calculate split premium, as stored value is rounded off to 4 decimal places*/
    SELECT
        @split_percentage = (@split_value / SUM(s.split_value)) * 100
    FROM event_policy_shared_premiums s
    WHERE s.insurance_file_cnt = @insurance_file_cnt

    /*Post Transactions to the Client*/
    SELECT @split_ipt = 0
    SELECT @split_vat = 0
    SELECT @split_extras = 0
    SELECT @split_fees = 0
    SELECT @split_discount = 0
    SELECT @split_insurer_fees = 0

    SELECT @split_ipt = ROUND(@ipt_amount * @split_percentage / 100, 2)
    SELECT @split_vat = ROUND(@vat_amount * @split_percentage / 100, 2)
    SELECT @split_extras = ROUND(@total_extras_gross * @split_percentage / 100, 2)
    SELECT @split_fees = ROUND(@total_fees_calc * @split_percentage / 100, 2)
    SELECT @split_discount = ROUND(@total_discount_calc * @split_percentage / 100, 2)
    SELECT @split_insurer_fees = ROUND(@total_insurer_fee_calc * @split_percentage / 100, 2)

    /*Credit Net Premium to the Client Account*/
    SELECT @transaction_ledger_code = 'SL'
    SELECT @account_type_code = 'SALESLEDGR'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = @split_vat
    SELECT @taxes_total = @split_ipt
    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @shortname
    SELECT @transaction_account_key = @party_cnt

    --SELECT @transaction_amount = ISNULL(@split_value, 0)  + ISNULL(@split_ipt, 0) + ISNULL(@split_vat, 0) + ISNULL(@split_extras, 0) - ISNULL(@split_discount, 0) + ISNULL(@split_insurer_fees, 0)
    SELECT @transaction_amount = ISNULL(@split_value, 0)  + ISNULL(@split_vat, 0) + ISNULL(@split_extras, 0) - ISNULL(@split_discount, 0) + ISNULL(@split_insurer_fees, 0)
    
    IF @transaction_amount < 0 AND @transaction_type = 'D'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    
    IF @transaction_amount > 0 AND @transaction_type = 'C'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
        
    SELECT @transaction_amount = @transaction_amount + ISNULL(@split_fees, 0)
            
    IF @party_cnt <> @policy_holder_cnt
    BEGIN
        
        /*Get next export record*/
        SELECT 
            @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM Transaction_Export_Detail
        WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        
        /*Insert each share record*/
        INSERT INTO transaction_export_detail 
        (
            transaction_export_folder_cnt,
            transaction_export_detail_id,
            transaction_amount,
            transaction_ledger_code,
            account_type_code,
            ceded_ref,
            cover_share_percent,
            sum_insured_total,
            charges_total,
            taxes_total,
            recoveries_total,
            commission_excluded,
            withholding_tax_excluded,
            mapping_code,
            transaction_account_key,
            transdetail_type_code
        )
        VALUES
        (
            @transaction_export_folder_cnt,
            @transaction_export_detail_id,
            @transaction_amount,
            @transaction_ledger_code,
            @account_type_code,
            @ceded_ref,
            @cover_share_percent,
            @sum_insured_total,
            @charges_total,
            @taxes_total,
            @recoveries_total,
            @commission_excluded,
            @withholding_tax_excluded,
            @mapping_code,
            @transaction_account_key,
            'NET'
        )
        
        SELECT @policy_holder_contra = @policy_holder_contra + @transaction_amount
        SELECT @policy_holder_ipt = @policy_holder_ipt + @split_ipt
        SELECT @policy_holder_vat = @policy_holder_vat +  @split_vat
    END

    /* Fetch Next */
    FETCH NEXT FROM s_shares INTO
        @party_cnt,
        @shortname,
        @split_value
END

/* Close and Deallocate Cursor */
CLOSE s_shares
DEALLOCATE s_shares

/* Post a reversal */
IF @policy_holder_contra <> 0 BEGIN

    SELECT @transaction_ledger_code = 'SL'
    SELECT @account_type_code = 'SALESLEDGR'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = 0
    SELECT @taxes_total = 0
    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @policy_holder_shortname
    SELECT @transaction_account_key = @Policy_holder_cnt
    SELECT @transaction_amount = @policy_holder_contra * -1
    SELECT @taxes_total = @policy_holder_ipt * -1
    SELECT @charges_total = @policy_holder_vat * -1
        
    /*Get next export record*/
    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM Transaction_Export_Detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        
    /*Insert contra record*/
    INSERT INTO Transaction_Export_Detail
    (
        transaction_export_folder_cnt,
        transaction_export_detail_id,
        transaction_amount,
        transaction_ledger_code,
        account_type_code,
        ceded_ref,
        cover_share_percent,
        sum_insured_total,
        charges_total,
        taxes_total,
        recoveries_total,
        commission_excluded,
        withholding_tax_excluded,
        mapping_code,
        transaction_account_key,
        transdetail_type_code
    )
    VALUES 
    (
        @transaction_export_folder_cnt,
        @transaction_export_detail_id,
        @transaction_amount,
        @transaction_ledger_code,
        @account_type_code,
        @ceded_ref,
        @cover_share_percent,
        @sum_insured_total,
        @charges_total,
        @taxes_total,
        @recoveries_total,
        @commission_excluded,
        @withholding_tax_excluded,
        @mapping_code,
        @transaction_account_key,
        'NET'
    )

END


GO


