SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_Screen_add'
GO

CREATE PROCEDURE spe_GIS_Screen_add
    @GIS_screen_id int OUTPUT,
    @GIS_data_model_id int,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @parent_id int,
    @is_maintainable tinyint,
    @script_defaults text,
    @script_dynamic_logic text,
	@screen_type tinyint,
    @screen_height int,
    @screen_width int,
	@risk_type_rule_set_type_id int,
	@file_name_Defaults varchar(255),
	@file_name_Validation varchar(255)
AS
BEGIN

IF @GIS_Screen_id = 0
    SELECT @GIS_Screen_id = NULL

IF @GIS_Screen_id IS NULL
    SELECT @GIS_Screen_id = MAX(GIS_Screen_id) + 1
    FROM GIS_Screen

IF @GIS_Screen_id IS NULL
    SELECT @GIS_Screen_id = 1

INSERT INTO GIS_Screen (
    GIS_screen_id,
    GIS_data_model_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date,
    parent_id,
    is_maintainable,
    script_defaults,
    script_dynamic_logic,
    screen_type,
    screen_height,
    screen_width,
	risk_type_rule_set_type_id,
	file_name_Defaults,
	file_name_Validation)
VALUES (
    @GIS_screen_id,
    @GIS_data_model_id,
    @caption_id,
    @code,
    @description,
    @is_deleted,
    @effective_date,
    @parent_id,
    @is_maintainable,
    @script_defaults,
    @script_dynamic_logic,
    @screen_type,
    @screen_height,
    @screen_width,
	@risk_type_rule_set_type_id,
	@file_name_Defaults,
	@file_name_Validation)

SELECT @GIS_screen_id

END

GO
