SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_risk_cnt_for_original_risk_cnt'
GO

CREATE PROCEDURE spu_Get_risk_cnt_for_original_risk_cnt  
    @original_risk_cnt int  
AS  
  
SELECT risk_cnt
FROM insurance_file_risk_link  
WHERE original_risk_cnt = @original_risk_cnt
GO
