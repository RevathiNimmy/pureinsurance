SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Sheet_Upd'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Sheet_Upd 
	@cover_note_book_id int,
	@old_cover_note_sheet_number int,
	@new_cover_note_sheet_number int,
	@insurance_file_cnt int,
	@assigned_date datetime,
	@cover_note_sheet_status_id int,
	@comments varchar(1024),
	@user_id int  
AS
BEGIN

Declare @lcover_note_sheet_status_id int

--Select status code for NOTISS sheets
Select @lcover_note_sheet_status_id = cover_note_sheet_status_id 
	From Cover_Note_Sheet_Status Where Code LIKE 'NOTISS'

    --Release old one and set the status to NOTISS 
    If (@old_cover_note_sheet_number <> @new_cover_note_sheet_number)
      UPDATE Cover_Note_Sheet 
	Set insurance_file_cnt = NULL,
	assigned_date = NULL,
	cover_note_sheet_status_id = @lcover_note_sheet_status_id,
	user_id = @user_id,
	last_updated = getdate()
      WHERE cover_sheet_number = @old_cover_note_sheet_number
	AND cover_note_book_id = @cover_note_book_id 
	AND is_deleted = 0
	
    UPDATE Cover_Note_Sheet 
	Set insurance_file_cnt = @insurance_file_cnt,
	assigned_date = @assigned_date,
	cover_note_sheet_status_id = @cover_note_sheet_status_id,
	comments = @comments,
	user_id = @user_id,
	last_updated = getdate()
    WHERE cover_sheet_number = @new_cover_note_sheet_number
	AND cover_note_book_id = @cover_note_book_id 
	AND is_deleted = 0

END

SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO
