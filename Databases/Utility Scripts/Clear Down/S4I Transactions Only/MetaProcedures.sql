-- Meta-Procedures for acting on database objects, version 2.3.
-- This code does not depend on anything else, and can be executed in an empty database.

set quoted_identifier on set ansi_nulls on
go

----------------------------------------------------------------------------------------------------
-- Version Stamp
----------------------------------------------------------------------------------------------------

if exists (select null from sysobjects where name = 'DDLVersion' and type = 'P') begin
    drop procedure DDLVersion
end
go
-- Return the version of these procedures for any code that needs to know.
create procedure DDLVersion
as begin
    set nocount on

    select '2.3' as Version
end
go

----------------------------------------------------------------------------------------------------
-- Private Helper Tables
----------------------------------------------------------------------------------------------------

-- Populated by DDLEnableIntegrity with constraints that cannot be checked.
if not exists (select null from sysobjects where name = 'DDLConstraintsWithBadData' and type = 'U') begin
    create table DDLConstraintsWithBadData (
        TableName sysname not null,
        ConstraintName sysname not null,
        constraint PK__DDLConstraintsWithBadData primary key clustered (
            TableName,
            ConstraintName
        )
    )
end
go

----------------------------------------------------------------------------------------------------
-- Private Helper Functions
----------------------------------------------------------------------------------------------------

if exists (select null from sysobjects where name = 'DDLMakeColumnList' and type = 'P') begin
    drop procedure DDLMakeColumnList
end
go
-- Join column names together to make a list
create procedure DDLMakeColumnList
    @o_sList nvarchar(1040) output,
    @sSeparator nvarchar(2),
    @sColumnName1 sysname,
    @sColumnName2 sysname,
    @sColumnName3 sysname,
    @sColumnName4 sysname,
    @sColumnName5 sysname,
    @sColumnName6 sysname,
    @sColumnName7 sysname,
    @sColumnName8 sysname
as begin
    set nocount on

    if @sColumnName8 is not null begin
        select @o_sList = @sColumnName1 +
            @sSeparator + @sColumnName2 +
            @sSeparator + @sColumnName3 +
            @sSeparator + @sColumnName4 +
            @sSeparator + @sColumnName5 +
            @sSeparator + @sColumnName6 +
            @sSeparator + @sColumnName7 +
            @sSeparator + @sColumnName8
    end else if @sColumnName7 is not null begin
        select @o_sList = @sColumnName1 +
            @sSeparator + @sColumnName2 +
            @sSeparator + @sColumnName3 +
            @sSeparator + @sColumnName4 +
            @sSeparator + @sColumnName5 +
            @sSeparator + @sColumnName6 +
            @sSeparator + @sColumnName7
    end else if @sColumnName6 is not null begin
        select @o_sList = @sColumnName1 +
            @sSeparator + @sColumnName2 +
            @sSeparator + @sColumnName3 +
            @sSeparator + @sColumnName4 +
            @sSeparator + @sColumnName5 +
            @sSeparator + @sColumnName6
    end else if @sColumnName5 is not null begin
        select @o_sList = @sColumnName1 +
            @sSeparator + @sColumnName2 +
            @sSeparator + @sColumnName3 +
            @sSeparator + @sColumnName4 +
            @sSeparator + @sColumnName5
    end else if @sColumnName4 is not null begin
        select @o_sList = @sColumnName1 +
            @sSeparator + @sColumnName2 +
            @sSeparator + @sColumnName3 +
            @sSeparator + @sColumnName4
    end else if @sColumnName3 is not null begin
        select @o_sList = @sColumnName1 +
            @sSeparator + @sColumnName2 +
            @sSeparator + @sColumnName3
    end else if @sColumnName2 is not null begin
        select @o_sList = @sColumnName1 +
            @sSeparator + @sColumnName2
    end else begin
        select @o_sList = @sColumnName1
    end
end
go

----------------------------------------------------------------------------------------------------
-- Existence Checks
----------------------------------------------------------------------------------------------------

if exists (select null from sysobjects where name = 'DDLExistsTable' and type = 'P') begin
    drop procedure DDLExistsTable
end
go
-- Table
create procedure DDLExistsTable
    @sName sysname
as begin
    set nocount on

    if exists (select null from sysobjects where name = @sName and type = 'U') begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsView' and type = 'P') begin
    drop procedure DDLExistsView
end
go
-- View
create procedure DDLExistsView
    @sName sysname
as begin
    set nocount on

    if exists (select null from sysobjects where name = @sName and type = 'V') begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsProcedure' and type = 'P') begin
    drop procedure DDLExistsProcedure
end
go
-- Procedure
create procedure DDLExistsProcedure
    @sName sysname
as begin
    set nocount on

    if exists (select null from sysobjects where name = @sName and type = 'P') begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsFunction' and type = 'P') begin
    drop procedure DDLExistsFunction
end
go
-- Function
create procedure DDLExistsFunction
    @sName sysname
as begin
    set nocount on

    if exists (select null from sysobjects where name = @sName and type in ('FN', 'IF', 'TF')) begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsTrigger' and type = 'P') begin
    drop procedure DDLExistsTrigger
end
go
-- Trigger
create procedure DDLExistsTrigger
    @sName sysname
as begin
    set nocount on

    if exists (select null from sysobjects where name = @sName and type = 'TR') begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsColumn' and type = 'P') begin
    drop procedure DDLExistsColumn
end
go
-- Column
create procedure DDLExistsColumn
    @sTableName sysname,
    @sColumnName sysname
