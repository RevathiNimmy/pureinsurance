SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_risk_types'
GO


CREATE PROCEDURE spu_get_risk_types
AS


SELECT Risk_type_id,Code, Description
FROM Risk_type
GO


