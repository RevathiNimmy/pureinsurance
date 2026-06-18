SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO


execute DDLDropTrigger 'tD_DOC_document'
go

/****** Object:  Trigger dbo.tD_DOC_document    Script Date: 01/07/99 11:15:20 ******/
create trigger tD_DOC_document on DOC_document for DELETE as
/* ERwin Builtin Wed Jul 01 13:44:38 1998 */
/* DELETE trigger on DOC_document */
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    /* ERwin Builtin Wed Jul 01 13:44:38 1998 */
    /* DOC_document R/13 DOC_annotation ON PARENT DELETE CASCADE */
    delete DOC_annotation
      from DOC_annotation,deleted
      where
        /*  %JoinFKPK(DOC_annotation,deleted," = "," and") */
        DOC_annotation.doc_num = deleted.doc_num

    /* ERwin Builtin Wed Jul 01 13:44:38 1998 */
    /* DOC_document R/12 DOC_doc_info ON PARENT DELETE CASCADE */
    delete DOC_doc_info
      from DOC_doc_info,deleted
      where
        /*  %JoinFKPK(DOC_doc_info,deleted," = "," and") */
        DOC_doc_info.doc_num = deleted.doc_num

    /* ERwin Builtin Wed Jul 01 13:44:38 1998 */
    /* DOC_document R/11 DOC_doc_keyword ON PARENT DELETE CASCADE */
    delete DOC_doc_keyword
      from DOC_doc_keyword,deleted
      where
        /*  %JoinFKPK(DOC_doc_keyword,deleted," = "," and") */
        DOC_doc_keyword.doc_num = deleted.doc_num


    /* ERwin Builtin Wed Jul 01 13:44:38 1998 */
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

