SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO
--**********************************************************************************************
-- Author : Prabodh Mishra
-- History: 20/08/2007 REL001 - Created
--	    17/10/2007 validation part moved into spu_SIR_Cover_Note_Sheet_Validate	
--**********************************************************************************************

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Sheet_Assign'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Sheet_Assign 
	@cover_note_book_number Varchar(50),
	@cover_note_sheet_number int,
	@insurance_file_cnt int,
	@user_id int
AS
Declare
	@cover_note_sheet_id int

BEGIN

	--Release cover note assigned to this policy version
	Update Cover_Note_Sheet
		SET insurance_file_cnt 			= NULL,
			assigned_date 			= NULL,
			last_updated 			= getdate(),
			user_id 			= @user_id,
			cover_note_sheet_status_id 	= CNSS.cover_note_sheet_status_id
		FROM Cover_Note_Sheet CNS, cover_note_sheet_status CNSS
		Where CNS.insurance_file_cnt = @insurance_file_cnt 
			AND CNSS.Code LIKE 'NOTISS' 

	-- If called without sheet number will only clear assigned sheet if there is any
	If (ISNULL(@cover_note_sheet_number, 0) = 0)
		Return

	SELECT @cover_note_sheet_id = CNS.Cover_Note_Sheet_Id
	From Cover_Note_Sheet CNS
		INNER JOIN Cover_Note_Sheet_Status CNSS 
			ON CNSS.Cover_Note_Sheet_Status_Id = CNS.Cover_Note_Sheet_Status_Id
		INNER JOIN Cover_Note_Book CNB
			ON CNB.Cover_Note_Book_Id = CNS.Cover_Note_Book_Id
		Where CNS.cover_sheet_number = @cover_note_sheet_number 
			AND UPPER(RTRIM(CNB.book_number)) = UPPER(RTRIM(@cover_note_book_number))
			AND cns.is_deleted = 0

	IF (ISNULL(@cover_note_sheet_id, 0) > 0)
	BEGIN
 	    Update Cover_Note_Sheet
		SET insurance_file_cnt 			= @insurance_file_cnt,
		    assigned_date 			= getdate(),
		    last_updated 			= getdate(),
		    user_id 				= @user_id,
		    cover_note_sheet_status_id 		= CNSS.cover_note_sheet_status_id
		FROM Cover_Note_Sheet CNS, cover_note_sheet_status CNSS
		Where CNS.cover_note_sheet_id = @cover_note_sheet_id 
			AND CNS.is_deleted = 0
			AND CNSS.Code LIKE 'ISSUED' 
	END
END

SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO

