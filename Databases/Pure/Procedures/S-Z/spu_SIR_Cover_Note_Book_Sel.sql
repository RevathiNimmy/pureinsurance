SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Cover_Note_Book_Sel'
GO

CREATE PROCEDURE spu_SIR_Cover_Note_Book_Sel    
 @cover_note_book_id int      
AS      
BEGIN      
    SELECT TOP 1      
 cnb.book_number,      
 cnb.start_number,      
 cnb.end_number,      
 cnb.effective_date,      
 cnb.agent_cnt,         
 p.name,      
 cnb.source_id,     
 cnb.cover_note_book_status_id,     
 cnb.user_id,      
 cnb.created_date,      
 cnb.last_updated, 
--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)      
S.Code CoverNoteBranchCode,   
CNBS.Code as CoverNoteBookStatusCode    
--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)     
    FROM Cover_Note_Book CNB      
    Left JOIN Party P ON P.Party_Cnt = CNB.Agent_Cnt    
--Start (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)         
Left JOIN Source S ON S.source_id = CNB.source_id     
Left JOIN Cover_note_book_status CNBS ON CNBS.cover_note_book_status_id = CNB.cover_note_book_status_id     
--End (Vijayakumar Ramasamy)-(Tech Spec - UIIC WR53 - Cover Note Maintenance - Get Cover Note Book Details.doc)-(6.2)        
    WHERE cover_note_book_id = @cover_note_book_id      
END      
      
SET QUOTED_IDENTIFIER OFF      
    
Go
SET ANSI_NULLS OFF  
GO
