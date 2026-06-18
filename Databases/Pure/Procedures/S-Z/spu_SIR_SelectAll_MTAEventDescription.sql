SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON  
GO

EXECUTE DDLDropProcedure 'spu_SIR_SelectAll_MTAEventDescription'
GO

CREATE PROCEDURE spu_SIR_SelectAll_MTAEventDescription
	@product_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS

SET NOCOUNT ON

	SELECT	med.mta_event_description_id,
			med.description,
			CASE WHEN pme.mta_event_description_id IS Null THEN 0 ELSE 1 END As Chosen,
			is_other,
			is_default
	FROM	MTA_Event_Description med
	LEFT JOIN Product_MTA_Events pme 
		ON pme.mta_event_description_id = med.mta_event_description_id
		AND pme.product_id = @product_id
	WHERE	med.is_deleted = 0
	ORDER BY med.mta_event_description_id

SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF 
GO

