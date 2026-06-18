SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_category_del'
GO


CREATE PROCEDURE spu_pmwrk_task_category_del
    @pmwrk_task_category_id INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 08/10/1999 DAK */
/********************************************************************************************************/
UPDATE PMWrk_Task_Category
    SET is_deleted = 1
    WHERE pmwrk_task_category_id = @pmwrk_task_category_id
GO


