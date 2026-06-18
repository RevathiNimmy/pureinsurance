SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_control_upd'
GO


CREATE PROCEDURE spu_SIR_renewal_control_upd
    @insurance_folder_cnt int,
    @status_code varchar(100)
AS


BEGIN
    UPDATE renewal_Control
    SET renewal_status_type_id = s.renewal_status_type_id
        FROM renewal_control c,
                           renewal_status_type s
        WHERE c.insurance_folder_cnt = @insurance_folder_cnt
        AND s.code = @status_code
END
GO


