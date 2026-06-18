-- Renames all primary key constraints to a consistent naming scheme.

begin
    declare @sTableName varchar(255)
    declare @sKeyName varchar(255)
    declare @sNewName varchar(255)
    declare @sSQL varchar(1000)

    set nocount on

    declare curKeys cursor fast_forward for
        select o1.name, o2.name
        from sysconstraints c
        inner join sysobjects o1 on c.id = o1.id
        inner join sysobjects o2 on c.constid = o2.id
        where (c.status & 15) = 1
        order by o1.name, o2.name

    open curKeys
    fetch next from curKeys into @sTableName, @sKeyName

    while @@fetch_status = 0 begin
        select @sNewName = 'PK__' + @sTableName

        if @sKeyName <> @sNewName begin
            print 'Renaming primary key for ' + @sTableName + ' from ' + @sKeyName + ' to ' + @sNewName
            execute sp_rename @objname=@sKeyName, @newname=@sNewName, @objtype='OBJECT'
        end

        fetch next from curKeys into @sTableName, @sKeyName
    end

    close curKeys
    deallocate curKeys

    set nocount off
end
