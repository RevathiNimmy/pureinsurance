SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmwrk_users_tasks_sel
GO

CREATE PROCEDURE spu_pmwrk_users_tasks_sel
    @user_id INTEGER,  
    @effective_date DATETIME,  
    @language_id INTEGER  
AS  
BEGIN  
    SET NOCOUNT ON  
    CREATE TABLE #Temp_uts_Groups (  
        pmuser_group_id INT NOT NULL,  
        code CHAR(10),  
        caption VARCHAR(255),  
        is_supervisor TINYINT NOT NULL)
  
    INSERT INTO #Temp_uts_Groups  
        (pmuser_group_id,
        code,  
        caption,  
        is_supervisor)  
    EXECUTE spu_pmuser_users_groups_sel  
        @user_id=@user_id,  
        @effective_date=@effective_date,  
        @language_id=@language_id  

    SELECT  
        tg.pmwrk_task_group_id,  
        t.pmwrk_task_id,  
	--Start (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.2)
	CASE WHEN MAX(c3.caption) IS NULL
		THEN MAX(c.caption)
		ELSE MAX(c3.caption)
	END as 'caption',
	--End (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.2)
        MAX(tempg.is_supervisor) is_supervisor,  
        MAX(t.is_system_task) is_system_task,  
        MAX(t.type_of_task) type_of_task,  
        MAX(t.pmnav_process_id) pmnav_process_id,  
        MAX(t.component_object_name) component_object_name,  
        MAX(t.component_class_name) component_class_name,  
        MAX(t.auto_delete_after_num_days) auto_delete_after_num_days,  
        MAX(t.display_icon) display_icon,  
        MAX(t.is_view_only_task) is_view_only_task,  
        MAX(t.linked_object_name) linked_object_name,  
        MAX(t.linked_class_name) linked_class_name,  
        MAX(c2.caption) linked_caption,  
        nav_xml_file = SPACE(100),  
        MAX(pmnavxm_process_id) pmnavxm_process_id  
    INTO #temp_tasks  
    
    FROM 
        #Temp_uts_Groups tempg
        INNER JOIN pmuser_group_activity ga
            ON tempg.pmuser_group_id = ga.pmuser_group_id   
        INNER JOIN pmwrk_task_group tg
            ON ga.pmwrk_task_group_id = tg.pmwrk_task_group_id 
            AND tg.is_deleted = 0  
            AND tg.effective_date <= @effective_date
        INNER JOIN pmwrk_task_group_task tgt
            ON tg.pmwrk_task_group_id = tgt.pmwrk_task_group_id  
        INNER JOIN pmwrk_task t
            ON tgt.pmwrk_task_id = t.pmwrk_task_id 
            AND t.is_deleted = 0  
            AND t.effective_date <= @effective_date  
            AND t.is_available_task = 1
        LEFT OUTER JOIN pmcaption c
            ON t.caption_id = c.caption_id
            AND c.language_id = 1 -- Take a default language_id as 1 only
        LEFT OUTER JOIN pmcaption c2
            ON t.linked_caption_id = c2.caption_id
            AND c2.language_id = @language_id
		--Start (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.2)
 	    LEFT OUTER JOIN pmcaption c3
			ON t.caption_id = c3.caption_id
               AND c3.language_id = @language_id 
		--End (Girija Chokkalingam) - Tech Spec - UIIC S4IRD001 - US Localisation -(6.2.2)
    GROUP BY  
        tg.pmwrk_task_group_id,  
        t.pmwrk_task_id  
    ORDER BY  
        tg.pmwrk_task_group_id ASC  
  
    UPDATE #temp_tasks SET  
        nav_xml_file = file_name 
    FROM pmnavxm_process p
        INNER JOIN #temp_tasks tt  
            ON p.pmnavxm_process_id = tt.pmnavxm_process_id 
  
    SELECT * FROM #temp_tasks  
  
    DROP TABLE #Temp_uts_Groups  
    DROP TABLE #temp_tasks  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO