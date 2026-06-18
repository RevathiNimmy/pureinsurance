-- Deletes *ALL* data in the database. Be careful when running it!

begin
    declare @sTable varchar(128)
    declare @sSQL varchar(255)

    execute DDLEnableIntegrity 0

    set nocount on

    declare curTables cursor fast_forward for
        select name from sysobjects
        where type = 'U' and name <> 'dtproperties'
        order by name

    open curTables
    fetch next from curTables into @sTable

    while @@fetch_status = 0 begin
        print 'Truncating table ' + @sTable
        select @sSQL = 'delete from ' + @sTable
        execute (@sSQL)
        fetch next from curTables into @sTable
    end

    close curTables
    deallocate curTables

    set nocount off

    execute DDLEnableIntegrity 1
end
