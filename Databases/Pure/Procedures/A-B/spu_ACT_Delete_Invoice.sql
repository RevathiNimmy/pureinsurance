SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Invoice'
GO


CREATE PROCEDURE spu_ACT_Delete_Invoice
    @invoice_id int
AS


DELETE FROM Invoice
WHERE invoice_id = @invoice_id
GO


