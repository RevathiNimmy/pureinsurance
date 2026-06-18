SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Get_Rate_Type_Count'
GO

CREATE PROCEDURE spu_GIS_Get_Rate_Type_Count
    @Description 	varchar(70),
    @GIS_Scheme_ID 	int
AS

    SELECT Count(*) AS NumMatches
    FROM   GIS_Rate_type
    WHERE  description   = @Description
    AND    gis_scheme_id = @GIS_Scheme_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO