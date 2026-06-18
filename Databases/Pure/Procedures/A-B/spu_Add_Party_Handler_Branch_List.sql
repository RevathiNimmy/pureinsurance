--Start(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.1.2.3)

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Add_Party_Handler_Branch_List'
GO
CREATE  PROCEDURE spu_Add_Party_Handler_Branch_List 
    @party_cnt INT,
    @source_id INT,
	@user_id int = NULL,
	@unique_id varchar(50) = NULL,
	@screen_hierarchy varchar(500) = NULL 
AS
BEGIN
    INSERT INTO party_handler_branch 	(party_cnt,
					source_id,UserId,UniqueId,ScreenHierarchy) 
				VALUES 	(@party_cnt,
					@source_id,@user_id,@unique_id,@screen_hierarchy)
END

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--Start(saurabh Agrawal) Tech Spec LOA008 Account Handlers(6.1.2.3)