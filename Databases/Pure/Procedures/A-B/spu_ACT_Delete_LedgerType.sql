SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_LedgerType'
GO


CREATE PROCEDURE spu_ACT_Delete_LedgerType
    @ledgertype_id smallint
AS


DELETE FROM LedgerType
WHERE ledgertype_id = @ledgertype_id
GO


