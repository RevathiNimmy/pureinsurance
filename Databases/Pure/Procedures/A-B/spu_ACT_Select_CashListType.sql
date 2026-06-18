SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_CashListType'
GO


CREATE PROCEDURE spu_ACT_Select_CashListType
    @cashlisttype_id int
AS


SELECT
    cashlisttype_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM CashListType
WHERE cashlisttype_id = @cashlisttype_id
GO


