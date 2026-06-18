EXECUTE DDLDropProcedure 'spu_Get_Versions_For_Scheme'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Versions_For_Scheme

@scheme_no int

 AS

SELECT gis_scheme_id, scheme_ver
FROM gis_Scheme
WHERE scheme_no = @scheme_no
ORDER BY  scheme_ver
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

