SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_TaxType'
GO


CREATE PROCEDURE spu_ACT_SelAll_TaxType
AS


SELECT
    taxtype_id,
    code,
    caption_id,
    description,
    tax_basis,
    is_deleted,
    effective_date
FROM TaxType
GO