as begin
    set nocount on

    if exists (select null from syscolumns where id = object_id(@sTableName) and name = @sColumnName) begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsPrimaryKey' and type = 'P') begin
    drop procedure DDLExistsPrimaryKey
end
go
-- Primary key constraint
create procedure DDLExistsPrimaryKey
    @sTableName sysname,
    @sColumnName1 sysname = null,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sConstraintName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    select @lTableID = object_id(@sTableName)

    -- Find the name of the constraint.
    -- If no column names are specified, then we can find the name
    -- very easily. Otherwise, it cannot be found directly, but is
    -- always equal to the name of the matching unique index, so we
    -- search for that instead.
    if @sColumnName8 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 8
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
            and index_col(@sTableName, indid, 7) = @sColumnName7
            and index_col(@sTableName, indid, 8) = @sColumnName8
    end else if @sColumnName7 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 7
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
            and index_col(@sTableName, indid, 7) = @sColumnName7
    end else if @sColumnName6 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 6
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
    end else if @sColumnName5 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 5
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
    end else if @sColumnName4 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 4
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
    end else if @sColumnName3 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 3
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
    end else if @sColumnName2 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 2
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
    end else if @sColumnName1 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 2048) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 1
            and index_col(@sTableName, indid, 1) = @sColumnName1
    end else begin
        select @o_sConstraintName = object_name(constid)
            from sysconstraints
            where id = @lTableID
            and (status & 15) = 1
    end

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsAlternateKey' and type = 'P') begin
    drop procedure DDLExistsAlternateKey
end
go
-- Alternate key constraint
create procedure DDLExistsAlternateKey
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sConstraintName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    select @lTableID = object_id(@sTableName)

    -- Find the name of the constraint.
    -- This cannot be found directly, but is always equal to the name
    -- of the matching unique index, so we search for that instead.
    if @sColumnName8 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 8
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
            and index_col(@sTableName, indid, 7) = @sColumnName7
            and index_col(@sTableName, indid, 8) = @sColumnName8
    end else if @sColumnName7 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 7
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
            and index_col(@sTableName, indid, 7) = @sColumnName7
    end else if @sColumnName6 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 6
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
    end else if @sColumnName5 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 5
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
    end else if @sColumnName4 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 4
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
    end else if @sColumnName3 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 3
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
    end else if @sColumnName2 is not null begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 2
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
    end else begin
        select @o_sConstraintName = name
            from sysindexes
            where id = @lTableID
            and ((status ^ 4096) & 8394784) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 1
            and index_col(@sTableName, indid, 1) = @sColumnName1
    end

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsForeignKey' and type = 'P') begin
    drop procedure DDLExistsForeignKey
end
go
-- Foreign key constraint
create procedure DDLExistsForeignKey
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sConstraintName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    select @lTableID = object_id(@sTableName)

    -- Find the name of the constraint.
    select @o_sConstraintName = object_name(constid)
        from sysreferences
        where fkeyid = @lTableID
        and fkey1 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName1), 0)
        and fkey2 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName2), 0)
        and fkey3 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName3), 0)
        and fkey4 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName4), 0)
        and fkey5 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName5), 0)
        and fkey6 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName6), 0)
        and fkey7 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName7), 0)
        and fkey8 = isnull((select colid from syscolumns
            where id = @lTableID and name = @sColumnName8), 0)
        and fkey9 = 0
        and fkey10 = 0
        and fkey11 = 0
        and fkey12 = 0
        and fkey13 = 0
        and fkey14 = 0
        and fkey15 = 0
        and fkey16 = 0

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsIndex' and type = 'P') begin
    drop procedure DDLExistsIndex
end
go
-- Index
create procedure DDLExistsIndex
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sIndexName sysname = null output,
    @o_bLinkedToConstraint integer = null output
as begin
    set nocount on

    declare @lTableID integer
    declare @nIndexStatus integer

    select @lTableID = object_id(@sTableName)

    -- Find the name of the index.
    if @sColumnName8 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 8
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
            and index_col(@sTableName, indid, 7) = @sColumnName7
            and index_col(@sTableName, indid, 8) = @sColumnName8
    end else if @sColumnName7 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 7
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
            and index_col(@sTableName, indid, 7) = @sColumnName7
    end else if @sColumnName6 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 6
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
            and index_col(@sTableName, indid, 6) = @sColumnName6
    end else if @sColumnName5 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 5
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
            and index_col(@sTableName, indid, 5) = @sColumnName5
    end else if @sColumnName4 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 4
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
            and index_col(@sTableName, indid, 4) = @sColumnName4
    end else if @sColumnName3 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 3
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
    end else if @sColumnName2 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 2
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
    end else begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid) = 1
            and index_col(@sTableName, indid, 1) = @sColumnName1
    end

    select @o_bLinkedToConstraint = (case when (@nIndexStatus & 6144) <> 0 then 1 else 0 end)

    if @o_sIndexName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsCheck' and type = 'P') begin
    drop procedure DDLExistsCheck
end
go
-- Check constraint
create procedure DDLExistsCheck
    @sTableName sysname,
    @sColumnName sysname,
    @o_sConstraintName sysname = null output
as begin
    set nocount on

    -- Find the name of the constraint.
    select @o_sConstraintName = object_name(sysconstraints.constid)
        from sysconstraints
        inner join syscomments on sysconstraints.constid = syscomments.id
        where sysconstraints.id = object_id(@sTableName)
        and (sysconstraints.status & 15) = 4
        and syscomments.text like '%' + @sColumnName + '%'

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsCheckDef' and type = 'P') begin
    drop procedure DDLExistsCheckDef
