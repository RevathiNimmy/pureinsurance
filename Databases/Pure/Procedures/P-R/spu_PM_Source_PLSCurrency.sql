SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXEC DDLDropProcedure 'spu_PM_Source_PLSCurrency'
GO

CREATE PROCEDURE spu_PM_Source_PLSCurrency
    @company_id INT,
    @key INT,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null

AS

INSERT INTO CompanyCurrency
(
	currency_id,
	company_id,
	UserId,
	UniqueId,
	ScreenHierarchy
)
VALUES
(
	@key,
	@company_id,
	 @user_id,
	 @unique_id,
	 @screen_hierarchy
)

 
GO

