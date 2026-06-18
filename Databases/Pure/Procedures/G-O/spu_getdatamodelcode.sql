SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_getdatamodelcode'
GO


CREATE PROCEDURE spu_getdatamodelcode
    @GIS_data_Model_id INT
AS


SELECT  code

FROM    gis_data_model

WHERE   gis_data_model_id = @gis_data_model_id
GO


