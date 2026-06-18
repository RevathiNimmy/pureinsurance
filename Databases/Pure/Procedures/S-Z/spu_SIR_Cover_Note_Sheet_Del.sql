SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Sheet_Del'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Sheet_Del
	@sheet_id int,
	@user_id int
AS
BEGIN

	Update Cover_Note_Sheet
		SET  last_updated 	= getdate(),
			user_id 	= @user_id,
			is_deleted	= 1
		Where cover_note_sheet_id = @sheet_id

END

SET QUOTED_IDENTIFIER OFF    
GO
SET ANSI_NULLS OFF 
GO

