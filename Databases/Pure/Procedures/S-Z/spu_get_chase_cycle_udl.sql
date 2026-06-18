
  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_chase_cycle_udl'
GO


/*************************************************************************/  
/*Description: Select gis_user_def_detail_id, description from gis_user_def_detail  table   on basis of gis_property_id            */  
/* DATE:-06/03/2013                 */  
/*************************************************************************/  
  
  
CREATE PROCEDURE spu_get_chase_cycle_udl        
 @gis_property_id INT        
AS        
        
BEGIN        
      
SELECT gis_user_def_detail_id, description from gis_user_def_detail gd      
JOIN GIS_property gp on gp.Specials_Type_Reference = gd.gis_user_def_header_id      
WHERE gp.gis_property_id = @gis_property_id  and gd.is_deleted=0    
        
END  
  go