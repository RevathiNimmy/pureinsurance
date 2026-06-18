SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_claim'
GO


CREATE PROCEDURE spu_pmb_trans_det_claim
    @transaction_export_folder_cnt int,
    @transaction_party_key int,
    @insurer_account_key int,
    @fee_account_key int,
    @gross_claim numeric(19, 4),
    @fee_amount numeric(19, 4),
    @net_claim numeric(19, 4)
AS


BEGIN
DECLARE @transaction_export_detail_id   int,
        @transaction_amount         numeric(19, 4),
        @transaction_ledger_code    char(2),
        @account_type_code      varchar(11),
        @mapping_code       varchar(20)

/****** CLIENT POSTING **************/

/* Get Transaction Party File Details */
SELECT @mapping_code = shortname
FROM   Party
WHERE  party_cnt = @transaction_party_key

/* Debit The Client */

SELECT  @transaction_ledger_code = 'SL'
SELECT  @account_type_code = 'SALESLEDGR'

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
    transaction_account_key,
    transdetail_type_code)
VALUES (
    @transaction_export_folder_cnt,
    @transaction_export_detail_id,
    @net_claim,
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
    'NET')

/******INSURER  POSTING **************/

/* Get Insurer  Details */
SELECT

    @mapping_code = P.shortname
FROM    Party       P
WHERE   p.party_id = @insurer_account_key

SELECT  @transaction_ledger_code = 'IN'
SELECT  @account_type_code = 'INSURERLED'

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
    transaction_account_key,
    transdetail_type_code)
VALUES (
    @transaction_export_folder_cnt,
    @transaction_export_detail_id,
    @gross_claim,
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
    @insurer_account_key,
    'GROSS')

/*****FEE  POSTING **************/

IF @fee_amount <> 0
    BEGIN
        SELECT @mapping_code = P.shortname
        FROM    Party       P
        WHERE   p.party_id = @fee_account_key
        SELECT  @transaction_ledger_code = 'FE'
        SELECT  @account_type_code =  'FEELEDGR'

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
            transaction_account_key,
            transdetail_type_code)
        VALUES (
            @transaction_export_folder_cnt,
            @transaction_export_detail_id,
            @fee_amount,
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
            @fee_account_key,
            'FEE')
    END
END

RETURN

Err_Add_Trans_Claim:
    BEGIN
        /* Delete all transactions for this folder */

        DELETE FROM Transaction_Export_Detail
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        /* Delete the transactions folder record */
        DELETE FROM Transaction_Export_Folder
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        RETURN
    END
GO


