SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_DocTypeGroup'
GO


CREATE PROCEDURE spu_ACT_Delete_DocTypeGroup
    @doctypegroup_id smallint
AS


DELETE FROM DocTypeGroup
WHERE doctypegroup_id = @doctypegroup_id
GO


