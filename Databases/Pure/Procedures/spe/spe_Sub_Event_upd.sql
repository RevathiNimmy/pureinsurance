SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Event_upd'
GO

CREATE PROCEDURE spe_Sub_Event_upd
    @main_event_id int,
    @sub_event_id int,
    @sub_event_type_id int,
    @date_created datetime,
    @description varchar(255),
    @started_date datetime,
    @completed_date datetime,
    @debit_credit char(1),
    @document_ref varchar(255),
    @is_permanent tinyint,
    @user_id smallint
AS
BEGIN
UPDATE Sub_Event
    SET
    sub_event_type_id=@sub_event_type_id,
    date_created=@date_created,
    description=@description,
    started_date=@started_date,
    completed_date=@completed_date,
    debit_credit=@debit_credit,
    document_ref=@document_ref,
    is_permanent=@is_permanent,
    user_id=@user_id
WHERE main_event_id = @main_event_id AND sub_event_id = @sub_event_id
END

GO

