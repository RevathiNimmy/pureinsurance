SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Private_Text_saa'
GO

CREATE PROCEDURE spe_Risk_Private_Text_saa
    @insurance_file_cnt int,
    @risk_id int
AS
SELECT
    risk_cnt,
    risk_private_text_id,
    text_line
 FROM Risk_Private_Text
WHERE risk_cnt = @risk_id
ORDER BY risk_private_text_id ASC

GO

