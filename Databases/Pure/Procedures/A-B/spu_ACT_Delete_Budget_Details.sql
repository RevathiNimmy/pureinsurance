SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Budget_Details'
GO

CREATE PROCEDURE spu_ACT_Delete_Budget_Details
    @budget_id int
AS

DELETE FROM Budget_Detail
WHERE budget_id = @budget_id

GO






