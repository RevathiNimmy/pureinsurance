SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Event_Public_Text_add'
GO

CREATE PROCEDURE spe_Event_Public_Text_add
    @event_cnt int,
    @event_public_text_id int,
    @text_line varchar(255)
AS
BEGIN
INSERT INTO Event_Public_Text (
    event_cnt ,
    event_public_text_id ,
    text_line )
VALUES (
    @event_cnt,
    @event_public_text_id,
    @text_line)
END

GO

