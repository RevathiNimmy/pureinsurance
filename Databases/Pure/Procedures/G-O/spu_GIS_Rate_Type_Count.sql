EXECUTE DDLDropProcedure 'spu_GIS_Get_Rate_Type_Count'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_GIS_Get_Rate_Type_Count
    @Description varchar(70)
AS

    SELECT Count(*) AS NumMatches
      FROM GIS_Rate_type
     WHERE description = @Description


GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
