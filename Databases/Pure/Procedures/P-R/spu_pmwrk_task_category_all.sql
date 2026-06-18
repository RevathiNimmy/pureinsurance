SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_category_all'
GO


CREATE PROCEDURE spu_pmwrk_task_category_all
    @pmwrk_task_category_id INT,
    @effective_date DATETIME
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 08/10/1999 DAK */
/********************************************************************************************************/
SELECT pmwrk_task_category_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date,
    licence_limit,
    licence_key,
    is_block_above_licence_limit,
    is_warn_above_licence_limit,
    warns_since_licence_upgrade
    FROM PMWrk_Task_Category
    WHERE pmwrk_task_category_id = @pmwrk_task_category_id
GO


