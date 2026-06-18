SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Event_Media_sel'
GO

CREATE PROCEDURE spe_Event_Media_sel
    @main_event_id int,
    @sub_event_id int,
    @event_media_id int
AS
SELECT
    main_event_id,
    sub_event_id,
    event_media_id,
    event_media_type_id,
    event_media_ref
 FROM Event_Media
WHERE main_event_id = @main_event_id AND sub_event_id = @sub_event_id AND event_media_id = @event_media_id

GO

