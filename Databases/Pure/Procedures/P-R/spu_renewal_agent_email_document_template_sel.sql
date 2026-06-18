--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 06/02/2008    
--
-- Task : Renewal Back Office Changes 
--**********************************************************************************************  
--exec spu_renewal_agent_email_document_template_sel 'selection'

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_renewal_agent_email_document_template_sel'
GO


CREATE PROCEDURE spu_renewal_agent_email_document_template_sel  
    @renewal_type varchar(40)  
AS  
  
IF @renewal_type = 'selection'  
 BEGIN  
  SELECT  
  pa.party_cnt,  
  p.agent_renewal_man_review_template_id,  
  r.report_id,  
  r.report_name,  
  p.true_monthly_policy_renewal_communication,  
  ifi.anniversary_copy  
  
  FROM renewal_report rr  
  LEFT JOIN insurance_file ifi on rr.insurance_file_cnt = ifi.insurance_file_cnt  
  LEFT JOIN product p ON ifi.product_id = p.product_id  
  LEFT JOIN party_agent pa ON ifi.lead_agent_cnt = pa.party_cnt  
  LEFT JOIN report r ON p.agent_renewal_man_review_report_id = r.report_id  
  
  WHERE p.is_agent_renewal_selection_enabled = 1  
  AND pa.party_agent_type_id in (1,3,5) --/ 1 (Broker) 3 (Commission Account) 5 (Intermediary)  
  AND p.TradeRNLOnline = 1  
  
  GROUP BY pa.party_cnt,  
  p.agent_renewal_man_review_template_id,  
  r.report_id,  
  r.report_name,  
  p.true_monthly_policy_renewal_communication,  
  ifi.anniversary_copy  
  ORDER BY pa.party_cnt  
  
 END  
  
IF @renewal_type = 'invitation'  
 BEGIN  
  SELECT  
  pa.party_cnt,  
  p.agent_renewal_invite_template_id,  
  r.report_id,  
  r.report_name,  
  p.true_monthly_policy_renewal_communication,  
  ifi.anniversary_copy  
  
  FROM renewal_status rs  
  LEFT JOIN insurance_file ifi on rs.renewal_insurance_file_cnt = ifi.insurance_file_cnt  
  LEFT JOIN product p ON ifi.product_id = p.product_id  
  LEFT JOIN party_agent pa ON ifi.lead_agent_cnt = pa.party_cnt  
  LEFT JOIN report r ON p.agent_renewal_invite_report_id = r.report_id  
  
  WHERE p.is_agent_renewal_invite_enabled = 1  
  AND pa.party_agent_type_id in (1,3,5) -- 1 (Broker) 3 (Commission Account) 5 (Intermediary)  
  AND rs.renewal_status_type_id = 2 -- 2 for Awaiting Notice Print
  AND P.TradeRNLOnline = 1  
  
  GROUP BY pa.party_cnt,  
  p.agent_renewal_invite_template_id,  
  r.report_id,  
  r.report_name,  
  p.true_monthly_policy_renewal_communication,  
  ifi.anniversary_copy  
  ORDER BY pa.party_cnt  
  
 END  
  
IF @renewal_type = 'acceptance'  
 BEGIN  
  SELECT  
  pa.party_cnt,  
  p.agent_renewal_update_template_id,  
  r.report_id,  
  r.report_name,  
  p.true_monthly_policy_renewal_communication,  
  ifi.anniversary_copy  
  
  FROM renewal_status rs  
  LEFT JOIN insurance_file ifi on rs.renewal_insurance_file_cnt = ifi.insurance_file_cnt  
  LEFT JOIN product p ON ifi.product_id = p.product_id  
  LEFT JOIN party_agent pa ON ifi.lead_agent_cnt = pa.party_cnt  
  LEFT JOIN report r ON p.agent_renewal_update_report_id = r.report_id  
  
  WHERE p.is_agent_renewal_update_enabled = 1  
  AND pa.party_agent_type_id in (1,3,5) -- 1 (Broker) 3 (Commission Account) 5 (Intermediary)  
  AND rs.renewal_status_type_id = 5 -- 5 for Awaiting Update  
  
  AND P.TradeRNLOnline = 1  
  
  GROUP BY pa.party_cnt,  
  p.agent_renewal_update_template_id,  
  r.report_id,  
  r.report_name,  
  p.true_monthly_policy_renewal_communication,  
  ifi.anniversary_copy  
  ORDER BY pa.party_cnt  
  
 END  

GO 

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


