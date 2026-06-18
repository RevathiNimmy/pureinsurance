SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_MTAEventDescription_Sel'
GO

CREATE PROCEDURE spu_SIR_MTAEventDescription_Sel
	@product_id INT
AS
	SELECT	med.mta_event_description_id,
		med.code,
		med.description,
		med.is_other,
		med.is_default
	FROM	MTA_Event_Description med
	INNER JOIN Product_MTA_Events pme 
		ON pme.mta_event_description_id = med.mta_event_description_id
		AND pme.product_id = @product_id
	WHERE	med.is_deleted = 0
	ORDER BY med.description
GO
