SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmwrk_task_sel_all
GO

CREATE PROCEDURE spu_pmwrk_task_sel_all
    @effective_date DATETIME,
    @language_id INTEGER
AS

/********************************************************************************************************/
/* sp_pmwrk_task_sel_all selects ALL tasks */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 04/01/1999 RFC */
/* 1.1 Amended to return whether the Task requires keys or not. 05/01/1999 RFC */
/* 1.2 Needs keys if ther is a linked object. 07/10/1999 DAK */
/********************************************************************************************************/
BEGIN
    SELECT
        wt.pmwrk_task_id,
        wt.code,
        CONVERT(VARCHAR(255), c.caption) caption,
        0 is_requires_keys
    FROM
        PMWrk_Task wt
        LEFT OUTER JOIN pmcaption c
            ON wt.caption_id = c.caption_id
            AND c.language_id = @language_id
    WHERE   
        wt.effective_date <= @effective_date
    AND wt.is_deleted = 0
    AND wt.linked_object_name IS NULL
        AND (
             wt.pmnav_process_id IS NULL
             OR pmnav_process_id NOT IN (
                                         SELECT pmnav_process_id
                                         FROM pmnav_set_process_key
                                        )
            )

    UNION

    SELECT
        wt.pmwrk_task_id,
        wt.code,
        CONVERT(VARCHAR(255), c.caption) caption,
        1 is_requires_keys
    FROM
        PMWrk_Task wt
        LEFT OUTER JOIN pmcaption c
            ON wt.caption_id = c.caption_id
            AND c.language_id = @language_id
    WHERE   
        wt.effective_date <= @effective_date
    AND wt.is_deleted = 0
        AND wt.linked_object_name IS NULL
        AND (
             wt.pmnav_process_id IS NULL
             OR pmnav_process_id NOT IN (
                                         SELECT pmnav_process_id
                                         FROM pmnav_set_process_key
                                        )
            )
    ORDER BY
         caption ASC

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO