SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Address_Usage_del'
GO

CREATE PROCEDURE spe_Party_Address_Usage_del
    @party_cnt int,
    @address_cnt int
AS
DELETE FROM Party_Address_Usage
WHERE party_cnt = @party_cnt AND address_cnt = @address_cnt

GO

