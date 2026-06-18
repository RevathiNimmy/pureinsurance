SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_GetRiskClonedRIBand'
GO

CREATE PROCEDURE spu_GetRiskClonedRIBand
	@RiskCnt INT  
AS  

SELECT  count(*)  
FROM ri_arrangement WHERE  risk_cnt = @RiskCnt  
AND  original_flag = 0  
AND  cloned = 1

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO