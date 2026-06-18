
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_PMLookups_Table_data'
GO

CREATE PROCEDURE spu_SAM_Get_PMLookups_Table_data

   @Tablename  VARCHAR(255),
   @effective_date Date = NULL,
   @WhereClause varchar(255) = NULL
AS
BEGIN
	DECLARE @SQL VARCHAR(300)

	SELECT @SQL = 'SELECT * FROM ' +    @Tablename

	IF @effective_date IS NOT NULL AND @Tablename like 'UDL%'
	BEGIN
	 SELECT @sql = @sql + ' WHERE ' +' udl_version  = (SELECT max(udl_version) FROM ' +@tableName + ' WHERE Effective_date <='''+ convert(varchar, @effective_date,106)  + ''')'
	END
	Else IF @effective_date IS NOT NULL 
	BEGIN
	 SELECT @sql = @sql + ' WHERE Effective_date <='''+ convert(varchar, @effective_date,106)  + ''''
	END
    ----------------------------------------------------
IF(@WhereClause IS NOT NULL)
    BEGIN 
    IF @effective_date IS NOT NULL
        SELECT @sql = @sql +  @WhereClause
        else
        
    Set @WhereClause=REPLACE(@WhereClause, 'AND', '')

 

        SELECT @sql = @sql + ' WHERE '+ @WhereClause
    END
-----------------------------------------------------
	EXECUTE(@SQL)



END
   

GO
  

