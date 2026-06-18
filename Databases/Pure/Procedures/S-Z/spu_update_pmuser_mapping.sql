SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_update_pmuser_mapping'
GO

CREATE PROCEDURE spu_update_pmuser_mapping 
    @user_id as integer,
    @alternative_identifier as varchar(255),
	@modified_by as int,
	@unique_id as varchar(50)
--****************************************************************************
-- Revision     Description of Modification         Date        Who 
-- -------- ---------------------------             ----------  --- 
-- 1.0          Created                             16/12/2004  AG
--****************************************************************************

AS

DECLARE @screen_hierarchy varchar(500)

BEGIN

	SELECT @screen_hierarchy = 'User(' + username + ')' 
	FROM PMUser 
	WHERE user_id = @user_id

    UPDATE PMUser SET alternative_identifier = @alternative_identifier,
					  ModifiedBy = @modified_by,
					  UniqueId = @unique_id,
					  ScreenHierarchy = @screen_hierarchy
    WHERE user_id = @user_id

END
GO
