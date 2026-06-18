SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_fee_amounts_del'
GO



CREATE PROCEDURE spe_fee_amounts_del
    @party_cnt INT
AS

DELETE FROM fee_amounts
WHERE party_cnt = @party_cnt 

GO

