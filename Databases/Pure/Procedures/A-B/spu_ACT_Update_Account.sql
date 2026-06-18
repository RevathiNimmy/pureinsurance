SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_Account'
GO

CREATE PROCEDURE spu_ACT_Update_Account
    @account_id int,
    @company_id int,
    @purgefrequency_id smallint,
    @accounttype_id smallint,
    @paymenttype_id smallint,
    @currency_id smallint,
    @ledger_id smallint,
    @account_name varchar(60),
    @short_code char(30), -- Sankar - (WPR85_Cash_Deposit_Process) - Paralleling - 20 to 30
    @restrict_enquiry bit,
    @restrict_update bit,
    @delete_at_purge bit,
    @contact_name varchar(60),
    @address1 varchar(40),
    @address2 varchar(40),
    @address3 varchar(40),
    @address4 varchar(40),
    @postal_code varchar(20),
    @address_country smallint,
    @phone_area_code varchar(10),
    @phone_number varchar(15),
    @phone_extension varchar(6),
    @fax_area_code varchar(10),
    @fax_number varchar(15),
    @fax_extension varchar(6),
    @payment_name varchar(60),
    @payment_account_code varchar(60),
    @payment_branch_code varchar(30),
    @payment_expiry_date datetime,
    @payment_reference1 varchar(30),
    @payment_reference2 varchar(30),
    @prooflist_report_id integer,
    @bordereau_report_id integer,
    @credit_limit numeric(19,4),
    @discount_percentage float,
    @settlement_period smallint,
    @bank_name varchar(60),
    @bank_address1 varchar(40),
    @bank_address2 varchar(40),
    @bank_address3 varchar(40),
    @bank_address4 varchar(40),
    @bank_postal_code varchar(20),
    @bank_country smallint,
    @bank_phone_area_code varchar(10),
    @bank_phone_number varchar(15),
    @bank_phone_extension varchar(6),
    @bank_fax_area_code varchar(10),
    @bank_fax_number varchar(15),
    @bank_fax_extension varchar(6),
    @comments varchar(255),
    @account_key int,
    @nominal_account_id int,
    @accountstatus_id smallint,
    @sub_branch_id int = NULL,
    @allow_electronic_payment smallint = 0,
    @client_money_calc_account_type smallint=0,
    @client_bank_account_type smallint,
    @merchant_id varchar(20) = Null
AS
IF ISNULL(@sub_branch_id, 0) = 0
    SELECT @sub_branch_id = sub_branch_id
    FROM   party
    WHERE  party_cnt = @account_key
    AND    source_id = @company_id
IF ISNULL(@sub_branch_id, 0) = 0
    EXEC spu_sub_branch_default @source_id=@company_id, @sub_branch_id=@sub_branch_id OUTPUT
    
if @contact_name = ''
Select @contact_name=contact_name from Account where Account_id=@account_id

UPDATE Account
    SET
    company_id=@company_id,
    purgefrequency_id=@purgefrequency_id,
    accounttype_id=@accounttype_id,
    paymenttype_id=@paymenttype_id,
    currency_id=@currency_id,
    ledger_id=@ledger_id,
    account_name=@account_name,
    short_code=@short_code,
    restrict_enquiry=@restrict_enquiry,
    restrict_update=@restrict_update,
    delete_at_purge=@delete_at_purge,
    contact_name=@contact_name,
    address1=@address1,
    address2=@address2,
    address3=@address3,
    address4=@address4,
    postal_code=@postal_code,
    address_country=@address_country,
    phone_area_code=@phone_area_code,
    phone_number=@phone_number,
    phone_extension=@phone_extension,
    fax_area_code=@fax_area_code,
    fax_number=@fax_number,
    fax_extension=@fax_extension,
    payment_name=@payment_name,
    payment_account_code=@payment_account_code,
    payment_branch_code=@payment_branch_code,
    payment_expiry_date=@payment_expiry_date,
    payment_reference1=@payment_reference1,
    payment_reference2=@payment_reference2,
    prooflist_report_id=@prooflist_report_id,
    bordereau_report_id=@bordereau_report_id,
    credit_limit=@credit_limit,
    discount_percentage=@discount_percentage,
    settlement_period=@settlement_period,
    bank_name=@bank_name,
    bank_address1=@bank_address1,
    bank_address2=@bank_address2,
    bank_address3=@bank_address3,
    bank_address4=@bank_address4,
    bank_postal_code=@bank_postal_code,
    bank_country=@bank_country,
    bank_phone_area_code=@bank_phone_area_code,
    bank_phone_number=@bank_phone_number,
    bank_phone_extension=@bank_phone_extension,
    bank_fax_area_code=@bank_fax_area_code,
    bank_fax_number=@bank_fax_number,
    bank_fax_extension=@bank_fax_extension,
    comments=@comments,
    account_key=@account_key,
    nominal_account_id=@nominal_account_id,
    accountstatus_id=@accountstatus_id,
    allow_electronic_payment = @allow_electronic_payment,
    client_money_calc_account_type=@client_money_calc_account_type,
    client_bank_account_type=@client_bank_account_type,
    merchant_id=@merchant_id,  
    sub_branch_id=@sub_branch_id
WHERE account_id = @account_id
UPDATE e
SET e.element_name = a.short_code
FROM element e
JOIN structuretree st
ON st.element_id = e.element_id
JOIN account a
ON a.account_id = st.account_id
WHERE a.account_id = @account_id
GO
