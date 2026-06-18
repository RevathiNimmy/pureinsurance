EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Desc_Get'
GO

CREATE PROCEDURE spu_GIS_Scheme_Desc_Get
AS

  SELECT scheme_desc,
         gis_scheme_id,
         gis_business_type_id,
         gis_insurer_id,
         scheme_status
    FROM GIS_Scheme
   --WHERE scheme_status > 0
    WHERE gis_insurer_id > 0
ORDER BY scheme_desc

