SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Branch_Currencies'
GO

CREATE PROCEDURE spu_CLM_Get_Branch_Currencies

	@source_id int

AS

BEGIN

	SELECT 	cy.currency_id, 
		cy.description,
		cy.code
	FROM CompanyCurrency cc
	INNER JOIN currency cy ON 
		cc.currency_id=cy.currency_id
	WHERE company_id = @source_id
	AND cy.is_deleted <> 1

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
