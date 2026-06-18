SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_DocTypeGroup'
GO


CREATE PROCEDURE spu_ACT_Check_DocTypeGroup
    @doctypegroup_id smallint OUTPUT
AS


BEGIN
    SELECT @doctypegroup_id = doctypegroup_id
    FROM DocTypeGroup
    WHERE doctypegroup_id = @doctypegroup_id
END
BEGIN
IF @doctypegroup_id = NULL
    SELECT @doctypegroup_id = -1
END
GO


