  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_SelAll_Chase_Cycle_Step'
GO


/*************************************************************************/  
/* Description: It wil select data from Chase_Cycle_step table                 */ 
/* Date:-06/03/2013                    */  
/*************************************************************************/  
create PROCEDURE spu_SIR_SelAll_Chase_Cycle_Step    
    @Chase_Cycle_rule_id INT    
AS    
    
BEGIN    
     
  
SELECT     ccr.chase_cycle_step_id, ccr.chase_cycle_rule_id, ccr.step_number, ccr.number_of_days, ccr.pmwrk_task_id, ccr.pmuser_group_id, ccr.check_auto_cancel, ccr.auto_cancel_policy, ccr.next_step,   
                      ccr.previous_step  
  
      FROM Chase_Cycle_Step  ccr  
     WHERE ccr.Chase_Cycle_rule_id = @Chase_Cycle_rule_id    
  ORDER BY step_number    
    
END 
go