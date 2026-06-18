SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Contact_Usage_upd'
GO

CREATE PROCEDURE spe_Party_Contact_Usage_upd
    @party_cnt int,
    @contact_cnt int,
    @description varchar(255)
AS
BEGIN
UPDATE Party_Contact_Usage
    SET
    description=@description
WHERE party_cnt = @party_cnt AND contact_cnt = @contact_cnt
END

GO

