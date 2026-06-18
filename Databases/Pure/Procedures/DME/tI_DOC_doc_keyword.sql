SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO

execute DDLDropTrigger 'tI_DOC_doc_keyword'
go

/****** Object:  Trigger dbo.tI_DOC_doc_keyword    Script Date: 01/07/99 11:15:20 ******/
create trigger tI_DOC_doc_keyword on DOC_doc_keyword for INSERT as
/* ERwin Builtin Wed Jul 01 13:44:36 1998 */
/* INSERT trigger on DOC_doc_keyword */
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)

  select @numrows = @@rowcount
  /* ERwin Builtin Wed Jul 01 13:44:36 1998 */
  /* DOC_document R/11 DOC_doc_keyword ON CHILD INSERT RESTRICT */
  if
    /* %ChildFK(" or",update) */
    update(doc_num)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,DOC_document
        where
          /* %JoinFKPK(inserted,DOC_document) */
          inserted.doc_num = DOC_document.doc_num
    /* %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and") */
    
    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT "DOC_doc_keyword" because "DOC_document" does not exist.'
      goto error
    end
  end

  /* ERwin Builtin Wed Jul 01 13:44:36 1998 */
  /* DOC_keyword R/10 DOC_doc_keyword ON CHILD INSERT RESTRICT */
  if
    /* %ChildFK(" or",update) */
    update(DOC_keyword_id)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,DOC_keyword
        where
          /* %JoinFKPK(inserted,DOC_keyword) */
          inserted.keyword_id = DOC_keyword.keyword_id
    /* %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and") */
    select @nullcnt = count(*) from inserted where
      inserted.keyword_id is null
    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT "DOC_doc_keyword" because "DOC_keyword" does not exist.'
      goto error
    end
  end


  /* ERwin Builtin Wed Jul 01 13:44:36 1998 */
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

