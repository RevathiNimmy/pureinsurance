SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_GetDataModelSearchFields'
GO


CREATE PROCEDURE spu_GIS_GetDataModelSearchFields
    @gis_data_model_id int
AS

SELECT  
 GP.gis_property_id,  
 GP.property_name,  
 GP.gis_object_id,  
 GP.data_type,  
 GP.specials_type,  
 GP.specials_type_reference,
 GOB.table_name
FROM GIS_Property GP  
JOIN GIS_Object GOB 
    ON GOB.gis_object_id = GP.gis_object_id  
JOIN  GIS_Data_Model GDM 
    ON GDM.gis_data_model_id = GOB.gis_data_model_id  
WHERE GDM.gis_data_model_id = @gis_data_model_id  
AND GP.is_deleted = 0  
AND GP.is_search_property = 1  
ORDER BY GP.property_name 

GO


