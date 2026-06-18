SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_User_Group_Id_For_WrkTask_Instance'
GO

CREATE PROCEDURE spu_Get_User_Group_Id_For_WrkTask_Instance
	@sCode VARCHAR(25)

AS
BEGIN
DECLARE @nUser_Group_Id AS INT
SELECT @nUser_Group_Id =  PMUser_group_Id
FROM PMUser_group
WHERE code = @sCode
AND is_deleted = 0

IF @nUser_Group_Id is null OR @nUser_Group_Id = 0
BEGIN
SELECT @nUser_Group_Id = value
		FROM System_Options
		WHERE option_number = 1022 and branch_id = 1
END

SELECT @nUser_Group_Id
END
GO