SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMWrk_Workflow_get_next_step'
GO

CREATE PROCEDURE spu_PMWrk_Workflow_get_next_step
    @pmwrk_workflow_step_id INT
AS
BEGIN

DECLARE @workflow_id INT
DECLARE @workflow_step_order SMALLINT

    SELECT @workflow_id = pmwrk_workflow_id, 
           @workflow_step_order = step_order 
    FROM pmwrk_workflow_step 
    WHERE pmwrk_workflow_step_id = @pmwrk_workflow_step_id

    SELECT pmwrk_workflow_step_id,
           task_group_id, 
           task_id, 
           pmuser_group_id, 
           [user_id], 
           step_days_duration, 
           complete_next_workflow_step_id, 
           overdue_next_workflow_step_id
    FROM pmwrk_workflow_step 
    WHERE pmwrk_workflow_id = @workflow_id
      AND step_order >= @workflow_step_order
    ORDER BY step_order

END
GO
