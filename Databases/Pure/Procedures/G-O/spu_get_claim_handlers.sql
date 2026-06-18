SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_claim_handlers'
GO

CREATE PROCEDURE spu_get_claim_handlers
   @user_id   INT = NULL       
AS
BEGIN

	SELECT 	handler_id, code, description
	FROM 	handler

END
GO
