--BSJ
--PN11502, revised on 18/09/2004 to include check for GIS_Object_ID 

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Screen_detail_extra_saa'
GO


CREATE PROCEDURE spu_GIS_Screen_detail_extra_saa
    @GIS_screen_id int
AS

SELECT  D.GIS_screen_id,  
    D.screen_detail_cnt,  
    D.gis_object_id,  
    D.gis_property_id,  
    D.is_frame,  
    D.tab_number,  
    D.caption,  
    D.item_top,  
    D.item_left,  
    D.item_height,  
    D.item_width,  
    D.column_width,  
    D.pre_quote_requirement,  
    D.post_quote_requirement,  
    D.purchase_requirement,  
    D.parent_id,  
    D.help_text,  
    D.default_object_id,  
    D.default_property_id,  
    D.is_valuation,  
    D.is_rate_and_premium,  
    D.child_screen_id,  
    D.data_model_type ,  
    D.PMFormat,  
    D.column_position,  
    D.tab_set_index,  
    Null,  
    Null,  
    P.property_name,  
    P.column_name,  
    P.Edit_Flags,  
    P.Specials_Type,  
    P.Specials_Type_Reference,  
    Null,  
    Null,
    P.is_formatted_text   
FROM  
 GIS_Screen_detail D  
     LEFT JOIN GIS_Property P  
  ON D.GIS_property_id = P.GIS_property_id  
  AND P.GIS_Object_ID = D.GIS_Object_ID  
WHERE   D.GIS_screen_id = @GIS_screen_id  
AND D.GIS_object_id IS NULL  
  
UNION  
SELECT  D.GIS_screen_id,  
    D.screen_detail_cnt,  
    D.gis_object_id,  
    D.gis_property_id,  
    D.is_frame,  
    D.tab_number,  
    D.caption,  
    D.item_top,  
    D.item_left,  
    D.item_height,  
    D.item_width,  
    D.column_width,  
    D.pre_quote_requirement,  
    D.post_quote_requirement,  
    D.purchase_requirement,  
    D.parent_id,  
    D.help_text,  
    D.default_object_id,  
    D.default_property_id,  
    D.is_valuation,  
    D.is_rate_and_premium,  
    D.child_screen_id,  
    D.data_model_type,  
    D.PMFormat,  
    D.column_position,  
    D.tab_set_index,  
    O.object_name,  
    O.table_name,  
    P.property_name,  
    P.column_name,  
    P.Edit_Flags,  
    P.Specials_Type,  
    P.Specials_Type_Reference,  
    O2.object_name,  
    O.is_non_gis,
    P.is_formatted_text   
FROM    GIS_Screen_detail D  
 INNER JOIN GIS_object O  
  ON  D.GIS_object_id = O.GIS_Object_id  
     LEFT JOIN GIS_object O2  
  ON  O.parent_object_id = O2.GIS_Object_id  
     LEFT JOIN GIS_property P  
  ON D.GIS_property_id = P.GIS_property_id  
  AND P.GIS_Object_ID = D.GIS_Object_ID  
WHERE   D.GIS_screen_id = @GIS_screen_id  
ORDER BY D.screen_detail_cnt
GO


