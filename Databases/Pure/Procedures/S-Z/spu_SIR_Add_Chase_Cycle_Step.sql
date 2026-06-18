  SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Add_Chase_Cycle_Step'
GO


/*************************************************************************/  
/* Description: Insert records into Chase_cycle_step table                 */  
/* Date:- 06/03/2013                */  
/*************************************************************************/  
CREATE PROCEDURE spu_SIR_Add_Chase_Cycle_Step    
 @Chase_Cycle_step_id INT OUTPUT,    
 @Chase_Cycle_rule_id INT,    
 @step_number SMALLINT,    
 @number_of_days SMALLINT,    
 @client_document_template_id INT,    
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
    
    INSERT INTO Chase_Cycle_Step (    
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
  pmwrk_task_group_id)    
    VALUES (    
        @Chase_Cycle_rule_id,    
        @step_number,    
        @number_of_days,    
        @client_document_template_id,    
        @pmwrk_task_id,    
        @pmuser_group_id,    
        @check_auto_cancel,    
        @auto_cancel_policy,    
        @next_step,    
        @previous_step,    
  @step_description,    
  @pmwrk_task_group_id)    
    
    SELECT @Chase_Cycle_step_id = @@IDENTITY    
    
END 
go