SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Main_Event_upd'
GO

CREATE PROCEDURE spe_Main_Event_upd
    @main_event_id int,
    @main_event_type_id int,
    @insurance_folder_cnt int,
    @insurance_file_cnt int,
    @effective_date datetime,
    @primary_reference varchar(40),
    @date_created datetime,
    @started_date datetime,
    @completed_date datetime,
    @user_id smallint
AS
BEGIN
UPDATE Main_Event
    SET
    main_event_type_id=@main_event_type_id,
    insurance_folder_cnt=@insurance_folder_cnt,
    insurance_file_cnt=@insurance_file_cnt,
    effective_date=@effective_date,
    primary_reference=@primary_reference,
    date_created=@date_created,
    started_date=@started_date,
    completed_date=@completed_date,
    user_id=@user_id
WHERE main_event_id = @main_event_id
END

GO

