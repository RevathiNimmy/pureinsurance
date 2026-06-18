DECLARE @table_name VARCHAR(255),
    @unmerged_table_id INT,
    @merged_table_id INT,
    @no_of_columns INT,
    @message VARCHAR(255)

DECLARE @unmerged_column_colid INT,
    @unmerged_column_name VARCHAR(255),
    @unmerged_column_type VARCHAR(255),
    @unmerged_column_computed VARCHAR(3),
    @unmerged_column_length INT,
    @unmerged_column_precision VARCHAR(5),
    @unmerged_column_scale VARCHAR(5),
    @unmerged_column_nullable VARCHAR(8),
    @unmerged_column_trim_trailing_blanks VARCHAR(3),
    @unmerged_column_fixed_len_null_in_source VARCHAR(3)

DECLARE @merged_column_colid INT,
    @merged_column_name VARCHAR(255),
    @merged_column_type VARCHAR(255),
    @merged_column_computed VARCHAR(3),
    @merged_column_length INT,
    @merged_column_precision VARCHAR(5),
    @merged_column_scale VARCHAR(5),
    @merged_column_nullable VARCHAR(8),
    @merged_column_trim_trailing_blanks VARCHAR(3),
    @merged_column_fixed_len_null_in_source VARCHAR(3)

DECLARE @yes VARCHAR(3),
    @no VARCHAR(2)

SELECT  @yes = 'Yes',
    @no = 'No'

DECLARE @numtypes NVARCHAR(80)
SELECT @numtypes = N'tinyint,smallint,decimal,int,real,money,float,numeric,smallmoney'

/*
First let's do the existing tables
*/

DECLARE table_cursor CURSOR FOR
    SELECT  bo.name,
        bo.id,
        mo.id
    FROM    sirius_for_broking..sysobjects bo,
        sirius_merged..sysobjects mo
    WHERE   bo.type = 'U'
    AND mo.type = 'U'
    AND bo.name = mo.name
    ORDER BY 1

OPEN table_cursor

FETCH NEXT FROM table_cursor
    INTO    @table_name,
        @unmerged_table_id,
        @merged_table_id

WHILE   @@FETCH_STATUS = 0
BEGIN

    DECLARE column_cursor CURSOR FOR
        SELECT  mc.name,
            type_name(mc.xusertype),
            case when mc.iscomputed = 0 then @no else @yes end,
            convert(int, mc.length),
            case when charindex(type_name(mc.xtype), @numtypes) > 0
                then convert(varchar(5),mc.xprec)
                else '' end,
            case when charindex(type_name(mc.xtype), @numtypes) > 0
                then convert(varchar(5),OdbcScale(mc.xtype,mc.xscale))
                else '' end,
            case when mc.isnullable = 0 then 'NOT NULL' else 'NULL' end,
            case ColumnProperty(@merged_table_id, mc.name, 'UsesAnsiTrim')
                when 1 then @no
                when 0 then @yes
                else '(n/a)' end,
            case when type_name(mc.xtype) not in ('varbinary','varchar','binary','char')
                Then '(n/a)'
                When mc.status & 0x20 = 0 Then @no
                Else @yes END,
            bc.name,
            type_name(bc.xusertype),
            case when bc.iscomputed = 0 then @no else @yes end,
            convert(int, bc.length),
            case when charindex(type_name(bc.xtype), @numtypes) > 0
                then convert(varchar(5),bc.xprec)
                else '' end,
            case when charindex(type_name(bc.xtype), @numtypes) > 0
                then convert(varchar(5),OdbcScale(bc.xtype,bc.xscale))
                else '' end,
            case when bc.isnullable = 0 then 'NOT NULL' else 'NULL' end,
            case ColumnProperty(@unmerged_table_id, bc.name, 'UsesAnsiTrim')
                when 1 then @no
                when 0 then @yes
                else '(n/a)' end,
            case when type_name(bc.xtype) not in ('varbinary','varchar','binary','char')
                Then '(n/a)'
                When bc.status & 0x20 = 0 Then @no
                Else @yes END
        FROM    sirius_merged..syscolumns mc,
            sirius_for_broking..syscolumns bc
        WHERE   mc.id = @merged_table_id
        AND bc.id = @unmerged_table_id
        AND bc.name =* mc.name

    OPEN column_cursor

    FETCH NEXT FROM column_cursor
        INTO    @merged_column_name,
            @merged_column_type,
            @merged_column_computed,
            @merged_column_length,
            @merged_column_precision,
            @merged_column_scale,
            @merged_column_nullable,
            @merged_column_trim_trailing_blanks,
            @merged_column_fixed_len_null_in_source,
            @unmerged_column_name,
            @unmerged_column_type,
            @unmerged_column_computed,
            @unmerged_column_length,
            @unmerged_column_precision,
            @unmerged_column_scale,
            @unmerged_column_nullable,
            @unmerged_column_trim_trailing_blanks,
            @unmerged_column_fixed_len_null_in_source

    WHILE   @@FETCH_STATUS = 0
    BEGIN

        IF ISNULL(@unmerged_column_name,'NULL') = 'NULL'
        BEGIN
            --Add extra column
            SELECT  @message = 'ALTER TABLE ' + @table_name
                     + ' ADD COLUMN ' + @merged_column_name
                     + ' ' + @merged_column_type

            IF @merged_column_type = 'varchar'
            OR @merged_column_type = 'char'
                SELECT  @message = @message + '(' + CONVERT(VARCHAR(255),@merged_column_length) + ')'

            IF @merged_column_type = 'numeric'
                SELECT  @message = @message + '(' + ISNULL(@merged_column_precision,'Frog')
                    + ',' + ISNULL(@merged_column_scale,'') + ')'

            SELECT  @message = @message + ' NULL'

            PRINT   @message

            IF @merged_column_nullable = 'NOT NULL'
            BEGIN
                SELECT  @message = 'UPDATE ' + @table_name
                         + ' SET ' + @merged_column_name + ' = '

                IF @merged_column_type = 'varchar'
                OR @merged_column_type = 'char'
                    SELECT  @message = @message + '''' + ''''
                ELSE
                    IF @merged_column_type = 'datetime'
                        SELECT  @message = @message + '''' + '01-Jan-2001' + ''''
                    ELSE
                        SELECT  @message = @message + '0'

                PRINT @message

                SELECT  @message = 'ALTER TABLE ' + @table_name
                         + ' ALTER ' + @merged_column_name
                         + ' ' + @merged_column_type

                IF @merged_column_type = 'varchar'
                OR @merged_column_type = 'char'
                    SELECT  @message = @message + '(' + CONVERT(VARCHAR(255),@merged_column_length) + ')'

                IF @merged_column_type = 'numeric'
                    SELECT  @message = @message + '(' + ISNULL(@merged_column_precision,'Frog')
                        + ',' + ISNULL(@merged_column_scale,'') + ')'

                SELECT  @message = @message + ' ' + @merged_column_nullable

                PRINT   @message

            END

            SELECT  @message = 'GO'
            PRINT   @message

            PRINT   ''

        END
        ELSE
        BEGIN
            IF @merged_column_name <> @unmerged_column_name
            OR @merged_column_type <> @unmerged_column_type
            OR @merged_column_computed <> @unmerged_column_computed
            OR @merged_column_length <> @unmerged_column_length
            OR @merged_column_precision <> @unmerged_column_precision
            OR @merged_column_scale <> @unmerged_column_scale
            OR @merged_column_nullable <> @unmerged_column_nullable
            BEGIN
