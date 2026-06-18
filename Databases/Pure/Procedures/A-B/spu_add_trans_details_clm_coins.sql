EXECUTE DDLDropProcedure 'spu_add_trans_details_clm_coins'
GO
CREATE PROCEDURE spu_add_trans_details_clm_coins
    @transaction_export_folder_cnt int,
    @stats_folder_cnt int
AS

-- Declare variable for all columns in transaction details table 
DECLARE 
    -- Working variables
    @transaction_type char(10),
    @transaction_amount numeric(19, 4),
    @out_transaction_ledger_code varchar(2),
    @out_account_type_code varchar(10),
    @nominal_ledger_code varchar(2),
    @mapping_code   varchar(30),
    @out_mapping_code varchar(20),
    @previous_coinsurer_cnt int,
    @previous_coinsurer_mapping_code varchar(30),
    @premium_running_total numeric(19, 4),
    @account_suffix varchar(3),
    -- Counter
    @count int,
    @rows int,
    -- Coinsurer cursor variables
    @ci_party_cnt int,
    @ci_shortname varchar(30),
    @ci_share_percent numeric(19, 4),
    @ci_premium_total numeric(19, 4),
    @ci_sum_insured_total numeric(19, 4),
    @ci_class_of_business_code varchar(30),
    -- Output variables
    @out_claim_ref varchar(30),
  @transdetail_type_code Varchar(50)


-- Get totals from stats table
DECLARE CoinsurerCursor CURSOR FAST_FORWARD FOR
    SELECT  ri_party_cnt,
            ri_shortname,
            ri_share_percent,
            ISNULL(SUM(this_premium_original), 0),
            ISNULL(SUM(sum_insured_home), 0),
            class_of_business_code
    FROM    stats_detail
    WHERE   stats_folder_cnt = @stats_folder_cnt
    AND     stats_detail_type IN ('COI', 'TAC')
    GROUP BY
            ri_party_cnt,
            ri_shortname,
            ri_share_percent,
            class_of_business_code,
            stats_detail_type
    HAVING  ISNULL(SUM(this_premium_original), 0) <> 0
    ORDER BY        
            ri_party_cnt

-- Open cursor
OPEN CoinsurerCursor
       
-- Check if we should initialise any more data
IF (@@CURSOR_ROWS <> 0) 
BEGIN
    -- Establish the transaction type and loss code
    SELECT  @transaction_type = transaction_type_code,
            @out_claim_ref = loss_code
    FROM    stats_folder
    WHERE   stats_folder_cnt = @stats_folder_cnt
    
    -- Get the account suffix for Salvage and Recovery
    SELECT  @account_suffix =   CASE @transaction_type
                                    WHEN 'C_SA' THEN 'SAL'
                                    WHEN 'C_RV' THEN 'TPR'
                                    ELSE '' END

	IF @transaction_type = 'C_CO' OR @transaction_type = 'C_CR'  
		 SELECT @transdetail_type_code = 'CLMRICOI'
	ELSE IF @transaction_type = 'C_RV' OR @transaction_type = 'C_SA'   
		 SELECT @transdetail_type_code = 'CLRRICOI' 
	ELSE IF @transaction_type = 'C_CP'  
		SELECT @transdetail_type_code = 'CLPRICOI'    
END    

-- Get first row
FETCH NEXT FROM CoinsurerCursor
    INTO    @ci_party_cnt,
            @ci_shortname,
            @ci_share_percent,
            @ci_premium_total,
            @ci_sum_insured_total,
            @ci_class_of_business_code

WHILE (@@FETCH_STATUS = 0)
BEGIN
    Print @transaction_type

    -- Set output fields
    IF (@transaction_type IN ('C_CO', 'C_CR'))
        SELECT  @out_transaction_ledger_code = 'NO',
                @out_account_type_code = 'CLMOSCI'
    ELSE
        SELECT  @out_transaction_ledger_code = 'IN',
                @out_account_type_code = 'COINSACC'

    -- Insert detail row
    INSERT INTO Transaction_Export_Detail
           (transaction_export_folder_cnt,
            transaction_export_detail_id,
            transaction_amount,
            transaction_ledger_code,
            account_type_code,
            sum_insured_total,
            mapping_code,
            transaction_account_key,    
            spare,
            transdetail_type_code)
    SELECT  @transaction_export_folder_cnt,
            ISNULL(MAX(transaction_export_detail_id), 0) + 1,
            @ci_premium_total,
            @out_transaction_ledger_code,
            @out_account_type_code,
            @ci_sum_insured_total,
            @ci_shortname,
            @ci_party_cnt,
            @out_claim_ref,
            @transdetail_type_code
    FROM    Transaction_Export_Detail
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

    -- Set output fields
    IF @transaction_type = 'C_CP'
        SELECT  @out_transaction_ledger_code = 'NO',
                @out_account_type_code = 'CLMOSCI' + @account_suffix,   -- Current Assets \ O/S CI Claims Recovered
                @out_mapping_code = 'CLMOSCI' + @account_suffix + @ci_class_of_business_code
    ELSE
        SELECT  @out_transaction_ledger_code = 'NO',
                @out_account_type_code = 'CLMCI' + @account_suffix,     -- Income \ CI Claims Recovered
                @out_mapping_code = 'CLMCI' + @account_suffix + @ci_class_of_business_code
                
    -- Insert the Trans Export Detail 
    INSERT INTO Transaction_Export_Detail
           (transaction_export_folder_cnt,
            transaction_export_detail_id,
            transaction_amount,
            transaction_ledger_code,
            account_type_code,
            sum_insured_total,
            mapping_code,
            spare,
            transdetail_type_code)
    SELECT  @transaction_export_folder_cnt,
            ISNULL(MAX(transaction_export_detail_id), 0) + 1,
            -@ci_premium_total,
            @out_transaction_ledger_code,
            @out_account_type_code,
            @ci_sum_insured_total,
            @out_mapping_code,
            @out_claim_ref,
            @transdetail_type_code
    FROM    Transaction_Export_Detail
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

    -- Get next record
    FETCH NEXT FROM CoinsurerCursor 
        INTO    @ci_party_cnt,
                @ci_shortname,
                @ci_share_percent,
                @ci_premium_total,
                @ci_sum_insured_total,
                @ci_class_of_business_code

END

CLOSE CoinsurerCursor
DEALLOCATE CoinsurerCursor

GO


