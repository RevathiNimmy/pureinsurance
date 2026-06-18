SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_DelGisLookupData'
GO


CREATE PROCEDURE spu_DelGisLookupData
    @lookup_key int,
    @line_key int
AS

/*
-- delete lookup data
*/
DELETE  gis_lookup_data
WHERE   lookup_key = @lookup_key
AND     line_key = @line_key
GO


