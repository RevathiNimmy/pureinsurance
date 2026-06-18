SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Claim_Public_Text_saa'
GO

CREATE PROCEDURE spe_Claim_Public_Text_saa
    @claim_cnt int
AS
SELECT
    claim_cnt,
    claim_public_text_id,
    text_line
 FROM Claim_Public_Text
WHERE claim_cnt = @claim_cnt
ORDER BY claim_public_text_id ASC

GO

