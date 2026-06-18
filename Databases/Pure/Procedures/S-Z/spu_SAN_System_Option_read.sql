SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAN_System_Option_read'
GO
CREATE PROCEDURE dbo.spu_SAN_System_Option_read
    @branch_id smallint,
    @option_number integer,
    @value varchar(255) output
AS BEGIN
    SET NOCOUNT ON

    SELECT @value = NULL
    SELECT @value = value FROM System_Options WHERE branch_id = @branch_id AND option_number = @option_number
END
GO
