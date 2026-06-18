SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_PaymentExport_CreateBatch'
GO

CREATE PROCEDURE spu_ACT_PaymentExport_CreateBatch
    @batch_id int output,
	@MediaTypeCode NVARCHAR(10),
	@bank_account_name NVARCHAR(60)
AS

    Declare
        @batch_type_id int,
        @batchstatus_id int,
		@MediaType NVARCHAR(255),
		@Description NVARCHAR(255) = 'Payments'

    -- Get batch type id the transactions of Payment Export Type
    Select  @batch_type_id = batch_type_id
    From    batch_type
    Where   code = 'SPYX' -- Payments_export

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
            'SPYX',
            GetDate(),
            @batch_type_id,
            'Payment Export',
			0,
			@Description)

    SELECT @batch_id = @@IDENTITY
		
    -- Update batch_ref for inserted row
    UPDATE batch
    SET batch_ref = 'SPYX' + convert(varchar(10), @batch_id)
    WHERE batch_id = @batch_id
GO
