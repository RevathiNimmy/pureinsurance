SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Invoice'
GO


CREATE PROCEDURE spu_ACT_Add_Invoice
    @invoice_id int,
    @account_id int,
    @invoice_number varchar(40),
    @order_no varchar(40),
    @description varchar(255),
    @code varchar(30),
    @reference varchar(40),
    @invoice_date datetime,
    @company_id int
AS

INSERT INTO Invoice
(
    invoice_id ,
    account_id ,
    invoice_number ,
    order_no ,
    description ,
    code ,
    reference ,
    invoice_date,
    company_id
)
VALUES
(
    @invoice_id,
    @account_id,
    @invoice_number,
    @order_no,
    @description,
    @code,
    @reference,
    @invoice_date,
    @company_id
)

GO


