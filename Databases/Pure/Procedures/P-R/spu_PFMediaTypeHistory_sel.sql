SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PFMediaTypeHistory_sel'
GO

CREATE PROCEDURE spu_PFMediaTypeHistory_sel  
  
@pfprem_finance_cnt int,  
@pfprem_finance_version int  
  
AS  
  
SELECT  
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
pfmediatypehistory.user_id,  
date_modified,  
pmuser.username, 
paperdd,
PaymentType,
AccountType,
business_identifier_code,
international_bank_account_number,
is_cardholder  
  
FROM pfmediatypehistory  
  
 LEFT JOIN pmuser on  
  pfmediatypehistory.user_id = pmuser.user_id  
  
WHERE pfprem_finance_cnt = @pfprem_finance_cnt  
AND pfprem_finance_version = @pfprem_finance_version  
  
ORDER BY date_modified ASC  



GO
