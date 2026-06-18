SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Claim_Base_Currency_Details'
GO

CREATE PROCEDURE spu_CLM_Get_Claim_Base_Currency_Details  
  
 @claim_id INT  
  
AS  
  
IF EXISTS  
 (  
  SELECT  
   NULL  
  FROM hidden_options  
  WHERE option_number = 1  
  AND Value = 'U'  
 )  
BEGIN  
  
 SELECT  
  s.base_currency_id  
 FROM source s  
 JOIN insurance_file i  
  ON i.source_id = s.source_id  
 JOIN claim c  
  ON c.policy_id = i.insurance_file_cnt  
 WHERE c.claim_id = @claim_id  
  
END  
ELSE  
BEGIN  
  
 SELECT  
  s.base_currency_id  
 FROM source s  
 JOIN insurance_file i  
  ON i.source_id = s.source_id  
 JOIN claim c  
  ON c.policy_id = i.insurance_file_cnt  
 WHERE c.claim_id = @claim_id  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
