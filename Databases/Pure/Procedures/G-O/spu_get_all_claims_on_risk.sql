SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_get_all_claims_on_risk
GO

CREATE PROCEDURE spu_get_all_claims_on_risk  
 @insurance_file_cnt INT
AS  
    
 SELECT clm.base_claim_id , clm.claim_id ,  
 clm.Risk_type_id AS old_risk_cnt,0 AS new_risk_cnt, 0 AS copy_claim_id  
 FROM claim  clm  
 INNER JOIN 
 (SELECT base_claim_id,MAX(claim_id) claim_id FROM claim
	WHERE claim.Policy_id = @insurance_file_cnt
	AND is_dirty=0 GROUP BY base_claim_id) clm1 ON clm.claim_id = clm1.claim_id
 WHERE   clm.Claim_Status_id <> 3   
 AND  clm.base_claim_id  NOT IN
   (SELECT Claim.Claim_id  
    FROM     Claim_Payment  
     INNER JOIN (SELECT MIN(version_id) AS version_id, base_claim_payment_id  
        FROM claim_payment  
        GROUP by base_claim_payment_id) claim_payment_versions  
     ON claim_payment.version_id = claim_payment_versions.version_id  
     and claim_payment.base_claim_payment_id = claim_payment_versions.base_claim_payment_id  
     INNER JOIN  Claim ON  
     Claim_Payment.claim_id = Claim.Claim_id  
     WHERE (Claim_Payment.amount <> 0) 
	 )
   
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO