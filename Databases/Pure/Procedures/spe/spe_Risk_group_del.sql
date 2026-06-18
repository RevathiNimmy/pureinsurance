SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_group_del'
GO

CREATE PROCEDURE spe_Risk_group_del
    @risk_group_id int
AS
DELETE FROM Risk_group
WHERE risk_group_id = @risk_group_id

GO

