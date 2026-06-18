-- Meta-Procedures for acting on database objects, version 2.4.1.
-- This code does not depend on anything else, and can be executed in an empty database.

set quoted_identifier on set ansi_nulls on
go

----------------------------------------------------------------------------------------------------
-- Version Stamp
----------------------------------------------------------------------------------------------------

if objectproperty(object_id('dbo.DDLVersion'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLVersion
end
go
-- Return the version of these procedures for any code that needs to know.
create procedure dbo.DDLVersion
as begin
    set nocount on

    select '2.4.1' as Version
end
go
   --------------------
    -- spu_dboption
    --------------------
	--SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
	GO
	EXECUTE DDLDropProcedure 'spu_dboption'
	GO
	Create PROCEDURE [dbo].[spu_dboption]
	   @sDatabaseName NVARCHAR(500),
	   @sUsertype  NVARCHAR(500)
	AS
		
	Begin
		DECLARE @SQLQuery AS NVARCHAR(500)
		DECLARE @results AS NVARCHAR(500)
		
	 
	   SET @SQLQuery = 'ALTER DATABASE '
						+ @sDatabaseName + ' SET ' + @sUsertype 
						
		 EXEC sp_executesql @SQLQuery
		 
	End
GO
----------------------------------------------------------------------------------------------------
-- Drop Old Objects
----------------------------------------------------------------------------------------------------
go
if objectproperty(object_id('dbo.DDLConstraintsWithBadData'), 'IsTable') = 1 begin
    execute ('
        if not exists (select null from dbo.DDLConstraintsWithBadData) begin
            drop table dbo.DDLConstraintsWithBadData
        end
    ')
end
go

if objectproperty(object_id('dbo.DDLMakeColumnList'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLMakeColumnList
end
go

----------------------------------------------------------------------------------------------------
-- Private Helper Functions
----------------------------------------------------------------------------------------------------

if objectproperty(object_id('dbo.DDLEscapeNamePart'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLEscapeNamePart
end
go
-- Replace all invalid characters in a name with safe underscores.
create procedure dbo.DDLEscapeNamePart
    @sName sysname,
    @o_sName sysname output
as begin
    set nocount on

    declare @iChar integer
    declare @nChars integer
    declare @sChar nchar(1)

    select @iChar = 1, @nChars = len(@o_sName), @o_sName = @sName

    while @iChar <= @nChars begin
        select @sChar = substring(@o_sName, @iChar, 1)
        if @sChar not between 'A' and 'Z' and @sChar not between '0' and '9' and @sChar <> '_' begin
            select @o_sName = stuff(@o_sName, @iChar, 1, '_')
        end
        select @iChar = @iChar + 1
    end
end
go

if objectproperty(object_id('dbo.DDLMakeNameList'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLMakeNameList
end
go
-- Join names together to make a list.
create procedure dbo.DDLMakeNameList
    @o_sList nvarchar(1040) output,
    @sSeparator nvarchar(2),
    @sName1 sysname,
    @sName2 sysname,
    @sName3 sysname,
    @sName4 sysname,
    @sName5 sysname,
    @sName6 sysname,
    @sName7 sysname,
    @sName8 sysname
as begin
    set nocount on

    -- Recognise certain separators and escape each name appropriately.
    if @sSeparator = ', ' begin
        select
            @sName1 = quotename(@sName1),
            @sName2 = quotename(@sName2),
            @sName3 = quotename(@sName3),
            @sName4 = quotename(@sName4),
            @sName5 = quotename(@sName5),
            @sName6 = quotename(@sName6),
            @sName7 = quotename(@sName7),
            @sName8 = quotename(@sName8)
    end else if @sSeparator = '__' begin
        execute DDLEscapeNamePart @sName1, @sName1 output
        execute DDLEscapeNamePart @sName2, @sName2 output
        execute DDLEscapeNamePart @sName3, @sName3 output
        execute DDLEscapeNamePart @sName4, @sName4 output
        execute DDLEscapeNamePart @sName5, @sName5 output
        execute DDLEscapeNamePart @sName6, @sName6 output
        execute DDLEscapeNamePart @sName7, @sName7 output
        execute DDLEscapeNamePart @sName8, @sName8 output
    end

    -- Join the names together.
    if @sName8 is not null begin
        select @o_sList = @sName1 +
            @sSeparator + @sName2 +
            @sSeparator + @sName3 +
            @sSeparator + @sName4 +
            @sSeparator + @sName5 +
            @sSeparator + @sName6 +
            @sSeparator + @sName7 +
            @sSeparator + @sName8
    end else if @sName7 is not null begin
        select @o_sList = @sName1 +
            @sSeparator + @sName2 +
            @sSeparator + @sName3 +
            @sSeparator + @sName4 +
            @sSeparator + @sName5 +
            @sSeparator + @sName6 +
            @sSeparator + @sName7
    end else if @sName6 is not null begin
        select @o_sList = @sName1 +
            @sSeparator + @sName2 +
            @sSeparator + @sName3 +
            @sSeparator + @sName4 +
            @sSeparator + @sName5 +
            @sSeparator + @sName6
    end else if @sName5 is not null begin
        select @o_sList = @sName1 +
            @sSeparator + @sName2 +
            @sSeparator + @sName3 +
            @sSeparator + @sName4 +
            @sSeparator + @sName5
    end else if @sName4 is not null begin
        select @o_sList = @sName1 +
            @sSeparator + @sName2 +
            @sSeparator + @sName3 +
            @sSeparator + @sName4
    end else if @sName3 is not null begin
        select @o_sList = @sName1 +
            @sSeparator + @sName2 +
            @sSeparator + @sName3
    end else if @sName2 is not null begin
        select @o_sList = @sName1 +
            @sSeparator + @sName2
    end else begin
        select @o_sList = @sName1
    end
end
go

if objectproperty(object_id('dbo.DDLValidateColumnList'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLValidateColumnList
end
go

-- Validates the comma seperated list of column exists in table
create procedure dbo.DDLValidateColumnList
(
@sTableName sysname output,
@sColumnList nvarchar(1040)
)
as
begin
	 declare @sColumnName sysname
	 declare @sOwnerName sysname
	 declare @bResult bit
	 
	 set @sColumnList = replace(@sColumnList, CHAR(13)+CHAR(10), '')
	while len(@sColumnList) > 0
	begin
		set @sColumnName = left(@sColumnList, isnull(nullif(charindex(',', @sColumnList) - 1, -1),
                     len(@sColumnList)))
		set @sColumnList = substring(@sColumnList,isnull(nullif(charindex(',', @sColumnList), 0),
                               len(@sColumnList)) + 1, len(@sColumnList))

		set @sColumnName = RTRIM(LTRIM(replace(REPLACE(@sColumnName, '[',''), ']','')))
 
		  -- Check that each column exists in table with include.
		execute @bResult = DDLExistsColumn @sTableName output, @sColumnName, @sOwnerName output

		if @bResult = 1
			continue
		if @bResult = 0
			break
	end

	return @bResult
end
go

----------------------------------------------------------------------------------------------------
-- Existence Checks
----------------------------------------------------------------------------------------------------

if objectproperty(object_id('dbo.DDLExistsTable'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsTable
end
go
-- Table existence test.
create procedure dbo.DDLExistsTable
    @sName sysname output,
    @o_lID integer = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    select @o_sOwnerName = parsename(@sName, 2)
    select @sName = parsename(@sName, 1)
    if @o_sOwnerName is null begin
        select @o_sOwnerName = 'dbo'
    end

    select @o_lID = null
    select @o_lID = id
        from sysobjects
        where name = @sName
        and uid = user_id(@o_sOwnerName)
        and type = 'U'

    if @o_lID is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if objectproperty(object_id('dbo.DDLExistsView'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsView
end
go
-- View existence test.
create procedure dbo.DDLExistsView
    @sName sysname output,
    @o_lID integer = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    select @o_sOwnerName = parsename(@sName, 2)
    select @sName = parsename(@sName, 1)
    if @o_sOwnerName is null begin
        select @o_sOwnerName = 'dbo'
    end

    select @o_lID = null
    select @o_lID = id
        from sysobjects
        where name = @sName
        and uid = user_id(@o_sOwnerName)
        and type = 'V'

    if @o_lID is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if objectproperty(object_id('dbo.DDLExistsProcedure'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsProcedure
end
go
-- Procedure existence test.
create procedure dbo.DDLExistsProcedure    
    @sName sysname output,    
    @o_lID integer = null output,    
    @o_sOwnerName sysname = null output,    
 @o_return int = 0 output  
as begin    
    set nocount on    
    
    select @o_sOwnerName = parsename(@sName, 2)    
    select @sName = parsename(@sName, 1)    
    if @o_sOwnerName is null begin    
        select @o_sOwnerName = 'dbo'    
    end    
    
    select @o_lID = null    
    select @o_lID = id    
        from sysobjects    
        where name = @sName    
        and uid = user_id(@o_sOwnerName)    
        and type = 'P'    
    
    if @o_lID is not null begin    
  set @o_return=1  
        return 1    
    end else begin    
  set @o_return=0  
        return 0    
    end    
end 
go

if objectproperty(object_id('dbo.DDLExistsFunction'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsFunction
end
go
-- Function existence test.
create procedure dbo.DDLExistsFunction
    @sName sysname output,
    @o_lID integer = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    select @o_sOwnerName = parsename(@sName, 2)
    select @sName = parsename(@sName, 1)
    if @o_sOwnerName is null begin
        select @o_sOwnerName = 'dbo'
    end

    select @o_lID = null
    select @o_lID = id
        from sysobjects
        where name = @sName
        and uid = user_id(@o_sOwnerName)
        and type in ('FN', 'IF', 'TF')

    if @o_lID is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if objectproperty(object_id('dbo.DDLExistsTrigger'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsTrigger
end
go
-- Trigger existence test.
create procedure dbo.DDLExistsTrigger
    @sName sysname output,
    @o_lID integer = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    select @o_sOwnerName = parsename(@sName, 2)
    select @sName = parsename(@sName, 1)
    if @o_sOwnerName is null begin
        select @o_sOwnerName = 'dbo'
    end

    select @o_lID = null
    select @o_lID = id
        from sysobjects
        where name = @sName
        and uid = user_id(@o_sOwnerName)
        and type = 'TR'

    if @o_lID is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if objectproperty(object_id('dbo.DDLExistsColumn'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsColumn
end
go
-- Column existence test.
create procedure dbo.DDLExistsColumn
    @sTableName sysname output,
    @sColumnName sysname,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    if exists (select null from syscolumns where id = @lTableID and name = @sColumnName) begin
        return 1
    end else begin
        return 0
    end
end
go

if objectproperty(object_id('dbo.DDLExistsPrimaryKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsPrimaryKey
end
go
-- Primary key constraint existence test.
create procedure dbo.DDLExistsPrimaryKey
    @sTableName sysname output,
    @sColumnName1 sysname = null,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sConstraintName sysname = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the constraint.
    -- If no column names are specified, then we can find the name
    -- very easily. Otherwise, it cannot be found directly, but is
    -- always equal to the name of the matching unique index, so we
    -- search for that instead.
    select @o_sConstraintName = null
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

if objectproperty(object_id('dbo.DDLExistsAlternateKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsAlternateKey
end
go
-- Alternate key constraint existence test.
create procedure dbo.DDLExistsAlternateKey
    @sTableName sysname output,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sConstraintName sysname = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the constraint.
    -- This cannot be found directly, but is always equal to the name
    -- of the matching unique index, so we search for that instead.
    select @o_sConstraintName = null
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

if objectproperty(object_id('dbo.DDLExistsForeignKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsForeignKey
end
go
-- Foreign key constraint existence test.
create procedure dbo.DDLExistsForeignKey
    @sTableName sysname output,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sConstraintName sysname = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the constraint.
    select @o_sConstraintName = null
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

if objectproperty(object_id('dbo.DDLExistsIndex'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsIndex
end
go
-- Index existence test.
create procedure dbo.DDLExistsIndex
    @sTableName sysname output,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
	@sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @o_sIndexName sysname = null output,
    @o_bLinkedToConstraint integer = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer
    declare @nIndexStatus integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the index.
    select @o_sIndexName = null
    if @sColumnName8 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 8
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
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 7
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
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 6
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
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 5
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
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 4
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
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 3
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
            and index_col(@sTableName, indid, 3) = @sColumnName3
    end else if @sColumnName2 is not null begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 2
            and index_col(@sTableName, indid, 1) = @sColumnName1
            and index_col(@sTableName, indid, 2) = @sColumnName2
    end else begin
        select @o_sIndexName = name, @nIndexStatus = status
            from sysindexes
            where id = @lTableID
            and (status & 32) = 0
            and (select count(*) from sysindexkeys
                where id = sysindexes.id and indid = sysindexes.indid and keyno > 0) = 1
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

if objectproperty(object_id('dbo.DDLExistsCheck'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsCheck
end
go
-- Check constraint existence test.
create procedure dbo.DDLExistsCheck
    @sTableName sysname output,
    @sColumnName sysname,
    @o_sConstraintName sysname = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the constraint.
    select @o_sConstraintName = null
    select @o_sConstraintName = object_name(sysconstraints.constid)
        from sysconstraints
        inner join syscomments on sysconstraints.constid = syscomments.id
        where sysconstraints.id = @lTableID
        and (sysconstraints.status & 15) = 4
        and syscomments.text like '%' + @sColumnName + '%'

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if objectproperty(object_id('dbo.DDLExistsCheckDef'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsCheckDef
end
go
-- Check constraint existence test.
create procedure dbo.DDLExistsCheckDef
    @sTableName sysname output,
    @sCheckDefinition nvarchar(256),
    @o_sConstraintName sysname = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the constraint.
    select @o_sConstraintName = null
    select @o_sConstraintName = object_name(sysconstraints.constid)
        from sysconstraints
        inner join syscomments on sysconstraints.constid = syscomments.id
        where sysconstraints.id = @lTableID
        and (sysconstraints.status & 15) = 4
        and syscomments.text = @sCheckDefinition

    if @o_sConstraintName is not null begin
        return 1
    end else begin
        return 0
    end
end
go

if objectproperty(object_id('dbo.DDLExistsDefault'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLExistsDefault
end
go
-- Default constraint existence test.
create procedure dbo.DDLExistsDefault
    @sTableName sysname output,
    @sColumnName sysname,
    @o_sConstraintName sysname = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the constraint.
    select @o_sConstraintName = null
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

if objectproperty(object_id('dbo.DDLBoundRule'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLBoundRule
end
go
-- Rule existence test.
create procedure dbo.DDLBoundRule
    @sTableName sysname output,
    @sColumnName sysname,
    @o_sRuleName sysname = null output,
    @o_sOwnerName sysname = null output
as begin
    set nocount on

    declare @lTableID integer

    execute DDLExistsTable @sTableName output, @lTableID output, @o_sOwnerName output

    -- Find the name of the rule.
    select @o_sRuleName = null
    select @o_sRuleName = sysobjects.name
        from syscolumns
        inner join sysobjects on syscolumns.domain = sysobjects.id
        where syscolumns.id = @lTableID
        and syscolumns.name = @sColumnName
        and sysobjects.type = 'R'

    if @o_sRuleName is not null begin
        return 1
    end else begin
        return 0
    end
end
go


----------------------------------------------------------------------------------------------------
-- Create Objects
----------------------------------------------------------------------------------------------------

if objectproperty(object_id('dbo.DDLAddColumn'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddColumn
end
go
-- Create a table column.
create procedure dbo.DDLAddColumn
    @sTableName sysname,
    @sColumnName sysname,
    @sColumnDefinition nvarchar(256),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the column exists.
    execute @bExists = DDLExistsColumn @sTableName output, @sColumnName, @sOwnerName output

    if @bExists = 1 begin
        -- Column exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: column ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName) + ' already exists'
        end
    end else begin
        -- Column does not exist, so add it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add ' + quotename(@sColumnName) + ' ' + @sColumnDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLAddOrAlterColumn'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddOrAlterColumn
end
go
-- Create or alter a table column.
create procedure dbo.DDLAddOrAlterColumn
    @sTableName sysname,
    @sColumnName sysname,
    @sColumnDefinition nvarchar(256),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the column exists.
    execute @bExists = DDLExistsColumn @sTableName output, @sColumnName, @sOwnerName output

    if @bExists = 1 begin
        -- Column exists, so alter it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' alter column ' + quotename(@sColumnName) + ' ' + @sColumnDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end else begin
        -- Column does not exist, so add it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add ' + quotename(@sColumnName) + ' ' + @sColumnDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLAlterColumn'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAlterColumn
end
go
-- Create or alter a table column.
create procedure dbo.DDLAlterColumn
    @sTableName sysname,
    @sColumnName sysname,
    @sColumnDefinition nvarchar(256),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the column exists.
    execute @bExists = DDLExistsColumn @sTableName output, @sColumnName, @sOwnerName output

    if @bExists = 1 begin
        -- Column exists, so alter it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' alter column ' + quotename(@sColumnName) + ' ' + @sColumnDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end else begin
        -- Column does not exist, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: column ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName) + ' does not exist'
        end
    end
end
go

if objectproperty(object_id('dbo.DDLAddPrimaryKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddPrimaryKey
end
go
-- Create a primary key defined on a number of columns.
create procedure dbo.DDLAddPrimaryKey
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
    declare @sTableNameU sysname
    declare @sColumnNames nvarchar(1040)
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsPrimaryKey @sTableName output,
        null, null, null, null,
        null, null, null, null,
        @sConstraintName output,
        @sOwnerName output

    execute DDLEscapeNamePart @sTableName, @sTableNameU output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: primary key ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sConstraintName) + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('PK__' + @sTableNameU, 128)
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add constraint ' + quotename(@sConstraintName) + ' primary key clustered (' + @sColumnNames + ')'
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLAddAlternateKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddAlternateKey
end
go
-- Create an alternate key defined on a number of columns.
create procedure dbo.DDLAddAlternateKey
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
    declare @sTableNameU sysname
    declare @sColumnNames nvarchar(1040)
    declare @sColumnNamesU nvarchar(1040)
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsAlternateKey @sTableName output,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output,
        @sOwnerName output

    execute DDLEscapeNamePart @sTableName, @sTableNameU output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    execute DDLMakeNameList @sColumnNamesU output, '__',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: alternate key ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sConstraintName) + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('AK__' + @sTableNameU + '__' + @sColumnNamesU, 128)
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add constraint ' + quotename(@sConstraintName) + ' unique nonclustered (' + @sColumnNames + ')'
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLAddIndex'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddIndex
end
go
-- Create an index defined on a number of columns.
create procedure dbo.DDLAddIndex
    @sTableName sysname,
    @sColumnName1 sysname,
    @sColumnName2 sysname = null,
    @sColumnName3 sysname = null,
    @sColumnName4 sysname = null,
    @sColumnName5 sysname = null,
    @sColumnName6 sysname = null,
    @sColumnName7 sysname = null,
    @sColumnName8 sysname = null,
    @bQuiet tinyint = 0,
	@sIncludeColumnNames nvarchar(1040)	= null		--Include column list.
as begin
    set nocount on

    declare @bExists integer
    declare @sTableNameU sysname
    declare @sColumnNames nvarchar(1040)
    declare @sColumnNamesU nvarchar(1040)
    declare @sIndexName sysname
    declare @bLinkedToConstraint integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)
	declare @bIncludeColumnNamesValid bit

    -- Check that the index exists.
    execute @bExists = DDLExistsIndex @sTableName output,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sIndexName output,
        @bLinkedToConstraint output,
        @sOwnerName output

	if @sIncludeColumnNames is not null
	execute @bIncludeColumnNamesValid = DDLValidateColumnList @sTableName output, @sIncludeColumnNames

    execute DDLEscapeNamePart @sTableName, @sTableNameU output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    execute DDLMakeNameList @sColumnNamesU output, '__',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

	
    if @bExists = 1 begin
        -- Index exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: index ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sIndexName) + ' already exists'
        end
    end else begin
        -- Index does not exist, so add it.
        select @sIndexName = left('I__' + @sTableNameU + '__' + @sColumnNamesU, 128)
		if @sIncludeColumnNames is null
		        select @sSQL = 'create nonclustered index ' + quotename(@sIndexName) + ' on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + @sColumnNames + ')'
		else begin
			if @bIncludeColumnNamesValid = 1
		        select @sSQL = 'create nonclustered index ' + quotename(@sIndexName) + ' on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + @sColumnNames + ') INCLUDE (' + @sIncludeColumnNames + ')' 
			else begin
				 if isnull(@bQuiet, 0) = 0  
						print 'INFO: incorrect list of columns for include in creating index ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sIndexName) + '.'
			end

		end
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLAddForeignKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddForeignKey
end
go
-- Create a foreign key defined on a number of columns.
create procedure dbo.DDLAddForeignKey
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
    declare @sTableNameU sysname
    declare @sColumnNames nvarchar(1040)
    declare @sColumnNamesU nvarchar(1040)
    declare @sRefColumnNames nvarchar(1040)
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sRefOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsForeignKey @sTableName output,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output,
        @sOwnerName output

    execute DDLEscapeNamePart @sTableName, @sTableNameU output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    execute DDLMakeNameList @sRefColumnNames output, ', ',
        @sRefColumnName1, @sRefColumnName2, @sRefColumnName3, @sRefColumnName4,
        @sRefColumnName5, @sRefColumnName6, @sRefColumnName7, @sRefColumnName8

    execute DDLMakeNameList @sColumnNamesU output, '__',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    -- Parse the referenced table name.
    execute DDLExistsTable @sRefTableName output, null, @sRefOwnerName output

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: foreign key ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sConstraintName) + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('FK__' + @sTableNameU + '__' + @sColumnNamesU, 128)
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add constraint ' + quotename(@sConstraintName) + ' foreign key (' + @sColumnNames + ') references ' + quotename(@sRefOwnerName) + '.' + quotename(@sRefTableName)
        if @sRefColumnNames is not null begin
            select @sSQL = @sSQL + ' (' + @sRefColumnNames + ')'
        end
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
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

if objectproperty(object_id('dbo.DDLAddCheck'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddCheck
end
go
-- Create a check constraint defined on a number of columns.
create procedure dbo.DDLAddCheck
    @sTableName sysname,
    @sColumnName sysname,
    @sCheckDefinition nvarchar(256),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sTableNameU sysname
    declare @sColumnNameU sysname
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsCheckDef @sTableName output, @sCheckDefinition, @sConstraintName output, @sOwnerName output

    execute DDLEscapeNamePart @sTableName, @sTableNameU output

    execute DDLEscapeNamePart @sColumnName, @sColumnNameU output

    -- Make sure that the definition is enclosed in brackets.
    if left(@sCheckDefinition, 1) <> '(' or right(@sCheckDefinition, 1) <> ')' begin
        select @sCheckDefinition = '(' + @sCheckDefinition + ')'
    end

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: constraint ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sConstraintName) + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('CH__' + @sTableNameU + '__' + @sColumnNameU, 128)
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add constraint ' + quotename(@sConstraintName) + ' check ' + @sCheckDefinition
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLAddDefault'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddDefault
end
go
-- Create a default constraint on a column.
create procedure dbo.DDLAddDefault
    @sTableName sysname,
    @sColumnName sysname,
    @sDefaultValue nvarchar(256),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sTableNameU sysname
    declare @sColumnNameU sysname
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsDefault @sTableName output, @sColumnName, @sConstraintName output, @sOwnerName output

    execute DDLEscapeNamePart @sTableName, @sTableNameU output

    execute DDLEscapeNamePart @sColumnName, @sColumnNameU output

    if @bExists = 1 begin
        -- Constraint exists, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: constraint ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sConstraintName) + ' already exists'
        end
    end else begin
        -- Constraint does not exist, so add it.
        select @sConstraintName = left('DF__' + @sTableNameU + '__' + @sColumnNameU, 128)
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add constraint ' + quotename(@sConstraintName) + ' default ' + @sDefaultValue + ' for ' + quotename(@sColumnName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLAddOrAlterDefault'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddOrAlterDefault
end
go
-- Create or alter a default constraint on a column.
create procedure dbo.DDLAddOrAlterDefault
    @sTableName sysname,
    @sColumnName sysname,
    @sDefaultValue nvarchar(256),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sTableNameU sysname
    declare @sColumnNameU sysname
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsDefault @sTableName output, @sColumnName, @sConstraintName output, @sOwnerName output

    execute DDLEscapeNamePart @sTableName, @sTableNameU output

    execute DDLEscapeNamePart @sColumnName, @sColumnNameU output

    if @bExists = 1 begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end

    -- Constraint does not exist, so add it.
    select @sConstraintName = left('DF__' + @sTableNameU + '__' + @sColumnNameU, 128)
    select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' add constraint ' + quotename(@sConstraintName) + ' default ' + @sDefaultValue + ' for ' + quotename(@sColumnName)
    if isnull(@bQuiet, 0) = 0 begin
        print 'EXEC: ' + @sSQL
    end
    execute (@sSQL)
end
go

if objectproperty(object_id('dbo.DDLBindRule'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLBindRule
end
go
-- Bind a rule to a column.
create procedure dbo.DDLBindRule
    @sTableName sysname,
    @sColumnName sysname,
    @sRuleName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sRuleName2 sysname
    declare @sOwnerName sysname
    declare @sObjectName nvarchar(776)
    declare @sSQL nvarchar(512)

    -- Check that the rule is bound.
    execute @bExists = DDLBoundRule @sTableName, @sColumnName, @sRuleName2 output, @sOwnerName output

    if @bExists = 1 begin
        -- Rule is bound, so do nothing.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: column ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName) + ' is already bound to rule ' + quotename(@sRuleName2)
        end
    end else begin
        -- Rule is not bound, so bind it.
        select @sObjectName = quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName)
        select @sSQL = 'execute sp_bindrule ''' + replace(@sRuleName, '''', '''''') + ''', ''' + replace(@sObjectName, '''', '''''') + ''''
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute sp_bindrule @sRuleName, @sObjectName
    end
end
go

----------------------------------------------------------------------------------------------------
-- Drop Objects
----------------------------------------------------------------------------------------------------

if objectproperty(object_id('dbo.DDLDropTable'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropTable
end
go
-- Drop table.
create procedure dbo.DDLDropTable
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsTable @sName output, @o_sOwnerName = @sOwnerName output

    if @bExists = 1 begin
        -- Table exists, so drop it.
        select @sSQL = 'drop table ' + quotename(@sOwnerName) + '.' + quotename(@sName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropView'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropView
end
go
-- Drop view.
create procedure dbo.DDLDropView
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsView @sName output, @o_sOwnerName = @sOwnerName output

    if @bExists = 1 begin
        -- View exists, so drop it.
        select @sSQL = 'drop view ' + quotename(@sOwnerName) + '.' + quotename(@sName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropProcedure'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropProcedure
end
go
-- Drop procedure.
create procedure dbo.DDLDropProcedure
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsProcedure @sName output, @o_sOwnerName = @sOwnerName output

    if @bExists = 1 begin
        -- Procedure exists, so drop it.
        select @sSQL = 'drop procedure ' + quotename(@sOwnerName) + '.' + quotename(@sName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropFunction'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropFunction
end
go
-- Drop function.
create procedure dbo.DDLDropFunction
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsFunction @sName output, @o_sOwnerName = @sOwnerName output

    if @bExists = 1 begin
        -- Function exists, so drop it.
        select @sSQL = 'drop function ' + quotename(@sOwnerName) + '.' + quotename(@sName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropTrigger'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropTrigger
end
go
-- Drop trigger.
create procedure dbo.DDLDropTrigger
    @sName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    execute @bExists = DDLExistsTrigger @sName output, @o_sOwnerName = @sOwnerName output

    if @bExists = 1 begin
        -- Trigger exists, so drop it.
        select @sSQL = 'drop trigger ' + quotename(@sOwnerName) + '.' + quotename(@sName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropColumn'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropColumn
end
go
-- Drop a table column.
create procedure dbo.DDLDropColumn
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the column exists.
    execute @bExists = DDLExistsColumn @sTableName output, @sColumnName, @sOwnerName output

    if @bExists = 1 begin
        -- Column exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop column ' + quotename(@sColumnName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end else begin
        -- Column does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: column ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName) + ' does not exist'
        end
    end
end
go

if objectproperty(object_id('dbo.DDLDropPrimaryKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropPrimaryKey
end
go
-- Drop a primary key constraint defined on any number of columns.
create procedure dbo.DDLDropPrimaryKey
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
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsPrimaryKey @sTableName output,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output,
        @sOwnerName output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    if @bExists = 0 begin
        -- Constraint does not exist.
        if @sColumnNames is not null begin
            if isnull(@bQuiet, 0) = 0 begin
                print 'INFO: primary key on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + @sColumnNames + ') does not exist'
            end
        end else begin
            if isnull(@bQuiet, 0) = 0 begin
                print 'INFO: primary key on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' does not exist'
            end
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropAlternateKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropAlternateKey
end
go
-- Drop an alternate key defined on a number of columns.
create procedure dbo.DDLDropAlternateKey
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
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsAlternateKey @sTableName output,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output,
        @sOwnerName output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: alternate key on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + @sColumnNames + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropForeignKey'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropForeignKey
end
go
-- Drop a foreign key constraint defined on a number of columns.
create procedure dbo.DDLDropForeignKey
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
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsForeignKey @sTableName output,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sConstraintName output,
        @sOwnerName output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: foreign key on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + @sColumnNames + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropIndex'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropIndex
end
go
-- Drop an index defined on a number of columns.
create procedure dbo.DDLDropIndex
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
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the index exists.
    execute @bExists = DDLExistsIndex @sTableName output,
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8,
        @sIndexName output,
        @bLinkedToConstraint output,
        @sOwnerName output

    execute DDLMakeNameList @sColumnNames output, ', ',
        @sColumnName1, @sColumnName2, @sColumnName3, @sColumnName4,
        @sColumnName5, @sColumnName6, @sColumnName7, @sColumnName8

    if @bExists = 0 begin
        -- Index does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: index on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + @sColumnNames + ') does not exist'
        end
    end else if @bLinkedToConstraint = 1 begin
        -- Index is linked to a constraint.
        if isnull(@bQuiet, 0) = 0 begin
            print 'ERROR: index ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sIndexName) + ' is linked to a constraint'
        end
    end else begin
        -- Index exists, so drop it.
        select @sSQL = 'drop index ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sIndexName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropCheck'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropCheck
end
go
-- Drop a table-level check constraint containing the specified column name.
create procedure dbo.DDLDropCheck
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsCheck @sTableName output, @sColumnName, @sConstraintName output, @sOwnerName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: check on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + quotename(@sColumnName) + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropCheckDef'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropCheckDef
end
go
-- Drop a table-level check constraint with the specified definition.
create procedure dbo.DDLDropCheckDef
    @sTableName sysname,
    @sCheckDefinition nvarchar(256),
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsCheckDef @sTableName output, @sCheckDefinition, @sConstraintName output, @sOwnerName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: check ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + @sCheckDefinition + ') does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLDropDefault'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropDefault
end
go
-- Drop a default constraint defined on a column.
create procedure dbo.DDLDropDefault
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sConstraintName sysname
    declare @sOwnerName sysname
    declare @sSQL nvarchar(4000)

    -- Check that the constraint exists.
    execute @bExists = DDLExistsDefault @sTableName output, @sColumnName, @sConstraintName output, @sOwnerName output

    if @bExists = 0 begin
        -- Constraint does not exist.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: default for ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName) + ' does not exist'
        end
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

if objectproperty(object_id('dbo.DDLUnbindRule'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLUnbindRule
end
go
-- Unbind a rule from a column.
create procedure dbo.DDLUnbindRule
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists integer
    declare @sRuleName sysname
    declare @sOwnerName sysname
    declare @sObjectName nvarchar(776)
    declare @sSQL nvarchar(256)

    -- Check that the rule is bound.
    execute @bExists = DDLBoundRule @sTableName, @sColumnName, @sRuleName output, @sOwnerName output

    if @bExists = 0 begin
        -- Rule is not bound.
        if isnull(@bQuiet, 0) = 0 begin
            print 'INFO: column ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName) + ' is not bound to a rule'
        end
    end else begin
        -- Rule is bound, so un-bind it.
        select @sObjectName = quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sColumnName)
        select @sSQL = 'execute sp_unbindrule ''' + replace(@sObjectName, '''', '''''') + ''''
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute sp_unbindrule @sObjectName
    end
end
go

----------------------------------------------------------------------------------------------------
-- Obsolete
----------------------------------------------------------------------------------------------------

if objectproperty(object_id('dbo.DDLDropConstraint'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropConstraint
end
go
-- OBSOLETE, DO NOT USE!
create procedure dbo.DDLDropConstraint
    @sTableName sysname,
    @sColumnName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @lTableID integer
    declare @sOwnerName sysname
    declare @sConstraintName sysname
    declare @sSQL nvarchar(4000)

    print 'WARNING: DDLDropConstraint is obsolete and dangerous to use. Please replace with a more specific DDL procedure.'

    execute DDLExistsTable @sTableName output, @lTableID output, @sOwnerName output

    -- Find the name of the constraint.
    select @sConstraintName = object_name(sysconstraints.constid)
        from sysconstraints
        inner join syscolumns on sysconstraints.colid = syscolumns.colid
        where sysconstraints.id = @lTableID
        and syscolumns.name = @sColumnName

    if @@rowcount = 0 begin
        -- Constraint does not exist.
        print 'INFO: constraint on ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '(' + quotename(@sColumnName) + ') does not exist'
    end else begin
        -- Constraint exists, so drop it.
        select @sSQL = 'alter table ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + ' drop constraint ' + quotename(@sConstraintName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
    end
end
go

----------------------------------------------------------------------------------------------------
-- Utilities
----------------------------------------------------------------------------------------------------

if objectproperty(object_id('dbo.DDLDropDuplicatedForeignKeys'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropDuplicatedForeignKeys
end
go
-- Drop all duplicated foreign keys in the database.
create procedure dbo.DDLDropDuplicatedForeignKeys
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sSQL nvarchar(512)

    -- Create temp table to hold the duplicates.
    create table #Duplicates (
        TableID integer not null,
        ConstraintID integer not null,
        primary key (TableID, ConstraintID)
    )

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
    declare curDuplicates cursor local fast_forward for
        select 'alter table ' + quotename(object_name(TableID)) + ' drop constraint ' + quotename(object_name(ConstraintID))
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

if objectproperty(object_id('dbo.DDLDropStatistics'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLDropStatistics
end
go
-- Drop non-index statistics on tables.
create procedure dbo.DDLDropStatistics
    @sTableName sysname = null,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sOwnerName sysname
    declare @sIndexName sysname
    declare @sSQL nvarchar(4000)

    if @sTableName is null begin
        -- Find non-index statistics in all user tables.
        declare curStatistics cursor local fast_forward for
            select sysusers.name, sysobjects.name, sysindexes.name
            from sysindexes
            inner join sysobjects on sysindexes.id = sysobjects.id and sysobjects.type = 'U'
            inner join sysusers on sysobjects.uid = sysusers.uid
            where sysobjects.name <> 'dtproperties'
            and (sysindexes.status & 32) <> 0
            order by sysusers.name, sysobjects.name, sysindexes.name
    end else begin
        -- Find non-index statistics in the specified user table.
        declare curStatistics cursor local fast_forward for
            select sysusers.name, sysobjects.name, sysindexes.name
            from sysindexes
            inner join sysobjects on sysindexes.id = sysobjects.id and sysobjects.type = 'U'
            inner join sysusers on sysobjects.uid = sysusers.uid
            where sysobjects.name = @sTableName
            and (sysindexes.status & 32) <> 0
            order by sysusers.name, sysobjects.name, sysindexes.name
    end

    -- Drop the statistics found.
    open curStatistics
    fetch next from curStatistics into @sOwnerName, @sTableName, @sIndexName
    while @@fetch_status = 0 begin
        select @sSQL = 'drop statistics ' + quotename(@sOwnerName) + '.' + quotename(@sTableName) + '.' + quotename(@sIndexName)
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
        fetch next from curStatistics into @sOwnerName, @sTableName, @sIndexName
    end
    close curStatistics

    -- Destroy the cursor.
    deallocate curStatistics
end
go

if objectproperty(object_id('dbo.DDLFindUncheckedConstraints'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLFindUncheckedConstraints
end
go
-- Find all integrity constraints that are disabled or untrusted.
create procedure dbo.DDLFindUncheckedConstraints
as begin
    set nocount on

    -- Create temp table to hold the results.
    create table #Output (
        ConstraintID integer not null primary key,
        OwnerName sysname not null,
        TableName sysname not null,
        ConstraintName sysname not null,
        Disabled bit not null,
        NotTrusted bit not null,
        ColumnNames nvarchar(2048) null
    )

    -- Find all disabled or untrusted constraints.
    insert into #Output (ConstraintID, OwnerName, TableName, ConstraintName, Disabled, NotTrusted)
    select
        [constraint].id,
        [owner].name,
        [table].name,
        [constraint].name,
        objectproperty([constraint].id, 'CnstIsDisabled'),
        objectproperty([constraint].id, 'CnstIsNotTrusted')
    from sysobjects as [constraint]
    inner join sysobjects as [table] on [constraint].parent_obj = [table].id
    inner join sysusers as [owner] on [table].uid = [owner].uid
    where [constraint].type in ('C', 'F')
    and (objectproperty([constraint].id, 'CnstIsDisabled') = 1 or objectproperty([constraint].id, 'CnstIsNotTrusted') = 1)

    -- Fill the list of column names for each one.
    declare @lConstraintID integer
    declare @sColumnName sysname
    declare @sColumnNames nvarchar(2048)

    declare curOutput cursor local fast_forward for
        select ConstraintID from #Output

    open curOutput
    fetch next from curOutput into @lConstraintID
    while @@fetch_status = 0 begin

        select @sColumnNames = ''

        declare curColumns cursor local fast_forward for
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
        OwnerName,
        TableName,
        ConstraintName,
        Disabled,
        NotTrusted,
        ColumnNames
    from #Output
    order by
        OwnerName,
        TableName,
        ConstraintName

    -- Drop temp table.
    drop table #Output
end
go

if objectproperty(object_id('dbo.DDLEnableIntegrity'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLEnableIntegrity
end
go
-- Enable or disable all integrity constraints and triggers.
create procedure dbo.DDLEnableIntegrity
    @bEnabled tinyint,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @bExists tinyint
    declare @sOwnerName sysname
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
        declare curTables cursor local fast_forward for
	--s.name : schema name ,t.name : table name,not changing the variable names in the sp
	select s.name,t.name 
	from sys.tables t
	join sys.schemas s on s.schema_id = t.schema_id
	where t.type = 'U'
	and t.name <> 'dtproperties'			
	order by s.name,t.name

        -- Disable all constraints on each one.
        select @sSQL2 = ' nocheck constraint all'
        open curTables
        fetch next from curTables into @sOwnerName, @sTableName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + quotename(@sOwnerName) + '.' + quotename(@sTableName) + @sSQL2
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curTables into @sOwnerName, @sTableName
        end
        close curTables
        deallocate curTables

    end else if @bEnabled = 1 begin

        -- Find all disabled constraints.
        declare curConstraints cursor local fast_forward for
	--s.name : schema name ,t.name : table name,o.name : constraint name, not changing the variable names in the sp
	select s.name,t.name,o.name
	from sys.tables t
	join sys.schemas s on s.schema_id = t.schema_id
	join sys.sysconstraints c on c.id = t.object_id -- Parent Object ID
	join sys.sysobjects o on c.constid = o.id -- Object ID
	where t.type = 'U'
	and o.type in ('C','F')	
        and objectproperty(o.id, 'CnstIsDisabled') = 1
        order by s.name, t.name, o.name


        -- Enable each one but don't check existing data.
        select @sSQL2 = ' with nocheck check constraint '
        open curConstraints
        fetch next from curConstraints into @sOwnerName, @sTableName, @sConstraintName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + quotename(@sOwnerName) + '.' + quotename(@sTableName) + @sSQL2 + quotename(@sConstraintName)
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curConstraints into @sOwnerName, @sTableName, @sConstraintName
        end
        close curConstraints
        deallocate curConstraints

    end else if @bEnabled = 2 begin

        -- Create exclusion table if it doesn't already exist.
        if isnull(objectproperty(object_id('dbo.DDLConstraintsWithBadData'), 'IsTable'), 0) = 0 begin
            create table dbo.DDLConstraintsWithBadData (
                OwnerName sysname not null,
                TableName sysname not null,
                ConstraintName sysname not null,
                constraint PK__DDLConstraintsWithBadData primary key clustered (
                    OwnerName,
                    TableName,
                    ConstraintName
                )
            )
        end else if isnull(columnproperty(object_id('dbo.DDLConstraintsWithBadData'), 'OwnerName', 'AllowsNull'), 1) = 1 begin
            alter table dbo.DDLConstraintsWithBadData add OwnerName sysname not null default 'dbo'
        end

        -- Find all disabled or untrusted constraints not already excluded.
        declare curConstraints cursor local fast_forward for
	   --s.name : schema name ,t.name : table name,o.name : constraint name, not changing the variable names in the sp
	   select s.name,t.name,o.name
	   from sys.tables t
	   join sys.schemas s on s.schema_id = t.schema_id
	   join sys.sysconstraints c on c.id = t.object_id -- Parent Object ID
	   join sys.sysobjects o on c.constid = o.id -- Object ID
	   where t.type = 'U'
	   and o.type in ('C','F')	

            and (objectproperty(o.id, 'CnstIsDisabled') = 1 or
                objectproperty(o.id, 'CnstIsNotTrusted') = 1)
            and not exists (
                select null
                from DDLConstraintsWithBadData
                where OwnerName = s.name
                and TableName = t.name
                and ConstraintName = o.name
            )
            order by s.name, t.name, o.name

        -- Enable and check them. This loop may very well abort the batch if one of the
        -- constraints does not validate against existing table data. In this situation,
        -- the exclusion list will contain all constraints that failed.
        select @sSQL2 = ' with check check constraint '
        open curConstraints
        fetch next from curConstraints into @sOwnerName, @sTableName, @sConstraintName
        while @@fetch_status = 0 begin

            insert into DDLConstraintsWithBadData
                values (@sOwnerName, @sTableName, @sConstraintName)

            select @sSQL = @sSQL1 + quotename(@sOwnerName) + '.' + quotename(@sTableName) + @sSQL2 + quotename(@sConstraintName)
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            delete from DDLConstraintsWithBadData
                where OwnerName = @sOwnerName
                and TableName = @sTableName
                and ConstraintName = @sConstraintName

            fetch next from curConstraints into @sOwnerName, @sTableName, @sConstraintName
        end
        close curConstraints
        deallocate curConstraints

        -- If there are any rows in the exclusion list, then enable the constraint
        -- without checking it. The constraint will not be used by the query optimizer,
        -- but at least it will still check new data as it is inserted.
        if exists (select null from DDLConstraintsWithBadData) begin

            declare curConstraints cursor local fast_forward for
                select OwnerName, TableName, ConstraintName
                from DDLConstraintsWithBadData
                order by OwnerName, TableName, ConstraintName

            -- Enable them but don't check existing data.
            select @sSQL2 = ' with nocheck check constraint '
            open curConstraints
            fetch next from curConstraints into @sOwnerName, @sTableName, @sConstraintName
            while @@fetch_status = 0 begin

                select @sSQL = @sSQL1 + quotename(@sOwnerName) + '.' + quotename(@sTableName) + @sSQL2 + quotename(@sConstraintName)
                if isnull(@bQuiet, 0) = 0 begin
                    print 'EXEC: ' + @sSQL
                end
                execute sp_executesql @sSQL

                fetch next from curConstraints into @sOwnerName, @sTableName, @sConstraintName
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
        declare curTables cursor local fast_forward for
	--s.name : schema name ,t.name : table name,not changing the variable names in the sp
	select s.name,t.name 
	from sys.tables t
	join sys.schemas s on s.schema_id = t.schema_id
	where t.type = 'U'
	and t.name <> 'dtproperties'			
	order by s.name,t.name


        -- Disable all triggers on each one.
        select @sSQL2 = ' disable trigger all'
        open curTables
        fetch next from curTables into @sOwnerName, @sTableName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + quotename(@sOwnerName) + '.' + quotename(@sTableName) + @sSQL2
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curTables into @sOwnerName, @sTableName
        end
        close curTables
        deallocate curTables

    end else if @bEnabled = 1 begin

        -- Find all disabled triggers.
        declare curTriggers cursor local fast_forward for
	    --s.name : schema name ,t.name : table name,o.name : trigger name, not changing the variable names in the sp
 	    select s.name,t.name,o.name
	    from sys.tables t
	    join sys.schemas s on s.schema_id = t.schema_id
	    join sys.triggers tr on tr.parent_id = t.object_id -- Parent Object ID
	    join sys.sysobjects o on tr.object_id= o.id -- Object ID
	    where t.type = 'U'
	    and o.type in ('TR')

            and objectproperty(o.id, 'ExecIsTriggerDisabled') = 1
            order by s.name, t.name, tr.name

        -- Enable each one.
        select @sSQL2 = ' enable trigger '
        open curTriggers
        fetch next from curTriggers into @sOwnerName, @sTableName, @sTriggerName
        while @@fetch_status = 0 begin

            select @sSQL = @sSQL1 + quotename(@sOwnerName) + '.' + quotename(@sTableName) + @sSQL2 + quotename(@sTriggerName)
            if isnull(@bQuiet, 0) = 0 begin
                print 'EXEC: ' + @sSQL
            end
            execute sp_executesql @sSQL

            fetch next from curTriggers into @sOwnerName, @sTableName, @sTriggerName
        end
        close curTriggers
        deallocate curTriggers

    end
end
go

if objectproperty(object_id('dbo.DDLUpdateStatsAndRecompile'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLUpdateStatsAndRecompile
end
go
-- Update all statistics and recompile all procedures and triggers.
create procedure dbo.DDLUpdateStatsAndRecompile
    @nSamplePercent tinyint = 0,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sTableName sysname
    declare @sOwnerName sysname
    declare @sObjectName nvarchar(776)
    declare @sSamplingClause nvarchar(24)
    declare @sSQL nvarchar(256)

    if @nSamplePercent between 1 and 100 begin
        select @sSamplingClause = ' with sample ' + convert(nvarchar(3), @nSamplePercent) + ' percent'
    end else begin
        select @sSamplingClause = ''
    end

    -- Find all user tables.
    declare curTables cursor local fast_forward for
    --s.name : schema name ,t.name : table name,not changing the variable names in the sp
    select s.name,t.name
    from sys.tables t
    join sys.schemas s on s.schema_id = t.schema_id
    where t.type = 'U'
    and t.name <> 'dtproperties'
    order by s.name,t.name


    -- Update statistics for each table.
    open curTables
    fetch next from curTables into @sOwnerName, @sTableName
    while @@fetch_status = 0 begin
        select @sObjectName = quotename(@sOwnerName) + '.' + quotename(@sTableName)
        select @sSQL = 'update statistics ' + @sObjectName + @sSamplingClause
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute (@sSQL)
        fetch next from curTables into @sOwnerName, @sTableName
    end
    close curTables

    -- Recompile all code that references each table.
    open curTables
    fetch next from curTables into @sOwnerName, @sTableName
    while @@fetch_status = 0 begin
        select @sObjectName = quotename(@sOwnerName) + '.' + quotename(@sTableName)
        select @sSQL = 'execute sp_recompile ''' + replace(@sObjectName, '''', '''''') + ''''
        if isnull(@bQuiet, 0) = 0 begin
            print 'EXEC: ' + @sSQL
        end
        execute sp_recompile @sObjectName
        fetch next from curTables into @sOwnerName, @sTableName
    end
    close curTables

    -- Destroy the cursor.
    deallocate curTables
end
go

if objectproperty(object_id('dbo.DDLReSeedIdentity'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLReSeedIdentity
end
go
-- Re-seed identity value in a table
create procedure dbo.DDLReSeedIdentity
    @sTableName sysname,
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sOwnerName sysname
		
    execute DDLExistsTable @sTableName output, @o_sOwnerName = @sOwnerName output
    select @sTableName = quotename(@sOwnerName) + '.' + quotename(@sTableName)
    
	declare @sTableNameQ sysname
	declare @nIdentSeed integer
    declare @sTestSQL nvarchar(1000)
    declare @sSQL nvarchar(4000)

    select @sTableNameQ = '''' + replace(@sTableName, '''', '''''') + ''''
    select @nIdentSeed = ident_seed(@sTableName)

    select @sTestSQL = 'exists(select null from ' + @sTableName + ')'
    select @sTestSQL = @sTestSQL + ' or ident_current(' + @sTableNameQ + ') <> ' + convert(nvarchar(10), @nIdentSeed)

    select @sSQL = '
    if ' + @sTestSQL + ' begin
        dbcc checkident(' + @sTableNameQ + ', reseed, ' + convert(nvarchar(10), @nIdentSeed - 1) + ')
        dbcc checkident(' + @sTableNameQ + ', reseed)
    end else begin
        dbcc checkident(' + @sTableNameQ + ', reseed, ' + convert(nvarchar(10), @nIdentSeed) + ')
    end
    '

	 if isnull(@bQuiet, 0) = 0 begin
        print 'EXEC: ' + @sSQL
    end
    execute (@sSQL)
	
	declare @CurrentIdentity int     

	SELECT @CurrentIdentity = IDENT_CURRENT(@sTableName)
	If @CurrentIdentity =  0 
		DBCC CHECKIDENT (@sTableName, RESEED, 1)

end
go

if objectproperty(object_id('dbo.DDLReSeedIdentities'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLReSeedIdentities
end
go
-- Re-seed identity values in all tables.
create procedure dbo.DDLReSeedIdentities
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sOwnerName sysname
    declare @sTableName sysname
    declare @sObjectName nvarchar(776)

    -- Find all user tables that have an identity column.
		--s.name : schema name ,t.name : table name,not changing the variable names in the sp
		select s.name,t.name
		from sys.tables t
		join sys.schemas s on s.schema_id = t.schema_id
		where t.type = 'U'
		and t.name <> 'dtproperties'
        and exists (
            select null
            from syscolumns
            where syscolumns.id = t.object_id
            and (syscolumns.status & 128) <> 0
        )
        order by s.name,t.name

    -- Re-seed each one.
    open curTables
    fetch next from curTables into @sOwnerName, @sTableName
    while @@fetch_status = 0 begin
        select @sObjectName = quotename(@sOwnerName) + '.' + quotename(@sTableName)
        execute DDLReSeedIdentity @sObjectName, @bQuiet
        fetch next from curTables into @sOwnerName, @sTableName
    end
    close curTables

    -- Destroy the cursor.
    deallocate curTables
end
go

if objectproperty(object_id('dbo.DDLFreeCaches'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLFreeCaches
end
go
-- Free all data and procedure caches on the server.
create procedure dbo.DDLFreeCaches
as begin
    set nocount on

    dbcc dropcleanbuffers
    dbcc freeproccache
end
go

if objectproperty(object_id('dbo.DDLRepairAndDefrag'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLRepairAndDefrag
end
go
-- Repair and defrag the database.
create procedure dbo.DDLRepairAndDefrag
as begin
    set nocount on

    declare @sDatabaseName sysname

    select @sDatabaseName = db_name()

    -- Switch to single-user mode (required for checkdb).
    Exec spu_dboption @sDatabaseName, 'SINGLE_USER'
    -- Repair and rebuild all indexes.
    dbcc checkdb (@sDatabaseName, repair_rebuild)

    -- Restore multi-user mode.
    Exec spu_dboption @sDatabaseName, 'MULTI_USER'
end
go

if objectproperty(object_id('dbo.DDLTruncateLogAndShrinkFiles'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLTruncateLogAndShrinkFiles
end
go
-- Truncate the transaction log and shrink database files.
create procedure dbo.DDLTruncateLogAndShrinkFiles
    @bQuiet tinyint = 0
as begin
    set nocount on

    declare @sDatabaseName sysname
    declare @sSQL nvarchar(256)

    select @sDatabaseName = db_name()

    -- Remove all committed transactions from the log.
    --select @sSQL = 'backup log ' + quotename(@sDatabaseName) + ' with truncate_only'
    
	--this will no longer work on SQL Server 2008 R2 onwards according to msdn
	--if isnull(@bQuiet, 0) = 0 begin
    --    print 'EXEC: ' + @sSQL
    --end
    --execute (@sSQL)

    -- Physically shrink the database and transaction log files.
    select @sSQL = 'dbcc shrinkdatabase(' + quotename(@sDatabaseName) + ')'
    if isnull(@bQuiet, 0) = 0 begin
        print 'EXEC: ' + @sSQL
    end
    execute (@sSQL)
end
go
if objectproperty(object_id('dbo.DDLAddIdentityColumn'), 'IsProcedure') = 1 begin
    drop procedure dbo.DDLAddIdentityColumn
end
GO
CREATE PROCEDURE [dbo].[DDLAddIdentityColumn]  
    @sTableName sysname,  
    @sColumnName sysname  
AS   
BEGIN  
  
Declare @ColSQL VARCHAR(8000)  
Declare @ColSQL2 VARCHAR(8000)  
declare @col varchar(100)  
SET @colSQL=''  
SET @colSQL2=''  
  
SET NOCOUNT ON  
  
DECLARE @sSQL nvarchar(4000)  

SET @sSQL = 'EXEC DDLDropTable Temp_Table'  
EXEC (@sSQL)

SELECT @sSQL = 'SELECT * INTO Temp_Table FROM ' + @sTableName + ' Where 1=0'  
EXEC (@sSQL)  
SET @sSQL = 'EXEC DDLDropColumn Temp_Table , ' +   @sColumnName  
EXEC (@sSQL)  
SET @sSQL = 'EXEC DDLADDColumn Temp_Table , ' +   @sColumnName +', '+ '''INT IDENTITY'''  
EXEC (@sSQL)  
  
Declare Cur Cursor FOR       
    Select Name From SysColumns  Where ID IN(  
    Select ID from Sysobjects where name = @stablename )  
Open cur  
Fetch next from Cur into @col  
  
While @@Fetch_status=0   
BEGIN  
    IF @COLSQL=''   
       SET @COLSQL=  @COL    
    ELSe  
        SET @COLSQL= @COLSQL + ','+ @COL   
  Fetch next from Cur into @col  
END  
  
Close CUR  
deallocate CUR  
  
SET @COLSQL2 = '(' + @COLSQL + ')'  
  
SET IDENTITY_INSERT Temp_table ON  
  
-- Insert all data back to Original Table from Temp_Table   
  
SELECT @sSQL = 'INSERT INTO temp_table ' + @COLSQL2 + ' SELECT ' +@COLSQL  + ' FROM ' + @sTableName  
  
exec (@sSQL)  
SELECT @sSQL = 'SET IDENTITY_INSERT ' + @sTableName + ' Off'  
EXEC DDLDropTable @sTableName  
EXEC SP_RENAME temp_table , @sTableName  
  
END 

GO

IF objectproperty(object_id('dbo.DDLAddOrAlterExtendedProperty'), 'IsProcedure') = 1 BEGIN
    DROP PROCEDURE dbo.DDLAddOrAlterExtendedProperty
END
GO
CREATE PROCEDURE DDLAddOrAlterExtendedProperty
@TableName VARCHAR(100),
@ColumnName VARCHAR(100),
@Description VARCHAR(500)
AS

IF NOT EXISTS (SELECT 1 FROM sys.extended_properties  WHERE 
        name = N'MS_Description' 
        AND major_id = OBJECT_ID(@TableName) 
        AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(@TableName) AND name = @ColumnName)
)
EXEC sys.sp_addextendedproperty 
    @name=N'MS_Description', @value=@Description,
    @level0type=N'SCHEMA',@level0name=N'dbo', 
    @level1type=N'TABLE',@level1name=@TableName,
    @level2type=N'COLUMN',@level2name=@ColumnName
ELSE
	EXEC sys.sp_updateextendedproperty
    @name=N'MS_Description', @value=@Description,
    @level0type=N'SCHEMA',@level0name=N'dbo', 
    @level1type=N'TABLE',@level1name=@TableName,
    @level2type=N'COLUMN',@level2name=@ColumnName

GO  
  

----------------------------------------------------------------------------------------------------
-- End of file
----------------------------------------------------------------------------------------------------
