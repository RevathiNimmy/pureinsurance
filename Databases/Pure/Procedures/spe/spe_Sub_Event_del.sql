SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Event_del'
GO

CREATE PROCEDURE spe_Sub_Event_del
    @main_event_id int,
    @sub_event_id int
AS
DELETE FROM Sub_Event
WHERE main_event_id = @main_event_id AND sub_event_id = @sub_event_id

GO

