SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_PaymentType'
GO


CREATE PROCEDURE spu_ACT_Delete_PaymentType
    @paymenttype_id smallint
AS


DELETE FROM PaymentType
WHERE paymenttype_id = @paymenttype_id
GO


