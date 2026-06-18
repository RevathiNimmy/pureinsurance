SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure spu_MID2Export_CreateBatch
GO

CREATE PROCEDURE spu_MID2Export_CreateBatch
	@batch_id int output,
	@branch_id int
AS
BEGIN
    Declare
        @batch_type_id int,
        @batchstatus_id int,
        @batch_ref varchar(6)

    -- Get batch type id the transactions of MID2 Export Type
    SELECT  @batch_type_id = batch_type_id
    FROM    batch_type
    WHERE   code = 'MID2' -- MID2_Export

    SELECT  @batchstatus_id = batchstatus_id
    FROM    batchstatus
    WHERE   code = 'BI' -- batch in progress
	
	SELECT @batch_ref = Current_File_Seq_Num 
	FROM MID_Rule R
	WHERE R.Source_id = @branch_id
		AND R.Is_Deleted <> 1
		AND R.MID_Type = 'MID2'
		AND CONVERT(DATE, GETDATE()) BETWEEN CONVERT(date, R.Start_Date) AND CONVERT(DATE, R.Expiry_Date) 
	IF ISNULL(@batch_ref,0) = 0
		SELECT @batch_ref = '000001'

    -- Insert new batch
    INSERT  Batch (
            batchstatus_id,
            batch_ref,
            created_date,
            batch_type_id,
            interface_code,
			auto_close,
			Description,
			company_id )
    VALUES (
            @batchstatus_id,
            RIGHT(RTRIM('000000'+CAST(ISNULL(@batch_ref,'') AS VARCHAR)),6),
            GetDate(),
            @batch_type_id,
            'MID2_Export',
			0,
			'MID2_Export',
			@branch_id)

    SELECT @batch_id = @@IDENTITY

END
GO