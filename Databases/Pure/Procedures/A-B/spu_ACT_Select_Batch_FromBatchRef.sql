SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Batch_FromBatchRef'
GO

CREATE PROCEDURE spu_ACT_Select_Batch_FromBatchRef
	@batch_ref VARCHAR(25),
	@batch_id INT OUTPUT
AS
	SELECT
		@batch_id=batch_id
	FROM
		Batch
	WHERE
		batch_ref=@batch_ref

GO