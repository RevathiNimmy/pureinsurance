SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_StatementDetail'
GO


CREATE PROCEDURE spu_ACT_Check_StatementDetail
    @statementdetail_id int OUTPUT
AS


BEGIN
    SELECT @statementdetail_id = statementdetail_id
    FROM StatementDetail
    WHERE statementdetail_id = @statementdetail_id
END
BEGIN
IF @statementdetail_id = NULL
    SELECT @statementdetail_id = -1
END
GO


