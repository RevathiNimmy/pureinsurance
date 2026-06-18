SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_ACT_Import_Payment_CreateCashListItem'
GO


CREATE PROCEDURE spu_ACT_Import_Payment_CreateCashListItem
    @cashlistitem_id int output,
    @mediatype_code char(10),
    @cashlist_id int,
    @account_code char(20),
    @media_ref varchar(100),
    @amount numeric(19,4),
    @contact_name varchar(60),
    @address1 varchar(40),
    @address2 varchar(40),
    @address3 varchar(40),
    @address4 varchar(40),
    @postal_code varchar(20),
    @country_code char(10),
    @payment_name varchar(60),
    @payment_account_code varchar(60),
    @payment_branch_code varchar(30),
    @payment_expiry_date datetime,
    @payment_reference1 varchar(30),
    @payment_reference2 varchar(30),
    @batch_id int,
    @username varchar(255),
    @transaction_date datetime,
    @cheque_date datetime,
    @cc_number varchar(30),
    @cc_expiry_date varchar(10),
    @cc_start_date varchar(10),
    @cc_issue varchar(2),
    @cc_pin varchar(20),
    @cc_auth_code varchar(50),
    @payment_type_code varchar(20),
    @mediatype_issuer_code char(10),
    @cc_name varchar(50),
    @cc_customer varchar(50),
    @cc_manual_auth_code varchar(50),
    @cc_transaction_code varchar(255),
    @sBusinessIdentifierCode VARCHAR(50)=NULL,
    @sInternationalBankAccountNumber VARCHAR(50)=NULL
AS

    Declare
        @company_id int,
        @currency_id int,
        @mediatype_id int,
        @account_id int,
        @address_country smallint,
        @pmuser_id int,
        @payment_type_id int,
        @mediatype_issuer_id int,
        @underwriting_year_id int,
    	@currency_base_date datetime,
    	@currency_base_xrate float,
    	@account_base_date datetime,
    	@account_base_xrate float,
    	@system_base_date datetime,
    	@system_base_xrate float,
        @return_status int

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
    Select  @payment_type_id = cashlistitem_payment_type_id
    From    cashlistitem_payment_type
    Where   code = @payment_type_code

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
    
    -- Add DorC sign
    Select @amount = ABS(@amount) * -1

    -- Get currency information
    Exec spu_ACT_Do_Currency_Conversion
    	@account_id = @account_id,
    	@company_id = @company_id,
    	@currency_id = @currency_id,
    	@currency_amount_unrounded = @amount,
    	@mode = 'ALL',
    	@currency_base_xrate = @currency_base_xrate output,
    	@currency_base_date = @currency_base_date output,
    	@account_base_xrate = @account_base_xrate output,
    	@account_base_date = @account_base_date output,
    	@system_base_xrate = @system_base_xrate output,
    	@system_base_date = @system_base_date output,
    	@return_status = @return_status output

    -- Raise an error is we couldn't convert currency
    If @return_status <> 1
        Raiserror ('Currency conversion failed.', 16, 1)

     -- Get new id
    --Select  @cashlistitem_id = IsNull(Max(cashlistitem_id), 0) + 1
    --From    cashlistitem

    -- Insert cashlist item
    Insert Into cashlistitem (
	    allocationstatus_id,
            mediatype_id,
            cashlist_id,
            account_id,
            media_ref,
            amount,
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
            cashlistitem_payment_type_id,
            cashlistitem_payment_status_id,
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
			business_identifier_code,
			international_bank_account_number)
    Values (1, -- Unallocated
            @mediatype_id,
            @cashlist_id,
            @account_id,
            @media_ref,
            @amount,
            @contact_name,
            @address1,
            @address2,
            @address3,
            @address4,
            @postal_code,
            @address_country,
            @payment_name,
            @payment_account_code,
            @payment_branch_code,
            @payment_expiry_date,
            @payment_reference1,
            @payment_reference2,
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
            @payment_type_id,
            1, -- Issued
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
			@sBusinessIdentifierCode,
			@sInternationalBankAccountNumber)
	SELECT @cashlistitem_id = @@IDENTITY
Go


