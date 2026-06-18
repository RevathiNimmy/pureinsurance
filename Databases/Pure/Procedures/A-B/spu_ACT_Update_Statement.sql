SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Statement'
GO


CREATE PROCEDURE spu_ACT_Update_Statement
    @statement_id int,
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
UPDATE Statement
    SET
    reportheader_id=@reportheader_id,
    statement_ref=@statement_ref,
    statement_date=@statement_date,
    bf_amount=@bf_amount,
    stmt_total_amount=@stmt_total_amount,
    cf_amount=@cf_amount,
    bf_currency_amount=@bf_currency_amount,
    stmt_total_currency_amount=@stmt_total_currency_amount,
    cf_currency_amount=@cf_currency_amount,
    company_id=@company_id,
    account_id=@account_id,
    currency_id=@currency_id,
    account_name=@account_name,
    short_code=@short_code,
    contact_name=@contact_name,
    address1=@address1,
    address2=@address2,
    address3=@address3,

    address4=@address4,
    postal_code=@postal_code,
    address_country=@address_country,
    credit_limit=@credit_limit,
    user_id=@user_id,
    period_id=@period_id,
    start_date=@start_date,
    end_date=@end_date,
    os_only=@os_only
WHERE statement_id = @statement_id
END
GO


