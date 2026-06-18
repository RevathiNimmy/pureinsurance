SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Details_For_Payment'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Details_For_Payment  
  
 @claim_id int,  
 @claim_peril_id int  
  
AS  
  
BEGIN  
DECLARE @AgentUnderwriter varchar(1)  
SELECT  @AgentUnderwriter = value  
FROM    hidden_options  
WHERE   branch_id = 1 and option_number = 1  
IF @AgentUnderwriter is null  
    SELECT @AgentUnderwriter = 'A'  
IF @AgentUnderwriter = ''  
    SELECT @AgentUnderwriter = 'A'  
  
IF @AgentUnderwriter = 'A'  
  
  BEGIN  
  
 SELECT  
  claim.risk_type_id,  
  isnull(risk_code.description,' '),  
  claim.currency_id,  
  currency.description,  
  loss_from_date,  
  ifi.lead_agent_cnt,  
  ifi.insured_cnt,  
  ifi.insured_name,  
  ifi.product_id,  
  agent.name,  
  ifi.source_id,  
  client_account.currency_id as client_currency,  
  agent_account.currency_id as agent_currency,  
  source.base_currency_id,  
  agent.domiciled_for_tax,  
  agent.tax_number,  
  agent.tax_percentage,  
  agent.tax_exempt,  
  client.domiciled_for_tax,  
  client.tax_number,  
  client.tax_percentage,  
  client.tax_exempt,  
  pa.is_in_transfer_mode,  
  business_type.code as transfer_to_business_type_id,  
  pa.transfer_to_party_cnt,  
  transfer_party.domiciled_for_tax,  
  transfer_party.tax_number,  
  transfer_party.tax_percentage,  
  transfer_party.tax_exempt,  
  transfer_party_account.currency_id as transfer_agent_currency,  
  transfer_party.resolved_name,  
  cob.code,  
  cob.class_of_business_id,  
  ifi.insurance_file_cnt,  
  ISNULL(risk_type.claims_is_post_taxes,0),  
  claim.claim_number,  
  cp.description,  
  isnull(product.prevent_cancelled_agents,0),  
  pa.date_cancelled,  
  transfer_agent.date_cancelled,  
  isnull(product.media_type_mandatory,0),  
  claim.Insurer_name
  
  FROM  claim  
  
  INNER JOIN (SELECT Peril_Type_ID,Claim_Id, description FROM Claim_Peril  
             WHERE claim_peril_id = @claim_peril_id)  cp ON  
     claim.claim_id = cp.claim_id  
  
   INNER JOIN Peril_Type pt ON  
     cp.peril_type_id = pt.peril_type_id  
  
    INNER JOIN Class_Of_Business cob ON  
      pt.class_of_business_id = cob.class_of_business_id  
      
    INNER JOIN insurance_file I ON
	I.insurance_file_cnt = claim.policy_id 

    LEFT OUTER JOIN risk_code ON  
   	I.risk_code_id = risk_code.risk_code_id    
  
  LEFT OUTER JOIN risk ON  
    claim.risk_type_id = risk.risk_cnt  
  
  LEFT OUTER JOIN risk_type ON  
   risk.risk_type_id = risk_type.risk_type_id  
  
  INNER JOIN Currency ON  
   claim.currency_id = currency.currency_id  
  
  INNER JOIN insurance_file ifi ON  
   ifi.insurance_file_cnt = claim.policy_id  
  
  INNER JOIN product ON  
   ifi.product_id = product.product_id  
  
  INNER JOIN Source ON  
   ifi.source_id = Source.source_id  
  
  LEFT JOIN party agent ON  
    ifi.lead_agent_cnt = agent.party_cnt  
  
  LEFT JOIN party_agent pa ON  
   ifi.lead_agent_cnt = pa.party_cnt  
  
  LEFT JOIN business_type ON  
   pa.transfer_to_business_type_id = business_type.business_type_id  
  
  LEFT JOIN account transfer_party_account ON  
   transfer_party_account.account_key = pa.transfer_to_party_cnt  
  
  LEFT JOIN party transfer_party ON  
   pa.transfer_to_party_cnt = transfer_party.party_cnt  
  
   LEFT JOIN party_agent transfer_agent ON  
    transfer_party.party_cnt = transfer_agent.party_cnt  
  
  LEFT JOIN party client ON  
   ifi.insured_cnt = client.party_cnt  
  
  LEFT JOIN account agent_account ON  
   ifi.lead_agent_cnt = agent_account.account_key  
  
  LEFT JOIN account client_account ON  
   ifi.insured_cnt = client_account.account_key  
  
 END  
