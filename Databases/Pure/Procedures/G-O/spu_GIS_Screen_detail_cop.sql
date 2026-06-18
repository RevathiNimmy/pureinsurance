SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GIS_Screen_detail_cop'
GO

CREATE PROCEDURE spu_GIS_Screen_detail_cop  
    @OLD_GIS_screen_id INT,  
    @NEW_GIS_screen_id INT  
AS  
  
BEGIN  
  
INSERT INTO GIS_Screen_detail (  
    GIS_screen_id ,  
    screen_detail_cnt ,  
    gis_object_id ,  
    gis_property_id ,  
    is_frame ,  
    tab_number ,  
    caption ,  
    item_top ,  
    item_left ,  
    item_height ,  
    item_width ,  
    column_width ,  
    pre_quote_requirement ,  
    post_quote_requirement ,  
    purchase_requirement ,  
    parent_id ,  
    help_text ,  
    default_object_id ,  
    default_property_id ,  
    is_valuation ,  
    is_rate_and_premium ,  
    child_screen_id ,  
    PMFormat,  
    column_position,  
    tab_set_index, 
    data_model_type)  
SELECT  @NEW_GIS_screen_id,  
    screen_detail_cnt ,  
    gis_object_id ,  
    gis_property_id ,  
    is_frame ,  
    tab_number ,  
    caption ,  
    item_top ,  
    item_left ,  
    item_height ,  
    item_width ,  
    column_width ,  
    pre_quote_requirement ,  
    post_quote_requirement ,  
    purchase_requirement ,  
    parent_id ,  
    help_text ,  
    default_object_id ,  
    default_property_id ,  
    is_valuation ,  
    is_rate_and_premium ,  
    child_screen_id ,  
    PMFormat ,  
    column_position ,  
    tab_set_index, 
    data_model_type  
FROM    GIS_Screen_detail  
WHERE   GIS_Screen_id = @OLD_GIS_Screen_id  
END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
