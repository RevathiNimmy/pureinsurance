SET QUOTED_IDENTIFIER ON    
GO
SET ANSI_NULLS  ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_SelectAll_CoverNoteProducts'
GO

CREATE PROCEDURE spu_SIR_SelectAll_CoverNoteProducts
	@cover_note_book_id INT
AS

SET NOCOUNT ON

	SELECT	P.Product_Id, P.description,
		CASE WHEN CN_P.Product_id IS Null THEN 0 ELSE 1 END As Chosen,
--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)   
P.Code ProductCode    
--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)   
	FROM Product P	LEFT JOIN Cover_Note_Book_Products CN_P 
		ON P.Product_Id = CN_P.Product_id AND CN_P.Cover_Note_Book_Id = @cover_note_book_id
	WHERE	p.is_deleted = 0


SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER  OFF    
GO
SET ANSI_NULLS  OFF 
GO
