SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmuser_groups_all_sel
GO

CREATE PROCEDURE spu_pmuser_groups_all_sel
    @effective_date DATETIME,  
    @language_id INTEGER,
	@AgentKey INT =0
AS

/********************************************************************************************************/
/* sp_pmuser_groups_all_sel selects ALL User Groups */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/10/1998 RFC */
/********************************************************************************************************/
BEGIN
    SELECT  DISTINCT
        ug.pmuser_group_id,
        ug.code,
        CONVERT(VARCHAR(255), c.caption) caption
    FROM    
        PMUser_Group ug
        LEFT OUTER JOIN pmcaption c
            ON ug.caption_id = c.caption_id  
            AND c.language_id = @language_id
		LEFT OUTER JOIN PMUser_Group_User pgu ON ug.pmuser_group_id = pgu.pmuser_group_id
		LEFT OUTER JOIN PMUser pu ON pgu.user_id =pu.user_id 
    WHERE   
        CONVERT(date , ug.effective_date,106) <= isnull(CONVERT(date , @effective_date,106), CONVERT(date , ug.effective_date,106))
    AND ug.is_deleted = 0
	AND (pu.party_cnt=@AgentKey OR @AgentKey=0)
    ORDER BY
        c.caption ASC  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
