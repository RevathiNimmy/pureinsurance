SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO

execute DDLDropTrigger 'tI_DOC_document'
go


/****** Object:  Trigger dbo.tI_DOC_document    Script Date: 01/07/99 11:15:20 ******/
create trigger tI_DOC_document on DOC_document for INSERT as
/* ERwin Builtin Wed Jul 01 13:44:38 1998 */
/* INSERT trigger on DOC_document */
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)

  select @numrows = @@rowcount
  /* ERwin Builtin Wed Jul 01 13:44:38 1998 */
  /* DOC_folder R/3 DOC_document ON CHILD INSERT RESTRICT */
  if
    /* %ChildFK(" or",update) */
    update(folder_num)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,DOC_folder
        where
          /* %JoinFKPK(inserted,DOC_folder) */
          inserted.folder_num = DOC_folder.folder_num
    /* %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and") */
    
    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT "DOC_document" because "DOC_folder" does not exist.'
      goto error
    end
  end


  /* ERwin Builtin Wed Jul 01 13:44:38 1998 */
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

