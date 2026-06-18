SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_Get_Default_Values'  
GO

CREATE PROCEDURE spu_SAM_Get_Default_Values  
@table_name  VARCHAR(100),  
@column_name1  VARCHAR(100),   
@value1  VARCHAR(50),  
@column_name2  VARCHAR(100) = NULL,  
@value2  VARCHAR(50) = NULL  
  
  
AS  
  
DECLARE @SQL VARCHAR(2000)  
IF ISNULL(CONVERT(VARCHAR(100),@column_name2),'0') = '0'  
  BEGIN  
   SELECT @SQL = "SELECT * FROM " + @table_name + " WHERE "  
   SELECT @SQL = @SQL  + @column_name1 + " = " + @value1  
     
   EXECUTE(@SQL)  
  END  
ELSE   
 IF ISNULL(CONVERT(VARCHAR(50),@column_name2),'0') <> '0'   
   
 BEGIN  
   --PRINT 'AA'   
   SELECT @SQL = "SELECT * FROM " + @table_name + " WHERE "  
   SELECT @SQL = @SQL  + @column_name1 + " = " + CONVERT(VARCHAR(50),@value1)   
   SELECT @SQL = @SQL  + " AND " + @column_name2 + " = " + CONVERT(VARCHAR(50),@value2)   
   
     
   EXECUTE(@SQL)  
 END  
  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