end
go
-- Check constraint
create procedure DDLExistsCheckDef
    @sTableName sysname,
    @sCheckDefinition nvarchar(255),
    @o_sConstraintName sysname = null output
as begin
    set nocount on

    -- Find the name of the constraint.
    select @o_sConstraintName = object_name(sysconstraints.constid)
        from sysconstraints
        inner join syscomments on sysconstraints.constid = syscomments.id
        where sysconstraints.id = object_id(@sTableName)
        and (sysconstraints.status & 15) = 4
        and syscomments.text = @sCheckDefinition

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if exists (select null from sysobjects where name = 'DDLExistsDefault' and type = 'P') begin
    drop procedure DDLExistsDefault
end
go
-- Primary key constraint
create procedure DDLExistsDefault
    @sTableName sysname,
    @sColumnName sysname,
    @o_sConstraintName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    select @lTableID = object_id(@sTableName)

    -- Find the name of the constraint.
    select @o_sConstraintName = object_name(sysconstraints.constid)
        from sysconstraints
        inner join syscolumns on sysconstraints.id = syscolumns.id and sysconstraints.colid = syscolumns.colid
        where sysconstraints.id = @lTableID
        and (sysconstraints.status & 15) = 5
        and syscolumns.name = @sColumnName

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

----------------------------------------------------------------------------------------------------
-- Create Objects
----------------------------------------------------------------------------------------------------

if exists (select null from sysobjects where name = 'DDLAddColumn' and type = 'P') begin
    drop procedure DDLAddColumn
end
go
-- Create a table column
create procedure DDLAddColumn
    @sTableName sysname,
    @sColumnName sysname,
    @sColumnDefinition nvarchar(255),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    -- Check that the column exists.
    execute @bExists = DDLExistsColumn @sTableName, @sColumnName

    if @bExists = 1 begin
        -- Column exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: column ' + @sTableName + '.' + @sColumnName + ' already exists'
        end
    end else begin
        -- Column does not exist, so add it.
        select @sSQL = 'alter table ' + @sTableName + ' add ' + @sColumnName + ' ' + @sColumnDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLAddOrAlterColumn' and type = 'P') begin
    drop procedure DDLAddOrAlterColumn
end
go
-- Create or alter a table column
create procedure DDLAddOrAlterColumn
    @sTableName sysname,
    @sColumnName sysname,
    @sColumnDefinition nvarchar(255),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    -- Check that the column exists.
    execute @bExists = DDLExistsColumn @sTableName, @sColumnName

    if @bExists = 1 begin
        -- Column exists, so alter it.
        select @sSQL = 'alter table ' + @sTableName + ' alter column ' + @sColumnName + ' ' + @sColumnDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end else begin
        -- Column does not exist, so add it.
        select @sSQL = 'alter table ' + @sTableName + ' add ' + @sColumnName + ' ' + @sColumnDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLAddPrimaryKey' and type = 'P') begin
    drop procedure DDLAddPrimaryKey
end
go
-- Create a primary key defined on a number of columns
create procedure DDLAddPrimaryKey
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the constraint exists.
    execute @bExists = DDLExistsPrimaryKey @sTableName,
        null, null, null, null,
        null, null, null, null,
        @sConstraintName output

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: primary key ' + @sTableName + '.' + @sConstraintName + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('PK__' + @sTableName, 128)
        select @sSQL = 'alter table ' + @sTableName + ' add constraint ' + @sConstraintName + ' primary key clustered (' + @sColumnNames + ')'
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLAddAlternateKey' and type = 'P') begin
    drop procedure DDLAddAlternateKey
end
go
-- Create an alternate key defined on a number of columns
create procedure DDLAddAlternateKey
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sColumnNamesU nvarchar(1040)
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    execute DDLMakeColumnList @sColumnNamesU output, '__',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the constraint exists.
    execute @bExists = DDLExistsAlternateKey @sTableName,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: alternate key ' + @sTableName + '.' + @sConstraintName + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('AK__' + @sTableName + '__' + @sColumnNamesU, 128)
        select @sSQL = 'alter table ' + @sTableName + ' add constraint ' + @sConstraintName + ' unique nonclustered (' + @sColumnNames + ')'
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLAddIndex' and type = 'P') begin
    drop procedure DDLAddIndex
end
go
-- Create an index defined on a number of columns
create procedure DDLAddIndex
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sColumnNamesU nvarchar(1040)
    declare @sIndexName sysname
    declare @bLinkedToConstraint integer
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    execute DDLMakeColumnList @sColumnNamesU output, '__',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the index exists.
    execute @bExists = DDLExistsIndex @sTableName,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sIndexName output, @bLinkedToConstraint output

    if @bExists = 1 begin
        -- Index exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: index ' + @sTableName + '.' + @sIndexName + ' already exists'
        end
    end else begin
        -- Index does not exist, so add it.
        select @sIndexName = left('I__' + @sTableName + '__' + @sColumnNamesU, 128)
        select @sSQL = 'create nonclustered index ' + @sIndexName + ' on ' + @sTableName + '(' + @sColumnNames + ')'
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLAddForeignKey' and type = 'P') begin
    drop procedure DDLAddForeignKey
end
go
-- Create a foreign key defined on a number of columns
create procedure DDLAddForeignKey
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @sRefTableName sysname,
    @sRefColumnName1 sysname = null,
    @sRefColumnName2 sysname = null,
    @sRefColumnName3 sysname = null,
    @sRefColumnName4 sysname = null,
    @sRefColumnName5 sysname = null,
    @sRefColumnName6 sysname = null,
    @sRefColumnName7 sysname = null,
    @sRefColumnName8 sysname = null,
    @bAddIndex tinyint = 0,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sColumnNamesU nvarchar(1040)
    declare @sRefColumnNames nvarchar(1040)
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    execute DDLMakeColumnList @sRefColumnNames output, ', ',
        @sRefColumnName1, @sRefColumnName2, @sRefColumnName3, @sRefColumnName4,
        @sRefColumnName5, @sRefColumnName6, @sRefColumnName7, @sRefColumnName8

    execute DDLMakeColumnList @sColumnNamesU output, '__',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the constraint exists.
    execute @bExists = DDLExistsForeignKey @sTableName,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: foreign key ' + @sTableName + '.' + @sConstraintName + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('FK__' + @sTableName + '__' + @sColumnNamesU, 128)
        select @sSQL = 'alter table ' + @sTableName + ' add constraint ' + @sConstraintName + ' foreign key (' + @sColumnNames + ') references ' + @sRefTableName
        if @sRefColumnNames is not null begin
            select @sSQL = @sSQL + ' (' + @sRefColumnNames + ')'
        end
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end

    -- Add an index on the constraint columns if requested.
    if isnull(@bAddIndex, 0) <> 0 begin
        execute DDLAddIndex @sTableName,
            @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
            @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
            @bQuiet
    end
end
go

if exists (select null from sysobjects where name = 'DDLAddCheck' and type = 'P') begin
    drop procedure DDLAddCheck
end
go
-- Create a check constraint defined on a number of columns
create procedure DDLAddCheck
    @sTableName sysname,
    @sColumnName sysname,
    @sCheckDefinition nvarchar(255),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    -- Make sure that the definition is enclosed in brackets.
    if left(@sCheckDefinition, 1) <> '(' or right(@sCheckDefinition, 1) <> ')' begin
        select @sCheckDefinition = '(' + @sCheckDefinition + ')'
    end

    -- Check that the constraint exists.
    execute @bExists = DDLExistsCheckDef @sTableName, @sCheckDefinition, @sConstraintName output

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: constraint ' + @sTableName + '.' + @sConstraintName + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('CH__' + @sTableName + '__' + @sColumnName, 128)
        select @sSQL = 'alter table ' + @sTableName + ' add constraint ' + @sConstraintName + ' check ' + @sCheckDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

----------------------------------------------------------------------------------------------------
-- Drop Objects
----------------------------------------------------------------------------------------------------

if exists (select null from sysobjects where name = 'DDLDropTable' and type = 'P') begin
    drop procedure DDLDropTable
end
go
-- Drop table
create procedure DDLDropTable
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsTable @sName
    if @bExists = 1 begin
        -- Table exists, so drop it.
        select @sSQL = 'drop table ' + @sName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropView' and type = 'P') begin
    drop procedure DDLDropView
end
go
-- Drop view
create procedure DDLDropView
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsView @sName
    if @bExists = 1 begin
        -- View exists, so drop it.
        select @sSQL = 'drop view ' + @sName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropProcedure' and type = 'P') begin
    drop procedure DDLDropProcedure
end
go
-- Drop procedure
create procedure DDLDropProcedure
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsProcedure @sName
    if @bExists = 1 begin
        -- Procedure exists, so drop it.
        select @sSQL = 'drop procedure ' + @sName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropFunction' and type = 'P') begin
    drop procedure DDLDropFunction
end
go
-- Drop function
create procedure DDLDropFunction
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsFunction @sName
    if @bExists = 1 begin
        -- Function exists, so drop it.
        select @sSQL = 'drop function ' + @sName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropTrigger' and type = 'P') begin
    drop procedure DDLDropTrigger
end
go
-- Drop trigger
create procedure DDLDropTrigger
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsTrigger @sName
    if @bExists = 1 begin
        -- Trigger exists, so drop it.
        select @sSQL = 'drop trigger ' + @sName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropColumn' and type = 'P') begin
    drop procedure DDLDropColumn
end
go
-- Drop a table column
create procedure DDLDropColumn
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sSQL nvarchar(4000)

    -- Check that the column exists.
    execute @bExists = DDLExistsColumn @sTableName, @sColumnName

    if @bExists = 1 begin
        -- Column exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop column ' + @sColumnName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end else begin
        -- Column does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: column ' + @sTableName + '.' + @sColumnName + ' does not exist'
        end
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropPrimaryKey' and type = 'P') begin
    drop procedure DDLDropPrimaryKey
end
go
-- Drop a primary key constraint defined on any number of columns
create procedure DDLDropPrimaryKey
    @sTableName sysname,
    @sColumnName1 sysname = null,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the constraint exists.
    execute @bExists = DDLExistsPrimaryKey @sTableName,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if @sColumnNames is not null begin
            if isnull(@bQuiet, 0) = 0 begin
                print 'INFO: primary key on ' + @sTableName + '(' + @sColumnNames + ') does not exist'
            end
        end else begin
            if isnull(@bQuiet, 0) = 0 begin
                print 'INFO: primary key on ' + @sTableName + ' does not exist'
            end
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sConstraintName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropAlternateKey' and type = 'P') begin
    drop procedure DDLDropAlternateKey
end
go
-- Drop an alternate key defined on a number of columns
create procedure DDLDropAlternateKey
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the constraint exists.
    execute @bExists = DDLExistsAlternateKey @sTableName,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: alternate key on ' + @sTableName + '(' + @sColumnNames + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sConstraintName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropForeignKey' and type = 'P') begin
    drop procedure DDLDropForeignKey
