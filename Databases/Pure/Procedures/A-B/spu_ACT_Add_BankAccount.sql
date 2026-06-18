
EXECUTE DDLDropProcedure 'spu_ACT_Add_BankAccount'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_ACT_Add_BankAccount
 @bankaccount_id int OUTPUT,  
 @currency_id smallint,  
 @account_id int,  
 @bank_id smallint,  
 @code char(10),  
 @bank_account_no varchar(30),  
 @bank_account_name varchar(60),  
 @description varchar(255),  
 @next_cheque_number int,  
 @reconciled_date datetime,  
 @default_bank_account_id INT = NULL,  
 @is_cash_receive_in_this_currency_only TINYINT,  
 @start_cheque_number varchar(10),
 @financial_institution_code varchar(50),
 @direct_debit_supplier_name varchar(50),
 @direct_debit_supplier_id INT,
 @remitter varchar(50),
 @processing_days smallint,
 @sBusinessIdentifierCode VARCHAR(50)=NULL,
 @sInternationalBankAccountNumber VARCHAR(50)=NULL,
 @user_id int = NULL,
 @unique_id varchar(50) = NULL,
 @screen_hierarchy varchar(500) = NULL 
AS  
  
DECLARE  
    @caption_id int,  
    @company_id int,  
    @sub_branch_id int  
  
IF (@description='')  
    SELECT @description=@bank_account_name  
  
IF (@start_cheque_number='')  
 SELECT @start_cheque_number=NULL  
  
EXEC spu_pm_caption_id_return  
    1,  
    @description,  
    @caption_id OUTPUT  
  
SELECT @company_id = company_id  
FROM   account  
WHERE  account_id = @account_id  
  
SELECT @sub_branch_id = sub_branch_id  
FROM   account  
WHERE  account_id = @account_id  
  
INSERT INTO BankAccount (  
 currency_id ,  
 company_id ,  
 sub_branch_id ,  
 account_id ,  
 bank_id ,  
 code ,  
 bank_account_no ,  
 bank_account_name ,  
 description ,  
 caption_id ,  
 effective_date ,  
 is_deleted,  
 next_cheque_number,  
 reconciled_date,  
 default_bank_account_id,  
 is_cash_receive_in_this_currency_only,  
 start_cheque_number,
 financial_institution_code,
 direct_debit_supplier_name,
 direct_debit_supplier_id,
 remitter,
 processing_days,
 business_identifier_code,
 international_bank_account_number,
 UserId,
 UniqueId,
 ScreenHierarchy)
VALUES (  
 @currency_id,  
 @company_id,  
 @sub_branch_id,  
 @account_id,  
 @bank_id,  
 @code,  
 @bank_account_no,  
 @bank_account_name,  
 @description,  
 @caption_id,  
 '2000/1/1',  
 0,  
 @next_cheque_number,  
 @reconciled_date,  
 @default_bank_account_id,  
 @is_cash_receive_in_this_currency_only,  
 CAST(@start_cheque_number AS bigint),
 @financial_institution_code ,
 @direct_debit_supplier_name ,
 @direct_debit_supplier_id ,
 @remitter ,
 @processing_days,
 @sBusinessIdentifierCode,
 @sInternationalBankAccountNumber,
 @user_id,
 @unique_id,
 @screen_hierarchy)

  
SELECT @bankaccount_id = SCOPE_IDENTITY()  
  
/* Ensure linked Account is in sync */  
IF (ISNULL(@account_id,0)<>0)  
 UPDATE Account  
 SET    currency_id=@currency_id  
 WHERE  account_id=@account_id  
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


