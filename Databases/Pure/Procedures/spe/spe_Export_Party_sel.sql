SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Party_sel'
GO

CREATE PROCEDURE spe_Export_Party_sel
    @export_party_id int
AS
SELECT
    export_party_id,
    shortname,
    resolved_name,
    company_name,
    contact_name,
    title,
    initials,
    surname,
    address1,
    address2,
    address3,
    address4,
    postal_code,
    country_code,
    currency_code,
    tel_area_code,
    tel_no,
    tel_extn,
    fax_area_code,
    fax_no,
    email,
    is_active,
    is_sales,
    is_purchase,
    last_modified
 FROM Export_Party
WHERE export_party_id = @export_party_id

GO

