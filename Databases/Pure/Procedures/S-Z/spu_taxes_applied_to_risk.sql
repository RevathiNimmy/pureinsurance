SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_taxes_applied_to_risk'
GO


CREATE PROCEDURE spu_taxes_applied_to_risk
    @risk_cnt int
AS


SELECT  rt.is_suppress_taxes
FROM        Risk r,
        Risk_type rt
WHERE   r.risk_cnt = @risk_cnt
AND     r.risk_type_id = rt.risk_type_id
GO