/*              SELECT  @message = @table_name + ':' + @merged_column_name + ' has changed'
                PRINT   @message
                SELECT  @message = 'name :' + @unmerged_column_name + ' - ' + @merged_column_name
                PRINT   @message
                SELECT  @message = 'type :' + @unmerged_column_type + ' - ' + @merged_column_type
                PRINT   @message
                SELECT  @message = 'computed :' + @unmerged_column_computed + ' - ' + @merged_column_computed
                PRINT   @message
                SELECT  @message = 'length :' + convert(varchar(255), @unmerged_column_length) + ' - ' + convert(varchar(255), @merged_column_length)
                PRINT   @message
                SELECT  @message = 'precision :' + @unmerged_column_precision + ' - ' + @merged_column_precision
                PRINT   @message
                SELECT  @message = 'scale :' + @unmerged_column_scale + ' - ' + @merged_column_scale
                PRINT   @message
                SELECT  @message = 'nullable :' + @unmerged_column_nullable + ' - ' + @merged_column_nullable
                PRINT   @message
                PRINT   ''
*/

                IF @merged_column_nullable = 'NOT NULL'
                AND @unmerged_column_nullable = 'NULL'
                BEGIN
                    SELECT  @message = 'UPDATE ' + @table_name
                             + ' SET ' + @merged_column_name + ' = '

                    IF @merged_column_type = 'varchar'
                    OR @merged_column_type = 'char'
                        SELECT  @message = @message + '''' + ''''
                    ELSE
                        IF @merged_column_type = 'datetime'
                            SELECT  @message = @message + '''' + '01-Jan-2001' + ''''
                        ELSE
                            SELECT  @message = @message + '0'

                    SELECT  @message = @message + ' WHERE ' + @merged_column_name + ' IS NULL'

                    PRINT @message
                END

                SELECT  @message = 'ALTER TABLE ' + @table_name
                         + ' ALTER ' + @merged_column_name
                         + ' ' + @merged_column_type

                IF @merged_column_type = 'varchar'
                OR @merged_column_type = 'char'
                    SELECT  @message = @message + '(' + CONVERT(VARCHAR(255),@merged_column_length) + ')'

                IF @merged_column_type = 'numeric'
                    SELECT  @message = @message + '(' + ISNULL(@merged_column_precision,'Frog')
                        + ',' + ISNULL(@merged_column_scale,'') + ')'

                SELECT  @message = @message + ' ' + @merged_column_nullable

                PRINT   @message
                SELECT  @message = 'GO'
                PRINT   @message

                PRINT   ''

            END
            --Has this one changed?
        END

        FETCH NEXT FROM column_cursor
            INTO    @merged_column_name,
                @merged_column_type,
                @merged_column_computed,
                @merged_column_length,
                @merged_column_precision,
                @merged_column_scale,
                @merged_column_nullable,
                @merged_column_trim_trailing_blanks,
                @merged_column_fixed_len_null_in_source,
                @unmerged_column_name,
                @unmerged_column_type,
                @unmerged_column_computed,
                @unmerged_column_length,
                @unmerged_column_precision,
                @unmerged_column_scale,
                @unmerged_column_nullable,
                @unmerged_column_trim_trailing_blanks,
                @unmerged_column_fixed_len_null_in_source

    END

    CLOSE column_cursor
    DEALLOCATE column_cursor

    FETCH NEXT FROM table_cursor
        INTO    @table_name,
            @unmerged_table_id,
            @merged_table_id

