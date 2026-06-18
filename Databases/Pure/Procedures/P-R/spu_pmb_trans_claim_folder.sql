SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmb_trans_claim_folder'
GO


CREATE PROCEDURE spu_pmb_trans_claim_folder
    @transaction_export_folder_cnt int OUTPUT,
    @user_id smallint,
    @user_name varchar(255),
    @transaction_party_source_id smallint,
    @transaction_party_id int,
    @effective_date datetime,
    @transaction_currency_id int,
    @claim_cnt int,
    @claim_ref varchar(30),
    @claim_amount numeric(19,4),
    @document_ref varchar(25)
AS

DECLARE     
    @product_id int,
    @transaction_export_folder_id int,
    @source_id smallint,
    @document_comment varchar (60),
    @document_date datetime,
    @accounting_date datetime,
    @transaction_type_id tinyint,
    @transaction_type_code varchar (10),
    @transaction_party_shortname varchar (20),
    @transaction_party_key int,
    @transaction_party_cnt int,
    @product_code varchar (10),
    @business_type_id smallint,
    @business_type_code varchar (10),
    @branch_id smallint,
    @branch_code varchar (10),
    @currency_code char(10),
    @created_by_user_id smallint,
    @created_by_username varchar (12),
    @accounts_export_status char(1),
    @Key_Suffix_Int int,
    @payment_receipt char(1),
    @sub_branch_id int,
    @period_start_date datetime

-- Set temporary default values
SELECT  
    @accounts_export_status = "p",
    @product_id = 1,
    @business_type_id = 1,
    @branch_id = 1
    
-- Get Transaction Party File Details
Select @source_id = P.source_id,
       @transaction_party_cnt = P.party_cnt,
       @transaction_party_key = P.party_cnt,
       @transaction_party_shortname = P.shortname
FROM   Party P
WHERE  P.party_id = @transaction_party_id
AND    P.source_id = @transaction_party_source_id

/* Set new folder_id */
SELECT @transaction_export_folder_id = MAX(transaction_export_folder_id) + 1
FROM   Transaction_Export_Folder
WHERE  source_id = @source_id

IF @transaction_export_folder_id IS NULL
    SELECT @transaction_export_folder_id = 1

SELECT @document_date = GetDate()

-- PWF - Sort out the period
-- Get sub branch id
SELECT @sub_branch_id = sub_branch_id
FROM   Document
WHERE  document_ref = @document_ref

-- Get previous period end date
EXEC spu_Report_GetPeriodEndComplete_Date
    @sub_branch_id,
    @period_start_date OUTPUT

-- Add a day
SELECT @period_start_date = DATEADD(DAY, 1, @period_start_date)
-- PWF - Sort out the period


IF @effective_date >= @period_start_date
    SELECT @accounting_date = @effective_date
ELSE
    SELECT @accounting_date = @period_start_date


/* Get codes from id */
SELECT  @product_code = code
FROM    Product
WHERE   product_id = @product_id

SELECT  @business_type_code = code
FROM    Business_Type
WHERE   business_type_id = @business_type_id

SELECT  @branch_code = code
FROM    Branch
WHERE   branch_id = @branch_id

SELECT  @currency_code = iso_code
FROM    Currency
WHERE   currency_id = @transaction_currency_id

/* Get Transaction_type details for Fee */
/*NOTE this don't work unless claims psoting types are added to the posting type table */
SELECT @transaction_type_id = P.posting_type_id,
       @transaction_type_code = P.code
FROM   Posting_Type    P
WHERE  P.description = 'Claim'

SELECT @Payment_receipt = substring(@document_ref,3,1)
If @payment_receipt = 'P'
    SELECT  @document_comment = 'Claim Payment'
If @payment_receipt = 'R'
    SELECT  @document_comment = 'Claim Receipt'
/* Set user details */

SELECT @created_by_user_id = @user_id,
       @created_by_username = @user_name
/* Insert the Trans Export Folder */

INSERT INTO Transaction_Export_Folder (
    product_id,
    transaction_export_folder_id,
    source_id,
    insurance_file_cnt,
    debit_credit,
    document_ref,
    document_comment,
    document_date,
    is_payable_by_instalments,
    accounting_date,
    premium_total,
    transaction_type_id,
    transaction_type_code,
    insurance_ref,
    effective_date,
    cover_start_date,
    expiry_date,
    insurance_holder_cnt,
    insurance_holder_id,
    insurance_holder_shortname,
    insurance_holder_account_key,
    product_code,
    business_type_id,
    business_type_code,
    account_handler_cnt,
    account_handler_id,
    account_handler_shortname,
    account_handler_account_key,
    agent_cnt,
    agent_id,
    agent_shortname,
    agent_account_key,
    branch_id,
    branch_code,
    currency_code,
    loss_id,
    loss_code,
    loss_date,
    created_by_user_id,
    created_by_username,
    accounts_export_status,
    posting_period_year,
    posting_period_number)
VALUES (
    @product_id,
    @transaction_export_folder_id,
    @source_id,
    @claim_cnt,
    @Payment_receipt,
    @document_ref,
    @document_comment,
    @document_date,
    0,
    @accounting_date,
    @claim_amount,
    @transaction_type_id,
    @transaction_type_code,
    @claim_ref,
    @effective_date,
    NULL,
    NULL,
    @transaction_party_cnt,
    @transaction_party_id,
    @transaction_party_shortname,
    @transaction_party_key,
    @product_code,
    @business_type_id,
    @business_type_code,
    0,
    NULL,
    NULL,
    0,
    0,
    NULL,
    NULL,
    0,
    @branch_id,
    @branch_code,
    @currency_code,
    NULL,
    NULL,
    NULL,
    @created_by_user_id,
    @created_by_username,
    @accounts_export_status,
    NULL,
    NULL)


/* Return the Count of the Record Added */
SELECT @transaction_export_folder_cnt = @@IDENTITY


GO


