SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_set_exist_ren_to_rep'
GO


CREATE PROCEDURE spu_SIR_set_exist_ren_to_rep
    @insurance_folder_cnt int
AS


BEGIN

    DECLARE @ren_status_id int
    DECLARE @rep_status_id int

    SELECT @ren_status_id
        = insurance_file_status_id
        FROM insurance_file_status
        WHERE code = 'REN'

    SELECT @rep_status_id
        = insurance_file_status_id
        FROM insurance_file_status
        WHERE code = 'REP'

    UPDATE insurance_file
    SET insurance_file_status_id = @rep_status_id
    WHERE insurance_file_status_id = @ren_status_id
    AND insurance_folder_cnt = @insurance_folder_cnt

END
GO


