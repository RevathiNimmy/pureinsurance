SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_add_trans_details_sales'
GO

  
CREATE PROCEDURE spu_add_trans_details_sales  
    @transaction_export_folder_cnt int,  
    @stats_folder_cnt int  
AS  
    -- **************************************************************************************  
    -- spu_add_trans_details_sales creates stats details records for                          
    -- sales transaction export.                                                              
    --                                                                                        
    -- 2 parameters are passed in - @transaction_export_folder_cnt,  @stats_folder_cnt        
    --                                                                                        
    -- This stored procedure is called by spu_add_stats_details_control.                      
    --                                                                                        
    -- A failure in this procedure will be passed back to the calling procedure.              
    -- **************************************************************************************  
      
     
    -- Declare variable for all columns in transaction details table   
    DECLARE   
        @transaction_export_detail_id   int,  
        @transaction_amount numeric(19, 4),  
        @transaction_ledger_code varchar(2),  
        @account_type_code varchar(10),  
        @ceded_ref varchar(10),  
        @cover_share_percent numeric(12, 8),  
        @sum_insured_total numeric(19, 4),  
        @charges_total numeric(19, 4),  
        @taxes_total numeric(19, 4),  
        @recoveries_total numeric(19, 4),  
        @commission_excluded numeric(19, 4),  
        @withholding_tax_excluded numeric(19, 4),  
        @peril_type_code varchar(10),  
        @mapping_code varchar(30),  
        @transaction_account_key int,  
        @spare varchar(255),  
        @stats_detail_type varchar(3),  
        @premium_total numeric(19, 4),  
        @premium_sub_total numeric(19, 4),  
        @commission_total numeric(19, 4),  
        @commission_tax_total numeric(19, 4),  
        @commission_sub_total numeric(19, 4),  
        @sum_insured_sub_total numeric(19, 4),  
        @discount_total numeric(9, 4),  
        @tax_total numeric(9, 4),  
        @agent_cnt int,  
        @nominal_ledger_code varchar(2),  
        @lead_agent_cnt int,  
        @insurance_holder_cnt int,  
        @transaction_type char(10),  
        @lead_agent_shortname varchar(30),  
        @insurance_holder_shortname varchar(30),  
        @lead_agent_account_key int,  
        @insurance_holder_account_key int,  
        @class_of_business_code varchar(10),  
		@peril_type_id int,
        @lead_agent_type int,  
        @acc_type varchar(1),  
        @CommissionAccount VARCHAR(20),  
        @CommissionLedgerCode varchar(2),  
        @CommisionAccountTypeCode CHAR(10),  
        @CommissionAccountKey INT,  
        @PostAccount VARCHAR(20),  
        --Create Tax Revenue transactions   
        @tax_value numeric(19, 4),   
        @tax_type_code varchar(13),  
        @tax_account varchar(20),  
        @tax_group_id int,  
        @tax_band_id int,  
        @so_tax_posted_by_band tinyint,  
        --Create Fee transactions   
        @fee_amount MONEY,  
        @fee_account VARCHAR(20),  
        @currency_rate numeric(19, 4),  
        @company_id INT,  
        @currency_id INT,  
        @insurance_file_cnt INT,  
        @effective_date DATETIME,  
        --True Monthly Policy  
        @SuspenseAccount VARCHAR(20),  
        @ReleaseAccountCode VARCHAR(20),  
        @leadconsolidate tinyint, --(TMP)  
        @suspend tinyint,  
        @releaseToIncome tinyint, --(TMP)  
        @transType varchar(10),  
        @transCodeType as varchar (10),  
        @monthLeft as int,  
        @cycle as tinyint,  
        @intermediary_agent_account_id INT,  
        @agent_account_id INT,  
        @BalanceType AS VARCHAR(10),  
		@CashDepositBasePartyCnt AS INT,
		@lBusiness_type_id int,              
		@sCoins_placement  varchar(10),    
		@lCoins_party_cnt int,                               
		@sCoins_shortname varchar(20),  
		@nCoins_premium_total numeric(19, 4),
		@nBusinessTypeIdCoinsLead int = 3,
		@nBusinessTypeIdCoinsFollow int = 4,
		@sFee_party AS VARCHAR(50),
		@nPostFeeAndTaxSeparately AS INT,
		@sTransdetail_Type_Code AS VARCHAR(20) 
      
       SELECT  @nPostFeeAndTaxSeparately = ISNULL(value, 0)
        FROM    system_options  
        WHERE   branch_id = 1  
        AND     option_number = 5118
      
    -- get details of agent commission account - if applicable  
    SELECT  @CommissionAccount = a.short_code,  
            @CommissionLedgerCode = l.ledger_short_name,  
            @CommisionAccountTypeCode = at.code  
    FROM    stats_folder sf  
    JOIN    Party_agent pa               ON sf.agent_cnt = pa.party_cnt  
    JOIN    Account a                    ON pa.party_cnt = a.account_key  
    JOIN    Ledger l                     ON a.ledger_id = l.ledger_id  
    JOIN    AccountType at               ON a.accounttype_id = at.accounttype_id  
    WHERE   sf.stats_folder_cnt = @stats_folder_cnt  
    AND     pa.Party_Agent_Type_id = '3'                -- commission account type  
      
    -- Get account type from hidden options  
    SELECT  @acc_type = Acc_Type  
    FROM    hidden_options  
    WHERE   branch_id = 1  
    AND     option_number = 1  
      
    -- JMK 19/05/2003 premium is ORIGINAL currency but commission value is HOME so we need to convert commission back to Original value  
    -- Get insurance file  
    SELECT  @insurance_file_cnt = insurance_file_cnt  
    FROM    stats_folder  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
      
      
    -- Get rate from insurance file  
    SELECT  @company_id = source_id,  
            @currency_id = currency_id,  
            @currency_rate = currency_base_xrate,  
   @intermediary_agent_account_id = intermediary_agent_account_id,  
            @BalanceType=Balance_Type,
            @sCoins_placement = coins_placement,          
            @lBusiness_type_id = business_type_id          
    FROM    insurance_file  
    WHERE   insurance_file_cnt = @insurance_file_cnt  
      
    DECLARE @NO_OF_COIS INT =0

    SELECT @NO_OF_COIS = COUNT(*) FROM Coi_Value WHERE insurance_file_cnt = @insurance_file_cnt
	SET @NO_OF_COIS = ISNULL(@NO_OF_COIS,0) 		

    	If (@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) and ((@sCoins_placement = 'NETT' OR @sCoins_placement = 'GROSS')) BEGIN  
			
			SELECT top 1  @lCoins_party_cnt= cv.party_cnt,                                     
					@sCoins_shortname=  p.shortname 
		    FROM    Coi_Value cv  JOIN    Party p  ON p.party_cnt = cv.party_cnt              
					JOIN    Party_insurer pin      ON pin.party_cnt = p.party_cnt              
			WHERE   cv.insurance_file_cnt = @insurance_file_cnt              
			AND     ISNULL(pin.is_retained, 0) = 0    order by coi_value_id asc , share_percent desc
		
     END

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
      
    -- Determine transaction type   
    SELECT  @lead_agent_cnt = agent_cnt,  
            @lead_agent_account_key = agent_account_key,  
            @lead_agent_shortname = agent_shortname,  
            @insurance_holder_cnt = insurance_holder_cnt,  
            @insurance_holder_account_key = insurance_holder_account_key,  
            @insurance_holder_shortname = insurance_holder_shortname  
    FROM    Transaction_Export_Folder T  
    WHERE   T.transaction_export_folder_cnt = @transaction_export_folder_cnt  
      
    -- Get column values required by SALES EXPORT   
    SELECT  @transaction_ledger_code = 'SL',  
            @account_type_code = 'SALESLEDGR',  
            @cover_share_percent = 0,  
            @sum_insured_total = 0,  
            @charges_total = 0, @taxes_total = 0,  
            @recoveries_total = 0,  
            @commission_excluded = 0,  
            @withholding_tax_excluded = 0,  
            @nominal_ledger_code = 'NO'  
      
    -- Get totals from stats table   
    SELECT  @premium_total = sum(this_premium_original) ,  
            -- AG 20/10/2004 - PN15402 - Get agent commission in transction currency.  
            -- lead_commission_value_home contains commission in base currency for lead agent.  
            @commission_total = SUM(CASE WHEN currency_rate<>0 THEN lead_commission_value_home/currency_rate ELSE lead_commission_value_home END),  
            @sum_insured_total = sum(sum_insured_home)   
    FROM    stats_detail  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
    AND     stats_detail_type = 'GRS'  
      
       If (@lBusiness_type_id = @nBusinessTypeIdCoinsLead) and (@sCoins_placement = 'GROSS')      
			 BEGIN  
				SELECT @nCoins_premium_total = sum(this_premium_original)
				FROM    stats_detail        
				WHERE   stats_folder_cnt = @stats_folder_cnt        
				AND     stats_detail_type = 'COI'      
				SELECT @premium_total = @premium_total --+    @nCoins_premium_total
			 END  

      
    -- Retrieve tax using new stats type for gross tax.  
    SELECT  @taxes_total = SUM(tax_value)   
    FROM    stats_detail  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
    AND     stats_detail_type = 'TAG'  
      
    SELECT  @discount_total = SUM(this_premium_original)   
    FROM    stats_detail  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
    AND     stats_detail_type = 'DIS'  
      
    SELECT  @charges_total = SUM(this_premium_original)   
    FROM    stats_detail  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
    AND     stats_detail_type = 'PFG'  
      
    -- Get peril code from stats table   
    -- Note that there may be many records with different peril type codes - why are we doing a select like this?  
    SELECT  @peril_type_code = peril_type_code  
    FROM    stats_detail  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
      
    SELECT  @premium_total = ISNULL(@premium_total, 0),  
            @commission_total = ISNULL(@commission_total, 0),  
            @sum_insured_total = ISNULL(@sum_insured_total, 0),  
            @discount_total = ISNULL(@discount_total, 0),  
            @charges_total = ISNULL(@charges_total, 0),  
            @taxes_total = ISNULL(@taxes_total, 0)  
      
    -- Determine transaction type   
    SELECT  @lead_agent_cnt = lead_agent_cnt  
    FROM    Insurance_File I  
    JOIN    Transaction_Export_Folder T  
            ON I.insurance_file_cnt = T.insurance_file_cnt  
    WHERE   T.transaction_export_folder_cnt = @transaction_export_folder_cnt  
      
  
    -- **************************************************************************************  
    --                                 WRITE GROSS ELEMENT  
    -- **************************************************************************************  
  
    -- Direct client transaction   
    IF @lead_agent_cnt IS NULL BEGIN -- No lead agent  
        SELECT  @transaction_amount = @premium_total + @discount_total,  
                @mapping_code = @insurance_holder_shortname,  
                @transaction_account_key = @insurance_holder_account_key,  
                @spare = 'GROSS' 
    END 
    ELSE
     BEGIN -- lead agent  
        SELECT  @transaction_type = C.code  
        FROM    Party P  
        JOIN    Collect_Type C  
                ON C.collect_type_ID = P.collect_type_ID  
        WHERE   P.party_cnt = @lead_agent_cnt  
  
  --Start - Renuka - (WPR85_Cash_Deposit_Process)  
  IF @BalanceType='CD'  
  BEGIN  
   --Get the party to which the cash deposit account belongs to  
   SELECT   
    @CashDepositBasePartyCnt=Party_ID  
   FROM  
    CashDeposit  
   WHERE  
    Account_ID=@intermediary_agent_account_id  
     
   --If party is agent, use agent ledger. Else, use client ledger  
   IF @CashDepositBasePartyCnt=@lead_agent_cnt  
   BEGIN  
    SELECT  @transaction_amount = @premium_total,    
      @mapping_code = @lead_agent_shortname,    
      @transaction_account_key = @lead_agent_account_key,    
      @spare = 'GROSS',    
      -- RWH (06/12/2000) Added next 2 lines to map correctly to Orion    
      @transaction_ledger_code = 'AG',    
      @account_type_code = 'AGENTLEDGR'  
   END  
   IF @CashDepositBasePartyCnt=@insurance_holder_cnt  
   BEGIN  
    SELECT  @transaction_amount = @premium_total + @discount_total,    
      @mapping_code = @insurance_holder_shortname,    
      @transaction_account_key = @insurance_holder_account_key,    
      @spare = 'GROSS'  
   END  
  END   
  ELSE  
  BEGIN  
  --End - Renuka - (WPR85_Cash_Deposit_Process)  
   SELECT  @lead_agent_type = party_agent_type_id  
   FROM    Party_Agent   
   WHERE   Party_Agent.party_cnt = @lead_agent_cnt  
   SELECT  @agent_account_id = account_id   
   FROM    ACCOUNT A   
   WHERE   A.Account_Key = @lead_agent_account_key  
          
   -- Special processing for a Commission Agent - premium posted to client acc   
   IF (@lead_agent_type = 3 OR @agent_account_id <> ISNULL(@intermediary_agent_account_id,@agent_account_id)) 
   BEGIN --Commission Agent  
    SELECT  @transaction_amount = @premium_total + @discount_total,  
      @mapping_code = @insurance_holder_shortname,  
      @transaction_account_key = @insurance_holder_account_key,  
      @spare = 'GROSS'  
   END   
   ELSE  
   BEGIN --Not Commission Agent  
    SELECT  @transaction_amount = @premium_total,  
      @mapping_code = @lead_agent_shortname,  
      @transaction_account_key = @lead_agent_account_key,  
      @spare = 'GROSS',  
      -- RWH (06/12/2000) Added next 2 lines to map correctly to Orion   
      @transaction_ledger_code = 'AG',  
      @account_type_code = 'AGENTLEDGR'  
   END  
  --Renuka - WPR85_Cash_Deposit_Process  
  END  
    END  
      
    -- RWH(11/07/01) Update stats_folder with total gross premium.  
    UPDATE  stats_folder  
    SET     premium_total = @transaction_amount  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
      
    -- Set transaction_detail_id   
    SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
    FROM    Transaction_Export_Detail  
    WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
          If (@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) and (@sCoins_placement = 'GROSS') AND @NO_OF_COIS > 1   
			Begin       
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
						transaction_account_key,        
						spare,        
						transdetail_type_code)        
				VALUES (@transaction_export_folder_cnt,        
						@transaction_export_detail_id,        
						@transaction_amount,        
						'IN',        
						'INSURERLED',        
						@ceded_ref,        
						@cover_share_percent,        
						@sum_insured_total,        
						@charges_total,        
						@taxes_total,        
						@recoveries_total,        
						@commission_excluded,        
						@withholding_tax_excluded,        
						@sCoins_shortname,        
						@lCoins_party_cnt,        
						'GROSS',        
						'GROSS')        
      
		END    
    
    Else    
    Begin      
    -- Insert the Trans Export Detail (premium total)  
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
            @sum_insured_total,  
            @charges_total,  
            @taxes_total,  
            @recoveries_total,  
            @commission_excluded,  
            @withholding_tax_excluded,  
            @mapping_code,  
            @transaction_account_key,  
            @spare,  
            'GROSS')  
      End
      
    -- **************************************************************************************  
    --                               WRITE GROSS TAX ELEMENT  
    -- **************************************************************************************  
  
    IF @lead_agent_cnt IS NULL BEGIN  
        SELECT  @transaction_amount = @taxes_total,  
                @mapping_code = @insurance_holder_shortname,  
                @transaction_account_key = @insurance_holder_account_key,  
                @spare = 'TAX'  
    END   
 ELSE   
 BEGIN  
  --Start Renuka - WPR85_Cash_Deposit_Process  
  IF @BalanceType='CD'  
  BEGIN  
    
   --If party is agent, use agent ledger. Else, use client ledger  
   IF @CashDepositBasePartyCnt=@lead_agent_cnt  
   BEGIN  
    SELECT  @transaction_amount = @taxes_total,    
      @mapping_code = @lead_agent_shortname,    
      @transaction_account_key = @lead_agent_account_key,    
      @spare = 'TAX',    
      @transaction_ledger_code = 'AG',    
      @account_type_code = 'AGENTLEDGR'  
   END  
   ELSE IF @CashDepositBasePartyCnt=@insurance_holder_cnt  
   BEGIN  
    SELECT  @transaction_amount = @taxes_total,    
      @mapping_code = @insurance_holder_shortname,    
      @transaction_account_key = @insurance_holder_account_key,    
      @spare = 'TAX'  
   END  
  END  
  ELSE  
  BEGIN  
  --End Renuka - WPR85_Cash_Deposit_Process  
        SELECT  @lead_agent_type = party_agent_type_id  
        FROM    Party_Agent   
        WHERE   Party_Agent.party_cnt = @lead_agent_cnt  
  
        -- Special processing for a Commission Agent - tax posted to client acc   
        IF (@lead_agent_type = 3 OR @agent_account_id <> ISNULL(@intermediary_agent_account_id,@agent_account_id)) BEGIN    --Commission Agent  
            SELECT  @transaction_amount = @taxes_total,  
                    @mapping_code = @insurance_holder_shortname,  
                    @transaction_account_key = @insurance_holder_account_key,  
                    @spare = 'TAX'  
        END   
