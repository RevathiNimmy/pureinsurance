SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetRiskStatusByRisk'
GO


CREATE PROCEDURE spu_GetRiskStatusByRisk
		@Risk_Cnt INT,
		@Risk_Status_Code VARCHAR(10) OUTPUT
AS

SELECT 		
        @Risk_Status_Code = code
FROM	risk_status
INNER JOIN risk ON risk.risk_status_id = risk_status.risk_status_id 
WHERE risk.risk_cnt = @Risk_Cnt


GO
