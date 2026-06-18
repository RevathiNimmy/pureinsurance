SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_user_def_header_rat_saa'
GO


CREATE PROCEDURE spu_GIS_user_def_header_rat_saa
    @GIS_user_def_header_id INT
AS


SELECT
    GIS_user_def_header_id,
    GIS_user_def_header_rates_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date
 FROM GIS_user_def_header_rates
WHERE GIS_user_def_header_id = @GIS_user_def_header_id
ORDER BY GIS_user_def_header_rates_id ASC
GO


