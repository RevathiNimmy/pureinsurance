SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_GLExport_CreateBatch'
GO

CREATE PROCEDURE spu_ACT_GLExport_CreateBatch
    @batch_id int output,
    @period_id int  = 0
AS

    Declare 
        @batch_type_id int,
        @batchstatus_id int,
        @total_amount money,
        @total_transactions int,
		@period nvarchar(11),
		@Description varchar(255) = 'General Ledger'
    -- Get batch type id
    Select  @batch_type_id = batch_type_id
    From    batch_type
    Where   code = 'GLX' -- general ledger export

    Select  @batchstatus_id = batchstatus_id
    From    batchstatus
    Where   code = 'BI' -- batch in progress

	SELECT @period = CONVERT(nvarchar(11), period_end_date, 106)
	FROM Period
	WHERE period_id = @period_id

	IF @period_id = 0
	SET @Description += '_All'
	ELSE
	SET @Description += '_' + @period

    -- Insert new batch
    Insert  Batch (
            batchstatus_id,
            batch_ref,
            created_date,
            batch_type_id,
            interface_code,
			Description)
    Values (@batchstatus_id,
            'GLX',
            GetDate(),
            @batch_type_id,
            'GL_Export',
			@Description)

    SELECT @batch_id = @@IDENTITY
		
		
    -- Update batch_ref for inserted row
    UPDATE batch
    SET batch_ref = 'GLX' + convert(varchar(10), @batch_id)
    WHERE batch_id = @batch_id

    -- Update batch on transdetails not yet batched
     if @period_id >0 
		Update  transdetail  
		Set     batch_id = @batch_id  
		Where   --batch_id Is Null  
		 period_id =@period_id 
	else
		Update  transdetail  
		Set     batch_id = @batch_id  
		Where   batch_id Is Null 
		
		-- Issue fix for 79082
		-- Exclude Insurer payment marked write-offs
	Update transdetail Set batch_id = Null
		From transdetail td
			Inner Join document d ON td.document_id = d.document_id
			Inner Join documenttype dt ON dt.documenttype_id = d.documenttype_id
		Where td.batch_id = @batch_id
			And Upper(RTRIM(td.spare)) like 'WRITEOFF' -- A write off entry
			And td.document_sequence > 2 -- injected in existing doc
			And Upper(RTRIM(dt.code)) <> 'SWD' -- not marked as writeoff type doc    
    
    -- if systemoption 5022 (Exclude Payments for Unprinted Cheques) is active
    IF EXISTS(SELECT * FROM system_options WHERE option_number=5022 AND value='1')
    BEGIN
    	-- Exclude those records where Cheques has not been printed 
    	UPDATE  transdetail
	SET     batch_id = NULL
	FROM transdetail td JOIN Cheque c ON c.transdetail_id=td.transdetail_id
    	WHERE c.printed_date IS NULL  
    	AND td.batch_id=@batch_id
    END

    -- Grab count and total amounts for batch
    Select  @total_amount = Sum(system_amount),
            @total_transactions = Count(*)
    From    transdetail
    Where   batch_id = @batch_id

    -- Update batch
    Update  batch
    Set     total_amount = IsNull(@total_amount, 0),
            total_transactions = IsNull(@total_transactions, 0)
    Where   batch_id = @batch_id

Go



