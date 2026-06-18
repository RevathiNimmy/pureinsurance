SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Company'
GO


CREATE PROCEDURE spu_ACT_SelAll_Company
AS


SELECT
    company_id,
    base_currency,
    code,
    description,
    caption_id,
    parent_id,
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
    email,
    vat_no,
    sender_mailbox_id,
    broker_abi_id,
    user_licence_id,
    pm_company_number,

    default_indicator
FROM Company
GO


