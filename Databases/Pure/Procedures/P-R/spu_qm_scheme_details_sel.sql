SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_qm_scheme_details_sel'
GO

CREATE PROCEDURE spu_qm_scheme_details_sel
    @GIS_SCHEME_ID int
AS

SELECT s.qm_insurer_ref, 
       s.scheme_no,
       i.abi_81_insurer
FROM   gis_scheme s, 
       gis_insurer i
WHERE gis_scheme_id = @GIS_SCHEME_ID
AND s.gis_insurer_id = i.gis_insurer_id

GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO
