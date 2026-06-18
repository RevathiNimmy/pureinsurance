SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Party_del'
GO

CREATE PROCEDURE spe_Export_Party_del
    @export_party_id int
AS
DELETE FROM Export_Party
WHERE export_party_id = @export_party_id

GO

