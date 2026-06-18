SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_get_Aging_band'
GO

CREATE PROCEDURE spu_get_Aging_band
    @option_number  INT,
    @branch_id           SMALLINT,
    @option_value      VARCHAR(50) OUTPUT
AS
BEGIN
    SELECT @option_value = [value] FROM system_options
        WHERE option_number = @option_number
          AND branch_id = @branch_id
END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

