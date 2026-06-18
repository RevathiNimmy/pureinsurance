SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Name_TypeIns_sel'
GO


CREATE PROCEDURE spu_GIS_Scheme_Name_TypeIns_sel
    @gis_business_type_id int,
    @gis_insurer_id int
AS


SELECT
    gis_scheme_id,
    gis_quote_engine_id,
    gis_business_type_id,
    gis_insurer_id,
    scheme_desc
 FROM GIS_Scheme
WHERE scheme_status = 1 and
      start_date <= GETDATE() and
      gis_business_type_id = @gis_business_type_id and
      gis_insurer_id = @gis_insurer_id
GO


