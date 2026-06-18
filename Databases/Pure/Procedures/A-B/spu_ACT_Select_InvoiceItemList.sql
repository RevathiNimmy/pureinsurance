SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_InvoiceItemList'
GO


CREATE PROCEDURE spu_ACT_Select_InvoiceItemList
    @invoice_id int
AS


SELECT
    invoice_id,
    invoice_item_no,
    description,
    nominal_code,
    value,
    currency_id,
    department_id,
    dept_amount,
    vat_rate,
    has_vat
FROM Invoice_item
WHERE invoice_id = @invoice_id
GO