end
go
-- Drop a foreign key constraint defined on a number of columns
create procedure DDLDropForeignKey
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the constraint exists.
    execute @bExists = DDLExistsForeignKey @sTableName,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: foreign key on ' + @sTableName + '(' + @sColumnNames + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sConstraintName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropIndex' and type = 'P') begin
    drop procedure DDLDropIndex
end
go
-- Drop an index defined on a number of columns
create procedure DDLDropIndex
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sColumnNames nvarchar(1040)
    declare @sIndexName sysname
    declare @bLinkedToConstraint integer
    declare @sSQL nvarchar(4000)

    execute DDLMakeColumnList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Check that the index exists.
    execute @bExists = DDLExistsIndex @sTableName,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sIndexName output, @bLinkedToConstraint output

    if @bExists = 0 begin
        -- Index does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: index on ' + @sTableName + '(' + @sColumnNames + ') does not exist'
        end
    end else if @bLinkedToConstraint = 1 begin
        -- Index is linked to a constraint.
        if isnull(@bQuiet, 0) = 0 begin
            print 'ERROR: index ' + @sTableName + '.' + @sIndexName + ' is linked to a constraint'
        end
    end else begin
        -- Index exists, so drop it.
        select @sSQL = 'drop index ' + @sTableName + '.' + @sIndexName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropCheck' and type = 'P') begin
    drop procedure DDLDropCheck
end
go
-- Drop a table-level check constraint containing the specified column name
create procedure DDLDropCheck
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsCheck @sTableName, @sColumnName, @sConstraintName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: check on ' + @sTableName + '(' + @sColumnName + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sConstraintName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropCheckDef' and type = 'P') begin
    drop procedure DDLDropCheckDef
