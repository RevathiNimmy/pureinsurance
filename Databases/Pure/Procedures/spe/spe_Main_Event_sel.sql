SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Main_Event_sel'
GO

CREATE PROCEDURE spe_Main_Event_sel
    @main_event_id int
AS
SELECT
    main_event_id,
    main_event_type_id,
    insurance_folder_cnt,
    insurance_file_cnt,
    effective_date,
    primary_reference,
    date_created,
    started_date,
    completed_date,
    user_id
 FROM Main_Event
WHERE main_event_id = @main_event_id

GO

