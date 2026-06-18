SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Marketing_Data_del'
GO

CREATE PROCEDURE spe_Party_Marketing_Data_del
    @party_cnt int,
    @marketing_code_id int
AS
DELETE FROM Party_Marketing_Data
WHERE party_cnt = @party_cnt AND marketing_code_id = @marketing_code_id

GO

