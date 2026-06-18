
--Start(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.1.2.1)
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Del_Party_Handler_Branch_List'
GO

CREATE PROCEDURE spu_Del_Party_Handler_Branch_List 
	@party_cnt INT,
	@user_id int = NULL,
	@unique_id varchar(50) = NULL,
	@screen_hierarchy varchar(500) = NULL

AS
BEGIN
	UPDATE  party_handler_branch  
		SET UserId = @user_id, UniqueId = @unique_id, ScreenHierarchy = @screen_hierarchy  
		WHERE  party_cnt = @Party_Cnt

	DELETE party_handler_branch WHERE party_cnt = @party_cnt
END


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--End(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.1.2.1)