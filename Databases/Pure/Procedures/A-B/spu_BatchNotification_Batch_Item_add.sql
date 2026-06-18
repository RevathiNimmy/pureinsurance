SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

DDLDROPPROCEDURE 'spu_BatchNotification_Batch_Item_add'
GO
CREATE PROCEDURE spu_BatchNotification_Batch_Item_add
	@batch_id int,
	@party_key int,
	@insurance_file_key int,
	@insurance_folder_key int,
	@claim_key int=NULL
AS

DECLARE @NewStatusId int

SELECT
	@NewStatusId = batch_notification_status_id
FROM
	batch_notification_status
WHERE
	code = 'NEW'

INSERT INTO Batch_notification_item 
	(Batch_Id,Party_Key,Insurance_File_Key,insurance_folder_key,claim_key,batch_notification_status_id,failure_text) 
VALUES
	(@batch_id,@party_key,@insurance_file_key,@insurance_folder_key,@claim_key,@NewStatusId,'')
GO