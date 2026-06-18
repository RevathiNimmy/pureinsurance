SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetRiskStatus'
GO

CREATE PROCEDURE spu_GetRiskStatus
	@RiskStatusID int,
	@RiskStatusCode varchar(10)
AS

SELECT 	risk_status_id,
        code
FROM	Risk_Status
WHERE 	(risk_status_id = @RiskStatusID OR @RiskStatusID Is Null)
AND		(code = @RiskStatusCode OR @RiskStatusCode Is Null)

GO