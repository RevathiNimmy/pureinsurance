SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_MapType'
GO


CREATE PROCEDURE spu_ACT_SelAll_MapType
AS


SELECT
    maptype_id,
    description,
    code,
    is_deleted,
    effective_date,
    caption_id
FROM MapType
GO


