EXECUTE DDLDropProcedure 'spu_add_trans_details_coins'
GO

CREATE PROCEDURE spu_add_trans_details_coins
    @transaction_export_folder_cnt int,
    @stats_folder_cnt int
AS

/* Declare variable for all columns in transaction details table */
DECLARE 
    -- Working variables
    @nTransaction_export_detail_id INT,
    @crTransaction_amount NUMERIC(19, 4),
    -- Coinsurance cursor variables
    @nCoins_party_cnt INT,
    @sCoins_mapping_code VARCHAR(30),
    @nCoins_account_key INT,
    @crCoins_share_percent NUMERIC(19, 4),
    @crCoins_premium_total NUMERIC(19, 4),
    @coins_commission_percent numeric(19,4),
    @coins_sum_insured_total numeric(19, 4),
    @coins_tax_total numeric(19,4),
    @coins_tax_type_code varchar(13),
    @coins_tax_type_id int,
    @coins_class_of_business_code varchar(30),
    -- Output variables
    @out_transaction_ledger_code varchar (2),
    @out_account_type_code varchar (10),
    @out_mapping_code varchar(20),
    @out_spare varchar(255),
    @out_commission_total numeric(19, 4),
    @lBusiness_type_id int,              
    @sCoins_placement  varchar(10),
	@transdetail_type_code VARCHAR(50),
    @nBusinessTypeIdCoinsLead int = 3    
   
	SELECT	@sCoins_placement = ifi.coins_placement,          
			@lBusiness_type_id = ifi. business_type_id               
    FROM    insurance_file ifi  left join  stats_folder  sf
			on ifi.insurance_file_cnt = sf.insurance_file_cnt
    WHERE   sf.stats_folder_cnt = @stats_folder_cnt


/* Get totals from stats table */
DECLARE Coinsurer_Cursor CURSOR FAST_FORWARD FOR
    SELECT  p.party_cnt,
            p.shortname,        -- Account Mapping
            p.party_cnt,        -- Account Key
            d.ri_share_percent,
            isnull(sum(d.this_premium_original), 0),
            d.commission_percent,
            isnull(sum(d.sum_insured_home), 0),
            isnull(sum(d.tax_value), 0),
            d.tax_type_code,
            d.tax_type_id,
            d.class_of_business_code
    FROM    stats_detail d
    JOIN    party p ON p.party_cnt = d.ri_party_cnt
    WHERE   d.stats_folder_cnt = @stats_folder_cnt
    AND     d.stats_detail_type IN ('COI', 'TAC')
    GROUP BY    
            p.party_cnt, p.shortname, d.ri_share_percent, d.commission_percent,
            d.tax_type_code, d.tax_type_id, d.class_of_business_code
    ORDER BY    
            p.party_cnt

-- Open cursor and get first row
OPEN Coinsurer_Cursor
FETCH NEXT FROM Coinsurer_Cursor 
    INTO    @nCoins_party_cnt,
            @sCoins_mapping_code,
            @nCoins_account_key,
            @crCoins_share_percent,
            @crCoins_premium_total,
            @coins_commission_percent,
            @coins_sum_insured_total,
            @coins_tax_total,
            @coins_tax_type_code,
            @coins_tax_type_id,
            @coins_class_of_business_code

