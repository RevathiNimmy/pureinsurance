EXECUTE DDLDropProcedure 'spu_Get_Table_Data'
GO
CREATE PROCEDURE spu_Get_Table_Data  
	@sTableName VARCHAR(70),
	@sColumns VARCHAR(200),
	@sCondition VARCHAR(300)
AS  

BEGIN

Declare @sSQL varchar(600)

	SELECT @sSQL = 'SELECT ' + @sColumns + 
				  ' FROM ' + @sTableName +
				  ' WHERE ' + @sCondition
	  
	  execute(@sSQL)
END  

Go
