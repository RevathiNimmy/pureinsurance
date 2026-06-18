SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_add_pmmessage'
GO
CREATE PROCEDURE spu_add_pmmessage
    @source_id INTEGER,
    @username VARCHAR(256),
    @log_date DATETIME,
    @message_type SMALLINT,
    @calling_app_name VARCHAR(30),
    @text VARCHAR(7365),
    @err_number INTEGER,
    @err_description VARCHAR(255),
    @app_name VARCHAR(30),
    @class_name VARCHAR(30),
    @method_name VARCHAR(30),
    @message_id INTEGER OUTPUT
AS
BEGIN

    INSERT INTO PMMessage
    (
        source_id,
        username,
        log_date,
        message_type,
        [text],
        err_number,
        err_description,
        calling_app_name,
        [app_name],
        class_name,
        method_name
    )
    VALUES
        (
        @source_id,
        @username,
        @log_date,
        @message_type,
        @text,
        @err_number,
        @err_description,
        @calling_app_name,
        @app_name,
        @class_name,
        @method_name
        )

    SELECT @message_id = @@IDENTITY

END


GO

