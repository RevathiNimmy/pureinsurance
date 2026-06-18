SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Locator_dar'
GO

CREATE PROCEDURE spe_Party_Locator_dar
 @party_cnt int , @locator_type_id int
AS
DELETE
FROM Party_Locator
WHERE party_cnt = @party_cnt AND locator_type_id = @locator_type_id

GO

