Execute DDLDropProcedure 'spu_Get_Original_Risk_Cnt_For_Risk_Cnt'
GO

CREATE PROCEDURE spu_Get_Original_Risk_Cnt_For_Risk_Cnt  
    @nRiskCnt INT,
    @nOriginalRiskCnt INT OUTPUT 
AS  
  
SELECT original_risk_cnt 
FROM insurance_file_risk_link  
WHERE risk_cnt = @nRiskCnt
GO
