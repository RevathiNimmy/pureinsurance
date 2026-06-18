SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_UDL_Max_Version'
GO

CREATE  PROCEDURE spu_Get_UDL_Max_Version  
	@table varchar(30)
AS  
  
DECLARE @VERSION INT,
		@V_SQL nvarchar(400)  

SET @V_SQL = 'SELECT @Version= MAX(udl_version) FROM ' + @table 
EXEC SP_EXECUTESQL @V_SQL,N'@Version INT OUTPUT',@VERSION OUTPUT

Select @VERSION


