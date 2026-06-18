SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetAllLineKey'
GO


CREATE PROCEDURE spu_GetAllLineKey
    @lookup_key int
AS


SELECT line_key
FROM Gis_Lookup_Data
WHERE lookup_key=@lookup_key
GROUP BY line_key
ORDER BY line_key
GO


