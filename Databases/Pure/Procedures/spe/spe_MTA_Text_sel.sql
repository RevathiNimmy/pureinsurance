SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_MTA_Text_sel'
GO

CREATE PROCEDURE spe_MTA_Text_sel
    @mta_cnt int,
    @mta_text_id int
AS
SELECT
    mta_cnt,
    mta_text_id,
    text_line
 FROM MTA_Text
WHERE mta_cnt = @mta_cnt AND mta_text_id = @mta_text_id

GO

