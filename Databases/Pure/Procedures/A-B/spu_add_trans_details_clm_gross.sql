SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_add_trans_details_clm_gross'
GO

CREATE PROCEDURE spu_add_trans_details_clm_gross
    @transaction_export_folder_cnt int,  
    @stats_folder_cnt int  ,
    @is_cloned Int =0
AS  
  
    /* Declare variable for all columns in transaction details table */  
    DECLARE  
        @transaction_export_detail_id INT,  
        @transaction_amount NUMERIC(19, 4),  
        @transaction_ledger_code VARCHAR (2),  
        @account_type_code VARCHAR (20),  
        @ceded_ref VARCHAR (10),  
        @Cover_Share_Percent NUMERIC(12, 8),  
        @stats_sum_insured_total NUMERIC(19, 4),  
        @charges_total NUMERIC(19, 4),  
        @taxes_total NUMERIC(19, 4),  
        @recoveries_total NUMERIC(19, 4),  
        @commission_excluded NUMERIC(19, 4),  
        @withholding_tax_excluded NUMERIC(19, 4),  
        @spare VARCHAR(255),  
        @premium_total NUMERIC(19, 4),  
        @premium_sub_total NUMERIC(19, 4),  
        @commission_total NUMERIC(19, 4),  
        @commission_sub_total NUMERIC(19, 4),  
        @sum_insured_sub_total NUMERIC(19, 4),  
        @nominal_ledger_code VARCHAR(2),  
        @lead_agent_cnt INT,  
        @transaction_type char(10),  
        @mapping_code   VARCHAR(30),  
        @nominal_mapping_code   VARCHAR(30),  
        @transaction_account_key    INT,  
        @stats_party_cnt  INT,  
        @stats_party_mapping_code VARCHAR(30),  
        @stats_party_account_key  INT,  
        @stats_class_of_business_code VARCHAR(30),  
        @stats_share_percent  NUMERIC(19, 4),  
        @stats_party_comm_percent NUMERIC(19, 4),  
        @stats_tax_value NUMERIC(19, 4),  
        @stats_tax_type_code VARCHAR(13),  
        @stats_tax_type_id INT,  
        @nominal_ledger_account VARCHAR(20),  
        @nominal_account_prefix VARCHAR(10),  
        @transaction_type_code VARCHAR(10),  
        @ledger_short_code VARCHAR(2),  
        @temp VARCHAR(6),  
        @premium_running_total NUMERIC(19, 4),  
        @stats_document_type VARCHAR(3),  
        @stats_this_premium_original NUMERIC(19, 4),  
        @claim_ref VARCHAR(30),  
        @stats_party_source_id INT,  
        @stats_detail_type VARCHAR(3),  
        @Claim_id INT,  
        @Stats_Transaction_type_code VARCHAR(10),  
        @ri_arrangement_version INT,  
        @Base_claim_id INT,  
		@isMaintained INT,  
		@tax_account VARCHAR(50),
		@is_ex_gratia TINYINT,  
		@sgratia_account VARCHAR(30),
		@nTransdetail_type_code VARCHAR(50)
		
SELECT @Claim_id=loss_id,@Stats_Transaction_type_code=transaction_type_code FROM stats_folder WHERE Stats_folder_cnt=@Stats_folder_cnt  
SELECT @is_ex_gratia = is_ex_gratia FROM claim_payment WHERE claim_id = @claim_id AND claim_payment_id = base_claim_payment_id
SELECT @sgratia_account= value FROM system_options WHERE option_number = 5114  
SELECT @Base_claim_id = base_claim_id FROM claim WHERE Claim_id = @Claim_id  
SELECT @ri_arrangement_version=ri_arrangement_version  
FROM  
Claim_ri_arrangement where claim_id =@claim_id  
  
