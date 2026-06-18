SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_PaymentType'
GO


CREATE PROCEDURE spu_ACT_Select_PaymentType
    @paymenttype_id smallint
AS


SELECT
    paymenttype_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM PaymentType
WHERE paymenttype_id = @paymenttype_id
GO