WHILE (@@FETCH_STATUS = 0)
BEGIN
    -- GET COMMISSION TOTAL
    SELECT  @out_commission_total = @crCoins_premium_total * (@coins_commission_percent / 100)

    	IF (@lBusiness_type_id = @nBusinessTypeIdCoinsLead and @sCoins_placement = 'NETT') 
 BEGIN  
  SELECT @out_commission_total=@crCoins_premium_total *-1
  SELECT @crCoins_premium_total=0  
 END
    -- IF WE HAVE TAX ADD THE TAX RECORD
    IF @coins_tax_total <> 0
    BEGIN
		SET @transdetail_type_code = 'COITAX'
        -- Get transaction_detail_id and set output fields
        SELECT  @nTransaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1,
                @out_transaction_ledger_code  = 'IN',
                @out_account_type_code = 'COINSACC',
                @out_spare = 'TAX ' + @coins_tax_type_code
        FROM    Transaction_Export_Detail
        WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt

        -- Insert record
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
        VALUES (@transaction_export_folder_cnt,
                @nTransaction_export_detail_id,
                @coins_tax_total,
                @out_transaction_ledger_code,
                @out_account_type_code,
                @coins_sum_insured_total,
                @sCoins_mapping_code,
                @nCoins_account_key, 
                @out_spare,
                @transdetail_type_code)
    END
    ELSE 
    BEGIN
        -- IF WE HAVE PREMIUM ADD THE PREMIUM RECORD
        IF @crCoins_premium_total <> 0
        BEGIN
			SET @transdetail_type_code = 'COIPREM'
            -- Get transaction_detail_id and set output fields
            SELECT  @nTransaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1,
                    @out_transaction_ledger_code  = 'IN',
                    @out_account_type_code = 'COINSACC',
                    @out_spare = 'COMM'
            FROM    Transaction_Export_Detail
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
            IF (@lBusiness_type_id = @nBusinessTypeIdCoinsLead and @sCoins_placement = 'GROSS')
	    SELECT @out_spare=null
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
            VALUES (@transaction_export_folder_cnt,
                    @nTransaction_export_detail_id,
                    -@crCoins_premium_total,
                    @out_transaction_ledger_code,
                    @out_account_type_code,
                    @coins_sum_insured_total,
                    @sCoins_mapping_code,
                    @nCoins_account_key, 
                    @out_spare,
                    @transdetail_type_code)

  --    IF NOT( (@lBusiness_type_id = @nBusinessTypeIdCoinsLead) and (@sCoins_placement = 'GROSS') )  
		--BEGIN
            -- ADD EXPENSE CONTRA RECORD
            -- Get transaction_detail_id and set output fields
            -- Write corresponding expense record to nominal ledger
            SELECT  @nTransaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1,
                    @out_transaction_ledger_code = 'NO',
                    @out_account_type_code = 'EXPCIOUT',
                    @out_mapping_code = 'NOCIOUT' +  @coins_class_of_business_code
            FROM    Transaction_Export_Detail
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
        
            INSERT INTO Transaction_Export_Detail
                   (transaction_export_folder_cnt,
                    transaction_export_detail_id,
                    transaction_amount,
                    transaction_ledger_code,
                    account_type_code,
                    sum_insured_total,
                    mapping_code,
					transdetail_type_code)
            VALUES (@transaction_export_folder_cnt,
                    @nTransaction_export_detail_id,
                    @crCoins_premium_total,
                    @out_transaction_ledger_code,
                    @out_account_type_code,
                    @coins_sum_insured_total,
                    @out_mapping_code,
                    @transdetail_type_code)
        END 
            --- IF WE HAVE COMMISSION ADD THE COMMISSION RECORD
            IF @out_commission_total <> 0
            BEGIN
				SET @transdetail_type_code = 'COICOMM'
                -- Get transaction_detail_id and set output fields
                SELECT  @nTransaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1,
                        @out_transaction_ledger_code  = 'IN',
                        @out_account_type_code = 'COINSACC',
                        @out_spare = 'COMM'
                FROM    Transaction_Export_Detail
                WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
    
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
                VALUES (@transaction_export_folder_cnt,
                        @nTransaction_export_detail_id,
                        @out_commission_total,
                        @out_transaction_ledger_code,
                        @out_account_type_code,
                        null,
                        @sCoins_mapping_code,
                        @nCoins_account_key,
                        @out_spare,
                        @transdetail_type_code)
        
                -- ADD EXPENSE CONTRA RECORD
                -- Get transaction_detail_id and set output fields
                -- Write corresponding expense record to nominal ledger
                SELECT  @nTransaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1,
                        @out_transaction_ledger_code = 'NO',
                        @out_account_type_code = 'INCCICOM',
                        @out_mapping_code = 'NOCICOM' +  @coins_class_of_business_code
                FROM    Transaction_Export_Detail
                WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
        
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
                VALUES (@transaction_export_folder_cnt,
                        @nTransaction_export_detail_id,
                        -@out_commission_total,
                        @out_transaction_ledger_code,
                        @out_account_type_code,
                        null,
                        @out_mapping_code,
                        @out_spare,
                        @transdetail_type_code)
    
            END 
       -- END 
    END 

    -- Get next record
    FETCH NEXT FROM Coinsurer_Cursor 
        INTO    @nCoins_party_cnt,
                @sCoins_mapping_code,
                @nCoins_account_key,
                @crCoins_share_percent,
                @crCoins_premium_total,
                @coins_commission_percent,
                @coins_sum_insured_total,
                @coins_tax_total,
                @coins_tax_type_code,
                @coins_tax_type_id,
                @coins_class_of_business_code

END
CLOSE Coinsurer_Cursor
DEALLOCATE Coinsurer_Cursor

GO


