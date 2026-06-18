-- Revokes all explicit object permissions.

begin
    declare @sObject varchar(128)
    declare @sSQL varchar(255)

    set nocount on

    declare curObjects cursor fast_forward for
        select name from sysobjects
        where type in ('U','V','P')
        and uid = 1
        and name <> 'dtproperties'
        and name not like 'dt[_]%'
        and name not like 'sys%'
        order by name

    open curObjects
    fetch next from curObjects into @sObject

    while @@fetch_status = 0 begin
        print 'Revoking permissions on ' + @sObject
        select @sSQL = 'revoke all on ' + @sObject + ' from [Gemini]'
        execute (@sSQL)
        select @sSQL = 'revoke all on ' + @sObject + ' from [public]'
        execute (@sSQL)
        select @sSQL = 'revoke all on ' + @sObject + ' from [SIRIUS]'
        execute (@sSQL)
        fetch next from curObjects into @sObject
    end

    close curObjects
    deallocate curObjects

    set nocount off
end
