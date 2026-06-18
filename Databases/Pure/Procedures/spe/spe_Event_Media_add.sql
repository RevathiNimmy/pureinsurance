SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Event_Media_add'
GO

CREATE PROCEDURE spe_Event_Media_add
    @main_event_id int,
    @sub_event_id int,
    @event_media_id int,
    @event_media_type_id int,
    @event_media_ref varchar(255)
AS
BEGIN
INSERT INTO Event_Media (
    main_event_id ,
    sub_event_id ,
    event_media_id ,
    event_media_type_id ,
    event_media_ref )
VALUES (
    @main_event_id,
    @sub_event_id,
    @event_media_id,
    @event_media_type_id,
    @event_media_ref)
END

GO

