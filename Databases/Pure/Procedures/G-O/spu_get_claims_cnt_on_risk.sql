EXECUTE DDLDropProcedure spu_get_claims_cnt_on_risk
GO

Create PROCEDURE spu_get_claims_cnt_on_risk  
    @risk_cnt INT      
AS  
  
BEGIN  
  
 SELECT  COUNT(DISTINCT clm.claim_id) 
 
	FROM claim  clm  
	INNER JOIN Risk rsk 
	ON clm.Risk_type_id = RSK.risk_cnt   
	WHERE clm.is_dirty=0  
	AND  (clm.Loss_from_date >=rsk.inception_date) 
	AND clm.Risk_type_id= @risk_cnt  
  
END  