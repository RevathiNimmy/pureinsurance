SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_ACT_CreateCommissionPaymentsBatch'
GO

CREATE PROCEDURE spu_ACT_CreateCommissionPaymentsBatch
(
 @user_id INT,
 @batch_id INT OUTPUT
)  
AS  
DECLARE @l_batch_type_id INT
DECLARE @batchstatus_id INT  

SELECT @l_batch_type_id = batch_type_id 
FROM Batch_Type 
WHERE code = 'CMS'

Select @batchstatus_id = batchstatus_id  
From batchstatus  
where code='C'  

-- Create New Batch
INSERT INTO Batch
(
 batchstatus_id, 
 batch_ref,
 created_date, 		
 user_id, 
 batch_type_id
)
Values
(
 @batchstatus_id, 
 'CMS',
 GETDATE(),
 @user_id, 		
 @l_batch_type_id
)
SELECT @batch_id = @@IDENTITY

-- Update batch_ref for inserted row  
UPDATE batch  
SET batch_ref = 'CMS'+ Convert(varchar(10),@batch_id)  
WHERE batch_id = @batch_id  

GO
