SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Budget'
GO


CREATE PROCEDURE spu_ACT_Delete_Budget
    @budget_id int
AS


DELETE FROM Budget
WHERE budget_id = @budget_id
GO