ELSE  
        BEGIN  
              If (@lBusiness_type_id = @nBusinessTypeIdCoinsFollow) and (@sCoins_placement = 'GROSS') AND @NO_OF_COIS > 1  
			BEGIN
				 --set @lead_agent_account_key = @lCoins_party_cnt
				 SELECT  @transaction_amount = @taxes_total,  
						@mapping_code = @sCoins_shortname,  
						@transaction_account_key = @lCoins_party_cnt,  
						@spare = 'TAX',  
						@transaction_ledger_code = 'IN',  
						@account_type_code = 'INSURERLED'  
			END
			ELSE  
			BEGIN   
				SELECT  @transaction_amount = @taxes_total,  
						@mapping_code = @lead_agent_shortname,  
						@transaction_account_key = @lead_agent_account_key,  
						@spare = 'TAX',  
						@transaction_ledger_code = 'AG',  
						@account_type_code = 'AGENTLEDGR' 
			END	
        END  
--Renuka - WPR85_Cash_Deposit_Process  
END  
END  

  If @nPostFeeAndTaxSeparately = 0  
    -- Check if taxes posted rolled up or by group and band  
    SELECT  @so_tax_posted_by_band = value   
    FROM    system_options   
    WHERE   branch_id = 1   
    AND     option_number = 5016  
  
    IF ISNULL(@so_tax_posted_by_band, 0) = 1 BEGIN  
        -- Taxes are posted to client account by band and group, get from base tables  
        DECLARE TaxesCursor CURSOR FAST_FORWARD FOR  
            SELECT  SUM(value) tax,  
                    tax_group_id,  
                    tax_band_id,
					NULL   
            FROM    tax_calculation  
            WHERE   insurance_file_cnt = @insurance_file_cnt  
            AND     transtype IN ('TTR', 'TTIF', 'TTF') -- Risk and insurance file taxes only  
            AND     ISNULL(is_not_applied_to_client, 0) = 0 -- Filter out taxes not applied to client  
            AND     (risk_cnt is NULL OR risk_cnt IN (select risk_cnt from insurance_file_risk_link where risk_cnt=tax_calculation.risk_cnt)) --40744, PN 49127   
            AND     (insurance_file_cnt  NOT IN (SELECT insurance_file_cnt FROM Insurance_File_System WHERE insurance_file_cnt = @insurance_file_cnt AND last_trans_type_id = 21))    
            GROUP BY tax_group_id, tax_band_id  
			UNION
			SELECT  SUM(this_premium) ,
				NULL,
				NULL,
				NULL
			FROM    peril
            WHERE   (risk_cnt IN (select risk_cnt from insurance_file_risk_link where insurance_file_cnt=@insurance_file_cnt and status_flag IN ('C','D'))) --40744, PN 49127
             AND     ISNULL(is_levy_tax,0) =1
    END ELSE BEGIN  
	If @nPostFeeAndTaxSeparately = 0    
        -- Use mock cursor for rolled up tax  
        DECLARE TaxesCursor CURSOR FAST_FORWARD FOR  
            SELECT  @taxes_total tax,  
                    null,  
                    null,null
