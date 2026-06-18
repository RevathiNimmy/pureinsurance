SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Statement'
GO


CREATE PROCEDURE spu_ACT_Add_Statement
    @statement_id int OUTPUT,
    @reportheader_id int,
    @statement_ref varchar(25),
    @statement_date datetime,
    @bf_amount numeric(19,4),
    @stmt_total_amount numeric(19,4),
    @cf_amount numeric(19,4),
    @bf_currency_amount numeric(19,4),
    @stmt_total_currency_amount numeric(19,4),
    @cf_currency_amount numeric(19,4),
    @company_id smallint,
    @account_id int,
    @currency_id smallint,
    @account_name varchar(60),
    @short_code char(20),
    @contact_name varchar(60),
    @address1 varchar(40),
    @address2 varchar(40),
    @address3 varchar(40),
    @address4 varchar(40),
    @postal_code varchar(20),
    @address_country smallint,
    @credit_limit money,
    @user_id smallint,
    @period_id int,
    @start_date datetime,
    @end_date datetime,
    @os_only smallint
AS


BEGIN
INSERT INTO Statement (
    reportheader_id,
    statement_ref,
    statement_date,
    bf_amount,
    stmt_total_amount,
    cf_amount,
    bf_currency_amount,
    stmt_total_currency_amount,
    cf_currency_amount,
    company_id,
    account_id,
    currency_id,
    account_name,
    short_code,
    contact_name,
    address1,
    address2,
    address3,
    address4,
    postal_code,
    address_country,
    credit_limit,
    user_id,
    period_id,
    start_date,
    end_date,
    os_only)
VALUES (
    @reportheader_id,
    @statement_ref,
    @statement_date,
    @bf_amount,
    @stmt_total_amount,
    @cf_amount,
    @bf_currency_amount,
    @stmt_total_currency_amount,
    @cf_currency_amount,
    @company_id,
    @account_id,
    @currency_id,
    @account_name,
    @short_code,
    @contact_name,
    @address1,
    @address2,
    @address3,
    @address4,
    @postal_code,
    @address_country,
    @credit_limit,
    @user_id,
    @period_id,
    @start_date,
    @end_date,
    @os_only)
END
BEGIN
SELECT @statement_id = @@IDENTITY
END
GO