ELSE  
  BEGIN  
 SELECT  
  claim.risk_type_id,  
  risk_Type.description risk_type_description,  
  claim.currency_id loss_currency_id,  
  currency.description loss_currency_description,  
  loss_from_date,  
  ifi.lead_agent_cnt,  
  ifi.insured_cnt,  
  ifi.insured_name,  
  ifi.product_id,  
  agent.name agent_name,  
  ifi.source_id,  
  client_account.currency_id as client_currency,  
  agent_account.currency_id as agent_currency,  
  source.base_currency_id,  
  agent.domiciled_for_tax agent_domiciled_for_tax,  
  agent.tax_number agent_tax_number,  
  agent.tax_percentage agent_tax_percentage,  
  agent.tax_exempt agent_tax_exempt,  
  client.domiciled_for_tax client_domiciled_for_tax,  
  client.tax_number client_tax_number,  
  client.tax_percentage client_tax_percentage,  
  client.tax_exempt client_tax_exempt,  
  pa.is_in_transfer_mode,  
  business_type.code as transfer_to_business_type_id,  
  pa.transfer_to_party_cnt,  
  transfer_party.domiciled_for_tax transfer_agent_domiciled_for_tax,  
  transfer_party.tax_number transfer_agent_tax_number,  
  transfer_party.tax_percentage transfer_agent_tax_percentage,  
  transfer_party.tax_exempt transfer_agent_tax_exempt,  
  transfer_party_account.currency_id as transfer_agent_currency,  
  transfer_party.resolved_name transfer_agent_name,  
  cob.code class_of_business_code,  
  cob.class_of_business_id,  
  ifi.insurance_file_cnt,  
  risk_type.claims_is_post_taxes,  
  claim.claim_number,  
  wcp.description claim_peril_description,  
  product.prevent_cancelled_agents,  
  pa.date_cancelled agent_date_cancelled,  
  transfer_agent.date_cancelled transfer_agent_date_cancelled,  
  product.media_type_mandatory  
  
  FROM claim  
  
  INNER JOIN (SELECT Peril_Type_ID,Claim_Id, description FROM claim_Peril  
             WHERE claim_peril_id = @claim_peril_id) wcp ON  
    claim.claim_id =wcp.claim_id  
  
   INNER JOIN Peril_Type pt ON  
     wcp.peril_type_id = pt.peril_type_id  
  
    INNER JOIN Class_Of_Business cob ON  
      pt.class_of_business_id = cob.class_of_business_id  
  
  INNER JOIN risk ON  
    claim.risk_type_id = risk.risk_cnt  
  
  INNER JOIN risk_type ON  
   risk.risk_type_id = risk_type.risk_type_id  
  
  INNER JOIN Currency ON  
   claim.currency_id = currency.currency_id  
  
  INNER JOIN insurance_file ifi ON  
   ifi.insurance_file_cnt = claim.policy_id  
  
  INNER JOIN product ON  
   ifi.product_id = product.product_id  
  
  INNER JOIN Source ON  
   ifi.source_id = Source.source_id  
  
  LEFT JOIN party agent ON  
    ifi.lead_agent_cnt = agent.party_cnt  
  
  LEFT JOIN party_agent pa ON  
   ifi.lead_agent_cnt = pa.party_cnt  
  
  LEFT JOIN business_type ON  
   pa.transfer_to_business_type_id = business_type.business_type_id  
  
  LEFT JOIN account transfer_party_account ON  
   transfer_party_account.account_key = pa.transfer_to_party_cnt  
  
  LEFT JOIN party transfer_party ON  
   pa.transfer_to_party_cnt = transfer_party.party_cnt  
  
   LEFT JOIN party_agent transfer_agent ON  
    transfer_party.party_cnt = transfer_agent.party_cnt  
  
  LEFT JOIN party client ON  
   ifi.insured_cnt = client.party_cnt  
  
  LEFT JOIN account agent_account ON  
   ifi.lead_agent_cnt = agent_account.account_key  
  
  LEFT JOIN account client_account ON  
   ifi.insured_cnt = client_account.account_key  
  
 END  
  
END  



GO
