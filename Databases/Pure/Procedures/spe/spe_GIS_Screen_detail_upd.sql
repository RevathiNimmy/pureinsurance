SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_Screen_detail_upd'
GO
/*************************************************************************/
/* ERWIN generated update a record based on the key                      */
/*************************************************************************/
/*************************************************************************/
/* 1.0  06/08/1997 RFC Original (Based on SP Original)                   */
/* 1.2  04/12/2001 CLG (Based on SP Original)                            */
/*************************************************************************/
CREATE PROCEDURE spe_GIS_Screen_detail_upd
    @GIS_screen_id int,
    @screen_detail_cnt int,
    @gis_object_id int,
    @gis_property_id int,
    @is_frame tinyint,
    @tab_number tinyint,
    @caption varchar(255),
    @item_top int,
    @item_left int,
    @item_height int,
    @item_width int,
    @column_width int,
    @pre_quote_requirement tinyint,
    @post_quote_requirement tinyint,
    @purchase_requirement tinyint,
    @parent_id int,
    @help_text varchar(255),
    @default_object_id int,
    @default_property_id int,
    @is_valuation tinyint,
    @is_rate_and_premium tinyint,
    @child_screen_id int,
    @PMFormat int,
    @column_position tinyint,
    @tab_set_index integer,
    @data_model_type tinyint
AS
BEGIN

UPDATE GIS_Screen_detail
    SET
    gis_object_id=@gis_object_id,
    gis_property_id=@gis_property_id,
    is_frame=@is_frame,
    tab_number=@tab_number,
    caption=@caption,
    item_top=@item_top,
    item_left=@item_left,
    item_height=@item_height,
    item_width=@item_width,
    column_width=@column_width,
    pre_quote_requirement=@pre_quote_requirement,
    post_quote_requirement=@post_quote_requirement,
    purchase_requirement=@purchase_requirement,
    parent_id=@parent_id,
    help_text=@help_text,
    default_object_id=@default_object_id,
    default_property_id=@default_property_id,
    is_valuation=@is_valuation,
    is_rate_and_premium=@is_rate_and_premium,
    child_screen_id=@child_screen_id,
    PMFormat=@PMFormat,
    column_position=@column_position,
    tab_set_index=@tab_set_index,
    data_model_type=@data_model_type
    WHERE GIS_screen_id = @GIS_screen_id
    AND screen_detail_cnt = @screen_detail_cnt

END
GO