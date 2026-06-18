SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_Screen_upd_ex'
GO
CREATE PROCEDURE spu_GIS_Screen_upd_ex
    @GIS_screen_id int,
    @GIS_data_model_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @parent_id int,
    @is_maintainable tinyint,
    @script_defaults text,
    @script_dynamic_logic text ,
    @screen_type tinyint,
    @screen_height int,
    @screen_width int,
	@risk_type_rule_set_type_id int,
	@file_name_Defaults varchar(255),
	@file_name_Validation varchar(255)
AS
BEGIN
update GIS_Screen 
    SET
    GIS_data_model_id=@GIS_data_model_id,
    caption_id=@caption_id,
    code=@code,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    parent_id=@parent_id,
    is_maintainable=@is_maintainable,
    script_defaults=@script_defaults,
    script_dynamic_logic=@script_dynamic_logic,
    screen_type=@screen_type,
    screen_height=@screen_height,
    screen_width=@screen_width,
	risk_type_rule_set_type_id=@risk_type_rule_set_type_id,
	file_name_Defaults=@file_name_Defaults,
	file_name_Validation=@file_name_Validation 
	where     GIS_screen_id=@GIS_screen_id
END
GO
