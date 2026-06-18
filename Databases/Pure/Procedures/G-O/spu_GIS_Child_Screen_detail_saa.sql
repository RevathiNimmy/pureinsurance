SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_GIS_Child_Screen_detail_saa
GO

CREATE PROCEDURE spu_GIS_Child_Screen_detail_saa
    @GIS_screen_id INT
AS  
BEGIN

    SELECT  D2.GIS_screen_id,
            D2.screen_detail_cnt,  
            D2.gis_object_id,  
            D2.gis_property_id,  
            D2.is_frame,  
            D2.tab_number,  
            D2.caption,  
            D2.item_top,  
            D2.item_left,  
            D2.item_height,  
            D2.item_width,  
            D2.column_width,  
            D2.pre_quote_requirement,  
            D2.post_quote_requirement,  
            D2.purchase_requirement,  
            D2.parent_id,  
            D2.help_text,  
            D2.default_object_id,  
            D2.default_property_id,  
            D2.is_valuation,  
            D2.is_rate_and_premium,  
            D2.child_screen_id,  
            D2.data_model_type ,  
            D2.PMFormat,  
            D2.column_position,  
            D2.tab_set_index,  
            NULL,  
            NULL,  
            P.property_name,  
            P.column_name,  
            P.Edit_Flags,  
            P.Specials_Type,  
            P.Specials_Type_Reference,  
            NULL,  
            NULL  
    FROM    GIS_Screen_detail D1
        INNER JOIN GIS_Screen_detail D2
            ON D2.GIS_screen_id = D1.child_screen_id 
        LEFT OUTER JOIN GIS_property P  
            ON D2.GIS_property_id = P.GIS_property_id  
               AND D2.GIS_object_id = P.GIS_object_id  
    WHERE   D1.GIS_screen_id = @GIS_screen_id  
        AND D2.GIS_object_id IS NULL  
    UNION  
    SELECT  D2.GIS_screen_id,  
            D2.screen_detail_cnt,  
            D2.gis_object_id,  
            D2.gis_property_id,  
            D2.is_frame,  
            D2.tab_number,  
            D2.caption,  
            D2.item_top,  
            D2.item_left,  
            D2.item_height,  
            D2.item_width,  
            D2.column_width,  
            D2.pre_quote_requirement,  
            D2.post_quote_requirement,  
            D2.purchase_requirement,  
            D2.parent_id,  
            D2.help_text,  
            D2.default_object_id,  
            D2.default_property_id,  
            D2.is_valuation,  
            D2.is_rate_and_premium,  
            D2.child_screen_id,  
            D2.data_model_type ,  
            D2.PMFormat,  
            D2.column_position,  
            D2.tab_set_index,  
            O.object_name,  
            O.table_name,  
            P.property_name,  
            P.column_name,  
            P.Edit_Flags,  
            P.Specials_Type,  
            P.Specials_Type_Reference,  
            O2.object_name,  
            O.is_non_gis  
    FROM    GIS_Screen_detail D1
        INNER JOIN GIS_Screen_detail D2
            ON D2.GIS_screen_id = D1.child_screen_id  
        INNER JOIN GIS_object O
            ON D2.GIS_object_id = O.GIS_Object_id
        LEFT OUTER JOIN GIS_object O2
            ON O.parent_object_id = O2.GIS_Object_id   
        LEFT OUTER JOIN GIS_property P  
            ON D2.GIS_property_id = P.GIS_property_id  
               AND D2.GIS_object_id = P.GIS_object_id  
    WHERE   D1.GIS_screen_id = @GIS_screen_id  
    ORDER BY D2.GIS_screen_id,  
            D2.child_screen_id,  
            D2.screen_detail_cnt

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO