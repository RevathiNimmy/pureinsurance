
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_UnAuthorised_Manual_Journals_Master'
GO

CREATE PROCEDURE spu_Get_UnAuthorised_Manual_Journals_Master 
@manualJournalId INT  
AS  
BEGIN  
  
SELECT CreatedDate,  
		 dType.code AS DocumentType,  
		 source.Code AS Branch,  
		 is_reffered AS IsReferred,  
		 Comment AS Comment,  
		 Reverses_on as ReverseDate,  
		 Recurring_Occurs AS RecurringOccurs,  
		 PerPeriodOnDay,  
		 PerMonthOnDay,  
		 PerQuarterOnDay,  
		 Authorisation_comment AS AuthorisationComment   
		FROM
		ManualJournal mj  
		JOIN DocumentType dType ON dType.documenttype_id = mj.documentType_id  
		LEFT JOIN Source source ON source.source_id = mj.source_id  
		WHERE ManualJournal_id = @manualJournalId   AND is_reffered=1
  
END  


GO
----------------------------------------------------  
  