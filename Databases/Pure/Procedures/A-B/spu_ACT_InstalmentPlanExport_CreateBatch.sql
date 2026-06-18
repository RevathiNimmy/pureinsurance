SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_InstalmentPlanExport_CreateBatch'
GO

CREATE PROCEDURE spu_ACT_InstalmentPlanExport_CreateBatch
    @batch_id int output,
	@pfscheme_type_code NVARCHAR(10)
AS

    Declare
        @batch_type_id int,
        @batchstatus_id int,
		@FsSchemeType NVARCHAR(255),
		@Description NVARCHAR(255) = 'Instalment Plans'

    -- Get batch type id the transactions of Payment Export Type
    Select  @batch_type_id = batch_type_id
    From    batch_type
    Where   code = 'IPX' -- Payments_export

    Select  @batchstatus_id = batchstatus_id
    From    batchstatus
    Where   code = 'BI' -- batch in progress

	SELECT @FsSchemeType = description
	FROM PFScheme_Type 
	WHERE code = @pfscheme_type_code 

	IF LEN(@FsSchemeType) > 0 
	SET @Description += '_'+@FsSchemeType

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
            'IPX',
            GetDate(),
            @batch_type_id,
            'Instalment Plan Export',
			0,
			@Description)

    SELECT @batch_id=@@IDENTITY
	--PN 57541
	UPDATE 	Batch 
	SET 	batch_ref = 'IPX'+ convert(varchar(10), IsNull(@batch_id,''))
	WHERE 	batch_id = @batch_id


GO
