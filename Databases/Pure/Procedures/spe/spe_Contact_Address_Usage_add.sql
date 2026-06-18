SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Contact_Address_Usage_add'
GO

CREATE PROCEDURE spe_Contact_Address_Usage_add
    @contact_cnt int,
    @address_cnt int,
    @description varchar(255)
AS
BEGIN
INSERT INTO Contact_Address_Usage (
    contact_cnt ,
    address_cnt ,
    description )
VALUES (
    @contact_cnt,
    @address_cnt,
    @description)
END

GO

