EXECUTE DDLDropProcedure 'spu_add_trans_export_folder'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_add_trans_export_folder
    @transaction_export_folder_cnt int OUTPUT,
    @stats_folder_cnt int
AS
/************************************************************************************************************
** Version  Description                                                 Who     Date
** -------  ------------------                                          ----    ---------
**  2.0     Removed source_id from calculation of account keys
**          for agents and insurance handlers.                          RWH     06/06/01
**  2.1     Use source_id specific to client to calculate account key.  RWH     09/08/01
**  2.2     Increase loss_code to 30 chars.                             RWH     30/08/01
*************************************************************************************************************/
BEGIN

DECLARE @insurance_file_cnt int,
    @source_id int,
    @transaction_export_folder_id int,
    @debit_credit char (1),
    @document_ref varchar (25),
    @document_comment varchar (60),
    @document_date datetime,
    @accounting_date datetime,
    -- JMK 04/05/2001
    -- @posting_period_year datetime,
    @posting_period_year int,
    @posting_period_number smallint,
    @premium_total numeric(19, 4),
    @transaction_type_id tinyint,
    @transaction_type_code varchar (10),
    @insurance_ref varchar (30),
    @effective_date datetime,
    @cover_start_date datetime,
    @expiry_date datetime,
    @insurance_holder_cnt int,
    @insurance_holder_shortname varchar (20),
    @product_id int,
    @product_code varchar (10),
    @business_type_id smallint,
    @business_type_code varchar (10),
    @account_handler_cnt int,
    @account_handler_shortname varchar (20),
    @agent_cnt int,
    @agent_shortname varchar(20),
    @branch_id smallint,
    @branch_code varchar (10),
    @currency_code char(10),
    @loss_id int,
    @loss_code varchar (30),
    @loss_date datetime,
    @created_by_user_id smallint,
    @created_by_username varchar (12),
    @accounts_export_status char(1),
    @is_payable_by_instalments tinyint,
	@underwriting_year_id int

DECLARE @insurance_holder_account_key   int,
        @account_handler_account_key    int,
        @agent_account_key  int,
        @insurance_holder_id    int,
        @account_handler_id int,
        @agent_id   int


/* Select the matching Stats Folder */

SELECT  @insurance_file_cnt = insurance_file_cnt,
    @source_id = source_id,
    @debit_credit = debit_credit ,
    @document_ref = document_ref ,
    @document_comment = document_comment ,
    @document_date = document_date ,
    @accounting_date = accounting_date ,
    @posting_period_year = posting_period_year ,
    @posting_period_number = posting_period_number ,
    @premium_total = premium_total ,
    @transaction_type_id = transaction_type_id ,
    @transaction_type_code = transaction_type_code ,
    @insurance_ref = insurance_ref ,
    @effective_date = effective_date ,
    @cover_start_date = cover_start_date ,
    @expiry_date = expiry_date ,
    @insurance_holder_cnt = insurance_holder_cnt ,
    @insurance_holder_shortname = insurance_holder_shortname ,
    @product_id = product_id ,
    @product_code = product_code ,
    @business_type_id = business_type_id ,
    @business_type_code = business_type_code ,
    @account_handler_cnt = account_handler_cnt ,
    @account_handler_shortname = account_handler_shortname ,
    @branch_id = branch_id ,
    @branch_code = branch_code ,
    @currency_code = currency_code ,
    @agent_cnt = agent_cnt,
    @agent_shortname = agent_shortname,
    @loss_id = loss_id ,
    @loss_code = loss_code ,
    @loss_date = loss_date ,
    @created_by_user_id = created_by_user_id ,
    @created_by_username = created_by_username,
	@underwriting_year_id = underwriting_year_id

FROM    Stats_Folder
WHERE   stats_folder_cnt = @stats_folder_cnt

-- Initalise installments payment
Select @is_payable_by_instalments = 0

IF  @account_handler_cnt IS NOT NULL
BEGIN
    SELECT  @account_handler_id = party_id
    FROM    party
    WHERE   party_cnt = @account_handler_cnt

    SELECT  @account_handler_account_key = @account_handler_cnt
END

-- RWH (09/08/01) We need the source_id specific to the client to calculate
-- account key below.
IF  @insurance_holder_cnt IS NOT NULL
BEGIN
    SELECT  @insurance_holder_id = party_id
    FROM    party
    WHERE   party_cnt = @insurance_holder_cnt

    SELECT  @insurance_holder_account_key = @insurance_holder_cnt
END

IF  @agent_cnt IS NOT NULL
BEGIN
    SELECT  @agent_id = party_id
    FROM    party
    WHERE   party_cnt = @agent_cnt

    SELECT  @agent_account_key = @agent_cnt
END

SET @transaction_export_folder_id=0
/* Insert the Trans Export Folder */
INSERT INTO Transaction_Export_Folder
                          (insurance_file_cnt,
              source_id,
              transaction_export_folder_id,
              debit_credit,
              document_ref,
              document_comment,
              document_date,
              accounting_date,
              posting_period_year,
              posting_period_number,
              premium_total,
              transaction_type_id,
              transaction_type_code,
              insurance_ref,
              effective_date,
              cover_start_date,
              expiry_date,
              insurance_holder_cnt,
              insurance_holder_shortname,
              product_id,
              product_code,
              business_type_id,
              business_type_code,
              account_handler_cnt,
              account_handler_shortname,
              agent_cnt,
              agent_shortname,
              branch_id,
              branch_code,
              currency_code,
              loss_id,
              loss_code,
              loss_date,
              created_by_user_id,
              created_by_username,
              accounts_export_status,
              is_payable_by_instalments,
              insurance_holder_id,
              insurance_holder_account_key,
              account_handler_id,
              account_handler_account_key,
        /* RWH (06/12/2000) Add next 2 lines. */
              agent_id,
              agent_account_key, 
			  underwriting_year_id)

VALUES                    (@insurance_file_cnt,
              @source_id,
              @transaction_export_folder_id,
              @debit_credit,
              @document_ref,
              @document_comment,
              @document_date,
              @accounting_date,
              @posting_period_year,
              @posting_period_number,
              @premium_total,
              @transaction_type_id,
              @transaction_type_code,
              @insurance_ref,
              @effective_date,
              @cover_start_date,
              @expiry_date,
              @insurance_holder_cnt,
              @insurance_holder_shortname,
              @product_id,
              @product_code,
              @business_type_id,
              @business_type_code,
              @account_handler_cnt,
              @account_handler_shortname,
              @agent_cnt,
              @agent_shortname,
              @branch_id,
              @branch_code,
              @currency_code,
              @loss_id,
              @loss_code,
              @loss_date,
              @created_by_user_id,
              @created_by_username,
              @accounts_export_status,
              @is_payable_by_instalments,
              @insurance_holder_id,
              @insurance_holder_account_key,
              @account_handler_id,
              @account_handler_account_key,
        /* RWH (06/12/2000) Add next 2 lines. */
              @agent_id,
              @agent_account_key, 
			  @underwriting_year_id)
END

BEGIN

/* Return the Count of the Record Added */
SELECT @transaction_export_folder_cnt = @@IDENTITY

END
GO


