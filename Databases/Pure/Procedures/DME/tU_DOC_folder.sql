SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO

execute DDLDropTrigger 'tU_DOC_folder'
go

/****** Object:  Trigger dbo.tU_DOC_folder    Script Date: 01/07/99 11:15:21 ******/
create trigger tU_DOC_folder on DOC_folder for UPDATE as
/* ERwin Builtin Wed Jul 01 13:44:39 1998 */
/* UPDATE trigger on DOC_folder */
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @insfolder_num int,
           @errno   int,
           @errmsg  varchar(255)

  select @numrows = @@rowcount
  /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
  /* DOC_folder R/3 DOC_document ON PARENT UPDATE RESTRICT */
  if
    /* %ParentPK(" or",update) */
    update(folder_num)
  begin
    if exists (
      select * from deleted,DOC_document
      where
        /*  %JoinFKPK(DOC_document,deleted," = "," and") */
        DOC_document.folder_num = deleted.folder_num
    )
    begin
      select @errno  = 30005,
             @errmsg = 'Cannot UPDATE "DOC_folder" because "DOC_document" exists.'
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

