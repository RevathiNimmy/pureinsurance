SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_BankAccount'
GO


CREATE PROCEDURE spu_ACT_SelAll_BankAccount
    @company_id INT,  
    @sub_branch_id INT  
AS  
  
DECLARE @Value VARCHAR(20)  
  
/* Get the Product Option for multi-tree accounting */  
SELECT  
    @Value=Value  
FROM  
    Hidden_options  
WHERE  
    option_number=16  
  
/*If Null/0 then there is only one tree.*/  
IF @Value IS NULL OR @Value=0  
BEGIN  
    SELECT @company_id = NULL  
END  
  
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
 is_cash_receive_in_this_currency_only,  
 start_cheque_number,
--start girija
 financial_institution_code,
 direct_debit_supplier_name,
 direct_debit_supplier_id,
 remitter,
 processing_days  
--end girija  
FROM BankAccount BA  
WHERE  
(  
 company_id = @company_id  
 OR  
 @company_id IS NULL  
)  
AND is_deleted <> 1 AND bank_id is not NULL  

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO