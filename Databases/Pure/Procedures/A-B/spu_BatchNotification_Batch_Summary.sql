SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

DDLDROPPROCEDURE 'spu_BatchNotification_Batch_Summary'
GO
CREATE PROCEDURE spu_BatchNotification_Batch_Summary
	@batch_id int
AS

DECLARE @CompleteStatusId INT

SELECT 
	@CompleteStatusId = batch_notification_status_id
FROM
	batch_notification_status
WHERE
	code = 'COMPLETE'


SELECT
	COUNT(*) as 'TotalInBatch',
	SUM(
		CASE 
			WHEN batch_notification_status_id = @CompleteStatusId THEN 1
			ELSE 0
		END) as 'TotalComplete'
FROM 
	batch_notification_item
WHERE 
	batch_id = @batch_id
GO