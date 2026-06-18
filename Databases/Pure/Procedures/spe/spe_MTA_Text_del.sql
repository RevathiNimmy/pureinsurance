SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_MTA_Text_del'
GO

CREATE PROCEDURE spe_MTA_Text_del
    @mta_cnt int,
    @mta_text_id int
AS
DELETE FROM MTA_Text
WHERE mta_cnt = @mta_cnt AND mta_text_id = @mta_text_id

GO

