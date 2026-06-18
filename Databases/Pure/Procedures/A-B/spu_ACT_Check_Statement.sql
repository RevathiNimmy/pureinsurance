SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_Statement'
GO


CREATE PROCEDURE spu_ACT_Check_Statement
    @statement_id int OUTPUT
AS


BEGIN
    SELECT @statement_id = statement_id
    FROM Statement
    WHERE statement_id = @statement_id
END
BEGIN
IF @statement_id = NULL
    SELECT @statement_id = -1
END
GO


