SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PaymentType_Exist'
GO

CREATE PROCEDURE spu_PaymentType_Exist  
    @Party_cnt  INT,      
    @Bank_Payment_Type_Id INT,  
    @Account_type VARCHAR(255)=NULL,  
    @IsEntryExits INT = 0 OUTPUT,
    @IsAccountTypeNullExists INT = 0 OUTPUT,
    @IsDuplicateAccountExists INT = 0 OUTPUT
 
AS  
IF @Account_type = '' 
    SET @Account_type = NULL 

SELECT  @IsEntryExits = COUNT(party_bank_id)  
   FROM party_bank PB  
   LEFT JOIN ACCOUNT  
   ON PB.account_id=ACCOUNT.account_id  
   WHERE  Bank_Payment_Type_Id = @Bank_Payment_Type_Id  
   AND  ACCOUNT.account_key=@Party_cnt  

SELECT  @IsAccountTypeNullExists = COUNT(party_bank_id)  
   FROM party_bank PB  
   LEFT JOIN ACCOUNT  
   ON PB.account_id=ACCOUNT.account_id  
   WHERE  Bank_Payment_Type_Id = @Bank_Payment_Type_Id  
   AND  ACCOUNT.account_key=@Party_cnt 
   AND ( Account_type IS NULL OR LTRIM(RTRIM(Account_type)) = '')

SELECT  @IsDuplicateAccountExists = COUNT(party_bank_id)  
   FROM party_bank PB  
   LEFT JOIN ACCOUNT  
   ON PB.account_id=ACCOUNT.account_id  
   WHERE  Bank_Payment_Type_Id = @Bank_Payment_Type_Id  
   AND  ACCOUNT.account_key=@Party_cnt 
   AND LTRIM(RTRIM(Account_type)) = @Account_Type

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO