SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Main_Event_add'
GO

CREATE PROCEDURE spe_Main_Event_add
    @main_event_id int OUTPUT ,
    @main_event_type_id int ,
    @insurance_folder_cnt int ,
    @insurance_file_cnt int ,
    @effective_date datetime ,
    @primary_reference varchar(40) ,
    @date_created datetime ,
    @started_date datetime ,
    @completed_date datetime ,
    @user_id smallint
AS
BEGIN
INSERT INTO Main_Event (
    main_event_type_id,
    insurance_folder_cnt,
    insurance_file_cnt,
    effective_date,
    primary_reference,
    date_created,
    started_date,
    completed_date,
    user_id)
VALUES (
    @main_event_type_id,
    @insurance_folder_cnt,
    @insurance_file_cnt,
    @effective_date,
    @primary_reference,
    @date_created,
    @started_date,
    @completed_date,
    @user_id)
END
BEGIN
SELECT @main_event_id = @@IDENTITY
END

GO

