SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Party_Personal_Client_del'
GO

CREATE PROCEDURE spe_Party_Personal_Client_del
    @party_cnt int
AS
DELETE FROM Party_Personal_Client
WHERE party_cnt = @party_cnt

GO

