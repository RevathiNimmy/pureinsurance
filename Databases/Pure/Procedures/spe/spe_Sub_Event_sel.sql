SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Event_sel'
GO

CREATE PROCEDURE spe_Sub_Event_sel
    @main_event_id int,
    @sub_event_id int
AS
SELECT
    main_event_id,
    sub_event_id,
    sub_event_type_id,
    date_created,
    description,
    started_date,
    completed_date,
    debit_credit,

    document_ref,
    is_permanent,
    user_id
 FROM Sub_Event
WHERE main_event_id = @main_event_id AND sub_event_id = @sub_event_id

GO

