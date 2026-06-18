SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO

execute DDLDropTrigger 'tU_DOC_document'
go

/****** Object:  Trigger dbo.tU_DOC_document    Script Date: 01/07/99 11:15:21 ******/
create trigger tU_DOC_document on DOC_document for UPDATE as
/* ERwin Builtin Wed Jul 01 13:44:39 1998 */
/* UPDATE trigger on DOC_document */
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @insdoc_num int,
           @errno   int,
           @errmsg  varchar(255)

  select @numrows = @@rowcount
  /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
  /* DOC_document R/13 DOC_annotation ON PARENT UPDATE RESTRICT */
  if
    /* %ParentPK(" or",update) */
    update(doc_num)
  begin
    if exists (
      select * from deleted,DOC_annotation
      where
        /*  %JoinFKPK(DOC_annotation,deleted," = "," and") */
        DOC_annotation.doc_num = deleted.doc_num
    )
    begin
      select @errno  = 30005,
             @errmsg = 'Cannot UPDATE "DOC_document" because "DOC_annotation" exists.'
      goto error
    end
  end

  /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
  /* DOC_document R/12 DOC_doc_info ON PARENT UPDATE RESTRICT */
  if
    /* %ParentPK(" or",update) */
    update(doc_num)
  begin
    if exists (
      select * from deleted,DOC_doc_info
      where
        /*  %JoinFKPK(DOC_doc_info,deleted," = "," and") */
        DOC_doc_info.doc_num = deleted.doc_num
    )
    begin
      select @errno  = 30005,
             @errmsg = 'Cannot UPDATE "DOC_document" because "DOC_doc_info" exists.'
      goto error
    end
  end

  /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
  /* DOC_document R/11 DOC_doc_keyword ON PARENT UPDATE RESTRICT */
  if
    /* %ParentPK(" or",update) */
    update(doc_num)
  begin
    if exists (
      select * from deleted,DOC_doc_keyword
      where
        /*  %JoinFKPK(DOC_doc_keyword,deleted," = "," and") */
        DOC_doc_keyword.doc_num = deleted.doc_num
    )
    begin
      select @errno  = 30005,
             @errmsg = 'Cannot UPDATE "DOC_document" because "DOC_doc_keyword" exists.'
      goto error
    end
  end

  /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
  /* DOC_document R/4 DOC_page ON PARENT UPDATE RESTRICT */
  if
    /* %ParentPK(" or",update) */
    update(doc_num)
  begin
    if exists (
      select * from deleted,DOC_page
      where
        /*  %JoinFKPK(DOC_page,deleted," = "," and") */
        DOC_page.doc_num = deleted.doc_num
    )
    begin
      select @errno  = 30005,
             @errmsg = 'Cannot UPDATE "DOC_document" because "DOC_page" exists.'
      goto error
    end
  end


  /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