END

CLOSE table_cursor
DEALLOCATE table_cursor

/*
Now let's do the new tables
*/

DECLARE table_cursor CURSOR FOR
    SELECT  mo.name,
        mo.id
    FROM    sirius_merged..sysobjects mo
    WHERE   mo.type = 'U'
    AND mo.name NOT IN (
        SELECT  bo.name
        FROM    sirius_for_broking..sysobjects bo
        WHERE   bo.type = 'U'
        )
    ORDER BY 1

OPEN table_cursor

FETCH NEXT FROM table_cursor
    INTO    @table_name,
        @merged_table_id

WHILE   @@FETCH_STATUS = 0
BEGIN

    SELECT  @message = 'CREATE TABLE ' + @table_name + ' ('
    PRINT   @message

    DECLARE column_cursor CURSOR FOR
        SELECT  mc.name,
            type_name(mc.xusertype),
            case when mc.iscomputed = 0 then @no else @yes end,
            convert(int, mc.length),
            case when charindex(type_name(mc.xtype), @numtypes) > 0
                then convert(varchar(5),mc.xprec)
                else '' end,
            case when charindex(type_name(mc.xtype), @numtypes) > 0
                then convert(varchar(5),OdbcScale(mc.xtype,mc.xscale))
                else '' end,
            case when mc.isnullable = 0 then 'NOT NULL' else 'NULL' end,
            case ColumnProperty(@merged_table_id, mc.name, 'UsesAnsiTrim')
                when 1 then @no
                when 0 then @yes
                else '(n/a)' end,
            case when type_name(mc.xtype) not in ('varbinary','varchar','binary','char')
                Then '(n/a)'
                When mc.status & 0x20 = 0 Then @no
                Else @yes END
        FROM    sirius_merged..syscolumns mc
        WHERE   mc.id = @merged_table_id
        ORDER BY mc.colid

    OPEN column_cursor

    FETCH NEXT FROM column_cursor
        INTO    @merged_column_name,
            @merged_column_type,
            @merged_column_computed,
            @merged_column_length,
            @merged_column_precision,
            @merged_column_scale,
            @merged_column_nullable,
            @merged_column_trim_trailing_blanks,
            @merged_column_fixed_len_null_in_source

    WHILE   @@FETCH_STATUS = 0
    BEGIN

        SELECT  @message = 'ADD ' + @merged_column_name + ' ' + @merged_column_type

        IF @merged_column_type = 'varchar'
        OR @merged_column_type = 'char'
            SELECT  @message = @message + '(' + CONVERT(VARCHAR(255),@merged_column_length) + ')'

        IF @merged_column_type = 'numeric'
            SELECT  @message = @message + '(' + ISNULL(@merged_column_precision,'Frog')
                + ',' + ISNULL(@merged_column_scale,'') + ')'

        SELECT  @message = @message + ' ' + @merged_column_nullable + ','

        PRINT   @message

        FETCH NEXT FROM column_cursor
            INTO    @merged_column_name,
                @merged_column_type,
                @merged_column_computed,
                @merged_column_length,
                @merged_column_precision,
                @merged_column_scale,
                @merged_column_nullable,
                @merged_column_trim_trailing_blanks,
                @merged_column_fixed_len_null_in_source

    END

    CLOSE column_cursor
    DEALLOCATE column_cursor

    SELECT  @message = ')'
    PRINT   @message
    SELECT  @message = 'GO'
    PRINT   @message
    PRINT   ''

    FETCH NEXT FROM table_cursor
        INTO    @table_name,
            @merged_table_id

END

CLOSE table_cursor
DEALLOCATE table_cursor

