SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

--ECK190802 Set Document date =   effective date
EXECUTE DDLDropProcedure 'spu_pmb_trans_fee_folder'
GO


CREATE PROCEDURE spu_pmb_trans_fee_folder
    @transaction_export_folder_cnt int OUTPUT,
    @user_id smallint,
    @user_name varchar(255),
    @transaction_party_source_id smallint,
    @transaction_party_id int,
    @effective_date datetime,
    @transaction_currency_id int,
    @fee_amount numeric(19,4),
    @document_ref varchar(25),
    @reason varchar(255)
AS

/*eck060601 added reason field*/
BEGIN
DECLARE @product_id             int,
    @transaction_export_folder_id   int,
    @source_id          smallint,
    @document_comment       varchar (60),
    @document_date          datetime,
    @accounting_date        datetime,
    @transaction_type_id        tinyint,
    @transaction_type_code      varchar (10),
    @transaction_party_shortname    varchar (20),
    @transaction_party_key      int,
    @transaction_party_cnt      int,
    @product_code           varchar (10),
    @business_type_id       smallint,
    @business_type_code         varchar (10),
    @fee_id             int,
    @fee_cnt            int,
    @fee_shortname          varchar (20),
    @branch_id          smallint,
    @branch_code            varchar (10),
    @currency_code          char(10),
    @created_by_user_id         smallint,
    @created_by_username        varchar (12),
    @accounts_export_status     char(1),
    @Key_Suffix_Int         int
/* Set temporary default values */

Declare @MultiBranch int
select @MultiBranch=0
IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number=16)
BEGIN
	select @MultiBranch=1
END

SELECT  @accounts_export_status = "p"
SELECT  @product_id = 1
SELECT  @business_type_id = 1
SELECT  @branch_id = 1

/* Get Transaction Party File Details */
SELECT
    @source_id = P.source_id,
    @transaction_party_cnt = P.party_cnt,
    @transaction_party_key = P.party_cnt,
    @transaction_party_shortname = P.shortname
FROM    Party       P
WHERE   P.party_id = @transaction_party_id
AND P.source_id = @transaction_party_source_id

/* Set new folder_id */
SELECT  @transaction_export_folder_id = 0

--ECK190802 Set Document date =   effective date
--SELECT @document_date = GetDate()
SELECT	@document_date = @effective_date
--

DECLARE @period_start_date  datetime,
        @period_end_date    datetime,
        @previous_period_id int
      
--Allow for periods set up just for the system and set up for each company.
IF (SELECT COUNT(DISTINCT company_id) FROM period) = 1
BEGIN
	--Only one set of periods set up under company 1
	--Get current period_end_date
	SELECT  @period_end_date = period_end_date
	FROM    Period P
	JOIN	Ledger L
	ON	L.current_period_id = P.period_id
	WHERE   L.ledger_short_name = 'SA'
	AND	P.company_id = 1


	--Get previous period_id by end_date
	SELECT  @previous_period_id = P.period_id
	FROM    Period P
	WHERE   P.period_end_date =
	(
		SELECT  MAX(P.period_end_date)
		FROM    Period P
		WHERE   P.period_end_date < @period_end_date
		AND	P.company_id = 1
	)
	AND	P.company_id = 1
END
ELSE
BEGIN
	--Multiple sets of periods set up for each company.
	--Use the @source_id to find which set to use.
	--Get current period_end_date
	SELECT  @period_end_date = period_end_date
	FROM    Period P
	JOIN	Ledger L
	ON	L.current_period_id = P.period_id
	WHERE   L.ledger_short_name = 'SA'
	AND     P.company_id = @source_id

	--Get previous period_id by end_date
	SELECT  @previous_period_id = P.period_id
	FROM    Period P
	WHERE   P.period_end_date =
	(
		SELECT  MAX(P.period_end_date)
		FROM    Period P
		WHERE   P.period_end_date < @period_end_date
		AND	P.company_id = @source_id
	)
	AND	P.company_id = @source_id
END

-- Get 1st day of current period
SELECT  @period_start_date = DATEADD(day, 1, period_end_date)
FROM    Period P
WHERE   P.period_id = @previous_period_id

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
SELECT  @transaction_type_id = P.posting_type_id,
    @transaction_type_code = P.code
FROM    Posting_Type    P
WHERE   P.description = 'Fee'
SELECT  @document_comment = 'Client Fee'

/* Set user details */

SELECT  @created_by_user_id = @user_id,
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

    posting_period_number,
    reason)
VALUES (

    @product_id,

    @transaction_export_folder_id,
    @source_id,
    0,
    "D",
    @document_ref,
    @document_comment,
    @document_date,
    0,
    @accounting_date,
    @fee_amount,
    @transaction_type_id,
    @transaction_type_code,
    NULL,

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

    NULL,
    @reason)
END
BEGIN
/* Return the Count of the Record Added */
SELECT @transaction_export_folder_cnt = @@IDENTITY
END
GO


