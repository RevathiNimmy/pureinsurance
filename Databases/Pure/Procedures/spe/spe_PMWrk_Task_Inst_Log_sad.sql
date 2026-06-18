SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMWrk_Task_Inst_Log_sad'
GO
CREATE PROCEDURE spe_PMWrk_Task_Inst_Log_sad
    @pmwrk_task_instance_cnt int
AS

SELECT
    pmwrk_task_instance_cnt,
    date_created,
    text,
    created_by_id
FROM PMWrk_Task_Inst_Log WITH (NOLOCK)
WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
ORDER BY date_created DESC

GO

