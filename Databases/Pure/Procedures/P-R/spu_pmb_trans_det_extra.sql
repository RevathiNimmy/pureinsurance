SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_extra'
GO


CREATE PROCEDURE spu_pmb_trans_det_extra
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @total_extras MONEY OUTPUT,
    @total_extras_commission MONEY OUTPUT
AS

DECLARE 
    @transaction_export_detail_id INT,
    @transaction_amount MONEY,
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
    @mapping_code VARCHAR(20),
    @transaction_account_key INT,
    @spare VARCHAR(255),
    @effective_date DATETIME,
    @this_premium MONEY,
    @tax_amount MONEY,
    @fee_percentage FLOAT,
    @insurance_file_cnt INT,
    @risk_code INT,
    @return_status INT,
    @fee_party INT,
    @fee_party_id INT,
    @fee_source_id SMALLINT,
    @fee_shortname VARCHAR(225),
    @fee_amount MONEY,
    @fee_tax_amount MONEY,
    @total_fee MONEY,
    @fee_commission_percentage FLOAT,
    @fee_commission_amount MONEY,
    @fee_commission_tax MONEY,
    @fee_commission_total MONEY,
    @fee_id INT,
    @trans_fee_gross_amount MONEY,
    @trans_fee_net_amount MONEY,
    @trans_fee_commission_amount MONEY,
    @branch_id INT,
    @suspended TINYINT,
    @release_to_income TINYINT,
    @fee_tax_type_code VARCHAR(20), 
    @fee_tax_account_mapping_code VARCHAR(20),
    @fee_tax_account_key INT,
    @transdetail_type_code VARCHAR(20),
    @tax_calculation_amount MONEY

SELECT @total_extras = 0
SELECT @total_extras_commission = 0

SELECT  
    @insurance_file_cnt = ei.insurance_file_cnt,
    @this_premium = ROUND(ei.this_premium, 2),
    @tax_amount = ROUND(ei.tax_amount, 2),  
    @risk_code = ei.risk_code_id,
    @branch_id = ei.source_id
FROM transaction_export_folder tef
JOIN event_insurance_file ei
    ON ei.insurance_file_cnt = tef.insurance_file_cnt
WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt

SELECT 
    @suspended = 0,
    @release_to_income = 0 
 
/* Get amounts from Policy Fees */
DECLARE f_amounts CURSOR FAST_FORWARD FOR
    SELECT 
        epf.party_cnt,
        p.party_id,
        p.source_id,
        p.shortname,
        epf.fee_percentage,
        ROUND(epf.fee_amount, 2),
        ROUND(epf.tax_amount, 2),
        ROUND(epf.total_fee, 2),
        epf.commission_percentage,
        ROUND(epf.commission_amount, 2),
        ROUND(epf.commission_tax_amount, 2),
        ROUND(epf.total_commission, 2),
        epf.policy_fee_id 
    FROM event_policy_fee epf
    JOIN party p
        ON p.party_cnt = epf.party_cnt
    JOIN party_type pt
        ON pt.party_type_id = p.party_type_id
        AND pt.code = 'EX'
    WHERE epf.insurance_file_cnt = @insurance_file_cnt

SELECT @trans_fee_gross_amount = 0
SELECT @trans_fee_net_amount = 0
SELECT @trans_fee_commission_amount = 0
 

/* Open the Amounts Cursor */
OPEN f_amounts
FETCH NEXT FROM f_amounts INTO
    @fee_party,
    @fee_party_id,
    @fee_source_id,
    @fee_shortname,
    @fee_percentage,
    @fee_amount,
    @fee_tax_amount,
    @total_fee,
    @fee_commission_percentage,
    @fee_commission_amount,
    @fee_commission_tax,
    @fee_commission_total, 
    @fee_id

