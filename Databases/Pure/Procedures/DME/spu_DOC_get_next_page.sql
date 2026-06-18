SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_DOC_get_next_page'
GO

CREATE PROCEDURE spu_DOC_get_next_page 
     @next_page integer OUTPUT
AS
BEGIN
    DECLARE @lID integer
    
    SELECT @next_page = NULL, @lID = NULL

    UPDATE DOC_System WITH (ROWLOCK)
        SET @lID = next_page, next_page = next_page + 1
    WHERE system_id = 1

    SELECT @next_page = @lID

    RETURN 0
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
