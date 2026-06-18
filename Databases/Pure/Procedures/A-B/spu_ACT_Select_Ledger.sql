SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Ledger'
GO


CREATE PROCEDURE spu_ACT_Select_Ledger
    @ledger_id smallint
AS


SELECT
    ledger_id,
    sequence,
    company_id,
    sub_branch_id,
    ledger_name,
    ledger_short_name,
    mapping_id,
    ledgertype_id,
    current_period_id,
    is_deletable
FROM Ledger
WHERE ledger_id = @ledger_id
GO


