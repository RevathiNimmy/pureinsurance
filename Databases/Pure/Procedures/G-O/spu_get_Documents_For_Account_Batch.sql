SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_Documents_For_Account_Batch'
GO

CREATE PROCEDURE spu_get_Documents_For_Account_Batch
(
 @account_id INT,
 @batch_id INT
)  
AS
SELECT	
Distinct TD.document_id
FROM 
TransDetail TD
INNER JOIN Account A
ON TD.account_id= A.account_id
WHERE	TD.account_id = @account_id 
AND TD.commission_payment_batch_id = @batch_id

GO
