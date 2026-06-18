SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_SingleLookupData'
GO
CREATE PROCEDURE spu_Get_SingleLookupData

    @lookup_key int,

    @key_level varchar(30)=NULL

AS

IF @key_level  IS NULL

SELECT  key_level,

        value,

        type

FROM    gis_lookup_data

WHERE   lookup_key = @lookup_key

ELSE
SELECT  key_level,

        value,

        type

FROM    gis_lookup_data

WHERE   lookup_key = @lookup_key

   AND  key_level = @key_level
GO