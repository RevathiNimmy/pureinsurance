SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_LedgerType_Code'
GO


CREATE PROCEDURE spu_ACT_Get_LedgerType_Code
    @account_id int,
    @ledger_id int OUTPUT,
    @ledgertype_code varchar(10) OUTPUT
AS

SELECT
        @ledger_id=L.ledger_id,
        @ledgertype_code = LT.code
FROM    LedgerType LT
INNER JOIN
        Ledger L ON L.ledgertype_id=LT.ledgertype_id
INNER JOIN
        Account A ON A.ledger_id=L.ledger_id
WHERE   A.account_id = @account_id

GO


