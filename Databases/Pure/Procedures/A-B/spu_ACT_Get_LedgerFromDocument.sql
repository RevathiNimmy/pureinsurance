SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_LedgerFromDocument'
GO


CREATE PROCEDURE spu_ACT_Get_LedgerFromDocument
    @DocumentID int,
    @LedgerID smallint OUTPUT
AS


DECLARE @AccountID int
SELECT @AccountID = MIN(account_id)
FROM Transdetail
WHERE document_id = @DocumentID
SELECT @LedgerID = ledger_id
FROM Account
WHERE account_id = @AccountID
GO


