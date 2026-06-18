SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Sel_Chase_Cycle_Udl_Status_Description'
GO


/*************************************************************************/  
/* Description: copy risk folder record and return Identity column                 */  
/* Date:-06/03/2013                  */  
/*************************************************************************/  
 CREATE PROCEDURE spu_SIR_Sel_Chase_Cycle_Udl_Status_Description                  
    @chase_cycle_rule_id INT = 0                  
AS                  
                  
BEGIN                  
                
            
    SELECT ccr.Chase_cycle_rule_id,          
       
gd.Description as 'ChaseCycle_Desc',          
ccr.chase_cycle_status_udl_value_id ,  
gp.gis_property_id,  
gp.property_name          
         
 from Chase_cycle_rule ccr              
join GIS_Object gm on ccr.gis_data_model_id=ccr.GIS_Data_Model_id                
join GIS_Property gp on (gp.gis_object_id=gm.gis_object_id and is_chase_cycle_property =1)               
join GIS_User_Def_Header gh on gh.gis_user_def_header_id=gp.Specials_Type_Reference            
JOIN GIS_User_Def_Detail gd ON (gd.GIS_User_Def_Header_id = gh.GIS_User_Def_Header_id  and  gd.gis_user_def_detail_id=(ccr.chase_cycle_status_udl_value_id ))          
where ccr.chase_cycle_rule_id=@chase_cycle_rule_id      
        
end 
GO  