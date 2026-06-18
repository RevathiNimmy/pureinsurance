SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_last_message_count_received_upd'
GO


CREATE PROCEDURE spu_last_message_count_received_upd
    @insurance_file_cnt INT,
    @last_edi_message_count_received INT
AS

UPDATE insurance_folder 
SET last_edi_message_count_received = @last_edi_message_count_received
FROM insurance_file fi
WHERE fi.insurance_folder_cnt = insurance_folder.insurance_folder_cnt
AND fi.insurance_file_cnt = @insurance_file_cnt

GO


