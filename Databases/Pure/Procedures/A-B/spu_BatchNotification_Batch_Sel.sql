SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

DDLDROPPROCEDURE 'spu_BatchNotification_Batch_Sel'
GO
CREATE PROCEDURE spu_BatchNotification_Batch_Sel
	@batch_id int
AS
SELECT
	bni.batch_notification_item_id,
	bni.party_key,
	bni.insurance_file_key,
	bni.insurance_folder_key,
 	bni.claim_key
FROM
	batch_notification_item bni
INNER JOIN
	batch_notification_status bns
ON
	bni.batch_notification_status_id = bns.batch_notification_status_id
WHERE
	bni.batch_id = @batch_id
AND
	bns.code = 'NEW'
GO