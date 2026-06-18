SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Lookup_Values_By_Effective_Date'
GO



CREATE PROCEDURE spu_SIR_Get_Lookup_Values_By_Effective_Date   
 @table varchar(40),
 @Id 	INT = NULL    
AS    
    
BEGIN    
    
 DECLARE @sql varchar(200)    
    
 SET @sql =''    
 SET @sql =@sql + ' Select ' + @table + '_id,  description,code from ' + @table    
 SET @sql =@sql + ' where is_deleted = 0'    
 SET @sql =@sql + ' and effective_date <= GetDate()' 
 	IF NOT ISNULL(@ID,0) = 0 
	BEGIN
		SET @sql = @sql + ' AND ' + @table + '_id = ' + CONVERT(VARCHAR(10),@id)  			
	END  
 SET @sql =@sql + ' order by description'    
    
 EXEC (@sql)    
    
END    
  
  
  






GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
