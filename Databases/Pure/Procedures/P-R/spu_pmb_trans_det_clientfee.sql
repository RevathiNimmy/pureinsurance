SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_clientfee'
GO


CREATE PROCEDURE spu_pmb_trans_det_clientfee
    @transaction_export_folder_cnt INT,
    @transaction_party_key INT,
    @fee_account_key INT,
    @fee_amount MONEY,
    @total_fee_amount MONEY,
    @tax_amount MONEY,
    @vat_amount MONEY
AS

DECLARE 
    @transaction_export_detail_id INT,
    @transaction_amount MONEY,
    @transaction_ledger_code CHAR(2),
    @account_type_code VARCHAR(11),
    @mapping_code VARCHAR(20),
    @branch_id INT,
    @suspended TINYINT,
    @release_to_income TINYINT,
    @transdetail_type_code VARCHAR(20)

/* Get Transaction Party File Details */
SELECT 
    @mapping_code = shortname,
    @branch_id = source_id
FROM party       
WHERE party_cnt = @transaction_party_key

SELECT 
    @suspended = 1,
    @release_to_income = 1 

/* Debit The Client */

SELECT 
    @transaction_ledger_code = 'SL',
    @account_type_code = 'SALESLEDGR',
    @transdetail_type_code = 'NET'

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
    transdetail_type_code
)
VALUES 
(
    @transaction_export_folder_cnt,
    @transaction_export_detail_id,
    @total_fee_amount,
    @transaction_ledger_code,
    @account_type_code,
    NULL,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    @mapping_code,
    @transaction_party_key,
    @transdetail_type_code
)

/* Get Transaction Party File Details */
SELECT
    @mapping_code = shortname
FROM party
WHERE party_cnt = @fee_account_key


/* Credit the Fee Account */

SELECT  
    @transaction_ledger_code = 'FE',
    @account_type_code =  'FEELEDGR',
    @transdetail_type_code = 'CFEE'

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
    suspended,
    release_to_income,
    release_account_code,
    transdetail_type_code
)
VALUES 
(
    @transaction_export_folder_cnt,
    @transaction_export_detail_id,
    @fee_amount * -1,
    @transaction_ledger_code,
    @account_type_code,
    NULL,
    0,
    0,
    0,
    @tax_amount,
    0,
    0,
    0,
    @mapping_code,
    @fee_account_key,
    @suspended,
    @release_to_income,
    NULL,
    @transdetail_type_code
)
 
IF ISNULL(@vat_amount,0) <> 0
BEGIN
/* Credit the Vat Account */

SELECT  @transaction_ledger_code = 'NO'
SELECT  @account_type_code = 'VAT'

/* Set new detail_id */

SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
FROM    Transaction_Export_Detail
WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
IF @transaction_export_detail_id IS NULL
    SELECT @transaction_export_detail_id = 1

INSERT INTO Transaction_Export_Detail (
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
    transaction_account_key)
VALUES (
    @transaction_export_folder_cnt,
    @transaction_export_detail_id,

    @vat_amount * -1,
    @transaction_ledger_code,

    @account_type_code,

    NULL,
    0,

    0,
    0,
    0,

    0,
    0,
    0,
    'VatAccount',
    0)
END 
GO


