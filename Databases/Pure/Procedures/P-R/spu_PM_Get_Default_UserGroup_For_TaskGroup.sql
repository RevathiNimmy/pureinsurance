SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXEC DDLDropProcedure 'spu_PM_Get_Default_UserGroup_For_TaskGroup'
GO

CREATE PROCEDURE spu_PM_Get_Default_UserGroup_For_TaskGroup
    @user_id INT,
    @task_group_id INT
AS
BEGIN
    SELECT
        uga.pmuser_group_id
    FROM pmuser_group_activity uga
    JOIN pmuser_group_user ugu
    	ON ugu.pmuser_group_id = uga.pmuser_group_id
    WHERE uga.pmwrk_task_group_id = @task_group_id 
    AND ugu.user_id = @user_id
    ORDER BY uga.pmuser_group_id
END
GO