end
go
-- Drop a table-level check constraint with the specified definition
create procedure DDLDropCheckDef
    @sTableName sysname,
    @sCheckDefinition nvarchar(255),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsCheckDef @sTableName, @sCheckDefinition, @sConstraintName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: check ' + @sTableName + '(' + @sCheckDefinition + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sConstraintName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

if exists (select null from sysobjects where name = 'DDLDropDefault' and type = 'P') begin
    drop procedure DDLDropDefault
end
go
-- Drop a default constraint defined on a column
create procedure DDLDropDefault
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsDefault @sTableName, @sColumnName, @sConstraintName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: default for ' + @sTableName + '.' + @sColumnName + ' does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sConstraintName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
    end
end
go

----------------------------------------------------------------------------------------------------
-- Obsolete
----------------------------------------------------------------------------------------------------

if exists (select null from sysobjects where name = 'DDLDropConstraint' and type = 'P') begin
    drop procedure DDLDropConstraint
end
go
-- OBSOLETE, DO NOT USE
create procedure DDLDropConstraint
    @sTableName sysname,
    @sColumnName sysname
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    print 'WARNING: DDLDropConstraint is obsolete and dangerous to use. Please replace with a more specific DDL procedure.'

    -- Find the name of the constraint.
    select @sConstraintName = object_name(sysconstraints.constid)
        from sysconstraints
        inner join syscolumns on sysconstraints.colid = syscolumns.colid
        where sysconstraints.id = object_id(@sTableName)
        and syscolumns.name = @sColumnName

    if @@rowcount = 0 begin
        -- Constraint does not exist.
        print 'INFO: constraint on ' + @sTableName + '(' + @sColumnName + ') does not exist'
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sConstraintName
        print 'EXEC: ' + @sSQL
        execute(@sSQL)
    end
end
go

----------------------------------------------------------------------------------------------------
-- Utilities
----------------------------------------------------------------------------------------------------

if exists (select null from sysobjects where name = 'DDLDropDuplicatedForeignKeys' and type = 'P') begin
    drop procedure DDLDropDuplicatedForeignKeys
end
go
-- Drop all duplicated foreign keys in the database
create procedure DDLDropDuplicatedForeignKeys
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sSQL nvarchar(512)

    -- Create temp table to hold the duplicates.
    create table #Duplicates (TableID integer not null, ConstraintID integer not null, primary key(TableID, ConstraintID))

    -- Find all FKs that have more than one identical copy.
    insert into #Duplicates
        select sysreferences.fkeyid, sysreferences.constid
        from sysreferences
        inner join (
            select fkeyid, fkey1, fkey2, fkey3, fkey4, fkey5, fkey6, fkey7, fkey8, fkey9, fkey10, fkey11, fkey12, fkey13, fkey14, fkey15, fkey16
            from sysreferences
            group by fkeyid, fkey1, fkey2, fkey3, fkey4, fkey5, fkey6, fkey7, fkey8, fkey9, fkey10, fkey11, fkey12, fkey13, fkey14, fkey15, fkey16
            having count(*) > 1
        ) as duplicates
            on sysreferences.fkeyid = duplicates.fkeyid
            and sysreferences.fkey1 = duplicates.fkey1
            and sysreferences.fkey2 = duplicates.fkey2
            and sysreferences.fkey3 = duplicates.fkey3
            and sysreferences.fkey4 = duplicates.fkey4
            and sysreferences.fkey5 = duplicates.fkey5
            and sysreferences.fkey6 = duplicates.fkey6
            and sysreferences.fkey7 = duplicates.fkey7
            and sysreferences.fkey8 = duplicates.fkey8
            and sysreferences.fkey9 = duplicates.fkey9
            and sysreferences.fkey10 = duplicates.fkey10
            and sysreferences.fkey11 = duplicates.fkey11
            and sysreferences.fkey12 = duplicates.fkey12
            and sysreferences.fkey13 = duplicates.fkey13
            and sysreferences.fkey14 = duplicates.fkey14
            and sysreferences.fkey15 = duplicates.fkey15
            and sysreferences.fkey16 = duplicates.fkey16

    -- Loop through all duplicated FKs except the first copy of each one.
    declare curDuplicates cursor fast_forward for
        select 'alter table ' + object_name(TableID) + ' drop constraint ' + object_name(ConstraintID)
        from #Duplicates
        where ConstraintID not in (
            select min(ConstraintID)
            from #Duplicates
            group by TableID
        )

    -- Drop each FK.
    open curDuplicates
    fetch next from curDuplicates into @sSQL
    while @@fetch_status = 0 begin
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
        fetch next from curDuplicates into @sSQL
    end
    close curDuplicates
    deallocate curDuplicates

    -- Drop temp table.
    drop table #Duplicates
end
go

if exists (select null from sysobjects where name = 'DDLDropStatistics' and type = 'P') begin
    drop procedure DDLDropStatistics
end
go
-- Drop non-index statistics on tables
create procedure DDLDropStatistics
    @sTableName sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sIndexName sysname
    declare @sSQL nvarchar(4000)

    if @sTableName is null begin
        -- Find non-index statistics in all user tables.
        declare curStatistics cursor fast_forward for
            select sysobjects.name, sysindexes.name
            from sysindexes
            inner join sysobjects on sysindexes.id = sysobjects.id and sysobjects.type = 'U'
            where sysobjects.name <> 'dtproperties'
            and (sysindexes.status & 32) <> 0
            order by sysobjects.name, sysindexes.name
    end else begin
        -- Find non-index statistics in the specified user table.
        declare curStatistics cursor fast_forward for
            select sysobjects.name, sysindexes.name
            from sysindexes
            inner join sysobjects on sysindexes.id = sysobjects.id and sysobjects.type = 'U'
            where sysobjects.name = @sTableName
            and (sysindexes.status & 32) <> 0
            order by sysobjects.name, sysindexes.name
    end

    -- Drop the statistics found.
    open curStatistics
    fetch next from curStatistics into @sTableName, @sIndexName
    while @@fetch_status = 0 begin
        select @sSQL = 'drop statistics ' + @sTableName + '.' + @sIndexName
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute(@sSQL)
        fetch next from curStatistics into @sTableName, @sIndexName
    end
    close curStatistics

    -- Destroy the cursor.
    deallocate curStatistics
end
go

if exists (select null from sysobjects where name = 'DDLFindUncheckedConstraints' and type = 'P') begin
    drop procedure DDLFindUncheckedConstraints
end
go
-- Find all integrity constraints that are disabled or untrusted
create procedure DDLFindUncheckedConstraints
as begin
    set nocount on

    -- Create temp table to hold the results.
    create table #Output (
        ConstraintID integer not null primary key,
        TableName sysname not null,
        ConstraintName sysname not null,
        Disabled bit not null,
        NotTrusted bit not null,
        ColumnNames nvarchar(2048) null
    )

    -- Find all disabled or untrusted constraints.
    insert into #Output (ConstraintID, TableName, ConstraintName, Disabled, NotTrusted)
    select
        [constraint].id,
        [table].name,
        [constraint].name,
        objectproperty([constraint].id, 'CnstIsDisabled'),
        objectproperty([constraint].id, 'CnstIsNotTrusted')
    from sysobjects as [constraint]
    inner join sysobjects as [table] on [constraint].parent_obj = [table].id
    where [constraint].type in ('C', 'F')
    and (objectproperty([constraint].id, 'CnstIsDisabled') = 1 or objectproperty([constraint].id, 'CnstIsNotTrusted') = 1)

    -- Fill the list of column names for each one.
    declare @lConstraintID integer
    declare @sColumnName sysname
    declare @sColumnNames nvarchar(2048)

    declare curOutput cursor fast_forward for
        select ConstraintID from #Output

    open curOutput
    fetch next from curOutput into @lConstraintID
    while @@fetch_status = 0 begin

        select @sColumnNames = ''

        declare curColumns cursor fast_forward for
            select syscolumns.name
                from syscolumns
                inner join sysconstraints on sysconstraints.id = syscolumns.id and sysconstraints.colid = syscolumns.colid
                where sysconstraints.constid = @lConstraintID
                order by sysconstraints.colid

        open curColumns
        fetch next from curColumns into @sColumnName
        while @@fetch_status = 0 begin

            if @sColumnNames <> '' begin
                select @sColumnNames = @sColumnNames + ', '
            end
            select @sColumnNames = @sColumnNames + @sColumnName

            fetch next from curColumns into @sColumnName
        end
        close curColumns
        deallocate curColumns

        update #Output set ColumnNames = @sColumnNames where ConstraintID = @lConstraintID

        fetch next from curOutput into @lConstraintID
    end
    close curOutput
    deallocate curOutput

    -- Output the results.
    select
        TableName,
        ConstraintName,
        Disabled,
        NotTrusted,
        ColumnNames
    from #Output
    order by TableName, ConstraintName

    -- Drop temp table.
    drop table #Output
end
go

if exists (select null from sysobjects where name = 'DDLEnableIntegrity' and type = 'P') begin
    drop procedure DDLEnableIntegrity
