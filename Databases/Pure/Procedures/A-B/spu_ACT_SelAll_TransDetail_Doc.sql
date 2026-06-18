SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_SelAll_TransDetail_Doc'
GO

CREATE PROCEDURE spu_ACT_SelAll_TransDetail_Doc  
    @document_id int  
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
    ref_units,  
    insurance_ref_index,  
    department_id,  
    sub_branch_id,  
    underwriting_year_id,  
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
 claim_ref
FROM TransDetail  
    WHERE document_id = @document_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
