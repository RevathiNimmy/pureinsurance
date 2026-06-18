SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_CheckShortName'
GO

CREATE PROCEDURE spu_CheckShortName

    @AccountCode VARCHAR(20),
    @Exists BIT OUTPUT
    
AS

/*Default parameter to not existing*/
SELECT @Exists = 0

/*If account code exists in the party table then set parameter to show it as existing*/
IF EXISTS
    (
        SELECT 
            NULL
        FROM party 
        WHERE shortname = @AccountCode
    )
BEGIN
    SELECT @Exists = 1
END

/*If account code exists in the account table then set parameter to show it as existing*/
IF EXISTS
    (
        SELECT 
            NULL
        FROM account
        WHERE short_code = @AccountCode
    )
BEGIN
    SELECT @Exists = 1
END

GO

