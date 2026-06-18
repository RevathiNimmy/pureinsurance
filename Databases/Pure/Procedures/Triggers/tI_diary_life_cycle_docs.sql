SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tI_diary_life_cycle_docs'
go
create trigger tI_diary_life_cycle_docs on diary_life_cycle_docs for INSERT as
-- INSERT trigger on diary_life_cycle_docs
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)
  select @numrows = @@rowcount
  -- documents R/19 diary_life_cycle_docs ON CHILD INSERT SET NULL
  if
    -- %ChildFK(" or",update)
    update(documents_id)
  begin
    update diary_life_cycle_docs
      set
        -- %SetFK(diary_life_cycle_docs,NULL)
        diary_life_cycle_docs.documents_id = NULL
      from diary_life_cycle_docs,inserted
      where
        -- %JoinPKPK(diary_life_cycle_docs,inserted," = "," and")
        diary_life_cycle_docs.action_no = inserted.action_no and
        diary_life_cycle_docs.transaction_type = inserted.transaction_type and
        diary_life_cycle_docs.gis_scheme_id = inserted.gis_scheme_id and
        diary_life_cycle_docs.diary_action_code_id = inserted.diary_action_code_id and
        diary_life_cycle_docs.diary_life_cycle_docs_id = inserted.diary_life_cycle_docs_id and
        not exists (
          select * from documents
          where
            -- %JoinFKPK(inserted,documents," = "," and")
            inserted.documents_id = documents.documents_id
        )
  end
  -- diary_life_cycle R/16 diary_life_cycle_docs ON CHILD INSERT RESTRICT
  if
    -- %ChildFK(" or",update)
    update(action_no) or
    update(transaction_type) or
    update(gis_scheme_id) or
    update(diary_action_code_id)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,diary_life_cycle
        where
          -- %JoinFKPK(inserted,diary_life_cycle)
          inserted.action_no = diary_life_cycle.action_no and
          inserted.transaction_type = diary_life_cycle.transaction_type and
          inserted.gis_scheme_id = diary_life_cycle.gis_scheme_id and
          inserted.diary_action_code_id = diary_life_cycle.diary_action_code_id
    -- %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and")

    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT diary_life_cycle_docs because diary_life_cycle does not exist.'
      goto error
    end
  end
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

