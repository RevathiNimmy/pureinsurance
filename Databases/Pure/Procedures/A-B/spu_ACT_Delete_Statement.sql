SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Statement'
GO


CREATE PROCEDURE spu_ACT_Delete_Statement
    @statement_id int
AS


DELETE FROM Statement
WHERE statement_id = @statement_id
GO


