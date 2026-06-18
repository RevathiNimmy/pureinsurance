SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_ACT_MarkCommissionPaymentsInBatch'
GO

CREATE PROCEDURE spu_ACT_MarkCommissionPaymentsInBatch				 
(
@account_Id INT,
@session_guid VARCHAR(40),
@batch_id INT,	
@bUpdFlag TINYINT
)  
AS  
	
If @bUpdFlag = 1
BEGIN
 UPDATE TransDetail
 SET commission_payment_batch_id = NULL 
 WHERE commission_payment_batch_id = @batch_id
END

UPDATE T
SET commission_payment_batch_id = @batch_id
FROM TransDetail T
INNER JOIN TransDetail_Selection TS 
ON 
TS.TransDetail_id = T.TransDetail_id
WHERE	TS.session_guid = @session_guid 
AND 
T.account_id = @account_Id

GO
