SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Ledger'
GO


CREATE PROCEDURE spu_ACT_Update_Ledger
    @ledger_id smallint,
    @sequence smallint,
    @company_id int,
    @sub_branch_id int,
    @ledger_name varchar(30),
    @ledger_short_name varchar(2),
    @mapping_id int,
    @ledgertype_id smallint,
    @current_period_id int,
    @is_deletable bit
AS

UPDATE Ledger SET
    sequence=@sequence,
    company_id=@company_id,
    sub_branch_id=@sub_branch_id,
    ledger_name=@ledger_name,
    ledger_short_name=@ledger_short_name,
    mapping_id=@mapping_id,
    ledgertype_id=@ledgertype_id,
    current_period_id=@current_period_id,
    is_deletable=@is_deletable
WHERE ledger_id = @ledger_id

GO


