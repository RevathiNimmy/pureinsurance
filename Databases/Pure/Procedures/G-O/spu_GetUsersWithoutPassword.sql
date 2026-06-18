SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GetUsersWithoutPassword'
GO

CREATE PROCEDURE spu_GetUsersWithoutPassword
    @security_model int,
	@username varchar(50)

--****************************************************************************
-- Revision     Description of Modification         Date        Who
-- --------     ---------------------------         ----------  ---
-- 1.0          Created                             16/12/2004  AG
--****************************************************************************

AS

IF @security_model = 0
BEGIN
    SELECT PMUser.user_id,  PMUser.username
    FROM PMUser
    WHERE PMUser.username = @username
	AND isnull(secure_password,'')=''
    AND is_deleted = 0
END

IF @security_model = 1
BEGIN
    SELECT PMUser.user_id,  PMUser.username
    FROM PMUser
    WHERE PMUser.username = @username
	AND isnull(secure_password,'')=''
    AND isnull(alternative_identifier,'') = ''
    AND is_deleted = 0
END
GO
