SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Sub_Event_add'
GO

CREATE PROCEDURE spe_Sub_Event_add
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
INSERT INTO Sub_Event (
    main_event_id ,
    sub_event_id ,
    sub_event_type_id ,
    date_created ,
    description ,
    started_date ,
    completed_date ,
    debit_credit ,
    document_ref ,
    is_permanent ,
    user_id )
VALUES (
    @main_event_id,
    @sub_event_id,
    @sub_event_type_id,
    @date_created,
    @description,
    @started_date,
    @completed_date,
    @debit_credit,
    @document_ref,
    @is_permanent,
    @user_id)
END

GO

