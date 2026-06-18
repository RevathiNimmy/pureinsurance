SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_get_lookups_deleted'
GO
/*******************************************************************************************************/
/* spu_pm_get_lookups_deleted selects the deleted entries in a PMLookup table.                         */
/* RFC13/11/2002                                                                                       */
/*******************************************************************************************************/
CREATE PROCEDURE spu_pm_get_lookups_deleted
    @tablename varchar(255),
    @language_id  integer
AS
BEGIN
    DECLARE @SQL varchar(2000)
    DECLARE @Underwriting char(1)	


    SELECT @Underwriting = value
    FROM hidden_options
    WHERE branch_id=1 and option_number=1

    SELECT @sql =        "SELECT tn." + @tablename + "_id, "
    SELECT @sql = @sql + "       cap.caption, "
    SELECT @sql = @sql + "       RTRIM(tn.code) 'code' "
    SELECT @sql = @sql + "       FROM " + @tablename + " tn "
    SELECT @sql = @sql + "       inner join pmcaption cap "
    SELECT @sql = @sql + "       on tn.caption_id = cap.caption_id "
    SELECT @sql = @sql + "       WHERE tn.is_deleted = 1 "
    SELECT @sql = @sql + "         AND cap.language_id = " + convert(varchar(20),@language_id) + " "

    /* If we are Underwriting then Order the results */
    IF @Underwriting = 'U'
        SELECT @sql = @sql + "       ORDER BY cap.caption_id"
    
--    PRINT (@sql)

    EXECUTE (@SQL)

END
GO

