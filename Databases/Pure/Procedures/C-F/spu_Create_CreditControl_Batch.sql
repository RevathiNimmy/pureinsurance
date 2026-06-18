SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Create_CreditControl_Batch'
GO
CREATE PROCEDURE spu_Create_CreditControl_Batch
    @batch_id INT OUTPUT
AS
BEGIN
    DECLARE @batch_type_id INT
    DECLARE @batchstatus_id INT
    DECLARE @total_amount MONEY
    DECLARE @total_transactions INT
	DECLARE	@Description VARCHAR(255) = 'Credit Control Processing'

    SELECT  @batch_type_id = batch_type_id
    FROM    batch_type
    WHERE   code = 'CREC'

    SELECT  @batchstatus_id = batchstatus_id
    FROM    batchstatus
    WHERE   code = 'BI'

    INSERT  Batch (
            batchstatus_id,
            batch_ref,
            created_date,
            batch_type_id,
            interface_code,
			Description)
    VALUES (@batchstatus_id,
            'CREC',
            GetDate(),
            @batch_type_id,
            'Credit Control Process - Credit Control',
			@Description)

    SELECT @batch_id = @@IDENTITY

    UPDATE batch
    SET batch_ref = 'CREC' + CONVERT(VARCHAR(10), @batch_id)
    WHERE batch_id = @batch_id

	END
	GO