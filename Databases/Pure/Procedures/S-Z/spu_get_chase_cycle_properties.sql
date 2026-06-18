
  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_chase_cycle_properties'
GO


/*************************************************************************/  
/*Description: Select gis_property_id, property_name from GIS_property  table   on basis of gis_data_model_id            */  
/* DATE:-06/03/2013                 */  
/*************************************************************************/  
  
  
CREATE PROCEDURE spu_get_chase_cycle_properties    
 @gis_data_model_id INT    
AS    
    
BEGIN    
  
SELECT gis_property_id, property_name from GIS_property gp  
JOIN GIS_Object gob on gob.GIS_Object_Id = gp.GIS_Object_Id  
WHERE gob.gis_data_model_id = @gis_data_model_id AND gp.is_chase_cycle_property=1  
  
    
END    
  go