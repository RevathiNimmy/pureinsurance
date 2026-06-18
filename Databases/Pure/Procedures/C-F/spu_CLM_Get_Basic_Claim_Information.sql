EXECUTE DDLDropProcedure 'spu_CLM_Get_Basic_Claim_Information'
GO
CREATE PROCEDURE spu_CLM_Get_Basic_Claim_Information
@nClaimKey INT
AS
BEGIN
SELECT CLM.Claim_Number, RT.claims_is_post_taxes,PROD.is_Gross_Claim_Payment_Amount, 
	   INF.insured_cnt,
	   INF.insurance_file_cnt,
	   INF.insurance_folder_cnt
FROM Claim CLM WITH(NOLOCK) 

JOIN Insurance_file INF WITH(NOLOCK) 
ON INF.insurance_file_cnt=ClM.Policy_id
JOIN RISK RSK 
ON CLM.Risk_type_id =RSK.risk_cnt
JOIN Risk_Type RT  ON
Rt.risk_type_id=RSK.risk_type_id
JOIN Product PROD
on PROD.product_id=INF.product_id
WHERE CLM.Claim_id=@nClaimKey

END  
GO


