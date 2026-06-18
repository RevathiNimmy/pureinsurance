SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_insurance_file_upd'
GO


CREATE PROCEDURE spu_SIR_insurance_file_upd
    @insurance_folder_cnt int,
    @status_code varchar(100)
AS

/* IJM 120701 To update the insurance file type id */
BEGIN
    /* Now update the insurance file */
    UPDATE insurance_file
    SET insurance_file_type_id = t.insurance_file_type_id
        FROM insurance_file i,
                           insurance_file_type t,
                   renewal_control rc
        WHERE rc.renewal_insurance_file_cnt = i.insurance_file_cnt
        AND rc.insurance_folder_cnt = @insurance_folder_cnt
        AND t.code = @status_code
END
GO


