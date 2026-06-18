SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_pmb_trans_folder_add'
GO

CREATE PROCEDURE spu_pmb_trans_folder_add
    @transaction_export_folder_cnt  int OUTPUT,
    @insurance_file_cnt         int,
    @user_id            smallint,
    @user_name          varchar (12),
    @insurance_holder_account_key   int,
    @account_handler_account_key    int,
    @agent_account_key      int,
    @document_ref           varchar(25),
    @debit_credit           char(1),
    @document_date          datetime,
    @reason         varchar(255),
    @terms_of_payment_id int = Null,
    @payment_due_date datetime = Null,
    @event_log_id int = NULL
AS
BEGIN
DECLARE @product_id             int,
    @transaction_export_folder_id   int,
    @source_id          smallint,
--  @debit_credit           char (1),
    @document_comment       varchar (60),
--  @document_date          datetime,
    @is_payable_by_instalments  tinyint,
    @accounting_date        datetime,
    @premium_total          numeric(19, 4),
    @transaction_type_id        tinyint,
    @transaction_type_code      varchar (10),
    @insurance_ref          varchar (30),
    @effective_date         datetime,
    @cover_start_date       datetime,
    @expiry_date            datetime,
    @insurance_holder_cnt       int,
    @insurance_holder_id        int,
    @product_code           varchar (10),
    @business_type_id       smallint,
    @business_type_code         varchar (10),
    @account_handler_cnt        int,
    @account_handler_id         int,
    @agent_cnt          int,
    @agent_id           int,
    @branch_id          smallint,
    @branch_code            varchar (10),
    @currency_code          char(10),
    @loss_id            int,
    @loss_code          varchar (10),
    @loss_date          datetime,   @created_by_user_id         smallint,
    @created_by_username        varchar (12),
    @accounts_export_status     char(1),
    @posting_period_year        datetime,
    @posting_period_number      smallint,
    @insurance_holder_shortname varchar (20),   @account_handler_shortname  varchar (20),
    @agent_shortname        varchar(20),
    @currency_id            int,
    @Key_Suffix_Int         int,
    @real_insurance_file_cnt int

/* Real insurance file cnt */
SELECT @real_insurance_file_cnt = insurance_file_cnt 
    FROM event_log 
    WHERE event_cnt = @insurance_file_cnt
  
/* Set temporary default values */
--SELECT    @debit_credit = 'D'
SELECT  @loss_id = NULL
SELECT  @loss_code = NULL
SELECT  @loss_date = NULL
SELECT  @accounts_export_status = "p"
/* DC 131000 commented out setting of effective_date */
--SELECT    @effective_date = GetDate()
SELECT  @posting_period_year = NULL
SELECT  @posting_period_number = NULL
SELECT  @agent_cnt = 0
SELECT  @account_handler_cnt = 0
/* Get Insurance File Details */
/* DC 131000 added effective_date below */
SELECT  @product_id = I.product_id,
    @source_id = I.source_id,
    @premium_total = this_premium,
    @insurance_ref = I.insurance_ref,
    @cover_start_date = I.cover_start_date,
    @expiry_date = I.expiry_date,
    @insurance_holder_cnt = F.insurance_holder_cnt, @business_type_id = I.business_type_id,
    @branch_id = I.branch_id,
    @currency_id = I.currency_id,
        @effective_date = I.cover_start_date
FROM    Event_Insurance_File        I,
    Event_Insurance_Folder  F
WHERE   I.insurance_file_cnt = @insurance_file_cnt
AND     F.insurance_folder_cnt= I.insurance_file_cnt

/* Do not set agent to null */
SELECT  @agent_cnt = I.lead_agent_cnt
FROM    Event_Insurance_File        I,
    Event_Insurance_Folder  F
WHERE   I.insurance_file_cnt = @insurance_file_cnt
AND     F.insurance_folder_cnt= I.insurance_file_cnt
AND     I.lead_agent_cnt IS NOT NULL

/* Do not set account handler to null */
SELECT  @account_handler_cnt = I.account_handler_cnt
FROM    Event_Insurance_File        I,
    Event_Insurance_Folder  F
WHERE   I.insurance_file_cnt = @insurance_file_cnt
AND     F.insurance_folder_cnt= I.insurance_file_cnt
AND     I.account_handler_cnt IS NOT NULL
/* Set new folder_id */
SELECT  @transaction_export_folder_id = MAX(transaction_export_folder_id) + 1
FROM    Transaction_Export_Folder
WHERE   source_id = @source_id
IF  @transaction_export_folder_id IS NULL
    SELECT  @transaction_export_folder_id = 1
--SELECT    @document_date = GetDate()

-- TF020301 - Bug fix for out of sequence period_ids
DECLARE @period_start_date  datetime,
        @period_end_date    datetime,
        @previous_period_id int
      
