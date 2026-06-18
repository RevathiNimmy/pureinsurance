SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Ledger'
GO


CREATE PROCEDURE spu_ACT_Delete_Ledger
    @ledger_id smallint
AS


DELETE FROM Ledger
WHERE ledger_id = @ledger_id
GO


