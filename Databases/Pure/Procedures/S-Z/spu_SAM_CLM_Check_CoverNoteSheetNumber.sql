SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_CLM_Check_CoverNoteSheetNumber'
GO


--Start (Arul Stephen)-(Tech Spec - UIIC WR24 - OpenClaim - FindInsuranceFile.doc)-(7.1.3.2.4)

Create  PROCEDURE spu_SAM_CLM_Check_CoverNoteSheetNumber    
    @Cover_Note_Sheet_Number int    
AS    
    
SELECT    
		Cover_Note_sheet_Id    
FROM	Cover_Note_Sheet    
WHERE	Cover_Sheet_Number=@Cover_Note_Sheet_Number    
    
--End (Arul Stephen)-(Tech Spec - UIIC WR24 - OpenClaim - FindInsuranceFile.doc)-(7.1.3.2.4)

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO
