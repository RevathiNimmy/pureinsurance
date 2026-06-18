SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_add_trans_details_reins_ri2007'
GO

CREATE PROCEDURE spu_add_trans_details_reins_ri2007    
 @transaction_export_folder_cnt int,    
 @stats_folder_cnt int    
AS    
    
    DECLARE    
        @transaction_export_detail_id int,    
        @transaction_ledger_code varchar (2),    
        @account_type_code varchar (10),    
        @Cover_Share_Percent float,    
        @charges_total numeric(19, 4),    
        @taxes_total numeric(19, 4),    
        @recoveries_total numeric(19, 4),    
        @commission_excluded numeric(19, 4),    
        @withholding_tax_excluded numeric(19, 4),    
        @nominal_ledger_code varchar(2),    
        @mapping_code varchar(30),    
        @nominal_mapping_code varchar(30),    
        @agent_cnt int,    
        @acc_type varchar(1),    
        @currency_rate float,    
        @company_id int,    
        @currency_id int,    
        @insurance_file_cnt int,    
        @effective_date datetime,    
  @transdetail_type_code Varchar(50),    
  @ceded_ref             VARCHAR (10)    
    
-- Change accounts posting for Intermediary mode    
    SELECT  @acc_type = ISNULL(Acc_Type, '')    
    FROM    hidden_options    
    WHERE   branch_id=1    
    AND     option_number=1    
    
    -- Get rate from insurance file    
    SELECT  @company_id = IFL.source_id,    
            @currency_id = IFL.currency_id,    
            @currency_rate = IFL.currency_base_xrate  ,    
            @insurance_file_cnt = IFL.insurance_file_cnt,    
            @agent_cnt = SF.agent_cnt    
    FROM Stats_Folder SF    
  JOIN Insurance_File IFL ON SF.insurance_file_cnt = IFL.insurance_file_cnt    
    WHERE SF.stats_folder_cnt = @stats_folder_cnt    
    
   -- If rate is blank then get it from rate table    
    IF ISNULL(@currency_rate ,0) = 0    
    BEGIN    
        SELECT @effective_date = GETDATE()    
    
        -- Get rate from currency rate table    
        EXEC spu_ACT_Get_Currency_Rate    
            @currency_id = @currency_id,    
            @company_id = @company_id,    
            @effective_date = @effective_date,    
            @rate = @currency_rate OUTPUT    
    END    
    
     -- Get column values required by SALES EXPORT    
    SELECT  @transaction_ledger_code  = 'IN',    
            @account_type_code = 'REINSACC',    
            @Cover_Share_Percent = 0,    
            --@sum_insured_total = 0,    
            @charges_total = 0,    
            @taxes_total = 0,    
            @recoveries_total = 0,    
            @commission_excluded = 0,    
            @withholding_tax_excluded = 0,    
            @nominal_ledger_code = 'NO',    
            @mapping_code = 'PLREINS',    
            @nominal_mapping_code = 'NOREINS',    
            @ceded_ref = NULL    
    
Create Table #Treaty    
(    
 ID INT IDENTITY PRIMARY KEY,    
 reinsurer_cnt INT,    
 reinsurer_mapping_code VARCHAR(100),    
 reinsurer_account_key int,    
 share_percent float,    
 comm_share_percent float,    
 premium_total numeric(19, 4),    
 commission_total numeric(19, 4),    
 sum_insured_total numeric(19, 4),    
 tax_value numeric(19, 4),    
 tax_type_code varchar(13),    
 stats_detail_type2 varchar(3),    
 stats_detail_id INT,    
 transaction_amount numeric(19, 4)    
)    
    
CREATE TABLE #Treaty2    
(    
 class_of_business_code VARCHAR(30),    
 premium_sub_total numeric(19, 4),    
 commission_sub_total numeric(19, 4),    
 sum_insured_sub_total numeric(19, 4),    
 stats_detail_type VARCHAR(3),    
 ri_party_type varchar(10),    
 transaction_amount NUMERIC(19,4),    
 stats_detail_id INT,    
 share_percent float,    
 reinsurer_cnt INT,    
 reinsurance_code VARCHAR(10),    
 transaction_ledger_code  VARCHAR(20),    
 account_type_code VARCHAR(20),    
 nominal_ledger_account VARCHAR(20),    
 comm_share_percent float,    
 transdetail_type_code VARCHAR(20),    
 ID INT IDENTITY PRIMARY KEY    
)    
    
