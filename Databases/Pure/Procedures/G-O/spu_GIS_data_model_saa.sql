SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_data_model_saa'
GO


CREATE PROCEDURE spu_GIS_data_model_saa
    @Date DateTime
AS

/******************************************************************************/
/* RWH(28/09/2000) - Created to retrieve all currently active GIS data models */
/*                                                                            */
/******************************************************************************/
SELECT  gis_data_model_id,
    description,
    code
FROM GIS_data_model
WHERE is_deleted = 0
AND effective_date < @Date
GO


