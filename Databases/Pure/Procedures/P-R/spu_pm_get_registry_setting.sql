SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_get_registry_setting'
GO
CREATE PROC spu_pm_get_registry_setting  
@UserName VARCHAR(100),
@MachineName VARCHAR(100)
AS  

SELECT rs.KeyPath, rs.KeyName, rs.KeyData, ISNULL(rs.System_Logged_in_User,''), ISNULL(s.system_name,'') 
                         FROM Registry_Setting rs LEFT JOIN PMSystem s ON s.system_id = rs.system_id 
						 WHERE System_Logged_in_User=@UserName OR s.system_name=@MachineName
GO


