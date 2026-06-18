SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tI_default_policy_xml'
go
create trigger tI_default_policy_xml on default_policy_xml for INSERT as
-- INSERT trigger on default_policy_xml
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)
  select @numrows = @@rowcount
  -- default_policy R/26 default_policy_xml ON CHILD INSERT RESTRICT
  if
    -- %ChildFK(" or",update)
    update(default_policy_id)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,default_policy
        where
          -- %JoinFKPK(inserted,default_policy)
          inserted.default_policy_id = default_policy.default_policy_id
    -- %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and")

    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT default_policy_xml because default_policy does not exist.'
      goto error
    end
  end
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

