SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_TransDetail'
GO


CREATE PROCEDURE spu_ACT_Select_TransDetail 
    @transdetail_id int,
	@is_third_party int = 0
AS

IF @is_third_party = 0
BEGIN
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
		amount_updated,
		tax_group_id,
		tax_band_id,
		claim_ref,
			balance_type
	FROM TransDetail
	WHERE transdetail_id = @transdetail_id
END

ELSE IF @is_third_party = 1
BEGIN
	select 
		t.transdetail_id,
		a.account_id,
		t.postingstatus_id,
		t.company_id,
		t.currency_id,
		t.period_id,
		t.document_id,
		t.document_sequence,
		t.accounting_date,
		t.amount,
		t.base_amount_unrounded,
		t.fully_matched,
		t.currency_amount,
		t.currency_amount_unrounded,
		t.currency_base_xrate,
		t.euro_currency_id,
		t.euro_amount,
		t.euro_base_xrate,
		t.euro_ccy_xrate,
		t.comment,
		t.insurance_ref,
		t.operator_id,
		t.purchase_order_no,
		t.purchase_invoice_no,
		t.department,
		t.spare,
		t.ref_date,
		t.ref_amount,
		t.ref_quantity,
		t.ref_units,
		t.department_id,
		t.sub_branch_id,
		t.underwriting_year_id,
		t.insurance_ref_index,
		t.currency_base_xrate,
		t.account_base_xrate,
		t.system_base_xrate,
		t.transdetail_type_id,
		t.reference,
		t.type_code,
		t.amount_currency_id,
		t.account_currency_id,
		t.account_amount,
		t.account_amount_unrounded,
		t.system_currency_id,
		t.system_amount,
		t.system_amount_unrounded,
		t.outstanding_currency_amount,
		t.outstanding_amount,
		t.outstanding_account_amount,
		t.outstanding_system_amount,
		t.amount_updated,
		t.tax_group_id,
		t.tax_band_id,
		t.claim_ref,
		t.balance_type
	from transdetail t
	inner join PFPremiumFinance pf on t.insurance_ref = pf.pfprem_finance_cnt 
	inner join account a on a.account_key = ISNULL(agent_cnt,ClientId)
	where t.transdetail_id = @transdetail_id
END
GO


