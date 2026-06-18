SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Unique_Number'
GO

-- Gets a unique number for a specified table and returns it by reference.
-- Return code: 0 = success, nonzero = database error
CREATE PROCEDURE spu_Get_Unique_Number
    @table_name varchar(70),
    @next_number integer OUTPUT
AS
BEGIN
    DECLARE @lID integer
    DECLARE @nRowCount integer
    DECLARE @nError integer

    SET NOCOUNT ON
    SELECT @next_number = NULL, @lID = NULL

    -- First, make sure there is always a row in the table for this particular
    -- table name. This statement is inherently atomic and is not serialised or
    -- rollbackable.
    INSERT INTO Unique_Number WITH (ROWLOCK) (table_name, next_number)
        SELECT @table_name, 1
        WHERE NOT EXISTS (
            SELECT NULL FROM Unique_Number WITH (ROWLOCK)
            WHERE table_name = @table_name
        )

    SELECT @nError = @@ERROR
    IF @nError <> 0 BEGIN
        SET NOCOUNT OFF
        RETURN @nError
    END

    -- Next, read the current value and simultaneously increment it. This statement
    -- is also inherently atomic and is not serialised or rollbackable.
    UPDATE Unique_Number WITH (ROWLOCK)
        SET @lID = next_number, next_number = next_number + 1
        WHERE table_name = @table_name

    SELECT @nRowCount = @@ROWCOUNT, @nError = @@ERROR
    IF @lID IS NULL OR @nRowCount = 0 OR @nError <> 0 BEGIN
        SET NOCOUNT OFF
        RETURN @nError
    END

    -- Return the current ID.
    SELECT @next_number = @lID

    SET NOCOUNT OFF
    RETURN 0
END
GO

