SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_pmwrk_task_group_everyone'
GO
CREATE PROCEDURE spu_pmwrk_task_group_everyone  
    @pmwrk_task_group_id INT,  
    @effective_date DATETIME,  
    @language_id INT = 1	
AS  
  
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.1 Use display_icon from task 20/09/1999 DAK */  
/* 1.2 New fields on task table 07/10/1999 DAK */  
/* 1.3 Category now associated with task rather than task group 21/12/1999 DAK */  
/* 1.4 Fixed bug with is_view_only_task being returned in 2 columns 15/12/1999 RFC */  
/********************************************************************************************************/  
SELECT wt.pmwrk_task_id id,  
    wt.caption_id caption_id,  
    wt.code code,  
--Start (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    description=isnull(pmc.caption, wt.description),  
--End (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    wt.is_deleted is_deleted,  
    wt.effective_date effective_date,  
    wt.display_icon display_icon,  
    1 included,  
    wt.is_view_only_task is_view_only_task,  
    wt.linked_object_name linked_object_name,  
    wt.linked_class_name linked_class_name,  
    wt.linked_caption_id linked_caption_id,  
    wt.is_available_task is_available_task,  
    wt.pmwrk_task_category_id task_category_id  
FROM PMWrk_Task_Group_Task tgt,  
PMWrk_Task wt  
--Start (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    left join  
    pmCaption pmc on wt.caption_id = pmc.caption_id  
    AND pmc.language_id = @language_id  
--End (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
WHERE wt.pmwrk_task_id = tgt.pmwrk_task_id  
    AND tgt.pmwrk_task_group_id = @pmwrk_task_group_id  
    AND wt.is_deleted = 0  
    AND wt.effective_date <= @effective_date
UNION  
SELECT wt.pmwrk_task_id id,  
    wt.caption_id caption_id,  
    wt.code code,  
--Start (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    description=isnull(pmc.caption, wt.description),  
--End (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
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
FROM PMWrk_Task wt  
--Start (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
    left join  
    pmCaption pmc on wt.caption_id = pmc.caption_id  
    AND pmc.language_id = @language_id  
--End (Sriram P)-(Tech Spec - UIIC WR01 - User Access - Get Task Groups.doc)-(6)  
WHERE wt.pmwrk_task_id not in (select tgt.pmwrk_task_id  
                 from PMWrk_Task_Group_Task tgt  
                 where tgt.pmwrk_task_group_id = @pmwrk_task_group_id)  
    AND wt.is_deleted = 0  
    and wt.effective_date <= @effective_date  
ORDER BY code asc  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
