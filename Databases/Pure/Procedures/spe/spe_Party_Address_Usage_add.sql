SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Address_Usage_add'
GO

CREATE PROCEDURE spe_Party_Address_Usage_add
    @party_cnt int,
    @address_cnt int,
    @description varchar(255),
    @address_usage_type_id int
AS
BEGIN
INSERT INTO Party_Address_Usage (
    party_cnt ,
    address_cnt ,
    description ,
    address_usage_type_id )
VALUES (
    @party_cnt,
    @address_cnt,
    @description,
    @address_usage_type_id)
END

GO

