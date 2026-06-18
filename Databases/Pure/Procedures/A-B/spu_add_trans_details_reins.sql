SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_add_trans_details_reins'
GO


CREATE PROCEDURE spu_add_trans_details_reins
    @transaction_export_folder_cnt int,  
    @stats_folder_cnt int  
AS  
    -- **************************************************************************************  
    -- spu_add_trans_details_reins creates transaction details records for
    -- reinsurance transactions export.  
    --  
    -- 2 parameters are passed in - @stats_folder_cnt,  @insurance_file_cnt  
    --  
    -- This stored procedure is called by spu_add_stats_details_control.  
    --  
    -- A failure in this procedure will be passed back to the calling procedure.  
    -- **************************************************************************************  
    -- Revision Description of Modification                                 Date        Who  
    -- -------- ---------------------------                                 ----        ---  
    -- 1.0      Original                                                    27/06/1997  TF  
    -- 1.1      transaction_ledger_id changed to transaction_ledger_code.   09/10/1997  TF  
    --          Added Code to Handle Mapping Code, Transaction Account Key  20/11/2000  RAM  
    -- 1.2      Updates to flag transactions as commission or tax.          04/04/2001  RWH  
    -- 1.3      New stats tax type 'TAT' and new nominal accounts.          30/04/2001  RWH  
    -- 1.4      More detailed account types.                                01/05/2001  RWH  
    -- 1.5      Add FAC stuff.                                              21/06/2001  RWH  
    -- 1.6      Don't apply share percent as it has already been applied  
    --          when creating stats.  Also map FAC stuff to correct ledger  
    --          and accounts.                                               11/07/2001  RWH  
    -- 1.7      Initialise @tax_value properly and check for = 0 to prevent
    --          zero value tax transactions.                                28/08/2001  RWH  
    -- 1.8      Change accounts posting for Intermediary mode, based on  
    --          new hidden option                                           03/01/2002  RAG  
    -- 1.9      Acc_type is selected by option number and branch  
    --          (ensuring unique record retrieved)                          13/06/2002  SJP  
    -- 1.10     Sundry fixes to do with FAC being calculated incorrectly    04/12/2002  Tomo  
    -- 1.11     Convert commission to Original currency to match premium  
    --          ...Not forgetting the TTY line                              19/05/2003  JMK  
    --          Remove ref to Orion_for_broking                             21/05/2003  JMK  
    -- **************************************************************************************  
  
    -- Declare variable for all columns in transaction details table  
    DECLARE  
        @transaction_export_detail_id int,  
        @transaction_amount numeric(19, 4),  
        @transaction_ledger_code varchar (2),  
        @account_type_code varchar (10),  
        @ceded_ref varchar (10),  
        @Cover_Share_Percent float,  
        @sum_insured_total numeric(19, 4),  
        @charges_total numeric(19, 4),  
        @taxes_total numeric(19, 4),  
        @recoveries_total numeric(19, 4),  
        @commission_excluded numeric(19, 4),  
        @withholding_tax_excluded numeric(19, 4),  
        @spare varchar(255),  
        @premium_total numeric(19, 4),  
        @premium_sub_total numeric(19, 4),  
        @commission_total numeric(19, 4),  
        @commission_sub_total numeric(19, 4),  
        @sum_insured_sub_total numeric(19, 4),  
        @nominal_ledger_code varchar(2),  
        @lead_agent_cnt int,  
        @transaction_type char(10),  
        @mapping_code varchar(30),
        @nominal_mapping_code varchar(30),  
        @transaction_account_key int,  
        @reinsurer_cnt int,  
        @reinsurer_mapping_code varchar(30),  
        @reinsurer_account_key int,  
        @agent_cnt int,  
        @agent_mapping_code varchar(30),  
        @agent_account_key int,  
        @class_of_business_code varchar(30),
        @share_percent float,
        @comm_share_percent float,
        @tax_value numeric(19, 4),
        @tax_type_code varchar(13),
        @nominal_ledger_account varchar(20),
        @stats_detail_type varchar(3),
        @stats_detail_type2 varchar(3),
        @stats_detail_id int,
        @acc_type varchar(1),
        @currency_rate float,
        @company_id int,
        @currency_id int,
        @insurance_file_cnt int,
        @effective_date datetime,  
	@transdetail_type_code Varchar(50),
        @nTreaty_ID int

    -- Change accounts posting for Intermediary mode
    SELECT  @acc_type = ISNULL(Acc_Type, '')
    FROM    hidden_options
    WHERE   branch_id=1
    AND     option_number=1

    -- Get insurance file
    SELECT  @insurance_file_cnt = insurance_file_cnt
    FROM    stats_folder
    WHERE   stats_folder_cnt = @stats_folder_cnt

    -- Get rate from insurance file  
    SELECT  @company_id = source_id,  
            @currency_id = currency_id,  
            @currency_rate = currency_base_xrate  
    FROM    insurance_file  
    WHERE   insurance_file_cnt = @insurance_file_cnt  
  
    -- If rate is blank then get it from rate table  
    IF ISNULL(@currency_rate ,0) = 0 BEGIN  
        SELECT @effective_date = GETDATE()  
  
        -- Get rate from currency rate table  
        EXEC spu_ACT_Get_Currency_Rate  
            @currency_id = @currency_id,  
            @company_id = @company_id,
            @effective_date = @effective_date,  
            @rate = @currency_rate OUTPUT  
    END  
  
    -- Get agent  
    SELECT  @agent_cnt = agent_cnt  
    FROM    stats_folder  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
  
    -- Get column values required by SALES EXPORT  
    SELECT  @transaction_ledger_code  = 'IN',  
            @account_type_code = 'REINSACC',  
            @Cover_Share_Percent = 0,  
            @sum_insured_total = 0,  
            @charges_total = 0,  
            @taxes_total = 0,  
            @recoveries_total = 0,  
            @commission_excluded = 0,  
            @withholding_tax_excluded = 0,  
            @nominal_ledger_code = 'NO',  
            @mapping_code = 'PLREINS',  
            @nominal_mapping_code = 'NOREINS'  
  
    -- Get totals from stats table  
    DECLARE TreatyCursor CURSOR FAST_FORWARD FOR  
        SELECT  p.party_cnt,  
                p.shortname,  
                p.party_cnt,  
                tp.share_percent,  
                -- The, now complicated, treaty commission rate  
                CASE WHEN d.is_commission_modified = 1 OR tpc.comm_split = 0 THEN  
                    tp.share_percent  
                ELSE  
                    (tp.share_percent * tp.commission_percent) / tpc.comm_split  
                END,
                SUM(ISNULL(d.this_premium_original, 0)),  
                SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate, -- Convert to original  
                SUM(ISNULL(d.sum_insured_home, 0)),  
                SUM(ISNULL(d.tax_value, 0)),  
                d.tax_type_code,  
                d.stats_detail_type,  
                d.stats_detail_id,
                tp.treaty_party_id  
        FROM    stats_detail d  
        JOIN    treaty_party tp  
                ON tp.treaty_id = d.ri_party_cnt  
        JOIN    party p  
                ON p.party_cnt = tp.party_cnt  
        JOIN   (SELECT  treaty_id,  
                       (SUM(share_percent * commission_percent) / 100) comm_split  
                FROM    treaty_party  
                GROUP BY treaty_id) tpc  
                ON tpc.treaty_id = tp.treaty_id  
        LEFT JOIN  
                party_insurer PIN  
                ON p.party_cnt = PIN.party_cnt  
  
        WHERE   d.stats_folder_cnt = @stats_folder_cnt  
        AND     d.stats_detail_type IN( 'TTY', 'TAT', 'TYX')  
        AND     p.shortname <> 'RETAINED'  
        AND     ISNULL(PIN.is_retained, 0) <>1  
        GROUP BY  
                p.party_cnt, p.shortname, tp.share_percent, d.tax_type_code, d.stats_detail_type, d.stats_detail_id,  
                d.is_commission_modified, tp.commission_percent, tpc.comm_split,tp.treaty_party_id  
  
        UNION  
  
        SELECT  p.party_cnt,  
                p.shortname,  
                p.party_cnt,  
                d.ri_share_percent,
                d.ri_share_percent,  
                SUM(ISNULL(d.this_premium_original, 0)),  
                SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate, -- Convert to original  
                SUM(ISNULL(d.sum_insured_home, 0)),  
                SUM(ISNULL(d.tax_value, 0)),  
                d.tax_type_code,  
                d.stats_detail_type,  
                d.stats_detail_id ,
                Null as treaty_party_id 
        FROM    stats_detail d  
        JOIN    party p  
                ON p.party_cnt = d.ri_party_cnt  
        LEFT JOIN  
                party_insurer PIN  
                ON p.party_cnt = PIN.party_cnt  
        WHERE   d.stats_folder_cnt = @stats_folder_cnt  
        AND     d.stats_detail_type IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')  
        AND     p.shortname <> 'RETAINED'  
        AND     ISNULL(PIN.is_retained, 0) <> 1  
        GROUP BY p.party_cnt, p.shortname, d.ri_share_percent, d.tax_type_code, d.stats_detail_type, d.stats_detail_id  
  
    -- Open the treaty cursor  
    OPEN TreatyCursor  
    FETCH NEXT FROM TreatyCursor INTO  
        @reinsurer_cnt,  
        @reinsurer_mapping_code,  
        @reinsurer_account_key,  
        @share_percent,  
        @comm_share_percent,  
        @premium_total,  
        @commission_total,  
        @sum_insured_total,  
        @tax_value,  
        @tax_type_code,  
        @stats_detail_type2,  
        @stats_detail_id,
        @nTreaty_ID
  
    WHILE (@@FETCH_STATUS = 0) BEGIN  
        -- Check for taxes  
        IF @tax_value <> 0 BEGIN  
            -- Get tax transaction amount  
            IF @stats_detail_type2 IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')  
                SELECT  @transaction_amount = @tax_value  
            ELSE  
                SELECT  @transaction_amount = @tax_value * @share_percent / 100  
  
            SELECT  @spare = 'TAX ' + @tax_type_code,  
                    @transaction_ledger_code  = 'IN',  
                    @account_type_code = 'REINSACC'  
  
  			IF @stats_detail_type2 IN ('TTC', 'TFC')
				SET @transdetail_type_code = 'REINCOMMTAX'  
            ELSE
				SET @transdetail_type_code = 'REINTAX'  
				
            -- Get transaction_detail_id  
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
            FROM    Transaction_Export_Detail  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
            INSERT INTO Transaction_Export_Detail (  
                    transaction_export_folder_cnt,  
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
                    @sum_insured_total,  
                    @charges_total,  
                    @taxes_total,  
                    @recoveries_total,  
                    @commission_excluded,  
                    @withholding_tax_excluded,  
                    @reinsurer_mapping_code,  
                    @reinsurer_account_key,  
                    @spare,
                    @transdetail_type_code)  
        END ELSE BEGIN  
            -- Check for premium  
            IF @premium_total <> 0  
            BEGIN  
                -- Apply share?  
                IF @stats_detail_type2 IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')  
                    SELECT  @transaction_amount = @premium_total  
                ELSE  
                    SELECT  @transaction_amount = @premium_total * @share_percent / 100  
  
                SELECT  @transaction_ledger_code  = 'IN',  
                        @account_type_code = 'REINSACC',
                        @transdetail_type_code = 'REINPREM'   
  
                -- Set transaction_detail_id  
                SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
                FROM    Transaction_Export_Detail  
                WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  

                INSERT INTO Transaction_Export_Detail (  
                        transaction_export_folder_cnt,  
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
                        transdetail_type_code)  
                VALUES (@transaction_export_folder_cnt,  
                        @transaction_export_detail_id,  
                        @transaction_amount,  
                        @transaction_ledger_code,  
                        @account_type_code,  
                        @ceded_ref,  
                        @Cover_Share_Percent,  
                        @sum_insured_total,  
                        @charges_total,  
                        @taxes_total,  
                        @recoveries_total,  
                        @commission_excluded,  
                        @withholding_tax_excluded,  
                        @reinsurer_mapping_code,  
                        @reinsurer_account_key,
                        @transdetail_type_code)  
  
                -- Check for commission  
                IF @commission_total <> 0 BEGIN  
                    -- Apply share?
                    IF @stats_detail_type2 IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')  
                        SELECT  @transaction_amount = -@commission_total  
                    ELSE  
                        SELECT  @transaction_amount = -@commission_total * @comm_share_percent / 100  
  
                    SELECT  @spare = 'COMM', 
							@transdetail_type_code = 'REINCOMM'   
  
                    -- Set transaction_detail_id  
                    SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
                    FROM    Transaction_Export_Detail  
                    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
                    INSERT INTO Transaction_Export_Detail (  
                            transaction_export_folder_cnt,  
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
                            spare,  
                            transaction_account_key,
                            transdetail_type_code)  
                    VALUES (@transaction_export_folder_cnt,  
                            @transaction_export_detail_id,  
                            @transaction_amount,  
                            @transaction_ledger_code,  
                            @account_type_code,  
                            @ceded_ref,
							@Cover_Share_Percent,  
                            null,  
                            @charges_total,  
                            @taxes_total,  
                            @recoveries_total,  
                            @commission_excluded,  
                            @withholding_tax_excluded,  
                            @reinsurer_mapping_code,  
                            @spare,  
                            @reinsurer_account_key,
                            @transdetail_type_code)  
                END  
  
                -- Include stats_detail_type in cursor so we can check if FAC or TTY  
                -- and set account & account type accordingly.  
                -- Peter Finney 03/07/2003  
                -- Also restrict by stats_detail_id so we don't end up duplicating data where  
                -- the same reinsurer has been used on multiple treaties or fac!  
                DECLARE TreatyCursor2 CURSOR FAST_FORWARD FOR  
                    SELECT  class_of_business_code,  
                            SUM(ISNULL(d.this_premium_original, 0)),  
                            SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate, -- Convert to original  
                            SUM(ISNULL(d.sum_insured_home, 0)),  
                            d.stats_detail_type  
                    FROM    stats_detail d  
                    JOIN    treaty_party tp  
                            ON tp.treaty_id = d.ri_party_cnt And tp.treaty_party_id= @nTreaty_ID 
                    WHERE   d.stats_folder_cnt = @stats_folder_cnt  
                    AND     d.stats_detail_id = @stats_detail_id  
                    AND     d.stats_detail_type IN ('TTY','TYX')  
                    AND     tp.party_cnt = @reinsurer_cnt  
                    AND     d.class_of_business_code IS NOT NULL  
                    GROUP BY class_of_business_code, d.stats_detail_type  
  
                    UNION  

                    SELECT  class_of_business_code,  
                            SUM(ISNULL(d.this_premium_original, 0)),
                            SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate,  -- Convert to original  
                            SUM(ISNULL(d.sum_insured_home, 0)),  
                            d.stats_detail_type  
                    FROM    stats_detail d  
                    WHERE   d.stats_folder_cnt = @stats_folder_cnt  
                    AND     d.stats_detail_id = @stats_detail_id  
                    AND     d.stats_detail_type IN ('FAC', 'FAX')  
                    AND     d.ri_party_cnt = @reinsurer_cnt  
                    AND     d.class_of_business_code is not null  
                    GROUP BY class_of_business_code, d.stats_detail_type  
  
                OPEN TreatyCursor2  
                FETCH NEXT FROM TreatyCursor2 INTO  
                    @class_of_business_code,  
                    @premium_sub_total,  
                    @commission_sub_total,  
                    @sum_insured_sub_total,  
                    @stats_detail_type  
  
                WHILE (@@FETCH_STATUS = 0) BEGIN  
                    -- Don't do this for for Intermediary mode  
                    IF @acc_type <> 'I' BEGIN  
                        -- Set transaction amount  
                        IF @stats_detail_type = 'FAC' OR @stats_detail_type = 'FAX'  
                            SELECT  @transaction_amount = -@premium_sub_total
                        ELSE  
                            SELECT  @transaction_amount = -@premium_sub_total * @share_percent / 100  
  
                        -- Check type to set account & account type.  
                        IF @stats_detail_type = 'TTY'  
                            -- Expenses \ RI Treaty Premium  
                            SELECT  @transaction_ledger_code = 'NO',
                                    @account_type_code = 'EXPRIOUTTR',
                                    @nominal_ledger_account = 'NORIOUTTR' + @class_of_business_code  
                        IF @stats_detail_type = 'FAC'  
                            -- Expenses \ RI Other Premium  
                            SELECT  @transaction_ledger_code = 'NO',  
                                    @account_type_code = 'EXPRIOUTOT',  
                                    @nominal_ledger_account = 'NORIOUTOT' + @class_of_business_code  
                        IF @stats_detail_type = 'TYX'  
                            -- Expenses \ RI Treaty XOL Premium  
                            SELECT  @transaction_ledger_code = 'NO',
                                    @account_type_code = 'EXPRIOUTTR',
                                    @nominal_ledger_account = 'NORIOUTTTY' + @class_of_business_code
                        IF @stats_detail_type = 'FAX'
                            -- Expenses \ RI FAC XOL Premium
                            SELECT  @transaction_ledger_code = 'NO',
                                    @account_type_code = 'EXPRIOUTFX',
                                    @nominal_ledger_account = 'NORIOUTFX' + @class_of_business_code
  
  						IF @stats_detail_type = 'FAC'
							SET @transdetail_type_code = 'REINPREMFAC'
						ELSE IF @stats_detail_type = 'FAX'  
							SET @transdetail_type_code = 'REINPREMFX'
						ELSE IF @stats_detail_type = 'TTY'
							SET @transdetail_type_code = 'REINPREMTR'
						ELSE
							SET @transdetail_type_code = 'REINPREMTR'	
							
                        -- Set transaction_detail_id
                        SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1
                        FROM    Transaction_Export_Detail  
                        WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
                        INSERT INTO Transaction_Export_Detail (  
                                transaction_export_folder_cnt,  
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
                                mapping_Code,  
                                transaction_account_key,
                                transdetail_type_code)  
                        VALUES (@transaction_export_folder_cnt,  
                                @transaction_export_detail_id,  
                                @transaction_amount,  
                                @nominal_ledger_code,  
                                @account_type_code,  
                                @ceded_ref,  
                                @Cover_Share_Percent,  
                                -@sum_insured_sub_total,  
                                @charges_total,  
                                @taxes_total,  
                                @recoveries_total,  
                                @commission_excluded,  
                                @withholding_tax_excluded,  
                                @nominal_ledger_account,  
                                @transaction_account_key,
                                @transdetail_type_code)  
                    END  
  
                    -- Check commission total  
                    IF @commission_sub_total <> 0 BEGIN  
                        -- Get amounts  
                        IF @stats_detail_type = 'FAC' OR @stats_detail_type = 'FAX'  
                            SELECT  @transaction_amount = @commission_sub_total  
                        ELSE  
                            SELECT  @transaction_amount = @commission_sub_total * @comm_share_percent / 100  
  
						SELECT @transdetail_type_code = 'REINCOMMTR'
						
                        -- Check type to set account & account type.  
                        IF @stats_detail_type = 'TTY'  
                            -- Income \ RI Treaty Commission  
                            SELECT  @transaction_ledger_code = 'NO',  
                                    @account_type_code = 'INCRICOMTR',  
                                    @nominal_ledger_account = 'NORICOMTR' + @class_of_business_code,
									@transdetail_type_code = 'REINCOMMTQ'    
                        IF @stats_detail_type = 'FAC'
                            -- Income \ RI FAC Commission  
                            SELECT  @transaction_ledger_code = 'NO',  
                                    @account_type_code = 'INCRICOMOT',  
                                    @nominal_ledger_account = 'NORICOMOT' + @class_of_business_code,
									@transdetail_type_code = 'REINCOMMFAC'  
                        IF @stats_detail_type = 'TYX'  
                            -- Expenses \ RI Treaty XOL Commission  
                            SELECT  @transaction_ledger_code = 'NO',  
                                    @account_type_code = 'INCRICOMTR',
                                    @nominal_ledger_account = 'NORICOMTTY' + @class_of_business_code,
									@transdetail_type_code = 'REINCOMMTQ'
                        IF @stats_detail_type = 'FAX'
                            -- Expenses \ RI FAC XOL Commission
                            SELECT  @transaction_ledger_code = 'NO',
                                    @account_type_code = 'INCRICOMFX',
                                    @nominal_ledger_account = 'NORICOMFX' + @class_of_business_code,
									@transdetail_type_code = 'REINCOMMFX'
  
                        -- Set transaction_detail_id  
                        SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
                        FROM    Transaction_Export_Detail  
                        WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt
  
                        INSERT INTO Transaction_Export_Detail (  
                                transaction_export_folder_cnt,  
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
                                mapping_Code,  
                                transaction_account_key,
                                transdetail_type_code)  
                        VALUES (@transaction_export_folder_cnt,  
                                @transaction_export_detail_id,  
                                @transaction_amount,  
                                @transaction_ledger_code,  
                                @account_type_code,  
                                @ceded_ref,  
                                @Cover_Share_Percent,  
                                null,  
                                @charges_total,  
                                @taxes_total,  
                                @recoveries_total,  
                                @commission_excluded,  
                                @withholding_tax_excluded,  
                                @nominal_ledger_account,  
                                @transaction_account_key,
                                @transdetail_type_code)  
                    END  
  
                    FETCH NEXT FROM TreatyCursor2 INTO  
                        @class_of_business_code,  
                        @premium_sub_total,  
                        @commission_sub_total,  
                        @sum_insured_sub_total,  
                        @stats_detail_type  
                END  
  
                CLOSE TreatyCursor2  
                DEALLOCATE TreatyCursor2  
            END  
        END  
  
        -- Create secondary AGENT transaction  
        IF @transaction_type IN ('Gross_Comb', 'Gross_Cros') BEGIN  
            -- NOTE: AS THE FOLLOWING VALUES ARE OVERWRITTEN HERE AND ONLY SET  
            --       BEFORE THE ORIGINAL LOOP THEY WILL AFFECT ALL FUTURE LINES.  
            --       IT APPEARS AT THE MOMENT THAT THIS CODE IS NOT CALLED BUT  
            --       IF IT IS AND THERE IS A PROBLEM WITH MULTIPLE POSTINGS  
            --       MOVE THE CODE ABOVE INSIDE THE CURSORS LOOP!!!  
  
            -- Initialise all transaction details column variables to NULL  
            SELECT  @transaction_amount = NULL,  
                    @transaction_ledger_code = NULL,  
                    @account_type_code = NULL,  
                    @ceded_ref = NULL,  
                    @Cover_Share_Percent = NULL,  
                    @sum_insured_total = NULL,  
                    @charges_total = NULL,  
                    @taxes_total = NULL,  
                    @recoveries_total = NULL,  
                    @commission_excluded = NULL,  
                    @withholding_tax_excluded = NULL  
  
            -- Get column values required by SALES EXPORT  
            SELECT  @transaction_ledger_code = 'S1',  
                    @account_type_code = 'Account Type',  
                    @Cover_Share_Percent = 0,  
                    @sum_insured_total = 0,  
                    @charges_total = 0,  
                    @taxes_total = 0,  
                    @recoveries_total = 0,  
                    @commission_excluded = 0,  
                    @withholding_tax_excluded = 0,  
                    @nominal_ledger_code = 'N1',  
                    @mapping_code = 'S1AccType',  
                    @nominal_mapping_code = 'N1AccType'  
  
            -- Agent Net Accounting transaction  
            SELECT  @transaction_amount = -1 * @commission_total  
  
            -- Insert the Trans Export Detail  
            IF @transaction_amount <> 0 BEGIN  
                -- Set transaction_detail_id  
                SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
                FROM    Transaction_Export_Detail  
                WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
                INSERT INTO Transaction_Export_Detail (  
                        transaction_export_folder_cnt,  
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
                        mapping_Code,  
                        transaction_account_key)  
                VALUES (@transaction_export_folder_cnt,  
                        @transaction_export_detail_id,  
                        @transaction_amount,  
                        @transaction_ledger_code,  
                        @account_type_code,  
                        @ceded_ref,  
                        @Cover_Share_Percent,  
                        @sum_insured_total,  
                        @charges_total,  
                        @taxes_total,  
                        @recoveries_total,  
                        @commission_excluded,  
                        @withholding_tax_excluded,  
                        @nominal_mapping_code,  
                        @transaction_account_key)  
  
                -- Insert the Nominal Analysis Export Detail  
                SELECT  @transaction_amount = -@transaction_amount  
  
                -- Set transaction_detail_id  
                SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
                FROM    Transaction_Export_Detail  
                WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
                INSERT INTO Transaction_Export_Detail (  
                        transaction_export_folder_cnt,  
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
                        mapping_Code,  
                        transaction_account_key)  
                VALUES (@transaction_export_folder_cnt,  
                        @transaction_export_detail_id,  
                        @transaction_amount,  
                        @nominal_ledger_code,  
                        @account_type_code,  
                        @ceded_ref,  
                        @Cover_Share_Percent,  
                        @sum_insured_total,  
                        @charges_total,  
                        @taxes_total,  
                        @recoveries_total,  
                        @commission_excluded,  
                        @withholding_tax_excluded,  
                        @mapping_code,  
                        @transaction_account_key)  
            END  
        END  
  
        FETCH NEXT FROM TreatyCursor INTO  
            @reinsurer_cnt,  
            @reinsurer_mapping_code,  
            @reinsurer_account_key,  
            @share_percent,  
            @comm_share_percent,  
            @premium_total,  
            @commission_total,  
            @sum_insured_total,  
            @tax_value,  
            @tax_type_code,  
            @stats_detail_type2,  
            @stats_detail_id,
            @nTreaty_id
    END  
  
    CLOSE TreatyCursor  
    DEALLOCATE TreatyCursor  
GO