SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_inst_category_sel'
GO


CREATE PROCEDURE spu_pmwrk_inst_category_sel
    @task_instance_cnt INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 12/10/1999 DAK */
/* 1.1 Category is now related to Task rather than Task Group 20/12/1999 DAK */
/********************************************************************************************************/
SELECT t.pmwrk_task_category_id pmwrk_task_category_id
    FROM PMWrk_Task t WITH (NOLOCK)
	INNER JOIN PMWrk_Task_Instance ti WITH (NOLOCK)
	    ON t.pmwrk_task_id = ti.pmwrk_task_id 
		WHERE ti.pmwrk_task_instance_cnt = @task_instance_cnt
GO


