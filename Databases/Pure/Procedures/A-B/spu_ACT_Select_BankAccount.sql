EXECUTE DDLDropProcedure 'spu_ACT_Select_BankAccount'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_ACT_Select_BankAccount
    @bankaccount_id int  
AS  
SELECT  
 bankaccount_id,  
 currency_id,  
 company_id,  
 account_id,  
 bank_id,  
 code,  
 bank_account_no,  
 bank_account_name,  
 description,  
 next_cheque_number,  
 reconciled_date,  
 default_bank_account_id,  
 (SELECT bank_account_type_id FROM bank B WHERE B.bank_id = BA.bank_id) 'bank_account_type_id',  
 is_cash_receive_in_this_currency_only,  
 start_cheque_number,
 financial_institution_code,
 direct_debit_supplier_name,
 direct_debit_supplier_id,
 remitter,
 processing_days,
 business_identifier_code,
 international_bank_account_number 
 FROM BankAccount BA  
WHERE bankaccount_id = @bankaccount_id  

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO