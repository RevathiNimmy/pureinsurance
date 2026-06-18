
EXECUTE DDLDropProcedure 'spu_add_trans_details_clm_reins_ri2007'
GO

CREATE PROCEDURE spu_add_trans_details_clm_reins_ri2007  
    @transaction_export_folder_cnt int,  
    @stats_folder_cnt int  
AS  
    /****************************************************************************************/  
    /* sp_add_trans_details_clm_reins creates transaction details records for               */  
    /* reinsurance transactions export.                                                     */  
    /*                                                                                      */  
    /* 2 parameters are passed in - @stats_folder_cnt, @insurance_file_cnt                  */  
    /*                                                                                      */  
    /* This stored procedure is called by sp_add_stats_details_control.                     */  
    /*                                                                                      */  
    /* A failure in this procedure will be passed back to the calling procedure.            */  
    /****************************************************************************************/  
    /* Revision Description of Modification                                 Date        Who */  
    /* -------- ---------------------------                                 ----        --- */  
    /* 1.0      Original                                                    27/06/1997  TF  */  
    /* 1.1      Ensure we pick up coinsurer stats with ri_party_cnt                         */  
    /*          =0 (Nominal reserve accounts)                               11/09/2001  RWH */  
    /* 1.2      Use ri_shortname to match to insurer in treaty2 cursor as                   */  
    /*          ri_party will be 0 for open & maintain claims.              13/09/2001  RWH */  
    /* 1.3      Credit the O/S account for a payment NOT the same one as                    */  
    /*          when adding a reserve.                                      14/09/2001  RWH */  
    /* 1.4      Amalgamate entries for same account. This may occur when                    */  
    /*          multipla insurers are attached and postings are made to the                 */  
    /*          same O/S claims account.                                    14/09/2001  RWH */  
    /* 1.5      Reverse sign of transaction for SAL and recovery.           01/10/2001  RWH */  
    /* 1.6      Unreverse sign of transaction for TPR & SAL. This is done                   */  
    /*          by reversing sign of main value in code.                    09/10/2001  RWH */  
    /****************************************************************************************/  
  
    -- Declare variable for all columns in transaction details table  
    DECLARE  
        -- Working variables  
        @transaction_type char(10),  
        @account_suffix varchar(3),  
        -- RI Cursor variables  
        @ri_party_cnt int,  
        @ri_shortname varchar(30),  
        @ri_share_percent  numeric(19, 4),  
        @ri_premium_total numeric(19, 4),  
        @ri_sum_insured_total numeric(19, 4),  
        @ri_stats_detail_type varchar(3),  
        -- COB Cursor variables  
        @cob_class_of_business_code varchar(30),  
        @cob_premium_total numeric(19, 4),  
        @cob_sum_insured_total numeric(19, 4),  
        @cob_ri_stats_detail_type varchar(3),  
        -- Output variables  
        @out_transaction_export_detail_id int,  
        @out_transaction_ledger_code varchar(2),  
        @out_account_type_code varchar(20),  
        @out_claim_ref varchar(30),  
        @out_nominal_ledger_account varchar(20),  
        @out_nominal_ledger_code varchar(2),  
		@prefix varchar(10),  
		@out_ri_party_cnt INT,  
		@Claim_id int,  
		@Stats_Transaction_type_code varchar(10),  
		@ri_arrangement_version int,  
		@Base_claim_id int,  
		@isMaintained int,  
		@reinsurancecode char(10),
		@transdetail_type_code Varchar(50)  
  
	Select @Claim_id=loss_id,@Stats_Transaction_type_code=transaction_type_code 
						From stats_folder Where Stats_folder_cnt=@Stats_folder_cnt  
	  
	Select @Base_claim_id = base_claim_id from claim where Claim_id = @Claim_id  
	Select @ri_arrangement_version=ri_arrangement_version  
	From  Claim_ri_arrangement where claim_id =@claim_id  
  
	IF Exists(  
        Select * From claim_ri_arrangement Where claim_id in  
        (Select Claim_id From Claim Where base_claim_id=@Base_Claim_id  
        and Claim_id<@Claim_id and ri_arrangement_version=@ri_arrangement_version-1)  
        )  
        Begin  
            If @Stats_Transaction_type_code='C_CR'  
                  Set @isMaintained=1  
        End  
  
    -- We need the transaction type to decide which nominal ledger accounts to post to,  
    -- Salvage, TP Recovery or the claim itself.  
    -- Actually, we don't now, the main problem is when we make a payment we can also  
    -- amend the reserve. If we only rely on the transaction code then we will post  
    -- reserve amendments as payments and mess up the accounts.  
    SELECT  @transaction_type = transaction_type_code,  
            @out_claim_ref = loss_code  
    FROM    stats_folder  
    WHERE   stats_folder_cnt = @stats_folder_cnt  
  
    -- Get the account suffix for Salvage and Recovery  
    SELECT  @account_suffix =     CASE @transaction_type  
                                      WHEN 'C_SA' THEN 'SAL'  
                                      WHEN 'C_RV' THEN 'TPR'  
                                      ELSE '' END  
  
    -- Get totals from stats table  
    -- RWH(11/09/01) Share has already been stored on stats so get it from there so we  
    -- don't need to link to claim_party. Also outer link stats_detail to Party so we  
    -- get rows where ri_party_cnt = 0 (Nominal reserve accounts)  
    DECLARE RI_Cursor CURSOR FAST_FORWARD FOR  
        SELECT  p.party_cnt,  
                d.ri_shortname,  
                isnull(sum(d.ri_share_percent), 0),  
                isnull(sum(d.this_premium_original), 0),  
                isnull(sum(d.sum_insured_home), 0),  
                d.stats_detail_type  
        FROM    stats_detail d  
        LEFT JOIN  
                party p ON p.party_cnt = d.ri_party_cnt  
        LEFT JOIN  
				party_insurer PIN ON p.party_cnt = PIN.party_cnt  
  
        WHERE   d.stats_folder_cnt = @stats_folder_cnt  
        AND     d.stats_detail_type IN('TTY','TFS', 'TAT', 'XOL', 'TAL', 'FAC', 'TAF', 'FAX', 'TYX')  
        AND  ISNULL(PIN.is_retained, 0) = 0  
        GROUP BY  
                d.ri_party_cnt, d.ri_shortname, p.party_cnt, d.stats_detail_type  
  
      -- Open cursor  
    OPEN RI_Cursor  
  
    -- Get first row  
    FETCH NEXT FROM RI_Cursor  
        INTO    @ri_party_cnt,  
                @ri_shortname,  
                @ri_share_percent,  
                @ri_premium_total,  
                @ri_sum_insured_total,  
                @ri_stats_detail_type  
  
    WHILE (@@FETCH_STATUS = 0)  
    BEGIN  
        -- Check we have a premium to post  
        IF @ri_premium_total <> 0 or @isMaintained=1  
        BEGIN  
            -- There are a few working codes we need, we can no longer use the transaction type  
            -- as it does not denote that actual purpose of the line, instead we "currently"  
            -- store reserve with a party_cnt of 0 so check that!!!  
            IF ISNULL(@ri_party_cnt, 0) = 0  
                SELECT  @out_transaction_ledger_code = 'NO',  
                        @out_nominal_ledger_code = 'NO',  
						@out_ri_party_cnt = NULL,  
						@prefix = ''  
            ELSE BEGIN  
				IF @transaction_type = 'C_CO' OR @transaction_type = 'C_CR'  
						SELECT @out_transaction_ledger_code = 'NO',  
							   @out_nominal_ledger_code = 'NO',  
							   @out_ri_party_cnt = NULL,  
							   @prefix = 'CLMOSRI'  
				ELSE  
                 SELECT  @out_transaction_ledger_code = 'IN',  
                         @out_nominal_ledger_code = 'NO',  
						 @out_ri_party_cnt = @ri_party_cnt,  
						 @prefix = ''  
		END  
  
            -- Get the account type code  
            IF @out_transaction_ledger_code = 'NO'  
                IF @ri_stats_detail_type IN ('TTY', 'TAT')  
				   SELECT  @out_account_type_code = 'CLMOSRITQ'  
				ELSE IF @ri_stats_detail_type IN ('TFS')  
				   SELECT  @out_account_type_code = 'CLMOSRITS'  
                ELSE IF @ri_stats_detail_type IN ('XOL', 'TAL', 'FAX', 'TYX')  
                    SELECT  @out_account_type_code = 'CLMOSRIXL'  
                ELSE  
                    SELECT  @out_account_type_code = 'CLMOSRIOT'  
            ELSE  
                SELECT  @out_account_type_code = 'REINSACC'  
  
			IF @transaction_type = 'C_CO' OR @transaction_type = 'C_CR'  
				IF @ri_stats_detail_type IN ('TTY', 'TAT')  
				   SELECT  @transdetail_type_code = 'CLMRITQ'
				ELSE IF @ri_stats_detail_type IN ('FAX')  
				   SELECT  @transdetail_type_code = 'CLMRIFX'  
				ELSE IF @ri_stats_detail_type IN ('TYX')  
				   SELECT  @transdetail_type_code = 'CLMRITX'
				ELSE IF @ri_stats_detail_type IN ('FAC')
				   SELECT  @transdetail_type_code = 'CLMRIFAC'  
				ELSE IF @ri_stats_detail_type IN ('XOL', 'TAL')  
				   SELECT  @transdetail_type_code = 'CLMRITX'
				ELSE IF @ri_stats_detail_type IN ('TFS')  
				   SELECT  @transdetail_type_code = 'CLMRIFS'					   
				ELSE IF @ri_stats_detail_type IN ('TTC')  
				   SELECT  @transdetail_type_code = 'CLMRICAT'					   
				ELSE  
				   SELECT  @transdetail_type_code = 'CLMRITR'
			ELSE IF @transaction_type = 'C_RV' OR @transaction_type = 'C_SA'   
				 SELECT @transdetail_type_code = 'CLRRIREC' 
			ELSE IF @transaction_type = 'C_CP'  
				SELECT @transdetail_type_code = 'CLPRIPAY' 
				
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
                    @ri_premium_total,  
                    @out_transaction_ledger_code,  
                    @out_account_type_code,  
                    @ri_sum_insured_total,  
                    @prefix+@ri_shortname,  
                    @out_ri_party_cnt,  
                    @out_claim_ref,
                    @transdetail_type_code  
            FROM    Transaction_Export_Detail  
            WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
            -- To retrieve different class of business entries for each insurer.  
            -- If we have no party_cnt get by shortname  
            IF ISNULL(@ri_party_cnt, 0) = 0  
                DECLARE COB_Cursor CURSOR FAST_FORWARD FOR  
                    SELECT  d.class_of_business_code,  
                            isnull(sum(d.this_premium_original), 0),  
                            isnull(sum(d.sum_insured_home), 0),  
                            d.stats_detail_type  
                    FROM    stats_detail d  
                    WHERE   d.stats_folder_cnt = @stats_folder_cnt  
                    AND     d.stats_detail_type = @ri_stats_detail_type  
                    AND     d.ri_shortname = @ri_shortname  
                    AND     d.class_of_business_code is not null  
                    GROUP BY  
                            class_of_business_code, d.stats_detail_type  
            ELSE  
                DECLARE COB_Cursor CURSOR FAST_FORWARD FOR  
                    SELECT  d.class_of_business_code,  
                            isnull(sum(d.this_premium_original), 0),  
                            isnull(sum(d.sum_insured_home), 0),  
                            d.stats_detail_type  
                    FROM    stats_detail d  
                    WHERE   d.stats_folder_cnt = @stats_folder_cnt  
                    AND     d.stats_detail_type = @ri_stats_detail_type  
                    AND     d.ri_party_cnt = @ri_party_cnt  
                    AND     d.ri_shortname = @ri_shortname  
                    AND     d.class_of_business_code is not null  
                    GROUP BY  
                            class_of_business_code, d.stats_detail_type  
  
            -- Open the cursor and get first row  
            OPEN COB_Cursor  
            FETCH NEXT FROM COB_Cursor  
                INTO    @cob_class_of_business_code,  
                        @cob_premium_total,  
                        @cob_sum_insured_total,  
                        @cob_ri_stats_detail_type  
  
            WHILE (@@FETCH_STATUS = 0)  
            BEGIN  
                -- Set base codes, check for party_cnt.  
                -- If we have one this is a payment, else a reserve amendment  
                IF @transaction_type IN ('C_CO','C_CR','C_SA','C_RV')  
                    SELECT  @out_account_type_code = 'CLMRI',  
                            @out_nominal_ledger_account = 'CLMRI'  
                ELSE IF ISNULL(@ri_party_cnt, 0) = 0  
                    SELECT  @out_account_type_code = 'CLMRI',  
                            @out_nominal_ledger_account = 'CLMRI'  
                ELSE  
                    SELECT  @out_account_type_code = 'CLMOSRI',  
                            @out_nominal_ledger_account = 'CLMOSRI'  
  
                -- Modify codes for stats type  
                IF @cob_ri_stats_detail_type IN ('TTY', 'TAT','TFS')  
                BEGIN  
  --V0.3  
   SELECT @reinsurancecode = UPPER(LTRIM(RTRIM(rt.code)))  
   FROM party_insurer ptyin  
   INNER JOIN reinsurance_type rt ON ptyin.reinsurance_type=rt.reinsurance_type_id  
   WHERE rt.is_deleted =0 AND ptyin.party_cnt = @ri_party_cnt  
  
    IF ISNULL(LTRIM(RTRIM(@reinsurancecode)),'')= 'QUO'  
    BEGIN  
     SELECT  @out_account_type_code = @out_account_type_code + 'TQ',  
            @out_nominal_ledger_account = @out_nominal_ledger_account + 'TQ'  
    END  
  
    ELSE IF (ISNULL(LTRIM(RTRIM(@reinsurancecode)),'')= '001' OR ISNULL(LTRIM(RTRIM(@reinsurancecode)),'')= '002' OR ISNULL(LTRIM(RTRIM(@reinsurancecode)),'')= '003')  
    BEGIN  
     SELECT  @out_account_type_code = @out_account_type_code + 'TS',  
            @out_nominal_ledger_account = @out_nominal_ledger_account + 'TS'  
    END  
  
    ELSE  
    BEGIN  
     SELECT  @out_account_type_code = @out_account_type_code + 'TR',  
            @out_nominal_ledger_account = @out_nominal_ledger_account + 'TR'  
    END  
  END  
  
                ELSE IF @cob_ri_stats_detail_type IN ('FAX')  
                   SELECT  @out_account_type_code = @out_account_type_code + 'FX',  
                           @out_nominal_ledger_account = @out_nominal_ledger_account + 'FX'  
                ELSE IF @cob_ri_stats_detail_type IN ('TYX')  
                   SELECT  @out_account_type_code = @out_account_type_code + 'TX',  
                           @out_nominal_ledger_account = @out_nominal_ledger_account + 'TX'  
                
                ELSE IF @cob_ri_stats_detail_type IN ('XOL', 'TAL')  
                   SELECT  @out_account_type_code = @out_account_type_code + 'XL',  
                           @out_nominal_ledger_account = @out_nominal_ledger_account + 'XL'  
                ELSE  
                   SELECT  @out_account_type_code = @out_account_type_code + 'OT',  
                           @out_nominal_ledger_account = @out_nominal_ledger_account + 'OT'  
  
                -- Append account suffix and cob code (to ledger)  
                SELECT  @out_account_type_code = @out_account_type_code + @account_suffix,  
                        @out_nominal_ledger_account = @out_nominal_ledger_account + @account_suffix +  @cob_class_of_business_code  
  
                -- If we are posting taxes for salvage and recovery override the posting account  
                IF @transaction_type IN ('C_SA', 'C_RV') AND @cob_ri_stats_detail_type IN ('TAT', 'TAF')  
                    SELECT  @out_account_type_code = 'CLMREC',  
                            @out_nominal_ledger_account = 'CLMRECEIVABLE'  
  
     			IF @transaction_type = 'C_CO' OR @transaction_type = 'C_CR'  
					IF @cob_ri_stats_detail_type IN ('TTY', 'TAT')  
					   SELECT  @transdetail_type_code = 'CLMRITQ'
					ELSE IF @cob_ri_stats_detail_type IN ('FAX')  
					   SELECT  @transdetail_type_code = 'CLMRIFX'  
					ELSE IF @cob_ri_stats_detail_type IN ('TYX')  
					   SELECT  @transdetail_type_code = 'CLMRITX'
					ELSE IF @cob_ri_stats_detail_type IN ('FAC')
					   SELECT  @transdetail_type_code = 'CLMRIFAC'  
					ELSE IF @cob_ri_stats_detail_type IN ('XOL', 'TAL')  
					   SELECT  @transdetail_type_code = 'CLMRITX'
					ELSE IF @cob_ri_stats_detail_type IN ('TFS')  
					   SELECT  @transdetail_type_code = 'CLMRIFS'					   
					ELSE IF @cob_ri_stats_detail_type IN ('TTC')  
					   SELECT  @transdetail_type_code = 'CLMRICAT'					   
					ELSE  
					   SELECT  @transdetail_type_code = 'CLMRITR'
				ELSE IF @transaction_type = 'C_RV' OR @transaction_type = 'C_SA'   
					IF @cob_ri_stats_detail_type IN ('TTY', 'TAT')  
					   SELECT  @transdetail_type_code = 'CLRRITQ'
					ELSE IF @cob_ri_stats_detail_type IN ('FAC')  
					   SELECT  @transdetail_type_code = 'CLRRIFAC'  
					ELSE IF @cob_ri_stats_detail_type IN ('FAX')  
					   SELECT  @transdetail_type_code = 'CLRRIFX'  					   
					ELSE IF @cob_ri_stats_detail_type IN ('TYX')  
					   SELECT  @transdetail_type_code = 'CLRRITX'
					ELSE IF @cob_ri_stats_detail_type IN ('XOL', 'TAL')  
					   SELECT  @transdetail_type_code = 'CLRRITX'
					ELSE IF @cob_ri_stats_detail_type IN ('TFS')  
					   SELECT  @transdetail_type_code = 'CLRRIFS'					   
					ELSE IF @cob_ri_stats_detail_type IN ('TTC')  
					   SELECT  @transdetail_type_code = 'CLRRICAT'					   
					ELSE  
					   SELECT  @transdetail_type_code = 'CLRRITR'
				ELSE IF @transaction_type = 'C_CP'  
					IF @cob_ri_stats_detail_type IN ('TTY', 'TAT')  
					   SELECT  @transdetail_type_code = 'CLPRITQ'
					ELSE IF @cob_ri_stats_detail_type IN ('FAC')
					   SELECT  @transdetail_type_code = 'CLPRIFAC' 					   
					ELSE IF @cob_ri_stats_detail_type IN ('FAX')  
					   SELECT  @transdetail_type_code = 'CLPRIFX'  
					ELSE IF @cob_ri_stats_detail_type IN ('TYX')  
					   SELECT  @transdetail_type_code = 'CLPRITX'
					ELSE IF @cob_ri_stats_detail_type IN ('XOL', 'TAL')  
					   SELECT  @transdetail_type_code = 'CLPRITX'
					ELSE IF @cob_ri_stats_detail_type IN ('TFS')  
					   SELECT  @transdetail_type_code = 'CLPRIFS'					   
					ELSE IF @cob_ri_stats_detail_type IN ('TTC')  
					   SELECT  @transdetail_type_code = 'CLPRICAT'					   
					ELSE  
					   SELECT  @transdetail_type_code = 'CLPRITR' 
				ELSE
					SELECT @transdetail_type_code = NULL
					
                -- Insert output row  
                INSERT INTO Transaction_Export_Detail  
                       (transaction_export_folder_cnt,  
                        transaction_export_detail_id,  
                        transaction_amount,  
                        transaction_ledger_code,  
                        account_type_code,  
                        sum_insured_total,  
                        mapping_Code,  
                        spare,
                        transdetail_type_code)  
                SELECT  @transaction_export_folder_cnt,  
                        ISNULL(MAX(transaction_export_detail_id), 0) + 1,  
                        -@cob_premium_total,  
                        @out_nominal_ledger_code,  
                        @out_account_type_code,  
                        -@cob_sum_insured_total,  
                        @out_nominal_ledger_account,  
                        @out_claim_ref,
                        @transdetail_type_code  
                FROM    Transaction_Export_Detail  
                WHERE   transaction_export_folder_cnt = @transaction_export_folder_cnt  
  
                -- Get next row  
                FETCH NEXT FROM COB_Cursor  
                    INTO    @cob_class_of_business_code,  
                            @cob_premium_total,  
                            @cob_sum_insured_total,  
                            @cob_ri_stats_detail_type  
            END  
  
            -- Shutdown the cursor  
            CLOSE COB_Cursor  
            DEALLOCATE COB_Cursor  
        END  
  
        -- Get next row  
        FETCH NEXT FROM RI_Cursor  
            INTO    @ri_party_cnt,  
                    @ri_shortname,  
                    @ri_share_percent,  
                    @ri_premium_total,  
                    @ri_sum_insured_total,  
                    @ri_stats_detail_type  
    END  
  
    -- Shutdown the cursor  
    CLOSE RI_Cursor  
    DEALLOCATE RI_Cursor  

GO

