SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetNextLineKey'
GO


CREATE PROCEDURE spu_GetNextLineKey
    @lookup_key int
AS

/*
-- get next available line key
*/
DECLARE @line_key int

-- get max line key
SELECT @line_key = max(line_key) FROM gis_lookup_data WHERE lookup_key = @lookup_key

-- if no record default it to zero
IF @line_key is null
    SELECT @line_key = 0

-- increment line key by 1
SELECT @line_key = @line_key + 1

SELECT @line_key
GO


