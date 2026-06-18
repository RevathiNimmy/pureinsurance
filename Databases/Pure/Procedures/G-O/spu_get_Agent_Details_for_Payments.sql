SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_Agent_Details_for_Payments'
GO

CREATE PROCEDURE spu_get_Agent_Details_for_Payments
(
 @account_id INT,
 @source_id INT
)  
AS
DECLARE @Payment_method INT
DECLARE @bank_default_account_ID INT
DECLARE @bank_account_ID INT

SELECT	@Payment_method = payment_method_id
FROM	payment_method
WHERE	Code = 'CSH'

SELECT  @bank_default_account_ID = bankaccount_id
FROM BankAccount_Default  
WHERE source_id = @source_id  
AND cashlisttype_id = (Select cashlisttype_id From CashListType Where code ='R') 
AND is_deleted <> 1  

SELECT @bank_account_ID = bankaccount_id
FROM account A INNER JOIN BankAccount BA 
ON A.account_id = BA.account_id 
where short_code = 'BANKSUSPAC'

SELECT	
A.account_id AS	account_id,
(AD.address1 + ' ' + AD.address2 + ' ' + AD.address3 + ' ' + AD.address4 + ' ' + AD.postal_code) AS Address, 
ISNULL(PA.payment_method, @Payment_method)	AS Media_Type,
--3 AS bank_account_ID
ISNULL(PA.bankaccount_ID, ISNULL(@bank_default_account_ID, @bank_account_ID)) AS bank_account_ID
FROM 
Account A 
INNER JOIN party_agent PA 
ON A.account_key = PA.party_cnt 
LEFT JOIN 
Party_Address_Usage PAU
ON PA.party_cnt = PAU.party_cnt		
LEFT JOIN 
address AD
ON PAU.address_cnt = AD.address_cnt
WHERE	A.account_id = @account_id
GO

