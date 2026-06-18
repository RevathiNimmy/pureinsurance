SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_PaymentType'
GO


CREATE PROCEDURE spu_ACT_Check_PaymentType
    @paymenttype_id smallint OUTPUT
AS


BEGIN
    SELECT @paymenttype_id = paymenttype_id
    FROM PaymentType
    WHERE paymenttype_id = @paymenttype_id
END
BEGIN
IF @paymenttype_id = NULL
    SELECT @paymenttype_id = -1
END
GO


