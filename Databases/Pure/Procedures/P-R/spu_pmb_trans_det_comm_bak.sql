SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_comm_bak'
GO


CREATE PROCEDURE spu_pmb_trans_det_comm_bak
    @transaction_export_folder_cnt int,
    @transaction_type char(1),
    @agent_amount_calc numeric(19,4),
    @total_extras_comm_calc numeric(19,4),
    @total_insurers_comm_calc numeric(19,4),
    @total_coinsurers_comm_calc numeric(19,4),
    @subagent_comm_calc numeric(19,4)
AS


BEGIN
DECLARE @transaction_export_detail_id   int,
    @transaction_amount         numeric(19, 4),
    @transaction_ledger_code    char(2),
    @account_type_code      varchar(10),
    @ceded_ref          varchar(10),
    @cover_share_percent        numeric(12, 8),
    @sum_insured_total      numeric(19, 4),
    @charges_total          numeric(19, 4),
    @taxes_total            numeric(19, 4),
    @recoveries_total       numeric(19, 4),
    @commission_excluded        numeric(19, 4),
    @withholding_tax_excluded   numeric(19, 4),
    @mapping_code           varchar(20),
    @source_id          smallint,
    @transaction_account_key    int,
    @insurer_amount         numeric(19, 4),
    @premium_amount         numeric(19, 4),
    @tax_amount         numeric(19, 4),
    @commission_amount      numeric(19, 4),
    @insurance_file_cnt     int,
    @return_status          int
/* Get amounts from Insurance File */
SELECT  @premium_amount = this_premium,
    @tax_amount = tax_amount,
    @commission_amount = commission_amount,
    @source_id = I.source_id,
    @insurance_file_cnt = I.insurance_file_cnt
FROM    Event_Insurance_File        I,
    Transaction_Export_Folder   T
WHERE   I.insurance_file_cnt = T.insurance_file_cnt
AND T.transaction_export_folder_cnt = @transaction_export_folder_cnt

/* Insert the Trans Export Details */

/* Credit the Commission Account */
SELECT  @transaction_ledger_code = 'CO'
SELECT  @account_type_code = 'COMMLEDGR'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = 'COMM REC'
SELECT  @transaction_account_key = NULL
/* Set transaction amount */
SELECT  @commission_amount = @total_insurers_comm_calc  + @total_coinsurers_comm_calc
                + @total_extras_comm_calc - @subagent_comm_calc

SELECT  @transaction_amount = @total_insurers_comm_calc  + @total_coinsurers_comm_calc
                + @total_extras_comm_calc - @subagent_comm_calc - @agent_amount_calc
IF @transaction_amount > 0 OR @commission_amount < @agent_amount_calc
    IF @transaction_type = "D"
        SELECT @transaction_amount = @transaction_amount * -1
IF @transaction_amount < 0 AND @commission_amount > @agent_amount_calc
    IF @transaction_type = "C"
        SELECT @transaction_amount = @transaction_amount * -1
/* Set new detail_id */
SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
FROM    Transaction_Export_Detail
WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
IF  @transaction_export_detail_id IS NULL
    SELECT  @transaction_export_detail_id = 1
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
    @transaction_account_key)

END
RETURN
Err_Add_Trans_Details:
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


