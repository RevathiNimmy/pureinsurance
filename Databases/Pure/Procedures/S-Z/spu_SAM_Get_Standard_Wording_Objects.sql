SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Standard_Wording_Objects'
GO

CREATE PROC spu_SAM_Get_Standard_Wording_Objects   
  
@gis_datamodel_code VARCHAR(20)  
  
as  
  
DECLARE @gis_datamodel_id INT  
  
SELECT @gis_datamodel_id=GIS_Data_Model_id FROM GIS_Data_Model WHERE code=@gis_datamodel_code 
  
SELECT gisobj.gis_object_id,gisobj.parent_object_id,gisobj.table_name,gisobj.object_name,  
 gp.property_name,gp.column_name,gp.Specials_Type_Reference,GISPARENTOBJ.object_name 'ParanetName'    
FROM  GIS_Object gisobj   
LEFT OUTER JOIN gis_property gp ON  
gisobj.gis_object_id =gp.gis_object_id  
LEFT OUTER JOIN GIS_Object GISPARENTOBJ ON  
gisobj.parent_object_id =GISPARENTOBJ.gis_object_id   
WHERE gisobj.gis_data_model_id=@gis_datamodel_id and  gp.Specials_Type =5  
