EXECUTE DDLDropProcedure 'spu_Claim_portfolio_transfer_sel'
GO

CREATE PROCEDURE spu_Claim_portfolio_transfer_sel  
 @nProduct_id int,  
 @nSource_id int  ,
 @transfer_date date = null
AS  

 IF @transfer_date IS NULL
     BEGIN
         EXEC spu_Get_Portfolio_Transfer_Date @transfer_date OUT
     END
;WITH CLPT(Claim_id,base_claim_id )
AS
(SELECT Claim_Payment.Claim_id ,claim.base_claim_id
    FROM     Claim_Payment
    JOIN Claim On   Claim_Payment.claim_id = Claim.Claim_id
    WHERE  is_dirty=0 AND
     (Claim_Payment.amount NOT IN (0)) AND (Claim_Payment.is_referred = 1))

SELECT M.base_claim_id, M.claim_id , M.policy_id, M.insurance_ref
FROM
(SELECT clm.base_claim_id, MAX(clm.claim_id) claim_id, policy_id, i.insurance_ref
FROM claim  clm
INNER JOIN Claim_RI_Arrangement ra ON clm.claim_id = ra.claim_id
INNER JOIN RI_Band_Version rb ON rb.ri_band_id = ra.ri_band_id
INNER JOIN insurance_file i ON clm.policy_id = i.insurance_file_cnt
      JOIN risk r                         ON r.risk_cnt = RA.risk_cnt
      JOIN risk_type_ri_model_usage u     ON u.risk_type_id = r.risk_type_id
                                          AND u.ri_band = ra.ri_band_id
                                          AND u.ri_model_id = ra.ri_model_id
      JOIN risk_type_ri_model_usage u2    ON u2.portfolio_transfer_from_cnt = u.risk_type_ri_model_usage_cnt
      JOIN RI_Model rim   ON rim.ri_model_id = u2.ri_model_id
  LEFT JOIN (SELECT * from Claim_pt_log WHERE Effective_Date>=@transfer_date) cptl ON clm.base_claim_id=cptl.base_claim_id
  LEFT JOIN CLPT on CLPT.base_claim_id=clm.base_claim_id

 WHERE (i.product_id = @nProduct_id OR ISNULL(@nProduct_id, 0) = 0)
AND (source_id = @nSource_id OR ISNULL(@nSource_id, 0) = 0)
AND Proportional_RI_Cal_Method =2 AND  clm.is_dirty=0
AND CLPT.base_claim_id IS NULL
AND cptl.base_claim_id  IS NULL
AND u.is_deleted = 0
AND u2.is_deleted = 0
GROUP BY clm.base_claim_id, policy_id, i.insurance_ref) M
JOIN Claim on claim.Claim_id=M.claim_id
WHERE  claim.Claim_Status_id<>3
AND claim.Reported_date<@transfer_date
GO
