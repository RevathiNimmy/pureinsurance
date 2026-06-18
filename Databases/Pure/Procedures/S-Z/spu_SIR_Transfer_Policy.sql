
--Start -Tech Spec QBENZ CR005 Client Portfolio Transfer(6.1.1)   
  
SET QUOTED_IDENTIFIER ON 
GO 
SET ANSI_NULLS ON 
GO 
EXECUTE DDLDropProcedure 'spu_SIR_Transfer_Policy'   
GO 

CREATE PROCEDURE spu_SIR_Transfer_Policy   
 @from_Party_cnt int,  
 @to_Party_cnt int,  
 @insurance_file_cnt int  
AS  
  
DECLARE @from_account_id int,  
 @to_account_id int,  
 @to_shortname varchar(20),  
 @to_name varchar(255),
 @to_resolvedname varchar(255),  
 @to_address_id int,  
 @insurance_folder_cnt int,
--Changes done by Krishna Nand
--Dated: 10-02-2010
--PN:62386
--Purpose: Convert the amount in transferred client currency
 @from_currencyid numeric,
 @to_Currencyid int,
 @to_CurrencyRate numeric(12, 8),
 @to_Company_id int,
 @effective_date datetime,
--End of Changes done by Krishna Nand on 10-02-2010 for PN: 62386

 @ClientName varchar(255),
 @ClientAddr1 varchar(60),
 @ClientAddr2 varchar(60),
 @ClientAddr3 varchar(60),
 @ClientAddr4 varchar(60), 
 @ClientPCode varchar(20),

 @BankName varchar(60),
 @BankSortCode varchar(60),
 @BankAccountNo varchar(60),
 @BankBranch varchar(60),
 @BankAddr1 varchar(60),
 @BankAddr2 varchar(60),
 @BankAddr3 varchar(60),
 @BankPCode varchar(60),
 @BankPhoneNo varchar(15),
 @BankAccountName varchar(60)
  
--Initialise Variables  
SELECT @from_account_id = account_id FROM Account WHERE account_key = @from_Party_cnt  
SELECT @to_account_id = account_id FROM Account WHERE account_key = @to_Party_cnt  
SELECT @to_shortname = shortname, @to_name = name,@to_resolvedname=resolved_name FROM Party where party_cnt = @to_Party_cnt  
SELECT TOP 1 @to_address_id = address_cnt FROM Party_Address_Usage WHERE party_cnt = @to_Party_cnt  
SELECT @insurance_folder_cnt = insurance_folder_cnt FROM Insurance_File WHERE insurance_file_cnt = @insurance_file_cnt  
  
--Get all policy versions  
CREATE TABLE #Matches_Found  
 (insurance_file_cnt     INT)  
  
INSERT INTO  
 #Matches_Found  
 (insurance_file_cnt)  
SELECT  
 insurance_file_cnt  
FROM  
 Insurance_File  
WHERE  
 insurance_folder_cnt = @insurance_folder_cnt  
  
--Basics  
UPDATE  
 Insurance_File  
SET  
 insured_cnt = @to_Party_cnt,
 insured_name=@to_resolvedname  
