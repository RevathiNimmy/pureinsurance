SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmwrk_task_sel'
GO


CREATE PROCEDURE spu_pmwrk_task_sel
    @pmwrk_task_id INT
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.1 New column, display_icon 20/09/1999 DAK */
/* 1.2 More new columns 06/10/1999 DAK */
/* 1.3 Add task category 21/12/1999 DAK */
/********************************************************************************************************/
SELECT wt.pmwrk_task_id id,
    wt.caption_id caption_id,
    wt.code code,
    wt.description description,
    wt.is_deleted is_deleted,
    wt.effective_date effective_date,
    wt.is_system_task is_system_task,
    wt.type_of_task type_of_task,
    wt.pmnav_process_id pmnav_process_id,
    wt.component_object_name component_object_name,
    wt.component_class_name component_class_name,
    wt.auto_delete_after_num_days auto_delete_after_num_days,
    wt.display_icon display_icon,
    wt.is_view_only_task is_view_only_task,
    wt.linked_object_name linked_object_name,
    wt.linked_class_name linked_class_name,
    wt.linked_caption_id linked_caption_id,
    wt.is_available_task is_available_task,
    wt.pmwrk_task_category_id pmwrk_task_category_id,
    np.file_name pmnavxm_file
    FROM PMWrk_Task wt
    LEFT OUTER JOIN PMNavxm_process np on wt.pmnavxm_process_id = np.pmnavxm_process_id
    WHERE pmwrk_task_id = @pmwrk_task_id
GO


