SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

DDLDROPPROCEDURE 'spu_BatchNotification_Purge'
GO
CREATE PROCEDURE spu_BatchNotification_Purge
@batch_ref varchar(20)  = ''
AS  

IF ISNULL(@batch_ref ,'') <> ''
BEGIN
	DELETE FROM batch_notification_item WHERE batch_id in (SELECT batch_id FROM Batch WHERE batch_ref = @batch_ref)  
  
	DELETE FROM Batch WHERE batch_ref = @batch_ref
END

ELSE
BEGIN

	CREATE TABLE #TempBatch(batch_id int)

	INSERT INTO #TempBatch
	SELECT batch_id FROM batch_notification_item

	DELETE FROM batch_notification_item
	DELETE FROM Batch WHERE batch_id in (SELECT batch_id FROM #TempBatch)	
	
	DROP TABLE #TempBatch

END

GO