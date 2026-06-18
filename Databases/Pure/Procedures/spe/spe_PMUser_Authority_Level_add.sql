SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMUser_Authority_Level_add'
GO

CREATE PROCEDURE spe_PMUser_Authority_Level_add
    @product_id int,
    @user_id smallint,
    @authority_level_type_id int,
	@UserId int = null,
	@UniqueId varchar(50) = null,
	@ScreenHierarchy varchar(500) = null
AS
BEGIN
    INSERT INTO PMUser_Authority_Level (
        product_id ,
        user_id ,
        authority_level_type_id,
		UserId,
		UniqueId,
		ScreenHierarchy )
    VALUES (
        @product_id,
        @user_id,
        @authority_level_type_id,
		@UserId,
		@UniqueId,
		@ScreenHierarchy)
END
GO

