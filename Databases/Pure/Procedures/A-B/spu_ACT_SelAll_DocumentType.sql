SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_DocumentType'
GO


CREATE PROCEDURE spu_ACT_SelAll_DocumentType
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
GO


