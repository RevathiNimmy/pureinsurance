EXECUTE DDLDropProcedure 'spu_PMB_Scheme_Sel'
GO

SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_PMB_Scheme_Sel
AS

SELECT GIS_Scheme_Id, Scheme_Desc, scheme_ver 
FROM GIS_Scheme
where gis_business_type_id = 4
ORDER BY Scheme_Desc, scheme_ver  
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO