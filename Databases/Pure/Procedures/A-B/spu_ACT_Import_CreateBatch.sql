SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_Import_CreateBatch'
GO

CREATE PROCEDURE spu_ACT_Import_CreateBatch
    @batch_code char(10),
    @batch_ref varchar(25) output,
    @interface_code varchar(50),
    @batch_id int output,
	@supplier_id int = 0,
	@site_number int = 0
AS

    Declare
        @batch_type_id int,
        @batchstatus_id int

    -- Check reference
    If IsNull(@batch_ref, '') = ''
        Select  @batch_ref = Null

    -- Get batch type id
    Select  @batch_type_id = batch_type_id
    From    batch_type
    Where   code = @batch_code

	    -- If this batch_ref already exists return with no new batch
    -- to indicate that this batch has already been imported.
    If (LEFT(@batch_code,3) <> 'MID' AND  Exists (Select * From batch Where batch_ref = @batch_ref And batch_type_id = @batch_type_id))
		RETURN
	ELSE
	BEGIN
		DECLARE @source_id int = NULL
		SET @source_id = (SELECT TOP 1 source_id FROM MID_Rule
							WHERE Site_Number = @site_number AND Supplier_id = @supplier_id
							AND Is_Deleted = 0 AND MID_Type =LTRIM(RTRIM(@batch_code)))
		IF Exists (Select 1 From batch Where batch_ref = @batch_ref And batch_type_id = @batch_type_id 
										AND interface_code = @interface_code AND company_id = @source_id)
        Return
	END

    -- Get batch status id
    Select  @batchstatus_id = batchstatus_id
    From    batchstatus
    Where   code = 'BI' -- batch in progress

    -- Insert new batch
    Insert  Batch (
            batchstatus_id,
            batch_ref,
            created_date,
            batch_type_id,
            interface_code,
			Description,
			company_id)
    Values (
            @batchstatus_id,
            IsNull(@batch_ref, RTrim(@batch_code)),
            GetDate(),
            @batch_type_id,
            @interface_code,
			@interface_code,
			@source_id)

	SELECT @batch_id=@@IDentity

    -- Now we're done check the batch_ref as we'll write this back to the file...
    Select  @batch_ref = IsNull(@batch_ref, RTrim(@batch_code) + convert(varchar(10), @batch_id))

    Update Batch Set Batch_ref= @batch_ref where batch_id=@batch_id

GO