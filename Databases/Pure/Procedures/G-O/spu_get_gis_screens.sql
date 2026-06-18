SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_GIS_screens'
GO

CREATE PROCEDURE spu_get_GIS_screens  
    @gis_data_model_type_code VARCHAR(10) = 'CLAIM'
AS  
	SELECT gis_screen_id, gis_screen.description from gis_screen  
	LEFT JOIN gis_data_model  
	ON gis_data_model.gis_data_model_id = gis_screen.gis_data_model_id  
	LEFT JOIN gis_data_model_type  
	ON gis_data_model_type.gis_data_model_type_id = gis_data_model.gis_data_model_type_id  
	WHERE gis_data_model_type.code = @gis_data_model_type_code  
GO