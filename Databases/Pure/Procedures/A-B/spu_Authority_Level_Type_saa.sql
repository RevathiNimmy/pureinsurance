SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Authority_Level_Type_saa'
GO


CREATE PROCEDURE spu_Authority_Level_Type_saa
AS


SELECT  authority_level_type_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date
FROM    Authority_Level_Type
WHERE   is_deleted = 0

ORDER BY description ASC
GO


