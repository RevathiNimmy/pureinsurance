SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Default_del'
GO

CREATE PROCEDURE spe_Coi_Default_del
    @coi_default_id int
AS
DELETE FROM Coi_Default
WHERE coi_default_id = @coi_default_id

GO

