SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_PaymentType'
GO


CREATE PROCEDURE spu_ACT_SelAll_PaymentType
AS


SELECT
    paymenttype_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM PaymentType
ORDER BY code
GO


