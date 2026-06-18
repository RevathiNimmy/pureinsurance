SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_account_handler_del'
GO

CREATE PROCEDURE spe_party_account_handler_del
    @party_cnt int
AS
DELETE FROM party_account_handler
WHERE party_cnt = @party_cnt

GO

