
EXECUTE DDLDropProcedure 'spu_ACT_CashListItem_SetPrinted'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_ACT_CashListItem_SetPrinted
    @cashlistitem_id int

AS BEGIN
    UPDATE CashLIstItem
    SET Letter = 0
    WHERE CashLIstItem_id = @cashlistitem_id
END

GO
