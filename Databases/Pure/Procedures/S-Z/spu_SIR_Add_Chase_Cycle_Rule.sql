SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Add_Chase_Cycle_Rule'
GO


/*************************************************************************/  
/* Description: insert record into Chase_Cycle_Rule table                 */   
/* Date:- 06/03/2013                   */  
/*************************************************************************/  
CREATE PROCEDURE spu_SIR_Add_Chase_Cycle_Rule      
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
    @include_cancelled TINYINT = NULL,      
    @Cancelled_only TINYINT = NULL      
AS      
      
BEGIN      
      
     
    INSERT INTO Chase_Cycle_Rule (      
        description,      
        source_id,      
        gis_data_model_id,   
        gis_property_id,     
        chase_cycle_status_udl_value_id,      
        is_active,      
        Processing_Days,      
        use_effective_date,      
        use_greater_of_transaction_and_effective_date,      
        Product_id,     
        Include_cancelled_policies,     
  Cancelled_only    
)      
    VALUES (      
        @description,      
        @source_id,      
        @gis_data_model_id,  
        @gis_property_id,      
        @chase_cycle_status_udl_value_id,      
        @is_active,      
        @Processing_Days,      
        @use_effective_date,      
        @use_Greater_TransEff_date,      
        @product_id,      
  @include_cancelled, @Cancelled_only)      
      
END      
      
BEGIN      
      
    SELECT @chase_cycle_rule_id = @@IDENTITY      
      
END 
GO