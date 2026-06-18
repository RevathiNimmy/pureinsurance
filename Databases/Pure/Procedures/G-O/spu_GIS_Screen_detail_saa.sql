SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Screen_detail_saa'
GO


CREATE PROCEDURE spu_GIS_Screen_detail_saa
    @GIS_screen_id int
AS


SELECT
    GIS_screen_id,
    screen_detail_cnt,
    gis_object_id,
    gis_property_id,
    is_frame,
    tab_number,
    caption,
    item_top,
    item_left,
    item_height,
    item_width,
    column_width,
    pre_quote_requirement,
    post_quote_requirement,
    purchase_requirement,
    parent_id,
    help_text,
    default_object_id,
    default_property_id,
    is_valuation,
    is_rate_and_premium,
    child_screen_id,
    data_model_type,
    PMFormat,
    column_position,
    tab_set_index
FROM GIS_Screen_detail

WHERE GIS_screen_id = @GIS_screen_id
ORDER BY screen_detail_cnt ASC
GO
