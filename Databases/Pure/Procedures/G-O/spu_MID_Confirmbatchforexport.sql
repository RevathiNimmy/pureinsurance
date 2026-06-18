
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

EXEC DDLDropProcedure 'spu_MID_Confirmbatchforexport'
GO

CREATE PROCEDURE spu_MID_ConfirmBatchForExport
	@batch_id as int,
	@mid_type as varchar(4)
AS

BEGIN

	DECLARE @batch_type_id INT
	SELECT @batch_type_id = batch_type_id
	FROM Batch_Type 
	WHERE code = @mid_type

	SELECT	batch_ref, company_id
	FROM Batch
	WHERE Batch_id = @batch_id
		AND batch_type_id = @batch_type_id

END
GO