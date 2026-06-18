EXECUTE DDLDropProcedure 'spu_SAM_Get_Scheduled_Tasks'
GO

CREATE   PROCEDURE spu_SAM_Get_Scheduled_Tasks
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
    @PartyCnt INT = NULL,
                @ReferenceNumber VARCHAR(30)=NULL
AS

DECLARE @extracted_pmuser_group_id INT
DECLARE @ISAdmin BIT = 0

IF @RestrictTaskList IS NULL SELECT @RestrictTaskList =-1
IF @source_id IS NULL SELECT @source_id =-1

CREATE TABLE #temp_user_group
    (
        pmuser_group_id INT
    )
CREATE INDEX temp_user_group__pmuser_group_id ON #temp_user_group (pmuser_group_id)

IF Len(@ReferenceNumber)>0 
BEGIN

                CREATE TABLE #temp_user_group_users
    (
                                                pmuser_group_id INT,
                                                is_supervisor BIT DEFAULT 0
                                )
    CREATE INDEX #temp_user_group_users_id ON #temp_user_group_users (pmuser_group_id)
                
        WHILE @pmuser_group_string <> ''
        BEGIN

            SELECT @extracted_pmuser_group_id = CAST(LEFT(@pmuser_group_string, CHARINDEX('|', @pmuser_group_string) - 1) AS INT)

            INSERT INTO #temp_user_group_users
            (
                pmuser_group_id
            )
            VALUES
            (
                @extracted_pmuser_group_id
            )

            SELECT @pmuser_group_string = RIGHT(@pmuser_group_string, LEN(@pmuser_group_string)  - CHARINDEX('|', @pmuser_group_string))

        END
    

                                CREATE TABLE #temp_users
                                                (
                                                                tempuser_id INT,
        pmuser_group_id INT
    )
                                CREATE INDEX temp_users_tempuser_id ON #temp_users (tempuser_id)

                                INSERT INTO #temp_users
                                   (
              tempuser_id,
              pmuser_group_id
                                                )
       SELECT  DISTINCT user_id,pmuser_group_id  FROM PMUser_Group_User WHERE pmuser_group_id in
       ( SELECT pmuser_group_id FROM PMUser_Group_User WHERE is_supervisor=1 and user_id=@user_id)

       DECLARE @Record_Count INT
       SELECT @Record_Count=COUNT(0) FROM PMWrk_Task_Instance WHERE description like '%'+@ReferenceNumber+'%'
       
       INSERT INTO #temp_users
                     (
                           tempUser_id,
                           pmuser_group_id
                     )
       SELECT @user_id,pmuser_group_id FROM #temp_user_group_users WHERE pmuser_group_id not in (SELECT pmuser_group_id FROM PMUser_Group_User WHERE is_supervisor=1)
                   
                   IF EXISTS (SELECT NULL FROM PMUser_Group_User WHERE user_id=@user_id and pmuser_group_id =(SELECT pmuser_group_id  FROM PMUser_Group WHERE  code = 'SYSADMIN' AND is_deleted=0) ) 
       BEGIN
              SET @ISAdmin = 1
       END

       UPDATE  tgu SET tgu.is_supervisor=pgu.is_supervisor
       FROM PMUser_Group_User pgu ,#temp_user_group_users tgu WHERE pgu.is_supervisor=1 and pgu.user_id=@user_id and pgu.pmuser_group_id=tgu.pmuser_group_id
                
                
                IF @Record_Count>0
                BEGIN
                                
                SELECT  
        ti.pmwrk_task_instance_cnt,
        ti.is_urgent,
        ti.task_status,
       (CASE t.type_of_task
  WHEN 0 THEN 'Memo'
  WHEN 1 THEN 'Non-Navigator Function'
  WHEN 2 THEN 'Navigator Process'
END)As Type_of_Task,
        t.is_system_task,
        ti.task_due_date,
        ti.customer,
	    ss.code As Branch  ,
        ti.description,	
        ti.pmuser_group_id,
        ti.user_id,
        t.pmnav_process_id,
        t.component_object_name,
        t.component_class_name,
        t.display_icon,
        t.is_view_only_task,
        t.linked_object_name,
        t.linked_class_name,
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
                c.shortname
            FROM pmwrk_task_inst_key v
            JOIN party c
                ON c.party_cnt = v.key_value
            WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt
            AND v.pmnav_key_id = 2
        ) 'shortname',
        ti.party_cnt PartyCnt, p.shortname PartyName,

        pug.Code As UserGroupCode,
        pug.description as UserGroupDescription,
