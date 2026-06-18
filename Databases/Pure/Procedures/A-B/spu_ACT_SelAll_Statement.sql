SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Statement'
GO


CREATE PROCEDURE spu_ACT_SelAll_Statement
    @company_id smallint
AS


SELECT
    statement_id,
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
    os_only
FROM Statement
     WHERE @company_id = company_id
GO


