SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Contact_Usage_add'
GO

CREATE PROCEDURE spe_Party_Contact_Usage_add
    @party_cnt int,
    @contact_cnt int,
    @description varchar(255)
AS
BEGIN
INSERT INTO Party_Contact_Usage (
    party_cnt ,
    contact_cnt ,
    description )
VALUES (
    @party_cnt,
    @contact_cnt,
    @description)
END

GO

