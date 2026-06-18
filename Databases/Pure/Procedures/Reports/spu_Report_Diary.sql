SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Report_Diary'
GO

CREATE PROCEDURE spu_Report_Diary
    @username VARCHAR(255),
    @end_date DATETIME
AS

CREATE TABLE #tasks
(
    pmwrk_task_instance_cnt INT,
    operator_id INT,
    operator_name VARCHAR(255),
    action_date DATETIME,
    client_name VARCHAR(255),
    policy_id INT,
    policy_no VARCHAR(30),
    description VARCHAR(255),
    logged_by_id INT,
    logged_by_name VARCHAR(255),
    taskstatus VARCHAR(20),
)

/*Get all values from pmwrk_task_instance and if a username has been passed in then restrict the records now*/
IF @username = 'All'
BEGIN
    INSERT INTO #tasks
    (
        pmwrk_task_instance_cnt,
        operator_id,
        logged_by_id,
        action_date,
        description,
        client_name,
        taskstatus
    )
    SELECT 
        ti.pmwrk_task_instance_cnt,
        ti.user_id,
        ti.created_by_id,
        ti.task_due_date,
        ti.description,
        ti.customer,
        CASE task_status           
            WHEN 0 THEN 'New'
            WHEN 1 THEN 'In Progress'
            WHEN 2 THEN 'In Complete'
        END
    FROM pmwrk_task_instance ti
    WHERE ti.task_status <> 3
    AND ti.is_visible = 1
    AND ti.task_due_date <= @end_date
END
ELSE
BEGIN
    INSERT INTO #tasks
    (
        pmwrk_task_instance_cnt,
        operator_id,
        logged_by_id,
        action_date,
        description,
        client_name,
        taskstatus
    )
    SELECT 
        ti.pmwrk_task_instance_cnt,
        ti.user_id,
        ti.created_by_id,
        ti.task_due_date,
        ti.description,
        ti.customer,
        CASE task_status           
            WHEN 0 THEN 'New'
            WHEN 1 THEN 'In Progress'
            WHEN 2 THEN 'In Complete'
        END
    FROM pmwrk_task_instance ti
    JOIN pmuser u 
        ON u.user_id = ti.user_id
        AND u.username = @username
    WHERE ti.task_status <> 3
    AND ti.is_visible = 1
    AND ti.task_due_date <= @end_date
END

/*Add the policy_id (it's quicker converting it to an integer here than joining to insurance_file with one side in a CAST function)*/
UPDATE t
SET t.policy_id = 
        (
            SELECT
                CAST(tik.key_value AS INT)
            FROM pmwrk_task_inst_key tik
            JOIN pmnav_key k 
                ON k.pmnav_key_id = tik.pmnav_key_id
                AND k.name = 'insurance_file_cnt'
            WHERE tik.pmwrk_task_instance_cnt = t.pmwrk_task_instance_cnt 
        )
FROM #tasks t

/*Update the remaining fields that are in other tables*/
UPDATE t
SET t.operator_name = 
        (
            SELECT
                username
            FROM pmuser 
            WHERE user_id = t.operator_id
        ),
    t.logged_by_name = 
        (
            SELECT
                username
            FROM pmuser 
            WHERE user_id = t.logged_by_id
        ),
    t.policy_no = 
        (
            SELECT 
                insurance_ref
            FROM insurance_file
            WHERE insurance_file_cnt = t.policy_id 
        )    
FROM #tasks t

/*Select all rows that need to be displayed in the correct order*/
SELECT 
    * 
FROM #tasks
ORDER BY
    operator_name,
    action_date

/*Remove the temporary table*/
DROP TABLE #Tasks

GO