IF Exists(  
        Select * From claim_ri_arrangement Where claim_id in  
        (Select Claim_id From Claim Where base_claim_id=@Base_Claim_id  
        and Claim_id<@Claim_id and ri_arrangement_version=@ri_arrangement_version-1)  
        )  
        Begin  
            If @Stats_Transaction_type_code='C_CR'  
                  Set @isMaintained=1  
        End  
    
    -- Establish the transaction type.  
    SELECT  @transaction_type_code = sf.transaction_type_code,  
            @stats_document_type = SUBSTRING(sf.document_ref, 1, 3),  
            @claim_ref = c.claim_number  
    FROM    stats_folder sf,  
            claim c  
    WHERE   sf.stats_folder_cnt = @stats_folder_cnt  
    AND     c.claim_id = sf.loss_id  
  
    SELECT @nominal_account_prefix =  
        CASE @transaction_type_code  
            WHEN 'C_CO' THEN 'CLMEXP'  
            WHEN 'C_CP' THEN 'CLMRES'  
            WHEN 'C_CR' THEN 'CLMEXP'  
            WHEN 'C_SA' THEN 'CLMSAL'  
            WHEN 'C_RV' THEN 'CLMTPR'  
        END  
  
  SELECT @nTransdetail_type_code =  
        CASE @transaction_type_code  
            WHEN 'C_CO' THEN 'CLMGROSS'  
            WHEN 'C_CP' THEN 'CLPGROSS'  
            WHEN 'C_CR' THEN 'CLMGROSS'  
            WHEN 'C_SA' THEN 'CLRGROSS'  
            WHEN 'C_RV' THEN 'CLRGROSS'  
        END   
    -- Get details from stats table  
    DECLARE StatsCursor CURSOR FAST_FORWARD FOR  
        SELECT  p.party_cnt,  
                d.ri_shortname,  
                0,  
                p.source_id,  
                d.ri_share_percent,  
                d.this_premium_original,  
                d.commission_percent,  
                d.sum_insured_home,  
                d.tax_value,  
                d.tax_type_code,  
				d.tax_type_id,  
                d.class_of_business_code,  
				d.stats_detail_type  
        FROM    stats_detail d  
        LEFT JOIN  
                party p ON p.party_cnt = d.ri_party_cnt  
        WHERE   d.stats_folder_cnt = @stats_folder_cnt  
        AND    (d.stats_detail_type = 'GRS'  
             OR d.stats_detail_type = CASE WHEN @transaction_type_code IN ('C_SA', 'C_RV') THEN 'TAN' ELSE 'TAG' END)  
  
    OPEN StatsCursor  
    FETCH NEXT FROM StatsCursor INTO  
        @stats_party_cnt,  
        @stats_party_mapping_code,  
        @stats_party_account_key,  
        @stats_party_source_id,  
        @stats_share_percent,  
        @stats_this_premium_original,  
        @stats_party_comm_percent,  
        @stats_sum_insured_total,  
        @stats_tax_value,  
        @stats_tax_type_code,  
        @stats_tax_type_id,  
        @stats_class_of_business_code,  
     @stats_detail_type  
  
    WHILE (@@FETCH_STATUS = 0)  
    BEGIN  
  
        SELECT @stats_party_cnt = ISNULL(@stats_party_cnt, 0)  
           
        -- Get account_type_code  
        IF @stats_party_cnt <> 0  AND @transaction_type_code <> 'C_CR'  
        BEGIN  
            SELECT  @stats_party_account_key = @stats_party_cnt  
  
            SELECT  @ledger_short_code = l.ledger_short_name  
            FROM    Ledger l  
            JOIN    Account a ON l.ledger_id = a.ledger_id  
            WHERE   a.account_key = @stats_party_account_key  
  
            SELECT @ledger_short_code =  
                CASE @ledger_short_code  
                    WHEN 'SA' THEN 'SL'  
                    WHEN 'PU' THEN 'PL'  
                    ELSE @ledger_short_code  
                END  
        END  
        ELSE  
        BEGIN  
            SELECT @ledger_short_code = 'NO'  
        END  
  
        -- Establish account_type_code  
        IF @ledger_short_code = 'NO'  
        BEGIN  
            SELECT @account_type_code = SUBSTRING(@stats_party_mapping_code, 1, 6)  
        END  
        ELSE  
        BEGIN  
            -- RWH(23/07/01) Include Other Party Ledgers.  
            SELECT @account_type_code =  
                CASE @ledger_short_code  
                    WHEN 'SL' THEN 'SALESLEDGR'  
                    WHEN 'AG' THEN 'AGENTLEDGR'  
                    WHEN 'CO' THEN 'AGENTLEDGR'  
                    WHEN 'PL' THEN 'PURCHLEDGR'  
                    WHEN 'IN' THEN 'INSURERLED'  
                    WHEN 'UB' THEN 'SUBAGENTLD'  
                    WHEN 'OR' THEN 'OTRECLEDGR'  
                    WHEN 'OP' THEN 'OTPAYLEDGR'  
                END  
        END  
  
        SELECT  
            @transaction_amount = CASE  
                WHEN @transaction_type_code IN ('C_SA', 'C_RV')  
                    THEN @stats_this_premium_original  
                    ELSE -@stats_this_premium_original  
                END,  
            @spare = @claim_ref,  
            @transaction_ledger_code = @ledger_short_code  
  
        -- Adjust the account for sign of payment/receipt  
        IF @account_type_code = 'CLMPAY'  
        BEGIN  
            -- RWH(24/09/01) If this is a payment transaction we credit the Payable account  
            -- irrespective of the sign of the payment.  
            IF @transaction_type_code <> 'C_CP'  
            BEGIN  
                IF @transaction_amount < 0  
                    SELECT  @stats_party_mapping_code = 'CLMPAYABLE'  
                ELSE  
                    SELECT  @account_type_code = 'CLMREC',  
                            @stats_party_mapping_code = 'CLMRECEIVABLE'  
            END  
        END  
        ELSE IF @account_type_code = 'CLMREC'  
        BEGIN  
            IF @transaction_type_code = 'C_CP'  
            BEGIN  
                IF @transaction_amount > 0  
                    SELECT  @stats_party_mapping_code = 'CLMRECEIVABLE'  
                ELSE  
                    SELECT  @account_type_code = 'CLMPAY',  
                            @stats_party_mapping_code = 'CLMPAYABLE'  
            END  
        END  
  
        if @stats_this_premium_original <> 0 or @isMaintained=1 Or @is_cloned = 1  
        BEGIN  
            UPDATE  stats_folder  
 SET     premium_total = ISNULL(premium_total,0) + @transaction_amount  
            WHERE   stats_folder_cnt = @stats_folder_cnt  
  
            -- SAME HERE  
            UPDATE  transaction_export_folder  
            SET     premium_total = ISNULL(premium_total,0) + @transaction_amount  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
            -- Set transaction_detail_id  
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
            FROM    Transaction_Export_Detail  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
    IF @stats_detail_type = 'TAG' AND @transaction_type_code NOT IN ('C_SA', 'C_RV')  

	    SELECT  @nominal_account_prefix = @account_type_code,  
                        @nominal_ledger_account = @stats_party_mapping_code,  
                        @account_type_code = 'CLMPAY',  
                        @stats_party_mapping_code = 'CLMPAYABLE',  
						@nTransdetail_type_code = 'CLPTAX' 

         IF @stats_detail_type IN ('TAG', 'TAN') 
				SELECT @nTransdetail_type_code =  
						CASE @transaction_type_code  
							WHEN 'C_CP' THEN 'CLPTAX'  
							WHEN 'C_SA' THEN 'CLRTAX'  
							WHEN 'C_RV' THEN 'CLRTAX'  
						END 
						
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
                    transaction_account_key,  
                    spare,
                    transdetail_type_code)    
            VALUES (@transaction_export_folder_cnt,  
                    @transaction_export_detail_id,  
                    @transaction_amount,  
                    @transaction_ledger_code,  
                    @account_type_code,  
                    @ceded_ref,  
                    @Cover_Share_Percent,  
                    @stats_sum_insured_total,  
                    @charges_total,  
                    @taxes_total,  
                    @recoveries_total,  
                    @commission_excluded,  
                    @withholding_tax_excluded,  
                    @stats_party_mapping_code,  
                    @stats_party_account_key,  
                    @spare,
                    @nTransdetail_type_code)   
  
      IF @stats_detail_type NOT IN ('TAG', 'TAN')  
             SELECT @nominal_ledger_account = @nominal_account_prefix + @stats_class_of_business_code  
  
            SELECT  @transaction_amount = CASE  
                        WHEN @transaction_type_code IN ('C_SA', 'C_RV')  
                        THEN -@stats_this_premium_original  
                        ELSE @stats_this_premium_original  
                    END,  
                    @transaction_ledger_code = 'NO'  
  
            IF @stats_detail_type = 'TAG'  
                SELECT @account_type_code = 'LIABTAX'  
  
            -- If we are posting TAN lines from S&R ensure they contra to the receivable account  
 
   IF @stats_detail_type = 'GRS'  
    SET @tax_account = @stats_party_mapping_code  
  
            IF @stats_detail_type = 'TAN'  
                SELECT  @account_type_code = 'CLMREC',  
                        @nominal_ledger_account = @tax_account  
  
            IF  @nominal_account_prefix = 'CLMEXP'  
             SELECT @account_type_code = 'CLMEXP'  
  
   IF  @transaction_type_code ='C_SA'  
     SELECT @account_type_code = 'CLMSAL'  
  
            IF  @transaction_type_code ='C_RV'  
     SELECT @account_type_code = 'CLMTPR'  
  
            /* Write corresponding expense record to nominal ledger */  
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
            FROM    Transaction_Export_Detail  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
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
                    spare,
                    transdetail_type_code)   
            VALUES (@transaction_export_folder_cnt,  
                    @transaction_export_detail_id,  
                    @transaction_amount,  
                    @transaction_ledger_code,  
                    @account_type_code,  
                    @ceded_ref,  
                    @cover_share_percent,  
                    @stats_sum_insured_total,  
                    @charges_total,  
                    @taxes_total,  
                    @recoveries_total,  
                    @commission_excluded,  
                    @withholding_tax_excluded,  
                    @nominal_ledger_account,  
                    @transaction_account_key,  
                    @spare,
                    @nTransdetail_type_code)  
  
        END  
   IF @stats_detail_type = 'GRS' AND @transaction_type_code = 'C_CP' AND @is_ex_gratia=1
	 BEGIN

            SELECT @nominal_ledger_account = 'CLMEXP' + @stats_class_of_business_code, @transaction_amount = -@stats_this_premium_original,

                    @transaction_ledger_code = 'NO', @account_type_code = 'CLMEXP'

            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1

            FROM    Transaction_Export_Detail

            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt


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

            VALUES (@transaction_export_folder_cnt,

                    @transaction_export_detail_id,

                    @transaction_amount,

                    @transaction_ledger_code,

                    @account_type_code,

                    @ceded_ref,

                    @cover_share_percent,

                    @stats_sum_insured_total,

                    @charges_total,

                    @taxes_total,

                    @recoveries_total,

                    @commission_excluded,

                    @withholding_tax_excluded,

                    @nominal_ledger_account,

                    @transaction_account_key,

                    @spare)

            SELECT @nominal_ledger_account = @sgratia_account,@transaction_amount = @stats_this_premium_original

            SELECT @transaction_ledger_code = 'NO', @account_type_code = 'CLMEXP'

            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1

            FROM    Transaction_Export_Detail

            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt


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

            VALUES (@transaction_export_folder_cnt,

                    @transaction_export_detail_id,

                    @transaction_amount,

                    @transaction_ledger_code,

                    @account_type_code,

                    @ceded_ref,

                    @cover_share_percent,

                    @stats_sum_insured_total,

                    @charges_total,

                    @taxes_total,

                    @recoveries_total,

                    @commission_excluded,

                    @withholding_tax_excluded,

                    @nominal_ledger_account,

                    @transaction_account_key,

                    @spare)
	END

        -- Get next row  
        FETCH NEXT FROM StatsCursor INTO  
            @stats_party_cnt,  
            @stats_party_mapping_code,  
            @stats_party_account_key,  
            @stats_party_source_id,  
            @stats_share_percent,  
            @stats_this_premium_original,  
            @stats_party_comm_percent,  
            @stats_sum_insured_total,  
            @stats_tax_value,  
            @stats_tax_type_code,  
            @stats_tax_type_id,  
            @stats_class_of_business_code,  
      @stats_detail_type  
  
    END  
  
    CLOSE StatsCursor  
    DEALLOCATE StatsCursor  

GO


