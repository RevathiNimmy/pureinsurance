-- Renames all alternate keys (unique constraints) to a consistent naming scheme.

begin
    declare @sTableName varchar(255)
    declare @sKeyName varchar(255)
    declare @lIndexID smallint
    declare @sNewName varchar(255)
    declare @iKey integer
    declare @sKey sysname
    declare @sSQL varchar(1000)

    set nocount on

    declare curKeys cursor fast_forward for
        select o.name, i.name, i.indid
        from sysindexes i
        inner join sysobjects o on i.id = o.id
        where i.indid between 1 and 254
        and o.type = 'U'
        and ((i.status ^ 4096) & 8394784) = 0
        order by o.name, i.name

    open curKeys
    fetch next from curKeys into @sTableName, @sKeyName, @lIndexID

    while @@fetch_status = 0 begin
        select @sNewName = 'AK__' + @sTableName

        select @iKey = 1
        select @sKey = index_col(@sTableName, @lIndexID, @iKey)
        while @sKey is not null begin
            select @sNewName = @sNewName + '__' + @sKey
            select @iKey = @iKey + 1
            select @sKey = index_col(@sTableName, @lIndexID, @iKey)
        end

        if @sKeyName <> @sNewName begin
            if exists(select null from sysobjects where name = @sNewName) begin
                print 'Dropping constraint ' + @sKeyName
                select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sKeyName
                execute (@sSQL)
            end else begin
                print 'Renaming alternate key for ' + @sTableName + ' from ' + @sKeyName + ' to ' + @sNewName
                execute sp_rename @objname=@sKeyName, @newname=@sNewName, @objtype='OBJECT'
            end
        end

        fetch next from curKeys into @sTableName, @sKeyName, @lIndexID
    end

    close curKeys
    deallocate curKeys

    set nocount off
end
