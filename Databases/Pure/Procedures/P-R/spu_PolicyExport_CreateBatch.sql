SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PolicyExport_CreateBatch'
GO

CREATE PROCEDURE spu_PolicyExport_CreateBatch
    @batch_id INT OUTPUT
AS
BEGIN
        DECLARE @batch_type_id INT
        DECLARE @batchstatus_id INT
        DECLARE @total_amount MONEY
        DECLARE @total_transactions INT
		DECLARE	@Description VARCHAR(255) = 'Policies'
    -- Get batch type id
    SELECT  @batch_type_id = batch_type_id
    FROM    batch_type
    WHERE   code = 'POLX' -- document_export

    SELECT  @batchstatus_id = batchstatus_id
    FROM    batchstatus
    WHERE   code = 'BI' -- batch in progress

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
            'POLX',
            GetDate(),
            @batch_type_id,
            'Policies Export',
	        0,
		    @Description)

    SELECT  @batch_id = @@IDENTITY

    -- Update batch_ref for inserted row
    UPDATE batch
    SET batch_ref = 'POLX' + CONVERT(VARCHAR(10), @batch_id)
    WHERE batch_id = @batch_id
END
GO