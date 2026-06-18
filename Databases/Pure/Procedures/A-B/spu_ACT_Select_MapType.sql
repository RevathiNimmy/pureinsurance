SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_MapType'
GO


CREATE PROCEDURE spu_ACT_Select_MapType
    @maptype_id smallint
AS


SELECT
    maptype_id,
    description,
    code,
    is_deleted,
    effective_date,
    caption_id
FROM MapType
WHERE maptype_id = @maptype_id
GO


