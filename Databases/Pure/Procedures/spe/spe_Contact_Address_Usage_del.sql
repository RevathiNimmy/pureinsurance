SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Contact_Address_Usage_del'
GO

CREATE PROCEDURE spe_Contact_Address_Usage_del
    @contact_cnt int,
    @address_cnt int
AS
DELETE FROM Contact_Address_Usage
WHERE contact_cnt = @contact_cnt AND address_cnt = @address_cnt

GO

