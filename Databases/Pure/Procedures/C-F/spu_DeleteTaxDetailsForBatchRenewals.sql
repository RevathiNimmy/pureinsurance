EXECUTE DDLDropProcedure 'spu_DeleteTaxDetailsForBatchRenewals'
GO

CREATE PROCEDURE spu_DeleteTaxDetailsForBatchRenewals
  @InsuranceFileCnt INT ,
  @RiskCnt INT   
    
AS    
    
    -- delete insurance file tax  
  DELETE Tax_Calculation FROM Tax_Calculation tc  
      						JOIN insurance_file_risk_link  ifrl ON ifrl.insurance_file_cnt = tc.insurance_file_cnt  and IFRL.risk_cnt = TC.risk_cnt
      						WHERE tc.insurance_file_cnt = @InsuranceFileCnt  and ifrl.risk_cnt =@RiskCnt  
  
 -- delete policy_fee_u  
 DELETE Policy_Fee_U FROM Policy_Fee_U  pfu  
     					JOIN insurance_file_risk_link  ifrl ON ifrl.insurance_file_cnt = pfu.insurance_file_cnt  AND iFRL.risk_cnt = PFU.risk_cnt
     					WHERE pfu.insurance_file_cnt = @InsuranceFileCnt and  ifrl.risk_cnt = @RiskCnt  
 

GO