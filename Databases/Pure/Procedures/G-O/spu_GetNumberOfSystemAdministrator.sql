SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetNumberOfSystemAdministrator'
GO

CREATE PROCEDURE spu_GetNumberOfSystemAdministrator
	@SecurityMode int
AS
--****************************************************************************
-- Revision 	Description of Modification 		Date 		Who 
-- --------		--------------------------- 		----------	--- 
-- 1.0 			Created 							16/12/2004	AG
--****************************************************************************

IF @SecurityMode = 0
BEGIN
	SELECT isnull(count(DISTINCT U.user_id),0) AS NumberOfSystemAdministrator  
	FROM PMUser U 
	INNER JOIN PMUser_Group_User UGU 
	ON U.user_id = UGU.user_id AND isnull(U.secure_password,'') <> '' AND U.is_deleted = 0
	AND U.effective_date <= getdate()
	INNER JOIN pmuser_group UG
	ON ug.pmuser_group_id = ugu.pmuser_group_id
	AND UG.is_sys_admin_group = 1
	AND UG.is_deleted = 0
	AND UG.effective_date <= getdate()
END

IF @SecurityMode = 1
BEGIN
	SELECT isnull(count(DISTINCT U.user_id),0) AS NumberOfSystemAdministrator  
	FROM PMUser U 
	INNER JOIN PMUser_Group_User UGU 
	ON U.user_id = UGU.user_id  AND U.is_deleted = 0
	AND (isnull(U.alternative_identifier,'') <> '' OR ( isnull(U.alternative_identifier,'') = '' AND isnull(U.secure_password,'') <> '')) 
	AND U.effective_date <= getdate()
	INNER JOIN pmuser_group UG
	ON ug.pmuser_group_id = ugu.pmuser_group_id
	AND UG.is_sys_admin_group = 1
	AND UG.is_deleted = 0
	AND UG.effective_date <= getdate()
END	

IF @SecurityMode = 2
BEGIN
	SELECT isnull(count(DISTINCT U.user_id),0) AS NumberOfSystemAdministrator 
	FROM PMUser U 
	INNER JOIN PMUser_Group_User UGU 
	ON U.user_id = UGU.user_id AND U.is_deleted = 0
	AND U.effective_date <= getdate()
	AND isnull(U.alternative_identifier,'') <> ''
	INNER JOIN pmuser_group UG
	ON ug.pmuser_group_id = ugu.pmuser_group_id
	AND UG.is_sys_admin_group = 1
	AND UG.is_deleted = 0
	AND UG.effective_date <= getdate()
END

GO
