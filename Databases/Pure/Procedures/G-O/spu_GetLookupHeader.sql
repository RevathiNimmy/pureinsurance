SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GetLookupHeader'
GO


CREATE PROCEDURE spu_GetLookupHeader
    @lookup_key int
AS


SELECT  insurer_panel_member_key,
    scheme_number,
    lookup_key,
    lookup_name,
    effective_date,
    modified_date,
    status,
    definition,
    valid_constants,
    default_value

FROM    Gis_Lookup_Header

WHERE   lookup_key = @lookup_key
GO


