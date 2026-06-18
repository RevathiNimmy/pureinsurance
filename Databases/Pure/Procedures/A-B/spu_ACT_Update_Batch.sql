SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_Update_Batch'
GO


CREATE PROCEDURE spu_ACT_Update_Batch
    @batch_id int,
    @company_id smallint,
    @batchstatus_code varchar(10),
    @user_id smallint,
    @batch_ref varchar(25),
    @created_date datetime,
    @authorised_date datetime,
    @accounting_date datetime,
    @comment varchar(60),
    @batch_type_id int,
    @batch_source_id int,
    @xml_object varchar(4000),
	@total_amount numeric = null,
	@total_transactions int = null,
	@imported_date datetime = null,
	@reject_amount numeric = null,
	@reject_transactions int = null,
	@import_file_name varchar(255) = null

AS

BEGIN
DECLARE @BatchStatusId INT
SELECT @BatchStatusId = batchStatus_id from batchstatus where code = @batchstatus_code
UPDATE Batch
    SET
    company_id=@company_id,
    batchstatus_id=@BatchStatusId,
    user_id=@user_id,
    batch_ref=@batch_ref,
    created_date=@created_date,
    authorised_date=@authorised_date,
    accounting_date=@accounting_date,
    comment=@comment,
    batch_source_id=@batch_source_id,
    xml_object=@xml_object,
    total_amount = @total_amount,
	total_transactions = @total_transactions,
	imported_date = @imported_date,
	reject_amount = @reject_amount,
	reject_transactions = @reject_transactions,
	import_file_name = @import_file_name,
	Completed_Date = GETDATE()
WHERE batch_id = @batch_id

IF ISNULL(@batch_type_id,'') <> ''
BEGIN
	UPDATE Batch
		SET 
		batch_type_id=@batch_type_id
	WHERE batch_id = @batch_id
END
END
GO

