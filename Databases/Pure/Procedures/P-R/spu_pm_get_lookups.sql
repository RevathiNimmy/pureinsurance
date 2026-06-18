SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_pm_get_lookups'
GO
/*******************************************************************************************************/
/* spu_pm_get_lookups selects the entries in a PMLookup table, based on the effective_date.            */
/* RFC13/11/2002                                                                                       */
/*******************************************************************************************************/
CREATE PROCEDURE spu_pm_get_lookups
    @tablename varchar(255),
    @effective_date datetime,
    @language_id  integer,
    @id integer = NULL
AS
BEGIN
    DECLARE @SQL varchar(2000)
    DECLARE @Underwriting char(1)	


    IF @id IS NULL BEGIN
    
        SELECT @Underwriting = value
        FROM hidden_options
        WHERE branch_id=1 and option_number=1

        SELECT @sql =        "SELECT tn." + @tablename + "_id, "
        SELECT @sql = @sql + "       cap.caption, "
        SELECT @sql = @sql + "       RTRIM(tn.code) 'code' "
        SELECT @sql = @sql + "       FROM " + @tablename + " tn "
        SELECT @sql = @sql + "       inner join pmcaption cap "
        SELECT @sql = @sql + "       on tn.caption_id = cap.caption_id "
        SELECT @sql = @sql + "       WHERE tn.is_deleted = 0 "
        SELECT @sql = @sql + "         AND tn.effective_date <= '" + convert(varchar(20),@effective_date,120) + "' "
        SELECT @sql = @sql + "         AND cap.language_id = " + convert(varchar(20),@language_id) + " "

        /* If we are Underwriting then Order the results */
	IF @Underwriting = 'U'
            SELECT @sql = @sql + "       ORDER BY cap.caption_id"
    
    END ELSE BEGIN

        SELECT @sql =        "SELECT tn." + @tablename + "_id, "
        SELECT @sql = @sql + "       cap.caption, "
        SELECT @sql = @sql + "       RTRIM(tn.code) 'code' "
        SELECT @sql = @sql + "       FROM " + @tablename + " tn "
        SELECT @sql = @sql + "       inner join pmcaption cap "
        SELECT @sql = @sql + "       on tn.caption_id = cap.caption_id "
        SELECT @sql = @sql + "       WHERE tn." + @tablename + "_id = " + convert(varchar(20),@id) + " "
        SELECT @sql = @sql + "         AND cap.language_id = " + convert(varchar(20),@language_id) + " "

    END

--    PRINT (@sql)

    EXECUTE (@SQL)

END
GO

