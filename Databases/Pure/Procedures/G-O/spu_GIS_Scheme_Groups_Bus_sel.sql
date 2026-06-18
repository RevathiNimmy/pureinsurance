SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Groups_Bus_sel'
GO


CREATE PROCEDURE spu_GIS_Scheme_Groups_Bus_sel
    @gis_business_type_id int
AS


SELECT gis_scheme_group_id,
           code,
           caption_id,
           description,
           effective_date,
           gis_business_type_id
      FROM gis_scheme_group
     WHERE gis_business_type_id = @gis_business_type_id AND
           is_deleted = 0
GO


