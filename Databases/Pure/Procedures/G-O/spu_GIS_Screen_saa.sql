SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Screen_saa'
GO


CREATE PROCEDURE spu_GIS_Screen_saa
AS


SELECT
    s.GIS_screen_id,
    s.GIS_data_model_id,
    s.caption_id,
    s.code,
    s.description,
    s.is_deleted,
    s.effective_date,
    s.parent_id,
    s.is_maintainable,
    dm.code,
    dm.description
FROM
	GIS_Screen s
JOIN GIS_Data_Model dm ON s.GIS_data_model_id = dm.GIS_data_model_id
LEFT JOIN hidden_options ON hidden_options.option_number = 	s.product_option 
WHERE (ISNULL(hidden_options.value,'0')='1' OR s.product_option IS NULL)
AND s.parent_id IS NULL
AND s.is_maintainable = 1
AND s.GIS_data_model_id = dm.GIS_data_model_id
ORDER BY s.code
GO


