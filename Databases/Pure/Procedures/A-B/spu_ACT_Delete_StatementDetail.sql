SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_StatementDetail'
GO


CREATE PROCEDURE spu_ACT_Delete_StatementDetail
    @statementdetail_id int
AS


DELETE FROM StatementDetail
WHERE statementdetail_id = @statementdetail_id
GO


