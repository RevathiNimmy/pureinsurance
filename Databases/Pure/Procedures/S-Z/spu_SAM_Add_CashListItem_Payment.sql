 
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Add_CashListItem_Payment'
GO

CREATE  PROCEDURE spu_SAM_Add_CashListItem_Payment  
	@cashlistitem_id int OUTPUT,  
	@batch_id int,  
	@cashlist_id int,  
	@account_code char(30),  
	@transaction_date datetime,  
	@amount numeric(19,4),  
	@media_ref varchar(100),  
	@our_ref varchar(255),  
	@their_ref varchar(30),  
	@contact_name varchar(255),  
	@address1 varchar(40),  
	@address2 varchar(40),  
	@address3 varchar(40),  
	@address4 varchar(40),  
	@postal_code varchar(20),  
	@payment_name varchar(255),  
	@letter tinyint,  
	@cc_name varchar(50),  
	@cc_number varchar(30),  
	@cc_expiry_date varchar(10),  
	@cc_start_date varchar(10),  
	@cc_issue varchar(2),  
	@cc_pin varchar(20),  
	@cc_auth_code varchar(50),  
	@cc_manual_auth_code varchar(50),  
	@cc_transaction_code varchar(255),  
	@cc_customer varchar(50),  
	@payment_type_id int,  
	@payment_status_id int,  
	@allocationstatus_id int,  
	@mediatype_id int =NULL,  
	@address_country_id int=NULL,  
	@pmuser_id int= NULL,  
	@receipt_details varchar(500)=NULL,  
	@payment_account_code varchar(60)=NULL,  
	@payment_branch_code varchar(30)=NULL,  
	@payment_expiry_date datetime =NULL,  
	@payment_reference1 varchar(30)=NULL,  
	@payment_reference2 varchar(30)=NULL,  
	@cc_tracking_number varchar(255)=NULL,  
	@party_bank_id INT=NULL,  
	@ntax_band_id INT=NULL,  
	@dcheque_date  datetime=NULL,
	@nReceipt_type_id INT=null,
	@nReceipt_status_id INT=null,
	@sReceipt_type_Code varchar(21)=null,
	@sBusinessIdentifierCode VARCHAR(50)=NULL,
	@sInternationalBankAccountNumber VARCHAR(50)=NULL,
	@amount_tendered numeric(19,4)=0.00,  
	@original_amount numeric(19,4)=0.00,
	@collection_date datetime =NULL, 
    @currency_base_date datetime = NULL,  
    @currency_base_xrate float = NULL,  
    @account_base_date datetime = NULL,  
    @account_base_xrate float = NULL,  
    @system_base_date datetime = NULL,  
    @system_base_xrate float = NULL,  
    @exchange_rate_override_reason_id int = NULL
	
AS  
BEGIN  
    DECLARE  
        @company_id int,  
        @currency_id int,  
        @account_id int,  
        @underwriting_year_id int,   
        @return_status int  
  
  
    SELECT  @company_id = company_id,  
            @currency_id = currency_id  
    FROM    cashlist  
    WHERE   cashlist_id = @cashlist_id  
  
  
    SELECT  @account_id = account_id  
    FROM    account  
    WHERE   short_code = @account_code  
  
  
    SELECT  @underwriting_year_id = underwriting_year_id  
    FROM    underwriting_year  
    WHERE   @transaction_date BETWEEN start_date AND end_date  
        AND is_deleted = 0  
        AND effective_date <= @transaction_date  
    SET @currency_base_date = COALESCE(@currency_base_date, @transaction_date)
  
  if (@nReceipt_type_id is null and @sReceipt_type_Code is not null)
		set @nReceipt_type_id=(select cashlistitem_receipt_type_id from CashListItem_Receipt_Type where code=@sReceipt_type_Code)

  IF  ISNULL(@cashlistitem_id,0)<>0  
   BEGIN  
  UPDATE Cashlistitem  
   SET mediatype_id=@cashlistitem_id,  
	cashlist_id=@cashlist_id,  
	account_id=@account_id,  
	media_ref=@media_ref,  
	our_ref=@our_ref,  
	their_ref=@our_ref,  
	amount=@amount,  
	contact_name=@contact_name,  
	address1=@address1,  
	address2=@address2,  
	address3=@address3,  
	address4=@address4,  
	postal_code=@postal_code,  
	address_country=@address_country_id,  
	payment_name=@payment_name,  
	letter=@letter,  
	batch_id=@batch_id,  
	pmuser_id=@pmuser_id,  
	transaction_date=@transaction_date,  
	original_amount=@amount,  
	amount_tendered=@amount,  
	change=0,  
	cc_number=@cc_number,  
	cc_expiry_date=@cc_expiry_date,  
	cc_start_date=@cc_start_date,  
	cc_issue=@cc_issue,  
	cc_pin=@cc_pin,  
	cc_auth_code=@cc_auth_code,  
	underwriting_year_id=@underwriting_year_id,  
	cc_name=@cc_name,  
	cc_customer=@cc_customer,  
	cc_manual_auth_code=@cc_manual_auth_code,  
	cc_transaction_code=@cc_transaction_code,  
	currency_base_xrate=@currency_base_xrate,  
	currency_base_date=@currency_base_date,  
	account_base_xrate=@account_base_xrate,  
	account_base_date=@account_base_date,  
	system_base_xrate=@system_base_xrate,  
	system_base_date=@system_base_date,  
	allocationstatus_id=@allocationstatus_id,  
	receipt_details=@receipt_details,  
	payment_account_code=@payment_account_code,  
	payment_branch_code=@payment_branch_code,  
	payment_expiry_date=@payment_expiry_date,  
	payment_reference1=@payment_reference1,  
	payment_reference2=@payment_reference2,  
	cc_tracking_number=@cc_tracking_number,  
	party_bank_id=@party_bank_id,  
	tax_band_id=@ntax_band_id,  
	cheque_date=@dcheque_date,
	cashlistitem_receipt_type_id=@nReceipt_type_id,
	cashlistitem_receipt_status_id=@nReceipt_status_id,
	business_identifier_code=@sBusinessIdentifierCode,
	international_bank_account_number=@sInternationalBankAccountNumber  
    
   WHERE cashlistitem_id=@cashlistitem_id  
			
   SET @cashlistitem_id = @cashlistitem_id  
   END  
  ELSE  
   BEGIN  
  INSERT INTO Cashlistitem (  
	mediatype_id,  
	cashlist_id,  
	account_id,  
	media_ref,  
	our_ref,  
	their_ref,  
	amount,  
	contact_name,  
	address1,  
	address2,  
	address3,  
	address4,  
	postal_code,  
	address_country,  
	payment_name,  
	letter,  
	batch_id,  
	pmuser_id,  
	transaction_date,  
	collection_date,
	original_amount,  
	amount_tendered,  
	change,  
	cc_number,  
	cc_expiry_date,  
	cc_start_date,  
	cc_issue,  
	cc_pin,  
	cc_auth_code,  
	underwriting_year_id,  
	cc_name,  
	cc_customer,  
	cc_manual_auth_code,  
	cc_transaction_code,  
	currency_base_xrate,  
	currency_base_date,  
	account_base_xrate,  
	account_base_date,  
	system_base_xrate,  
	system_base_date,  
	cashlistitem_payment_type_id,  
	cashlistitem_payment_status_id,  
	allocationstatus_id,  
	receipt_details,  
	payment_account_code,  
	payment_branch_code,  
	payment_expiry_date,  
	payment_reference1,  
	payment_reference2,  
	cc_tracking_number,  
	party_bank_id,  
	tax_band_id,  
	cheque_date, 
	cashlistitem_receipt_type_id, 
	cashlistitem_receipt_status_id,
	business_identifier_code,
	international_bank_account_number  
     )  
  VALUES (  
	@mediatype_id,  
	@cashlist_id,  
	@account_id,  
	@media_ref,  
	@our_ref,  
	@their_ref,  
	@amount,  
	@contact_name,  
	@address1,  
	@address2,  
	@address3,  
	@address4,  
	@postal_code,  
	@address_country_id,  
	@payment_name,  
	@letter,  
	@batch_id,  
	@pmuser_id,  
	@transaction_date,  
	@collection_date,  
	@original_amount ,  
	@amount_tendered,  
	0,
	@cc_number,  
	@cc_expiry_date,  
	@cc_start_date,  
	@cc_issue,  
	@cc_pin,  
	@cc_auth_code,  
	@underwriting_year_id,  
	@cc_name,  
	@cc_customer,  
	@cc_manual_auth_code,  
	@cc_transaction_code,  
	@currency_base_xrate,  
	@currency_base_date,  
	@account_base_xrate,  
	@account_base_date,  
	@system_base_xrate,  
	@system_base_date,  
	@payment_type_id,  
	@payment_status_id,  
	@allocationstatus_id,  
	@receipt_details,  
	@payment_account_code,  
	@payment_branch_code,  
	@payment_expiry_date,  
	@payment_reference1,  
	@payment_reference2,  
	@cc_tracking_number,  
	@party_bank_id,  
	@ntax_band_id,  
	@dcheque_date,
	@nReceipt_type_id,
	@nReceipt_status_id,
	@sBusinessIdentifierCode,
	@sInternationalBankAccountNumber  
)  
	SET @cashlistitem_id = @@IDENTITY  
  END  
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
