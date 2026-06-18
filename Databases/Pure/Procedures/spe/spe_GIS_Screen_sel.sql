SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_Screen_sel'
GO
CREATE PROCEDURE spe_GIS_Screen_sel
    @GIS_screen_id int
AS
SELECT
    gs.GIS_screen_id,
    gs.caption_id,
    gs.code,
    gs.description,
    gs.is_deleted,
    gs.effective_date,
    gs.parent_id,
    gs.is_maintainable,
    gs.gis_data_model_id,
    gs.script_defaults,
    gs.script_dynamic_logic,
	gs.screen_type,
    gs.screen_height,
    gs.screen_width, 	
	gs.risk_type_rule_set_type_id,
	gs.file_name_Defaults,
	gs.file_name_Validation
FROM GIS_Screen gs (nolock)
WHERE gs.GIS_screen_id = @GIS_screen_id
GO