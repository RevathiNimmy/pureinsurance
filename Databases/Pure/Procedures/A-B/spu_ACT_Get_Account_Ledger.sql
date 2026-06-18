SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Account_Ledger'
GO


CREATE PROCEDURE spu_ACT_Get_Account_Ledger
    @account_id int,
    @Ledger_id int OUTPUT,
    @Ledger_Code varchar(255) OUTPUT
AS
--DC270503 -ISS4353 -rewrote as wasnt working correctly
SELECT @ledger_id = a.ledger_id,
    @ledger_code = lt.description
FROM Account a
JOIN ledger l
ON a.ledger_id = l.ledger_id
JOIN ledgertype lt
ON l.ledgertype_id = lt.ledgertype_id
WHERE a.account_id = @account_id

GO


