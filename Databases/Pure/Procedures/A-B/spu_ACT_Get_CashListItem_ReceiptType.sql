
EXECUTE DDLDropProcedure 'spu_ACT_Get_CashListItem_ReceiptType'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
CREATE PROCEDURE spu_ACT_Get_CashListItem_ReceiptType
    @cashlistitem_receipt_type_id varchar(20)
AS
SELECT
    code
FROM cashlistitem_receipt_type
WHERE cashlistitem_receipt_type_id = @cashlistitem_receipt_type_id

GO