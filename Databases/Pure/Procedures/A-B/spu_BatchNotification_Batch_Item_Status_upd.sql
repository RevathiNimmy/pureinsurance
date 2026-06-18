SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


DDLDROPPROCEDURE 'spu_BatchNotification_Batch_Item_Status_upd'
GO
CREATE PROCEDURE spu_BatchNotification_Batch_Item_Status_upd
	@batch_notification_item_id int,
	@failure_text varchar(255)
AS

DECLARE @StatusId int
DECLARE @StatusCode varchar(8)

IF LEN(RTRIM(LTRIM(@failure_text))) = 0
	SELECT @StatusCode = 'COMPLETE'
ELSE
	SELECT @StatusCode = 'FAILED'

SELECT
	@StatusId = batch_notification_status_id
FROM
	batch_notification_status
WHERE
	code = @StatusCode

UPDATE 
	batch_notification_item
SET
	batch_notification_status_id = @StatusId,
	failure_text = @failure_text
WHERE 
	batch_notification_item_id = @batch_notification_item_id
GO