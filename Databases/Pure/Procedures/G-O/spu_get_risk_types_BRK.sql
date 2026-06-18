SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_risk_types_BRK'
GO


CREATE PROCEDURE spu_get_risk_types_BRK
AS


SELECT Risk_Code_id,Code, Description
FROM Risk_Code
WHERE is_deleted = 0
GO


