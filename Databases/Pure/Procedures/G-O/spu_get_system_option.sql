SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_system_option'
GO

CREATE PROCEDURE spu_get_system_option
    @option_number INT,
    @branch_id SMALLINT,
    @option_value VARCHAR(50) OUTPUT
AS
BEGIN

    SELECT @option_value = [value] FROM hidden_options
        WHERE option_number = @option_number
          AND branch_id = @branch_id

END
GO