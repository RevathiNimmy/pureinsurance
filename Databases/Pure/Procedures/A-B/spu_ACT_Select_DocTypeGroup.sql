SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_DocTypeGroup'
GO


CREATE PROCEDURE spu_ACT_Select_DocTypeGroup
    @doctypegroup_id smallint
AS


SELECT
    doctypegroup_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM DocTypeGroup
WHERE doctypegroup_id = @doctypegroup_id
GO


