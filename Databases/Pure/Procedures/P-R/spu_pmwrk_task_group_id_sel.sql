SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_group_id_sel'
GO


CREATE PROCEDURE spu_pmwrk_task_group_id_sel
    @task_group_code varchar(10)
AS


SELECT ptg.pmwrk_task_group_id
    FROM PMWrk_Task_Group ptg
    WHERE ptg.code = @task_group_code
GO


