SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_data_model_sel'
GO

-- RAW 19/07/2004 : consolidated scripts : added compiled script details to result set

CREATE PROCEDURE spu_GIS_data_model_sel
    @v_sDataModelCode varchar(10)
AS

SELECT  
    gis_data_model_id,
    description,
    code
FROM GIS_data_model
WHERE code = @v_sDataModelCode
GO


