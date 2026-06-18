SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO
--**********************************************************************************************
-- Author : Prabodh Mishra
-- History: 20/08/2007 REL001 - Created
-- Purpose: If called from Import routine translate IDs from Code 
-- Return Status: 1 - Ok 
-- 		  -1 - Book Number already exist
-- 		  -2 - Source doesn't exist
-- 		  -3 - Agent doesn't exist
-- 		  0 - Unknown Error
--**********************************************************************************************

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Book_Add'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Book_Add
	@new_book_id int OUTPUT,
	@book_number varchar(50),
	@start_number int,
	@end_number int,
	@effective_date datetime = NULL,
	@agent_cnt int = NULL,
	@source_id int = NULL,
	@cover_note_book_status_id int = NULL,
	@user_id int,
	@agent_code varchar(20) = NULL,
	@source_code varchar(20) = NULL
AS
BEGIN

Declare 
    @iSheets int,
    @cover_note_sheet_status_id int

If Exists (Select book_number From Cover_Note_Book Where book_number like @book_number)
Begin
	Set @new_book_id = -1
	Return
End

--Import will create books with ISSUED status
If (@cover_note_book_status_id IS NULL)
Begin
    Select @cover_note_book_status_id = cover_note_book_status_id 
	From Cover_Note_Book_Status Where Code LIKE 'ISSUED'

	--Import will call it without source_id
	If (@source_id IS NULL) 
	BEGIN
	    Select @source_id = source_id From Source Where code LIKE @source_code
	
	    if (ISNULL(@source_id, 0) = 0)
		Begin
		    SET @new_book_id = -2
		    Return	
		End
	END
	
	--Import will call it without agent_cnt
	If (@agent_cnt IS NULL) 
	Begin
	    Select @agent_cnt = party_cnt From Party Where shortname LIKE @agent_code
	
	    if (ISNULL(@agent_cnt, 0) = 0)
		Begin
		    SET @new_book_id = -3
		    Return	
		End
	End
End

IF (@effective_date IS NULL)
    Set @effective_date = GetDate()

Insert Into Cover_Note_Book (
	book_number,
	start_number,
	end_number,
	effective_date,
	agent_cnt,
	source_id,
	cover_note_book_status_id,
	user_id,
	created_date,
	last_updated
)
Values (
	@book_number,
	@start_number,
	@end_number,
	@effective_date,
	@agent_cnt,
	@source_id,
	@cover_note_book_status_id,
	@user_id,
	getdate(),
	getdate()
)

--Select status code for NOTISS sheets
Select @cover_note_sheet_status_id = cover_note_sheet_status_id 
	From Cover_Note_Sheet_Status Where Code LIKE 'NOTISS'

Select @new_book_id = @@identity
Set @iSheets = @start_number

   WHILE @iSheets <= @end_number
      BEGIN
	Insert Into Cover_Note_Sheet (
	cover_note_book_id,
	cover_sheet_number,
	cover_note_sheet_status_id,
	user_id,
	last_updated,
	date_imported,
	is_deleted
	)
	Values (
	@new_book_id,
	@iSheets,
	@cover_note_sheet_status_id,
	@user_id,
	getdate(),
	getdate(),
	0
	)	
	Set @iSheets = @iSheets + 1
      END
END

SET QUOTED_IDENTIFIER OFF    
GO
SET ANSI_NULLS OFF 
GO

