SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Sheet_Add'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Sheet_Add
	@new_sheet_id int OUTPUT,
	@book_id int,
	@sheet_number int,
	@cover_note_sheet_status_id int = NULL,
	@comments varchar(1024),
	@user_id int
AS
BEGIN

If Exists (Select cover_sheet_number From Cover_Note_Sheet
		Where cover_note_book_id = @book_id and cover_sheet_number = @sheet_number and is_deleted = 0)
Begin
	Set @new_sheet_id = -1
	Return
End

    --Select status code for NOTISS sheets
    if (@cover_note_sheet_status_id IS NULL)
	Select @cover_note_sheet_status_id = cover_note_sheet_status_id 
	    From Cover_Note_Sheet_Status Where Code LIKE 'NOTISS'

	Insert Into Cover_Note_Sheet (
	cover_note_book_id,
	cover_sheet_number,
	cover_note_sheet_status_id,
	user_id,
	last_updated,
	date_imported,
	comments,
	is_deleted
	)
	Values (
	@book_id,
	@sheet_number,
	@cover_note_sheet_status_id,
	@user_id,
	getdate(),
	getdate(),
	@comments,
	0
	)	

	Select @new_sheet_id = @@identity
END

SET QUOTED_IDENTIFIER OFF    
GO
SET ANSI_NULLS OFF 
GO