WHERE  
 insurance_file_cnt IN (SELECT insurance_file_cnt FROM #Matches_Found)  
AND  
 insured_cnt = @from_Party_cnt 

SELECT 
 @ClientName=p.resolved_name, 
 @ClientAddr1=a.address1,
 @ClientAddr2=a.address2,
 @ClientAddr3=a.address3,
 @ClientAddr4=a.address4,
 @ClientPCode=a.postal_code
FROM Party p 
	INNER JOIN 
		party_address_usage pau 
			ON p.party_cnt=pau.party_cnt
	INNER JOIN 
		address a ON pau.address_cnt=a.address_cnt
WHERE p.party_cnt=@to_Party_cnt

 SELECT 
     @BankName=b.bank_name,
	 @BankSortCode=b.code, 
	 @BankAccountNo=ba.bank_account_no,
	 @BankBranch=b.branch_code,
	 @BankAddr1=b.bank_address1,
	 @BankAddr2=b.bank_address2,
	 @BankAddr3=b.bank_address3,
	 @BankPCode=b.bank_postal_code,	 
	 @BankPhoneNo=b.bank_phone_number,	 
	 @BankAccountName=ba.bank_account_name
FROM bank b
	JOIN bankaccount ba ON b.bank_id=ba.bank_id
WHERE ba.account_id=@to_account_id
 
UPDATE
 pfpremiumfinance
SET
 clientid=@to_Party_cnt,
 ClientName=@ClientName,
 ClientAddr1=@ClientAddr1,
 ClientAddr2=@ClientAddr2,
 ClientAddr3=@ClientAddr3,
 ClientPCode=@ClientPCode,
 ClientAddr4=@ClientAddr4,
 BankName=@BankName,
 BankSortCode=@BankSortCode, 
 BankAccountNo=@BankAccountNo,
 BankBranch=@BankBranch,
 BankAddr1=@BankAddr1,
 BankAddr2=@BankAddr2,
 BankAddr3=@BankAddr3,
 BankPCode=@BankPCode,
 BankPhoneNo=@BankPhoneNo,	 
 BankAccountName=@BankAccountName 
FROM pfpremiumfinance pfp
 JOIN Insurance_file inf ON pfp.insurance_file_cnt=inf.insurance_file_cnt
WHERE inf.insurance_file_cnt=@insurance_file_cnt
  
UPDATE  
 Insurance_folder  
SET  
 insurance_holder_cnt = @to_Party_cnt  
WHERE  
 insurance_folder_cnt = @insurance_folder_cnt  
AND  
 insurance_holder_cnt = @from_Party_cnt  
  
--Claims  
UPDATE  
 Claim  
SET  
 Client_id = @to_Party_cnt,  
 client_short_name = @to_shortname,  
 client_name = @to_name,  
 client_address = @to_address_id  
WHERE  
 Policy_id IN (SELECT insurance_file_cnt FROM #Matches_Found)  
  
--Accounts  
UPDATE  
 a  
SET  
 a.insurance_holder_cnt = @to_Party_cnt,  
 a.insurance_holder_shortname = @to_shortname,  
 a.insurance_holder_name = @to_name  
FROM  
 Stats_Folder a  
INNER JOIN  
 document b  
ON  
 a.document_ref = b.document_ref  
INNER JOIN  
 TransDetail c  
ON  
 c.document_id = b.document_id  
INNER JOIN  
 Stats_Detail d  
ON  
 a.stats_folder_cnt = d.stats_folder_cnt  
INNER JOIN  
 #Matches_Found e  
ON  
 e.insurance_file_cnt = b.insurance_file_cnt  
WHERE  
 c.account_id = @from_account_id  
AND  
 c.document_id NOT IN (SELECT DISTINCT document_id FROM TransDetail 
						WHERE amount <> outstanding_amount AND account_id = @from_account_id)
  
UPDATE  
 a  
SET  
 a.account_id = @to_account_id  
FROM  
 TransDetail a  
INNER JOIN  
 Document b  
ON  
 a.document_id = b.document_id  
left JOIN  
 Stats_Folder c  
ON  
 b.document_ref = c.document_ref  
left JOIN  
 Stats_Detail d  
ON  
 c.stats_folder_cnt = d.stats_folder_cnt  
INNER JOIN  
 #Matches_Found e  
ON  
 e.insurance_file_cnt = b.insurance_file_cnt  
WHERE  
 a.account_id = @from_account_id  
AND  
 a.document_id NOT IN (SELECT DISTINCT document_id FROM TransDetail 
							WHERE amount <> outstanding_amount AND account_id = @from_account_id)
 
--Start for Insert address line from Address Table 

Declare @to_address_id_Chk int
If not exists (Select ca.address_cnt  from Claim_Address ca where ca.address_cnt = @to_address_id)
Begin
SET IDENTITY_INSERT Claim_Address ON
Insert Claim_Address (address_cnt,source_id,address_id,address1,address2,address3,address4,postal_code,country_id,created_by_id,
date_created,modified_by_id,last_modified)
Select address_cnt,source_id,address_id,address1,address2,address3,address4,postal_code,country_id,created_by_id,
date_created,modified_by_id,last_modified from Address A Where A.address_cnt = @to_address_id
SET IDENTITY_INSERT Claim_Address OFF
End

--End for Insert address line from Address Table 
  
--End - Tech Spec QBENZ CR005 Client Portfolio Transfer(6.1.1) 
 
UPDATE
 Credit_Control_Item
SET account_id = @to_account_id 
WHERE insurance_file_cnt IN (SELECT insurance_file_cnt FROM #Matches_Found)
AND account_id = @from_account_id
AND is_deleted <>1
