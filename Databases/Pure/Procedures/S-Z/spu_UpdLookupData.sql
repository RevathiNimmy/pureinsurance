SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_UpdLookupData'
GO


CREATE PROCEDURE spu_UpdLookupData
    @lookup_key int,
    @line_key int,
    @key_level varchar(60),
    @value varchar(200),
    @type int
AS


UPDATE Gis_Lookup_Data
SET lookup_key=@lookup_key,
    line_key=@line_key,
    key_level=@key_level,
    value=@value,
    type=@type
WHERE lookup_key = @lookup_key
AND line_key = @line_key
GO


