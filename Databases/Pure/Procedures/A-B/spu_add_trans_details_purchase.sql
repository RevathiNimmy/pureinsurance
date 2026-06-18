SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_add_trans_details_purchase'
GO


CREATE PROCEDURE spu_add_trans_details_purchase
    @transaction_export_folder_cnt INT,
    @stats_folder_cnt INT
AS

BEGIN

/* Declare variable for all columns in transaction details table */
DECLARE @nTransaction_export_detail_id INT,
    @nTransaction_amount NUMERIC(19, 4),
    @nTransaction_ledger_code CHAR(2),
    @sAccount_type_code VARCHAR(10),
    @sCeded_ref VARCHAR(10),
    @crCover_Share_Percent NUMERIC(12, 8),
    @crSum_insured_total NUMERIC(19, 4),
    @crCharges_total NUMERIC(19, 4),
    @crTaxes_total NUMERIC(19, 4),
    @crRecoveries_total NUMERIC(19, 4),
    @crCommission_excluded NUMERIC(19, 4),
    @crWithholding_tax_excluded NUMERIC(19, 4),
    @crMapping_code VARCHAR(30),
    @nTransaction_account_key INT

/* Declare additional variables required for processing */
DECLARE @crCommission_total NUMERIC(19, 4),
    @nAgent_cnt INT,
    @sNominal_ledger_code CHAR(2),
    @nlead_agent_cnt INT,
    @sTransaction_type CHAR(10)

/* Get column values required by SALES EXPORT */
SELECT  @nTransaction_ledger_code = 'P1',
    @sAccount_type_code = 'Account Type',
    @crCover_Share_Percent = 0,
    @crSum_insured_total = 0,
    @crCharges_total = 0,
    @crTaxes_total = 0,
    @crRecoveries_total = 0,
    @crCommission_excluded = 0,
    @crWithholding_tax_excluded = 0,
    @sNominal_ledger_code = 'N1'

/* Get totals from stats table */
SELECT  @crCommission_total = sum(sub_commission_value_home)
FROM    stats_detail
WHERE   stats_folder_cnt = @stats_folder_cnt
AND     stats_detail_type = 'COM'

IF @crCommission_total is NULL
    SELECT @crCommission_total = 0

/* Sub Agent transaction */
    SELECT  @nTransaction_amount = -1 * @crCommission_total

/* Set transaction_detail_id */
SELECT  @nTransaction_export_detail_id = MAX(transaction_export_detail_id) + 1
FROM    Transaction_Export_Detail
WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

IF  @nTransaction_export_detail_id is NULL
    SELECT  @nTransaction_export_detail_id = 1

/* Insert the Trans Export Detail */
if @nTransaction_amount <> 0
BEGIN
    INSERT INTO Transaction_Export_Detail
            (transaction_export_folder_cnt,
            transaction_export_detail_id,
            transaction_amount,
            transaction_ledger_code,
            account_type_code,
            ceded_ref,
            Cover_Share_Percent,
            sum_insured_total,
            charges_total,
            taxes_total,
            recoveries_total,
            commission_excluded,
            withholding_tax_excluded,
            mapping_code,
			transaction_account_key)        

    VALUES  (@transaction_export_folder_cnt,
            @nTransaction_export_detail_id,
            @nTransaction_amount,
            @nTransaction_ledger_code,
            @sAccount_type_code,
            @sCeded_ref,
            @crCover_Share_Percent,
            @crSum_insured_total,
            @crCharges_total,
            @crTaxes_total,
            @crRecoveries_total,
            @crCommission_excluded,
            @crWithholding_tax_excluded,
            @crMapping_code,
            @nTransaction_account_key)       

    /* Insert the Nominal Analysis Export Detail */
    SELECT  @nTransaction_amount = -1 * @nTransaction_amount

    /* Set transaction_detail_id */
    SELECT  @nTransaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM    Transaction_Export_Detail
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF  @nTransaction_export_detail_id is NULL
        SELECT  @nTransaction_export_detail_id = 1

    INSERT INTO Transaction_Export_Detail
            (transaction_export_folder_cnt,
            transaction_export_detail_id,
            transaction_amount,
            transaction_ledger_code,
            account_type_code,
            ceded_ref,
            Cover_Share_Percent,
            sum_insured_total,
            charges_total,
            taxes_total,
            recoveries_total,
            commission_excluded,
            withholding_tax_excluded,
            mapping_code,
            transaction_account_key)        

    VALUES  (@transaction_export_folder_cnt,
            @nTransaction_export_detail_id,
            @nTransaction_amount,
            @sNominal_ledger_code,
            @sAccount_type_code,
            @sCeded_ref,
            @crCover_Share_Percent,
            @crSum_insured_total,
            @crCharges_total,
            @crTaxes_total,
            @crRecoveries_total,
            @crCommission_excluded,
            @crWithholding_tax_excluded,
            @crMapping_code,
            @nTransaction_account_key)       

END

END
GO


