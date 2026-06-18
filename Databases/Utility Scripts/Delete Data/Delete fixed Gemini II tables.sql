begin
    declare @sTable varchar(255)

    declare curTables cursor fast_forward for
        select name from sysobjects
        where type = 'U' and name like 'GII%'
        order by name

    open curTables
    fetch curTables into @sTable
    while @@fetch_status = 0 begin
        print @sTable
        execute ('delete from ' + @sTable)
        fetch curTables into @sTable
    end
    close curTables
    deallocate curTables
end
