SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_TransDetail_Acct'
GO


CREATE PROCEDURE spu_ACT_SelAll_TransDetail_Acct
    @account_id int,
    @period_id int = NULL,
    @start_date datetime = NULL,
    @end_date datetime = NULL,
    @os_only smallint = NULL
AS


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
    ref_units
FROM TransDetail
    WHERE account_id = @account_id
          AND (period_id = @period_id OR @period_id = NULL)
          AND (accounting_date >= @start_date OR @start_date = NULL)
          AND (accounting_date <= @end_date OR @end_date = NULL)
          AND (fully_matched = 0 OR @os_only = NULL)
GO


