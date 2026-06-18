SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Standard_Wording_Properties'
GO
/*********************************************************************************************************/    
/* spu_SAM_Get_Standard_Wording_Properties - Gets the Standard_Wording preoperties for a given Data Model*/    
/*                                                                                                       */    
/* RDT 7/6/2007                                                                                          */    
/*********************************************************************************************************/    
CREATE PROCEDURE spu_SAM_Get_Standard_Wording_Properties    
    @gis_datamodel_code varchar(30)    
    
AS    
BEGIN    
    
SELECT    
 upper(go.object_name),    
 upper(gp.property_name),    
 CHILD = CASE    
          WHEN gpo.parent_object_id IS NULL THEN 0    
          ELSE 1    
 END,  
 upper(go.table_name)  
FROM    
 gis_property gp    
INNER JOIN gis_object go ON go.gis_object_id = gp.gis_object_id    
INNER JOIN gis_object gpo ON go.parent_object_id = gpo.gis_object_id    
INNER JOIN GIS_Data_Model gdm ON gdm.gis_data_model_id = go.gis_data_model_id    
WHERE    
 gdm.code = @gis_datamodel_code    
 AND gp.specials_type = 5    
    
END 