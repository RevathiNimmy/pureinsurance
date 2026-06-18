SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_sys_admin_status'
GO

CREATE PROCEDURE spu_get_sys_admin_status
	@user_id INT,
	@effective_date DATETIME
AS
BEGIN

SELECT ISNULL(SUM(ug.is_sys_admin_group), 0) AS sys_admin_count
FROM pmuser_group ug, pmuser_group_user ugu
WHERE ug.pmuser_group_id = ugu.pmuser_group_id
  AND ug.is_deleted = 0
  AND ug.effective_date <= @effective_date
  AND ugu.user_id = @user_id


END
GO
