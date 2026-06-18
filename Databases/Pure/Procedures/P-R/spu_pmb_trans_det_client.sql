SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_client'
GO


CREATE PROCEDURE spu_pmb_trans_det_client
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
    @party_cnt INT,
    @party_id INT,
    @shortname VARCHAR(255),
    @this_premium MONEY,
    @ipt_amount MONEY,
    @vat_amount MONEY,
    @branch_id INT,
    @suspended TINYINT,
    @release_to_income TINYINT,
    @transdetail_type_code VARCHAR(20)



/*Get client details*/
SELECT  
    @party_cnt = t.insurance_holder_cnt,
    @party_id = p.party_cnt,
    @shortname = p.shortname,
    @this_premium = ROUND(i.this_premium, 2),
    @ipt_amount = ROUND(i.tax_amount, 2),
    @vat_amount = 0,
    @branch_id = i.source_id
FROM transaction_export_folder t
JOIN event_insurance_file i
    ON i.insurance_file_cnt = t.insurance_file_cnt
JOIN party p
    ON p.party_cnt = t.insurance_holder_cnt
WHERE t.transaction_export_folder_cnt = @transaction_export_folder_cnt

/*FSA Phase 3.2 Suspension Information */
SELECT @suspended = 0
SELECT @release_to_income = 0
SELECT @Transdetail_type_code = 'NET'

/*Post Transactions to the Client*/

/*Set client fields*/
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
SELECT @mapping_code = @shortname
SELECT @transaction_account_key = @party_id

--PN34751/040607
IF @transaction_type = 'C' 
BEGIN
SELECT @total_insurer_fee_calc = (@total_insurer_fee_calc * -1)
END

/*Extras and discount are applied before the amount is signed correctly and fees after.*/
SELECT @transaction_amount = @this_premium + @total_extras_gross - @total_discount_calc + @total_insurer_fee_calc
 
IF @transaction_amount < 0 AND @transaction_type = 'D' AND (@this_premium + @total_extras_gross) <> 0
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
    @transaction_export_detail_id = isnull(MAX(transaction_export_detail_id),0) + 1
FROM Transaction_Export_Detail
WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
        
/*Insert client record*/
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
    @suspended,
    @release_to_income,
    NULL,
    @transdetail_type_code
)


GO


