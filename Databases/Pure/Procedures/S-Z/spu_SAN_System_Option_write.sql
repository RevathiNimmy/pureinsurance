SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAN_System_Option_write'
GO
CREATE PROCEDURE dbo.spu_SAN_System_Option_write
    @branch_id smallint,
    @option_number integer,
    @value varchar(255)
AS BEGIN
    SET NOCOUNT ON

    IF EXISTS (SELECT NULL FROM System_Options WHERE branch_id = @branch_id AND option_number = @option_number) BEGIN
        UPDATE System_Options SET value = @value WHERE branch_id = @branch_id AND option_number = @option_number
    END ELSE BEGIN
        INSERT INTO System_Options (branch_id, option_number, value) VALUES (@branch_id, @option_number, @value)
    END
END
GO
