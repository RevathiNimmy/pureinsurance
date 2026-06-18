SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tD_default_policy'
go
create trigger tD_default_policy on default_policy for DELETE as
-- DELETE trigger on default_policy
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    -- default_policy R/26 default_policy_xml ON PARENT DELETE RESTRICT
    if exists (
      select * from deleted,default_policy_xml
      where
        --  %JoinFKPK(default_policy_xml,deleted," = "," and")
        default_policy_xml.default_policy_id = deleted.default_policy_id
    )
    begin
      select @errno  = 30001,
             @errmsg = 'Cannot DELETE default_policy because default_policy_xml exists.'
      goto error
    end
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

