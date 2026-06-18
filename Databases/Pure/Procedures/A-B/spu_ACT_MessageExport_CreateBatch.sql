SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO 

EXECUTE DDLDropProcedure 'spu_ACT_MessageExport_CreateBatch'
GO
Create Procedure spu_ACT_MessageExport_CreateBatch
    @batch_id int OUTPUT,
	@event_type_code VARCHAR(10)
AS

Declare 
    @batch_type_id INT,
    @batchstatus_id INT,
	@event_type NVARCHAR(255),
	@Description NVARCHAR(255) = 'Message Export'


Select @batch_type_id = batch_type_id 
    From Batch_type
    Where Code='MSGX'

Select @batchstatus_id = batchstatus_id 
    From batchstatus
    where code='BI' 

	SELECT @event_type = description
	FROM event_type 
	WHERE code = @event_type_code 

	IF LEN(@event_type) > 0 
	SET @Description += '_'+@event_type

-- Create New Batch

Insert into Batch
(
    batchstatus_id,
    batch_ref,
    created_date,
    batch_type_id,
    interface_code,
    auto_close,
	Description
)
values
(
    @batchstatus_id,
    'MSGX',
    GetDate(),
    @Batch_type_id,
    'Message Export',
    0,
	@Description
)

    SELECT @batch_id = @@IDENTITY
		
    -- Update batch_ref for inserted row
    UPDATE batch
    SET batch_ref = 'MSGX'+Convert(varchar(10),@batch_id)
    WHERE batch_id = @batch_id

GO
