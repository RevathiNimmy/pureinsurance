SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_property_saa'
GO


CREATE PROCEDURE spu_GIS_property_saa 
    @gis_object_id int  
AS  
  
SELECT  
    gis_property_id,  
    gis_object_id,  
    property_name,  
    column_name,  
    data_type,  
    is_input_property,  
    is_identifying_property,  
    is_primary_key,  
    polaris_property_id,  
    is_deleted,  
    is_search_property,  
    index_linking_id,  
    Edit_Flags,  
    Specials_Type,  
    Specials_Type_Reference,  
 is_in_mis_export,  
 is_formatted_text,  
 is_chase_cycle_property,
 is_claim360display   
 FROM GIS_property  
  
WHERE gis_object_id = @gis_object_id  
ORDER BY gis_property_id ASC  
GO


