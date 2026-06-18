GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_UnAuthorised_Manual_Journals_Details'  
GO  
  
  
CREATE PROCEDURE spu_Get_UnAuthorised_Manual_Journals_Details 
@manualJournalId INT  
AS  
BEGIN  

		SELECT      
			act.short_code AS AccountCode,  
			ManualJournalDetail_id AS ManualJournalDetailId,    
			Amount, 
			CU.code CurrencyCode, 
			CU.description CurrencyTypeDescription, 
			Currency_Rate AS CurrencyRate,  
			Base_Amount AS BaseAmount,  
			Alternate_Ref AS AlternateRef,  
			mjd.Comment,  
			uwy.description AS UnderwritingYearDescription,  
			ccentre.description AS CostCentreDescription,   
			Insurance_ref AS InsuranceRef,  
			Purchase_Order_No AS PurchaseOrderNumber,  
			Purchase_Invoice_No AS PurchaseInvoiceNumber , 
			Transdetail_id AS TransDetailId
		FROM ManualJournalDetail mjd  
		INNER JOIN ManualJournal mj ON mj.ManualJournal_id=mjd.ManualJournal_id  
		INNER JOIN Account act ON act.account_id=mjd.account_id  
		LEFT JOIN Underwriting_Year uwy ON uwy.underwriting_year_id = mjd.underwritingyear_id  
		LEFT JOIN costcentre ccentre ON cceNtre.costcentre_id = mjd.costcenterid  
		LEFT JOIN Currency CU ON CU.Currency_Id = mjd.currency_id  
		WHERE  
		mj.manualjournal_id=@manualJournalId  ORDER BY mj.ManualJournal_Id  desc
  
END  
  
  GO