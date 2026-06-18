SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SirRen_Task_Log_Del'
GO


CREATE PROCEDURE spu_SirRen_Task_Log_Del
    @task_log_cnt int
AS


BEGIN
    IF (@task_log_cnt = -1)
        TRUNCATE TABLE Renewal_Task_Log
    ELSE
        DELETE
        FROM Renewal_Task_Log
        WHERE renewal_task_log_cnt = @task_log_cnt
END
GO


