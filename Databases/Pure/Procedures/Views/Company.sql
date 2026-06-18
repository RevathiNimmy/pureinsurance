SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'Company'
GO
-- Replacement for the table called Company, because the two tables were completely
-- equivalent. This saves us having to re-write lots of code.
CREATE VIEW Company AS SELECT
    parent_id,
    source_id AS company_id,
    base_currency_id AS base_currency,
    code,
    description,
    caption_id,
    reg_no_1,
    reg_no_2,
    address1,
    address2,
    address3,
    address4,
    postal_code,
    country_id,
    phone_area_code,
    phone_number,
    phone_extension,
    fax_area_code,
    fax_number,
    fax_extension,
    is_deleted,
    effective_date,
    email,
    vat_no,
    sender_mailbox_id,
    broker_abi_id,
    user_licence_id,
    pm_company_number,
    default_indicator
    FROM Source
GO

