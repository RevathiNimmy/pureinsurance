SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMUser_Authority_Level_upd'
GO

/*************************************************************************/
/* 1.0  21/12/2000 RWH Original (Based on SP Original)           */
/*************************************************************************/
CREATE PROCEDURE spe_PMUser_Authority_Level_upd
    @product_id int,
    @user_id smallint,
    @authority_level_type_id int,
	@UserId int = null,
	@UniqueId varchar(50) = null,
	@ScreenHierarchy varchar(500) = null
AS
BEGIN

UPDATE PMUser_Authority_Level
    SET authority_level_type_id = @authority_level_type_id,UserId = @UserId,UniqueId = @UniqueId,ScreenHierarchy = @ScreenHierarchy

WHERE user_id = @user_id
    AND product_id = @product_id

END

GO

