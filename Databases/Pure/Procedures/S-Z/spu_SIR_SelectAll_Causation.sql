SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_SelectAll_Causation'
GO

CREATE PROCEDURE spu_SIR_SelectAll_Causation
	@product_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS

SET NOCOUNT ON

	SELECT	pc.primary_cause_id,
		pc.description,
		CASE WHEN pac.primary_cause_id IS Null THEN 0 ELSE 1 END As Chosen
	FROM	primary_cause pc
	LEFT JOIN product_allowed_causation pac
		ON pc.primary_cause_id = pac.primary_cause_id
		AND pac.product_id = @product_id
	ORDER BY pc.primary_cause_id

SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO

