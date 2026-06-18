SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Transactions_from_TransMatch' 
GO
CREATE PROCEDURE spu_Get_Transactions_from_TransMatch
@cashlistitem_id int,  
@account_id int  
AS  
BEGIN  
SELECT td.transdetail_id,ccl.base_match_amount as amount  
FROM  
TransMatch ccl JOIN TransDetail td ON ccl.transdetail_id=td.transdetail_id  
WHERE  
ccl.cashlistitem_id = @cashlistitem_id  
AND account_id=@account_id  
END
Go