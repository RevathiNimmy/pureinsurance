SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_renewal_control_edi_audit_upd'
GO


CREATE PROCEDURE spu_renewal_control_edi_audit_upd
    @insurance_folder_cnt int,
    @renewal_edi_audit_id int
AS

/* Select Fields For MtaAtRenewal */
BEGIN
    UPDATE renewal_control
    SET renewal_edi_audit_id = @renewal_edi_audit_id
    WHERE insurance_folder_cnt = @insurance_folder_cnt
END
GO


