/******************************************************************
	PN-15484	JT		-Join of Source_id if hiddenOption 16 is 1
******************************************************************/
	
EXEC DDLDropProcedure 'spu_ACT_Get_Insurance_File_Information'
GO

CREATE PROCEDURE spu_ACT_Get_Insurance_File_Information  
 @insurance_file_cnt INT,  
 @source_id INT OUTPUT,  
 @agent_account_id INT OUTPUT,  
 @currency_id SMALLINT OUTPUT,  
 @premium MONEY OUTPUT,  
 @currency_base_xrate FLOAT OUTPUT,  
 @currency_base_date DATETIME OUTPUT,  
 @account_base_xrate FLOAT OUTPUT,  
 @account_base_date DATETIME OUTPUT,  
 @system_base_xrate FLOAT OUTPUT,  
 @system_base_date DATETIME OUTPUT,  
 @exchange_rate_override_reason_id INT OUTPUT,
  --Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
 @InsuranceRef  VARCHAR(30)= NULL OUTPUT,
 @SubBranchId INT=0 OUTPUT,
 @Account_Key INT=0 OUTPUT,
 --End-(Arul Stephen)-(Tech Spec - WPR67 - Enhancement_Tax_Round Off)
 @cover_start_date DATETIME =NULL OUTPUT,
 @accounting_date  DATETIME =NULL OUTPUT,
 @transaction_export_folder_cnt INT =0   
  
AS  
  
 IF (@Source_id IS null) OR (@Source_id=0)  
 BEGIN  
  SELECT @Source_id = source_id FROM insurance_file WHERE insurance_file_cnt = @insurance_file_cnt  
 END  
  
 DECLARE @hidden_option VARCHAR(20)  
 SELECT @hidden_option = ISNULL(value,0) FROM hidden_options  
  WHERE  branch_id= @Source_id AND option_number =16  
  
DECLARE @accountingdate DATETIME
IF @transaction_export_folder_cnt<>0
    SELECT  @accountingdate=accounting_date  
    FROM    Transaction_Export_Folder F  
    JOIN    Transaction_Export_Detail D ON D.transaction_export_folder_cnt = F.transaction_export_folder_cnt  
    WHERE   F.transaction_export_folder_cnt = @transaction_export_folder_cnt   

DECLARE  
      @tax_value money,  
      @tax_value1 money,  
      @tax_value2 money,  
           @fee_value money  
  
       -- Get tax  
      SELECT  @tax_value1 = SUM(value)  
      FROM    Tax_Calculation  
      WHERE   insurance_file_cnt = @insurance_file_cnt  
      AND  risk_cnt IS NULL  
      AND  transtype in ('TTR','TTF','TTIF')  
  
       SELECT  @tax_value2 = SUM(value)  
       FROM    Tax_Calculation rt  
       JOIN    insurance_file_risk_link ifrl      ON ifrl.risk_cnt = rt.risk_cnt  
       JOIN    risk r                             ON r.risk_cnt = rt.risk_cnt  
       WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt  
       AND     ifrl.status_flag <> 'U'  
       AND     r.is_risk_selected = 1  
       AND  rt.risk_cnt IS NOT NULL  
       AND  transtype in ('TTR','TTF','TTIF')  
  
       SELECT  @tax_value = ISNULL(@tax_value1, 0) + ISNULL(@tax_value2, 0)  
  
       -- Get fee  
       SELECT  @fee_value = isnull(SUM(currency_amount),0)  
       FROM    policy_fee_u  
       WHERE   insurance_file_cnt = @insurance_file_cnt  
  
IF @hidden_option= '1'  
 BEGIN  
  
  SELECT  
   @source_id = i.source_id,  
   @agent_account_id = Case ISNULL(pa.party_agent_type_id, 0)
							When 1 Then aa.account_id
						Else ac.account_id End,  
   @currency_id = i.currency_id,  
   @premium = i.this_premium+@tax_value+@fee_value,  
   @currency_base_xrate = i.currency_base_xrate,  
   @currency_base_date = i.currency_base_date,  
   @account_base_xrate = i.agent_account_base_xrate,  
   @account_base_date = i.account_base_date,  
   @system_base_xrate = i.system_base_xrate,  
   @system_base_date = i.system_base_date,  
--Start-(Arul Stephen)-(Tech Spec - WPR67 - Enhancement_Tax_Round Off)  
   @exchange_rate_override_reason_id = i.exchange_rate_override_reason_id  ,  
   @InsuranceRef=i.insurance_ref,  
   @SubBranchId= ISNULL(aa.sub_branch_id,ac.sub_branch_id),  
   @Account_Key=ISNULL(aa.account_Key,ac.account_Key), 
   --End-(Arul Stephen)-(Tech Spec - WPR67 - Enhancement_Tax_Round Off)
   @accounting_date=@accountingdate, 
   @cover_start_date=i.cover_start_date     
  FROM insurance_file i  
  LEFT JOIN account aa  
   ON aa.account_key = i.lead_agent_cnt  
   AND aa.company_id = i.source_id  
  LEFT JOIN account ac  
   ON ac.account_key = i.insured_cnt  
   AND ac.company_id = i.source_id 
    LEFT JOIN party_agent pa  
   ON pa.party_cnt = i.lead_agent_cnt  
  WHERE insurance_file_cnt = @insurance_file_cnt  
  
 END  
ELSE  
 BEGIN  
  SELECT  
   @source_id = i.source_id,  
   @agent_account_id = Case ISNULL(pa.party_agent_type_id, 0)
							When 1 Then aa.account_id
						Else ac.account_id End,  
   @currency_id = i.currency_id,  
   @premium = i.this_premium+@tax_value+@fee_value,  
   @currency_base_xrate = i.currency_base_xrate,  
   @currency_base_date = i.currency_base_date,  
   @account_base_xrate = i.agent_account_base_xrate,  
   @account_base_date = i.account_base_date,  
   @system_base_xrate = i.system_base_xrate,  
   @system_base_date = i.system_base_date,  
   @exchange_rate_override_reason_id = i.exchange_rate_override_reason_id,
   --Start - Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
   @InsuranceRef=i.insurance_ref,
   @SubBranchId= ISNULL(aa.sub_branch_id,ac.sub_branch_id) ,
   @Account_Key=ISNULL(aa.account_Key,ac.account_Key),
  --End-(Arul Stephen)-(Tech Spec - WPR67 - Enhancement_Tax_Round Off)
   @accounting_date=@accountingdate,
   @cover_start_date=i.cover_start_date       
  FROM insurance_file i  
  LEFT JOIN account aa  
   ON aa.account_key = i.lead_agent_cnt
   LEFT JOIN party_agent pa  
   ON pa.party_cnt = i.lead_agent_cnt   
  LEFT JOIN account ac  
   ON ac.account_key = i.insured_cnt  
  WHERE insurance_file_cnt = @insurance_file_cnt  
  
 END  
