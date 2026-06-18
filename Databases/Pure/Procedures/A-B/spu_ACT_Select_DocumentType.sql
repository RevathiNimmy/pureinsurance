SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_DocumentType'
GO


CREATE PROCEDURE spu_ACT_Select_DocumentType
    @documenttype_id smallint
AS


SELECT
    documenttype_id,
    doctypegroup_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM DocumentType
WHERE documenttype_id = @documenttype_id
GO


