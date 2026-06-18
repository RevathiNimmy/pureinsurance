SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_inst_del'
GO


CREATE PROCEDURE spu_pmwrk_task_inst_del
    @pmwrk_task_instance_cnt integer
AS

/********************************************************************************************************/
/* sp_pmwrk_task_inst_del deletes a Task Instance, and Key, Log entries associated. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 29/10/1998 RFC */
/********************************************************************************************************/
BEGIN
    /* Delete any associated Keys */
    DELETE
    FROM PMWrk_Task_Inst_Key
    WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt

    /* Delete any assocaited Log entries */
    DELETE
    FROM PMWrk_Task_Inst_Log
    WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt

    /* Delete the Task Instance Itself */
    DELETE
    FROM PMWrk_Task_Instance
    WHERE pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt

END
GO


