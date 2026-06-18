SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmwrk_all_task_group_sel
GO

CREATE PROCEDURE spu_pmwrk_all_task_group_sel
    @effective_date DATETIME,  
    @language_id INTEGER
AS

/********************************************************************************************************/
/* sp_pmwrk_all_task_group_sel selects ALL tasks */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/10/1998 RFC */
/********************************************************************************************************/
BEGIN

    SELECT  
        wt.pmwrk_task_id,
        wt.code,
        CONVERT(VARCHAR(255), c.caption) caption
    FROM    
        PMWrk_Task wt
        LEFT OUTER JOIN pmcaption c
            ON wt.caption_id = c.caption_id
            AND c.language_id = @language_id
    WHERE 
        wt.effective_date <= @effective_date
    AND wt.is_deleted = 0
    ORDER BY
        caption ASC  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO