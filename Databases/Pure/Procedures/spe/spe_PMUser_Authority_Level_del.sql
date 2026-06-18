SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMUser_Authority_Level_del'
GO

CREATE PROCEDURE spe_PMUser_Authority_Level_del
    @product_id int,
    @user_id smallint,
    @authority_level_type_id int,
	@UserId int = null,
	@UniqueId varchar(50) = null,
	@ScreenHierarchy varchar(500) = null
AS

UPDATE PMUser_Authority_Level SET UserId = @UserId,UniqueId = @UniqueId, ScreenHierarchy = @ScreenHierarchy
WHERE product_id = @product_id
    AND user_id = @user_id
    AND authority_level_type_id = @authority_level_type_id

DELETE FROM PMUser_Authority_Level
    WHERE product_id = @product_id
    AND user_id = @user_id
    AND authority_level_type_id = @authority_level_type_id

GO

