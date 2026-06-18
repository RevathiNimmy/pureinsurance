SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_risk_desc'
GO


CREATE PROCEDURE spu_get_risk_desc
    @risk_code_id int
AS


SELECT code, description
FROM Risk_code
WHERE (risk_code_id = @risk_code_id)
GO


