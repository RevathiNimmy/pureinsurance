SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_renewal_edi_audit_del'
GO
CREATE PROCEDURE spe_renewal_edi_audit_del
    @renewal_edi_audit_id integer
AS

DELETE FROM renewal_edi_audit
WHERE renewal_edi_audit_id = @renewal_edi_audit_id
GO

