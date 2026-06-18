SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_Currency_Version_Details'
GO

CREATE  PROCEDURE spu_SAM_CLM_Get_Claim_Currency_Version_Details  
 
 @claim_id INT , 
  @Claim_Peril_id int
AS  

BEGIN

SELECT  i.exchange_rate_override_reason_id,
i.base_currency_id,
i.currency_base_xrate,
i.agent_account_currency_id,
i.agent_account_base_xrate,
i.system_base_xrate,
i.system_base_xrate,
i.currency_base_date,
i.account_base_date,
i.system_base_date,
i.insurance_file_cnt,	c.version_id,c.currency_id,s.Source_id,c.created_by_id,c.Claim_number,ISNULL(risk_type.claims_is_post_taxes,0) 'isPostTax',cob.Class_Of_Business_id,cob.code

FROM insurance_file i
JOIN Source  s
 ON i.source_id = s.source_id  
 JOIN claim c  
  ON c.policy_id = i.insurance_file_cnt  
LEFT OUTER JOIN risk ON
    c.risk_type_id = risk.risk_cnt
LEFT OUTER JOIN risk_type ON
   risk.risk_type_id = risk_type.risk_type_id
INNER JOIN (SELECT Peril_Type_ID,Claim_Id, description FROM Claim_Peril  
             WHERE claim_peril_id = @claim_peril_id)  cp ON  
     c.claim_id = cp.claim_id  
  
   INNER JOIN Peril_Type pt ON  
     cp.peril_type_id = pt.peril_type_id  

    INNER JOIN Class_Of_Business cob ON  
      pt.class_of_business_id = cob.class_of_business_id  
 WHERE c.claim_id = @claim_id  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
