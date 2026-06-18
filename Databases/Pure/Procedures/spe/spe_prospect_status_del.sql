SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_status_del'
GO

CREATE PROCEDURE spe_prospect_status_del
    @prospect_status_id int
AS
DELETE FROM prospect_status
WHERE prospect_status_id = @prospect_status_id

GO