--Create Clustered INDEX CIndex_ID ON #Treaty(ID)    
--Create Clustered INDEX CIndex_ID2 ON #Treaty2(ID)    
--EXEC DDLAddIndex '#Treaty','stats_detail_type2'    
--EXEC DDLAddIndex '#Treaty2','stats_detail_type'    
    
SELECT stats_folder_cnt ,stats_detail_id,is_commission_modified,this_premium_original,lead_commission_value_home,    
sum_insured_home,tax_value,tax_type_code,ri_share_percent,stats_detail_type, class_of_business_code,ri_party_type,    
ri_party_cnt,    
    
P.Party_cnt, P.shortname,    
P2.party_cnt TParty_cnt,P2.shortname TShortname, tp.share_percent , tp.commission_percent,tp.treaty_id    
    
INTO #StatsDetail    
FROM Stats_Detail    
LEFT JOIN Party P ON P.party_cnt = ri_party_cnt  AND stats_detail_type NOT IN ('TTY' ,'TAT', 'TYX','CAT')    
    
LEFT JOIN    treaty_party tp    
 ON tp.treaty_id = ri_party_cnt AND ri_shortname IS NOT NULL AND stats_detail_type IN ('TTY' ,'TAT', 'TYX','CAT')    
LEFT JOIN    party p2    
    ON p2.party_cnt = tp.party_cnt    
    
Where stats_folder_cnt = @stats_folder_cnt    
    
