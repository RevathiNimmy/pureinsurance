SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  OFF 
GO

execute DDLDropTrigger 'tD_DOC_folder'
go

/****** Object:  Trigger dbo.tD_DOC_folder    Script Date: 01/07/99 11:15:20 ******/
create trigger tD_DOC_folder on DOC_folder for DELETE as
/* ERwin Builtin Wed Jul 01 13:44:39 1998 */
/* DELETE trigger on DOC_folder */
begin
  declare  @errno   int,
           @errmsg  varchar(255)
    /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
    /* DOC_folder R/3 DOC_document ON PARENT DELETE CASCADE */
    delete DOC_document
      from DOC_document,deleted
      where
        /*  %JoinFKPK(DOC_document,deleted," = "," and") */
        DOC_document.folder_num = deleted.folder_num


    /* ERwin Builtin Wed Jul 01 13:44:39 1998 */
    return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

