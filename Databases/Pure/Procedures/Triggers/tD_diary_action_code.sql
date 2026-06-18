SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tD_diary_action_code'
go
create trigger tD_diary_action_code on diary_action_code for DELETE as
-- DELETE trigger on diary_action_code
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    -- diary_action_code R/20 policy_life_cycle_actions ON PARENT DELETE SET NULL
    update policy_life_cycle_actions
      set
        -- %SetFK(policy_life_cycle_actions,NULL)
        policy_life_cycle_actions.diary_action_code_id = NULL
      from policy_life_cycle_actions,deleted
      where
        -- %JoinFKPK(policy_life_cycle_actions,deleted," = "," and")
        policy_life_cycle_actions.diary_action_code_id = deleted.diary_action_code_id
    -- diary_action_code R/12 diary_life_cycle ON PARENT DELETE RESTRICT
    if exists (
      select * from deleted,diary_life_cycle
      where
        --  %JoinFKPK(diary_life_cycle,deleted," = "," and")
        diary_life_cycle.diary_action_code_id = deleted.diary_action_code_id
    )
    begin
      select @errno  = 30001,
             @errmsg = 'Cannot DELETE diary_action_code because diary_life_cycle exists.'
      goto error
    end
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

