SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Locator_saa'
GO

CREATE PROCEDURE spe_Party_Locator_saa
    @party_cnt int,
    @locator_type_id int
AS
SELECT
    party_cnt,
    locator_type_id,
    party_locator_id,
    value
 FROM Party_Locator
WHERE party_cnt = @party_cnt AND locator_type_id = @locator_type_id
ORDER BY party_locator_id ASC

GO

