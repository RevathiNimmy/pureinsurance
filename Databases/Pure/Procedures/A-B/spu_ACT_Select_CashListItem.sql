SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_CashListItem'
GO


CREATE PROCEDURE spu_ACT_Select_CashListItem  
    @cashlistitem_id int  
AS  
  
SELECT  
    cashlistitem_id,  
    allocationstatus_id,  
    mediatype_id,  
    CLI.cashlist_id,  
    account_id,  
    media_ref,  
    our_ref,  
    their_ref,  
    amount,  
    transdetail_id,  
    contact_name,  
    address1,  
    address2,  
    address3,  
    address4,  
    postal_code,  
    address_country,  
    payment_name,  
    payment_account_code,  
    payment_branch_code,  
    payment_expiry_date,  
    payment_reference1,  
    payment_reference2,  
    letter,  
    CLI.batch_id,  
    CLI.pmuser_id,  
    transaction_date,  
    original_amount,  
    amount_tendered,  
    change,  
    CLI.cashlistitem_receipt_type_id,  -- PN 56851  
    cashlistitem_receipt_status_id,  
    cashlistitem_bank_id,  
    cheque_date,  
    cc_number,  
    cc_expiry_date,  
    cc_start_date,  
    cc_issue,  
    cc_pin,  
    cc_auth_code,  
    receipt_details,  
    cashlistitem_reverse_pmuser_id,  
    cashlistitem_reverse_reason_id,  
    cashlistitem_payment_type_id,  
    cashlistitem_payment_status_id,  
    date_presented,  
    cheque_in_possession,  
    stop_requested_date,  
    stop_printed_date,  
    stop_confirmation_date,  
    reason,  
    replaces_cashlistitem_id,  
    xml_object,  
    underwriting_year_id,  
    currency_base_date,  
 currency_base_xrate,  
 account_base_date,  
 account_base_xrate,  
 system_base_date,  
 system_base_xrate,  
 exchange_rate_override_reason_id,  
 mediatype_issuer_id,  
 cc_name,  
 cc_customer,  
 cc_manual_auth_code,  
 cc_transaction_code,  
 party_bank_id,  
 collection_date,  
 comments,  
 CRT.is_instalment,
 PMUser.Username,  
 bank_location,  
 bank_branch,  
 chequetype_id,  
 Cheque_clearing_type_id,  
 cc_bank_id,  
 type_of_card_id,  
 cc_trans_slip_no,
 is_lead,
 split_total,
 CL.currency_id,
 Taxamount,
 tax_band_id,
 business_identifier_code,
 international_bank_account_number, 
 insurance_ref 
FROM CashListItem CLI
 LEFT JOIN CashListItem_Receipt_Type CRT  
 ON CRT.CashListItem_Receipt_Type_id = CLI.cashlistitem_receipt_type_id  
LEFT JOIN PMUser ON PMUser.User_id = CLI.PMUser_id 
LEFT JOIN CashList CL ON CL.cashlist_id = CLI.cashlist_id
WHERE cashlistitem_id = @cashlistitem_id  
GO


