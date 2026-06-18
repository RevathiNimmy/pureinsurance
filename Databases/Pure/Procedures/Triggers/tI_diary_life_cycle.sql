SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tI_diary_life_cycle'
go
create trigger tI_diary_life_cycle on diary_life_cycle for INSERT as
-- INSERT trigger on diary_life_cycle
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)
  select @numrows = @@rowcount
  -- diary_action_code R/12 diary_life_cycle ON CHILD INSERT RESTRICT
  if
    -- %ChildFK(" or",update)
    update(diary_action_code_id)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,diary_action_code
        where
          -- %JoinFKPK(inserted,diary_action_code)
          inserted.diary_action_code_id = diary_action_code.diary_action_code_id
    -- %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and")

    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT diary_life_cycle because diary_action_code does not exist.'
      goto error
    end
  end
  -- policy_life_cycle R/11 diary_life_cycle ON CHILD INSERT RESTRICT
  if
    -- %ChildFK(" or",update)
    update(action_no) or
    update(transaction_type) or
    update(gis_scheme_id)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,policy_life_cycle
        where
          -- %JoinFKPK(inserted,policy_life_cycle)
          inserted.action_no = policy_life_cycle.action_no and
          inserted.transaction_type = policy_life_cycle.transaction_type and
          inserted.gis_scheme_id = policy_life_cycle.gis_scheme_id
    -- %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and")

    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT diary_life_cycle because policy_life_cycle does not exist.'
      goto error
    end
  end
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

