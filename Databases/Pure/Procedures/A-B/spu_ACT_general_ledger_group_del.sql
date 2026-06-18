SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_general_ledger_group_del'
GO

CREATE PROCEDURE spu_ACT_general_ledger_group_del
AS
    TRUNCATE TABLE general_ledger_group
GO