ELSE
			DECLARE TaxesCursor CURSOR FAST_FORWARD FOR  
				SELECT  SUM(tax_value),
						null,  
						null,ri_shortname  	
				FROM    stats_detail  
				WHERE   stats_folder_cnt = @stats_folder_cnt
				AND     stats_detail_type = 'TAG' and peril_type_id is null							
				GROUP BY ri_shortname  
    END      
  
    OPEN TaxesCursor  
    FETCH NEXT FROM TaxesCursor   
        INTO @transaction_amount, @tax_group_id, @tax_band_id, @sFee_party     
  
    WHILE @@FETCH_STATUS = 0 BEGIN  
        -- Insert the Trans Export Detail (Tax total)   
        IF  @transaction_amount <> 0 BEGIN  
            -- Set transaction_detail_id   
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
            FROM    Transaction_Export_Detail  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
          
		  	If @sFee_party IS NULL
				SET @sTransdetail_Type_Code = 'TAX'
			Else
				SET @sTransdetail_Type_Code = 'FEETAX' 

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
                    transaction_account_key,  
                    spare,  
                    transdetail_type_code,  
                    tax_group_id,  
                    tax_band_id, 
		            fee_type)   
            VALUES (@transaction_export_folder_cnt,  
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
                    @transaction_account_key,  
                    @spare,  
                    @sTransdetail_Type_Code,  
                    @tax_group_id,  
                    @tax_band_id,
		           @sFee_party)  
        END  
  
        FETCH NEXT FROM TaxesCursor   
            INTO @transaction_amount, @tax_group_id, @tax_band_id,@sFee_party 
    END  
  
    CLOSE TaxesCursor  
    DEALLOCATE TaxesCursor  
  
    -- **************************************************************************************  
    --                               WRITE GROSS FEE ELEMENT  
    -- **************************************************************************************  
    -- Insert the Trans Export Detail (Fee total)   
    IF @charges_total <> 0 AND @nPostFeeAndTaxSeparately = 0 BEGIN  
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
                @charges_total,  
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
                @transaction_account_key,  
                'FEE',  
                'FEE')  
    END  
  	ELSE BEGIN
		DECLARE FeesCursor CURSOR FAST_FORWARD FOR  
			SELECT  SUM(sd.this_premium_original) 'fee_amount',  
					sd.ri_shortname			
			FROM    stats_detail sd 
			LEFT JOIN tax_band tb --E001
			on sd.tax_type_id=tb.tax_band_id
			LEFT JOIN tax_band_rate tbr
			ON tb.tax_band_id=tbr.tax_band_id 		
			WHERE   sd.stats_folder_cnt = @stats_folder_cnt  
			AND     sd.Stats_detail_type = 'PFG'
			GROUP BY sd.ri_shortname
	      
		OPEN FeesCursor  
		FETCH NEXT FROM FeesCursor   
			INTO @fee_amount, @sfee_party
	      
		WHILE @@FETCH_STATUS = 0 BEGIN  
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
					transdetail_type_code,
					fee_type)  
			VALUES (@transaction_export_folder_cnt,  
					@transaction_export_detail_id,  
					@Fee_amount,  
					@transaction_ledger_code,
					@Account_type_code,  
					@Ceded_ref,  
					@Cover_share_percent,  
					@Sum_insured_total,  
					@Charges_total,  
					@Taxes_total,  
					@Recoveries_total,  
					@Commission_excluded,  
					@Withholding_tax_excluded,  
					@Mapping_code,  
					@Transaction_account_key,  
					'FEE',  
					'FEE',
					@sfee_party)    
	  
			FETCH NEXT FROM FeesCursor       
				INTO @fee_amount, @sfee_party
		END  
  
		-- Close and release cursor  
		CLOSE FeesCursor  
		DEALLOCATE FeesCursor  

		
	END
  
    -- **************************************************************************************  
    --                                  WRITE NET ELEMENT  
    -- **************************************************************************************  
  
    -- Process the General Ledger Analysis Export Detail   
    -- Agent Net Accounting transaction   
    IF @transaction_type = 'Net' BEGIN  
        SELECT  @transaction_amount = @premium_total - (@commission_total + @discount_total + @charges_total + @taxes_total)  
    END ELSE BEGIN  
        -- Agent Gross Accounting (Combined) transaction   
        SELECT  @transaction_amount = @premium_total + @discount_total + @charges_total + @taxes_total  
  
        IF @transaction_type = 'Gross_Cros'  
            SELECT  @transaction_ledger_code = 'PU'  
    END  
  
    -- Written Premiums In  
    SELECT  @transaction_account_key = null  
      
    DECLARE IncomeCursor CURSOR FAST_FORWARD FOR  
        SELECT  class_of_business_code,  
                SUM(this_premium_original),  
                -- Get agent commission in transction currency.  
                -- lead_commission_value_home contains commission in base currency for lead agent.  
                SUM(CASE WHEN currency_rate<>0 THEN lead_commission_value_home/currency_rate ELSE lead_commission_value_home END),  
                SUM(sum_insured_home)  
				,peril_type_id
        FROM    stats_detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
        AND     stats_detail_type = 'GRS'  
        AND     class_of_business_code is not null          -- RWH(02/04/2001) Ignore tax record.  
        GROUP BY class_of_business_code  
		,peril_type_id
  
    OPEN IncomeCursor  
    FETCH NEXT FROM IncomeCursor   
        INTO @class_of_business_code, @premium_sub_total, @commission_sub_total, @sum_insured_sub_total,@peril_type_id  
  
    WHILE (@@FETCH_STATUS = 0) BEGIN  
        IF (@acc_type <> 'I') OR (@acc_type IS NULL) BEGIN  
            -- Set transaction_detail_id   
            SELECT  @transaction_amount = -@transaction_amount,  
                    @transaction_ledger_code = 'NO',  
                    -- Income \ Gross Written Premium.  
                    @account_type_code = 'INCGWP'           -- RWH(01/05/01) More detailed account types.  
  
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
                    cover_share_percent,  
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
                    -@premium_sub_total,  
                    @transaction_ledger_code,  
                    @account_type_code,  
                    @ceded_ref,  
                    @cover_share_percent,  
                    -@sum_insured_sub_total,  
                    @charges_total,  
                    @taxes_total,  
                    @recoveries_total,  
                    @commission_excluded,  
                    @withholding_tax_excluded,  
                    'NOIN' + @class_of_business_code,  
                    @transaction_account_key,
					'GROSS')  
        END  
		--------------------------------------------
		if @lBusiness_type_id=@nBusinessTypeIdCoinsLead or @lBusiness_type_id=@nBusinessTypeIdCoinsFollow
		BEGIN
			if exists(select 1 from peril_type where is_stamp_duty_insured=1 and peril_type_id=@peril_type_id)
			begin
			SELECT @account_type_code = 'EXPGWP'	
				SELECT @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1
				FROM Transaction_Export_Detail
				WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt
				INSERT INTO Transaction_Export_Detail (
					transaction_export_folder_cnt
					,transaction_export_detail_id
					,transaction_amount
					,transaction_ledger_code
					,account_type_code
					,ceded_ref
					,cover_share_percent
					,sum_insured_total
					,charges_total
					,taxes_total
					,recoveries_total
					,commission_excluded
					,withholding_tax_excluded
					,mapping_code
					,transaction_account_key
					,transdetail_type_code
					)
				VALUES (
					@transaction_export_folder_cnt
					,@transaction_export_detail_id
					,@premium_sub_total
					,@transaction_ledger_code
					,@account_type_code
					,@ceded_ref
					,@cover_share_percent
					, @sum_insured_sub_total
					,@charges_total
					,@taxes_total
					,@recoveries_total
					,@commission_excluded
					,@withholding_tax_excluded
					,'NOIN' + @class_of_business_code
					,@transaction_account_key
					,'GROSS'
					)
			END
		END
	--------------------------------------------
        -- Commission record should be created for each class of business.  
            SELECT  @transaction_amount = @commission_sub_total,  
                    @transaction_ledger_code = 'NO',  
                    -- Expenses \ Lead Commission  
                    @account_type_code = 'EXPCOM'           -- RWH(01/05/01) More detailed account types.  
  
            -- Set transaction_detail_id  
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
            FROM    Transaction_Export_Detail  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
        if @transaction_amount <>0
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
                    0,  
                    @charges_total,  
                    @taxes_total,  
                    @recoveries_total,  
                    @commission_excluded,  
                    @withholding_tax_excluded,  
                    'CO' + @class_of_business_code,     --RWH(30/04/01) New nominal ledger account for commission.  
                    @transaction_account_key,  
                    'COMSUSP',
					'COMM')  
  
        FETCH NEXT FROM IncomeCursor   
            INTO @class_of_business_code, @premium_sub_total, @commission_sub_total, @sum_insured_sub_total ,@peril_type_id
    END  
  
    CLOSE IncomeCursor  
    DEALLOCATE IncomeCursor  
      
  
    -- **************************************************************************************  
    --                              WRITE NET COMMISSION ELEMENT  
    -- **************************************************************************************  
    -- Lead Commission Out  
    IF @commission_total <> 0 or @lead_agent_cnt <> 0 BEGIN  
        -- Get the Tax on Commission  
        SELECT  @commission_tax_total = SUM(this_premium_original)  
        FROM    Stats_Detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
        AND     stats_detail_type = 'TCO'  
  AND  ceded_ref = @lead_agent_shortname  
  
        -- Ensure it's not null  
        SELECT  @commission_tax_total = ISNULL(@commission_tax_total, 0)  
          
  --If leadconsolidate option is ticked.(TMP)  
  SELECT  @leadconsolidate = inf.lead_allow_consolidated_commission,   
    @suspenseAccount = a.short_code,  
    @transType = tef.transaction_type_code,  
    @cycle = p.lead_month_in_cycle  
  FROM product p  
  JOIN account a ON a.account_id = p.lead_suspense_account_id  
  JOIN insurance_file inf on inf.product_id=p.product_id  
  JOIN transaction_export_folder tef ON tef.insurance_file_cnt = inf.insurance_file_cnt  
  WHERE tef.transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
        -- Made ledger code and account type dependent on agent   
        IF @lead_agent_cnt IS NULL BEGIN  
            SELECT  @transaction_amount = -@commission_total,    
                    @transaction_ledger_code = 'SL',  
                    @account_type_code = 'SALESLEDGR',
                    @PostAccount = @mapping_code,
                    @CommissionAccountKey = @insurance_holder_account_key
        END ELSE BEGIN  
            IF (@CommissionAccount IS NULL) and (NOT(@lBusiness_type_id=@nBusinessTypeIdCoinsFollow and @sCoins_placement= 'GROSS') OR @NO_OF_COIS =1)  BEGIN
                --Posting to agent account  
                SELECT  @transaction_amount = -@commission_total,    
                        @transaction_ledger_code = 'AG',  
                        @account_type_code = 'AGENTLEDGR',  
                        @spare = 'COMM',  
                        @PostAccount = @lead_agent_shortname,  
                        @CommissionAccountKey = @lead_agent_account_key  
            END ELSE IF (@lBusiness_type_id=@nBusinessTypeIdCoinsFollow and @sCoins_placement= 'GROSS') BEGIN  
                --Posting to Coins account
			           SELECT @CommissionAccountKey=  p.party_cnt,   -- Account Key
                       @PostAccount=   p.shortname        -- Account Mapping
                            
                        FROM    stats_detail d
                        JOIN    party p ON p.party_cnt = d.ri_party_cnt
                        WHERE   d.stats_folder_cnt = @stats_folder_cnt
                        AND     d.stats_detail_type IN ('COI')
                        GROUP BY
                        p.party_cnt, p.shortname,  d.class_of_business_code
                 
				 
                       SELECT  @transaction_amount = -@commission_total,    
                        @transaction_ledger_code = 'IN',  
                        @account_type_code = 'COINSACC',  
                        @spare = 'COMM'  
                       
            END ELSE
			
			BEGIN  
                --posting to commission suspense account on scheme because it is spread  
                SELECT  @transaction_amount = -@commission_total,    
                        @transaction_ledger_code = @CommissionLedgerCode,  
                        @account_type_code = @CommisionAccountTypeCode,  
                        @spare = 'COMM',  
                        @CommissionAccountKey = 0,  
                        @PostAccount = @CommissionAccount  
            END  
        END  
      
        --(RC) PLICO 9-10   
 DECLARE @commission_posting_type_id INT,   
  @agent_code varchar(50),  
         @agent_commission_suspended_postings INT,  
  @manually_released INT,  
  @released_on_full_settlement INT,  
  @released_for_whole_posting INT,  
  @released_on_policy_effective INT  
  
 --get system option  
        SELECT  @agent_commission_suspended_postings = value  
        FROM    system_options  
        WHERE   branch_id = 1  
        AND     option_number = 5037  
  
 --get comm posting type and agent type code  
 SELECT @commission_posting_type_id = PA.commission_posting_type_id,   
 @agent_code = PAT.code  
 FROM party_agent PA   
 INNER JOIN party_agent_type PAT ON PA.party_agent_type_id = PAT.party_agent_type_id  
 WHERE PA.party_cnt = @lead_agent_cnt  
  
 IF ((ISNULL(@agent_commission_suspended_postings, 0) = 1) AND  
  (@commission_posting_type_id = 2) AND (RTRIM(@agent_code) = 'Comm Acc')) BEGIN  
  -- SELECT @suspenseAccount = account_id for system option   
  -- Agent suspense account code  
         SELECT  @suspenseAccount = value  
         FROM    system_options  
         WHERE   branch_id = 1  
         AND     option_number = 5039  
 END  
  
  IF (@leadconsolidate = 1) and (@transType = 'NB') and (@cycle >= 1) BEGIN  
  SELECT  @releaseAccountCode = @PostAccount,  
   @CommissionAccountKey = NULL,  
   @PostAccount = @suspenseAccount,  
   @suspend = 1,  
   @releaseToIncome = 0  
 END    
  
 IF ((ISNULL(@agent_commission_suspended_postings, 0) = 1) AND (@commission_posting_type_id = 2) AND (RTRIM(@agent_code) = 'Comm Acc') AND (@transaction_amount <> 0)) 
 BEGIN
  SELECT    @releaseAccountCode = @PostAccount,  
   @CommissionAccountKey = NULL,  
   @PostAccount = @suspenseAccount,  
   @suspend = 1,  
    @releaseToIncome = 0,  
   @manually_released = 1,  
   @released_on_full_settlement = 1,  
   @released_for_whole_posting = 1,  
   @released_on_policy_effective = 1  
 END  
    
        -- Set transaction_detail_id   
        SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
        FROM    Transaction_Export_Detail  
        WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
      if @transaction_amount<> 0
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
                transaction_account_key,  
                spare,  
                suspended,  
  release_to_income,  
  release_account_code,  
                transdetail_type_code,  
  manually_released,  --(RC) PLICO 9-10  
  released_on_full_settlement,  --(RC) PLICO 9-10  
  released_for_whole_posting,  --(RC) PLICO 9-10  
  released_on_policy_effective  --(RC) PLICO 9-10  
  )  
        VALUES (@transaction_export_folder_cnt,  
                @transaction_export_detail_id,  
                @transaction_amount,  
                @transaction_ledger_code,  
                @account_type_code,  
                @ceded_ref,  
                @cover_share_percent,  
                null,  
                @charges_total,  
                @taxes_total,  
                @recoveries_total,  
                @commission_excluded,  
                @withholding_tax_excluded,  
                @PostAccount,  
                @CommissionAccountkey,  
                @spare,  
                @suspend,  
  @releaseToIncome,  
  @releaseAccountCode,  
                'COMM',  
  @manually_released,  --(RC) PLICO 9-10  
  @released_on_full_settlement,  --(RC) PLICO 9-10  
  @released_for_whole_posting,  --(RC) PLICO 9-10  
  @released_on_policy_effective  --(RC) PLICO 9-10  
                )  
            -- DD 13/12/2005 - Reversal of PN23800  
            -- Tax on Commission should be rolled up into Commission amount  
            -- rather than into Tax on Premium Amount (if Broker)  
            -- This may change in the future so the basic logic remains  
            -- and instead just altering the Spare field.  
            IF @CommissionAccount IS NULL BEGIN  
                --Posting to agent account  
                SELECT  @transaction_amount = (@commission_tax_total) * -1,  
                        @transaction_ledger_code = 'AG',  
                        @account_type_code = 'AGENTLEDGR',  
                        @spare = 'COMM',  
                        @PostAccount = @lead_agent_shortname,  
                        @CommissionAccountKey = @lead_agent_account_key  
            END ELSE BEGIN  
                --posting to commission suspense account on scheme because it is spread  
                SELECT  @transaction_amount = (@commission_tax_total) * -1,  
                        @transaction_ledger_code = @CommissionLedgerCode,  
                        @account_type_code = @CommisionAccountTypeCode,  
                        @spare = 'COMM',  
                        @CommissionAccountKey = 0,  
                        @PostAccount = @CommissionAccount  
            END  
              
            SELECT @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id) + 1, 1)   
            FROM Transaction_Export_Detail  
            WHERE transaction_export_folder_cnt = @transaction_export_folder_cnt  
            if @transaction_amount<> 0
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
                    null,  
                    @charges_total,  
                    @taxes_total,  
                    @recoveries_total,  
                    @commission_excluded,  
                    @withholding_tax_excluded,  
                    @PostAccount,  
                    @CommissionAccountkey,  
                    @spare,
                    'COMMTAX')   
        END  
  
  
    -- **************************************************************************************  
    --                               WRITE NET FEES ELEMENT  
    -- **************************************************************************************  
    -- Fees/Charges, Tax is rolled up into this value  
    DECLARE FeesCursor CURSOR FAST_FORWARD FOR  
        SELECT  SUM(this_premium_original) 'fee_amount',  
                ri_shortname,	sd.Stats_detail_type  
        FROM    stats_detail  sd
        WHERE   stats_folder_cnt = @stats_folder_cnt  
        AND     Stats_detail_type IN ('PFE', 'TFE')  
        GROUP BY ri_shortname , sd.Stats_detail_type  
      
    OPEN FeesCursor  
    FETCH NEXT FROM FeesCursor   
        INTO @fee_amount, @fee_account , @Stats_detail_type 
      
    WHILE @@FETCH_STATUS = 0 BEGIN  
        SELECT  @transaction_amount = @fee_amount ,  
                @transaction_ledger_code = 'FE',  
                @account_type_code = 'FEELEDGR',  
                @spare = 'FEE'  
  
  		IF @Stats_detail_type = 'TFE'                
			SET @sTransdetail_Type_Code = 'FEETAX'
		ELSE
			SET @sTransdetail_Type_Code = 'FEE'

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
                mapping_code,   
                spare,  
                transdetail_type_code)  
        VALUES (@transaction_export_folder_cnt,  
                @transaction_export_detail_id,  
                @transaction_amount,  
                @transaction_ledger_code,  
                @account_type_code,  
                @fee_account,   
                @spare,  
               @sTransdetail_Type_Code)  
  
        FETCH NEXT FROM FeesCursor       
            INTO @fee_amount, @fee_account, @Stats_detail_type 
    END  
  
    -- Close and release cursor  
    CLOSE FeesCursor  
    DEALLOCATE FeesCursor  
      
      
    -- **************************************************************************************  
    --                            WRITE SECONDARY AGENT ELEMENT  
    -- **************************************************************************************  
  
    -- Create secondary AGENT transaction   
    IF @transaction_type <> 'Net' BEGIN  
        -- Get column values required by SALES EXPORT   
        SELECT  @transaction_amount = -@commission_total,  
                @transaction_ledger_code = 'PU',  
                @account_type_code = 'PURCHLEDGR',  
           @cover_share_percent = 0,  
                @sum_insured_total = 0,  
                @charges_total = 0,  
                @taxes_total = 0,  
                @recoveries_total = 0,  
                @commission_excluded = 0,  
                @withholding_tax_excluded = 0,  
                @nominal_ledger_code = 'NO',  
                @ceded_ref = NULL  
          
        -- Agent Net Accounting transaction (commission_total)  
        IF @transaction_amount <> 0 BEGIN  
            -- Set transaction_detail_id   
            SELECT  @transaction_export_detail_id = ISNULL(MAX(transaction_export_detail_id), 0) + 1  
            FROM    Transaction_Export_Detail  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
      
            -- Insert the Trans Export Detail   
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
            VALUES (@transaction_export_folder_cnt,  
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
                    @lead_agent_shortname,  
                    @lead_agent_account_key)  
      
            -- Insert the Nominal Analysis Export Detail   
      
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
                    cover_share_percent,  
                    sum_insured_total,  
                    charges_total,  
                    taxes_total,  
                    recoveries_total,  
                    commission_excluded,  
                    withholding_tax_excluded,  
                    mapping_code,  
                    transaction_account_key)  
            VALUES (@transaction_export_folder_cnt,  
                    @transaction_export_detail_id,  
                    -@transaction_amount,  
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
                    @peril_type_code,  
                    @transaction_account_key)  
        END  
    END  
      
    -- **************************************************************************************  
    --                              WRITE NET TAXES ELEMENT  
    -- **************************************************************************************  
  
    DECLARE TaxesCursor CURSOR FAST_FORWARD FOR  
        SELECT  sum(tax_value) tax,  
                tax_type_code,  
                ri_shortname,  
                stats_detail_type  
        FROM    stats_detail  
        WHERE   stats_folder_cnt = @stats_folder_cnt  
        AND     Stats_detail_type IN ('TAN', 'TAX')  
        AND     tax_type_code IS NOT NULL  
        GROUP BY tax_type_code, ri_shortname, stats_detail_type  
      
    OPEN TaxesCursor  
    FETCH NEXT FROM TaxesCursor   
        INTO @tax_value, @tax_type_code, @tax_account, @stats_detail_type  
      
    WHILE (@@FETCH_STATUS = 0) BEGIN  
        -- Account for tax types not applied to customer.  
        IF @stats_detail_type = 'TAX' BEGIN  
            SELECT  @transaction_ledger_code = 'NO',  
                    -- Expense \ Tax Expense  
                    @account_type_code = 'EXPTAX',  
                    @spare = 'TAX'  
        END ELSE BEGIN  
            SELECT  @transaction_ledger_code = 'NO',  
                    -- Liabilities \ Current Liabilities \ Tax  
                    @account_type_code = 'LIABTAX',  
                    @spare = 'TAX'  
        END  
      
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
                mapping_code,   
                spare,  
                transdetail_type_code)  
        VALUES (@transaction_export_folder_cnt,  
                @transaction_export_detail_id,  
                @tax_value,  
                @transaction_ledger_code,  
                @account_type_code,  
                @tax_account,   
                @spare,  
                'TAX')  
      
        FETCH NEXT FROM TaxesCursor   
            INTO @tax_value, @tax_type_code, @tax_account, @stats_detail_type  
    END  
  
    CLOSE TaxesCursor  
    DEALLOCATE TaxesCursor  
      
