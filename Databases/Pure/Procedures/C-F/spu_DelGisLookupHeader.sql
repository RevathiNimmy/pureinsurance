SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_DelGisLookupHeader'
GO


CREATE PROCEDURE spu_DelGisLookupHeader
    @lookup_key int,
    @gis_data_model_id int
AS

/*
-delete lookup header and all associated records in lookup data
*/
DELETE  Gis_Lookup_Data
WHERE   lookup_key = @lookup_key

DELETE Gis_Lookup_Header
WHERE   lookup_key = @lookup_key
AND gis_data_model_id = @gis_data_model_id
GO


