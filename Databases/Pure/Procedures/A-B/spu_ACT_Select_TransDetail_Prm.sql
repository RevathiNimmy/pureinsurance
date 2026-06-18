SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_TransDetail_Prm'
GO


CREATE PROCEDURE spu_ACT_Select_TransDetail_Prm
    @transdetail_id int
AS


DECLARE @document_id int
SELECT
    @document_id = document_id
    FROM TransDetail
    WHERE transdetail_id = @transdetail_id
SELECT
    transdetail_id,
    account_id,
    postingstatus_id,
    company_id,
    currency_id,
    period_id,
    document_id,
    document_sequence,
    accounting_date,
    amount,
    base_amount_unrounded,
    fully_matched,
    currency_amount,
    currency_amount_unrounded,
    currency_base_xrate,
    euro_currency_id,
    euro_amount,
    euro_base_xrate,
    euro_ccy_xrate,
    comment,
    insurance_ref,
    operator_id,
    purchase_order_no,
    purchase_invoice_no,
    department,
    spare,
    ref_date,
    ref_amount,
    ref_quantity,
    ref_units,
    department_id
FROM TransDetail
WHERE (document_id = @document_id AND document_sequence = 1)
GO


