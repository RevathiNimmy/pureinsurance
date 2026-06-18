SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_PMWrk_Task_Instance_sel'
GO
--Start (Sriram P) - Tech Spec - UIIC WR33 - Work Manager - View Task-additional changes-(6.2.2)
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.1 Add is_visible column 14/12/1999 DAK */  
/********************************************************************************************************/  
CREATE PROCEDURE spu_PMWrk_Task_Instance_sel  
    @pmwrk_task_instance_cnt INT  
AS  
SELECT  
    pmwrk_task_instance_cnt,  
    PTI.pmwrk_task_group_id,  
    PTI.pmwrk_task_id,  
    PTI.customer,  
    PTI.task_due_date,  
    PTI.pmuser_group_id,  
    PTI.user_id,  
    PTI.description,  
    PTI.task_status,  
    PTI.is_urgent,  
    PTI.date_created,  
    PTI.created_by_id,  
    PTI.last_modified,  
    PTI.modified_by_id,  
    PTI.is_visible,  
    PTI.workflow_information,
    PTI.is_task_review,
    PTI.original_pmuser_group_id,
    PTG.Code As TaskGroupCode,
    PT.Code As TaskCode,
    PUG.Code As UserGroupCode,
    PU.username As UserCode,  
    PCr.username As CreatedByUser,  
    PMo.username As ModifiedByUser
    
    FROM PMWrk_Task_Instance PTI
    LEFT JOIN pmwrk_task_group PTG
    ON PTG.pmwrk_task_group_id = PTI.pmwrk_task_group_id

    LEFT JOIN pmwrk_task PT
    ON PT.pmwrk_task_id = PTI.pmwrk_task_id

    LEFT JOIN pmuser_group PUG
    ON PUG.pmuser_group_id = PTI.pmuser_group_id
    
    LEFT JOIN pmuser PU
    ON PU.user_id = PTI.user_id

    LEFT JOIN pmuser PCr  
    ON PCr.user_id = PTI.created_by_id  

    LEFT JOIN pmuser PMo  
    ON PMo.user_id = PTI.modified_by_id

    WHERE PTI.pmwrk_task_instance_cnt = @pmwrk_task_instance_cnt  

--End (Sriram P) - Tech Spec - UIIC WR33 - Work Manager - View Task-additional changes-(6.2.2)

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO



