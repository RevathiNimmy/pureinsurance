SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_ACT_RemoveCommissionPaymentsBatch'
GO

CREATE PROCEDURE spu_ACT_RemoveCommissionPaymentsBatch
@batch_id int
AS

UPDATE TRANSDETAIL
SET	commission_payment_batch_id = NULL
WHERE commission_payment_batch_id = @batch_id

DELETE	Batch
WHERE	batch_id = @batch_id

GO

