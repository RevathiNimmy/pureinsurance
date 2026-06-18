SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_lifestyle_del'
GO

CREATE PROCEDURE spe_party_lifestyle_del
    @party_cnt int,
    @party_lifestyle_id int
AS
DELETE FROM party_lifestyle
WHERE party_cnt = @party_cnt AND party_lifestyle_id = @party_lifestyle_id

GO

