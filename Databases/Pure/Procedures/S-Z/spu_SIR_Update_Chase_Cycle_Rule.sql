SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Update_Chase_Cycle_Rule'
GO


/*************************************************************************/  
/* Description: Update Chase_Cycle_Rule table  on basis of Chase_Cycle_rule_id            */  
/*DATE:- 06/03/2013             */  
/*************************************************************************/  
CREATE PROCEDURE spu_SIR_Update_Chase_Cycle_Rule      
    @Chase_Cycle_rule_id INT OUTPUT,      
    @description VARCHAR(50),      
    @source_id INT,      
    @gis_data_model_id INT,   
        @gis_property_id INT,       
    @chase_cycle_status_udl_value_id INT,      
    @is_active TINYINT,      
    @Processing_Days INT,      
    @use_effective_date TINYINT = 0,      
    @use_Greater_TransEff_date TINYINT = 0,      
    @product_id INT=NULL,      
    @Include_cancelled_policies TINYINT = NULL,      
    @cancelled_only TINYINT = NULL      
AS      
      
BEGIN      
      
    UPDATE Chase_Cycle_Rule      
        SET description = @description,      
        source_id = @source_id,      
        gis_data_model_id = @gis_data_model_id,  
        gis_property_id = @gis_property_id,      
        chase_cycle_status_udl_value_id = @chase_cycle_status_udl_value_id,      
        is_active = @is_active,      
        Processing_Days = @Processing_Days,      
        use_effective_date=@use_effective_date,      
        use_greater_of_transaction_and_effective_date=@use_Greater_TransEff_date,      
        product_id=@product_id,      
  Include_cancelled_policies  = @Include_cancelled_policies,    
  cancelled_only = @cancelled_only      
      
     WHERE Chase_Cycle_rule_id = @Chase_Cycle_rule_id      
      
END 
go