SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_add_trans_export_detail_clm'
GO


CREATE PROCEDURE spu_add_trans_export_detail_clm
    @transaction_export_folder_cnt int,
    @stats_folder_cnt int,
    @Debit_Transaction_Ledger_Code varchar(2),
    @Debit_Account_Type_Code varchar(10),
    @Credit_Transaction_Ledger_Code varchar(2),
    @Credit_Account_Type_Code varchar(10),
    @spare varchar(255)
AS

/*************************************************************************************************
* Name: sp_add_trans_export_detail_clm
*
* Version: 1.00.0000
*
***************************************************************************************************/
BEGIN

-- Declare variable for all columns in transaction details table
DECLARE @transaction_export_detail_id   int,
    @transaction_amount         numeric(19, 4),
    @ceded_ref          varchar (10),
    @cover_share_percent        numeric(12, 8),
    @sum_insured_total      numeric(19, 4),
    @charges_total          numeric(19, 4),
    @taxes_total            numeric(19, 4),
    @recoveries_total       numeric(19, 4),
    @commission_excluded        numeric(19, 4),
    @withholding_tax_excluded   numeric(19, 4),
    @peril_type_code        varchar(10),
    @mapping_code           varchar(10),
    @transaction_account_key    int

-- Declare additional variables required for processing
DECLARE @premium_total          numeric(19, 4),
    @premium_sub_total      numeric(19, 4),
    @commission_total       numeric(19, 4),
    @commission_sub_total       numeric(19, 4),
    @sum_insured_sub_total      numeric(19, 4),
    @discount_total         numeric (9,4),
    @tax_total          numeric (9,4),
    @agent_cnt          int,
    @lead_agent_cnt         int,
    @insurance_holder_cnt       int,
    @transaction_type       char(10),
    @lead_agent_shortname       varchar(30),
    @insurance_holder_shortname varchar(30),
    @lead_agent_account_key     int,
    @insurance_holder_account_key   int,
    @class_of_business_code     varchar(10)

    -- Determine transaction type
    SELECT  @lead_agent_cnt = agent_cnt,
            @lead_agent_account_key = agent_account_key,
            @lead_agent_shortname = agent_shortname,
            @insurance_holder_cnt = insurance_holder_cnt,
            @insurance_holder_account_key = insurance_holder_account_key,
            @insurance_holder_shortname = insurance_holder_shortname
    FROM    Transaction_Export_Folder T
    WHERE   T.transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF @@Error <> 0
        GOTO Err_Add_Trans_Export_Details

    SELECT  @cover_share_percent = 0,
            @sum_insured_total = 0,
            @charges_total = 0,
            @taxes_total = 0,
            @recoveries_total = 0,
            @commission_excluded = 0,
            @withholding_tax_excluded = 0

    -- Get totals from stats table
    SELECT  @transaction_amount = sum_insured_total FROM stats_detail
    WHERE   stats_folder_cnt = @stats_folder_cnt
    AND     stats_detail_type = 'CLM'

    IF @@Error <> 0
        GOTO Err_Add_Trans_Export_Details

    -- get mapping code
    IF @lead_agent_cnt is Null
        SELECT  @mapping_code = @insurance_holder_shortname,
                @transaction_account_key = @insurance_holder_account_key

    ELSE
        SELECT  @mapping_code = @lead_agent_shortname,
                @transaction_account_key = @lead_agent_account_key

    -- Set transaction_detail_id
    SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM    Transaction_Export_Detail
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF @@Error <> 0
        GOTO Err_Add_Trans_Export_Details

    IF  @transaction_export_detail_id is NULL
        SELECT  @transaction_export_detail_id = 1

    -- credit account
    INSERT INTO Transaction_Export_Detail
            (transaction_export_folder_cnt,
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
            spare)

    VALUES
            (@transaction_export_folder_cnt,
            @transaction_export_detail_id,
            @transaction_amount,
            @Credit_Transaction_Ledger_Code,
            @Credit_Account_Type_Code,
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
            @spare)

    IF @@Error <> 0
        GOTO Err_Add_Trans_Export_Details

    -- negate transaction amount
    SELECT @transaction_amount = @transaction_amount * -1

    -- Set transaction_detail_id
    SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
    FROM    Transaction_Export_Detail
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF @@Error <> 0
        GOTO Err_Add_Trans_Export_Details

    IF  @transaction_export_detail_id is NULL
        SELECT  @transaction_export_detail_id = 1

    -- debit account
    INSERT INTO Transaction_Export_Detail
            (transaction_export_folder_cnt,
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
            spare)

    VALUES
            (@transaction_export_folder_cnt,
            @transaction_export_detail_id,
            @transaction_amount,
            @Debit_Transaction_Ledger_Code,
            @Debit_Account_Type_Code,
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
            @spare)

    IF @@Error <> 0
        GOTO Err_Add_Trans_Export_Details

/*
    -- set status to pending
    UPDATE  Transaction_Export_Folder
    SET     accounts_export_status = 'P'
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

    IF @@Error <> 0
        GOTO Err_Add_Trans_Export_Details

*/

    RETURN

Err_Add_Trans_Export_Details:
    BEGIN
        -- Delete all transactions for this folder
        DELETE FROM Transaction_Export_Detail
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        -- Delete the transactions folder record
        DELETE FROM Transaction_Export_Folder
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt

        RETURN
    END

END
GO


