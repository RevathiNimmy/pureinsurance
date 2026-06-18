SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_ReceiptExport_CreateBatch'
GO

create  PROCEDURE spu_ACT_ReceiptExport_CreateBatch
    @batch_id int output,
	@MediaTypeCode nvarchar(10),
	@bank_account_name varchar(60)
AS

    Declare
        @batch_type_id int,
        @batchstatus_id int,
		@Description VARCHAR(255) = 'Receipt Export',
		@MediaType NVARCHAR(255)

    -- Get batch type id the transactions of Payment Export Type
    Select  @batch_type_id = batch_type_id
    From    batch_type
    Where   code = 'RCPTX' -- Payments_export

    Select  @batchstatus_id = batchstatus_id
    From    batchstatus
    Where   code = 'BI' -- batch in progress

	SELECT @MediaType = description
	FROM MediaType 
	WHERE code = @MediaTypeCode

	IF LEN(@MediaType) > 0 
	SET @Description += '_'+@MediaType

	IF LEN(@bank_account_name) > 0 
	SET @Description += '_'+@bank_account_name

    -- Insert new batch
    INSERT  Batch (
            batchstatus_id,
            batch_ref,
            created_date,
            batch_type_id,
            interface_code,
	        auto_close,
		    Description)
    VALUES (
            @batchstatus_id,
            'RCPTX',
            GetDate(),
            @batch_type_id,
            'Receipt Export',
			0,
			@Description)

    SELECT @batch_id = @@IDENTITY

    -- Update batch_ref for inserted row
    UPDATE batch
    SET batch_ref = 'RCPTX' + convert(varchar(10), @batch_id)
    WHERE batch_id = @batch_id

GO