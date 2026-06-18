SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_QEM_Usage_new'
GO


CREATE PROCEDURE spu_GIS_QEM_Usage_new
    @gis_scheme_id int,
    @gis_previous_scheme_id int
AS


-- Make a copy of the GIS_QEM_Usage row updated with the NEW gis_scheme_id
INSERT INTO GIS_QEM_Usage(gis_data_model_id, gis_business_type_id, gis_scheme_id, gis_qem_id)
SELECT gis_data_model_id, gis_business_type_id, @gis_scheme_id, gis_qem_id
FROM GIS_QEM_Usage
WHERE gis_scheme_id = @gis_previous_scheme_id
GO


