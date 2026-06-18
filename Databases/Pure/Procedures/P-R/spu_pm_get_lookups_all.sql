SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_pm_get_lookups_all'  
GO

CREATE PROCEDURE spu_pm_get_lookups_all  
 @tablename varchar(255),  
 @language_id  integer,  
 @parentkeyfield varchar(255) = NULL,  
 @excludedeletedrecords bit = 0,  
 @excludeeffectivedate bit = 0,  
 @parentfield varchar(255)= NULL,  
 @parentvalue integer= 0,  
 @effectiveDate Datetime = NULL,  
 @version integer= 0,
 @WhereClause varchar(255) = NULL

AS
BEGIN
 DECLARE @SQL varchar(2000)
 DECLARE @Underwriting char(1)
 DECLARE @ISUDLTable integer

    SELECT @Underwriting = value
    FROM hidden_options
    WHERE branch_id=1 and option_number=1

	SELECT @ISUDLTable =0
    Select @ISUDLTable = 1 from GIS_List_type  WHERE Code = substring(@tablename,5,len(@tablename))

 IF @ISUDLTable = 1
 BEGIN
  IF @version >0 AND @effectiveDate IS NULL
   SET @EffectiveDate = getdate()
 END

 SELECT @sql =        'SELECT tn.' + @tablename + '_id, '
    SELECT @sql = @sql + '       cap.caption, '
    SELECT @sql = @sql + '       RTRIM(tn.code) code, '
    SELECT @sql = @sql + '       tn.effective_date, '
    SELECT @sql = @sql + '       tn.is_deleted  '

    IF NOT (@parentkeyfield = '') AND (Not @parentkeyfield IS  NULL )
    SELECT @sql = @sql + ',' + @parentkeyfield + ' parent_id'
    SELECT @sql = @sql + '       FROM ' + @tablename + ' tn '
    SELECT @sql = @sql + '       inner join pmcaption cap '
    SELECT @sql = @sql + '       on tn.caption_id = cap.caption_id '
    SELECT @sql = @sql + '       WHERE cap.language_id = ' + convert(varchar(20),@language_id) + ' '

IF NOT (@excludedeletedrecords = 0)
SELECT @sql = @sql + 'AND' +'       tn.is_deleted = 0' + ' '

IF NOT (@excludeeffectivedate = 0)
 SELECT @sql = @sql + 'AND' +'       tn.effective_date <= getdate()' + ' '

IF @ISUDLTable = 1
	BEGIN
		IF NOT @version = 0 AND  @EffectiveDate IS NOT NULL
			BEGIN
			 SELECT @sql = @sql + ' AND ' +' tn.udl_version  =' + convert(varchar,@version)
			 SELECT @sql = @sql + ' AND ' +' tn.effective_date <='''+ convert(varchar, @effectiveDate,106)  + ''''
			END
		ELSE
		 SELECT @sql = @sql + ' AND ' +' tn.udl_version  = (SELECT max(udl_version) FROM ' +@tableName + ' WHERE Effective_date <='''+ convert(varchar, @effectiveDate,106)  + ''')'
	END
ELSE
	BEGIN
		IF   (@EffectiveDate IS NOT NULL AND UPPER(@tablename)='RI_OVERRIDE_REASON')
			SELECT @sql = @sql + ' AND ' +' convert(Date,tn.effective_date) <='''+ convert(varchar, @effectiveDate,106)  + ''''
	END

-----------------------------------------------------
IF(@WhereClause IS NOT NULL)
	BEGIN 
		SELECT @sql = @sql + @WhereClause
	END
-----------------------------------------------------


--IF (NOT @parentvalue = 0)
IF  (@parentfield IS NOT NULL) AND (NOT @parentvalue = 0)
 SELECT @sql = @sql + ' AND ' +       ' tn.' + @parentfield + ' = ' + convert(varchar(20),@parentvalue)  + ' '

    /* If we are Underwriting then Order the results */

    IF @Underwriting = 'U'
        SELECT @sql = @sql + ' ORDER BY cap.caption_id'

    PRINT (@sql)

    EXECUTE (@SQL)

END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
