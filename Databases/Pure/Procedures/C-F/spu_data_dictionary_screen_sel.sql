SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_data_dictionary_screen_sel
GO

CREATE PROCEDURE spu_data_dictionary_screen_sel
    @GIS_screen_id INT  
AS  
BEGIN 
  
DECLARE @GIS_data_model_id INT

    SELECT  @GIS_data_model_id = (  
        SELECT  DISTINCT (O.GIS_data_model_id)  
        FROM GIS_object O
            INNER JOIN GIS_screen_detail D  
                ON  D.gis_object_id = O.gis_object_id  
        WHERE   D.gis_screen_id = @GIS_screen_id  
        )  
      
    SELECT  O.gis_object_id,  
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
    FROM GIS_object O2
        INNER JOIN GIS_object O
            ON O.parent_object_id = O2.gis_object_id  
        LEFT OUTER JOIN GIS_property P  
            ON O.gis_object_id = P.gis_object_id  
    WHERE   O2.gis_data_model_id = @GIS_data_model_id  
        AND O2.parent_object_id IS NULL  
        AND O.is_selectable_for_screen = 1  
        AND P.is_primary_key = 0  
    UNION  
    SELECT  O.gis_object_id,  
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
    FROM GIS_object O3  
        INNER JOIN GIS_object O2  
            ON O2.parent_object_id = O3.gis_object_id
        INNER JOIN GIS_object O  
            ON O.parent_object_id = O2.gis_object_id  
    WHERE   O3.gis_data_model_id = @GIS_data_model_id  
        AND O3.parent_object_id IS NULL  
        AND O.is_selectable_for_screen = 1  
	ORDER BY  O.is_non_gis,
			O.gis_object_id,
			P.gis_object_id,
			P.gis_property_id

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
