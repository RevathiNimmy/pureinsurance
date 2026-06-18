SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO

execute DDLDropTrigger 'tU_DOC_page'
go


create trigger tU_DOC_page on DOC_page for UPDATE as
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @inspage_name char(15),
           @errno   int,
           @errmsg  varchar(255)

  select @numrows = @@rowcount
  if
    update(doc_num)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,DOC_document
        where
          inserted.doc_num = DOC_document.doc_num
  
    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30007,
             @errmsg = 'Cannot UPDATE DOC_page because DOC_document does not exist.'
      goto error
    end
  end

 return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

