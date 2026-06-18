SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMWrk_Workflow_on_first_step'
GO

CREATE PROCEDURE spu_PMWrk_Workflow_on_first_step
	@task_id INT,
	@effective_date DATETIME
AS
BEGIN

    SELECT wfs.pmwrk_workflow_id,
           wfs.pmwrk_workflow_step_id,
           wfs.step_description,
           wf.code,
           wf.description
    FROM pmwrk_workflow wf, 
         pmwrk_workflow_step wfs
   WHERE wfs.task_id = @task_id 
     AND wfs.effective_date <= @effective_date
     AND wfs.is_deleted <> 1
     AND wfs.pmwrk_workflow_id = wf.pmwrk_workflow_id
     AND wfs.step_order = 1

END
GO
