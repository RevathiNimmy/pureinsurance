EXECUTE DDLDropProcedure 'spu_PMB_Is_Same_Scheme'
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE spu_PMB_Is_Same_Scheme 
		@OldSchemeId int,
		@NewSchemeId int
AS
BEGIN
   
   SELECT COUNT(*)
      FROM (SELECT g1.gis_scheme_id 
              FROM gis_scheme g1,
                   gis_scheme g2
             WHERE g1.scheme_no = g2.scheme_no  
               AND g1.scheme_desc=g2.scheme_desc
               AND g2.gis_scheme_id = @OldSchemeId) AS GIS_Comparison
     WHERE GIS_Comparison.gis_scheme_id = @NewSchemeId
 
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

