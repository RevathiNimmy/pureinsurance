SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetAllLookupData'
GO


CREATE PROCEDURE spu_GetAllLookupData
    @lookup_key int
AS

/*
-- get all lookup data for given lookup key
*/
SELECT  lookup_key,
        line_key,
        key_level,
        value,
        type
FROM    gis_lookup_data
WHERE   lookup_key = @lookup_key
ORDER BY    line_key
GO


