SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_And_Validate_Field'
GO


CREATE PROCEDURE spu_SAM_Get_And_Validate_Field
  
@table_name varchar(255),  
@field_to_return_name varchar(255),  
@field_to_validate_name varchar(255),  
@field_to_validate_value varchar(255)  
  

AS  
  
 DECLARE @sSQL nvarchar(4000)  
  
 SET @sSQL =''  
 SET @sSQL = 'SELECT ' + @field_to_return_name + ' FROM ' + @table_name  
 SET @sSQL = @sSQL + ' WHERE ' + @field_to_validate_name + ' = '  
 SET @sSQL = @sSQL + '''' + replace(@field_to_validate_value,'''','''''') + ''''
  
 --EXEC(@sSQL)  
 EXEC sp_executesql @sSQL

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
