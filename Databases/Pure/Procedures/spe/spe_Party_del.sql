SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_del'
GO

CREATE PROCEDURE spe_Party_del
    @party_cnt int
AS

DELETE FROM Party

WHERE party_cnt = @party_cnt

GO

