SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Invoice'
GO


CREATE PROCEDURE spu_ACT_Select_Invoice
    @invoice_id int
AS

SELECT
    invoice_id,
    account_id,
    invoice_number,
    order_no,
    description,
    code,
    reference,
    invoice_date,
    company_id
FROM Invoice
WHERE invoice_id = @invoice_id

GO


