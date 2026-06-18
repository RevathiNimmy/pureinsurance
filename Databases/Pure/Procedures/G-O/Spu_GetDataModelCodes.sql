SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'Spu_GetDataModelCodes'
GO


CREATE PROCEDURE Spu_GetDataModelCodes
	@SpecificModelIndex	INT

AS
IF @SpecificModelIndex = 0
	SELECT 	Code 
	FROM 	GIS_Data_Model
	WHERE 	is_deleted = 0
ELSE IF @SpecificModelIndex <> 0
	SELECT 	Code 
	FROM 	GIS_Data_Model 
	WHERE 	GIS_Data_Model_type_id = @SpecificModelIndex
	AND	is_deleted = 0

GO
