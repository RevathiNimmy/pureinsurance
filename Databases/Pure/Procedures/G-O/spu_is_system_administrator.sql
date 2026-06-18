SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_is_system_administrator'
GO

CREATE PROCEDURE spu_is_system_administrator
	@user_id INT,
	@security_model INT,
	@effective_date DATETIME
--****************************************************************************
-- Revision 	Description of Modification 		Date 		Who 
-- --------		--------------------------- 		----------	--- 
-- 1.0 			Created 							16/12/2004	AG
--****************************************************************************
	
AS

BEGIN

if @security_model = 0

BEGIN

	SELECT  ISNULL(SUM(ug.is_sys_admin_group), 0) AS sys_admin_count  
	FROM PMUser U 
	INNER JOIN PMUser_Group_User UGU 
	ON U.user_id = UGU.user_id AND isnull(U.password,'') <> '' AND U.is_deleted = 0 AND U.user_id = @user_id
	AND U.effective_date <= @effective_date
	INNER JOIN pmuser_group UG
	ON ug.pmuser_group_id = ugu.pmuser_group_id
	AND UG.is_deleted = 0
	AND UG.effective_date <= @effective_date

END

if @security_model = 1
BEGIN
	SELECT ISNULL(SUM(ug.is_sys_admin_group), 0) AS sys_admin_count  
	FROM PMUser U 
	INNER JOIN PMUser_Group_User UGU 
	ON U.user_id = UGU.user_id  AND U.is_deleted = 0 AND U.user_id = @user_id
	AND (isnull(U.alternative_identifier,'') <> '' OR ( isnull(U.alternative_identifier,'') = '' AND isnull(U.password,'') <> '')) 
	AND U.effective_date <= @effective_date
	INNER JOIN pmuser_group UG
	ON ug.pmuser_group_id = ugu.pmuser_group_id
	AND UG.is_deleted = 0
	AND UG.effective_date <= @effective_date

END

if @security_model = 2
BEGIN
	SELECT ISNULL(SUM(ug.is_sys_admin_group), 0) AS sys_admin_count 
	FROM PMUser U 
	INNER JOIN PMUser_Group_User UGU 
	ON U.user_id = UGU.user_id AND U.is_deleted = 0 AND U.user_id = @user_id
	AND U.effective_date <= @effective_date
	AND isnull(U.alternative_identifier,'') <> ''
	INNER JOIN pmuser_group UG
	ON ug.pmuser_group_id = ugu.pmuser_group_id
	AND UG.is_deleted = 0
	AND UG.effective_date <= @effective_date

END

END
GO
