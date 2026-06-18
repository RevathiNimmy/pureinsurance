SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Report_Outstanding_Claims_Gross_To_Net_SFU'
GO


CREATE PROCEDURE spu_Report_Outstanding_Claims_Gross_To_Net_SFU
    @SalvageAndTPRecovery varchar(10),
    @end_date nvarchar(50),  
    @Catastrophe varchar(20),
    @branch_id int,
    @TypeOfCurrency varchar(30),
    @GroupByCode varchar(30)
AS
    /*
        Claim status id constants
        1 = Provisional Open Claim
        2 = Live Open Claim
        3 = Closed
        4 = ReOpen
        5 = ReClosed
    
        @SalvageAndTPRecovery:  Exclude - Exclude Salvage and Recovery
                                Include - All
                                Only    - Salvage and Recovery Only
                                
        @Catastrophe:     Include    -    include claims with catastrophe code
                          Exclude    -    exclude claims with catastrophe code
    */

	SELECT @end_date = CONVERT(DATETIME, @end_date, 103)
    DECLARE 
        @SystemCurrencyCode varchar(10),
        @SystemCurrencyDesc varchar(255)


    -- Check parameters
    SELECT  @branch_id = IsNull(@branch_id, 0),
            @end_date = IsNull(@end_date, GetDate())

    
    -- Get System Currency Details
    SELECT  @SystemCurrencyCode = c.iso_code,
            @SystemCurrencyDesc = c.description
    FROM    PMSystem pms
    JOIN    currency c ON c.currency_id = pms.currency_id
    WHERE   pms.system_id = 1


    -- Return report data directly
    SELECT  sf.loss_code AS ClaimNum,
            sf.insurance_ref AS PolNum,
            sf.insurance_holder_shortname AS Client,
            sf.document_ref AS DocRef,
            sf.agent_shortname AS Agency,
            sf.product_code AS ProductCode,
            p.description AS ProductDesc,
            sd.risk_type_code AS RiskTypeCode,
            rt.description AS RiskTypeDescription,
            --Gross (negative for Claim Payments, positive for others)
           (SELECT  CASE @TypeOfCurrency 
                        WHEN 'Base' THEN sd.this_premium_home 
                        WHEN 'system' THEN sd.this_premium_system
                        WHEN 'Transaction' THEN 
						(SELECT CASE WHEN sd.currency_rate<>(SELECT TOP 1 currency_rate FROM Stats_Detail WHERE stats_detail_type = 'NET' AND stats_folder_cnt = sf.stats_folder_cnt)
                        THEN (SELECT TOP 1 this_premium_original FROM Stats_Detail WHERE stats_detail_type = 'NET' AND stats_folder_cnt = sf.stats_folder_cnt)/sd.currency_rate 
                        ELSE sd.this_premium_original END)
			WHEN 'Account' THEN (SELECT MIN(T.account_base_xrate)
						FROM TransDetail T
						JOIN Document D ON d.document_id=t.document_id
						WHERE D.insurance_file_cnt = i.insurance_file_cnt
						AND T.account_id = a.account_id)* sd.this_premium_home 
                    END
            WHERE   sd.stats_detail_type = 'GRS') * 
                    CASE WHEN sf.transaction_type_code = 'C_CP' THEN -1 ELSE 1 END AS Gross,
            --Coinsurance (negative for Claim Payments, positive for others)
           (SELECT  CASE @TypeOfCurrency 
                        WHEN 'Base' THEN  sd.this_premium_home 
                        WHEN 'System' THEN sd.this_premium_system 
                        WHEN 'Transaction' THEN sd.this_premium_original
			WHEN 'Account' THEN (SELECT MIN(T.account_base_xrate)
						FROM TransDetail T
						JOIN Document D ON d.document_id=t.document_id
						WHERE D.insurance_file_cnt = i.insurance_file_cnt
						AND T.account_id = a.account_id)* sd.this_premium_home
                    END
            WHERE   sd.stats_detail_type = 'COI') *
                    CASE WHEN sf.transaction_type_code = 'C_SA' THEN -1 ELSE 1 END * 
                    CASE WHEN sf.transaction_type_code = 'C_CP' THEN -1 ELSE 1 END As Coinsurance,
            --Treaty (Leave for all)
           (SELECT  CASE @TypeOfCurrency 
                        WHEN 'Base' THEN  sd.this_premium_home 
                        WHEN 'System' THEN sd.this_premium_system 
                        WHEN 'Transaction' THEN sd.this_premium_original
			WHEN 'Account' THEN (SELECT MIN(T.account_base_xrate)
						FROM TransDetail T
						JOIN Document D ON d.document_id=t.document_id
						WHERE D.insurance_file_cnt = i.insurance_file_cnt
						AND T.account_id = a.account_id)* sd.this_premium_home			
                    END 
            WHERE   sd.stats_detail_type = 'TTY' OR sd.stats_detail_type = 'TFS') *
                    CASE WHEN sf.transaction_type_code = 'C_SA' THEN -1 ELSE 1 END * 
                    CASE WHEN sf.transaction_type_code = 'C_CP' AND sd.ri_party_cnt <> 0 AND sd.stats_version >= 13 THEN -1 ELSE 1 END AS Treaty,
            --FAC (Leave for all)
           (SELECT  CASE @TypeOfCurrency 
                        WHEN 'Base' THEN  sd.this_premium_home 
                        WHEN 'System' THEN sd.this_premium_system 
                        WHEN 'Transaction' THEN sd.this_premium_original
			WHEN 'Account' THEN (SELECT MIN(T.account_base_xrate)
						FROM TransDetail T
						JOIN Document D ON d.document_id=t.document_id
						WHERE D.insurance_file_cnt = i.insurance_file_cnt
						AND T.account_id = a.account_id)* sd.this_premium_home			
                    END 
            WHERE   sd.stats_detail_type = 'FAC') *
                    CASE WHEN sf.transaction_type_code = 'C_SA' THEN -1 ELSE 1 END * 
                    CASE WHEN sf.transaction_type_code = 'C_CP' AND sd.ri_party_cnt <> 0 AND sd.stats_version >= 13 THEN -1 ELSE 1 END AS Facultative,
            --XOL (Leave for all)
           (SELECT  CASE @TypeOfCurrency 
                        WHEN 'Base' THEN  sd.this_premium_home 
                        WHEN 'System' THEN sd.this_premium_system 
                        WHEN 'Transaction' THEN sd.this_premium_original
			WHEN 'Account' THEN (SELECT MIN(T.account_base_xrate)
						FROM TransDetail T
						JOIN Document D ON d.document_id=t.document_id
						WHERE D.insurance_file_cnt = i.insurance_file_cnt
						AND T.account_id = a.account_id)* sd.this_premium_home
                    END 
            WHERE   sd.stats_detail_type = 'XOL' or sd.stats_detail_type = 'TYX'  or sd.stats_detail_type = 'FAX' ) *
                    CASE WHEN sf.transaction_type_code = 'C_SA' THEN -1 ELSE 1 END *  
                    CASE WHEN sf.transaction_type_code = 'C_CP' AND sd.ri_party_cnt <> 0 THEN -1 ELSE 1 END AS XOL,
            --NET (Leave for all)
           (SELECT  CASE @TypeOfCurrency 
                        WHEN 'Base' THEN  sd.this_premium_home 
                        WHEN 'System' THEN sd.this_premium_system 
                        WHEN 'Transaction' THEN 
						 (SELECT CASE WHEN sd.currency_rate<>(SELECT TOP 1 currency_rate FROM Stats_Detail WHERE stats_detail_type = 'GRS' AND stats_folder_cnt = sf.stats_folder_cnt)
                        THEN (SELECT TOP 1 this_premium_original FROM Stats_Detail WHERE stats_detail_type = 'GRS' AND stats_folder_cnt = sf.stats_folder_cnt)/sd.currency_rate 
                        ELSE sd.this_premium_original END)
			WHEN 'Account' THEN (SELECT MIN(T.account_base_xrate)
						FROM TransDetail T
						JOIN Document D ON d.document_id=t.document_id
						WHERE D.insurance_file_cnt = i.insurance_file_cnt
						AND T.account_id = a.account_id)* sd.this_premium_home
                    END 
            WHERE (sd.stats_detail_type = 'NET' OR sd.stats_detail_type = 'TR')) *		--PN: 72519
                    CASE WHEN sf.transaction_type_code = 'C_SA' THEN -1 ELSE 1 END *
                    CASE WHEN sf.transaction_type_code = 'C_CP' AND sd.ri_party_cnt <> 0 AND sd.stats_version >= 13 THEN -1 ELSE 1 END AS Retained,
            sf.transaction_date AS TransDate,
            sf.loss_date AS LossDate,
            sd.Currency_code AS Currency_Code,
            sf.branch_id AS SourceId,
            S.Code AS CompanyCode,
            S.Description AS CompanyDesc,
            Case @TypeOfCurrency 
                WHEN 'Base' THEN CB.Code
                WHEN 'System' THEN @SystemcurrencyCode
                WHEN 'Transaction' THEN CT.Code
		WHEN 'Account' THEN ca.Code
                ELSE ' '
            END AS CurrencyCode,
          tran_curr.description As CurencyDesc,
            Case @GroupByCode 
                WHEN 'Branch' THEN S.Code
                WHEN 'Branch And Currency' THEN S.Code
                WHEN 'Currency' THEN CT.Code
		
                ELSE ' '
            END AS GroupByCode
    FROM    Stats_Folder sf
    JOIN    Stats_Detail sd     ON sd.stats_folder_cnt = sf.stats_folder_cnt
    JOIN    claim c             ON c.claim_id = sf.loss_id

	--PN59825 Exclude all transactions having closed and re-closed claims
	INNER JOIN
	(SELECT clm1.base_claim_id, clm1.Claim_id, clm1.Claim_Status_id
		FROM  Claim clm1 INNER JOIN
		(SELECT base_claim_id, MAX(Claim_id) AS max_clm_id
			FROM Claim
			GROUP BY base_claim_id) clm2 ON clm1.Claim_id = clm2.max_clm_id
		) clm ON c.base_claim_id = clm.base_claim_id AND clm.Claim_Status_id NOT IN (3, 5)

    JOIN    Product p           ON p.product_id = sf.product_id
    JOIN    Risk_Type rt        ON rt.risk_type_id = sd.risk_type_id
    JOIN    Sub_Branch sb       ON sb.Sub_branch_id = sf.branch_id
    JOIN    Company s           ON s.company_id = sb.source_id
    JOIN    Currency cb         ON cb.Currency_id = s.Base_currency
    JOIN    Currency ct         ON ct.iso_code = sd.Currency_code

    JOIN    Insurance_File i	ON i.insurance_file_cnt=sf.insurance_file_cnt
    JOIN    Account a		ON a.account_key=i.insured_cnt
    JOIN    Currency ca         ON ca.currency_id =a.currency_id 
	JOIN 	Currency tran_curr with (nolock) ON sf.currency_code = tran_curr.iso_code
  WHERE ((@SalvageAndTPRecovery = 'exclude' AND sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP'))
        OR (@SalvageAndTPRecovery = 'only'    AND sf.transaction_type_code IN ('C_SA', 'C_RV'))
        OR (@SalvageAndTPRecovery = 'Include' AND sf.transaction_type_code IN ('C_CO', 'C_CR', 'C_CP', 'C_SA', 'C_RV')))
      AND     @end_date >= sf.Loss_date
    AND    (@Catastrophe <> 'exclude' OR IsNull(c.Catastrophe_code_id, 0) = 0)
    AND    (@branch_id = 0 OR sb.source_id = @branch_id)
    AND   ((@TypeOfCurrency = 'Base'        AND IsNull(sd.this_premium_home, 0) <> 0)
        OR (@TypeOfCurrency = 'System'      AND IsNull(sd.this_premium_system, 0) <> 0)
        OR (@TypeOfCurrency = 'Transaction' AND IsNull(sd.this_premium_original, 0) <> 0)
        OR (@TypeOfCurrency = 'Account'     AND IsNull(sd.this_premium_home, 0) <> 0))
		AND sf.document_ref<>'Doc Ref' 
		AND     EXISTS(select claim_id from claim where claim.is_dirty <> 1 AND claim_status_id NOT IN (3, 5) and claim_id in 
                      (select max(claim_id) from claim where claim_number = c.claim_number AND claim.is_dirty <> 1))
					  AND  c.is_dirty <> 1
    GO

