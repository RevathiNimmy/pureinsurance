SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_CashListType'
GO


CREATE PROCEDURE spu_ACT_SelAll_CashListType
AS


SELECT
    cashlisttype_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM CashListType
GO


