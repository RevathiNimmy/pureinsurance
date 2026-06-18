SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Relationship_del'
GO

CREATE PROCEDURE spe_Party_Relationship_del
    @party_cnt int,
    @relation_cnt int
AS
DELETE FROM Party_Relationship
WHERE party_cnt = @party_cnt AND relation_cnt = @relation_cnt

DELETE FROM Party_Relationship
WHERE party_cnt = @relation_cnt AND relation_cnt = @party_cnt

GO

