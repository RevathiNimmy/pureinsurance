SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXEC DDLDropProcedure 'spu_PM_Source_PLLCurrency'
GO

CREATE PROCEDURE spu_PM_Source_PLLCurrency
    @company_id INT,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS

SELECT
    C.currency_id,
    C.description,
    CASE WHEN CC.currency_id IS Null
      THEN 0
      ELSE 1
    END As Chosen
FROM Currency C
LEFT JOIN CompanyCurrency CC
	ON CC.currency_id = C.currency_id
	AND CC.company_id = @company_id


GO

