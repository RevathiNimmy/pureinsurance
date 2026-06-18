SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_Claim_ProductRisk_Options'
GO

CREATE  PROCEDURE spu_SAM_Get_Claim_ProductRisk_Options
    @claim_id INT =0   
AS
BEGIN
  
  SELECT TOP 1 P.is_Gross_Claim_Payment_Amount,RT.claims_is_post_taxes  
  FROM Product P INNER JOIN Insurance_File IFL ON P.product_id =IFL.product_id 
  INNER JOIN Claim C ON C.Policy_id  = IFL.insurance_file_cnt
  INNER JOIN insurance_file_risk_link IFRL ON IFRL.insurance_file_cnt =C.Policy_id
  INNER JOIN Risk R ON R.risk_cnt =IFRL.risk_cnt 
  INNER JOIN Risk_TYPE RT ON RT.risk_type_id =R.risk_type_id 
  WHERE C.Claim_id =@claim_id
  
 END
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  

