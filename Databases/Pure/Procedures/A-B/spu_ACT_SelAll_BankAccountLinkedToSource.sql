--**********************************************************************************************  
-- Author : Pankaj Kaushik
--   
-- History: 26/05/2008    
--
-- Task : Account Function and CCY Cash Allocation
--**********************************************************************************************  

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_ACT_SelAll_BankAccountLinkedToSource'
GO

CREATE PROCEDURE spu_ACT_SelAll_BankAccountLinkedToSource(
    @Source_Id INT)
    
AS  
  
/*select the bank accounts*/  
SELECT bankaccount_id,  
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
	BA.is_cash_receive_in_this_currency_only,
    start_cheque_number,  
    financial_institution_code,  
    direct_debit_supplier_name,  
    direct_debit_supplier_id,  
    remitter,  
    processing_days,
	business_identifier_code,
	international_bank_account_number    

FROM BankAccount BA  
WHERE  

BA.bankaccount_id IN (select bankaccount_id from bankaccount_source where source_id = @Source_Id )

AND is_deleted <> 1 AND bank_id is not NULL  




