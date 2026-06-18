SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

DDLDropProcedure 'spu_Check_Lookup_Linked_Data_Mandatory'
GO

CREATE PROCEDURE spu_Check_Lookup_Linked_Data_Mandatory
                @lookup_table_name          VARCHAR(255),
                @linked_data_mandatory BIT  OUTPUT
AS
  BEGIN
  
    DECLARE  @linked_data_table_name VARCHAR(255)
    
    DECLARE  @sql VARCHAR(255)
                  
    SET @linked_data_mandatory = 1
                                      
    CREATE TABLE #TempTable (
      Linked_Data_Mandatory INT)
    
    SELECT @linked_data_table_name = Linked_data_table_name
    FROM   PMProduct_Lookup
    WHERE  lookup_table_name = @lookup_table_name
           AND Linked_data_mandatory = 1
                                       
    SET @sql = 'INSERT INTO #TempTable SELECT ' + @lookup_table_name + '_id  FROM ' + @lookup_table_name + ' WHERE ' + @lookup_table_name + '_id NOT IN('
    
    SET @sql = @sql + 'SELECT DISTINCT ' + @lookup_table_name + '_id FROM ' + @Linked_data_table_name + ') AND is_deleted=0'
                                                                                                        
    EXEC (@sql)
    
    IF EXISTS (SELECT *
               FROM   #TempTable)
      SET @linked_data_mandatory = 0
    ELSE
      SET @linked_data_mandatory = 1
                                        
    DROP TABLE #TempTable
    
  END

GO