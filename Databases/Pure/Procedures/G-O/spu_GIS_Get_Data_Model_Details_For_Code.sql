SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_GIS_Get_Data_Model_Details_For_Code'
GO

CREATE PROCEDURE spu_GIS_Get_Data_Model_Details_For_Code

@code varchar(10)

AS

BEGIN

	SELECT 
		gdmt.code

	FROM gis_data_model gdm

	INNER JOIN gis_data_model_type gdmt ON
		gdm.gis_data_model_type_id = gdmt.gis_data_model_type_id

	WHERE gdm.code = @code

END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
