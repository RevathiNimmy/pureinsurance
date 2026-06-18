SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropTrigger 'PMUser_trd'
GO
CREATE TRIGGER PMUser_trd
ON PMUser
FOR DELETE AS
BEGIN
SET NOCOUNT ON
    DELETE
    FROM    user_authorities
    WHERE   user_id IN
        (SELECT user_id FROM deleted WHERE user_id NOT IN
            (SELECT user_id FROM inserted))
SET NOCOUNT OFF
END
GO

