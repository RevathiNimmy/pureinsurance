SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tI_policy_life_cycle_actions'
go
create trigger tI_policy_life_cycle_actions on policy_life_cycle_actions for INSERT as
-- INSERT trigger on policy_life_cycle_actions
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)
  select @numrows = @@rowcount
  -- diary_action_code R/20 policy_life_cycle_actions ON CHILD INSERT SET NULL
  if
    -- %ChildFK(" or",update)
    update(diary_action_code_id)
  begin
    update policy_life_cycle_actions
      set
        -- %SetFK(policy_life_cycle_actions,NULL)
        policy_life_cycle_actions.diary_action_code_id = NULL
      from policy_life_cycle_actions,inserted
      where
        -- %JoinPKPK(policy_life_cycle_actions,inserted," = "," and")
        policy_life_cycle_actions.sequence_no = inserted.sequence_no and
        policy_life_cycle_actions.action_no = inserted.action_no and
        policy_life_cycle_actions.transaction_type = inserted.transaction_type and
        policy_life_cycle_actions.gis_scheme_id = inserted.gis_scheme_id and
        not exists (
          select * from diary_action_code
          where
            -- %JoinFKPK(inserted,diary_action_code," = "," and")
            inserted.diary_action_code_id = diary_action_code.diary_action_code_id
        )
  end
  -- policy_life_cycle R/10 policy_life_cycle_actions ON CHILD INSERT RESTRICT
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
             @errmsg = 'Cannot INSERT policy_life_cycle_actions because policy_life_cycle does not exist.'
      goto error
    end
  end
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

