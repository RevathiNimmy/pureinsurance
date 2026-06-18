SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Budget_Detail'
GO


CREATE PROCEDURE spu_ACT_Delete_Budget_Detail
    @budget_detail_id int
AS


DELETE FROM Budget_Detail
WHERE budget_detail_id = @budget_detail_id
GO


