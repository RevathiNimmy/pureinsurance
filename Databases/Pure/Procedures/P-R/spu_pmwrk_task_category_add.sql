SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_category_add'
GO


CREATE PROCEDURE spu_pmwrk_task_category_add
    @caption_id INT,
    @code VARCHAR(10),
    @description VARCHAR(255),
    @is_deleted SMALLINT,
    @effective_date DATETIME,
    @licence_limit INT,
    @licence_key VARCHAR(30),
    @is_block_above_licence_limit SMALLINT,
    @is_warn_above_licence_limit SMALLINT,
    @warns_since_licence_upgrade INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 08/10/1999 DAK */
/********************************************************************************************************/
DECLARE @@pmwrk_task_category_id INT

	SET NOCOUNT ON
	
    SELECT @@pmwrk_task_category_id = max(pmwrk_task_category_id) FROM PMWrk_Task_category

    if @@pmwrk_task_category_id is null
        select @@pmwrk_task_category_id = 1
    else
        select @@pmwrk_task_category_id = @@pmwrk_task_category_id + 1

    insert into PMWrk_Task_Category(
                    pmwrk_task_category_id,
                    caption_id,
                    code,
                    description,
                    is_deleted,
                    effective_date,
                    licence_limit,
                    licence_key,
                    is_block_above_licence_limit,
                    is_warn_above_licence_limit,
                    warns_since_licence_upgrade)
    values (@@pmwrk_task_category_id,
        @caption_id,
        @code,
        @description,
        @is_deleted,
        @effective_date,
        @licence_limit,
        @licence_key,
        @is_block_above_licence_limit,
        @is_warn_above_licence_limit,
        @warns_since_licence_upgrade)

    SELECT @@pmwrk_task_category_id AS pmwrk_task_category_id
    
    SET NOCOUNT OFF
    
GO


