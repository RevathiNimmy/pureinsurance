SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Audit_Trail_UserList'
GO

CREATE PROCEDURE spu_Get_Audit_Trail_UserList
   
AS
SELECT DISTINCT Pmuser.username 'Username',    
 Pmuser.user_id    'UserId'
 FROM  configuration_audit_master   CAM  
 INNER  JOIN  (SELECT user_id, username FROM PMUser UNION SELECT -1, 'PB2 User') Pmuser ON CAM.UserId = pmuser.user_id  
GO
