SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAN_PMMessage_add'
GO
-- Called from the SA.NET Application Block code.
CREATE PROCEDURE dbo.spu_SAN_PMMessage_add
    @event_id integer,
    @handling_instance_id uniqueidentifier,
    @priority integer,
    @severity nvarchar(32),
    @title nvarchar(256),
    @log_date datetime,
    @machine_name nvarchar(32),
    @app_domain_name nvarchar(512),
    @process_id nvarchar(256),
    @process_name nvarchar(512),
    @thread_name nvarchar(512),
    @thread_id nvarchar(128),
    @text ntext,
    @message_id integer output
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @message_type smallint

    -- Derive the Sirius message type enum value.
    SELECT @message_type = CASE @severity
        WHEN N'Critical' THEN 1 -- PMLogFatal
        WHEN N'Error' THEN 2 -- PMLogError
        WHEN N'Warning' THEN 3 -- PMLogWarning
        WHEN N'Information' THEN 5 -- PMLogInfo
        WHEN N'Verbose' THEN 7 -- PMLogDebug2
        ELSE 6 -- PMLogDebug1
        END

    -- Insert the row.
    INSERT INTO PMMessage (
        err_number,
        handling_instance_id,
        priority,
        severity,
        title,
        log_date,
        machine_name,
        app_domain_name,
        process_id,
        process_name,
        thread_id,
        thread_name,
        [text],
        message_type
    ) VALUES (
        @event_id,
        @handling_instance_id,
        @priority,
        @severity,
        @title,
        @log_date,
        @machine_name,
        @app_domain_name,
        @process_id,
        @process_name,
        @thread_id,
        @thread_name,
        @text,
        @message_type
    )

    SELECT @message_id = @@IDENTITY
END
GO
