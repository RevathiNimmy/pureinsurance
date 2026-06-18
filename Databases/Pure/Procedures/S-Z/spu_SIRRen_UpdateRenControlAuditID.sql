SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_UpdateRenControlAuditID'
GO


CREATE PROCEDURE spu_SIRRen_UpdateRenControlAuditID
AS


BEGIN
    DECLARE @rensel_id int

    /* Dont display rows affected */
    SET NOCOUNT ON

    /* Get the renewal selection id */
    SELECT @rensel_id = renewal_status_type_id FROM renewal_status_type WHERE code = 'RENSEL'

    UPDATE renewal_control
    SET renewal_edi_audit_id =
    (
        SELECT MAX(rea.renewal_edi_audit_id)
        FROM renewal_edi_audit rea
        WHERE rea.insurance_folder_cnt = rc.insurance_folder_cnt
          AND rea.renewal_edi_status = 0
    )
    FROM renewal_control rc
    -- AK 031201 will probably not need to check this, specially when a revised EDI
    -- is received, it should check for Null anyway.
    WHERE /* rc.renewal_edi_audit_id IS NULL
      AND */ rc.renewal_status_type_id = @rensel_id
    /* Display the rows affected */
    SET NOCOUNT OFF
END
GO


