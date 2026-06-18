SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Book_Upd'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Book_Upd
	@cover_note_book_id int OUTPUT,
	@effective_date datetime,
	@agent_cnt int,
	@source_id int,
	@cover_note_book_status_id int,
	@user_id int
AS
    BEGIN
	Update Cover_Note_Book Set
		effective_date  		= @effective_date,
		agent_cnt			= @agent_cnt,
		source_id 			= @source_id,
		cover_note_book_status_id	= @cover_note_book_status_id,
		user_id				= @user_id,
		last_updated			= getdate()
	Where cover_note_book_id = @cover_note_book_id
    END

SET QUOTED_IDENTIFIER OFF    
GO
SET ANSI_NULLS OFF 
GO

