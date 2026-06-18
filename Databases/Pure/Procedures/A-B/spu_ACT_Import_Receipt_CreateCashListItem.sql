SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_ACT_Import_Receipt_CreateCashListItem'
GO


CREATE PROCEDURE spu_ACT_Import_Receipt_CreateCashListItem  
    @cashlistitem_id int output,  
    @account_id int output,  
    @batch_id int,  
    @cashlist_id int,  
    @receipt_type_code varchar(20),  
    @mediatype_code char(10),  
    @mediatype_issuer_code char(10),  
    @account_code char(20),  
    @transaction_date datetime,  
    @amount numeric(19,4),  
    @media_ref varchar(100),  
    @our_ref varchar(255),  
    @their_ref varchar(30),  
    @contact_name varchar(60),  
    @address1 varchar(60),  
    @address2 varchar(60),  
    @address3 varchar(60),  
    @address4 varchar(60),  
    @postal_code varchar(20),  
    @country_code char(10),  
    @payment_name varchar(60),  
    @cheque_date datetime,  
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
    @username varchar(255),  
    @collection_date datetime = Null,  
    @comments VARCHAR(100) = Null,  
    @party_bank_id INT=NULL,  
    @cc_tracking_number VARCHAR(255) =NULL,  
    @bank_code varchar(20)=NULL,  
    @bank_location Varchar(100)=NULL,  
    @bank_branch Varchar(50)=NULL,  
    @chequetype_id int=NULL,  
    @Cheque_clearing_type_id int=NULL,  
    @cc_bank_id int=NULL,  
    @type_of_card_id int=NULL,  
    @cc_trans_slip_no Varchar(50)=NULL,  
    @sBusinessIdentifierCode VARCHAR(50)=NULL,  
    @sInternationalBankAccountNumber VARCHAR(50)=NULL,  
 @cc_token_id As Varchar(50)=NULL,  
 @cc_insurance_file_cnt As Int=NULL, 
    @currency_base_date datetime = NULL,  
    @currency_base_xrate float = NULL,  
    @account_base_date datetime = NULL,  
    @account_base_xrate float = NULL,  
    @system_base_date datetime = NULL,  
    @system_base_xrate float = NULL,  
    @exchange_rate_override_reason_id int = NULL
AS  
  
    Declare  
        @company_id int,  
        @currency_id int,  
        @mediatype_id int,  
        @address_country smallint,  
        @pmuser_id int,  
        @receipt_type_id int,  
        @mediatype_issuer_id int,  
        @underwriting_year_id int, 
        @return_status int,  
  @bank_id int  
  
    -- Get header info  
    Select  @company_id = company_id,  
            @currency_id = currency_id  
    From    cashlist  
    Where   cashlist_id = @cashlist_id  
  
    -- Get media type  
    Select  @mediatype_id = mediatype_id  
    From    mediatype  
    Where   code = @mediatype_code  
  
    -- Get account id  
    Select  @account_id = account_id  
    From    account  
    Where   short_code = @account_code  
  
    -- Get country id  
    Select  @address_country = country_id  
    From    country  
    Where   code = @country_code  
  
    -- Get user id  
    Select  @pmuser_id = user_id  
    From    pmuser  
    Where   username = @username  
  
    -- Get payment type  
    Select  @receipt_type_id = cashlistitem_receipt_type_id  
    From    cashlistitem_receipt_type  
    Where   code = @receipt_type_code  
  
    -- Get mediatype issuer  
    Select  @mediatype_issuer_id = mediatype_issuer_id  
    From    mediatype_issuer  
    Where   code = @mediatype_issuer_code  
  
    -- Get underwriting year  
    Select  @underwriting_year_id = underwriting_year_id  
    From    underwriting_year  
    Where   @transaction_date Between start_date And end_date  
    And     is_deleted = 0  
    And     effective_date <= @transaction_date  
  
    -- Get currency information  
    --Exec spu_ACT_Do_Currency_Conversion  
    -- @account_id = @account_id,  
    -- @company_id = @company_id,  
    -- @currency_id = @currency_id,  
    -- @currency_amount_unrounded = @amount,  
    -- @mode = 'ALL',  
    -- @currency_base_xrate = @currency_base_xrate output,  
    -- @currency_base_date = @currency_base_date output,  
    -- @account_base_xrate = @account_base_xrate output,  
    -- @account_base_date = @account_base_date output,  
    -- @system_base_xrate = @system_base_xrate output,  
    -- @system_base_date = @system_base_date output,  
    -- @return_status = @return_status output  
  
    ---- Raise an error is we couldn't convert currency  
    --If @return_status <> 1  
    --    Raiserror ('Currency conversion failed.', 16, 1)  
  
 DECLARE @MediaType_Status_Id AS INT  
  
 SELECT  
  @MediaType_Status_Id=MediaType_Status_Id  
 FROM  
   MediaType  
 WHERE  
  MediaType_Id=@mediatype_id  
  
    SELECT @bank_id = cashlistitem_bank_id  
    FROM   cashlistitem_bank  
    WHERE  code = @bank_code  
  
    Insert Into cashlistitem (  
            allocationstatus_id,  
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
            original_amount,  
            amount_tendered,  
            change,  
            cheque_date,  
            cc_number,  
            cc_expiry_date,  
            cc_start_date,  
            cc_issue,  
            cc_pin,  
            cc_auth_code,  
            cashlistitem_receipt_type_id,  
            cashlistitem_receipt_status_id,  
            underwriting_year_id,  
            mediatype_issuer_id,  
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
            collection_date,  
            comments,  
            party_bank_id,  
            cc_tracking_number,  
            MediaType_Status_Id,  
            cashlistitem_bank_id,  
            bank_location,  
            bank_branch,  
            chequetype_id,  
            Cheque_clearing_type_id,  
            cc_bank_id,  
            type_of_card_id,  
            cc_trans_slip_no,  
            business_identifier_code,  
            international_bank_account_number,  
     cc_token_id,  
     cc_insurance_file_cnt
	 )  
    Values (  
            1, -- Unallocated  
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
            @address_country,  
            @payment_name,  
            0, -- Not printed  
            @batch_id,  
            @pmuser_id,  
            @transaction_date,  
            @amount,  
            @amount,  
            0, -- No change  
            @cheque_date,  
            @cc_number,  
            @cc_expiry_date,  
            @cc_start_date,  
            @cc_issue,  
            @cc_pin,  
            @cc_auth_code,  
            @receipt_type_id,  
            1, -- Added  
            @underwriting_year_id,  
            @mediatype_issuer_id,  
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
            @collection_date,  
            @comments,  
            @party_bank_id,  
            @cc_tracking_number,  
            @MediaType_Status_Id ,  
            @bank_id,  
            @bank_location,  
            @bank_branch,  
            @chequetype_id,  
            @Cheque_clearing_type_id,  
            @cc_bank_id,  
            @type_of_card_id,  
            @cc_trans_slip_no,  
            @sBusinessIdentifierCode,  
            @sInternationalBankAccountNumber,  
   @cc_token_id,  
   @cc_insurance_file_cnt)  
 SELECT @cashlistitem_id = @@IDENTITY   

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 

