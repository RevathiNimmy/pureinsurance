SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Sheet_Get'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Sheet_Get
	@cover_note_book_id int 
AS
BEGIN
    SELECT TOP 500
	cns.cover_note_sheet_id,
	cns.cover_sheet_number,
	ifi.insured_name,
	CNS.cover_note_sheet_status_id,	
	CNSS.description,
	ifi.insurance_ref,
	s.description,
	p.name,
 cns.date_imported  , 
--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)      
CNSS.Code CoverNoteSheetStatusCode    
--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)      
    FROM Cover_Note_Sheet CNS
    INNER JOIN Cover_Note_Sheet_Status CNSS ON CNSS.cover_note_sheet_status_id = CNS.cover_note_sheet_status_id
    LEFT JOIN Insurance_File ifi ON ifi.insurance_file_cnt = CNS.insurance_file_cnt
    LEFT JOIN Party P ON ifi.lead_agent_cnt = p.party_cnt
    LEFT JOIN Source S ON S.Source_id = ifi.Source_id
    WHERE cns.cover_note_book_id = @cover_note_book_id
    AND cns.is_deleted = 0
    ORDER BY cns.cover_sheet_number
END

SET QUOTED_IDENTIFIER OFF    
Go
SET ANSI_NULLS OFF  
GO

