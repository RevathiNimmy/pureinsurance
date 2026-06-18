SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAN_PMCategory_add'
GO
-- Called from the SA.NET Application Block code.
CREATE PROCEDURE dbo.spu_SAN_PMCategory_add
    @category_name nvarchar(64),
    @message_id integer
AS BEGIN
    SET NOCOUNT ON

    DECLARE @category_id integer

    -- Get the category ID from the name, creating it if necessary.
    SELECT @category_id = NULL
    SELECT @category_id = category_id FROM PMCategory WHERE category_name = @category_name
    IF @category_id IS NULL BEGIN
        INSERT INTO PMCategory (category_name) VALUES (@category_name)
        SELECT @category_id = @@IDENTITY
    END

    -- Insert the row.
    INSERT INTO PMCategory_Message (
        category_id, message_id
    ) SELECT
        @category_id, @message_id
    WHERE NOT EXISTS (
        SELECT NULL
        FROM PMCategory_Message
        WHERE category_id = @category_id
        AND message_id = @message_id
    )
END
GO
