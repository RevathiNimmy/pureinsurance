SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_insurer_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_insurer_sel
    @scheme_id int
AS


BEGIN
    SELECT gsi.gis_insurer_id,
    gsi.description
    FROM GIS_Insurer gsi,
    GIS_Scheme gssch
    WHERE gssch.gis_insurer_id = gsi.gis_insurer_id
    AND gssch.gis_scheme_id = @scheme_id
    ORDER BY gsi.description
END
GO


