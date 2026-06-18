SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetAllLookupKey'
GO


CREATE PROCEDURE spu_GetAllLookupKey
AS


SELECT lookup_key
FROM Gis_Lookup_Data
GROUP BY lookup_key
ORDER BY lookup_key
GO