end
go
-- Enable or disable all integrity constraints and triggers
create procedure DDLEnableIntegrity
    @bEnabled tinyint,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists tinyint
    declare @sTableName sysname
    declare @sConstraintName sysname
    declare @sTriggerName sysname
    declare @sSQL nvarchar(256)
    declare @sSQL1 nvarchar(12)
    declare @sSQL2 nvarchar(32)

    select @sSQL1 = 'alter table '

    --------------------
    -- Constraints
    --------------------

    if @bEnabled = 0 begin

        -- Find all user tables.
        declare curTables cursor fast_forward for
            select name
            from sysobjects
            where type = 'U'
            and name <> 'dtproperties'
            order by name

        -- Disable all constraints on each one.
        select @sSQL2 = ' nocheck constraint all'
        open curTables
        fetch next from curTables into @sTableName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + @sTableName + @sSQL2
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curTables into @sTableName
        end
        close curTables
        deallocate curTables

    end else if @bEnabled = 1 begin

        -- Find all disabled constraints.
        declare curConstraints cursor fast_forward for
            select [table].name, [constraint].name
            from sysobjects as [constraint]
            inner join sysobjects as [table] on [constraint].parent_obj = [table].id
            where [constraint].type in ('C', 'F')
            and objectproperty([constraint].id, 'CnstIsDisabled') = 1
            order by [table].name, [constraint].name

        -- Enable each one but don't check existing data.
        select @sSQL2 = ' with nocheck check constraint '
        open curConstraints
        fetch next from curConstraints into @sTableName, @sConstraintName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + @sTableName + @sSQL2 + @sConstraintName
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curConstraints into @sTableName, @sConstraintName
        end
        close curConstraints
        deallocate curConstraints

    end else if @bEnabled = 2 begin

        -- Find all disabled or untrusted constraints not already excluded.
        declare curConstraints cursor fast_forward for
            select [table].name, [constraint].name
            from sysobjects as [constraint]
            inner join sysobjects as [table] on [constraint].parent_obj = [table].id
            where [constraint].type in ('C', 'F')
            and (objectproperty([constraint].id, 'CnstIsDisabled') = 1 or
                objectproperty([constraint].id, 'CnstIsNotTrusted') = 1)
            and not exists (
                select null
                from DDLConstraintsWithBadData
                where TableName = [table].name
                and ConstraintName = [constraint].name
            )
            order by [table].name, [constraint].name

        -- Enable and check them. This loop may very well abort the batch if one of the
        -- constraints does not validate against existing table data. In this situation,
        -- the exclusion list will contain all constraints that failed.
        select @sSQL2 = ' with check check constraint '
        open curConstraints
        fetch next from curConstraints into @sTableName, @sConstraintName
        while @@fetch_status = 0 begin

            insert into DDLConstraintsWithBadData
                values (@sTableName, @sConstraintName)

            select @sSQL = @sSQL1 + @sTableName + @sSQL2 + @sConstraintName
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            delete from DDLConstraintsWithBadData
                where TableName = @sTableName
                and ConstraintName = @sConstraintName

            fetch next from curConstraints into @sTableName, @sConstraintName
        end
        close curConstraints
        deallocate curConstraints

        -- If there are any rows in the exclusion list, then enable the constraint
        -- without checking it. The constraint will not be used by the query optimizer,
        -- but at least it will still check new data as it is inserted.
        if exists (select null from DDLConstraintsWithBadData) begin

            declare curConstraints cursor fast_forward for
                select TableName, ConstraintName
                from DDLConstraintsWithBadData
                order by TableName, ConstraintName

            -- Enable them but don't check existing data.
            select @sSQL2 = ' with nocheck check constraint '
            open curConstraints
            fetch next from curConstraints into @sTableName, @sConstraintName
            while @@fetch_status = 0 begin

                select @sSQL = @sSQL1 + @sTableName + @sSQL2 + @sConstraintName
                if isnull(@bQuiet, 0) = 0 begin
                    print 'EXEC: ' + @sSQL
                end
                execute sp_executesql @sSQL

                fetch next from curConstraints into @sTableName, @sConstraintName
            end
            close curConstraints
            deallocate curConstraints

        end

    end

    --------------------
    -- Triggers
    --------------------

    if @bEnabled = 0 begin

        -- Find all user tables.
        declare curTables cursor fast_forward for
            select name
            from sysobjects
            where type = 'U'
            and name <> 'dtproperties'
            order by name

        -- Disable all triggers on each one.
        select @sSQL2 = ' disable trigger all'
        open curTables
        fetch next from curTables into @sTableName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + @sTableName + @sSQL2
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curTables into @sTableName
        end
        close curTables
        deallocate curTables

    end else if @bEnabled = 1 begin

        -- Find all disabled triggers.
        declare curTriggers cursor fast_forward for
            select [table].name, [trigger].name
            from sysobjects as [trigger]
            inner join sysobjects as [table] on [trigger].parent_obj = [table].id
            where [trigger].type in ('TR')
            and objectproperty([trigger].id, 'ExecIsTriggerDisabled') = 1
            order by [table].name, [trigger].name

        -- Enable each one.
        select @sSQL2 = ' enable trigger '
        open curTriggers
        fetch next from curTriggers into @sTableName, @sTriggerName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + @sTableName + @sSQL2 + @sTriggerName
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curTriggers into @sTableName, @sTriggerName
        end
        close curTriggers
        deallocate curTriggers

    end
end
go

if exists (select null from sysobjects where name = 'DDLUpdateStatsAndRecompile' and type = 'P') begin
    drop procedure DDLUpdateStatsAndRecompile
