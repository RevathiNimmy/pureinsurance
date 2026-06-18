SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Update_Credit_Control_Rule'
GO

CREATE PROCEDURE spu_ACT_Update_Credit_Control_Rule  
    @credit_control_rule_id INT,  
    @description VARCHAR(50),  
    @source_id INT,  
    @business_type VARCHAR(20),  
    @pffrequency_id INT,  
    @is_active TINYINT,  
    @Processing_Days INT,  
    @use_effective_date TINYINT = 0,  
    @use_Greater_TransEff_date TINYINT = 0, 
    @pfinstalments_result_id  INT  = null,  
    @policy_is_paid tinyint = null,
    @product_id INT = NULL,
    @use_due_date TINYINT = NULL,
    @use_inception_Date TINYINT = 0,
	@user_id INT,
	@unique_id VARCHAR(50),
	@screen_hierarchy VARCHAR(500)
AS  
  
BEGIN  
    IF @product_id =0 SET @Product_id = NULL

    UPDATE Credit_Control_Rule  
        SET description = @description,  
        source_id = @source_id,  
        business_type = @business_type,  
        pffrequency_id = @pffrequency_id,  
        is_active = @is_active,  
        Processing_Days = @Processing_Days,  
        use_effective_date=@use_effective_date,  
        use_greater_of_transaction_and_effective_date=@use_Greater_TransEff_date, 
        pfinstalments_result_id = @pfinstalments_result_id,   
        policy_is_paid = @policy_is_paid,
        product_id=@product_id, 
	use_due_date = @use_due_date,
	use_inception_Date = @use_inception_Date,
	UserId = @user_id,
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy
 
     WHERE credit_control_rule_id = @credit_control_rule_id  
  
END  



GO
