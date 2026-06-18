SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Contact_Usage_del'
GO

CREATE PROCEDURE spe_Party_Contact_Usage_del
    @party_cnt int,
    @contact_cnt int
AS
DELETE FROM Party_Contact_Usage
WHERE party_cnt = @party_cnt AND contact_cnt = @contact_cnt

GO