-- Get totals from stats table    
    
 INSERT into #Treaty (    
  reinsurer_cnt ,    
  reinsurer_mapping_code,    
  reinsurer_account_key,    
  share_percent,    
  comm_share_percent,    
  premium_total,    
  commission_total,    
  sum_insured_total,    
  tax_value,    
  tax_type_code,    
  stats_detail_type2,    
  stats_detail_id)    
    
        SELECT  d.Tparty_cnt,    
                d.Tshortname,    
                d.Tparty_cnt,    
                d.share_percent,    
    
                -- The, now complicated, treaty commission rate    
                CASE WHEN d.is_commission_modified = 1 OR tpc.comm_split = 0 THEN    
                    d.share_percent    
                ELSE    
                    (d.share_percent * d.commission_percent) / tpc.comm_split    
                END,    
                SUM(ISNULL(d.this_premium_original, 0)),    
                SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate, -- Convert to original    
                SUM(ISNULL(d.sum_insured_home, 0)),    
                SUM(ISNULL(d.tax_value, 0)),    
                d.tax_type_code,    
                d.stats_detail_type,    
                d.stats_detail_id    
    
        FROM    #StatsDetail d    
    
        JOIN   (SELECT  treaty_id,    
                       (SUM(share_percent * commission_percent) / 100) comm_split    
                FROM    treaty_party    
                GROUP BY treaty_id) tpc    
                ON tpc.treaty_id = d.treaty_id    
        LEFT JOIN    
                party_insurer PIN    
                ON d.Tparty_cnt = PIN.party_cnt    
    
        WHERE   --d.stats_folder_cnt = @stats_folder_cnt  AND    
                d.stats_detail_type IN( 'TTY', 'TAT', 'TYX','CAT')    
        AND     d.Tshortname <> 'RETAINED'    
        AND     ISNULL(PIN.is_retained, 0) <>1    
        GROUP BY    
                d.Tparty_cnt, D.Tshortname, d.share_percent, d.tax_type_code, d.stats_detail_type, d.stats_detail_id,    
                d.is_commission_modified, d.commission_percent, tpc.comm_split    
    
        UNION    
    
        SELECT  d.party_cnt,    
                d.shortname,    
                d.party_cnt,    
                d.ri_share_percent,    
                d.ri_share_percent,    
                SUM(ISNULL(d.this_premium_original, 0)),    
                SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate, -- Convert to original    
                SUM(ISNULL(d.sum_insured_home, 0)),    
                SUM(ISNULL(d.tax_value, 0)),    
                d.tax_type_code,    
                d.stats_detail_type,    
                d.stats_detail_id    
        --FROM    stats_detail d    
        FROM #StatsDetail d    
        --JOIN    party p    
        --        ON p.party_cnt = d.ri_party_cnt    
        LEFT JOIN    
                party_insurer PIN    
                ON d.party_cnt = PIN.party_cnt    
        WHERE   d.stats_folder_cnt = @stats_folder_cnt    
        AND     d.stats_detail_type IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')    
        AND     d.shortname <> 'RETAINED'    
        AND     ISNULL(PIN.is_retained, 0) <> 1    
        GROUP BY d.party_cnt, d.shortname, d.ri_share_percent, d.tax_type_code, d.stats_detail_type, d.stats_detail_id    
    
  -- Get transaction_detail_id    
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0)    
            FROM    Transaction_Export_Detail    
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt    
    
            Update T SET T.transaction_amount = T.tax_value    
          FROM #Treaty T    
   WHERE T.stats_detail_type2 IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')    
   AND tax_value <> 0    
    
   Update T SET T.transaction_amount = T.tax_value * T.share_percent / 100    
            FROM #Treaty T    
   WHERE T.stats_detail_type2 NOT IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')    
   AND tax_value <> 0    
    
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
   SELECT  @transaction_export_folder_cnt,    
                    @transaction_export_detail_id + ROW_NUMBER() OVER(ORDER BY T.ID),    
                    T.transaction_amount,    
                    'IN',    
                    'REINSACC',    
                    @ceded_ref,    
                    @Cover_Share_Percent,    
                    T.sum_insured_total,    
                    @charges_total,    
                    @taxes_total,    
                    @recoveries_total,    
                    @commission_excluded,    
                    @withholding_tax_excluded,    
                    T.reinsurer_mapping_code,    
                    T.reinsurer_account_key,    
                    'TAX ' + T.tax_type_code,    
                    CASE T.stats_detail_type2    
      WHEN 'TTC' THEN 'REINCOMMTAX'    
      WHEN 'TFC' THEN 'REINCOMMTAX'    
      ELSE 'REINTAX' end    
    
            FROM #Treaty T WHERE tax_value <> 0    
    
            Update T SET T.transaction_amount = T.premium_total    
            FROM #Treaty T    
   WHERE T.stats_detail_type2 IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')    
   AND premium_total <> 0    
    
            Update T SET T.transaction_amount = T.premium_total * T.share_percent / 100.00    
            FROM #Treaty T    
   WHERE T.stats_detail_type2 NOT IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')    
   AND premium_total <> 0    
    
   SELECT  @transaction_ledger_code  = 'IN',    
                        @account_type_code = 'REINSACC',    
                        @transdetail_type_code = 'REINPREM'    
         -- Set transaction_detail_id    
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0)    
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
                SELECT  @transaction_export_folder_cnt,    
                        @transaction_export_detail_id + ROW_NUMBER() OVER(ORDER BY T.ID),    
                     T.transaction_amount,    
                        @transaction_ledger_code,    
                        @account_type_code,    
                        @ceded_ref,    
                        @Cover_Share_Percent,    
            T.sum_insured_total,    
                        @charges_total,    
                        @taxes_total,    
                        @recoveries_total,    
                        @commission_excluded,    
                        @withholding_tax_excluded,    
                        T.reinsurer_mapping_code,    
                        T.reinsurer_account_key,    
                        @transdetail_type_code    
                  FROM #Treaty T WHERE T.premium_total <> 0 AND T.tax_value = 0    
    
 -- Check for commission    
         Update T SET T.transaction_amount = - T.commission_total    
            FROM #Treaty T    
   WHERE T.stats_detail_type2 IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')    
   AND commission_total <> 0    
    
            Update T SET T.transaction_amount = - T.commission_total * T.comm_share_percent / 100.00    
            FROM #Treaty T    
   WHERE T.stats_detail_type2 NOT IN ('FAC', 'TAF', 'TFP', 'TFC', 'TTP', 'TTC', 'FAX')    
   AND premium_total <> 0    
    
   SELECT  @transdetail_type_code = 'REINCOMM'    
    
         -- Set transaction_detail_id    
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0)    
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
                    SELECT  @transaction_export_folder_cnt,    
                            @transaction_export_detail_id + ROW_NUMBER() OVER(ORDER BY T.ID),    
                            T.transaction_amount,    
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
                            T.reinsurer_mapping_code,    
                            'COMM',    
                            T.reinsurer_account_key,    
                            @transdetail_type_code    
                          FROM #Treaty T WHERE T.commission_total <> 0 AND T.premium_total <> 0 AND T.tax_value = 0    
    
                    INSERT INTO #Treaty2    
                    (    
      class_of_business_code, premium_sub_total,    
      commission_sub_total,sum_insured_sub_total,    
      stats_detail_type, ri_party_type,stats_detail_id,    
      share_percent,    
      reinsurer_cnt,comm_share_percent    
                    )    
    
     SELECT  class_of_business_code,    
                            SUM(ISNULL(d.this_premium_original, 0)),    
                            SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate, -- Convert to original    
                            SUM(ISNULL(d.sum_insured_home, 0)),    
                            d.stats_detail_type,    
       d.ri_party_type,    
       T.stats_detail_id,    
       T.share_percent,    
       T.reinsurer_cnt,    
       T.comm_share_percent    
    
                    FROM    #statsdetail d    
