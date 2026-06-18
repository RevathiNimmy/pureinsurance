
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Report_Paid_Bordereau_Detail_SFU'
GO

CREATE PROCEDURE [spu_Report_Paid_Bordereau_Detail_SFU]
        @branch_id  int,
        @AgentShortName varchar (40),
        @end_date VARCHAR (20), 
        @TypeOfCurrency Varchar(40),
        @CurrencyName Varchar(40)
AS
SET NOCOUNT ON
	CREATE TABLE #tempAgentStat
		(
			 Row_count              INT IDENTITY,
			 CertificateYear        VARCHAR(20) NULL,
			 PolicySectionName      VARCHAR(255) NULL,
			 AgentResolvedName      VARCHAR(255) NULL,
			 TaxPercentage          DECIMAL(19, 4) NULL,
			 BorderauxCurrencyDesc  VARCHAR(255) NULL,
			 CertificatePreFix      VARCHAR(255) NULL,
			 InsuranceRef           VARCHAR(30) NULL,
			 InsuredName            VARCHAR(255) NULL,
			 PolicyType             VARCHAR(30) NULL,
			 InceptionDate          DATETIME NULL,
			 CurrencyDesc           VARCHAR(255) NULL,
			 GrossPremium           DECIMAL(19, 4) NULL,
			 TaxAmount              DECIMAL(19, 4) NULL,
			 TotalCommissionAmount  DECIMAL(19, 4) NULL,
			 TotalCommissionPercent DECIMAL(19, 4) NULL,
			 DueToUWEXTax           DECIMAL(19, 4) NULL,
			 DueToUWINTax           DECIMAL(19, 4) NULL,
			 BranchName             VARCHAR(30) NULL,
			 DataModelCode          VARCHAR(30) NULL,
			 GisPolicyLinkId        INT NULL,
			 TypeOfCurrency         VARCHAR(40) NULL,
			 Stats_folder_cnt       INT NULL,
			 Account_key            INT NULL,
			 Insurance_file_cnt     INT NULL,
			 Document_id            INT NULL,
			 Currency_base_xrate FLOAT NULL,
			 Account_base_XRate  FLOAT NULL, 
			 System_Base_XRate   FLOAT NULL,
			-- Class_of_Business_code VARCHAR(10)NULL,
		)
		
		
		
DECLARE @Declaration VARCHAR(500)
DECLARE @Query VARCHAR(MAX)
DECLARE @SELECT VARCHAR(MAX)
DECLARE @WHERE VARCHAR(MAX)
DECLARE @Group VARCHAR(100)


DECLARE @dtSelectedPeriodEnd datetime, @SelectedPeriodID int,@dtPriorSelectedPeriodEnd datetime, @iBasis int    

SELECT @end_date = @end_date + " 23:59:59.000"  

SELECT @dtSelectedPeriodEnd = CONVERT (Datetime, @end_date) 

 
    SELECT @iBasis = 1    -- Transaction Period  
  
     
    SELECT @SelectedPeriodID = MAX(period_id)
    FROM Period  
    WHERE period_end_date <= @dtSelectedPeriodEnd 
    AND  company_id = 1



