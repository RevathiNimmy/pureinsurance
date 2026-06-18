SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Insurance_File_Details'
GO

CREATE PROCEDURE spu_SIR_Get_Insurance_File_Details  
 @insurance_file_cnt int,  
 @original_insurance_file_cnt int = NULL ,
 @bIsSelectLivePlan TINYINT=0 

AS  
  
BEGIN  
  
 IF @original_insurance_file_cnt = 0  
 BEGIN  
  SET @original_insurance_file_cnt = NULL  
 END  
 SELECT * INTO #tempData FROM (
 SELECT  
  ifile.insurance_ref,  
  iFolder.inception_date,  
  ifile.cover_start_date,  
  ifile.expiry_date,  
  party.shortname AS PartyShortname,
  party.name AS PartyName,
  party.resolved_name AS PartyResolved_name,
  agent.shortname,  
  agent.name,  
  agent.resolved_name,  
  currency.description,  
  instalments.insurance_file_cnt,  
  business_type.code AS Business_typeCode,
  party_agent_type.code as Party_agent_Type_code,       
  ifa.sub_agent_count,  
  product.is_true_monthly_policy,  
  ifile.put_on_next_instalment_renewal,  
  ifile.discount_reason_id,  
  ifile.discount_percentage,  
  product.product_id,  
  ifile.discounted_premium,  
  ifile.match_discounted_premium_flag,  
  ifile.anniversary_date,  
  party.party_cnt,  
  ifolder.insurance_folder_cnt,  
  ifile.lead_agent_cnt,  
  agent_commission.total_net_premium,  
  agent_commission.total_agent_commission,  
  instalments.pfprem_finance_cnt,  
  instalments.pfprem_finance_version,  
  @original_insurance_file_cnt original_insurance_file_cnt, 
  ifile.anniversary_copy,  

  ifile.Source_id ,    
  ifile.Currency_id, 
  total_tax_amount,
  ifile.insurance_file_type_id,	
 
  product.allow_written_status,    
  ift.code,    
  product.written_task_manager_days,    
  product.written_rem_user_group,    
  product.written_rem_task_group,
  ifile.inception_date_tpi,
  ifile.CollectionFrequency_id,
  ifile.DOPaymentTerms_id,
  ifile.insurance_file_status_id ,
  ifile.is_marketplace_policy  ,
  ifile.currency_id AS TransCurrencyID,
  ifile.base_currency_id AS BaseCurrencyID,
  Currency.iso_code AS TransISOCode,
  c.iso_code AS BaseISOCode,
  ifile.Correspondence_Type,
  ifile.Default_Preferred_Correspondence,
  ifile.Is_Agent_Correspondence,
  ifile.discount_recurring_type_id
  
  
 FROM insurance_file ifile  
  
  INNER JOIN insurance_folder ifolder ON  
   ifolder.insurance_folder_cnt = ifile.insurance_folder_cnt  
  
  LEFT JOIN (SELECT Count(*) AS sub_agent_count, insurance_File_cnt FROM insurance_file_agent  
  
  GROUP BY insurance_file_cnt) ifa ON  
 ifile.insurance_file_cnt = ifa.insurance_file_cnt  
  
  LEFT JOIN party ON  
    ifolder.insurance_holder_cnt = party.party_cnt  
  
  LEFT JOIN party agent ON  
    ifile.lead_agent_cnt = agent.party_cnt  
  
  LEFT JOIN party_agent ON  
   agent.party_cnt = party_agent.party_cnt  
  
  LEFT JOIN (  
 SELECT  
  ISNULL(SUM(premium),0) total_net_premium,  
  ISNULL(SUM(commission_value),0) total_agent_commission, 
  ISNULL(SUM(tax_amount),0) total_tax_amount, 
  insurance_file_cnt  
 FROM agent_commission  
 GROUP BY insurance_file_cnt  
 ) agent_commission ON  
  
 agent_commission.insurance_file_cnt = ifile.insurance_file_cnt  
  
  LEFT JOIN party_agent_type ON  
  party_agent.party_agent_type_id = party_agent_type.party_agent_type_id  
  
  LEFT JOIN currency ON  
   ifile.currency_id = currency.currency_id  
  
   LEFT JOIN currency c ON
   ifile.base_currency_id = c.currency_id

  LEFT JOIN (select Top 1 PF.pfprem_finance_cnt, PF.pfprem_finance_version, PF.insurance_file_cnt, INS.insurance_ref  
            from pfpremiumfinance PF  
  INNER JOIN Insurance_file INS ON  
   PF.insurance_file_cnt = INS.insurance_file_cnt  
            where PF.statusind = '040'  
            and INS.insurance_file_cnt = IsNull(@original_insurance_file_cnt, @insurance_file_cnt)) instalments ON  
    ifile.insurance_ref= instalments.insurance_ref  
  
  LEFT JOIN business_type ON  
 ifile.business_type_id = business_type.business_type_id  
  
  LEFT JOIN Product ON  
 ifile.product_id =product.product_id  
     -- Written Status
 LEFT JOIN insurance_file_type ift ON    
 ifile.insurance_file_type_id = ift.insurance_file_type_id    
 --End -Written Status

 WHERE ifile.insurance_file_cnt = @insurance_file_cnt)  TempData
 
 IF @bIsSelectLivePlan=1
 BEGIN
    DECLARE @tblFinancePlan TABLE
    (
        DefaultPaymentMethod VARCHAR(20) ,
        DefaultInstalmentPlan INT ,
        DefaultInstalmentPlanVersion INT, 
        DefaultSchemeNumber INT ,
        DefaultSchemeVersion INT ,
        InstalmentInsuranceFileCnt INT,
        SavedPreferredDate DateTime,
        SavedDayInMonth INT,
		Frequency VARCHAR(10)  

    )
	INSERT INTO @tblFinancePlan
	EXEC spu_SAM_Get_Default_Payment_Terms @insurance_file_cnt
	
	UPDATE #tempData SET pfprem_finance_cnt=FP.DefaultInstalmentPlan,pfprem_finance_version=FP.DefaultInstalmentPlanVersion FROM @tblFinancePlan FP 
	WHERE  FP.DefaultInstalmentPlan<>0 AND FP.DefaultInstalmentPlanVersion<>0
	
 END

 SELECT 
 insurance_ref,  
  inception_date,  
  cover_start_date,  
  expiry_date,  
  PartyShortname AS shortname,  
  PartyName AS name,  
  PartyResolved_name AS resolved_name,  
  shortname,  
  name,  
  resolved_name,  
  description,  
  insurance_file_cnt,  
  Business_typeCode AS code,  
  Party_agent_Type_code as Party_agent_Type_code,       
  sub_agent_count,  
  is_true_monthly_policy,  
  put_on_next_instalment_renewal,  
  discount_reason_id,  
  discount_percentage,  
  product_id,  
  discounted_premium,  
  match_discounted_premium_flag,  
  anniversary_date,  
  party_cnt,  
  insurance_folder_cnt,  
  lead_agent_cnt,  
  total_net_premium,  
  total_agent_commission,  
  pfprem_finance_cnt,  
  pfprem_finance_version,  
  original_insurance_file_cnt, 
  anniversary_copy,     
  Source_id ,    
  Currency_id, 
  total_tax_amount,
  insurance_file_type_id,	 
  allow_written_status,    
  code,    
  written_task_manager_days,    
  written_rem_user_group,    
  written_rem_task_group,   
  inception_date_tpi,
  CollectionFrequency_id,
  DOPaymentTerms_id,
  insurance_file_status_id ,
  is_marketplace_policy  ,
  TransCurrencyID,
  BaseCurrencyID,
  TransISOCode,
  BaseISOCode,   
  Correspondence_Type,
  Default_Preferred_Correspondence,
  Is_Agent_Correspondence,
  discount_recurring_type_id
  

 FROM #tempData

 IF OBJECT_ID('tempdb..#tempData') IS NOT NULL
 BEGIN
	DROP TABLE #tempData
 END
 
END       
  
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO       
