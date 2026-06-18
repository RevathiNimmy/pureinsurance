SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_SAM_UpdateTransactionDueDate'
GO



CREATE Procedure spu_SAM_UpdateTransactionDueDate  
 @Insurance_File_cnt Int,  
 @Due_Date Date  
AS  
  
DECLARE @Document_ID Int   
  
Select @Document_ID = document_id   
From Document where insurance_file_cnt = @Insurance_File_cnt  
  
Update Transdetail   
set due_date = @Due_Date  
Where document_id = @Document_ID  
AND due_date IS NOT NULL 