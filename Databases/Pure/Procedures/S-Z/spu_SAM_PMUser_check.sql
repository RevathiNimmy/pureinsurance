SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_PMUser_check'
GO
-- Looks up a user name in the user table and returns all relevant information for use by SA.NET security code.  
--  
-- Parameters:  
--  @username   User name  
--  <others>    User data if valid user, NULL if not  
--  
CREATE PROCEDURE spu_SAM_PMUser_check  
    @username varchar(255),
	@AgentKey INT=0,
    @password varchar(30) output,  
    @user_id smallint output,  
    @language_id smallint output,  
    @email_address varchar(255) output,  
    @party_cnt integer output,  
    @party_type_code char(10) output  
AS BEGIN  
    SET NOCOUNT ON  
  
    DECLARE @effective_date datetime  
    SELECT @effective_date = GETDATE()  
  
    SELECT  
        @password = NULL,  
        @user_id = NULL,  
        @language_id = NULL,  
        @email_address = NULL,  
        @party_cnt = NULL,  
        @party_type_code = NULL  
  
    SELECT  
        @password = PMUser.password,  
        @user_id = PMUser.user_id,  
        @language_id = PMUser.language_id,  
        @email_address = PMUser.email_address,  
        @party_cnt = PMUser.party_cnt,  
        @party_type_code = Party_Type.code  
        FROM PMUser  
        LEFT OUTER JOIN Party ON PMUser.party_cnt = Party.party_cnt  
        LEFT OUTER JOIN Party_Type on Party.party_type_id = Party_Type.party_type_id  
        WHERE PMUser.username = @username  
        AND PMUser.is_deleted = 0  
        AND PMUser.effective_date <= @effective_date  
		AND (@AgentKey=0 OR PMUser.party_cnt=@AgentKey)
  
    SELECT DISTINCT RTRIM(PMWrk_Task.code) As task_code  
        FROM PMUser  
        INNER JOIN PMUser_Group_User ON PMUser.user_id = PMUser_Group_User.user_id  
        INNER JOIN PMUser_Group ON PMUser_Group.pmuser_group_id = PMUser_Group_User.pmuser_group_id  
        INNER JOIN PMUser_Group_Activity ON PMUser_Group.pmuser_group_id = PMUser_Group_Activity.pmuser_group_id  
        INNER JOIN PMWrk_Task_Group ON PMWrk_Task_Group.pmwrk_task_group_id = PMUser_Group_Activity.pmwrk_task_group_id  
        INNER JOIN PMWrk_Task_Group_Task ON PMWrk_Task_Group.pmwrk_task_group_id = PMWrk_Task_Group_Task.pmwrk_task_group_id  
        INNER JOIN PMWrk_Task ON PMWrk_Task.pmwrk_task_id = PMWrk_Task_Group_Task.pmwrk_task_id  
        WHERE PMUser.user_id = @user_id  
        AND PMUser_Group.is_deleted = 0  
        AND PMUser_Group.effective_date <= @effective_date  
        AND PMWrk_Task_Group.is_deleted = 0  
        AND PMWrk_Task_Group.effective_date <= @effective_date  
        AND PMWrk_Task.is_deleted = 0  
        AND PMWrk_Task.effective_date <= @effective_date  
END  
GO

