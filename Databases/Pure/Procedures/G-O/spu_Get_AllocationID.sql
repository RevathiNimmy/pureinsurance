SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_AllocationID'
Go 

CREATE PROCEDURE spu_Get_AllocationID
(  
 @reverse_transaction_log_id INT 
)  

AS  
   
	SELECT allocation_id, 
	account_id	 
	FROM void_reverse_transaction_log_detail
	WHERE reverse_transaction_log_id = @reverse_transaction_log_id  