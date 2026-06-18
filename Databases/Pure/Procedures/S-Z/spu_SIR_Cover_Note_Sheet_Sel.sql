SET QUOTED_IDENTIFIER ON    
Go
SET ANSI_NULLS ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Sheet_Sel'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Sheet_Sel 
	@cover_note_book_id int,
	@cover_note_sheet_number int 
AS
BEGIN
    SELECT 
	CNS.cover_note_sheet_id,
	CNS.cover_sheet_number,
	CNS.insurance_file_cnt,
	ifi.insurance_ref,
	CNS.assigned_date,
	CNS.cover_note_sheet_status_id,	
	CNSS.code,
	CNSS.description,
	CNS.Comments
    FROM Cover_Note_Sheet CNS
    INNER JOIN Cover_Note_Sheet_Status CNSS ON CNSS.cover_note_sheet_status_id = CNS.cover_note_sheet_status_id
    LEFT JOIN Insurance_File ifi ON ifi.insurance_file_cnt = CNS.insurance_file_cnt
    WHERE cns.cover_note_book_id = @cover_note_book_id
	AND cns.cover_sheet_number = @cover_note_sheet_number
	AND CNS.is_deleted = 0

END

SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO
