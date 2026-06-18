SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'Spu_PartyBank_Details_DelUnDel'
GO

CREATE PROCEDURE Spu_PartyBank_Details_DelUnDel
    @party_bank_id 		int,
    @delete 			int,
	@user_id			int = null,  
	@unique_id			varchar(50) = null,  
	@screen_hierarchy	varchar(500) = null  
  
AS  

	UPDATE party_bank
	SET
	    	Is_deleted = @Delete,
			UserId = @user_id,
			UniqueId = @unique_id,
			ScreenHierarchy = @screen_hierarchy
	WHERE party_bank_id = @party_bank_id


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO