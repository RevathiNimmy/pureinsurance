SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAN_PMWrk_Task_check'
GO
-- Authorise a user against the requested PMWrk_Task code.
--
-- Parameters:
--  @user_id    User ID to check
--  @task_code  Task code to check
--  @exists     0 = access denied, 1 = access granted
--
CREATE PROCEDURE dbo.spu_SAN_PMWrk_Task_check
    @user_id integer,
    @task_code char(10),
    @exists bit output
AS BEGIN
    SET NOCOUNT ON

    SELECT @exists = CASE WHEN EXISTS (
        SELECT NULL
        FROM PMUser WITH(NOLOCK)
        INNER JOIN PMUser_Group_User WITH(NOLOCK) ON PMUser.user_id = PMUser_Group_User.user_id
        INNER JOIN PMUser_Group WITH(NOLOCK) ON PMUser_Group.pmuser_group_id = PMUser_Group_User.pmuser_group_id
        INNER JOIN PMUser_Group_Activity WITH(NOLOCK) ON PMUser_Group.pmuser_group_id = PMUser_Group_Activity.pmuser_group_id
        INNER JOIN PMWrk_Task_Group WITH(NOLOCK) ON PMWrk_Task_Group.pmwrk_task_group_id = PMUser_Group_Activity.pmwrk_task_group_id
        INNER JOIN PMWrk_Task_Group_Task WITH(NOLOCK) ON PMWrk_Task_Group.pmwrk_task_group_id = PMWrk_Task_Group_Task.pmwrk_task_group_id
        INNER JOIN PMWrk_Task WITH(NOLOCK) ON PMWrk_Task.pmwrk_task_id = PMWrk_Task_Group_Task.pmwrk_task_id
        WHERE PMUser.user_id = @user_id
        AND PMUser_Group.is_deleted = 0
        AND PMUser_Group.effective_date <= GETDATE()
        AND PMWrk_Task_Group.is_deleted = 0
        AND PMWrk_Task_Group.effective_date <= GETDATE()
        AND PMWrk_Task.is_deleted = 0
        AND PMWrk_Task.effective_date <= GETDATE()
        AND RTRIM(PMWrk_Task.code) = @task_code
    ) THEN 1 ELSE 0 END
END
GO
