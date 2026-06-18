SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Closures'
GO


CREATE PROCEDURE spu_ACT_Select_Closures
    @ledger_id smallint
AS


DECLARE
    @sequence smallint,
    @sub_branch_id int

SELECT  @sub_branch_id=sub_branch_id, @sequence = sequence
FROM    Ledger
WHERE   ledger_id = @ledger_id

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
WHERE sequence = @sequence and
    sub_branch_id=@sub_branch_id
GO


