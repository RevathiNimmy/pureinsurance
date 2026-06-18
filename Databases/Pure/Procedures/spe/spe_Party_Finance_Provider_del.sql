SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Party_Finance_Provider_del'
GO
CREATE PROCEDURE spe_Party_Finance_Provider_del
    @party_cnt int
AS

DELETE FROM Party_Finance_Provider

WHERE party_cnt = @party_cnt

GO

