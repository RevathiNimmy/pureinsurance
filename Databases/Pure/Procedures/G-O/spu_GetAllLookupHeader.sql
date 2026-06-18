SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetAllLookupHeader'
GO


CREATE PROCEDURE spu_GetAllLookupHeader
    @gis_data_model_id int
AS

/*
- get all header
*/
SELECT insurer_panel_member_key,
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

WHERE gis_data_model_id = @gis_data_model_id
GO


