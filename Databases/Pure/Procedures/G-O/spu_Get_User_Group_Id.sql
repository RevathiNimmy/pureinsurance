
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_User_Group_Id'
GO


CREATE PROCEDURE spu_Get_User_Group_Id
	@Code varchar(25)
   
AS


SELECT PMUser_group_Id
FROM PMUser_group 
where code = @Code

GO

