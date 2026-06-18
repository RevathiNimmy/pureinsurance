-- Renames all foreign key constraints to a consistent naming scheme.

begin
    declare @sTableName varchar(255)
    declare @sKeyName varchar(255)
    declare @sColumn1 varchar(255)
    declare @sColumn2 varchar(255)
    declare @sColumn3 varchar(255)
    declare @sColumn4 varchar(255)
    declare @sColumn5 varchar(255)
    declare @sNewName varchar(255)
    declare @sSQL varchar(1000)

    set nocount on

    declare curKeys cursor fast_forward for
        select o1.name, o2.name,
        isnull((select name from syscolumns where id = r.fkeyid and colid = r.fkey1),''),
        isnull((select name from syscolumns where id = r.fkeyid and colid = r.fkey2),''),
        isnull((select name from syscolumns where id = r.fkeyid and colid = r.fkey3),''),
        isnull((select name from syscolumns where id = r.fkeyid and colid = r.fkey4),''),
        isnull((select name from syscolumns where id = r.fkeyid and colid = r.fkey5),'')
        from sysreferences r
        inner join sysobjects o1 on r.fkeyid = o1.id
        inner join sysobjects o2 on r.constid = o2.id
        order by o1.name, o2.name

    open curKeys
    fetch next from curKeys into @sTableName, @sKeyName,
        @sColumn1, @sColumn2, @sColumn3, @sColumn4, @sColumn5

    while @@fetch_status = 0 begin
        select @sNewName = 'FK__' + @sTableName +
            (case when @sColumn1 <> '' then '__' + @sColumn1 else '' end) +
            (case when @sColumn2 <> '' then '__' + @sColumn2 else '' end) +
            (case when @sColumn3 <> '' then '__' + @sColumn3 else '' end) +
            (case when @sColumn4 <> '' then '__' + @sColumn4 else '' end) +
            (case when @sColumn5 <> '' then '__' + @sColumn5 else '' end)

        if @sKeyName <> @sNewName begin
            if exists(select null from sysobjects where name = @sNewName) begin
                print 'Dropping constraint ' + @sKeyName
                select @sSQL = 'alter table ' + @sTableName + ' drop constraint ' + @sKeyName
                execute (@sSQL)
            end else begin
                print 'Renaming foreign key for ' + @sTableName + ' from ' + @sKeyName + ' to ' + @sNewName
                execute sp_rename @objname=@sKeyName, @newname=@sNewName, @objtype='OBJECT'
            end
        end

        fetch next from curKeys into @sTableName, @sKeyName,
            @sColumn1, @sColumn2, @sColumn3, @sColumn4, @sColumn5
    end

    close curKeys
    deallocate curKeys

    set nocount off
end
