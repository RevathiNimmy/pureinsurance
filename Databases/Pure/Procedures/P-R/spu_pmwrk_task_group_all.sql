SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_group_all'
GO


CREATE PROCEDURE spu_pmwrk_task_group_all
    @pmwrk_task_group_id INT,
    @effective_date DATETIME
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.1 Use display_icon from task 20/09/1999 DAK */
/* 1.2 More fields on task 07/10/1999 DAK */
/* 1.3 Category now associated with task rather than task group 21/12/1999 DAK */
/********************************************************************************************************/
SELECT wt.pmwrk_task_id id,
    wt.caption_id caption_id,
    wt.code code,
    wt.description description,
    wt.is_deleted is_deleted,
    wt.effective_date effective_date,
    wt.display_icon display_icon,
    0 included,
    wt.is_view_only_task is_view_only_task,
    wt.linked_object_name linked_object_name,
    wt.linked_class_name linked_class_name,
    wt.linked_caption_id linked_caption_id,
    wt.is_available_task is_available_task,
    wt.pmwrk_task_category_id task_category_id
FROM PMWrk_Task wt,
    PMWrk_Task_Group_Task tgt
WHERE wt.pmwrk_task_id = tgt.pmwrk_task_id
    AND tgt.pmwrk_task_group_id = @pmwrk_task_group_id
    AND wt.is_deleted = 0
    AND wt.effective_date <= @effective_date
ORDER BY code asc
GO


