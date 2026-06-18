GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_UnAuthorised_Manual_Journals'
GO
CREATE PROCEDURE spu_Get_UnAuthorised_Manual_Journals      
@accountCode varchar(200)=NULL,      
@documentTypeId INT=NULL,      
@dateFrom DATE=NULL,      
@dateTo DATE =NULL      
AS      
BEGIN      
IF @accountCode  = ''      
SELECT @accountCode = NULL      
      
IF @documentTypeId  = 0      
SELECT @documentTypeId = NULL;      
      
WITH CTE AS (      
   SELECT manualjournaldetail_id,      
   ROW_NUMBER() OVER (PARTITION BY mjd.manualjournal_id ORDER BY manualjournaldetail_id ASC) as RowNumber      
   FROM ManualJournalDetail mjd      
   INNER JOIN ManualJournal mj ON mj.ManualJournal_id=mjd.ManualJournal_id      
   WHERE ( @documentTypeId IS NULL OR mj.DocumentType_id=@documentTypeId )      
   AND (@dateFrom IS NULL OR CreatedDate>=@dateFrom )      
   AND (@dateTo IS NULL OR CreatedDate <=@dateTo )      
   AND mj.is_reffered=1      
      )      
   SELECT mj.ManualJournal_Id,      
     act.short_code as AccountCode,      
     Amount,      
     mjd.Currency_Id,      
     Currency_Rate,      
     Base_Amount,      
     Alternate_Ref,      
     mjd.Comment,      
     uwy.description AS UnderwritingYear_Id,      
     ccentre.description AS CostCenterId,      
     Insurance_Ref,      
     Purchase_Order_No,      
     Purchase_Invoice_No,      
     CU.code CurrencyCode,
     mj.CreatedDate CreatedDate,      
     CASE ISNULL(No_of_steps,0) WHEN 0 THEN 'Pending Approval' ELSE 'Approved at step ' + CAST(No_of_steps AS CHAR)  END  As Status,    
     username AS CreatedBy      
   FROM CTE      
   INNER JOIN ManualJournalDetail mjd ON CTE.ManualJournalDetail_id=mjd.ManualJournalDetail_id      
   INNER JOIN ManualJournal mj ON mj.ManualJournal_id=mjd.ManualJournal_id      
   INNER JOIN Account act ON act.account_id=mjd.account_id      
   INNER JOIN PMUser pmuser ON pmuser.user_id = mj.PMuser_id    
   LEFT JOIN    
 (SELECT manualjournal_id,COUNT(manualjournal_id) No_of_steps,    
 MAX(approval_cnt) approval_cnt FROM ManualJournalApproval GROUP BY manualjournal_id) MA ON ma.manualjournal_id=mj.manualjournal_id    
   LEFT JOIN Underwriting_Year uwy ON uwy.underwriting_year_id = mjd.underwritingyear_id      
   LEFT JOIN costcentre ccentre ON cceNtre.costcentre_id = mjd.costcenterid      
   LEFT JOIN Currency CU ON CU.Currency_Id = mjd.currency_id      
   WHERE      
   (act.short_code LIKE @accountCode OR @accountCode IS NULL)      
         AND CTE.RowNumber =1  ORDER BY mj.ManualJournal_Id  desc
END 

GO