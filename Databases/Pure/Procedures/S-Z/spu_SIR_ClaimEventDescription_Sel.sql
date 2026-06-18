SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_ClaimEventDescription_Sel'
GO

CREATE PROCEDURE spu_SIR_ClaimEventDescription_Sel
	@product_id INT
AS
	SELECT	ced.claim_event_description_id,
		ced.code,
		ced.description,
		ced.is_other,
		ced.is_default
	FROM	Claim_Event_Description ced
	INNER JOIN Product_Claim_Events pce 
		ON pce.claim_event_description_id = ced. claim_event_description_id
		AND pce.product_id = @product_id
	WHERE	ced.is_deleted = 0
	ORDER BY ced.description
GO
