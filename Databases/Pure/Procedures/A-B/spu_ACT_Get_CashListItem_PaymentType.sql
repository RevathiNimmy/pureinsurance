EXECUTE DDLDropProcedure 'spu_ACT_Get_CashListItem_PaymentType'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_ACT_Get_CashListItem_PaymentType
    @CashListPayTypeCode varchar(20)
AS
SELECT
    cashlistitem_payment_type_id
FROM cashlistitem_payment_type
WHERE code = @CashListPayTypeCode
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
