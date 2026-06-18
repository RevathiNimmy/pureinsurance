SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_category_upd'
GO


CREATE PROCEDURE spu_pmwrk_task_category_upd
    @pmwrk_task_category_id INT,
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
UPDATE PMWrk_Task_Category
    SET caption_id = @caption_id,
        code = @code,
        description = @description,
        is_deleted = @is_deleted,
        effective_date = @effective_date,
        licence_limit = @licence_limit,
        licence_key = @licence_key,
        is_block_above_licence_limit = @is_block_above_licence_limit,
        is_warn_above_licence_limit = @is_warn_above_licence_limit,
        warns_since_licence_upgrade = @warns_since_licence_upgrade
    WHERE pmwrk_task_category_id = @pmwrk_task_category_id
GO


