SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_Ledger_Code'
GO


CREATE PROCEDURE spu_ACT_Get_Ledger_Code
    @account_id int,
    @ledger_code varchar(10) OUTPUT
AS


DECLARE @ledger_id smallint,
    @ledgertype_id smallint
SELECT @ledger_id = ledger_id
FROM Account
WHERE account_id = @account_id
SELECT @ledgertype_id = ledgertype_id
FROM Ledger
WHERE ledger_id = @ledger_id
SELECT @ledger_code = code
FROM Ledgertype
WHERE ledgertype_id = @ledgertype_id
GO


