SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_TransDetail'
GO


CREATE PROCEDURE spu_ACT_SelAll_TransDetail
    @company_id int = NULL,
    @account_id int = NULL,
    @accounting_date datetime = NULL,
    @postingstatus_id smallint = NULL,
    @sub_branch_id int = NULL
AS

IF (@sub_branch_id IS NULL) AND (@company_id IS NOT NULL)
    EXEC spu_sub_branch_default @company_id, @sub_branch_id OUTPUT


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
    insurance_ref_index,
    department_id,
    sub_branch_id,
    underwriting_year_id,
    insurance_ref_index,
	currency_base_xrate,
	account_base_xrate,
	system_base_xrate,
	transdetail_type_id,
	reference,
	type_code,
	amount_currency_id,
	account_currency_id,
	account_amount,
	account_amount_unrounded,
	system_currency_id,
	system_amount,
	system_amount_unrounded,
	outstanding_currency_amount,
	outstanding_amount,
	outstanding_account_amount,
	outstanding_system_amount,
	amount_updated
FROM   TransDetail
WHERE (sub_branch_id = @sub_branch_id OR @sub_branch_id = NULL)
AND   (account_id = @account_id OR @account_id = NULL)
AND   (accounting_date <= @accounting_date OR @accounting_date = NULL)
AND   (postingstatus_id = @postingstatus_id OR @postingstatus_id = NULL)


GO


