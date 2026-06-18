SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMWrk_Workflow_create_step_instance'
GO

CREATE PROCEDURE spu_PMWrk_Workflow_create_step_instance
    @pmwrk_workflow_step_id INT,
    @pmwrk_task_instance_cnt INT
AS
BEGIN

    INSERT INTO pmwrk_workflow_step_instance
        (
            pmwrk_workflow_step_id,
            pmwrk_task_instance_cnt
        )
    VALUES
        (
            @pmwrk_workflow_step_id,
            @pmwrk_task_instance_cnt
        )
END
GO