SELECT @Declaration ='DECLARE @TypeOfCurrency VARCHAR(40), @CurrencyName   VARCHAR(40) ' 
SELECT  @Declaration = @Declaration + ' SET @TypeOfCurrency =''' + @TypeOfCurrency + ''''
SELECT  @Declaration = @Declaration + ' SET @CurrencyName =''' + @CurrencyName + ''''

EXEC DDLADDINDEX '#tempAgentStat','Document_id'

SELECT @Query = " INSERT INTO #tempAgentStat
				(CertificateYear,
				 PolicySectionName,
				 AgentResolvedName,
				 TaxPercentage,
				 BorderauxCurrencyDesc,
				 CertificatePreFix,
				 InsuranceRef,
				 InsuredName,
				 PolicyType,
				 InceptionDate,
				 CurrencyDesc,
				 GrossPremium,
				 TaxAmount,
				 TotalCommissionAmount,
				 TotalCommissionPercent,
				 DueToUWEXTax,
				 DueToUWINTax,
				 BranchName,
				 DataModelCode,
				 GisPolicyLinkId,
				 TypeOfCurrency,
				 Stats_folder_cnt,
				 Account_key,
				 Insurance_file_cnt,
				 Document_id)" 
				 
				 
SELECT @SELECT ="

SELECT DISTINCT NULL                   CertificateYear,
	   NULL                   AS PolicySectionName,
	   PA.TRADING_NAME        AgentResolvedName,
	   NULL                   AS TaxPercentage,
	   CB.DESCRIPTION         AS BorderauxCurrencyDesc,
	   PR.DESCRIPTION         AS CertificatePreFix,
	   IFL.INSURANCE_REF      AS InsuranceRef,
	   IFL.INSURED_NAME       AS InsuredName,
	   (SELECT CASE Substring(d.document_ref, 1, 3)
				 WHEN 'SND' THEN 'IP'
				 WHEN 'SEC' THEN 'RP'
				 WHEN 'SED' THEN 'AP'
				 WHEN 'SRD' THEN 'RN'
			   END)           AS 'PolicyType',
	   IFL.cover_start_date   AS 'InceptionDate',
	   @CurrencyName        AS 'CurrencyDesc',
	   ((SELECT CASE @TypeOfCurrency
				  WHEN 'Base' THEN (SELECT SUM(Isnull(amount, 0))
									FROM   TransDetail TD_SUB
									WHERE  TD_SUB.document_id = d.document_id
										   AND spare IN ( 'GROSS', 'TAX', 'FEE' )
										   AND Isnull(amount, 0) > 0)
				  WHEN 'System' THEN (SELECT SUM(Isnull(system_amount, 0))
									  FROM   TransDetail TD_SUB
									  WHERE  TD_SUB.document_id = d.document_id
											 AND spare IN ( 'GROSS', 'TAX', 'FEE' )
											 AND Isnull(system_amount, 0) > 0)
				  WHEN 'Account' THEN (SELECT SUM(Isnull(account_amount, 0))
									   FROM   TransDetail TD_SUB
									   WHERE  TD_SUB.document_id = d.document_id
											  AND spare IN ( 'GROSS', 'TAX', 'FEE' )
											  AND Isnull(account_amount, 0) > 0)
				  WHEN 'Transaction' THEN (SELECT SUM(Isnull(currency_amount, 0))
										   FROM   TransDetail TD_SUB
										   WHERE  TD_SUB.document_id = d.document_id
												  AND spare IN ( 'GROSS', 'TAX', 'FEE' )
												  AND Isnull(currency_amount, 0) > 0)
				END))         'GrossPremium',
	   NULL                   AS TaxAmount,
	   NULL                   AS TotalCommissionAmount,
	   NULL                   AS TotalCommissionPercent,
	   NULL                   DueToUWEXTax,
	   NULL                   DueToUWINTax,
	   SB.CODE                BranchName,
	   DM.code                DataModelCode,
	   GPL.gis_policy_link_id GisPolicyLinkId,
	   @TypeOfCurrency      AS TypeOfCurrency,
	   SF.Stats_folder_cnt AS Stats_folder_cnt,
	   A.Account_key,
	   IFL.Insurance_file_cnt,
	   D.Document_id
	  
FROM   Ledger L
JOIN   ACCOUNT A
	ON a.ledger_id = l.ledger_id
JOIN   transdetail td
	ON a.account_id = td.account_id
JOIN   Document d
	ON td.document_id = d.document_id
JOIN   Insurance_File IFL
	ON D.insurance_file_cnt = IFL.insurance_file_cnt
JOIN   insurance_file_risk_link IFRL
	ON IFL.insurance_file_cnt = IFRL.insurance_file_cnt
JOIN   Risk r
	ON R.risk_cnt = IFRL.risk_cnt
JOIN   Party_AGENT PA
	ON PA.party_cnt = A.account_key
JOIN Stats_Folder SF
	ON SF.document_ref = D.document_ref
LEFT JOIN Stats_Detail SD
	ON SF.stats_folder_cnt =SD.stats_folder_cnt
	
JOIN   gis_policy_link GPL
	ON gpl.risk_id = r.risk_cnt
JOIN   gis_data_model DM
	ON DM.gis_data_model_id = GPL.gis_data_model_id
JOIN   Sub_Branch SB
	ON SB.source_id = IFL.source_id
JOIN   Product PR
	ON PR.product_id = IFL.product_id
JOIN   Currency CB
	ON CB.currency_id = td.Currency_id"
	
	
SELECT @WHERE = " WHERE     l.ledger_name IN ( 'Commission', 'Sub Agent' )"

SELECT @WHERE = @WHERE + " AND  td.outstanding_currency_amount=0 AND td.spare NOT LIKE 'Revers%'"

IF @iBasis =1 
	    SELECT @WHERE = @WHERE +  ' AND TD.period_id = ' + CONVERT(VARCHAR,@SelectedPeriodID)
    
--IF @iBasis =0
	--SELECT @WHERE = @WHERE + ' AND Isnull(tef.cover_start_date, d.document_date) <= '''+  CONVERT(VARCHAR,@End_date) + ''''
--SELECT @WHERE = @WHERE + 'D.document_date <= ''' + @dtPriorSelectedPeriodEnd + ''''

IF @AgentShortName <> 'ALL'
	SELECT @WHERE = @WHERE + ' AND PA.Trading_name = ''' + @AgentShortName + ''''
	
IF @branch_id <> 0
	SELECT @WHERE = @WHERE + ' AND d.company_id = ''' + CONVERT(VARCHAR,@branch_id)  + ''''
IF @CurrencyName <> 'ALL'
	SELECT @WHERE = @WHERE + ' AND CB.DESCRIPTION = ''' + @CurrencyName + ''''

SELECT @Group = ' GROUP     BY IFL.insurance_ref, GPL.gis_policy_link_id '

SELECT @Query = @Declaration + @Query + @SELECT + @WHERE 


EXEC (@Query)

Update T 
SET T.CertificateYear  = (SELECT MAX(CertYearCode) FROM Party_Certificate_Year PCY WHERE PCY.Party_Cnt = T.account_KEY)
FROM #tempAgentStat T 

--Premium in Transaction currency
Update T 
SET T.GrossPremium  = (SELECT SUM(annual_premium) FROM Stats_detail SD  WHERE SD.stats_folder_cnt = T.Stats_folder_cnt and SD.stats_detail_type='GRS') 
FROM #tempAgentStat T 

Update T 
SET T.TaxAmount  = (SELECT SUM(tax_value) FROM Stats_detail SD  WHERE SD.stats_folder_cnt = T.Stats_folder_cnt and SD.stats_detail_type='TAG') 
FROM #tempAgentStat T 

--Get sub_agent and Commission in Transqaction Currency
Update T 
SET T.DueToUWEXTax  = (SELECT SUM(Commission_value) FROM Agent_Commission AC WHERE AC.insurance_file_cnt = T.Insurance_file_cnt 
and is_lead_agent = 0) 
FROM #tempAgentStat T 

Update T 
SET T.DueToUWINTax  = (SELECT SUM(Commission_value) + ISNULL(SUM(tax_amount),0)FROM Agent_Commission AC WHERE AC.insurance_file_cnt = T.Insurance_file_cnt 
and is_lead_agent = 0) 
FROM #tempAgentStat T 

Update T Set T.GrossPremium = T.GrossPremium + T.TaxAmount 
FROM  #tempAgentStat T 

Update T 
SET T.TaxPercentage  = T.TaxAmount / T.GrossPremium *100
FROM #tempAgentStat T 
Where  T.GrossPremium <> 0 


UPDATE T SET T.TotalCommissionAmount = T.GrossPremium - ISNULL(T.taxAmount,0)- ISNULL(T.DueToUWEXTax,0)
FROM #tempAgentStat T 

UPDATE T 
		SET T.TotalCommissionPercent = T.TotalCommissionAmount/(T.GrossPremium - ISNULL(T.taxAmount,0))* 100
		FROM #tempAgentStat T 
		WHERE (T.GrossPremium - ISNULL(T.taxAmount,0)) <> 0 
		
UPDATE T SET T.Account_base_XRate = TD.Account_base_XRate, T.Currency_base_xrate = TD.currency_base_xrate , T.System_Base_XRate = TD.system_base_xrate
 FROM #tempAgentStat T 
JOIN TransDetail TD ON T.Document_id = TD.document_id
SET NOCOUNT OFF

SELECT       Row_count ,             
			 CertificateYear,        
			 PolicySectionName,     
			 AgentResolvedName,
			 TaxPercentage,
			 BorderauxCurrencyDesc,
			 CertificatePreFix,
			 InsuranceRef ,
			 InsuredName    ,
			 PolicyType  ,
			 InceptionDate ,
			 CurrencyDesc ,
(SELECT CASE @TypeOfCurrency
			WHEN 'Base' THEN GrossPremium * Currency_base_xrate
			WHEN 'Account' THEN (GrossPremium * Currency_base_xrate)/Account_base_XRate
			WHEN 'Transaction' THEN GrossPremium
			WHEN 'System' THEN  (GrossPremium * Currency_base_xrate)/System_base_XRate
			end)GrossPremium,

(SELECT CASE @TypeOfCurrency
			WHEN 'Base' THEN TaxAmount * Currency_base_xrate
			WHEN 'Account' THEN (TaxAmount * Currency_base_xrate)/Account_base_XRate
			WHEN 'Transaction' THEN TaxAmount
			WHEN 'System' THEN  (TaxAmount * Currency_base_xrate)/System_base_XRate
			end)TaxAmount,
(SELECT CASE @TypeOfCurrency
			WHEN 'Base' THEN TotalCommissionAmount * Currency_base_xrate
			WHEN 'Account' THEN (TotalCommissionAmount * Currency_base_xrate)/Account_base_XRate
			WHEN 'Transaction' THEN TotalCommissionAmount
			WHEN 'System' THEN  (TotalCommissionAmount * Currency_base_xrate)/System_base_XRate
			end)TotalCommissionAmount,						 
			 TotalCommissionPercent ,

(SELECT CASE @TypeOfCurrency
			WHEN 'Base' THEN DueToUWEXTax * Currency_base_xrate
			WHEN 'Account' THEN (DueToUWEXTax * Currency_base_xrate)/Account_base_XRate
			WHEN 'Transaction' THEN DueToUWEXTax
			WHEN 'System' THEN  (DueToUWEXTax * Currency_base_xrate)/System_base_XRate
			end)DueToUWEXTax,
						 


(SELECT CASE @TypeOfCurrency
			WHEN 'Base' THEN DueToUWINTax * Currency_base_xrate
			WHEN 'Account' THEN (DueToUWINTax * Currency_base_xrate)/Account_base_XRate
			WHEN 'Transaction' THEN DueToUWINTax
			WHEN 'System' THEN  DueToUWINTax
			end)DueToUWINTax,			 
			 BranchName,
			 DataModelCode,
			 GisPolicyLinkId,
			 TypeOfCurrency,
			 Stats_folder_cnt,
			 Account_key,
			 Insurance_file_cnt,
			 Document_id
FROM   #tempAgentStat WHERE GrossPremium <>0 
order by document_id
 

	DROP TABLE #tempAgentStat

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 


