SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Corporate_Client_del'
GO
CREATE PROCEDURE spe_Party_Corporate_Client_del
    @party_cnt int
AS
DELETE FROM Party_Corporate_Client
WHERE party_cnt = @party_cnt

GO

