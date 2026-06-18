SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_category_in_prog'
GO


CREATE PROCEDURE spu_pmwrk_task_category_in_prog
    @task_category_id INT
AS

/********************************************************************************************************/
/* Returns a count of licence task instances that are in progress */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 12/10/1999 DAK */
/* 1.1 Link task to task instance 26/01/2000 DAK */
/********************************************************************************************************/
SELECT COUNT(ti.pmwrk_task_instance_cnt) category_task_count
FROM PMWrk_Task_Instance ti WITH (NOLOCK)
	INNER JOIN PMWrk_Task wt WITH (NOLOCK) ON ti.pmwrk_task_id = wt.pmwrk_task_id
WHERE wt.pmwrk_task_category_id = @task_category_id
    AND ti.task_status = 1
    AND wt.is_view_only_task = 0
GO


