SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Details  
  
@claim_id int   

AS  
SELECT   
 claim_number,   
 policy_id,  
 ifi.insured_cnt,   
 ifi.insurance_folder_cnt,  
 loss_from_date,   
 ifi.source_id,
 ifold.insurance_holder_cnt, 
 claim.policy_number, 
 party_cnt,
 Suppress_Reserves,
 Suppress_payments,
 risk_type.claims_is_post_taxes  

FROM claim WITH (NOLOCK)   

 LEFT JOIN insurance_file ifi  WITH (NOLOCK) ON   
  ifi.insurance_file_cnt = policy_id  
  
 INNER JOIN Party  WITH (NOLOCK) ON
	party.party_cnt  = ifi.insured_cnt

 INNER JOIN insurance_folder ifold  WITH (NOLOCK) ON  
  ifi.insurance_folder_cnt = ifold.insurance_folder_cnt  
  
  INNER JOIN risk  WITH (NOLOCK) ON  
	claim.risk_type_id = risk.risk_cnt  

   INNER JOIN risk_type  WITH (NOLOCK) ON  
	risk.risk_type_id = risk_type.risk_type_id  
WHERE claim_id = @claim_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
