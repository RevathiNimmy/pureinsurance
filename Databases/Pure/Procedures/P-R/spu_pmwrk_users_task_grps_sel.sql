SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmwrk_users_task_grps_sel
GO

CREATE PROCEDURE spu_pmwrk_users_task_grps_sel
    @user_id INTEGER,  
    @effective_date DATETIME,  
    @language_id INTEGER  
AS

/********************************************************************************************************/
/* sp_pmwrk_users_task_grps_sel selects the Task Groups that the user is allowed. */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 07/10/1998 RFC */
/********************************************************************************************************/
BEGIN
    SET NOCOUNT ON

    /* Create the Temporary Group Table */
    CREATE TABLE #Temp_utgs_Groups (
        pmuser_group_id INT NOT NULL,  
        code CHAR(10),  
        caption VARCHAR(255),  
        is_supervisor TINYINT NOT NULL)  

    /* To find out what Task Groups this user is allowed access to, */
    /* we must first find out what Groups he is a member of. */
    INSERT INTO #Temp_utgs_Groups
        (pmuser_group_id,
        code,
        caption,
        is_supervisor)
    EXECUTE spu_pmuser_users_groups_sel
        @user_id=@user_id,
        @effective_date=@effective_date,
        @language_id=@language_id

	SELECT tg.pmwrk_task_group_id,
    --Start (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.1)
	CASE WHEN c2.caption IS NULL
		THEN c.caption
		ELSE c2.caption
	END as 'caption',
	--End (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.1)
        tempg.is_supervisor
    FROM #Temp_utgs_Groups tempg
        INNER JOIN pmuser_group_activity ga
            ON tempg.pmuser_group_id = ga.pmuser_group_id  
        INNER JOIN pmwrk_task_group tg
            ON ga.pmwrk_task_group_id = tg.pmwrk_task_group_id 
                 AND tg.is_deleted = 0
            AND tg.effective_date <= @effective_date 
        LEFT OUTER JOIN pmcaption c  
            ON tg.caption_id = c.caption_id 
            AND c.language_id = @language_id 
	 --Start (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.1)
        LEFT OUTER JOIN pmcaption c2
            ON tg.caption_id = c2.caption_id 
            AND c2.language_id = @language_id  
	 --End (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.1)
    ORDER BY
        caption ASC

    /* Drop the Temporary Groups & Sub Groups Table */
    DROP TABLE #Temp_utgs_Groups

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO