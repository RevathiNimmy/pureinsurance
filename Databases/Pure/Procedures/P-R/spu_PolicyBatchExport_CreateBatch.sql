SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PolicyBatchExport_CreateBatch'
GO 

CREATE PROCEDURE spu_PolicyBatchExport_CreateBatch
    @batch_id int output,
	@StartDate DATETIME = NULL,
	@EndDate DATETIME = NULL  
AS
  
    Declare  
        @batch_type_id int,
        @batchstatus_id int,  
        @total_amount money,  
        @total_transactions int ,
		@Description VARCHAR(255) = 'Policy Batch Export' 
  
    -- Get batch type id  
    Select  @batch_type_id = batch_type_id  
    From    batch_type  
    Where   code = 'POLBAT' -- document_export
  
    Select  @batchstatus_id = batchstatus_id  
    From    batchstatus  
    Where   code = 'BI' -- batch in progress  
  
	IF len(@StartDate) > 0 
	SET @Description += '_'+CONVERT(VARCHAR(11),@StartDate,101)

	IF len(@EndDate) > 0 
	SET @Description += '_'+CONVERT(VARCHAR(11),@EndDate,101)

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
            'POLBAT',  
            GetDate(),  
            @batch_type_id,  
            'RECURRING',
	        0,
		    @Description)  

    Select  @batch_id = @@IDENTITY

    -- Update batch_ref for inserted row
    UPDATE batch
    SET batch_ref = 'POLBAT' + convert(varchar(10), @batch_id)
    WHERE batch_id = @batch_id

	GO