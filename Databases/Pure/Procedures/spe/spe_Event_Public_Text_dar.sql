SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Event_Public_Text_dar'
GO

CREATE PROCEDURE spe_Event_Public_Text_dar
 @event_cnt int
AS
DELETE
FROM Event_Public_Text
WHERE Event_cnt = @event_cnt

GO