WHILE @@FETCH_STATUS = 0
BEGIN

    /*accumulate values */
    IF @fee_amount = 0
    BEGIN
        SELECT @fee_amount = ROUND(@fee_percentage * @this_premium / 100, 2)
    END
    
    SELECT @trans_fee_gross_amount = @fee_amount
    
    IF @trans_fee_gross_amount < 0 
    BEGIN
        SELECT @trans_fee_gross_amount = @trans_fee_gross_amount * -1
        SELECT @fee_tax_amount = @fee_tax_amount * -1    
    END

    SELECT @trans_fee_gross_amount = @trans_fee_gross_amount + @fee_tax_amount
    IF @fee_commission_total = 0
    BEGIN
        SELECT @fee_commission_total = ROUND(@fee_commission_percentage * @fee_amount / 100, 2) + @fee_commission_tax
    END

    SELECT @trans_fee_commission_amount = @fee_commission_total
         
    IF @trans_fee_commission_amount > 0 
    BEGIN
        SELECT @trans_fee_commission_amount = @trans_fee_commission_amount * -1
    END

    SELECT @trans_fee_net_amount = @trans_fee_gross_amount - (@trans_fee_commission_amount * -1)
    SELECT @total_extras = @total_extras + @trans_fee_net_amount
    SELECT @total_extras_commission = @total_extras_commission + (@trans_fee_commission_amount * -1)

    /*Datasure calculate tax on insurers premium */
    SELECT @tax_calculation_amount =
    (SELECT ISNULL(SUM(ROUND(ETC.value, 2)),0)
    FROM  Event_Tax_Calculation ETC,
        Transaction_Export_Folder   T 
    WHERE ETC.Insurance_file_cnt = T.insurance_file_cnt
    AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt
    AND ETC.TransType = 'TTF' AND ETC.policy_fee_id = @fee_id)
    
    SELECT @Taxes_Total = @tax_calculation_amount
  

 

    /* Write Out Transactions */
    /* Credit the Extras Account with Gross Premium */

    SELECT @transaction_ledger_code = 'IN'
    SELECT @account_type_code = 'INSURERLED'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = 0

    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @fee_shortname
    SELECT @transaction_account_key = @fee_party_id
    SELECT @transaction_Account_key = @fee_party
    SELECT @spare = 'GROSS'
    SELECT @transaction_amount = ROUND(ISNULL(@trans_fee_gross_amount,0), 2)

    IF @transaction_amount > 0 AND @transaction_type = 'D'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    IF @transaction_amount < 0 AND @transaction_type = 'C'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    If @transaction_amount < 0 
    BEGIN
        SELECT @taxes_total = @taxes_total * -1
    END

    /* Set new detail_id */
    SELECT 
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF @transaction_export_detail_id IS NULL
    BEGIN
        SELECT @transaction_export_detail_id = 1
    END
    
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
        spare,
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
        @spare,
        'GROSS'
    )

    /* Debit the Extras Account with Commission */

    SELECT @transaction_ledger_code = 'IN'
    SELECT @account_type_code = 'INSURERLED'
    SELECT @ceded_ref = NULL
    SELECT @cover_share_percent = 0
    SELECT @sum_insured_total = 0
    SELECT @charges_total = 0
    SELECT @taxes_total = 0
    SELECT @recoveries_total = 0
    SELECT @commission_excluded = 0
    SELECT @withholding_tax_excluded = 0
    SELECT @mapping_code = @fee_shortname
    SELECT @transaction_account_key = @fee_party
    SELECT @spare = 'COMM'
    SELECT @transaction_amount = @trans_fee_commission_amount

    /*Datasure calculate tax on insurers premium */
    SELECT @tax_calculation_amount =
    (SELECT ISNULL(SUM(ROUND(ETC.value, 2)),0)
    FROM  Event_Tax_Calculation ETC,
        Transaction_Export_Folder   T 
    WHERE ETC.Insurance_file_cnt = T.insurance_file_cnt
    AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt
    AND ETC.TransType = 'TTFC' AND ETC.policy_fee_id = @fee_id)
    
    SELECT @Taxes_Total = @tax_calculation_amount

    IF @transaction_amount < 0 AND @transaction_type = 'D'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    IF @transaction_amount > 0 AND @transaction_type = 'C'
    BEGIN
        SELECT @transaction_amount = @transaction_amount * -1
    END
    If @transaction_amount < 0 
    BEGIN
        SELECT @taxes_total = @taxes_total * -1
    END

    SELECT  
        @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM transaction_export_detail
    WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
    
    IF @transaction_export_detail_id IS NULL
    BEGIN
        SELECT @transaction_export_detail_id = 1
    END
    
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
        spare,
        suspended,
        release_to_income,
        release_account_code,
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
        @spare,
        @suspended,
        @release_to_income,
        NULL,
        'COMM'
    )

    /* Fetch Next */
    FETCH NEXT FROM f_amounts INTO
        @fee_party,
        @fee_party_id,
        @fee_source_id,
        @fee_shortname,
        @fee_percentage,
        @fee_amount,
        @fee_tax_amount,
        @total_fee,
        @fee_commission_percentage,
        @fee_commission_amount,
        @fee_commission_tax,
        @fee_commission_total, 
        @fee_id
END

CLOSE f_amounts
DEALLOCATE f_amounts

GO


