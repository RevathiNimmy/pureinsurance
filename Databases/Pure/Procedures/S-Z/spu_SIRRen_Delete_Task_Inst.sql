SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_Delete_Task_Inst'
GO

CREATE PROCEDURE spu_SIRRen_Delete_Task_Inst
    @task_code varchar(20)
AS

DECLARE @pmwrk_task_instance_cnt int

DECLARE c_cursor CURSOR FAST_FORWARD FOR
    SELECT pmwrk_task_instance_cnt
    FROM PMWrk_Task_Instance
    WHERE pmwrk_task_id IN (
        SELECT pmwrk_task_id
        FROM PMWrk_Task
        WHERE code = @task_code
    )

OPEN c_cursor
FETCH NEXT FROM c_cursor INTO @pmwrk_task_instance_cnt

WHILE @@FETCH_STATUS = 0 BEGIN
    EXECUTE spu_pmwrk_task_inst_del @pmwrk_task_instance_cnt
    FETCH NEXT FROM c_cursor INTO @pmwrk_task_instance_cnt
END

CLOSE c_cursor
DEALLOCATE c_cursor

GO

