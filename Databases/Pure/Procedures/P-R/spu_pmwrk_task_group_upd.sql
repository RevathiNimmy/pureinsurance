SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_group_upd'
GO


CREATE PROCEDURE spu_pmwrk_task_group_upd
    @pmwrk_task_group_id INT,
    @caption_id INT,
    @code VARCHAR(10),
    @description VARCHAR(255),
    @is_deleted SMALLINT,
    @effective_date DATETIME,
    @display_icon INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/09/1999 DAK */
/* 1.1 Addition of task group category 07/10/1999 DAK */
/* 1.2 Removal of task group category 21/12/1999 DAK */
/********************************************************************************************************/
UPDATE pmwrk_task_group SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    display_icon = @display_icon
WHERE pmwrk_task_group_id = @pmwrk_task_group_id
GO


