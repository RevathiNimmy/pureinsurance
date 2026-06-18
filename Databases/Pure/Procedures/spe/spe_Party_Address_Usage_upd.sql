SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Address_Usage_upd'
GO

CREATE PROCEDURE spe_Party_Address_Usage_upd
    @party_cnt int,
    @address_cnt int,
    @description varchar(255),
    @address_usage_type_id int
AS
BEGIN
UPDATE Party_Address_Usage
    SET
    description=@description,
    address_usage_type_id=@address_usage_type_id
WHERE party_cnt = @party_cnt AND address_cnt = @address_cnt
END

GO

