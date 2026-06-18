SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Payment_To'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Payment_To  
  
AS  
  
BEGIN  
 SELECT claim_payment_to_id,  
  description,  
  code,  
  is_to_claim_payable,  
  is_to_party,  
  is_to_agent,  
  is_to_client  
 FROM claim_payment_to  
 WHERE is_deleted = 0  
 AND effective_date <=GetDate()  
  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
