SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_event_log_del'
GO

CREATE PROCEDURE spe_event_log_del
    @event_cnt int
AS

DELETE FROM event_log

WHERE event_cnt = @event_cnt

GO

