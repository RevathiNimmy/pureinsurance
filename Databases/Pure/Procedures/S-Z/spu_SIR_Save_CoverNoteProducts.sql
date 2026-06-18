SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS  ON  
GO
--**********************************************************************************************
-- Author : Prabodh Mishra
-- History: 20/08/2007 REL001 - Created
-- Purpose: If called from Import routine translate ID from Code 
-- Return Status: 1 - Ok 
-- 		  -1 - Product doesn't exist
-- 		  0 - Unknown Error
--**********************************************************************************************

EXECUTE DDLDropProcedure 'spu_SIR_Save_CoverNoteProducts'
GO

CREATE PROCEDURE spu_SIR_Save_CoverNoteProducts
	@cover_note_book_id INT,
	@product_id INT = NULL,
	@product_code VARCHAR(20) = NULL,
	@return_status INT = NULL OUTPUT
AS
BEGIN
    SET @return_status = 0

    If (@product_id IS NULL)
    Begin
	Select @product_id = Product_Id From Product Where code LIKE @product_code

	if (ISNULL(@product_id, 0) = 0)
	    Begin
		SET @return_status = -1
		Return	
	    End
    End

    INSERT INTO Cover_Note_Book_Products (product_id, cover_note_book_id)
	VALUES (@product_id, @cover_note_book_id)
	
	SET @return_status = 1
END

SET QUOTED_IDENTIFIER  OFF    
GO
SET ANSI_NULLS  OFF 
GO