pu.username as UserDescription,
ti.pmwrk_task_group_id 'TaskGroupKey',
ti.pmwrk_task_id 'TaskKey',
                     ( SELECT
                MAX(Inf.insurance_folder_cnt ) AS insurance_folder_cnt
            FROM pmwrk_task_inst_key v
            JOIN Insurance_File Inf
                ON Inf.insurance_file_cnt= CASE WHEN ISNUMERIC(v.key_value)=1 THEN v.key_value END
            WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt AND v.pmnav_key_id in (38,39)) AS InsuranceFolderKey

    FROM pmwrk_task_instance ti
    JOIN pmwrk_task t
        ON t.pmwrk_task_id = ti.pmwrk_task_id
		LEFT JOIN  Source ss 
	ON ss.source_id = ti.source_id
    JOIN #temp_user_group_users ug
        ON (ug.pmuser_group_id = ti.pmuser_group_id) 
    JOIN pmuser_group pug
        ON pug.pmuser_group_id = ti.pmuser_group_id
    LEFT JOIN pmuser pu
        ON pu.user_id = ti.user_id
    LEFT JOIN party p 
        ON (p.party_cnt = ti.party_cnt)
  
                WHERE ti.description like '%'+@ReferenceNumber+'%'
                AND  (@ISAdmin = 1 OR (ti.user_id IN(SELECT tempUser_id FROM #temp_users WHERE ti.pmuser_group_id=pmuser_group_id) OR ti.user_id IS NULL))


                END

   DROP TABLE #temp_users
   DROP TABLE #temp_user_group_users
END

ELSE IF @is_system_task = 0
BEGIN

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

    SELECT
        ti.pmwrk_task_instance_cnt,
        ti.is_urgent,
        ti.task_status,
       (CASE t.type_of_task
  WHEN 0 THEN 'Memo'
  WHEN 1 THEN 'Non-Navigator Function'
  WHEN 2 THEN 'Navigator Process'
 END)As Type_of_Task,
        t.is_system_task,
        ti.task_due_date,
        ti.customer,
		ss.code As Branch  ,
        ti.description,
        ti.pmuser_group_id,
        ti.user_id,
        t.pmnav_process_id,
        t.component_object_name,
        t.component_class_name,
        t.display_icon,
        t.is_view_only_task,
        t.linked_object_name,
        t.linked_class_name,
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
                c.shortname
            FROM pmwrk_task_inst_key v
            JOIN party c
                ON c.party_cnt = CASE WHEN ISNUMERIC(v.key_value)=1 THEN v.key_value END
            WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt AND CHARINDEX('.', key_value) =0
            AND v.pmnav_key_id = 2
        ) 'shortname',
        ti.party_cnt PartyCnt, p.shortname PartyName,
        pug.Code As UserGroupCode,
        pug.description as UserGroupDescription,
 pu.username as UserDescription,
ti.pmwrk_task_group_id 'TaskGroupKey',
ti.pmwrk_task_id 'TaskKey',
			( SELECT
                MAX(Inf.insurance_folder_cnt ) AS insurance_folder_cnt
            FROM pmwrk_task_inst_key v
            JOIN Insurance_File Inf
                ON Inf.insurance_file_cnt= CASE WHEN ISNUMERIC(v.key_value)=1 THEN v.key_value END
		WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt AND CHARINDEX('.', key_value) =0 AND v.pmnav_key_id in (38,39)) AS InsuranceFolderKey

    FROM pmwrk_task_instance ti
    JOIN pmwrk_task t
        ON t.pmwrk_task_id = ti.pmwrk_task_id
    LEFT JOIN Source ss 
		ON ss.source_id = ti.source_id  
    LEFT JOIN #temp_user_group ug
	    ON (ug.pmuser_group_id = ti.pmuser_group_id)
  LEFT JOIN pmuser_group pug
        ON pug.pmuser_group_id = ti.pmuser_group_id
    LEFT JOIN pmuser pu
        ON pu.user_id = ti.user_id
LEFT JOIN party p 
        ON (p.party_cnt = ti.party_cnt)

    WHERE
        (
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
               --     OR
               -- (ug.pmuser_group_id = ti.original_pmuser_group_id AND ti.is_task_review = 1)
                )
                  AND (@task_due_date_limit IS NULL OR (ti.task_due_date >= convert(date, getdate()) AND ti.task_due_date <= ISNULL(@task_due_date_limit, ti.task_due_date)))
                AND t.is_system_task = 0
                AND ug.pmuser_group_id IS NOT NULL
            )
        )
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
		AND ISNULL(ti.source_id, 0) NOT IN(SELECT source_id
                             FROM   pmuser_source
                             WHERE  [user_id] = @user_id) 
 --Start (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR33 - Work Manager - Get Tasks.doc)
    --AND t.type_of_task <> 2
--End(Ravikumar Pasupuleti) - (Tech Spec - UIIC WR33 - Work Manager - Get Tasks.doc)
AND (ti.party_cnt=@PartyCnt OR @PartyCnt IS NULL) And ISNULL(ti.Is_External_WorkItem,0)<>1
    ORDER BY ti.task_due_date ASC

    DROP TABLE #temp_user_group

END
ELSE
BEGIN
    SELECT
        ti.pmwrk_task_instance_cnt,
        ti.is_urgent,
        ti.task_status,
       (CASE t.type_of_task
  WHEN 0 THEN 'Memo'
  WHEN 1 THEN 'Non-Navigator Function'
  WHEN 2 THEN 'Navigator Process'
 END)As Type_of_Task,
        t.is_system_task,
        ti.task_due_date,
        ti.customer,
		ss.code As Branch  ,
        ti.description,
        ti.pmuser_group_id,
        ti.user_id,
        t.pmnav_process_id,
        t.component_object_name,
        t.component_class_name,
        t.display_icon,
        t.is_view_only_task,
        t.linked_object_name,
        t.linked_class_name,
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
                c.shortname
            FROM pmwrk_task_inst_key v
            JOIN party c
                ON c.party_cnt = CASE WHEN ISNUMERIC(v.key_value)=1 THEN v.key_value END
            WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt AND CHARINDEX('.', key_value) =0
            AND v.pmnav_key_id = 2
        ) 'shortname',
        ti.party_cnt PartyCnt, p.shortname PartyName,
        pug.Code As UserGroupCode,
        pug.description as UserGroupDescription,
 pu.username as UserDescription ,
 ti.pmwrk_task_group_id 'TaskGroupKey',
ti.pmwrk_task_id 'TaskKey',
			( SELECT
                MAX(Inf.insurance_folder_cnt ) AS insurance_folder_cnt
            FROM pmwrk_task_inst_key v
            JOIN Insurance_File Inf
                ON Inf.insurance_file_cnt= CASE WHEN ISNUMERIC(v.key_value)=1 THEN v.key_value END
	WHERE v.pmwrk_task_instance_cnt = ti.pmwrk_task_instance_cnt AND CHARINDEX('.', key_value) =0 AND v.pmnav_key_id in (38,39)) AS InsuranceFolderKey
    FROM pmwrk_task t
    JOIN pmwrk_task_instance ti
        ON ti.pmwrk_task_id = t.pmwrk_task_id
		JOIN Source ss 
	ON ss.source_id = ti.source_id
   LEFT JOIN pmuser_group pug
        ON pug.pmuser_group_id = ti.pmuser_group_id
   LEFT JOIN pmuser pu
        ON pu.user_id = ti.user_id
          LEFT JOIN party p 
        ON (p.party_cnt = ti.party_cnt)


    WHERE t.is_system_task = 1



     AND (@task_due_date_limit IS NULL OR (ti.task_due_date >= convert(date, getdate()) AND ti.task_due_date <= ISNULL(@task_due_date_limit, ti.task_due_date)))
        AND (ti.party_cnt=@PartyCnt OR @PartyCnt IS NULL) AND  ISNULL(ti.Is_External_WorkItem,0)<>1
    ORDER BY ti.task_due_date ASC

	DROP TABLE #temp_user_group
END

--End (Ravikumar Pasupuleti) - (Tech Spec - UIIC WR33 - Work Manager - Get Tasks.doc)

