SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_LedgerType'
GO


CREATE PROCEDURE spu_ACT_Select_LedgerType
    @ledgertype_id smallint
AS


SELECT
    ledgertype_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM LedgerType
WHERE ledgertype_id = @ledgertype_id
GO


