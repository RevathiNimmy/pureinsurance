SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_Account'
GO

CREATE PROCEDURE spu_ACT_Select_Account
    @account_id int
AS
SELECT
    account_id,
    company_id,
    purgefrequency_id,
    accounttype_id,
    paymenttype_id,
    currency_id,
    ledger_id,
    account_name,
    short_code,
    restrict_enquiry,
    restrict_update,
    delete_at_purge,
    contact_name,
    address1,
    address2,
    address3,
    address4,
    postal_code,
    address_country,
    phone_area_code,
    phone_number,
    phone_extension,
    fax_area_code,
    fax_number,
    fax_extension,
    payment_name,
    payment_account_code,
    payment_branch_code,
    payment_expiry_date,
    payment_reference1,
    payment_reference2,
    credit_limit,
    discount_percentage,
    settlement_period,
    bank_name,
    bank_address1,
    bank_address2,
    bank_address3,
    bank_address4,
    bank_postal_code,
    bank_country,
    bank_phone_area_code,
    bank_phone_number,
    bank_phone_extension,
    bank_fax_area_code,
    bank_fax_number,
    bank_fax_extension,
    comments,
    account_key,
    nominal_account_id,
    accountstatus_id,
    prooflist_report_id,
    bordereau_report_id,
    sub_branch_id,
    allow_electronic_payment,
    client_money_calc_account_type,
    client_bank_account_type,
    merchant_id
FROM Account
WHERE account_id = @account_id
GO


