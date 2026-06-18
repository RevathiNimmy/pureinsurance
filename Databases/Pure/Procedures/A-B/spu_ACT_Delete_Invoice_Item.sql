SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Invoice_Item'
GO


CREATE PROCEDURE spu_ACT_Delete_Invoice_Item
    @invoice_id int
AS


DELETE FROM Invoice_item
WHERE invoice_id = @invoice_id
GO


