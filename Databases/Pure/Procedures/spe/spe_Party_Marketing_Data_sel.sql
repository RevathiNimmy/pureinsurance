SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Marketing_Data_sel'
GO

CREATE PROCEDURE spe_Party_Marketing_Data_sel
    @party_cnt int,
    @marketing_code_id int
AS
SELECT
    party_cnt,
    marketing_code_id,
    value
 FROM Party_Marketing_Data
WHERE party_cnt = @party_cnt AND marketing_code_id = @marketing_code_id

GO