--DJM 06/08/2003 : Allow for periods set up just for the system and set up for each sub-branch.
IF (SELECT COUNT(DISTINCT sub_branch_id) FROM period) = 1
BEGIN
	--Only one set of periods set up under sub-branch 1
	--Get current period_end_date
	SELECT  @period_end_date = period_end_date
	FROM    Period P
	JOIN	Ledger L
	ON	L.current_period_id = P.period_id
	WHERE   L.ledger_short_name = 'SA'
	AND	P.sub_branch_id = 1


	--Get previous period_id by end_date
	SELECT  @previous_period_id = P.period_id
	FROM    Period P
	WHERE   P.period_end_date =
	(
		SELECT  MAX(P.period_end_date)
		FROM    Period P
		WHERE   P.period_end_date < @period_end_date
		AND	P.sub_branch_id = 1
	)
	AND	P.sub_branch_id = 1
END
ELSE
BEGIN
	--Multiple sets of periods set up for each sub-branch.
	--Use the insurance files sub branch to find which set to use.
	--Get current period_end_date
	SELECT  @period_end_date = period_end_date
	FROM    Period P
	JOIN	Ledger L
	ON	L.current_period_id = P.period_id
	JOIN	Event_Insurance_file I
	ON	I.branch_id = P.sub_branch_id
	WHERE   L.ledger_short_name = 'SA'
	AND	I.insurance_file_cnt = @insurance_file_cnt

	--Get previous period_id by end_date
	SELECT  @previous_period_id = P.period_id
	FROM    Period P
	JOIN	Event_Insurance_file I
	ON	I.branch_id = P.sub_branch_id
	WHERE   P.period_end_date =
	(
		SELECT  MAX(P.period_end_date)
		FROM    Period P
		JOIN	Event_Insurance_file I
		ON	I.branch_id = P.sub_branch_id
		WHERE   P.period_end_date < @period_end_date
		AND	I.insurance_file_cnt = @insurance_file_cnt
	)
	AND	I.insurance_file_cnt = @insurance_file_cnt
END

-- Get 1st day of current period
SELECT  @period_start_date = DATEADD(day, 1, period_end_date)
FROM    Period P
WHERE   P.period_id = @previous_period_id
-- TF020301 - End of Bug fix for out of sequence period_ids
/* KB move this so we can tell if its a MTA
IF @cover_start_date >= @period_start_date
    SELECT @accounting_date = @cover_start_date
ELSE
    SELECT @accounting_date = @period_start_date */
    
/* Get Party details */
SELECT  @insurance_holder_shortname = shortname,
    @insurance_holder_id = party_id
FROM    Party
WHERE   party_cnt = @insurance_holder_cnt
SELECT  @account_handler_shortname = shortname,
    @account_handler_id = party_id
FROM    Party
WHERE   party_cnt = @account_handler_cnt
SELECT  @agent_shortname = shortname,
    @agent_id = party_id
FROM    Party
WHERE   party_cnt = @agent_cnt
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
WHERE   currency_id = @currency_id
/* Get Transaction_type details retrieved from Insurance_File_System */
/* Get transaction details from new posting table */
SELECT  @transaction_type_id = P.posting_type_id,
    @transaction_type_code = P.code
FROM    Event_Insurance_File_System I,
    Posting_Type    P
WHERE   I.insurance_file_cnt = @insurance_file_cnt
AND P.posting_type_id = I.last_trans_type_id
/* KB  PN 5284  - if it is a MTA use cover_start_date regardless */
IF (@cover_start_date >= @period_start_date) OR (@period_start_date IS NULL)
    SELECT @accounting_date = @cover_start_date
ELSE
	if @transaction_type_id = 3
		SELECT @accounting_date = @cover_start_date
	else
   SELECT @accounting_date = @period_start_date
/* CTAF 160101 - Dont need this */
SELECT @is_payable_by_instalments = 0

/* Set Document Comment based on Transaction Type Code */
/*
IF RTRIM(@transaction_type_code) = 'NB D'
BEGIN
    SELECT  @document_comment = 'New Business - Direct Premium'
    SELECT  @is_payable_by_instalments = 0
END
ELSE IF RTRIM(@transaction_type_code) = 'NB DI'
BEGIN
    SELECT  @document_comment = 'New Business - Direct Premium'
    SELECT  @is_payable_by_instalments = 1
END
ELSE IF RTRIM(@transaction_type_code) = 'NB I'
BEGIN
    SELECT  @document_comment = 'New Business - Indirect Premium'
    SELECT  @is_payable_by_instalments = 0
END
ELSE IF RTRIM(@transaction_type_code) = 'NB II'
BEGIN
    SELECT  @document_comment = 'New Business - Indirect Premium'   SELECT  @is_payable_by_instalments = 1
END
ELSE IF RTRIM(@transaction_type_code) = 'MTA DI'
BEGIN
    SELECT  @document_comment = 'Change in Payment method - Contra Entry'
    SELECT  @is_payable_by_instalments = 1
END
ELSE IF RTRIM(@transaction_type_code) = 'MTA II'
BEGIN
    SELECT  @document_comment = 'Change in Payment method - Contra Entry'
    SELECT  @is_payable_by_instalments = 1
END
ELSE IF RTRIM(@transaction_type_code) = 'AP D'
BEGIN
    SELECT  @document_comment = 'MTA - Direct Additional Premium'
    SELECT  @is_payable_by_instalments = 0
END
ELSE IF RTRIM(@transaction_type_code) = 'AP DI'
BEGIN
    SELECT  @document_comment = 'MTA - Direct Additional Premium'
    SELECT  @is_payable_by_instalments = 1
END
ELSE IF RTRIM(@transaction_type_code) = 'AP I'
BEGIN
    SELECT  @document_comment = 'MTA - Indirect Additional Premium'
    SELECT  @is_payable_by_instalments = 0
END
ELSE IF RTRIM(@transaction_type_code) = 'AP II'
BEGIN   SELECT  @document_comment = 'MTA - Indirect Additional Premium'
    SELECT  @is_payable_by_instalments = 1
END
ELSE IF RTRIM(@transaction_type_code) = 'RP D'
BEGIN
    SELECT  @document_comment = 'MTA - Direct Return Premium'
    SELECT  @is_payable_by_instalments = 0--    SELECT  @debit_credit = 'C'
END
ELSE IF RTRIM(@transaction_type_code) = 'RP DI'
BEGIN
    SELECT  @document_comment = 'MTA - Direct Return Premium'   SELECT  @is_payable_by_instalments = 1
--  SELECT  @debit_credit = 'C'
END
ELSE IF RTRIM(@transaction_type_code) = 'RP I'
BEGIN
    SELECT  @document_comment = 'MTA - Indirect Return Premium'
    SELECT  @is_payable_by_instalments = 0
--  SELECT  @debit_credit = 'C'
END
ELSE IF RTRIM(@transaction_type_code) = 'RP II'
BEGIN
    SELECT  @document_comment = 'MTA - Indirect Return Premium'
    SELECT  @is_payable_by_instalments = 1
--  SELECT  @debit_credit = 'C'
END
ELSE IF RTRIM(@transaction_type_code) = 'RN D'
BEGIN
    SELECT  @document_comment = 'Renewal - Direct Premium'
    SELECT  @is_payable_by_instalments = 0
END
ELSE IF RTRIM(@transaction_type_code) = 'RN DI'
BEGIN
    SELECT  @document_comment = 'Renewal - Direct Premium'
    SELECT  @is_payable_by_instalments = 1
END
ELSE IF RTRIM(@transaction_type_code) = 'RN I'
BEGIN
    SELECT  @document_comment = 'Renewal - Indirect Premium'
    SELECT  @is_payable_by_instalments = 0
END
ELSE IF RTRIM(@transaction_type_code) = 'RN II'
BEGIN   SELECT  @document_comment = 'Renewal - Indirect Premium'
    SELECT  @is_payable_by_instalments = 1
END
*/

/* CTAF 160101 - Replaced the above with this
         Don't need to worry about the RTRIM, SQL takes care of this. */
SELECT    @document_comment = t.document_comment,
          @is_payable_by_instalments = t.is_payable_by_instalments
FROM      Trans_Code_Type_Lookup t
WHERE     transaction_type_code = @transaction_type_code

/* Set user details */SELECT    @created_by_user_id = @user_id,
    @created_by_username = @user_name

IF  @payment_due_date IS NULL
BEGIN
    SET @payment_due_date = @effective_date
END

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
    reason,
    real_insurance_file_cnt,
    terms_of_payment_id,
    payment_due_date,
    event_log_id)
SELECT
    @product_id,
    0,
    @source_id,
    @insurance_file_cnt,
    @debit_credit,
    @document_ref,
    @document_comment,
    @document_date,
    @is_payable_by_instalments,
    @accounting_date,
    @premium_total,
    @transaction_type_id,
    @transaction_type_code,
    @insurance_ref,
    @effective_date,
    @cover_start_date,
    @expiry_date,
    @insurance_holder_cnt,
    @insurance_holder_id,
    @insurance_holder_shortname,
    @insurance_holder_account_key,
    @product_code,
    @business_type_id,
    @business_type_code,
    @account_handler_cnt,
    @account_handler_id,
    @account_handler_shortname,
    @account_handler_account_key,
    @agent_cnt,
    @agent_id,
    @agent_shortname,
    @agent_account_key,
    @branch_id,
    @branch_code,
    @currency_code,
    @loss_id,
    @loss_code,
    @loss_date,
    @created_by_user_id,
    @created_by_username,
    @accounts_export_status,
    @posting_period_year,
    @posting_period_number,
    @reason,
    @real_insurance_file_cnt,
    @terms_of_payment_id,
    @payment_due_date,
    @event_log_id

/* Return the Count of the Record Added */
SELECT @transaction_export_folder_cnt = @@IDENTITY
END

GO

