SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

DDLDropProcedure 'spu_Get_RI_Model_Type'
GO

 CREATE PROCEDURE spu_Get_RI_Model_Type  
   @RiskCnt INT = NULL  
As  
  
BEGIN  
  

SELECT ra.ri_model_id, rm.ri_model_type FROM RI_Arrangement ra JOIN RI_Model rm ON ra.ri_model_id = rm.ri_model_id 
WHERE ra.risk_cnt = @RiskCnt  
 

END  
  
GO