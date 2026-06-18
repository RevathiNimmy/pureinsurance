SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Private_Text_dar'
GO

CREATE PROCEDURE spe_Party_Private_Text_dar
 @party_cnt int
AS
DELETE
FROM Party_Private_Text
WHERE party_cnt = @party_cnt

GO

