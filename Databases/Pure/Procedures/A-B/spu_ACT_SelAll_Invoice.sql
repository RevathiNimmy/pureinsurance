SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Invoice'
GO


CREATE PROCEDURE spu_ACT_SelAll_Invoice
AS


SELECT
    invoice_id,
    account_id,
    invoice_number,
    order_no,
    description,
    code,
    reference,
    invoice_date
FROM Invoice
GO


