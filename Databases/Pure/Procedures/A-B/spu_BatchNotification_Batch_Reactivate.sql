SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


DDLDROPPROCEDURE 'spu_BatchNotification_Batch_Reactivate'
GO
CREATE PROCEDURE spu_BatchNotification_Batch_Reactivate
	@batch_id int
AS

DECLARE @NewStatusId int
DECLARE @FailedStatusId int

SELECT 
	@NewStatusId = batch_notification_status_id 
FROM 
	batch_notification_status 
WHERE 
	code = 'NEW'

SELECT 
	@FailedStatusId = batch_notification_status_id 
FROM 
	batch_notification_status 
WHERE 
	code = 'FAILED'

UPDATE 
	batch_notification_item
SET 
	batch_notification_status_id = @NewStatusId,
	failure_text = ''
WHERE
	batch_notification_status_id = @FailedStatusId
AND
	batch_id = @batch_id
GO