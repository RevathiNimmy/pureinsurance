SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_inst_auto_del'
GO


CREATE PROCEDURE spu_pmwrk_task_inst_auto_del
AS
/********************************************************************************************************/
/* Deletes completed Task Instances.                                                                    */
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/12/1998 RFC */
/********************************************************************************************************/
BEGIN
    DECLARE @pmwrk_task_instance_cnt integer,
        @task_due_date datetime,
        @auto_delete_after_num_days integer

    /* Declare the Completed Task Cursor */
    DECLARE c_completed_tasks CURSOR FAST_FORWARD FOR
        SELECT ti.pmwrk_task_instance_cnt,
        ti.task_due_date,
        t.auto_delete_after_num_days
        FROM pmwrk_task_instance ti,
        pmwrk_task t
        WHERE ti.task_status = 3
        AND ti.pmwrk_task_id = t.pmwrk_task_id
        AND t.auto_delete_after_num_days > 0

    OPEN c_completed_tasks
    FETCH NEXT FROM c_completed_tasks INTO
        @pmwrk_task_instance_cnt,
        @task_due_date,
        @auto_delete_after_num_days

    WHILE @@FETCH_STATUS = 0 BEGIN

        IF @auto_delete_after_num_days > 31 BEGIN
            IF DATEADD(wk, (@auto_delete_after_num_days / 7), @task_due_date) <= getdate() BEGIN
                EXECUTE spu_pmwrk_task_inst_del
                    @pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
            END
        END ELSE BEGIN
            IF DATEADD(dd, @auto_delete_after_num_days, @task_due_date) <= getdate() BEGIN
                EXECUTE spu_pmwrk_task_inst_del
                    @pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt
            END
        END

        FETCH NEXT FROM c_completed_tasks INTO
            @pmwrk_task_instance_cnt,
            @task_due_date,
            @auto_delete_after_num_days

    END

    CLOSE c_completed_tasks
    DEALLOCATE c_completed_tasks
END
GO


