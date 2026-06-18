SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Event_Public_Text_saa'
GO

CREATE PROCEDURE spe_Event_Public_Text_saa
    @event_cnt int
AS
SELECT
    event_cnt,
    event_public_text_id,
    text_line
 FROM Event_Public_Text
WHERE event_cnt = @event_cnt
ORDER BY event_public_text_id ASC

GO

