SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_GrpSchemes_sel'
GO


CREATE PROCEDURE spu_GIS_Scheme_GrpSchemes_sel
    @gis_business_type_id int,
    @gis_scheme_group_id int
AS


SELECT DISTINCT
    s.gis_scheme_id,
    s.gis_quote_engine_id,
    s.gis_business_type_id,
    s.gis_insurer_id,
    s.scheme_desc
 FROM GIS_Scheme s, GIS_Scheme_Group_Member sgm
WHERE s.gis_scheme_id = sgm.gis_scheme_id and
      s.gis_business_type_id = @gis_business_type_id and
      sgm.gis_scheme_group_id = @gis_scheme_group_id and
      scheme_status = 1 and
      start_date <= GETDATE()
GO


