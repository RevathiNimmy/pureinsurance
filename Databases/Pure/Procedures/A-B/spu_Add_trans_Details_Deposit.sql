EXECUTE DDLDropProcedure 'spu_Add_trans_Details_Deposit'
GO

CREATE PROCEDURE spu_Add_trans_Details_Deposit
    @transaction_export_folder_cnt INT,
    @stats_folder_cnt INT

AS BEGIN

DECLARE @transaction_export_detail_id   INT,
    @Deposit         NUMERIC(19, 4),
    @transaction_ledger_code    VARCHAR (2),
    @account_type_code      VARCHAR (10),
    @mapping_code           VARCHAR(10),
    @transaction_account_key    INT,
    @stats_detail_type VARCHAR(3)

/* Declare additional variables required for processing */
DECLARE 
    @agent_cnt          INT,
    @lead_agent_cnt         INT,
    @insurance_holder_cnt       INT,
    @transaction_type       char(10),
    @lead_agent_shortname       VARCHAR(30),
    @insurance_holder_shortname VARCHAR(30),
    @lead_agent_account_key     INT,
    @insurance_holder_account_key   INT,
    @lead_agent_type INT

    SELECT  @Deposit = this_premium_original
    FROM    stats_detail
    WHERE   stats_folder_cnt = @stats_folder_cnt
    AND     stats_detail_type = 'JN'
    AND     this_premium_original > 0

    IF ISNULL(@Deposit,0) != 0 BEGIN
 
        /* Determine transaction type */
        SELECT  @lead_agent_cnt = agent_cnt,
            @lead_agent_account_key = agent_account_key,
            @lead_agent_shortname = agent_shortname,
            @insurance_holder_cnt = insurance_holder_cnt,
            @insurance_holder_account_key = insurance_holder_account_key,
            @insurance_holder_shortname = insurance_holder_shortname
        FROM    Transaction_Export_Folder T
        WHERE   T.transaction_export_folder_cnt = @transaction_export_folder_cnt

        --Ledger details for client account
        SELECT  @transaction_ledger_code = 'SL',
                @account_type_code = 'SALESLEDGR'
        SELECT  @mapping_code = @insurance_holder_shortname,
               @transaction_account_key = @insurance_holder_account_key

        --Get the next transaction export ID
        SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM    Transaction_Export_Detail
        WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

        IF @transaction_export_detail_id IS NULL
            SET @transaction_export_detail_id = 1 
      --Insert the record
       INSERT INTO Transaction_Export_Detail (
            transaction_export_folder_cnt,
            transaction_export_detail_id,
            transaction_amount,
            transaction_ledger_code,
            account_type_code,
            mapping_code,
            transaction_account_key,
            transdetail_type_code)
        VALUES  (
            @transaction_export_folder_cnt,
            @transaction_export_detail_id,
            @Deposit *-1,
            @transaction_ledger_code,
            @account_type_code,
            @mapping_code,
            @transaction_account_key,
            'TRANS')

        --Same again, but positive
        --Get the next transaction export ID
        SELECT  @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
        FROM    Transaction_Export_Detail
        WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
 
       --Insert the record
       INSERT INTO Transaction_Export_Detail (
            transaction_export_folder_cnt,
            transaction_export_detail_id,
            transaction_amount,
            transaction_ledger_code,
            account_type_code,
            mapping_code,
            transaction_account_key,
            transdetail_type_code)
        VALUES  (
            @transaction_export_folder_cnt,
            @transaction_export_detail_id,
            @Deposit,
            @transaction_ledger_code,
            @account_type_code,
            @mapping_code,
            @transaction_account_key,
            'TRANS')
    END


END


GO

