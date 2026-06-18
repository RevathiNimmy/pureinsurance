SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure [spu_Report_Paid_Bordereau_Detailed_SFU]
GO

CREATE PROCEDURE [spu_Report_Paid_Bordereau_Detailed_SFU]
        @branch_id  int,      
        @AgentShortName varchar (40),      
        @End_date   datetime,      
        @TypeOfCurrency Varchar(40),     
        @CurrencyName Varchar(40)
AS  
  
  
  CREATE TABLE #tempAgentStat        
(        
 Row_count    INT IDENTITY,      
 CertificateYear   Varchar(20)  NULL,      
 PolicySectionName  Varchar(255)  NULL,      
 AgentResolvedName       varchar(255)    NULL,        
 TaxPercentage    decimal(19,4)   NULL,       
 BorderauxCurrencyDesc            Varchar(255)    NULL,        
 CertificatePreFix Varchar(255) NULL,      
 InsuranceRef            varchar(30)     NULL,        
 InsuredName             varchar(255)    NULL,       
 PolicyType               varchar(30)     NULL,        
 InceptionDate                 datetime        NULL,   
 CurrencyDesc            Varchar(255)    NULL     ,       
 GrossPremium             decimal(19,4)   NULL,        
 TaxAmount decimal(19,4)     NULL,       
 TotalCommissionAmount        decimal(19,4)   NULL,        
 TotalCommissionPercent       decimal(19,4)   NULL,        
 DueToUWEXTax decimal(19,4)   NULL,       
 DueToUWINTax decimal(19,4)   NULL,   
 BranchName Varchar(30) NULL,       
 DataModelCode Varchar(30) NULL,      
 GisPolicyLinkId INT NULL,      
 TypeOfCurrency Varchar(40) null,      
  )    
  INSERT INTO   #tempAgentStat  
  (  
 CertificateYear,  
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
 TypeOfCurrency  
  )         
  Select      
   MAX(PCY.CertYearCode) 'CertificateYear' ,      
   MAX(PT.description) 'PolicySectionName',       
   MAX(PA.TRADING_NAME) 'AgentResolvedName',       
   MAX(TC.PERCENTAGE) 'TaxPercentage',      
   MAX(CB.DESCRIPTION) 'BorderauxCurrencyDesc',      
   MAX(PR.DESCRIPTION) 'CertificatePreFix',      
   IFL.INSURANCE_REF 'InsuranceRef',         
   MAX(IFL.INSURED_NAME) 'InsuredName',       
   (SELECT Case MAX(SUBSTRING(d.document_ref,1,3)) 
   WHEN 'SND' THEN 'IP'
   WHEN 'SEC' THEN 'RP'
   WHEN 'SED' THEN 'AP'
   WHEN 'SRD' THEN 'RN'
   END ) 'PolicyType',        
   MAX(IFL.cover_start_date)  'InceptionDate',        
   MAX(@CurrencyName) 'CurrencyDesc',  
 ((SELECT        
    Case  @TypeOfCurrency       
    When 'Base' Then (SELECT SUM(ISNULL(amount,0)) FROM TransDetail TD_SUB WHERE TD_SUB.document_id = MAX(d.document_id) AND spare IN ('GROSS','TAX','FEE') and ISNULL(amount,0) > 0 )
    When 'System' Then (SELECT SUM(ISNULL(system_amount,0)) FROM TransDetail TD_SUB WHERE TD_SUB.document_id = MAX(d.document_id) AND spare IN ('GROSS','TAX','FEE') AND ISNULL(system_amount,0) > 0 )
    When  'Account' Then (SELECT SUM(ISNULL(account_amount,0)) FROM TransDetail TD_SUB WHERE TD_SUB.document_id = MAX(d.document_id) AND spare IN ('GROSS','TAX','FEE') AND ISNULL(account_amount,0) > 0 )  
    WHEN  'Transaction' THEN (SELECT SUM(ISNULL(currency_amount,0)) FROM TransDetail TD_SUB WHERE TD_SUB.document_id = MAX(d.document_id) AND spare IN ('GROSS','TAX','FEE')AND ISNULL(currency_amount,0) > 0 )  
    END)  )'GrossPremium',  
 MAX(ISNULL(Sd.tax_value,0))  'TaxAmount' ,  
 ( (SELECT        
    Case  @TypeOfCurrency       
    When 'Base' Then (SELECT SUM(ISNULL(SD_SUB.this_premium_home,0) - ISNULL(SD_SUB.sub_commission_value_home,0)) FROM Stats_Detail SD_SUB   WHERE SD_SUB.stats_folder_cnt = MAX(SD.stats_folder_cnt) AND SD_SUB.stats_detail_type = 'SUB')
    When 'System' Then (SELECT SUM(ISNULL(SD_SUB.this_premium_system,0) - ISNULL(SD_SUB.sub_commission_value_system,0)) FROM Stats_Detail SD_SUB   WHERE SD_SUB.stats_folder_cnt = MAX(SD.stats_folder_cnt) AND SD_SUB.stats_detail_type = 'SUB')
    When  'Account' Then (SELECT SUM(ISNULL(SD_SUB.this_premium_home,0) - ISNULL(SD_SUB.sub_commission_value_home,0)) FROM Stats_Detail SD_SUB   WHERE SD_SUB.stats_folder_cnt = MAX(SD.stats_folder_cnt) AND SD_SUB.stats_detail_type = 'SUB')
    WHEN  'Transaction' THEN (SELECT SUM(ISNULL(SD_SUB.this_premium_home,0) - ISNULL(SD_SUB.sub_commission_value_home,0)) FROM Stats_Detail SD_SUB   WHERE SD_SUB.stats_folder_cnt = MAX(SD.stats_folder_cnt) AND SD_SUB.stats_detail_type = 'SUB')
    END )         
  )  'TotalCommissionAmount' ,     
 (

SELECT ROUND(
			 (
				SELECT (100 *COUNT(stats_folder_cnt)) 
				- 
				SUM(100 *
					(	CASE ISNULL(SD_SUB.this_premium_home,0) WHEN 0 THEN 0
						ELSE ROUND(ISNULL(SD_SUB.sub_commission_value_home,0),2) 
								/ 
							( ROUND(ISNULL(SD_SUB.this_premium_home,0),2))
						END
					) 
				   )
				FROM Stats_Detail SD_SUB 
				WHERE 
				SD_SUB.stats_detail_type = ('SUB')
				AND SD_SUB.stats_folder_cnt = MAX(SD.stats_folder_cnt)
			)
		 ,2)		   
)  'TotalCommissionPercent'  ,   
 NULL 'DueToUWEXTax' ,      
 NULL 'DueToUWINTax' ,  
 MAX(SB.CODE) 'BranchName' ,  
 MAX(DM.code)  'DataModelCode' ,  
 GPL.gis_policy_link_id  'GisPolicyLinkId' ,  
 MAX(@TypeOfCurrency) 'TypeOfCurrency'  
   FROM    Account a          
    JOIN   Ledger l       ON a.ledger_id = l.ledger_id          
    JOIN   transdetail td     ON a.account_id = td.account_id          
    JOIN   Document d      ON td.document_id = d.document_id          
    JOIN   Currency CB      ON CB.currency_id =td.Currency_id    
    LEFT JOIN transaction_export_folder tef ON tef.document_ref = d.document_ref      
 JOIN Party_Certificate_Year PCY   ON a.account_kEY = PCY.Party_Cnt         
    JOIN Insurance_File IFL     ON ifl.insurance_ref =td.insurance_ref         
    JOIN insurance_file_risk_link IFRL  ON IFL.insurance_file_cnt =IFRL.insurance_file_cnt         
    JOIN Risk r        ON R.risk_cnt =IFRL.risk_cnt         
    JOIN Peril P       ON P.risk_cnt =R.risk_cnt         
    JOIN Peril_Type PT      ON PT.peril_type_id =P.peril_type_id          
    JOIN Product PR       ON PR.product_id =IFL.product_id         
    JOIN Tax_Calculation TC     ON TC.risk_cnt =R.risk_cnt         
    JOIN Stats_Detail SD     ON SD.risk_id  =R.risk_cnt         
    JOIN Party_AGENT PA      ON PA.party_cnt = A.account_key        
    JOIN gis_policy_link GPL    ON gpl.risk_id=r.risk_cnt        
    JOIN gis_data_model DM     ON DM.gis_data_model_id=GPL.gis_data_model_id        
    JOIN Transaction_Type tt    ON td.transdetail_type_id  =tt.transaction_type_id     
    JOIN Sub_Branch SB      ON SB.source_id =IFL.source_id   
    WHERE   l.ledger_name IN ('Commission','Sub Agent') AND        
            ( d.document_date <= @End_date )        
              AND ((a.short_code = @AgentShortName)        
              OR (@AgentShortName = 'ALL' ))        
            AND (@branch_id= 0 OR (@branch_id <> 0 AND d.company_id = @branch_id ))   
            AND (@CurrencyName = 'ALL' OR CB.description =@CurrencyName)     
            AND  td.outstanding_currency_amount=0 AND td.spare NOT LIKE 'Revers%'     
            GROUP BY IFL.insurance_ref,GPL.gis_policy_link_id  
              
 DECLARE @intFlag INT         
 DECLARE @LBOUND INT        
 DECLARE @DM_CODE VARCHAR(30)        
 DECLARE @UpdateSQL VARCHAR(2000)        
 DECLARE @SELECTSQL NVARCHAR(3000)        
 DECLARE @GPL_ID INT        
 DECLARE @DueToUWEXTax decimal(19,4)   
 DECLARE @nPremium nvarchar(max)   
     
 SELECT @LBOUND=1        
 SELECT @intFlag =COUNT(*) FROM #tempAgentStat        
 WHILE (@LBOUND <=@intFlag)         
 BEGIN         
   
  SELECT @DM_CODE=DataModelCode ,@GPL_ID=GisPolicyLinkId FROM #tempAgentStat WHERE Row_count=@LBOUND        
  SELECT @DM_CODE  = RTRIM(@DM_CODE)  
     
     
  SELECT @SELECTSQL='SELECT @nPremium = sum(Premium) FROM ' + @DM_CODE +'_OUTPUT DMO JOIN ' + @DM_CODE + '_policy_binder DPB ON ' + ' DPB.' + @DM_CODE +'_policy_binder_ID=DMO.' + @DM_CODE +'_policy_binder_ID WHERE DPB.GIS_POLICY_LINK_ID=' + CONVERT(varchar,@GPL_ID)  
  EXEC sp_executesql @SELECTSQL,N'@nPremium nvarchar(max) output', @nPremium output     
     
  set @DueToUWEXTax = CONVERT(decimal(19,4),ISNULL(@nPremium,0))  
  UPDATE #tempAgentStat set DueToUWEXTax= ISNULL(@DueToUWEXTax,0),DueToUWINTax=ISNULL(@DueToUWEXTax,0) + TaxAmount WHERE Row_count =@LBOUND        
  SET @LBOUND = @LBOUND + 1         
 END      
    
SELECT * FROM  #tempAgentStat  
DROP TABLE #tempAgentStat    
  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


