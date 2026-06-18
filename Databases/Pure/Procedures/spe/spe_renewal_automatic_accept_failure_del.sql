SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_renewal_automatic_accept_failure_del'
GO

CREATE PROCEDURE spe_renewal_automatic_accept_failure_del
AS
BEGIN
    DELETE FROM renewal_automatic_accept_failure
END

GO
