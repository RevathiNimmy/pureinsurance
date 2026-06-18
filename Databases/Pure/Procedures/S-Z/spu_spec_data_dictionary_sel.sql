SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_spec_data_dictionary_sel
GO

CREATE PROCEDURE spu_spec_data_dictionary_sel
    @GIS_object_id INT  
AS  
BEGIN
      
    SELECT  
        O.gis_object_id,  
        O.gis_data_model_id,  
        O.object_name,  
        O.table_name,  
        O.max_instances,  
        O.is_quote_object,  
        O.parent_object_id,  
        O.polaris_object_id,  
        O.is_selectable_for_screen,  
        O.is_non_gis,  
        P.gis_property_id,  
        P.gis_object_id,  
        P.property_name,  
        P.column_name,  
        P.data_type,  
        P.is_input_property,  
        P.is_identifying_property,  
        P.is_primary_key,  
        P.polaris_property_id,  
        P.is_deleted,  
        P.is_search_property,  
        P.Edit_Flags,  
        P.Specials_Type,  
        P.Specials_Type_Reference,  
        O.gis_object_id  
    FROM    
        GIS_object O
        LEFT OUTER JOIN GIS_property P  
            ON O.gis_object_id = P.gis_object_id  
            AND P.is_primary_key = 0
            AND O.is_selectable_for_screen = 1    
    WHERE   
        O.gis_object_id = @GIS_object_id  
    UNION  
    SELECT  
        O.gis_object_id,  
        O.gis_data_model_id,  
        O.object_name,  
        O.table_name,  
        O.max_instances,  
        O.is_quote_object,  
        O.parent_object_id,  
        O.polaris_object_id,  
        O.is_selectable_for_screen,  
        O.is_non_gis,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        NULL,  
        O.parent_object_id  
    FROM    
        GIS_object O  
    WHERE   
        O.parent_object_id = @GIS_object_id  
        AND O.is_selectable_for_screen = 1  
	ORDER BY
		is_non_gis,
		o.gis_object_id,
		gis_property_id  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

