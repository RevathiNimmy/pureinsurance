SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Agent_del'
GO

CREATE PROCEDURE spe_Party_Agent_del
    @party_cnt int
AS
DELETE FROM Party_Agent
WHERE party_cnt = @party_cnt

GO

