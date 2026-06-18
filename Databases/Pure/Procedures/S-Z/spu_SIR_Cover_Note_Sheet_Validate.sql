SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO
--**********************************************************************************************
-- Author : Prabodh Mishra
-- History: 17/10/2007 REL001 - Created
-- Return Status: 1 - Ok 
-- 		  2 - Book doesn't exist
-- 		  3 - Book not available for this agent
-- 		  4 - Book not available for this product
-- 		  5 - Book not available for this branch
-- 		  6 - No sheets left in book
-- 		  7 - Sheet Number out of range
-- 		  8 - Sheet is already used
--**********************************************************************************************

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Sheet_Validate'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Sheet_Validate
	@sCover_Note_Book_Number Varchar(50),
	@lCover_Note_Sheet_Number int,
	@lInsurance_File_Cnt int,
	@lBranch_Id int,
	@lAgent_Cnt int,
	@lProduct_Id int,
	@lReturn_Status int output,
	@sSheet_Status varchar(20) output
AS
Declare
	@lCoverNoteBookId int,
	@lAgentCnt int,
	@lBranchId int,
	@lStartNumber int,
	@lEndNumber int,
	@lInsFileCnt int
	
BEGIN
	--Check if sheet is already assigned to same policy
	IF (@lInsurance_File_Cnt > 0) 
		SELECT @lInsFileCnt = CNS.Insurance_File_Cnt
	  	    From Cover_Note_Sheet CNS
			INNER JOIN Cover_Note_Book CNB
			ON CNB.Cover_Note_Book_Id = CNS.Cover_Note_Book_Id
			Where CNS.cover_sheet_number = @lCover_Note_Sheet_Number 
			AND UPPER(RTRIM(CNB.book_number)) = UPPER(RTRIM(@sCover_Note_Book_Number))
			AND CNS.insurance_file_cnt = @lInsurance_File_Cnt

	--Check If book available
	If not exists(Select CNB.cover_note_book_id From Cover_Note_Book CNB
			Inner JOIN Cover_Note_Book_Status CNBS 
			ON CNB.cover_note_book_status_id = CNBS.cover_note_book_status_id
			Where UPPER(RTRIM(CNB.book_number)) = UPPER(RTRIM(@sCover_Note_Book_Number))
			AND CNBS.Code = 'ISSUED'
			AND DateDiff(Day, CNB.effective_date, getdate()) >= 0)
	BEGIN
		Set @lReturn_Status = 2		
		return 
	END

	Select @lCoverNoteBookId = CNB.cover_note_book_id, 
		@lStartNumber = CNB.start_number,
		@lEndNumber = CNB.end_number,
		@lAgentCnt = CNB.agent_cnt,
		@lBranchId = CNB.source_id
	From Cover_Note_Book CNB
		Inner JOIN Cover_Note_Book_Status CNBS 
			ON CNB.cover_note_book_status_id = CNBS.cover_note_book_status_id
		Where UPPER(RTRIM(CNB.book_number)) = UPPER(RTRIM(@sCover_Note_Book_Number))

	--Most unlikely, Just for Resilience
	If (ISNULL(@lCoverNoteBookId, 0) = 0)
	BEGIN
		Set @lReturn_Status = 2		
		return 
	END

	--Check If sheet number is in range
	If (@lCover_Note_Sheet_Number < @lStartNumber OR @lCover_Note_Sheet_Number > @lEndNumber)
	BEGIN
		Set @lReturn_Status = 7
		return 
	END

	If (ISNULL(@lInsFileCnt, 0) <> @lInsurance_File_Cnt OR @lInsurance_File_Cnt = 0)
	    Begin
		Select @sSheet_Status = CNSS.code From Cover_Note_Sheet CNS
			INNER JOIN Cover_Note_Sheet_Status CNSS
				ON CNSS.Cover_Note_Sheet_Status_Id = CNS.Cover_Note_Sheet_Status_Id
			Where CNS.Cover_Note_Book_Id = @lCoverNoteBookId
				AND CNS.Cover_Sheet_Number = @lCover_Note_Sheet_Number
				AND CNS.is_deleted = 0
		--Check If sheet number is available
		IF (ISNULL(@sSheet_Status, '') <> 'NOTISS')
		BEGIN
			Set @lReturn_Status = 8
			return 
		END
	    End

	--Check If book available to branch
	If (@lBranchId <> @lBranch_Id)
	BEGIN
		Set @lReturn_Status = 5		
		return 
	END

	--Check If book available to agent
	If (@lAgentCnt <> @lAgent_Cnt)
	BEGIN
		Set @lReturn_Status = 3
		return 
	END

	--Check If book available to policy product
	If not exists(SELECT * From Cover_Note_Book_Products 
			Where product_id = @lProduct_Id AND cover_note_book_id = @lCoverNoteBookId)
	BEGIN
		Set @lReturn_Status = 4
		return 
	END

	--Return Ok	
	Set @lReturn_Status = 1
END

SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO

