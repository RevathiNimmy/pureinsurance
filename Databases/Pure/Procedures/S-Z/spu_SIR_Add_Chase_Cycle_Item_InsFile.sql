SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Add_Chase_Cycle_Item_InsFile'
GO

CREATE PROCEDURE spu_SIR_Add_Chase_Cycle_Item_InsFile               
@insurance_file_cnt INT,               
@business_type VARCHAR(20)               
AS  
    DECLARE @chase_cycle_status_udl_value_id INT  
    DECLARE @gis_data_model_id INT  
    DECLARE @RiskCnt INT  
    DECLARE @can_auto_cancel TINYINT  
    DECLARE @Chase_Cycle_step_id INT  
    DECLARE @due_date DATETIME  
    DECLARE @processing_days SMALLINT  
    DECLARE @peril_count INT  
    DECLARE @auto_cancel_count INT  
    DECLARE @insurance_folder_cnt INT  
    DECLARE @use_effective_date TINYINT  
    DECLARE @cover_start_date DATETIME  
    DECLARE @greater_of_effective_transaction TINYINT  
    DECLARE @use_due_date TINYINT  
    DECLARE @table_name AS VARCHAR(100)  
    DECLARE @property_name AS VARCHAR(100)  
    DECLARE @gis_data_model_code AS VARCHAR(100)  
    DECLARE @gis_property_id AS INT  
    DECLARE @SQLQuery AS NVARCHAR(500)  
    DECLARE @results AS NVARCHAR(500)  
    DECLARE @ParmDefinition AS NVARCHAR(500)  
    DECLARE @Chase_Cycle_rule_id AS INT  
    DECLARE @risk_folder_cnt as INT  
    DECLARE @lapsed_date as DATETIME


	DECLARE @is_Chase_cycle_exists INT
   SELECT @is_Chase_cycle_exists = Count(*)
   FROM gis_object  
    JOIN gis_property  
    ON gis_property.gis_object_id = gis_object.gis_object_id  
    WHERE  gis_data_model_id IN
	(
		SELECT gis_data_model_id FROM   risk  
		JOIN gis_screen  
        ON risk.gis_screen_id = gis_screen.gis_screen_id
		WHERE risk.risk_cnt IN
		(
			 SELECT risk_cnt  
			  FROM   insurance_file_risk_link  
			  WHERE  insurance_file_cnt = @insurance_file_cnt 
		)

	)  AND is_chase_cycle_property = 1 
	
	IF @is_Chase_cycle_exists = 0 
		return
 
      DECLARE currisks CURSOR FAST_FORWARD FOR  
      SELECT risk_cnt  
      FROM   insurance_file_risk_link  
      WHERE  insurance_file_cnt = @insurance_file_cnt  
  
    OPEN currisks  
    FETCH next FROM currisks INTO @RiskCnt  
    WHILE @@FETCH_STATUS = 0  
      BEGIN  
  
  SELECT @peril_count = Count(DISTINCT peril_type.peril_type_id)  
  FROM   peril_type WITH(nolock)  
      INNER JOIN peril WITH(nolock)  
        ON peril.peril_type_id = peril_type.peril_type_id  
  WHERE  risk_cnt IN (SELECT risk_cnt  
       FROM   insurance_file_risk_link  
       WHERE  insurance_file_cnt = @insurance_file_cnt)  
  
			              
  SELECT @auto_cancel_count = Count(peril_type_id)  
  FROM   peril_type PT WITH(nolock)  
  WHERE  PT.is_auto_cancel = 1  
   AND PT.peril_type_id IN (SELECT peril_type_id  
         FROM   peril WITH(nolock)  
         WHERE  
   risk_cnt IN (SELECT risk_cnt  
   FROM   insurance_file_risk_link  
   WITH(  
   nolock)  
   WHERE  insurance_file_cnt =  
   @insurance_file_cnt))  
  
  IF @auto_cancel_count = @peril_count  
    SELECT @can_auto_cancel = 1  
  ELSE  
    SELECT @can_auto_cancel = 0  
  
  SELECT @gis_data_model_id = gis_data_model_id  
  FROM   risk  
    JOIN gis_screen  
      ON risk.gis_screen_id = gis_screen.gis_screen_id  
  WHERE  risk.risk_cnt = @RiskCnt  
  
  SELECT @gis_data_model_code = code  
  FROM   gis_data_model  
  WHERE  gis_data_model_id = @gis_data_model_id  
  
  SELECT @insurance_folder_cnt=insurance_folder_cnt  
  FROM insurance_file  
  WHERE insurance_file_cnt = @insurance_file_cnt  
  
  select @risk_folder_cnt = risk_folder_cnt  
    from risk where  
   risk_cnt = @RiskCnt  
  
			              
  DECLARE currgis CURSOR FAST_FORWARD FOR  
   SELECT table_name,column_name,gis_property_id  
   FROM gis_object  
    JOIN gis_property  
    ON gis_property.gis_object_id = gis_object.gis_object_id  
    WHERE  gis_data_model_id = @gis_data_model_id  
    AND is_chase_cycle_property = 1  
  
  OPEN currgis  
  FETCH next FROM currgis INTO  @table_name,@property_name,@gis_property_id  
  WHILE @@FETCH_STATUS = 0  
      BEGIN  
    set @table_name = replace(@table_name, ' ', '')  
    set @property_name = replace(@property_name, ' ', '')  
    set @gis_property_id = replace(@gis_property_id, ' ', '')  
    set @gis_data_model_code = replace(@gis_data_model_code, ' ', '')  
  
       SET @SQLQuery = 'select @chase_cycle_status_udl_value_id = '  
          + @property_name+ ' from ' + @table_name  
          + ' join ' + @gis_data_model_code+'_Policy_Binder ' + ' on '  
          + @gis_data_model_code+'_Policy_Binder.'+@gis_data_model_code+'_Policy_Binder_id = '  
          + @table_name+'.'+ @gis_data_model_code+'_Policy_Binder_id'  
          + ' join GIS_Policy_Link on '  
          + @gis_data_model_code+'_Policy_Binder.gis_policy_link_id = '  
          + ' GIS_Policy_Link.gis_policy_link_id '  
          + 'Where risk_id = ' + CAST(@RiskCnt AS VARCHAR(11))  
     SET @ParmDefinition = N'@chase_cycle_status_udl_value_id int OUTPUT'  
  
     EXEC Sp_executesql  
     @SQLQuery,  
     @ParmDefinition,  
     @chase_cycle_status_udl_value_id = @results output  
  
      IF ( @results IS NULL )  
  
     BEGIN  
      DELETE chase_cycle_item  
      WHERE  chase_cycle_step_id in (SELECT chase_cycle_step_id  
      FROM   chase_cycle_step join Chase_cycle_rule on  
      chase_cycle_rule.chase_cycle_rule_id =chase_cycle_step.chase_cycle_rule_id  
      WHERE  
      chase_cycle_rule.chase_cycle_status_udl_value_id = @results )  
      And  
      chase_cycle_item.insurance_folder_cnt = @insurance_folder_cnt  
     END  
  
      ELSE  
     BEGIN  
      SELECT @Chase_Cycle_rule_id = chase_cycle_step.chase_cycle_rule_id,  
          @Chase_Cycle_step_id = chase_cycle_step.chase_cycle_step_id,  
          @processing_days = chase_cycle_rule.processing_days,  
          @cover_start_date = insurance_file.cover_start_date,
          @lapsed_date = insurance_file.lapsed_date,  
          @use_effective_date = Isnull(chase_cycle_rule.use_effective_date, 0),  
          @greater_of_effective_transaction = Isnull(  
          chase_cycle_rule.use_greater_of_transaction_and_effective_date, 0)  
      FROM   insurance_file WITH(nolock)  
          LEFT JOIN chase_cycle_rule WITH(nolock)  
           ON insurance_file.source_id = chase_cycle_rule.source_id  
          LEFT JOIN chase_cycle_step WITH(nolock)  
           ON chase_cycle_rule.chase_cycle_rule_id =  
           chase_cycle_step.chase_cycle_rule_id  
      WHERE  chase_cycle_step.step_number = 1  
          AND chase_cycle_rule.is_active = 1  
          AND chase_cycle_rule.gis_data_model_id = @gis_data_model_id  
          AND chase_cycle_rule.gis_property_id = @GIS_Property_id  
          AND chase_cycle_rule.chase_cycle_status_udl_value_id = @results  
          AND ( ( @business_type = 'MTC'  
            AND ( cancelled_only = 1  
             OR include_cancelled_policies = 1 ) )  
           OR ( @business_type <> 'MTC'  
             AND cancelled_only = 0 ) )  
          AND insurance_file.insurance_file_cnt = @insurance_file_cnt  
          AND ( chase_cycle_rule.product_id IS NULL  
           OR chase_cycle_rule.product_id = insurance_file.product_id )  
  
      IF ( @Chase_Cycle_rule_id IS NULL )  
       BEGIN  
         DELETE chase_cycle_item  
        WHERE  chase_cycle_step_id in (SELECT chase_cycle_step_id  
        FROM   chase_cycle_step join Chase_cycle_rule on  
         chase_cycle_rule.chase_cycle_rule_id =chase_cycle_step.chase_cycle_rule_id
	    	WHERE  chase_cycle_rule.Gis_property_id = @gis_property_id)  
         And  
         chase_cycle_item.insurance_folder_cnt = @insurance_folder_cnt  
         And  
         chase_cycle_item.risk_cnt in(select risk_cnt from risk where risk_folder_cnt = @risk_folder_cnt)    
  
       END  
      Else  
       BEGIN  
  
									                
       IF( @use_effective_date = 0 )  
        BEGIN  
         SELECT @due_date = Dateadd(day, Isnull(@processing_days, 0),Getdate())  
        END  
       ELSE  
        BEGIN  
         IF ( @greater_of_effective_transaction = 1 AND @cover_start_date < Getdate() )  
          SELECT @due_date = Dateadd(day, Isnull(@processing_days, 0),Getdate())  
         ELSE  
          SELECT @due_date = Dateadd(day, Isnull(@processing_days, 0),@cover_start_date)  
        END  
        
		IF ( @business_type = 'RENLAP' AND @use_effective_date = 1 AND @greater_of_effective_transaction = 0 ) 
		OR (@business_type = 'RENLAP' AND @use_effective_date = 1 AND @greater_of_effective_transaction = 1 AND @lapsed_date > Getdate())   
			BEGIN  
				SELECT @due_date = Dateadd(day, Isnull(@processing_days, 0),@lapsed_date)    
			End
         
       IF @Chase_Cycle_step_id IS NOT NULL  
         IF ( @business_type = 'NB' OR @business_type = 'REN')  
          INSERT INTO chase_cycle_item  
             (chase_cycle_reason,  
             insurance_file_cnt,  
             insurance_folder_cnt,  
             can_auto_cancel,  
             will_auto_cancel,  
             chase_cycle_step_id,  
             created_date,  
             due_date,  
             letter_sent,  
             risk_cnt)  
          VALUES    ( @business_type,  
             @insurance_file_cnt,  
             @insurance_folder_cnt,  
             @can_auto_cancel,  
             0,  
             @Chase_Cycle_step_id,  
             Getdate(),  
             @due_date,  
             0,  
             @RiskCnt)  
  
          ELSE IF EXISTS (SELECT NULL  
              FROM   chase_cycle_item  
                JOIN chase_cycle_step  
                ON chase_cycle_item.chase_cycle_step_id =  
                chase_cycle_step.chase_cycle_step_id  
              WHERE  chase_cycle_rule_id = @Chase_Cycle_Rule_id  
                AND insurance_folder_cnt = @insurance_folder_cnt)  
            BEGIN  
             UPDATE chase_cycle_item  
             SET    insurance_file_cnt = @insurance_file_cnt  
             WHERE  chase_cycle_step_id in (SELECT chase_cycle_step_id  
             FROM   chase_cycle_step  
             WHERE  
              chase_cycle_rule_id = @Chase_Cycle_rule_id )  
              AND insurance_folder_cnt = @insurance_folder_cnt  
            END  
          ELSE  
            BEGIN  
  
             DELETE chase_cycle_item  
               WHERE  chase_cycle_step_id in (SELECT chase_cycle_step_id  
               FROM   chase_cycle_step join Chase_cycle_rule on  
              chase_cycle_rule.chase_cycle_rule_id =chase_cycle_step.chase_cycle_rule_id)  
              And  
              chase_cycle_item.insurance_folder_cnt = @insurance_folder_cnt  
              And  
              chase_cycle_item.risk_cnt in(select risk_cnt from risk where risk_folder_cnt = @risk_folder_cnt)  
  
														
            INSERT INTO chase_cycle_item  
             (chase_cycle_reason,  
             insurance_file_cnt,  
             insurance_folder_cnt,  
             can_auto_cancel,  
             will_auto_cancel,  
             chase_cycle_step_id,  
             created_date,  
             due_date,  
             letter_sent,  
             risk_cnt)  
            VALUES      ( @business_type,  
             @insurance_file_cnt,  
             @insurance_folder_cnt,  
             @can_auto_cancel,  
             0,  
             @Chase_Cycle_step_id,  
             Getdate(),  
             @due_date,  
             0,  
             @RiskCnt)  
         END  
      END  
                  END  
      Set @Chase_Cycle_rule_id = Null  
   FETCH next FROM currgis INTO  @table_name,@property_name,@gis_property_id             
			END  -- inner cursor loop              
			CLOSE currgis              
			DEALLOCATE currgis               
                
		FETCH next FROM currisks INTO @RiskCnt               
		END  -- outer cursor loop              
		CLOSE currisks               
		DEALLOCATE currisks  

GO
