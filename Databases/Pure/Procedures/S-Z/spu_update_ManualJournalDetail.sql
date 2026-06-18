SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_update_ManualJournalDetail'
GO

CREATE PROCEDURE  spu_update_ManualJournalDetail  
 @TransDetail_id INT,  
 @ManualJournalDetail_Id INT  
AS  
  
UPDATE ManualJournalDetail  
SET Transdetail_id=@TransDetail_id  
WHERE ManualJournalDetail_Id = @ManualJournalDetail_Id  