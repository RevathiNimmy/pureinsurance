SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_update_renewal_status'
GO


CREATE PROCEDURE spu_update_renewal_status
    @insurance_file_cnt INT,
    @renewal_status_type_id INT
AS


UPDATE  renewal_status
SET renewal_status_type_id = @renewal_status_type_id
WHERE   insurance_file_cnt in (
    SELECT  if2.insurance_file_cnt
    FROM    insurance_file if1,
        insurance_file if2
    WHERE   if1.insurance_file_cnt = @insurance_file_cnt
    AND if2.insurance_folder_cnt = if1.insurance_folder_cnt)
GO


