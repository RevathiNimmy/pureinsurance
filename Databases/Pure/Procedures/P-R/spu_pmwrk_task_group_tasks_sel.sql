SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmwrk_task_group_tasks_sel
GO

CREATE PROCEDURE spu_pmwrk_task_group_tasks_sel
    @pmwrk_task_group_id INTEGER,  
    @effective_date DATETIME,  
    @language_id INTEGER  
AS  
  
/********************************************************************************************************/  
/* sp_pmwrk_task_group_tasks_sel selects ALL Tasks within the supplied Task Group. */  
/********************************************************************************************************/  
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */
/* 1.0 Original 27/10/1998 RFC */  
/* 1.1 Amended to return whether the Task requires keys or not. 04/01/1999 RFC */  
/* 1.2 Linked tasks require keys also 07/10/1999 DAK */  
/********************************************************************************************************/  
BEGIN  
    SELECT
        t.pmwrk_task_id,  
        t.code,
        c.caption,
        0 is_requires_keys
    FROM    
        pmwrk_task_group_task tgt
        INNER JOIN pmwrk_task t
            ON tgt.pmwrk_task_id = t.pmwrk_task_id
            AND t.is_deleted = 0  
            AND t.effective_date <= @effective_date
            AND (
                 t.pmnav_process_id IS NULL 
                 OR pmnav_process_id NOT IN (
                                             SELECT pmnav_process_id 
                                             FROM pmnav_set_process_key
                                            )
                )    
        LEFT OUTER JOIN pmcaption c  
            ON t.caption_id = c.caption_id
            AND c.language_id = @language_id  
    WHERE   
        tgt.pmwrk_task_group_id = @pmwrk_task_group_id  
        AND t.linked_object_name IS NULL  

    UNION  

    SELECT  t.pmwrk_task_id,  
            t.code,  
            c.caption,  
            1 is_requires_keys  
    FROM    
        pmwrk_task_group_task tgt
        INNER JOIN pmwrk_task t
            ON tgt.pmwrk_task_id = t.pmwrk_task_id
            AND t.is_deleted = 0  
            AND t.effective_date <= @effective_date
            AND (
                 t.pmnav_process_id IS NULL
                 OR pmnav_process_id NOT IN (
                                             SELECT pmnav_process_id 
                                             FROM pmnav_set_process_key
                                            )
                )    
        LEFT OUTER JOIN pmcaption c  
            ON t.caption_id = c.caption_id
            AND c.language_id = @language_id  
    WHERE   
        tgt.pmwrk_task_group_id = @pmwrk_task_group_id  
        AND t.linked_object_name IS NULL  
    ORDER BY  
            c.caption ASC  

END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO