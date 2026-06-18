SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAN_PMUser_check'
GO
-- Authenticate a user in the user table and return the ID.
--
-- Parameters:
--  @name                   Name to search for
--  @integrated             0 = Sirius authentication, 1 = Windows authentication
--  @user_id                Details of user found, or null if user not found
--  @username               "
--  @alternative_identifier "
--  @password               "
--
CREATE PROCEDURE dbo.spu_SAN_PMUser_check
    @name varchar(255),
    @integrated bit,
    @user_id integer OUTPUT,
    @username varchar(255) OUTPUT,
    @alternative_identifier varchar(255) OUTPUT,
    @password varchar(30) OUTPUT
AS 

    SELECT @user_id = NULL, @username = NULL, @alternative_identifier = NULL, @password = NULL

    IF @integrated = 0 BEGIN
        SELECT @user_id = user_id, @username = username, @alternative_identifier = alternative_identifier, @password = password
            FROM PMUser WITH(NOLOCK)
            WHERE username = @name
            AND is_deleted = 0
            AND effective_date <= GETDATE()
    END ELSE BEGIN
        SELECT TOP 1 @user_id = user_id, @username = username, @alternative_identifier = alternative_identifier, @password = password
            FROM PMUser WITH(NOLOCK)
            WHERE alternative_identifier = @name
            AND is_deleted = 0
            AND effective_date <= GETDATE()
            ORDER BY user_id
    END

GO
