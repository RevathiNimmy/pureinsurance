SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_QEM_Usage_sel'
GO


CREATE PROCEDURE spu_GIS_QEM_Usage_sel
    @GIS_data_model_id int,
    @GIS_business_type_id int,
    @GIS_scheme_id int
AS


SELECT
    GIS_data_model_id,
    GIS_business_type_id,
    GIS_scheme_id,
    GIS_QEM_id
 FROM GIS_QEM_Usage

WHERE GIS_data_model_id = @GIS_data_model_id
AND GIS_business_type_id = @GIS_business_type_id
AND GIS_scheme_id = @GIS_scheme_id
GO


