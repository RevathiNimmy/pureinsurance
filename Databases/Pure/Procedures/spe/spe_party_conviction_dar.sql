SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_conviction_dar'
GO

CREATE PROCEDURE spe_party_conviction_dar
 @party_cnt int
AS
DELETE
FROM party_conviction
WHERE party_cnt = @party_cnt

GO

