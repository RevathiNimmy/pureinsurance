
Execute DDLDropProcedure 'spu_PM_Get_Scheduled_Tasks'
GO
CREATE PROCEDURE spu_PM_Get_Scheduled_Tasks    



    @user_id INT,    



    @task_status INT,    



    @omit_task_status BIT,    



    @pmuser_group_id INT,    



    @selected_user_id INT,    



    @task_due_date_limit DATETIME,    



    @pmuser_group_string VARCHAR(1000),    



    @is_system_task BIT,    



    @source_id INT=NULL,    



    @RestrictTaskList INT=NULL,    



    @PartyCnt INT = NULL    



AS    
    






DECLARE @extracted_pmuser_group_id INT    
    






IF @RestrictTaskList IS NULL SELECT @RestrictTaskList =-1    



IF @source_id IS NULL SELECT @source_id =-1    
    






IF @is_system_task = 0    



BEGIN    
    






    CREATE TABLE #temp_user_group    



    (    



        pmuser_group_id INT    



    )    



    CREATE INDEX temp_user_group__pmuser_group_id ON #temp_user_group (pmuser_group_id)    
    






    IF @pmuser_group_id IS NOT NULL    



    BEGIN    



        INSERT INTO #temp_user_group    



        (    



            pmuser_group_id    



        )    



        VALUES    



        (    



            @pmuser_group_id    



        )    



    END    



    ELSE    



    BEGIN    



        WHILE @pmuser_group_string <> ''    



        BEGIN    
    






            SELECT @extracted_pmuser_group_id = CAST(LEFT(@pmuser_group_string, CHARINDEX('|', @pmuser_group_string) - 1) AS INT)    
    






            INSERT INTO #temp_user_group    



            (    



                pmuser_group_id    



            )    



            VALUES    



            (    



                @extracted_pmuser_group_id    



            )    
    






            SELECT @pmuser_group_string = RIGHT(@pmuser_group_string, LEN(@pmuser_group_string)  - CHARINDEX('|', @pmuser_group_string))    
    






        END    



    END    
    






    SELECT   DISTINCT  



        ti.pmwrk_task_instance_cnt,    



        ti.is_urgent,    



        ti.task_status,    



        t.type_of_task,    



        t.is_system_task,    



        ti.task_due_date,    
        RTRIM(ti.customer) as Customer,  
        RTRIM(ti.description) as Description,  



        ti.pmuser_group_id,    



        ti.user_id,    



        t.pmnav_process_id,    
        RTRIM(t.component_object_name) as component_object_name,  
        RTRIM(t.component_class_name) as component_class_name,  



        t.display_icon,    



        t.is_view_only_task,    
        RTRIM(t.linked_object_name) as linked_object_name,  
        RTRIM(t.linked_class_name) as linked_class_name ,   



        t.linked_caption_id,    



        ti.is_visible,    



        (    



            SELECT    



                file_name    



            FROM pmnavxm_process    



            WHERE pmnavxm_process_id = t.pmnavxm_process_id    



        ) 'nav_xml_file',    



        (    



            SELECT    
                RTRIM(c.shortname)  



            FROM pmwrk_task_inst_key v    



            JOIN party c    



                ON c.party_cnt = v.key_value    



            WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt    



            AND v.pmnav_key_id = 2    



        ) 'shortname',  



       ( Select 1 From pmwrk_task_instance ti1 With (nolock) LEFT JOIN #temp_user_group tg1 ON   



   ti1.pmuser_group_id  =tg1.pmuser_group_id   



  Where ti1.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt  



   AND  tg1.pmuser_group_id IS NULL AND (ti1.created_by_id = @user_id  AND ti1.is_task_review = 1)) as ReadOnly,  
  
        ti.party_cnt PartyCnt, RTRIM(p.shortname) PartyName    



    FROM pmwrk_task_instance ti WITH (NOLOCK)    



    JOIN pmwrk_task t WITH (NOLOCK)    



        ON t.pmwrk_task_id = ti.pmwrk_task_id    



    LEFT JOIN #temp_user_group ug    



        ON (ug.pmuser_group_id = ti.pmuser_group_id) OR (ti.created_by_id = @user_id AND ti.is_task_review = 1)  --OR (ug.pmuser_group_id = ti.original_pmuser_group_id AND ti.is_task_review = 1)    



    LEFT JOIN party p    



        ON (ti.party_cnt=p.party_cnt)    
    






    WHERE    



        (    



            (    



                ti.user_id = @user_id    



                AND    



                ti.is_visible = 0    



            )    



            OR    



            (    



                (    



                    (    



                        @omit_task_status = 0    



                        AND    



                        ti.task_status = ISNULL(@task_status, ti.task_status)    



                   )    



                    OR    



                    (    



                        @omit_task_status = 1    



                        AND    



                        ti.task_status <> @task_status    



                    )    



                )    



                AND (    



                        ti.user_id IS NULL    



                        OR    



                        ti.user_id = ISNULL(@selected_user_id, ti.user_id)    



                         OR    



   (ti.created_by_id = @user_id AND ti.is_task_review = 1)    
  






                    )    



                )    



          )    
                AND (@task_due_date_limit IS NULL OR (ti.task_due_date >= convert(date, getdate()) AND ti.task_due_date <= ISNULL(@task_due_date_limit, ti.task_due_date)))       



                AND t.is_system_task = 0    



       AND ug.pmuser_group_id IS NOT NULL    
    






        AND    



        (    



       @RestrictTaskList <> 1    



            OR    



            (    



                @RestrictTaskList = 1    



                AND    



                @source_id > 0    



                AND    



                (ISNULL(ti.source_id, 0) = @source_id OR ti.source_id IS NULL)    



            )    



        )    
        AND (ti.party_cnt=@PartyCnt OR ISNULL(@PartyCnt,0)=0) And ISNUll(ti.Is_External_WorkItem,0)<>1
    ORDER BY ti.task_due_date ASC    
    






    DROP TABLE #temp_user_group    
    






