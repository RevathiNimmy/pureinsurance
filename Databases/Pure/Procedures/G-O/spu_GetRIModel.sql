SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GetRIModel'
GO


Create Procedure spu_GetRIModel 
    @Effective_Date dateTime,
    @risk_type_id int,
    @ri_band_id int,
    @allow_deferred int,
    @RI_Model_id int output
As
    
    Select  @ri_model_id = Null  

    Select  @ri_model_id = rmu.ri_model_id
    From    risk_type_ri_model_usage rmu  
    Join    ri_model rm  
            On rm.ri_model_id = rmu.ri_model_id  
    Where   rmu.risk_type_id = @risk_type_id  
    And     rmu.ri_band = @ri_band_id  
    And     rmu.is_deleted = 0  
    And     rmu.effective_date <= @effective_date  
    And    (rmu.expiry_date >= @effective_date or IsNull(rmu.expiry_date, '1899.12.29') = '1899.12.29')  
    And    (rm.ri_model_type = 0  
        Or (rm.ri_model_type = 2 And @allow_deferred = 1))  
    And     rm.is_deleted = 0  
    And     rm.effective_date <= @effective_date  
    And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')  
    Order By  
            rm.ri_model_type Desc,    -- give priority to none-deferred models  
            rmu.effective_date Asc   -- give priority to newer models  
  
    -- If model is not specified for band, check for a system default model  
    If @ri_model_id Is Null  
        Select  @ri_model_id = rm.ri_model_id
        From    ri_model rm  
        Where   rm.ri_model_type = 1 -- Default  
        And     rm.is_deleted = 0  
        And     rm.effective_date <= @effective_date  
        And    (rm.expiry_date >= @effective_date or IsNull(rm.expiry_date, '1899.12.29') = '1899.12.29')  
        Order By  
                rm.effective_date   -- give priority to newer models 

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
