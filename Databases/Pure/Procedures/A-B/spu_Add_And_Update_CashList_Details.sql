SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Add_And_Update_CashList_Details'
GO

CREATE PROCEDURE spu_Add_And_Update_CashList_Details
    @OldInsuranceFileCnt VARCHAR(255),
    @NewInsuranceFileCnt VARCHAR(255),
	@ccTokenId VARCHAR(255)
AS
BEGIN

DECLARE @cashlist_id INT 
DECLARE @OldCashlistId INT
DECLARE @cashlistitem_id INT

SELECT @OldCashlistId= cashList_id FROM CashListItem WHERE cc_insurance_file_cnt=@OldInsuranceFileCnt

INSERT INTO CashList (  
    bankaccount_id ,      cashlisttype_id ,      cashliststatus_id ,      cashlist_ref ,      company_id ,      sub_branch_id ,  
    currency_id ,      list_date ,      control_total ,      item_count ,      cashlist_drawer_id ,      batch_id ,  
    pmuser_id ,      confirm_pmuser_id ,      confirm2_pmuser_id ,      date_approved ,      banking_total ,  
    cash_float_amount ,      date_deposited )  
 SELECT bankaccount_id ,      cashlisttype_id ,      cashliststatus_id ,      cashlist_ref ,      company_id ,      sub_branch_id ,  
    currency_id ,      list_date ,      control_total ,      item_count ,      cashlist_drawer_id ,      batch_id ,  
    pmuser_id ,      confirm_pmuser_id ,      confirm2_pmuser_id ,      date_approved ,      banking_total ,  
    cash_float_amount ,      date_deposited
FROM CashList WITH (NOLOCK) WHERE cashlist_id =@OldCashlistId

SELECT @cashlist_id=@@IDENTITY  


INSERT INTO CashListItem  
(  
    allocationstatus_id ,  mediatype_id ,      cashlist_id ,      account_id ,      media_ref ,      our_ref ,      their_ref ,  
    amount ,      transdetail_id ,      contact_name ,      address1 ,      address2 ,      address3 ,      address4 ,      postal_code ,  
    address_country ,      Party_Bank_Id ,      payment_name ,      payment_account_code ,      payment_branch_code ,      payment_expiry_date ,  
    payment_reference1 ,      payment_reference2,      letter,      batch_id,      pmuser_id,      transaction_date,      original_amount,  
    amount_tendered,      change,      cashlistitem_receipt_type_id,      cashlistitem_receipt_status_id,      cashlistitem_bank_id,  
    cheque_date,      cc_number,      cc_expiry_date,      cc_start_date,      cc_issue,      cc_pin,      cc_auth_code,      receipt_details,  
    cashlistitem_reverse_pmuser_id,      cashlistitem_reverse_reason_id,      cashlistitem_payment_type_id,      cashlistitem_payment_status_id,  
    date_presented,      cheque_in_possession,      stop_requested_date,      stop_printed_date,      stop_confirmation_date,      reason,  
    replaces_cashlistitem_id,      xml_object,      underwriting_year_id,      currency_base_date,      currency_base_xrate,      account_base_date,  
    account_base_xrate,      system_base_date,      system_base_xrate,      exchange_rate_override_reason_id,      mediatype_issuer_id,  
    cc_name,      cc_customer,      cc_manual_auth_code,      cc_transaction_code,      collection_date,      comments,      MediaType_Status_id,  
    bank_location,      bank_branch,      chequetype_id,      Cheque_clearing_type_id,      cc_bank_id,      type_of_card_id,  
    cc_trans_slip_no,      is_lead,      split_total,      tax_band_id,      TaxAmount,      business_identifier_code,      international_bank_account_number ,
	cc_insurance_file_cnt,cc_token_id 
)  
SELECT   allocationstatus_id ,  mediatype_id ,      @cashlist_id ,      account_id ,      media_ref ,      our_ref ,      their_ref ,  
    amount ,      null ,      contact_name ,      address1 ,      address2 ,      address3 ,      address4 ,      postal_code ,  
    address_country ,      Party_Bank_Id ,      payment_name ,      payment_account_code ,      payment_branch_code ,      payment_expiry_date ,  
    payment_reference1 ,      payment_reference2,      letter,      batch_id,      pmuser_id,      transaction_date,      original_amount,  
    amount_tendered,      change,      cashlistitem_receipt_type_id,      cashlistitem_receipt_status_id,      cashlistitem_bank_id,  
    cheque_date,      cc_number,      cc_expiry_date,      cc_start_date,      cc_issue,      cc_pin,      cc_auth_code,      receipt_details,  
    cashlistitem_reverse_pmuser_id,      cashlistitem_reverse_reason_id,      cashlistitem_payment_type_id,      cashlistitem_payment_status_id,  
    date_presented,      cheque_in_possession,      stop_requested_date,      stop_printed_date,      stop_confirmation_date,      reason,  
    replaces_cashlistitem_id,      xml_object,      underwriting_year_id,      currency_base_date,      currency_base_xrate,      account_base_date,  
    account_base_xrate,      system_base_date,      system_base_xrate,      exchange_rate_override_reason_id,      mediatype_issuer_id,  
    cc_name,      cc_customer,      cc_manual_auth_code,      NEWID() ,      collection_date,      comments,      MediaType_Status_id,  
    bank_location,      bank_branch,      chequetype_id,      Cheque_clearing_type_id,      cc_bank_id,      type_of_card_id,  
    cc_trans_slip_no,      is_lead,      split_total,      tax_band_id,      TaxAmount,      business_identifier_code,      international_bank_account_number ,
	@NewInsuranceFileCnt,@ccTokenId 

FROM CashListItem WITH (NOLOCK)  WHERE CashList_ID= @OldCashlistId

SELECT @cashlistitem_id=@@IDENTITY 

SELECT @cashlist_id,@cashlistitem_id


END
