SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'Spu_pfPremiumFinance_bankDet_upd'
GO

CREATE PROCEDURE Spu_pfPremiumFinance_bankDet_upd   
	@party_bank_id INT  ,
	@sAccount_type   VARCHAR(255)=NULL,
	@nBank_payment_type_id   INT=0
  
AS  
  
UPDATE PFPremiumFinance SET 
	 BankAccountNo= PB.account_number,     
	 BankAccountName = PB.Account_Holder_Name,  
     BankName = (SELECT DESCRIPTION  FROM CashListItem_Bank
				 WHERE CashListItem_Bank_id = PB.bank_name_id),  
     BankBranch = PB.bank_branch,  
     BankSortCode = PB.bank_branch_code,  
     BankAddr1 = PB.bank_add1,  
     BankAddr2 = PB.bank_add2,  
     BankAddr3 = PB.bank_add3,  
     BankTown = PB.bank_town,  
     BankPCode = PB.bank_pcode,  
     BankRegion = PB.bank_region ,  
     BankCountry = PB.bank_country,  
     cc_number = PB.CC_Num,  
     cc_start_date = PB.cc_start_date,  
     cc_expiry_date = PB.cc_expiry_date,  
     cc_issue = PB.cc_issue_num,  
     cc_pin = PB.cc_num,  
     cardholder_address1 = PB.cc_add1,  
     cardholder_address2 = PB.cc_add2,  
     cardholder_address3 = PB.cc_add3,  
     cardholder_address4 = PB.cc_town,  
     cardholder_postcode = PB.cc_pcode,
	 business_identifier_code = PB.business_identifier_code,
	 international_bank_account_number = PB.international_bank_account_number, 
     date_modified=getdate()
  
	FROM Party_Bank PB  
	INNER JOIN PFPremiumFinance  
		ON PFPremiumFinance.Party_Bank_id = PB.Party_bank_ID   
	WHERE PFPremiumFinance.party_bank_id = @party_bank_id  
		AND  PFPremiumFinance.StatusInd IN ('040','140')  

INSERT INTO pfMediaTypeHistory (  
     pfprem_finance_cnt,  
     pfprem_finance_version,  
     mediatype_validation_code,  
     action_code,  
     BankAccountName,  
     BankSortCode,  
     BankAccountNo,  
     BankName,  
     BankBranch,  
     BankAddr1,  
     BankAddr2,  
     BankAddr3,  
     BankTown,  
     BankPCode,  
     BankRegion,  
     BankCountry,  
     BankAreaCode,  
     BankPhoneNo,  
     BankExtension,  
     BankFaxAreaCode,  
     BankFaxNo,  
     cc_number,  
     cc_expiry_date,  
     cc_start_date,  
     cc_issue,  
     cc_pin,  
     cardholder_name,  
     cardholder_address1,  
     cardholder_address2,  
     cardholder_address3,  
     cardholder_address4,  
     cardholder_postcode,  
     user_id,  
     date_modified,  
     paperdd,
	 business_identifier_code,
	 international_bank_account_number,
	 accounttype,
	 PaymentType)  
  
SELECT      pfprem_finance_cnt,  
     pfprem_finance_version,  
     mediatype_validation.code,  
     'Amendment',  
     LTRIM(RTRIM(BankAccountName)),  
     LTRIM(RTRIM(BankSortCode)),  
     LTRIM(RTRIM(BankAccountNo)),  
     LTRIM(RTRIM(BankName)),  
     LTRIM(RTRIM(BankBranch)),  
     LTRIM(RTRIM(BankAddr1)),  
     LTRIM(RTRIM(BankAddr2)),  
     LTRIM(RTRIM(BankAddr3)),  
     LTRIM(RTRIM(BankTown)),  
     LTRIM(RTRIM(BankPCode)),  
     LTRIM(RTRIM(BankRegion)),  
     LTRIM(RTRIM(BankCountry)),  
     LTRIM(RTRIM(BankAreaCode)),  
     LTRIM(RTRIM(BankPhoneNo)),  
     LTRIM(RTRIM(BankExtension)),  
     LTRIM(RTRIM(BankFaxAreaCode)),  
     LTRIM(RTRIM(BankFaxNo)),  
     LTRIM(RTRIM(cc_number)),  
     LTRIM(RTRIM(cc_expiry_date)),  
     LTRIM(RTRIM(cc_start_date)),  
     LTRIM(RTRIM(cc_issue)),  
     LTRIM(RTRIM(cc_pin)),  
     LTRIM(RTRIM(cardholder_name)),  
     LTRIM(RTRIM(cardholder_address1)),  
     LTRIM(RTRIM(cardholder_address2)),  
     LTRIM(RTRIM(cardholder_address3)),  
     LTRIM(RTRIM(cardholder_address4)),  
     LTRIM(RTRIM(cardholder_postcode)),  
     user_id,  
     date_modified,  
     paperdd,
	 business_identifier_code,
	 international_bank_account_number,
	 @sAccount_type,
	 Bank_Payment_Type.description  
  
FROM  pfpremiumfinance  
  INNER JOIN pfscheme on  
    pfscheme.schemeno = pfpremiumfinance.schemeno  
   AND  pfscheme.schemeversion = pfpremiumfinance.schemeversion  
   AND  pfscheme.companyno = pfpremiumfinance.companyno  
  
   INNER JOIN mediatype ON  
     mediatype.mediatype_id = pfscheme.mediatype_id  
    INNER JOIN mediatype_validation ON  
      mediatype_validation.mediatype_validation_id = mediatype.mediatype_validation_id  
	  INNER JOIN Bank_Payment_Type ON
	  Bank_Payment_Type.bank_payment_type_id=@nBank_payment_type_id
 WHERE  pfpremiumfinance.party_bank_id = @party_bank_id   
    AND  PFPremiumFinance.StatusInd IN ('040','140')  

	
	DECLARE @pfMedia_History_Curr_Id INT, 
			@pfPrem_Finance_Cnt      INT,
			@pfPrem_Finance_Version  INT
	SELECT TOP 1 @pfMedia_History_Curr_Id = pfmediatypehistory_id,@pfPrem_Finance_Cnt= premFinance.pfprem_finance_cnt,@pfPrem_Finance_Version = premFinance.pfprem_finance_version  
	FROM pfmediatypehistory mediaHistory  
	JOIN PFPremiumFinance premFinance ON mediaHistory.pfprem_finance_cnt = premFinance.pfprem_finance_cnt
	AND mediaHistory.pfprem_finance_version = premFinance.pfprem_finance_version 
	WHERE premFinance.party_bank_id = @party_bank_id ORDER BY pfmediatypehistory_id DESC, 
  premFinance.pfprem_finance_cnt DESC,premFinance.pfprem_finance_version DESC

   UPDATE pfinstalments
	 SET pfmediatype_history_id = @pfMedia_History_Curr_Id
	 WHERE pfinstalments.status not in (3,4,6,10) AND pfprem_finance_cnt = @pfPrem_Finance_Cnt 
	 AND pfprem_finance_version = @pfPrem_Finance_Version
	 
	
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

