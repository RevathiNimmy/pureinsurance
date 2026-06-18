SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tD_policy_life_cycle'
go
create trigger tD_policy_life_cycle on policy_life_cycle for DELETE as
-- DELETE trigger on policy_life_cycle
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    -- policy_life_cycle R/14 policy_life_cycle_docs ON PARENT DELETE RESTRICT
    if exists (
      select * from deleted,policy_life_cycle_docs
      where
        --  %JoinFKPK(policy_life_cycle_docs,deleted," = "," and")
        policy_life_cycle_docs.action_no = deleted.action_no and
        policy_life_cycle_docs.transaction_type = deleted.transaction_type and
        policy_life_cycle_docs.gis_scheme_id = deleted.gis_scheme_id
    )
    begin
      select @errno  = 30001,
             @errmsg = 'Cannot DELETE policy_life_cycle because policy_life_cycle_docs exists.'
      goto error
    end
    -- policy_life_cycle R/11 diary_life_cycle ON PARENT DELETE RESTRICT
    if exists (
      select * from deleted,diary_life_cycle
      where
        --  %JoinFKPK(diary_life_cycle,deleted," = "," and")
        diary_life_cycle.action_no = deleted.action_no and
        diary_life_cycle.transaction_type = deleted.transaction_type and
        diary_life_cycle.gis_scheme_id = deleted.gis_scheme_id
    )
    begin
      select @errno  = 30001,
             @errmsg = 'Cannot DELETE policy_life_cycle because diary_life_cycle exists.'
      goto error
    end
    -- policy_life_cycle R/10 policy_life_cycle_actions ON PARENT DELETE RESTRICT
    if exists (
      select * from deleted,policy_life_cycle_actions
      where
        --  %JoinFKPK(policy_life_cycle_actions,deleted," = "," and")
        policy_life_cycle_actions.action_no = deleted.action_no and
        policy_life_cycle_actions.transaction_type = deleted.transaction_type and
        policy_life_cycle_actions.gis_scheme_id = deleted.gis_scheme_id
    )
    begin
      select @errno  = 30001,
             @errmsg = 'Cannot DELETE policy_life_cycle because policy_life_cycle_actions exists.'
      goto error
    end
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

