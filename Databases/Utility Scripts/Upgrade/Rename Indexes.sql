-- Renames all indexes to a consistent naming scheme.

begin
    declare @sTableName varchar(255)
    declare @sKeyName varchar(255)
    declare @lIndexID smallint
    declare @sNewName varchar(255)
    declare @iKey integer
    declare @sKey sysname
    declare @sSQL varchar(1000)

    set nocount on

    declare curIndexes cursor fast_forward for
        select o.name, i.name, i.indid
        from sysindexes i
        inner join sysobjects o on i.id = o.id
        where i.indid between 1 and 254
        and o.type = 'U'
        and (i.status & 8394784) = 0
        order by o.name, i.name

    open curIndexes
    fetch next from curIndexes into @sTableName, @sKeyName, @lIndexID

    while @@fetch_status = 0 begin
        select @sNewName = 'I__' + @sTableName

        select @iKey = 1
        select @sKey = index_col(@sTableName, @lIndexID, @iKey)
        while @sKey is not null begin
            select @sNewName = @sNewName + '__' + @sKey
            select @iKey = @iKey + 1
            select @sKey = index_col(@sTableName, @lIndexID, @iKey)
        end

        if @sKeyName <> @sNewName begin
            if exists(select null from sysindexes where name = @sNewName) begin
                print 'Dropping index ' + @sKeyName
                select @sSQL = 'drop index ' + @sTableName + '.' + @sKeyName
                execute (@sSQL)
            end else begin
                print 'Renaming index for ' + @sTableName + ' from ' + @sKeyName + ' to ' + @sNewName
                select @sKeyName = @sTableName + '.' + @sKeyName
                execute sp_rename @objname=@sKeyName, @newname=@sNewName, @objtype='INDEX'
            end
        end

        fetch next from curIndexes into @sTableName, @sKeyName, @lIndexID
    end

    close curIndexes
    deallocate curIndexes

    set nocount off
end
