EXECUTE DDLDropProcedure 'spu_Risks_Cloned_RI_Status_ForClaim_Sel'
GO

CREATE PROCEDURE spu_Risks_Cloned_RI_Status_ForClaim_Sel  
 @BaseClaimId INT,  
 @NewInsuranceFileCnt INT,  
 @NewRiskCnt INT,  
 @OldInsuranceFileCnt INT,  
 @OldRiskCnt INT  
AS  

;WITH CLPT(Claim_id,base_claim_id )  
AS  
(SELECT Claim_Payment.Claim_id ,claim.base_claim_id  
    FROM     Claim_Payment  
    JOIN Claim On   Claim_Payment.claim_id = Claim.Claim_id  
    WHERE  is_dirty=0 AND
     (Claim_Payment.amount NOT IN (0)) AND (Claim_Payment.is_referred = 1))  

SELECT  
 clm.Claim_id,  
 clm.Policy_id,  
 clm.Policy_Number,  
 clm.Claim_Number,  
 clm.Risk_type_id,  
 clm.Client_id,  
 clm.Client_name,  
 0,  
 ifi.insurance_file_cnt,  
 ifi.insurance_file_cnt,  
 r.Risk_Cnt,  
 r.Risk_Cnt,  
 1,  
 tt.code,  
    par.shortname,  
    par.[name],  
    clm.Version_id  
 FROM Claim clm  
 INNER JOIN  
 Insurance_File ifi on ifi.insurance_file_cnt=clm.Policy_id  
 INNER JOIN  
 Transaction_Type tt ON tt.transaction_type_id = clm.transaction_type_id  
 LEFT JOIN  
 Party par ON par.shortname = clm.Client_short_name  
 INNER JOIN  
 Risk r ON r.risk_cnt = clm.Risk_type_id  
 LEFT JOIN CLPT on CLPT.claim_id=clm.claim_id  
 WHERE clm.base_claim_id=@BaseClaimId  and EXISTS (SELECT NULL FROM Claim_RI_Arrangement WHERE Claim_id=clm.Claim_id AND Cloned=1)
 And is_dirty=0
 AND CLPT.base_claim_id IS NULL  
 ORDER BY clm.Claim_id, clm.Version_id  
  
GO

