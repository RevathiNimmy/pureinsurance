SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Groups_All_sel'
GO


CREATE PROCEDURE spu_GIS_Scheme_Groups_All_sel
AS


SELECT gis_scheme_group_id,
           code,
           caption_id,
           description,
           effective_date,
           gis_business_type_id
      FROM gis_scheme_group
     WHERE is_deleted = 0
GO


