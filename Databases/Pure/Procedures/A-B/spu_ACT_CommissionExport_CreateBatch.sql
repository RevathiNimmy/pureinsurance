SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_CommissionExport_CreateBatch'
GO

CREATE PROCEDURE spu_ACT_CommissionExport_CreateBatch
	@Batch_Id INT OUTPUT,
	@Agent_Type_Code varchar(255),
	@Currency_Id int
AS

	DECLARE @batch_type_id int,
			@batchstatus_id int,
			@MediaType NVARCHAR(255),
			@Description NVARCHAR(255) = 'Commission'

	SELECT  @batch_type_id = batch_type_id
	FROM    batch_type
	WHERE   code = 'COMMX'

	SELECT  @batchstatus_id = batchstatus_id
	FROM    batchstatus
	WHERE   code = 'BI' -- batch in progress

	SET @Description = @Description + '_' + CONVERT(VARCHAR(10), @Currency_ID) + '_' + @Agent_Type_Code
	SET @Description = LEFT(@Description, 255)

	-- Insert new batch
	INSERT INTO Batch (
		batchstatus_id,
		batch_ref,
		created_date,
		batch_type_id,
		interface_code,
		auto_close,
		Description)
	VALUES (
		@batchstatus_id,
		'COMMX',
		GetDate(),
		@batch_type_id,
		'Commission_Export',
		0,
		@Description)

	SELECT @batch_id = @@IDENTITY
		
	-- Update batch_ref for inserted row
	UPDATE batch 
		SET batch_ref = 'COMMX' + convert(varchar(10), @batch_id)
	WHERE batch_id = @batch_id

GO
