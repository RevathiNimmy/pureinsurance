SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_det_add_NBI'
GO


CREATE PROCEDURE spu_pmb_trans_det_add_NBI
    @transaction_export_folder_cnt int
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
    @agent_shortname        varchar(255),
    @agent_account_key      int,
    @source_id          smallint,

    @transaction_account_key    int,
    @premium_amount         numeric(19, 4),
    @tax_amount         numeric(19, 4),
    @commission_amount      numeric(19, 4),
    @mbp_fee            numeric(19, 4)
/* Get amounts from Insurance File */
SELECT  @premium_amount = this_premium,
    @tax_amount = tax_amount,
    @commission_amount = commission_amount,
    @mbp_fee = 0,
    @agent_shortname = agent_shortname,
    @agent_account_key = agent_account_key,
    @source_id = I.source_id
FROM    Insurance_File          I,
    Transaction_Export_Folder   T
WHERE   I.Insurance_file_cnt = T.Insurance_File_cnt
AND     T.transaction_export_folder_cnt = @transaction_export_folder_cnt
/* Insert the Trans Export Details */
/* Debit Gross Premium to the Agent's Account */
SELECT  @transaction_ledger_code = 'SL'
SELECT  @account_type_code = 'SALESLEDGR'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = @agent_shortname
/* Set accountkey to 3 for now as there is no agent 1 */
/* SELECT  @transaction_account_key = @agent_account_key */
SELECT  @transaction_account_key = 3
/* Set transaction amount */
SELECT  @transaction_amount = @premium_amount
IF @transaction_amount < 0
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
/* Credit Commission to the Agent's Account */
SELECT  @transaction_ledger_code = 'SL'
SELECT  @account_type_code = 'SALESLEDGR'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = @agent_shortname
SELECT  @transaction_account_key = @agent_account_key
/* Set transaction amount */
SELECT  @transaction_amount = @commission_amount
IF @transaction_amount > 0
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
/* Credit Gross Premium to the Nominal Income Account */
SELECT  @transaction_ledger_code = 'GL'
SELECT  @account_type_code = 'INCOME'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = 'GROSS PM'
SELECT  @transaction_account_key = NULL
/* Set transaction amount */
SELECT  @transaction_amount = @premium_amount
IF @transaction_amount > 0
    SELECT @transaction_amount = @transaction_amount * -1
    SELECT  @transaction_export_detail_id = @transaction_export_detail_id + 1
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
/* Credit Net Premium to the NU Account */
SELECT  @transaction_ledger_code = 'GL'
SELECT  @account_type_code = 'PURCHLEDGR'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = 'NORWUNIO'
SELECT  @transaction_account_key = NULL
/* Set transaction amount */
SELECT  @transaction_amount = @premium_amount - (@tax_amount + @commission_amount + @mbp_fee)
IF @transaction_amount > 0
    SELECT @transaction_amount = @transaction_amount * -1
    SELECT  @transaction_export_detail_id = @transaction_export_detail_id + 1
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
/* Credit Tax to the NU Account */
SELECT  @transaction_ledger_code = 'GL'
SELECT  @account_type_code = 'PURCHLEDGR'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = 'NORWUNIO'
SELECT  @transaction_account_key = NULL
/* Set transaction amount */
SELECT  @transaction_amount = @tax_amount
IF @transaction_amount > 0
    SELECT @transaction_amount = @transaction_amount * -1
    SELECT  @transaction_export_detail_id = @transaction_export_detail_id + 1
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
/* Debit Net Premium to the Nominal expense Account */
SELECT  @transaction_ledger_code = 'GL'
SELECT  @account_type_code = 'EXPENSE'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = 'NET PM'
SELECT  @transaction_account_key = NULL
/* Set transaction amount */
SELECT  @transaction_amount = @premium_amount - (@tax_amount + @commission_amount + @mbp_fee)
IF @transaction_amount < 0
    SELECT @transaction_amount = @transaction_amount * -1
    SELECT  @transaction_export_detail_id = @transaction_export_detail_id + 1
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
/* Debit Tax to the Nominal expense Account */
SELECT  @transaction_ledger_code = 'GL'
SELECT  @account_type_code = 'EXPENSE'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = 'NET PM'
SELECT  @transaction_account_key = NULL
/* Set transaction amount */
SELECT  @transaction_amount = @tax_amount
IF @transaction_amount < 0
    SELECT @transaction_amount = @transaction_amount * -1
    SELECT  @transaction_export_detail_id = @transaction_export_detail_id + 1
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
/* Debit Commission to the Brokers' Commission Nominal Account */
SELECT  @transaction_ledger_code = 'GL'
SELECT  @account_type_code = 'BROKERSCOM'
SELECT  @ceded_ref = NULL
SELECT  @cover_share_percent = 0
SELECT  @sum_insured_total = 0
SELECT  @charges_total = 0
SELECT  @taxes_total = 0
SELECT  @recoveries_total = 0
SELECT  @commission_excluded = 0
SELECT  @withholding_tax_excluded = 0
SELECT  @mapping_code = 'BROKERS COMM'
SELECT  @transaction_account_key = NULL
/* Set transaction amount */
SELECT  @transaction_amount = @commission_amount
IF @transaction_amount < 0
    SELECT @transaction_amount = @transaction_amount * -1
    SELECT  @transaction_export_detail_id = @transaction_export_detail_id + 1
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
GO