END    



ELSE    



BEGIN    



    SELECT    



        ti.pmwrk_task_instance_cnt,    



        ti.is_urgent,    



        ti.task_status,    



        t.type_of_task,    



        t.is_system_task,    



        ti.task_due_date,    
        RTRIM(ti.customer) as Customer,  
        RTRIM(ti.description) as Description,  



        ti.pmuser_group_id,    



        ti.user_id,    



        t.pmnav_process_id,    
        RTRIM(t.component_object_name) as component_object_name,  
        RTRIM(t.component_class_name) as component_class_name,   



        t.display_icon,    



        t.is_view_only_task,    
        RTRIM(t.linked_object_name) as linked_object_name,  
        RTRIM(t.linked_class_name) as linked_class_name ,     



        t.linked_caption_id,    



        ti.is_visible,    



        (    



            SELECT    



                file_name    



            FROM pmnavxm_process    



            WHERE pmnavxm_process_id = t.pmnavxm_process_id    



        ) 'nav_xml_file',    



        (    



            SELECT    
                RTRIM(c.shortname)    



            FROM pmwrk_task_inst_key v    



            JOIN party c    



                ON c.party_cnt = v.key_value    



            WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt    



            AND v.pmnav_key_id = 2    



        ) 'shortname',0 as ReadOnly ,   
        ti.party_cnt PartyCnt, RTRIM(p.shortname) PartyName    



    FROM pmwrk_task t WITH (NOLOCK)    



    JOIN pmwrk_task_instance ti WITH (NOLOCK)    



        ON ti.pmwrk_task_id = t.pmwrk_task_id    



        LEFT JOIN party p    



        ON (ti.party_cnt=p.party_cnt)    
    






    WHERE t.is_system_task = 1    
    AND (@task_due_date_limit IS NULL OR (ti.task_due_date >= convert(date, getdate()) AND ti.task_due_date <= ISNULL(@task_due_date_limit, ti.task_due_date)))    
            AND (ti.party_cnt=@PartyCnt OR ISNULL(@PartyCnt,0)=0) And ISNUll(ti.Is_External_WorkItem,0)<>1   
    ORDER BY ti.task_due_date ASC    



END 

GO
