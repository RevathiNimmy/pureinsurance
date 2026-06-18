SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Bank'
GO


CREATE PROCEDURE spu_ACT_SelAll_Bank
AS


SELECT
    bank_id,
    code,
    branch_code,
    bank_name,
    head_office,
    bank_address1,
    bank_address2,
    bank_address3,
    bank_address4,
    bank_postal_code,
    bank_country,
    bank_phone_area_code,
    bank_phone_number,
    bank_phone_extension,
    bank_fax_area_code,
    bank_fax_number,
    bank_fax_extension,
    comments
FROM Bank
GO


