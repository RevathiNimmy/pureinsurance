SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS  ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Delete_CoverNoteProducts'
GO

CREATE PROCEDURE spu_SIR_Delete_CoverNoteProducts
	@product_id int,
	@cover_note_book_id INT
AS

DELETE FROM Cover_Note_Book_Products
WHERE Cover_Note_Book_Id = @cover_note_book_id

GO

SET QUOTED_IDENTIFIER  OFF    
GO
SET ANSI_NULLS  OFF 
GO

