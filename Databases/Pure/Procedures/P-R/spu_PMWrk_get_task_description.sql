SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMWrk_get_task_description'
GO

CREATE PROCEDURE spu_PMWrk_get_task_description
    @task_id INT,
    @task_description VARCHAR(255) OUTPUT
AS
BEGIN

    SELECT @task_description = description 
    FROM PMWrk_task 
    WHERE pmwrk_task_id = @task_id

END
GO
