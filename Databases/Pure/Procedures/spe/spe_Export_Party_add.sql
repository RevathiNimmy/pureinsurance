SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Export_Party_add'
GO

CREATE PROCEDURE spe_Export_Party_add
    @export_party_id int OUTPUT ,
    @shortname varchar(20) ,
    @resolved_name varchar(40) ,
    @company_name varchar(40) ,
    @contact_name varchar(40) ,
    @title varchar(10) ,
    @initials varchar(10) ,
    @surname varchar(30) ,
    @address1 varchar(40) ,
    @address2 varchar(40) ,
    @address3 varchar(40) ,
    @address4 varchar(40) ,
    @postal_code varchar(20) ,
    @country_code char(10) ,
    @currency_code char(10) ,
    @tel_area_code char(10) ,
    @tel_no varchar(30) ,
    @tel_extn char(10) ,
    @fax_area_code char(10) ,
    @fax_no varchar(30) ,
    @email varchar(30) ,
    @is_active tinyint ,
    @is_sales tinyint ,
    @is_purchase tinyint ,
    @last_modified datetime
AS
BEGIN
INSERT INTO Export_Party (
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
    last_modified)
VALUES (
    @shortname,
    @resolved_name,
    @company_name,
    @contact_name,
    @title,
    @initials,
    @surname,
    @address1,
    @address2,
    @address3,
    @address4,
    @postal_code,
    @country_code,
    @currency_code,
    @tel_area_code,
    @tel_no,
    @tel_extn,
    @fax_area_code,
    @fax_no,
    @email,
    @is_active,
    @is_sales,
    @is_purchase,
    @last_modified)
END
BEGIN
SELECT @export_party_id = @@IDENTITY
END

GO