end
go
-- Update all statistics and recompile all procedures and triggers
create procedure DDLUpdateStatsAndRecompile
    @nSamplePercent tinyint = 0,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sTableName sysname
    declare @sSamplingClause nvarchar(24)

    if @nSamplePercent between 1 and 100 begin
        select @sSamplingClause = ' with sample ' + convert(nvarchar(3), @nSamplePercent) + ' percent'
    end else begin
        select @sSamplingClause = ''
    end

    -- Find all tables we want to act on.
    declare curTables cursor fast_forward for
        select name from sysobjects
        where type = 'U' and name <> 'dtproperties'
        order by name

    -- Update statistics for each table.
    open curTables
    fetch next from curTables into @sTableName
    while @@fetch_status = 0 begin
        if isnull(@bQuiet, 0) = 0 begin
            print 'Update Stats ' + @sTableName
        end
        execute ('update statistics ' + @sTableName + @sSamplingClause)
        fetch next from curTables into @sTableName
    end
    close curTables

    -- Recompile all code that references each table.
    open curTables
    fetch next from curTables into @sTableName
    while @@fetch_status = 0 begin
        if isnull(@bQuiet, 0) = 0 begin
            print 'Recompile ' + @sTableName
        end
        execute sp_recompile @sTableName
        fetch next from curTables into @sTableName
    end
    close curTables

    -- Destroy the cursor.
    deallocate curTables
end
go

if exists (select null from sysobjects where name = 'DDLReSeedIdentity' and type = 'P') begin
    drop procedure DDLReSeedIdentity
end
go
set nocount on
declare @sSQL nvarchar(512)
if @@version like 'Microsoft SQL Server  7%' begin
    -- SQL 7 does not have the ident_current() function, so the safest fallback
    -- is to reseed to 1 for truncated tables and 2 for deleted tables.
    select @sSQL = '-- Re-seed identity value in a table
create procedure DDLReSeedIdentity
    @sTableName sysname
as begin
    set nocount on

    dbcc checkident(@sTableName, reseed, 1)
    dbcc checkident(@sTableName, reseed)
end
'
end else begin
    -- SQL 8 and above can re-seed properly under all circumstances.
    select @sSQL = '-- Re-seed identity value in a table
create procedure DDLReSeedIdentity
    @sTableName sysname
as begin
    set nocount on

    if ident_current(@sTableName) > 1 begin
        dbcc checkident(@sTableName, reseed, 0)
        dbcc checkident(@sTableName, reseed)
    end else begin
        dbcc checkident(@sTableName, reseed, 1)
    end
end
'
end
execute (@sSQL)
go

if exists (select null from sysobjects where name = 'DDLReSeedIdentities' and type = 'P') begin
    drop procedure DDLReSeedIdentities
end
go
-- Re-seed identity values in all tables
create procedure DDLReSeedIdentities
as begin
    set nocount on

    declare @sTableName sysname

    -- Find all user tables that have an identity column.
    declare curTables cursor fast_forward for
        select name from sysobjects
        where type = 'U'
        and name <> 'dtproperties'
        and exists (
            select null from syscolumns
            where syscolumns.id = sysobjects.id
            and (syscolumns.status & 128) <> 0
        )
        order by name

    -- Re-seed each one.
    open curTables
    fetch next from curTables into @sTableName
    while @@fetch_status = 0 begin
        execute DDLReSeedIdentity @sTableName
        fetch next from curTables into @sTableName
    end
    close curTables

    -- Destroy the cursor.
    deallocate curTables
end
go

if exists (select null from sysobjects where name = 'DDLFreeCaches' and type = 'P') begin
    drop procedure DDLFreeCaches
end
go
-- Free all data and procedure caches on the server
create procedure DDLFreeCaches
as begin
    set nocount on

    dbcc dropcleanbuffers
    dbcc freeproccache
end
go

if exists (select null from sysobjects where name = 'DDLRepairAndDefrag' and type = 'P') begin
    drop procedure DDLRepairAndDefrag
end
go
-- Repair and defrag the database
create procedure DDLRepairAndDefrag
as begin
    set nocount on

    declare @sDatabaseName sysname

    select @sDatabaseName = db_name()

    -- Switch to single-user mode (required for checkdb).
    execute sp_dboption @sDatabaseName, 'single user', 'true'

    -- Repair and rebuild all indexes.
    dbcc checkdb (@sDatabaseName, repair_rebuild)

    -- Restore multi-user mode.
    execute sp_dboption @sDatabaseName, 'single user', 'false'
end
go

if exists (select null from sysobjects where name = 'DDLTruncateLogAndShrinkFiles' and type = 'P') begin
    drop procedure DDLTruncateLogAndShrinkFiles
end
go
-- Truncate the transaction log and shrink database files
create procedure DDLTruncateLogAndShrinkFiles
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sDatabaseName sysname
    declare @sSQL nvarchar(256)

    select @sDatabaseName = db_name()

    -- Remove all committed transactions from the log.
    select @sSQL = 'backup log ' + @sDatabaseName + ' with truncate_only'
    if isnull(@bQuiet, 0) = 0 begin
        print 'EXEC: ' + @sSQL
    end
    execute (@sSQL)

    -- Physically shrink the database and transaction log files.
    select @sSQL = 'dbcc shrinkdatabase(' + @sDatabaseName + ')'
    if isnull(@bQuiet, 0) = 0 begin
        print 'EXEC: ' + @sSQL
    end
    execute (@sSQL)
end
go

-- End of file.
