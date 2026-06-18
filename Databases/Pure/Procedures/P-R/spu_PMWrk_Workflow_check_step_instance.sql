SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMWrk_Workflow_check_step_instance'
GO

CREATE PROCEDURE spu_PMWrk_Workflow_check_step_instance
    @task_instance_cnt INT,
    @step_instance_cnt INT OUTPUT,
    @workflow_step_id INT OUTPUT
AS
BEGIN

    SELECT @step_instance_cnt = pmwrk_workflow_step_instance_cnt,
           @workflow_step_id = pmwrk_workflow_step_id 
    FROM   pmwrk_workflow_step_instance 
    WHERE  pmwrk_task_instance_cnt = @task_instance_cnt

END
GO
