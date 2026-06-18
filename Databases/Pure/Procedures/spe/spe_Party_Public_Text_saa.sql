SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Public_Text_saa'
GO

CREATE PROCEDURE spe_Party_Public_Text_saa
    @party_cnt int
AS
SELECT
    party_cnt,
    party_public_text_id,
    text_line
 FROM Party_Public_Text
WHERE party_cnt = @party_cnt
ORDER BY party_public_text_id ASC

GO

