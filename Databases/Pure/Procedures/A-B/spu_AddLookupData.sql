SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_AddLookupData'
GO


CREATE PROCEDURE spu_AddLookupData
    @lookup_key int,
    @line_key int,
    @key_level varchar(60),
    @value varchar(200),
    @type int
AS


INSERT INTO Gis_Lookup_Data
(
    lookup_key,
    line_key,
    key_level,
    value,
    type
)
VALUES
(
    @lookup_key,
    @line_key,
    @key_level, @value,
    @type
)
GO


