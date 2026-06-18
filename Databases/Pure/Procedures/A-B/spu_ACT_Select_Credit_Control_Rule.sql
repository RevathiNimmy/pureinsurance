SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Select_Credit_Control_Rule'
GO

CREATE PROCEDURE spu_ACT_Select_Credit_Control_Rule    
    @credit_control_rule_id INT    
AS    
    
BEGIN    
    
    SELECT crr.credit_control_rule_id,    
        crr.description,    
        crr.source_id,    
        crr.business_type,    
        crr.pffrequency_id,    
        pff.description,    
        crr.is_active,    
        crr.Processing_Days,    
        ISNULL(crr.use_effective_date,0),    
        ISNULL(crr.use_greater_of_transaction_and_effective_date,0) ,   
        pfinstalments_result_id, 
        policy_is_paid,
        Product_id,
        ISNULL(crr.use_due_date,0),
	ISNULL(use_inception_date,0) use_inception_date
    FROM Credit_Control_Rule crr    
        LEFT JOIN PFFrequency pff    
        ON crr.pffrequency_id = pff.pffrequency_id    
    WHERE crr.credit_control_rule_id = @credit_control_rule_id    
    
END    



GO
