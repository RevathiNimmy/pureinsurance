SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_discount'
GO


CREATE PROCEDURE spu_pmb_trans_det_discount
    @transaction_export_folder_cnt INT,
    @transaction_type CHAR(1),
    @total_discount MONEY OUTPUT
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
    @fee_account_key INT,
    @fee_shortname VARCHAR(255),
    @fee_percentage FLOAT,
    @fee_amount MONEY,
    @tot_fee_percentage FLOAT,
    @tot_fee_amount MONEY,
    @this_premium MONEY,
    @tax_amount MONEY,
    @insurance_file_cnt INT,
    @return_status INT 

SELECT  
    @insurance_file_cnt = ei.insurance_file_Cnt,
    @this_premium = ROUND(ei.this_premium, 2),
    @tax_amount = ROUND(ei.tax_amount, 2) + ROUND(ei.vat_amount, 2)
FROM transaction_export_folder tef
JOIN event_insurance_file ei
    ON ei.insurance_file_cnt = tef.insurance_file_cnt
WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt

/* Get amounts from Policy Fees */
DECLARE f_amounts CURSOR FAST_FORWARD FOR
    SELECT
        p.party_cnt,
        p.shortname,
        epf.fee_percentage,
        ROUND(epf.fee_amount, 2)
    FROM event_policy_fee epf
    JOIN party p
        ON p.party_cnt = epf.party_cnt
    JOIN party_type pt
        ON pt.party_type_id = p.party_type_id
        AND pt.code = 'DI'
    WHERE epf.insurance_file_cnt = @insurance_file_cnt
    
    
SELECT @total_discount = 0
SELECT @tot_fee_percentage = 0
SELECT @tot_fee_amount = 0

OPEN f_amounts
FETCH NEXT FROM f_amounts INTO
    @fee_account_key,
    @fee_shortname,
    @fee_percentage,
    @fee_amount

WHILE @@FETCH_STATUS = 0
BEGIN
    /*accumulate values*/
    IF @fee_amount = 0
    BEGIN
        SELECT @fee_amount = ROUND(@fee_percentage * @this_premium / 100, 2)
    END
    
    SELECT @tot_fee_amount = @tot_fee_amount + @fee_amount

    /* Fetch Next */
    FETCH NEXT FROM f_amounts INTO
        @fee_account_key,
        @fee_shortname,
        @fee_percentage ,
        @fee_amount
END

CLOSE f_amounts
DEALLOCATE f_amounts

/* Insert the Trans Export Details */

/* Debit Fees Nominal Account */

SELECT @transaction_ledger_code = 'DI'
SELECT @account_type_code = 'DISCLEDGR'
SELECT @ceded_ref = NULL
SELECT @cover_share_percent = 0
SELECT @sum_insured_total = 0
SELECT @charges_total = 0
SELECT @taxes_total = 0
SELECT @recoveries_total = 0
SELECT @commission_excluded = 0
SELECT @withholding_tax_excluded = 0
SELECT @mapping_code = @fee_shortname
SELECT @transaction_account_key = @fee_account_key
SELECT @total_discount = @tot_fee_amount
SELECT @transaction_amount = @total_discount

/*Change sign depending on credit or debit.*/
IF @transaction_amount < 0 AND @transaction_type = 'D'
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1
END
IF @transaction_amount > 0 AND @transaction_type = 'C'
BEGIN
    SELECT @transaction_amount = @transaction_amount * -1
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
    'DIS'
)

GO


