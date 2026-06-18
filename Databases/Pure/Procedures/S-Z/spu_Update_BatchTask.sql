SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Update_BatchTask'
GO 


CREATE PROCEDURE spu_Update_BatchTask
	@Batch_Id INT,
	@BatchStatusCode Varchar(5),
	@FileName VARCHAR(100),
	@Total_Transactions INT = NULL,
	@Reject_Transactions INT = NULL
AS
BEGIN
	Declare @BatchStatusId INT
	IF @Total_Transactions = 0
	Set @Total_Transactions = null 
	IF @Reject_Transactions = 0
	Set @Reject_Transactions = null 

	SELECT @BatchStatusId = batchstatus_id FROM BatchStatus WHERE code = @BatchStatusCode	
		IF @BatchStatusId is not null
		BEGIN
		UPDATE Batch 
		SET 
		Completed_Date = GETDATE(),
		Import_File_Name = @Filename,
		batchstatus_id = @BatchStatusId,
		total_transactions = ISNULL(@Total_Transactions,total_transactions),
		reject_transactions = ISNULL(@reject_transactions,reject_transactions)
		WHERE
		Batch_Id = @Batch_Id
		END
END
GO