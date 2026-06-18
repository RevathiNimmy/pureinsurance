SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Bank'
GO


CREATE PROCEDURE spu_ACT_Delete_Bank
    @bank_id smallint,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS

Update bank Set UserId = @user_id, UniqueId = @unique_id, ScreenHierarchy = @screen_hierarchy
WHERE bank_id = @bank_id

DELETE FROM Bank
WHERE bank_id = @bank_id
GO


