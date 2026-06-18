SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_InstalmentExport_CreateBatch'
GO


CREATE PROCEDURE spu_ACT_InstalmentExport_CreateBatch  
    @batch_id int output,
	@MediaTypeCode nvarchar(10),
	@lead_days int,
	@bank_account_name varchar(60)  
AS
  
    Declare  
        @batch_type_id int,
        @batchstatus_id int,  
        @total_amount money,  
        @total_transactions int,
		@MediaType nvarchar(255),
		@Description nvarchar(255) = 'Instalment'
  
  
    -- Get batch type id  
    Select  @batch_type_id = batch_type_id  
    From    batch_type  
    Where   code = 'BAC' -- instalment_export
  
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
	
	IF LEN(@lead_days) > 0 
	SET @Description += '_'+CONVERT(VARCHAR, @lead_days)

    -- Insert new batch  
    Insert  Batch (  
            batchstatus_id,  
            batch_ref,  
            created_date,  
            batch_type_id,  
            interface_code, 
	        auto_close,
		    Description)  
    Values (
            @batchstatus_id,  
            'BAC',  
            GetDate(),  
            @batch_type_id,  
            'RECURRING',
	        0,
		    @Description)  

    Select  @batch_id = @@IDENTITY

    -- Update batch_ref for inserted row
    UPDATE batch
    SET batch_ref = 'BAC' + convert(varchar(10), @batch_id)
    WHERE batch_id = @batch_id

GO
