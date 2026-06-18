SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_SelAll_Chase_Cycle_Rule'
GO


/*************************************************************************/  
/* Description: Select records from Chase_Cycle_Rule table           */  
/*Date:-06/03/2013                 */  
/*************************************************************************/  
CREATE PROCEDURE spu_SIR_SelAll_Chase_Cycle_Rule                 
    @source_id INT = 0                  
AS                  
                  
  begin              
        
    SELECT distinct ccr.Chase_cycle_rule_id,          
ccr.description,          
ccr.source_id,          
ccr.Gis_Data_Model_id,          
gd.Description as 'ChaseCycle_Desc',    
gp.Gis_property_id,    
gp.property_name,          
ccr.chase_cycle_status_udl_value_id,           
ccr.is_active,          
ccr.use_effective_date  ,  
gdm.description            
from Chase_cycle_rule ccr   
  join GIS_Data_Model  gdm on gdm.gis_data_model_id   =ccr.Gis_data_model_id               
join GIS_Object gm on gm.gis_data_model_id=ccr.GIS_Data_Model_id                
join GIS_Property gp on (gp.gis_object_id=gm.gis_object_id and is_chase_cycle_property =1)               
join GIS_User_Def_Header gh on gh.gis_user_def_header_id=gp.Specials_Type_Reference            
JOIN GIS_User_Def_Detail gd ON (gd.GIS_User_Def_Header_id = gh.GIS_User_Def_Header_id  and  gd.gis_user_def_detail_id=(ccr.chase_cycle_status_udl_value_id ))          
where ccr.source_id = @source_id or ISNULL(@source_id, 0)=0       
           
end          

GO