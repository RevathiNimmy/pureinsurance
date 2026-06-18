SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Claim_Payment_Transactions_from_CashListItem' 
GO

CREATE PROCEDURE spu_Get_Claim_Payment_Transactions_from_CashListItem  
@cashlistitem_id int,  
@account_id int  
AS
BEGIN
SELECT td.transdetail_id,td.amount  
FROM    
CashListItem_Claim_Link ccl JOIN Claim_Payment cp  
ON ccl.claim_payment_id=cp.claim_payment_id  
JOIN TransDetail td ON cp.document_id=td.document_id  
WHERE    
ccl.cashlistitem_id = @cashlistitem_id  
AND account_id=@account_id  
END
GO
