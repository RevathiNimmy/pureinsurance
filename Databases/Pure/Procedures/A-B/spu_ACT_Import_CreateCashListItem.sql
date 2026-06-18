Execute DDLDropProcedure 'spu_ACT_Import_CreateCashListItem'
GO

CREATE PROCEDURE spu_ACT_Import_CreateCashListItem  
    @cashlistitem_id  INT OUTPUT,  
    @account_id  INT OUTPUT,  
    @batch_id  INT,  
    @cashlist_id  INT,  
    @cash_type_code VARCHAR(20),  
    @mediatype_code CHAR(10),  
    @account_code CHAR(20),  
    @transaction_date DATETIME,  
    @amount NUMERIC(19,4),  
    @media_ref VARCHAR(30),  
    @our_ref VARCHAR(30),  
    @their_ref VARCHAR(30),  
    @contact_name VARCHAR(255),  
    @address1 VARCHAR(60),  
    @address2 VARCHAR(60),  
    @address3 VARCHAR(60),  
    @address4 VARCHAR(60),  
    @postal_code VARCHAR(20),  
    @country_code CHAR(10),  
    @payment_name VARCHAR(60),  
    @cheque_date DATETIME,  
    @cc_name VARCHAR(50),  
    @cc_number VARCHAR(30),  
    @cc_expiry_date VARCHAR(10),  
    @cc_start_date VARCHAR(10),  
    @cc_issue VARCHAR(2),  
    @cc_pin VARCHAR(20),  
    @cc_auth_code VARCHAR(50),  
    @cc_manual_auth_code VARCHAR(50),  
    @cc_transaction_code VARCHAR(255),  
    @cc_customer VARCHAR(50),  
    @username VARCHAR(255),  
    @collection_date DATETIME = Null,  
    @comments VARCHAR(100) = Null,  
    @party_bank_id INT=NULL,  
    @cc_tracking_number VARCHAR(255) =NULL, 
	@bank_code VARCHAR(20)=NULL, 
	@bank_location Varchar(100)=NULL,  
	@bank_branch Varchar(50)=NULL,  
	@chequetype_id  INT=NULL,  
	@Cheque_clearing_type_id  INT=NULL,  
	@cc_bank_id  INT=NULL,  
	@type_of_card_id  INT=NULL,
	@sBusinessIdentifierCode VARCHAR(50)=NULL,
	@sInternationalBankAccountNumber VARCHAR(50)=NULL,
	@cc_token_id As Varchar(50)=NULL,
	@cc_insurance_file_cnt As Int=NULL
AS  
  
    Declare  
        @company_id  INT,  
        @currency_id  INT,  
        @mediatype_id  INT,  
        @address_country smallint,  
        @pmuser_id  INT,  
        @cash_type_id  INT,  
        @underwriting_year_id  INT,  
     @currency_base_date DATETIME,  
     @currency_base_xrate float,  
     @account_base_date DATETIME,  
     @account_base_xrate float,  
     @system_base_date DATETIME,  
     @system_base_xrate float,  
        @return_status  INT,
		@bank_id  INT    
  
    -- Get header info  
    SELECT  @company_id = company_id,  
            @currency_id = currency_id  
    FROM    cashlist  
    WHERE   cashlist_id = @cashlist_id  
    
    -- Get media type  
    SELECT  @mediatype_id = mediatype_id  
    FROM    mediatype  
    WHERE   code = @mediatype_code  
  
    -- Get account id  
    SELECT  @account_id = account_id  
    FROM    account  
    WHERE   short_code = @account_code  
  
    -- Get country id  
    SELECT  @address_country = country_id  
    FROM    country  
    WHERE   code = @country_code  
  
    -- Get user id  
    SELECT  @pmuser_id = user_id  
    FROM    pmuser  
    WHERE   username = @username  
  
    -- Get cash type  
    IF @amount > 0
		SELECT  @cash_type_id = cashlistitem_receipt_type_id  
		FROM    cashlistitem_receipt_type  
		WHERE   code = @cash_type_code  
	ELSE
		SELECT  @cash_type_id = cashlistitem_payment_type_id
		FROM    CashListItem_Payment_Type
		WHERE   code = @cash_type_code  
			
    -- Raise an error is we couldn't convert currency  
    IF @cash_type_id = 0
        RAISERROR ('Invalid cashlist type.', 16, 1) 	
			
    -- Get underwriting year  
    SELECT  @underwriting_year_id = underwriting_year_id  
    FROM    underwriting_year  
    WHERE   @transaction_date BETWEEN start_date AND end_date  
    AND     is_deleted = 0  
    AND     effective_date <= @transaction_date  
  
    -- Get currency information  
    EXEC spu_ACT_Do_Currency_Conversion  
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
    IF @return_status <> 1  
        Raiserror ('Currency conversion failed.', 16, 1)  
  
	DECLARE @MediaType_Status_Id AS INT

	-- get the media type status for this receipt 
	SELECT
		@MediaType_Status_Id=MediaType_Status_Id
	FROM
	 	MediaType
	WHERE
		MediaType_Id=@mediatype_id 
		
  --Get bank id  
    SELECT @bank_id = cashlistitem_bank_id  
    FROM   cashlistitem_bank  
    WHERE  code = @bank_code 
  
    -- Insert cashlist item  
    INSERT INTO cashlistitem (  
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
		   business_identifier_code,
		   international_bank_account_number,
		   cc_token_id,
		   cc_insurance_file_cnt) 

    VALUES (  
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
            @cash_type_id,  
            1, -- Added  
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

