
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Select_Chase_Cycle_Rule'
GO


/*************************************************************************/  
/* Description: Select data from Chase_Cycle_Rule table              */  
/* and generate ID column if required.                                   */  

/* Date:-  06/03/2013                 */  
/*************************************************************************/  
CREATE PROCEDURE  spu_SIR_Select_Chase_Cycle_Rule          
    @Chase_Cycle_rule_id INT          
AS          
          
BEGIN          
        
    SELECT crr.Chase_Cycle_rule_id,          
        crr.description,          
        crr.source_id,          
        crr.GIS_data_model_id,         
        crr.GIS_property_id,       
        crr.chase_cycle_status_udl_value_id,          
        crr.is_active,          
        crr.Processing_Days,          
        ISNULL(crr.use_effective_date,0),          
        ISNULL(crr.use_greater_of_transaction_and_effective_date,0) ,          
        Product_id,          
        ISNULL(crr.include_cancelled_POLICIES,0),          
        ISNULL(crr.cancelled_only,0)  ,    
        gdm.code       
    FROM Chase_Cycle_Rule crr       
     join GIS_Data_Model gdm on  gdm.gis_data_model_id=crr.gis_data_model_id       
    WHERE crr.Chase_Cycle_rule_id = @Chase_Cycle_rule_id          
          
END
go     