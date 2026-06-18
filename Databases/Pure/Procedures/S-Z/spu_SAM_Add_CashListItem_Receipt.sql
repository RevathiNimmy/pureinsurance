SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Add_CashListItem_Receipt'
GO

CREATE   PROCEDURE spu_SAM_Add_CashListItem_Receipt  
    @cashlistitem_id int OUTPUT,  
    @batch_id int,  
    @cashlist_id int,  
    @account_code char(30),
    @transaction_date datetime,  
    @amount numeric(19,4),  
    @media_ref varchar(100),  
    @our_ref varchar(255),  
    @their_ref varchar(30),  
    @contact_name varchar(60),  
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
    @allocationstatus_id int,  
    @mediatype_id int =NULL,  
    @address_country_id int=NULL,  
    @pmuser_id int= NULL,  
    @receipt_details varchar(500)=NULL,  
    @receipt_type_id int,  
    @receipt_status_id int,  
    @bank_code varchar(20)=NULL,  
    @cheque_date datetime=NULL,  
    @payment_status_id int=1,  
    @cc_tracking_number varchar(255)=NULL,
    @party_bank_id INT=NULL,
    @ntax_band_id INT=NULL,
	@amount_tendered numeric(19,4)=0.00,  
	@original_amount numeric(19,4)=0.00, 
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
        @return_status int,  
        @bank_id int  
  
  
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
  
    SELECT @bank_id = cashlistitem_bank_id  
    FROM   cashlistitem_bank  
    WHERE  code = @bank_code  
  
	
	DECLARE @MediaType_Status_Id AS INT
	
	SELECT
		@MediaType_Status_Id=MediaType_Status_Id
	FROM
	 	MediaType
	WHERE
		MediaType_Id=@mediatype_id 



    INSERT INTO cashlistitem (  
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
            allocationstatus_id,  
            receipt_details,  
            cashlistitem_receipt_type_id,  
            cashlistitem_receipt_status_id,  
            cashlistitem_bank_id,  
			cheque_date,  
			cashlistitem_payment_status_id,
			cc_tracking_number,
			party_bank_id,            
			MediaType_Status_id,
			tax_band_id
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
            @amount,  
            @amount_tendered,  
            @original_amount, -- No change  
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
            @allocationstatus_id,  
            @receipt_details,  
            @receipt_type_id,  
            @receipt_status_id,  
            @bank_id,  
            @cheque_date,  		
            @payment_status_id,
            @cc_tracking_number,
            @party_bank_id,
	    @MediaType_Status_id,
	    @ntax_band_id 
)  

  
SET @cashlistitem_id =@@IDENTITY  
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO 
