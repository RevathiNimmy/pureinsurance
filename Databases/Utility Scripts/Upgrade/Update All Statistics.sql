-- Updates all table statistics and recompiles all procedures.

begin
    declare @sTable varchar(128)
    declare @sSQL varchar(255)

    set nocount on

    declare curTables cursor fast_forward for
        select name from sysobjects
        where type = 'U' and name <> 'dtproperties'
        order by name

    open curTables
    fetch next from curTables into @sTable

    while @@fetch_status = 0 begin
        print 'Updating statistics on ' + @sTable
        select @sSQL = 'update statistics ' + @sTable
        execute (@sSQL)
        select @sSQL = 'execute sp_recompile ''' + @sTable + ''''
        execute (@sSQL)
        fetch next from curTables into @sTable
    end

    close curTables
    deallocate curTables

    set nocount off
end
