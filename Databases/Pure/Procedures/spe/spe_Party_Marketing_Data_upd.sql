SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Marketing_Data_upd'
GO

CREATE PROCEDURE spe_Party_Marketing_Data_upd
    @party_cnt int,
    @marketing_code_id int,
    @value varchar(255)
AS
BEGIN
UPDATE Party_Marketing_Data
    SET
    value=@value
WHERE party_cnt = @party_cnt AND marketing_code_id = @marketing_code_id
END

GO

