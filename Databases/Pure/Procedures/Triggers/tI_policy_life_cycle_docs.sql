SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tI_policy_life_cycle_docs'
go
create trigger tI_policy_life_cycle_docs on policy_life_cycle_docs for INSERT as
-- INSERT trigger on policy_life_cycle_docs
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)
  select @numrows = @@rowcount
  -- documents R/18 policy_life_cycle_docs ON CHILD INSERT SET NULL
  if
    -- %ChildFK(" or",update)
    update(documents_id)
  begin
    update policy_life_cycle_docs
      set
        -- %SetFK(policy_life_cycle_docs,NULL)
        policy_life_cycle_docs.documents_id = NULL
      from policy_life_cycle_docs,inserted
      where
        -- %JoinPKPK(policy_life_cycle_docs,inserted," = "," and")
        policy_life_cycle_docs.action_no = inserted.action_no and
        policy_life_cycle_docs.transaction_type = inserted.transaction_type and
        policy_life_cycle_docs.gis_scheme_id = inserted.gis_scheme_id and
        policy_life_cycle_docs.policy_life_cycle_docs_id = inserted.policy_life_cycle_docs_id and
        not exists (
          select * from documents
          where
            -- %JoinFKPK(inserted,documents," = "," and")
            inserted.documents_id = documents.documents_id
        )
  end
  -- policy_life_cycle R/14 policy_life_cycle_docs ON CHILD INSERT RESTRICT
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
             @errmsg = 'Cannot INSERT policy_life_cycle_docs because policy_life_cycle does not exist.'
      goto error
    end
  end
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

