SET QUOTED_IDENTIFIER ON    SET ANSI_NULLS  ON 
GO

EXECUTE DDLDropProcedure 'spu_SIR_SelectAll_ClaimEventDescription'
GO

CREATE PROCEDURE spu_SIR_SelectAll_ClaimEventDescription
	@product_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS

SET NOCOUNT ON

	SELECT	ced.claim_event_description_id, 
			ced.description, 
			CASE WHEN pce.claim_event_description_id IS Null THEN 0 ELSE 1 END As Chosen,
			is_other,
			is_default
	FROM	Claim_Event_Description ced
	LEFT JOIN Product_Claim_Events pce 
		ON 	pce.claim_event_description_id = ced.claim_event_description_id
		AND pce.product_id = @product_id
	WHERE	ced.is_deleted = 0
	ORDER BY ced.claim_event_description_id

SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO

