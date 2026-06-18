SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Select_Chase_Cycle_Step'
GO


/*************************************************************************/  
/*Description: Select records from Chase_cycle_Step table on the basis os chase_cycle_step_id              */  
/* DATE:-  06/03/2013                  */  
/*************************************************************************/  
CREATE PROCEDURE spu_SIR_Select_Chase_Cycle_Step  
 @Chase_Cycle_step_id INT    
AS    
    
BEGIN    
    
 SELECT    
  Chase_Cycle_step_id,    
  Chase_Cycle_rule_id,    
  step_number,    
  number_of_days,    
 document_template_id,    
  pmwrk_task_id,    
  pmuser_group_id,    
  check_auto_cancel,    
  auto_cancel_policy,    
  next_step,    
  previous_step,    
  step_description,    
  pmwrk_task_group_id   
    
 FROM Chase_Cycle_Step    
 WHERE Chase_Cycle_step_id = @Chase_Cycle_step_id    
    
END 
go