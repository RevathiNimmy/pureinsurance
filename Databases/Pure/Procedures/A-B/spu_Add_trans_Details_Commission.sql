EXECUTE DDLDropProcedure 'spu_add_trans_details_commission'
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE Spu_add_trans_details_commission 
@transaction_export_folder_cnt INT,
@stats_folder_cnt INT
AS
  BEGIN
      DECLARE @transaction_export_detail_id INT,
              @amount                       NUMERIC(19, 4),
              @mapping_code                 VARCHAR(20),
              @agent                        VARCHAR(20),
              @transaction_account_key      INT,
              @source_id                    INT,
              @class_of_business            VARCHAR(10),
              @tax_amount                   NUMERIC(19, 4),
              @leadconsolidate              TINYINT,
              @subleadconsolidate           TINYINT,
              @suspenseAccount              VARCHAR(20),
              @transType                    VARCHAR(10),
              @PostAccount                  VARCHAR(20),
              @suspend                      TINYINT,
              @CommissionAccountKey         VARCHAR(20),
              @releaseToIncome              TINYINT,
              @ReleaseAccountCode           VARCHAR(20),
              @CommissionAccount            VARCHAR(20),
              @cycle                        TINYINT,
              @MergedCommTax                INT

      SET @MergedCommTax=1

      SELECT @MergedCommTax = Isnull(VALUE, 0)
      FROM   System_Options
      WHERE  option_number = 5095

      DECLARE c_commission CURSOR FAST_FORWARD FOR
       
        SELECT SF.source_id,
               SD.ceded_ref,
               SD.class_of_business_code,
			   SUM(CASE WHEN currency_rate<>0 THEN sub_commission_value_home/currency_rate ELSE sub_commission_value_home END)
              -- SUM(SD.sub_commission_value_home) /*Contains the sub agent commission in transaction currency*/  -- because it is sub-agent
        FROM   Stats_Detail SD
               INNER JOIN Stats_Folder SF
                 ON SF.stats_folder_cnt = SD.stats_folder_cnt

        WHERE  SD.stats_folder_cnt = @stats_folder_cnt
               AND SD.stats_detail_type = 'SUB'       /* Open the Sub Commission Cursor */
        GROUP  BY SF.source_id,
                  SD.class_of_business_code,
                  SD.ceded_ref 
        

      /* Open the Commission Cursor */
      OPEN c_commission

      FETCH NEXT FROM c_commission INTO @source_id, @agent, @class_of_business, @amount

      /* Get the column values */
      WHILE ( @@FETCH_STATUS = 0 )
        BEGIN
            --Get the Account for the Sub Agent  
            SELECT @transaction_account_key = account_key
            FROM   ACCOUNT
            WHERE  short_code = @agent 
			--AND company_id=1

            --Get the next transaction export ID  
            SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
            FROM   Transaction_Export_Detail
            WHERE  transaction_export_folder_cnt = @transaction_export_folder_cnt

            IF @transaction_export_detail_id IS NULL
              SET @transaction_export_detail_id = 1

            --If leadconsolidate option is ticked.(TMP)  
            SELECT @leadconsolidate = allow_consolidated_commission
            FROM   party_agent
            WHERE  party_cnt = @transaction_account_key

            SELECT @subleadconsolidate = inf.sub_allow_consolidated_commission,
                   @suspenseAccount = a.short_code,
                   @transType = tef.transaction_type_code,
                   @cycle = p.sub_month_in_cycle
            FROM   product p
                   JOIN ACCOUNT a
                     ON a.account_id = p.sub_suspense_account_id
                   JOIN insurance_file inf
                     ON inf.product_id = p.product_id
                   JOIN transaction_export_folder tef
                     ON tef.insurance_file_cnt = inf.insurance_file_cnt
            WHERE  tef.transaction_export_folder_cnt = @transaction_export_folder_cnt

            IF ( @leadconsolidate = 1 )
               AND ( @subleadconsolidate = 1 )
               AND ( @transType = 'NB' )
               AND ( @cycle >= 1 )
              BEGIN
                  SELECT @releaseAccountCode = @agent

                  SELECT @transaction_account_key = NULL,
                         @agent = @suspenseAccount,
                         @suspend = 1,
                         @releaseToIncome = 0
              END

            --(RC) PLICO 9-10 --START  
            DECLARE @commission_posting_type_id          INT,
                    @agent_commission_suspended_postings INT,
                    @agent_code                          VARCHAR(50),
                    @manually_released                   INT,
                    @released_on_full_settlement         INT,
                    @released_for_whole_posting          INT,
                    @released_on_policy_effective        INT

            --get system option  
            SELECT @agent_commission_suspended_postings = VALUE
            FROM   system_options
            WHERE  branch_id = 1
                   AND option_number = 5037

            --get comm posting type and agent type code  
            SELECT @commission_posting_type_id = PA.commission_posting_type_id,
                   @agent_code = PAT.code
            FROM   party_agent PA
                   INNER JOIN party_agent_type PAT
                     ON PA.party_agent_type_id = PAT.party_agent_type_id
            WHERE  PA.party_cnt = (SELECT party_cnt
                                   FROM   party
                                   WHERE  shortname = @agent)

            IF ( ( Isnull(@agent_commission_suspended_postings, 0) = 1 )
                 AND ( @commission_posting_type_id = 2 )
                 AND ( Rtrim(@agent_code) = 'Sub-Agent' ) )
              BEGIN
                  -- SELECT @suspenseAccount = account_id for system option  
                  -- Agent suspense account code  
                  SELECT @suspenseAccount = VALUE
                  FROM   system_options
                  WHERE  branch_id = 1
                         AND option_number = 5039

                  SELECT @releaseAccountCode = @agent

                  SELECT @transaction_account_key = NULL,
                         @agent = @suspenseAccount,
                         @suspend = 1,
                         @releaseToIncome = 0,
                         @manually_released = 1,
                         @released_on_full_settlement = 1,
                         @released_for_whole_posting = 1,
                         @released_on_policy_effective = 1
              END

                     --Insert the Agent record  
            INSERT INTO Transaction_Export_Detail
                        (transaction_export_folder_cnt,
                         transaction_export_detail_id,
                         transaction_amount,
                         transaction_ledger_code,
                         account_type_code,
                         mapping_code,
                         transaction_account_key,
                         spare,
                         suspended,
                         release_to_income,
                         release_account_code,
                         transdetail_type_code,
                         manually_released,
                         released_on_full_settlement,
                         released_for_whole_posting,
                         released_on_policy_effective)
            VALUES      ( @transaction_export_folder_cnt,
                          @transaction_export_detail_id,
                          @amount * -1,
                          'UB',
                          'SUBAGENTLD',
                          @agent,
                          @transaction_account_key,
                          'COMM',
                          @suspend,
                          @releaseToIncome,
                          @releaseAccountCode,
                          'COMM',
                          @manually_released,
                          @released_on_full_settlement,
                          @released_for_whole_posting,
                          @released_on_policy_effective )

            FETCH NEXT FROM c_commission INTO @source_id, @agent, @class_of_business, @amount
        END

      DEALLOCATE c_commission

         BEGIN
          DECLARE c_subagent_tax CURSOR FAST_FORWARD FOR
            SELECT P.party_cnt,
                   P.shortname,
                   SUM(tc.VALUE)
            FROM   Tax_Calculation tc
                   JOIN Stats_Folder sf
                     ON sf.insurance_file_cnt = tc.insurance_file_cnt
                   JOIN Agent_Commission ac
                     ON ac.agent_commission_cnt = tc.agent_commission_cnt
                   JOIN Party p
                     ON p.party_cnt = ac.party_cnt
                   JOIN Tax_Band tb
                     ON tb.tax_band_id = tc.tax_band_id
                   JOIN Tax_Type tt
                     ON tt.tax_type_id = tb.tax_type_id
            WHERE  sf.stats_folder_cnt = @stats_folder_cnt
                   AND ac.is_lead_agent = 0
            GROUP  BY P.party_cnt,
                      P.shortname

          -- Open the Commission Cursor  
          OPEN c_subagent_tax

          FETCH NEXT FROM c_subagent_tax INTO @transaction_account_key, @mapping_code, @amount

          -- Get the column values  
          WHILE ( @@FETCH_STATUS = 0 )
            BEGIN
                --Get the next transaction export ID  
                SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
                FROM   Transaction_Export_Detail
                WHERE  transaction_export_folder_cnt = @transaction_export_folder_cnt

                --Insert the Agent record  
                IF @MergedCommTax = 0
                  INSERT INTO Transaction_Export_Detail
                              (transaction_export_folder_cnt,
                               transaction_export_detail_id,
                               transaction_amount,
                               transaction_ledger_code,
                               account_type_code,
                               mapping_code,
                               transaction_account_key,
                               spare)
                  VALUES      ( @transaction_export_folder_cnt,
                                @transaction_export_detail_id,
                                @amount * -1,
                                'UB',
                                'SUBAGENTLD',
                                @mapping_code,
                                @transaction_account_key,
                                'TAX' )
                
                FETCH NEXT FROM c_subagent_tax INTO @transaction_account_key, @mapping_code, @amount
            END

          DEALLOCATE c_subagent_tax
      END

      /* Now repeat the process summarised by COB */
      DECLARE c_commission CURSOR FAST_FORWARD FOR
        SELECT SD.class_of_business_code,
				SUM(CASE WHEN currency_rate<>0 THEN sub_commission_value_home/currency_rate ELSE sub_commission_value_home END)
               --SUM(SD.sub_commission_value_home) /*Contains the sub agent commission in transaction currency*/
        FROM   Stats_Detail SD
               INNER JOIN Stats_Folder SF
                 ON SF.stats_folder_cnt = SD.stats_folder_cnt
        WHERE  SD.stats_folder_cnt = @stats_folder_cnt
               AND SD.stats_detail_type = 'SUB' /* Open the Sub Commission Cursor */
        GROUP  BY SD.class_of_business_code

      /* Open the Commission Cursor */
      OPEN c_commission

      FETCH NEXT FROM c_commission INTO @class_of_business, @amount

      /* Get the column values */
      WHILE ( @@FETCH_STATUS = 0 )
        BEGIN
            --Get the next transaction export ID  
            SELECT @transaction_export_detail_id = MAX(transaction_export_detail_id) + 1
            FROM   Transaction_Export_Detail
            WHERE  transaction_export_folder_cnt = @transaction_export_folder_cnt

            --Insert the Agent record  
            INSERT INTO Transaction_Export_Detail
                        (transaction_export_folder_cnt,
                         transaction_export_detail_id,
                         transaction_amount,
                         transaction_ledger_code,
                         account_type_code,
                         mapping_code,
                         transaction_account_key,
                         spare)
            VALUES      ( @transaction_export_folder_cnt,
                          @transaction_export_detail_id,
                          @amount,
                          'NO',
                          'EXPSUBAG',
                          'SUBCO' + @class_of_business,
                          NULL,
                          'COMSUSP')

            FETCH NEXT FROM c_commission INTO @class_of_business, @amount
        END

      DEALLOCATE c_commission
  END 
