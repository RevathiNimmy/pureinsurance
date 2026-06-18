SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_header_saa'
GO


CREATE PROCEDURE spu_GIS_user_def_header_saa
AS


SELECT
    GIS_user_def_header_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date,
    Parent
 FROM GIS_user_def_header

ORDER BY GIS_user_def_header_id ASC
GO


