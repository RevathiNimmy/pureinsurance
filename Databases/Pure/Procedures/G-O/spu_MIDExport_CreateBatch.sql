SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_MIDExport_CreateBatch'
GO

CREATE PROCEDURE spu_MIDExport_CreateBatch      
    @batch_id int output,
	@branch_id int
AS
BEGIN
    DECLARE
        @batch_type_id int,
        @batchstatus_id int,
        @batch_ref varchar(6)

    -- Get batch type id the transactions of Payment Export Type
    SELECT  @batch_type_id = batch_type_id
	FROM    batch_type
    WHERE   code = 'MID1' -- MID_Export

    SELECT  @batchstatus_id = batchstatus_id
    FROM    batchstatus
    WHERE   code = 'BI' -- batch in progress

	----IF EXISTS(SELECT * FROM Batch WHERE batch_type_id = @batch_type_id)
	----	SELECT TOP 1 @batch_ref = CONVERT(INT, batch_ref) + 1 FROM Batch WHERE batch_type_id = @batch_type_id ORDER BY batch_id DESC
	----ELSE
	----	SELECT @batch_ref = '1'

	SELECT @batch_ref = Current_File_Seq_Num 
	FROM MID_Rule R
	WHERE R.Source_id = @branch_id
		AND R.Is_Deleted <> 1
		AND R.MID_Type = 'MID1'
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
            'MID Export',
			0,
			'MID Export'
			,@branch_id)

    SELECT @batch_id = @@IDENTITY

END
GO