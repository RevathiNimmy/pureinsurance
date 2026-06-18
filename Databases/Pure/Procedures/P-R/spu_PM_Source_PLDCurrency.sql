SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXEC DDLDropProcedure 'spu_PM_Source_PLDCurrency'
GO

CREATE PROCEDURE spu_PM_Source_PLDCurrency
    @company_id INT,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS

UPDATE  CompanyCurrency  
		SET UserId = @user_id, UniqueId = @unique_id, ScreenHierarchy = @screen_hierarchy  
		WHERE  company_id = @company_id 

DELETE FROM CompanyCurrency
WHERE company_id = @company_id

    

GO