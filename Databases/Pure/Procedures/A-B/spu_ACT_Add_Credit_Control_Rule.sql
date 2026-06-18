SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Add_Credit_Control_Rule'
GO

CREATE PROCEDURE spu_ACT_Add_Credit_Control_Rule    
    @credit_control_rule_id INT OUTPUT,    
    @description VARCHAR(50),    
    @source_id INT,    
    @business_type VARCHAR(20),    
    @pffrequency_id INT,    
    @is_active TINYINT,    
    @Processing_Days INT,    
    @use_effective_date TINYINT = 0,    
    @use_Greater_TransEff_date TINYINT = 0,   
    @pfinstalments_result_id int = NULL, 
    @policy_is_paid tinyint = null,
    @product_id INT=NULL,    
    @use_due_date TINYINT = NULL,
    @use_inception_Date TINYINT = 0,
	@user_id INT,
	@unique_id VARCHAR(50),
	@screen_hierarchy VARCHAR(500)
AS    
    
BEGIN    
  
    -- if frequency =0 then set to NULL to avoid breaking foreign key    
    DECLARE @pffrequency_id_null INT    
    IF(@pffrequency_id=0)    
        SET @pffrequency_id_null = NULL    
    ELSE    
  SET @pffrequency_id_null = @pffrequency_id    
    IF @product_id = 0 SET @Product_ID= NULL
    
    INSERT INTO Credit_Control_Rule (    
        description,    
        source_id,    
        business_type,    
        pffrequency_id,    
        is_active,    
        Processing_Days,    
        use_effective_date,    
        use_greater_of_transaction_and_effective_date,   
        pfinstalments_result_id, 
        policy_is_paid,
        Product_id, 
	use_due_date,
	use_inception_Date,
	UserId,
	UniqueId,
	ScreenHierarchy)    
    VALUES (    
        @description,    
        @source_id,    
        @business_type,    
        @pffrequency_id_null,    
        @is_active,    
        @Processing_Days,    
        @use_effective_date,    
        @use_Greater_TransEff_date,   
        @pfinstalments_result_id, 
        @policy_is_paid,
        @product_id, 
	@use_due_date,
	@use_inception_Date,
	@user_id,
	@unique_id,
	@screen_hierarchy)    
      
END    
    
BEGIN    
    
    SELECT @credit_control_rule_id = SCOPE_IDENTITY()    
    
END    
  




GO
