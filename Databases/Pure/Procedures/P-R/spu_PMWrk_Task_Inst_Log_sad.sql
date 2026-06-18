SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_PMWrk_Task_Inst_Log_sad'
GO


CREATE PROCEDURE spu_PMWrk_Task_Inst_Log_sad
    @pmwrk_task_instance_cnt int
AS


SELECT til.pmwrk_task_instance_cnt,
    til.date_created,
    til.text,
    til.created_by_id,
    u.username
FROM PMWrk_Task_Inst_Log til WITH (NOLOCK)
    INNER JOIN PMUser u ON til.created_by_id = u.user_id
    	WHERE til.pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
ORDER BY til.date_created DESC
GO


