SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Contact_Address_Usage_upd'
GO

CREATE PROCEDURE spe_Contact_Address_Usage_upd
    @contact_cnt int,
    @address_cnt int,
    @description varchar(255)
AS
BEGIN
UPDATE Contact_Address_Usage
    SET
    description=@description
WHERE contact_cnt = @contact_cnt AND address_cnt = @address_cnt
END

GO

