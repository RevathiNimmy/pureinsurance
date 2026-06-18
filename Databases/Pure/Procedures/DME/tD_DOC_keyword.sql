SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO

execute DDLDropTrigger 'tD_DOC_keyword'
go

/****** Object:  Trigger dbo.tD_DOC_keyword    Script Date: 01/07/99 11:15:20 ******/
create trigger tD_DOC_keyword on DOC_keyword for DELETE as
/* ERwin Builtin Wed Jul 01 13:44:37 1998 */
/* DELETE trigger on DOC_keyword */
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    /* ERwin Builtin Wed Jul 01 13:44:37 1998 */
    /* DOC_keyword R/10 DOC_doc_keyword ON PARENT DELETE RESTRICT */
    if exists (
      select * from deleted,DOC_doc_keyword
      where
        /*  %JoinFKPK(DOC_doc_keyword,deleted," = "," and") */
        DOC_doc_keyword.keyword_id = deleted.keyword_id
    )
    begin
      select @errno  = 30001,
             @errmsg = 'Cannot DELETE "DOC_keyword" because "DOC_doc_keyword" exists.'
      goto error
    end


    /* ERwin Builtin Wed Jul 01 13:44:37 1998 */
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

