SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_WPTableName_DataStructureName'
GO
 
  
CREATE PROCEDURE spu_get_WPTableName_DataStructureName
	@CCMWPFields CCMWPFields READONLY
AS        
        
BEGIN  
	Create table #tempTable
	(TableName VARCHAR(255),
	 ColumnName VARCHAR(255),
	 CCMTable_Name VARCHAR(255)
	)

	insert into #tempTable ( ColumnName , CCMTable_Name , TableName ) 
	SELECT F.column_name , F.Table_Name , WP.Table_Name 
	From @CCMWPFields F
       LEFT JOIN wp_fields WP
       ON WP.DataStructure_Name = F.Table_name  
	   AND WP.column_name= F.Column_Name 

	UPDATE T 
	SET T.TableName = WP.table_name         
	from #tempTable T
	LEFT JOIN wp_fields WP
	ON WP.Table_Name = T.CCMTable_Name
	where T.TableName IS NULL	
	
	UPDATE T
	set T.TableName = 'PolicyStandardWordings'
	from #tempTable T
	LEFT JOIN @CCMWPFields F
	ON F.Table_Name = T.CCMTable_Name
	where F.Table_Name='PolicyStandardWordings'

	UPDATE T
	set T.TableName = WP.Table_Name
	from #tempTable T
	LEFT JOIN wp_fields WP
	ON WP.DataStructure_Name = T.CCMTable_Name
	where WP.specials_type = 5

	SELECT  TableName, ColumnName from #tempTable


END  
GO