SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_prospect_del'
GO

CREATE PROCEDURE spe_party_prospect_del
    @party_cnt int
AS
DELETE FROM party_prospect
WHERE party_cnt = @party_cnt

GO

