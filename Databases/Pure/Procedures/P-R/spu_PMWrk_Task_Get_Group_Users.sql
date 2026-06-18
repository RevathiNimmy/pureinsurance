SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_PMWrk_Task_Get_Group_Users'
GO

CREATE PROCEDURE spu_PMWrk_Task_Get_Group_Users
	@UserGroupID INT
AS
BEGIN

SELECT u.[user_id], u.username 
FROM PMUser u, PMUser_Group_User ugu
WHERE u.[user_id] = ugu.[user_id]
  AND ugu.pmuser_group_id = @UserGroupID

END
GO
