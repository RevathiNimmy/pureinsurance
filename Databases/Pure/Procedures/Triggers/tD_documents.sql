SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tD_documents'
go
create trigger tD_documents on documents for DELETE as
-- DELETE trigger on documents
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    -- documents R/22 PVD_Data_Docs ON PARENT DELETE SET NULL
    update PVD_Data_Docs
      set
        -- %SetFK(PVD_Data_Docs,NULL)
        PVD_Data_Docs.documents_id = NULL
      from PVD_Data_Docs,deleted
      where
        -- %JoinFKPK(PVD_Data_Docs,deleted," = "," and")
        PVD_Data_Docs.documents_id = deleted.documents_id
    -- documents R/19 diary_life_cycle_docs ON PARENT DELETE SET NULL
    update diary_life_cycle_docs
      set
        -- %SetFK(diary_life_cycle_docs,NULL)
        diary_life_cycle_docs.documents_id = NULL
      from diary_life_cycle_docs,deleted
      where
        -- %JoinFKPK(diary_life_cycle_docs,deleted," = "," and")
        diary_life_cycle_docs.documents_id = deleted.documents_id
    -- documents R/18 policy_life_cycle_docs ON PARENT DELETE SET NULL
    update policy_life_cycle_docs
      set
        -- %SetFK(policy_life_cycle_docs,NULL)
        policy_life_cycle_docs.documents_id = NULL
      from policy_life_cycle_docs,deleted
      where
        -- %JoinFKPK(policy_life_cycle_docs,deleted," = "," and")
        policy_life_cycle_docs.documents_id = deleted.documents_id
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

