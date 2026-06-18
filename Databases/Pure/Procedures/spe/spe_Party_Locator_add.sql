SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Locator_add'
GO

CREATE PROCEDURE spe_Party_Locator_add
    @party_cnt int,
    @locator_type_id int,
    @party_locator_id int,
    @value varchar(255)
AS
BEGIN
INSERT INTO Party_Locator (
    party_cnt ,
    locator_type_id ,
    party_locator_id ,
    value )
VALUES (
    @party_cnt,
    @locator_type_id,
    @party_locator_id,
    @value)
END

GO

