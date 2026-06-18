SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Rsk_Code_sel'
GO


CREATE PROCEDURE spu_Rsk_Code_sel
AS


SELECT risk_code_id, code, description
FROM Risk_code
GO


