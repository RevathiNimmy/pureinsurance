SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tD_diary_life_cycle'
go
create trigger tD_diary_life_cycle on diary_life_cycle for DELETE as
-- DELETE trigger on diary_life_cycle
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    -- diary_life_cycle R/16 diary_life_cycle_docs ON PARENT DELETE RESTRICT
    if exists (
      select * from deleted,diary_life_cycle_docs
      where
        --  %JoinFKPK(diary_life_cycle_docs,deleted," = "," and")
        diary_life_cycle_docs.action_no = deleted.action_no and
        diary_life_cycle_docs.transaction_type = deleted.transaction_type and
        diary_life_cycle_docs.gis_scheme_id = deleted.gis_scheme_id and
        diary_life_cycle_docs.diary_action_code_id = deleted.diary_action_code_id
    )
    begin
      select @errno  = 30001,
             @errmsg = 'Cannot DELETE diary_life_cycle because diary_life_cycle_docs exists.'
      goto error
    end
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

