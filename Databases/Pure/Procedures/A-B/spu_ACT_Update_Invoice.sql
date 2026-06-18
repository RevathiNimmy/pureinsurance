SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Invoice'
GO


CREATE PROCEDURE spu_ACT_Update_Invoice
    @invoice_id int,
    @account_id int,
    @invoice_number varchar(40),
    @order_no varchar(40),
    @description varchar(255),
    @code varchar(10),
    @reference varchar(40),
    @invoice_date datetime,
    @company_id int
AS

UPDATE Invoice
SET
	account_id=@account_id,
    invoice_number=@invoice_number,
    order_no=@order_no,
    description=@description,
    code=@code,
    reference=@reference,
    invoice_date=@invoice_date,
    company_id = @company_id 
WHERE invoice_id = @invoice_id

GO


