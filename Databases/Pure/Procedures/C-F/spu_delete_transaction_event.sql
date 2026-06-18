SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_delete_transaction_event'
GO


CREATE PROCEDURE spu_delete_transaction_event
    @EventCnt numeric
AS


BEGIN

DELETE from Event_Insurance_Folder WHERE insurance_folder_cnt = @EventCnt
DELETE from Event_Insurance_File WHERE insurance_file_cnt =  @EventCnt
DELETE from Event_Insurance_File_System WHERE insurance_file_cnt =  @EventCnt
DELETE from Event_Policy_fee WHERE insurance_file_cnt = @EventCnt

END
GO


