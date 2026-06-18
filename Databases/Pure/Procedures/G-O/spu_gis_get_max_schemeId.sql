SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.spu_GIS_Get_Max_SchemeId  Script Date: 13/02/2002 15:55:47 ******/
EXECUTE DDLDropProcedure 'spu_GIS_Get_Max_SchemeId'
GO

CREATE PROCEDURE spu_GIS_Get_Max_SchemeId

AS

Select max(gis_scheme_id) from gis_scheme
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

