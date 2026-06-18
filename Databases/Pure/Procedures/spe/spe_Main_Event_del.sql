SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Main_Event_del'
GO

CREATE PROCEDURE spe_Main_Event_del
    @main_event_id int
AS
DELETE FROM Main_Event
WHERE main_event_id = @main_event_id

GO

