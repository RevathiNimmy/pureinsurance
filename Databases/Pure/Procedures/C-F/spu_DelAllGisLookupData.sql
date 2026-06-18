SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_DelAllGisLookupData'
GO


CREATE PROCEDURE spu_DelAllGisLookupData
    @lookup_key integer
AS

/*
-- AK 220101 - delete all lookup data for the given lookup key
*/
DELETE GIS_Lookup_Data
    WHERE lookup_key = @lookup_key
GO


