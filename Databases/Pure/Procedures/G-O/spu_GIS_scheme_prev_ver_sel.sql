SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_scheme_prev_ver_sel'
GO


CREATE PROCEDURE spu_GIS_scheme_prev_ver_sel
    @gis_insurer_id int,
    @scheme_no int,
    @scheme_ver int
AS


DECLARE @SchemeId Int
DECLARE @PreviousVer int
DECLARE @PreviousSchemeId Int

SET @SchemeId = (SELECT gis_scheme_id
  FROM GIS_Scheme
  WHERE gis_insurer_id = @gis_insurer_id
  AND scheme_no = @scheme_no
  AND scheme_ver = @scheme_ver)

IF @SchemeId IS NULL SET @SchemeId = 0

-- Get previous scheme version
SET @PreviousVer = (SELECT MAX(scheme_ver)
   FROM GIS_Scheme
   WHERE gis_insurer_id = @gis_insurer_id
   AND scheme_no = @scheme_no
   AND scheme_ver < @scheme_ver)

-- if found, get the previous scheme id
IF NOT @PreviousVer IS NULL
SET @PreviousSchemeId = (SELECT gis_scheme_id
   FROM GIS_Scheme
   WHERE gis_insurer_id = @gis_insurer_id
   AND scheme_no = @scheme_no
   AND scheme_ver = @PreviousVer)

-- Pass the two values back as a result set
SELECT @SchemeId AS gis_scheme_id, @PreviousSchemeId AS gis_previous_scheme_id
GO