--JOIN    treaty_party tp    
                   --        ON tp.treaty_id = d.ri_party_cnt    
                    JOIN #Treaty T ON d.stats_detail_id = T.stats_detail_id    
                    WHERE  -- d.stats_folder_cnt = @stats_folder_cnt    
                   -- AND     d.stats_detail_id = @stats_detail_id    
                    d.stats_detail_type IN ('TTY','TYX','CAT')    
                    AND     d.Tparty_cnt = T.reinsurer_cnt    
                    AND     d.class_of_business_code IS NOT NULL    
                    GROUP BY class_of_business_code, d.stats_detail_type, d.ri_party_type ,T.stats_detail_id,T.share_percent,T.reinsurer_cnt  , T.comm_share_percent    
    
                    UNION    
    
                    SELECT  class_of_business_code,    
                            SUM(ISNULL(d.this_premium_original, 0)),    
                            SUM(ISNULL(d.lead_commission_value_home, 0)) / @currency_rate,  -- Convert to original    
                            SUM(ISNULL(d.sum_insured_home, 0)),    
                            d.stats_detail_type,    
       d.ri_party_type,    
       T.stats_detail_id,    
       T.share_percent,    
       T.reinsurer_cnt,    
       T.comm_share_percent    
                    FROM    #statsdetail d    
                    JOIN #Treaty T ON d.stats_detail_id = T.stats_detail_id    
                    WHERE  -- d.stats_folder_cnt = @stats_folder_cnt AND    
                    --AND     d.stats_detail_id = @stats_detail_id    
                         d.stats_detail_type IN ('FAC', 'FAX')    
                    AND     d.ri_party_cnt = T.reinsurer_cnt    
                    AND     d.class_of_business_code is not null    
                    GROUP BY class_of_business_code, d.stats_detail_type, d.ri_party_type    
       ,T.stats_detail_id,T.share_percent,T.reinsurer_cnt , T.comm_share_percent    
    
-- Don't do this for for Intermediary mode    
IF @acc_type <> 'I'    
BEGIN    
   Update T2 SET T2.transaction_amount = - T2.premium_sub_total    
   FROM #Treaty2 T2    
   WHERE  (T2.stats_detail_type = 'FAC' OR T2.stats_detail_type = 'FAX')    
    
   Update T2 SET T2.transaction_amount = - T2.premium_sub_total * T2.share_percent / 100    
   FROM #Treaty2 T2    
   WHERE  (T2.stats_detail_type  NOT IN ( 'FAC' ,'FAX'))    
    
   --IF @stats_detail_type = 'TTY'    
   Update T2 SET T2.reinsurance_code = UPPER(LTRIM(RTRIM(rt.code)))    
   FROM #Treaty2 T2    
   JOIN party_insurer ptyin ON ptyin.party_cnt = T2.reinsurer_cnt    
   INNER JOIN reinsurance_type rt    
      ON ptyin.reinsurance_type = rt.reinsurance_type_id    
      WHERE rt.is_deleted = 0 AND T2.stats_detail_type = 'TTY'    
    
   Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code = 'EXPRIOUT' + 'TQ',    
    T2.nominal_ledger_account = 'NORIOUT' + 'TQ' + T2.class_of_business_code    
   FROM #Treaty2 T2    
   WHERE T2.reinsurance_code = 'QUO'  AND T2.stats_detail_type = 'TTY'    
    
   Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code = 'EXPRIOUT' + 'TS',    
    T2.nominal_ledger_account = 'NORIOUT' + 'TS' + T2.class_of_business_code    
   FROM #Treaty2 T2    
   WHERE T2.reinsurance_code IN( '001' ,'002','003') AND T2.stats_detail_type = 'TTY'    
    
   Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code = 'EXPRIOUTTR',    
    T2.nominal_ledger_account = 'NORIOUTTR' + T2.class_of_business_code    
   FROM #Treaty2 T2    
   WHERE  T2.transaction_ledger_code IS NULL AND T2.stats_detail_type = 'TTY'    
    
   --End Accounts Tag Treaty Reinsurance    
    
   Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code = 'EXPRIOUTOT',    
    T2.nominal_ledger_account = 'NORIOUTOT' + T2.class_of_business_code    
   FROM #Treaty2 T2    
   WHERE T2.stats_detail_type = 'FAC'    
   ----------------------------------------------------------    
   --IF @stats_detail_type = 'TYX'    
   Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code =  'EXPRIOUT' + 'TC',    
    T2.nominal_ledger_account = 'NORIOUT' + 'TC' + T2.class_of_business_code    
   FROM #Treaty2 T2    
   WHERE T2.stats_detail_type = 'TYX' and T2.ri_party_type = 'CAT'    
    
   Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code =  'EXPRIOUTTX',    
    T2.nominal_ledger_account = 'NORIOUT' + 'TX' + T2.class_of_business_code    
   FROM #Treaty2 T2    
   WHERE T2.stats_detail_type = 'TYX' and T2.ri_party_type <> 'CAT'    
   ---------------------------------------    
    
   --IF @stats_detail_type = 'FAX'    
   Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code =  'EXPRIOUTFX',    
    T2.nominal_ledger_account = 'NORIOUTFX' + T2.class_of_business_code    
   FROM #Treaty2 T2    
   WHERE T2.stats_detail_type = 'FAX'    
    
         -- Set transaction_detail_id    
   SELECT @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0)    
   FROM   Transaction_Export_Detail    
   WHERE  transaction_export_folder_cnt = @transaction_export_folder_cnt    
    
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
                       mapping_Code,    
                       transaction_account_key,    
                       transdetail_type_code)    
                    SELECT   @transaction_export_folder_cnt,    
                       @transaction_export_detail_id   + ROW_NUMBER() OVER(ORDER BY T2.ID),    
                       T2.transaction_amount,    
                       @nominal_ledger_code,    
                       T2.account_type_code,    
                       @ceded_ref,    
                       @Cover_Share_Percent,    
                       -T2.sum_insured_sub_total,    
                       @charges_total,    
                       @taxes_total,    
                       @recoveries_total,    
                       @commission_excluded,    
                       @withholding_tax_excluded,    
                       T2.nominal_ledger_account,    
                       NULL, --@transaction_account_key,    
        CASE T2.stats_detail_type    
           WHEN 'FAC' THEN 'REINPREMFAC'    
           WHEN  'FAX' THEN 'REINPREMFX'    
           ELSE CASE T2.Ri_party_type    
            WHEN '001' THEN 'REINPREMFS'    
            WHEN '002' THEN 'REINPREMFS'    
            WHEN '003' THEN 'REINPREMFS'    
            WHEN 'CAT' THEN 'REINPREMCAT'    
            WHEN 'XOL' THEN 'REINPREMTX'    
            WHEN 'QUO' THEN 'REINPREMTQ'    
            ELSE 'REINPREMTR'    
            end    
            end    
    
                    FROM #Treaty2 T2    
END    
    
-- Check commission total    
    
  Update T2 SET T2.transaction_amount = T2.commission_sub_total    
  FROM #Treaty2 T2    
  WHERE  (T2.stats_detail_type = 'FAC' OR T2.stats_detail_type = 'FAX') AND T2.commission_sub_total <> 0    
    
  Update T2 SET T2.transaction_amount = T2.commission_sub_total * T2.comm_share_percent / 100    
  FROM #Treaty2 T2    
  WHERE  (T2.stats_detail_type  NOT IN ( 'FAC' ,'FAX')) AND T2.commission_sub_total <> 0    
    
  ------IF @stats_detail_type = 'TTY'    
  Update T2 SET T2.reinsurance_code = UPPER(LTRIM(RTRIM(rt.code)))    
  FROM #Treaty2 T2    
  JOIN party_insurer ptyin ON ptyin.party_cnt = T2.reinsurer_cnt    
  INNER JOIN reinsurance_type rt    
  ON ptyin.reinsurance_type = rt.reinsurance_type_id    
  WHERE rt.is_deleted = 0 AND T2.stats_detail_type = 'TTY'    
    
  Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code = 'INCRICOM' + 'TQ',    
    T2.nominal_ledger_account = 'NORICOM' + 'TQ' + T2.class_of_business_code,    
    T2.Transdetail_Type_Code ='REINCOMMTQ'    
  FROM #Treaty2 T2    
  WHERE T2.reinsurance_code = 'QUO'  AND T2.stats_detail_type = 'TTY'    
    
  Update T2 SET T2.transaction_ledger_code = 'NO',    
   T2.account_type_code = 'INCRICOM' + 'TS',    
    T2.nominal_ledger_account = 'NORICOM' + 'TS' + T2.class_of_business_code,    
    T2.Transdetail_Type_Code ='REINCOMMFS'    
  FROM #Treaty2 T2    
  WHERE T2.reinsurance_code IN('001','002','003')  AND T2.reinsurance_code <> 'QUO' AND T2.stats_detail_type = 'TTY'    
    
  Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code = 'INCRICOMTR',    
    T2.nominal_ledger_account = 'NORICOMTR' + T2.class_of_business_code,    
    T2.Transdetail_Type_Code ='REINCOMMTR'    
  FROM #Treaty2 T2    
  WHERE T2.reinsurance_code NOT IN('001','002','003','QUO')  AND T2.stats_detail_type = 'TTY'    
    
  Update T2 SET T2.transaction_ledger_code = 'NO',    
    T2.account_type_code =    
      CASE T2.stats_detail_type    
       WHEN 'FAC' THEN 'INCRICOMOT'    
       WHEN 'FAX' THEN 'INCRICOMFX'    
       WHEN 'TYX' THEN    
          CASE WHEN T2.ri_party_type ='CAT'  THEN 'INCRICOM' + 'TC'    
            ELSE 'INCRICOM' + 'TX' end    
    
      end,    
    T2.nominal_ledger_account =    
      CASE T2.stats_detail_type    
       WHEN 'FAC' THEN 'NORICOMOT' + T2.class_of_business_code    
       WHEN 'FAX' THEN 'NORICOMFX'+ T2.class_of_business_code    
       WHEN 'TYX' THEN    
          CASE WHEN T2.ri_party_type ='CAT'  THEN 'NORICOM' + 'TC' + T2.class_of_business_code    
            ELSE 'NORICOM' + 'TX' + T2.class_of_business_code end    
      end,    
    T2.transdetail_type_code =    
      CASE T2.stats_detail_type    
       WHEN 'FAC' THEN 'REINCOMMFAC'    
       WHEN 'FAX' THEN 'REINCOMMFX'    
       WHEN 'TYX' THEN    
          CASE WHEN T2.ri_party_type ='CAT'  THEN 'REINCOMMCAT'    
            ELSE 'REINCOMMTX' end    
      end    
  FROM #Treaty2 T2    
  WHERE T2.stats_detail_type IN( 'FAX','FAC','TYX')    
    
  -- Set transaction_detail_id    
  SELECT @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0)    
  FROM   Transaction_Export_Detail    
  WHERE  transaction_export_folder_cnt = @transaction_export_folder_cnt    
    
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
      mapping_Code,    
      transaction_account_key,    
      transdetail_type_code)    
  SELECT       @transaction_export_folder_cnt,    
      @transaction_export_detail_id + ROW_NUMBER() OVER(ORDER BY T2.ID),    
      T2.transaction_amount,    
      @transaction_ledger_code,    
      T2.account_type_code,    
      @ceded_ref,    
      @Cover_Share_Percent,    
      NULL,    
      @charges_total,    
      @taxes_total,    
      @recoveries_total,    
      @commission_excluded,    
      @withholding_tax_excluded,    
      T2.nominal_ledger_account,    
      NULL,--@transaction_account_key,    
      T2.transdetail_type_code    
  FROM #Treaty2 T2    
  WHERE T2.commission_sub_total <> 0 


