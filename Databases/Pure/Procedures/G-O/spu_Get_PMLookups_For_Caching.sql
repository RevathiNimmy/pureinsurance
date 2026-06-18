SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_PMLookups_For_Caching'
GO

CREATE PROCEDURE spu_Get_PMLookups_For_Caching    
    @Table_Name varchar(255),    
    @language_id int,    
    @Is_Underwriting int    
AS    
BEGIN    
 DECLARE @strSQL varchar(MAX)    
 DECLARE @strSQL1 varchar(MAX)    
 DECLARE @strSQL2 varchar(MAX)   
 DECLARE @column_name varchar(255)    
    
    -- set it to empty string first, else we don't get any results    
    SELECT @strSQL = '', @strSQL1 = '', @strSQL2 = ''    
    
 -- WE NEED THE RESULTING COLUMNS IN THE FOLLOWING COLUMN ORDER IN THE LOOKUPS    
 SET @strSQL1 = 'SELECT'  
                + ' Cast(tn.' +  @Table_Name + '_ID as varchar(10)) as ' + @Table_Name + '_ID,'  
                + ' RTRIM(LTRIM(IsNull(IsNull(cap.caption, tn.description), tn.code))) AS ' + '''' + 'caption' + ''''  + ','  
                + ' tn.code, '  
                + ' Cast(tn.is_deleted as varchar(1)) as is_deleted' + ','  
                + ' tn.effective_date '   -- NOTE THIS WILL BE LAST ENTRY FOR MOST OF THE TABLE, SO NO ','  
   
 SET @strSQL2 =    
 (SELECT  ',' + 'tn.' + '[' + column_name + ']' AS [text()]  
 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Table_Name AND COLUMN_NAME NOT IN (  
 @Table_Name + '_id' ,    
 'description'  ,    
 'code'  ,    
 'is_deleted'  ,    
 'effective_date'  ,    
 'caption'    
 )  
 FOR XML PATH(''))  
   
 SET @strSQL = @strSQL1 + RTRIM(@strSQL2)    
  
 -- MERGE OTHER CRITERIA    
 SET @strSQL = @strSQL +  ' FROM ' + @Table_Name + ' tn LEFT OUTER JOIN pmcaption cap on tn.caption_id = cap.caption_id '    
   +  ' AND (cap.language_id = ' + Cast(@language_id AS varchar(10)) + ') ORDER BY 2 '

EXEC (@strSQL)
    
END

GO
