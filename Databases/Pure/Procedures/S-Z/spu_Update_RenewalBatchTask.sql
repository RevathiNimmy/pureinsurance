SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Update_RenewalBatchTask'
GO

CREATE PROCEDURE spu_Update_RenewalBatchTask
	@Batch_Id INT,
	@BatchStatusCode VARCHAR(10),
	@FileName VARCHAR(100),
	@BatchDescription VARCHAR(255)
AS
BEGIN
	DECLARE @BatchStatusId INT
	SELECT @BatchStatusId = batchstatus_id FROM BatchStatus WHERE code = @BatchStatusCode	
		IF @BatchStatusId is not null
		BEGIN
		UPDATE Batch 
		SET 
		Completed_Date = GETDATE(),
		Import_File_Name = @Filename,
		batchstatus_id = @BatchStatusId,
		Description = @BatchDescription
		WHERE
		Batch_Id = @Batch_Id
		END
END
GO