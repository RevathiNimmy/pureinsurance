--Start(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.3.1) 
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Specific_User_Authority_Sel'
GO
CREATE PROCEDURE spu_Specific_User_Authority_Sel 
	@user_id 	INT,
	@Authority  	VARCHAR(50)
 
AS 

BEGIN
DECLARE @SQL VARCHAR(100)
			

SET @SQL = 'SELECT ' + @Authority + ' FROM User_Authorities WHERE user_id=' + Convert(varchar(10),@user_id)
			EXEC(@SQL)
END

--End(Saurabh Agrawal) Tech Spec WR3 User Level RI Display Restriction - (5.3.1) 



