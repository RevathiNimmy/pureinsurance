SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_GIS_data_model_code'
GO


CREATE PROCEDURE spu_get_GIS_data_model_code
    @GIS_data_Model_id INT
AS


SELECT  code

FROM    gis_data_model

WHERE   gis_data_model_id = @gis_data_model_id
GO


