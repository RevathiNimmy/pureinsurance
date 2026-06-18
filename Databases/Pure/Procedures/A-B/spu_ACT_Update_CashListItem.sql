SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_CashListItem'
GO


CREATE PROCEDURE spu_ACT_Update_CashListItem
    @cashlistitem_id int,
    @allocationstatus_id int,
    @mediatype_id int,
    @cashlist_id int,
    @account_id int,
    @media_ref varchar(100),
    @our_ref varchar(255),
    @their_ref varchar(100),
    @amount numeric(19,4),
    @transdetail_id int,
    @contact_name varchar(60),
    @address1 varchar(60),
    @address2 varchar(60),
    @address3 varchar(60),
    @address4 varchar(60),
    @postal_code varchar(20),
    @address_country smallint,
    @payment_name varchar(60),
    @payment_account_code varchar(60),
    @payment_branch_code varchar(30),
    @payment_expiry_date datetime,
    @payment_reference1 varchar(30),
    @payment_reference2 varchar(30),
    @letter tinyint,
    @batch_id int,
    @pmuser_id int,
    @transaction_date datetime,
    @original_amount numeric(19, 4),
    @amount_tendered numeric(19, 4),
    @change numeric(19, 4),
    @cashlistitem_receipt_type_id int,
    @cashlistitem_receipt_status_id int,
    @cashlistitem_bank_id int,
    @cheque_date datetime,
    @cc_number varchar (30),
    @cc_expiry_date varchar (10),
    @cc_start_date varchar (10),
    @cc_issue varchar (2),
    @cc_pin varchar (20),
    @cc_auth_code varchar (50),
    @receipt_details varchar (500),
    @cashlistitem_reverse_pmuser_id smallint,
    @cashlistitem_reverse_reason_id int,
    @cashlistitem_payment_type_id int,
    @cashlistitem_payment_status_id int,
    @date_presented datetime,
    @cheque_in_possession tinyint,
    @stop_requested_date datetime,
    @stop_printed_date datetime,
    @stop_confirmation_date datetime,
    @reason varchar (500),
    @replaces_cashlistitem_id int,
    @xml_object varchar (4000),
    @underwriting_year_id int,
	@currency_base_date datetime,
	@currency_base_xrate float,
	@account_base_date datetime,
	@account_base_xrate float,
	@system_base_date datetime,
	@system_base_xrate float,
	@exchange_rate_override_reason_id int,
	@mediatype_issuer_id int,
	@cc_name VARCHAR(50),
	@cc_customer VARCHAR(50),
	@cc_manual_auth_code VARCHAR(50),
	@cc_transaction_code VARCHAR(255),
	@Party_Bank_Id INT,
	@collection_date datetime,
	@comments VARCHAR(100),
	@bank_location Varchar(100),
	@bank_branch Varchar(50),
	@chequetype_id int,
	@Cheque_clearing_type_id int,
	@cc_bank_id int,
	@type_of_card_id int,
	@cc_trans_slip_no Varchar(50),
	@is_lead TINYINT = NULL,
	@split_total NUMERIC(19,4) = NULL,
    @tax_band_id INT = NULL,  	
    @taxamount numeric(19,4),
    @sBusinessIdentifierCode VARCHAR(50)=NULL,
    @sInternationalBankAccountNumber VARCHAR(50)=NULL,
    @insurance_ref VARCHAR(30) = null  
AS

IF (@underwriting_year_id=0)
    SELECT @underwriting_year_id=NULL


DECLARE @MediaType_Status_Id AS INT
--Check the cashlist is of type receipt
	IF EXISTS(SELECT 1 FROM CashList cl INNER JOIN CashlistType clt ON clt.CashlistType_id=cl.CashlistType_Id AND clt.CODE='R'
			  WHERE cl.CashList_Id=@cashlist_id)
	BEGIN
		-- receipt item, get the media type status
		SELECT
			@MediaType_Status_Id=MediaType_Status_Id
		FROM
		 	MediaType
		WHERE
			MediaType_Id=@mediatype_id 
	END

UPDATE CashListItem
    SET
    allocationstatus_id=@allocationstatus_id,
    mediatype_id=@mediatype_id,
    cashlist_id=@cashlist_id,
    account_id=@account_id,
    media_ref=@media_ref,
    our_ref=@our_ref,
    their_ref=@their_ref,
    amount=@amount,
    transdetail_id=@transdetail_id,
	contact_name=ISNULL(NULLIF(@contact_name,''),contact_name),
	address1=ISNULL(NULLIF(@address1,''),address1),
	address2=ISNULL(NULLIF(@address2,''),address2),
	address3=ISNULL(NULLIF(@address3,''),address3),
	address4=ISNULL(NULLIF(@address4,''),address4),
	postal_code=ISNULL(NULLIF(@postal_code,''),postal_code),
    address_country=@address_country,
     payment_name=ISNULL(NULLIF(@payment_name,''),payment_name),
    payment_account_code= ISNULL(NULLIF(@payment_account_code,''),payment_account_code),
    payment_branch_code=ISNULL(NULLIF(@payment_branch_code,''),payment_branch_code),
 
    payment_expiry_date=@payment_expiry_date,
    payment_reference1=@payment_reference1,
    payment_reference2=@payment_reference2,
    letter=@letter,
    batch_id=@batch_id,
    pmuser_id=@pmuser_id,
    transaction_date=@transaction_date,
    original_amount=@original_amount,
    amount_tendered=@amount_tendered,
    change=@change,
    cashlistitem_receipt_type_id=@cashlistitem_receipt_type_id,
    cashlistitem_receipt_status_id=@cashlistitem_receipt_status_id,
    cashlistitem_bank_id=@cashlistitem_bank_id,
    cheque_date=@cheque_date,
    cc_number=@cc_number,
    cc_expiry_date=@cc_expiry_date,
    cc_start_date=@cc_start_date,
    cc_issue=@cc_issue,
    cc_pin=@cc_pin,
    cc_auth_code=@cc_auth_code,
    receipt_details=@receipt_details,
    cashlistitem_reverse_pmuser_id=@cashlistitem_reverse_pmuser_id,
    cashlistitem_reverse_reason_id=@cashlistitem_reverse_reason_id,
    cashlistitem_payment_type_id=@cashlistitem_payment_type_id,
    cashlistitem_payment_status_id=@cashlistitem_payment_status_id,
    date_presented=@date_presented,
    cheque_in_possession=@cheque_in_possession,
    stop_requested_date=@stop_requested_date,
    stop_printed_date=@stop_printed_date,
    stop_confirmation_date=@stop_confirmation_date,
    reason=@reason,
    replaces_cashlistitem_id=@replaces_cashlistitem_id,
    xml_object=@xml_object,
    underwriting_year_id=@underwriting_year_id,
    currency_base_date=@currency_base_date,
	currency_base_xrate=@currency_base_xrate,
	account_base_date=@account_base_date,
	account_base_xrate=@account_base_xrate,
	system_base_date=@system_base_date,
	system_base_xrate=@system_base_xrate,
	exchange_rate_override_reason_id=@exchange_rate_override_reason_id,
	mediatype_issuer_id=@mediatype_issuer_id,
	cc_name=@cc_name, 
	cc_customer=@cc_customer,
	cc_manual_auth_code=@cc_manual_auth_code,
	cc_transaction_code=@cc_transaction_code,
	party_bank_id=@Party_Bank_Id,
	collection_date=@collection_date,
	comments=@comments,
	MediaType_Status_Id=@MediaType_Status_Id,	
	bank_location=@bank_location,
	bank_branch=@bank_branch,
	chequetype_id=@chequetype_id,
	Cheque_clearing_type_id=@Cheque_clearing_type_id,
	cc_bank_id=@cc_bank_id,
	type_of_card_id=@type_of_card_id,
	cc_trans_slip_no=@cc_trans_slip_no,
	split_total = @split_total,
	is_lead = @is_lead,
    TaxAmount = @taxamount,
    tax_band_id = @tax_band_id,
	business_identifier_code = @sBusinessIdentifierCode,
	international_bank_account_number = @sInternationalBankAccountNumber,
	 insurance_ref =@insurance_ref 
	
WHERE cashlistitem_id = @cashlistitem_id

GO



