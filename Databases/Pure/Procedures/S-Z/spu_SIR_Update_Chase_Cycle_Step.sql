
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Update_Chase_Cycle_Step'
GO


/*************************************************************************/  
/* Description: Update Chase_Cycle_Step table  on basis of Chase_Cycle_step_id            */  
/*DATE:- 06/03/2013             */  
/*************************************************************************/  
CREATE PROCEDURE spu_SIR_Update_Chase_Cycle_Step    
 @Chase_Cycle_step_id INT OUTPUT,    
 @Chase_Cycle_rule_id INT,    
 @step_number SMALLINT,    
 @number_of_days SMALLINT,    
 @document_template_id INT,    
 @pmwrk_task_id INT,    
 @pmuser_group_id INT,    
 @check_auto_cancel TINYINT,    
 @auto_cancel_policy TINYINT,    
 @next_step SMALLINT,    
 @previous_step SMALLINT,    
 @step_description varchar(255),    
 @pmwrk_task_group_id int  
    
AS    
    
BEGIN    
    
  UPDATE Chase_Cycle_Step SET    
  Chase_Cycle_rule_id = @Chase_Cycle_rule_id,    
  step_number = @step_number,    
  number_of_days = @number_of_days,    
  document_template_id = @document_template_id,    
  pmwrk_task_id = @pmwrk_task_id,    
  pmuser_group_id = @pmuser_group_id,    
  check_auto_cancel = @check_auto_cancel,    
  auto_cancel_policy = @auto_cancel_policy,    
  next_step = @next_step,    
  previous_step = @previous_step,    
  step_description = @step_description,    
  pmwrk_task_group_id = @pmwrk_task_group_id   
   
  
  WHERE Chase_Cycle_step_id = @Chase_Cycle_step_id    
    
END    
  

go