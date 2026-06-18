SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Screen_sel_by_type'
GO
CREATE PROCEDURE spu_GIS_Screen_sel_by_type  
    @screen_type int,  
    @gis_data_model_id int = -1  
AS  
if @gis_data_model_id = -1  
BEGIN  
 SELECT  
     GIS_screen_id,  
     caption_id,  
     code,  
     description,  
     is_deleted,  
     effective_date,  
     parent_id,  
     is_maintainable,  
     gis_data_model_id,  
     script_defaults,  
     script_dynamic_logic,  
     screen_type  
 FROM GIS_Screen  
 WHERE screen_type = @screen_type  
END  
ELSE  
BEGIN  
 SELECT  
     GIS_screen_id,  
     caption_id,  
     code,  
     description,  
     is_deleted,  
     effective_date,  
     parent_id,  
     is_maintainable,  
     gis_data_model_id,  
     script_defaults,  
     script_dynamic_logic,  
     screen_type  
 FROM GIS_Screen  
 WHERE screen_type = @screen_type  
 AND gis_data_model_id =@gis_data_model_id  
END  
GO