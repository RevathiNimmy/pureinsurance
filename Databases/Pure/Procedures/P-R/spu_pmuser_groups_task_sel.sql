SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmuser_groups_task_sel
GO

CREATE PROCEDURE spu_pmuser_groups_task_sel
    @pmwrk_task_group_id INTEGER,  
    @effective_date DATETIME,  
    @language_id INTEGER
AS

/********************************************************************************************************/
/* sp_pmuser_group_task_sel selects ALL User Groups of the supplied Task Group */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/10/1998 RFC */
/********************************************************************************************************/
BEGIN

    SELECT  
        ug.pmuser_group_id,
        ug.code,
        CONVERT(VARCHAR(255), c.caption) caption
    FROM    
        PMUser_Group_Activity uga
        INNER JOIN PMUser_Group ug
            ON uga.pmuser_group_id = ug.pmuser_group_id
        LEFT OUTER JOIN pmcaption c
            ON ug.caption_id = c.caption_id
            AND c.language_id = @language_id
    WHERE   
        uga.pmwrk_task_group_id = @pmwrk_task_group_id
    AND ug.effective_date <= @effective_date
    AND ug.is_deleted = 0
    ORDER BY
        caption ASC  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO