SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_CopyLookup'
GO


CREATE PROCEDURE spu_CopyLookup
    @lookup_key_in integer,
    @lookup_key_out integer OUTPUT,
    @lookup_name varchar(50)
AS


-- GET max lookup key
    SELECT @lookup_key_out = max(lookup_key) FROM GIS_Lookup_Header

    -- if no record then default it to 0
    IF @lookup_key_out IS NULL
        SELECT @lookup_key_out = 0

    --increment lookup key by 1
    SELECT @lookup_key_out = @lookup_key_out + 1

    INSERT INTO GIS_Lookup_Header
    (
        insurer_panel_member_key,
        scheme_number,
        lookup_key,
        lookup_name,
        effective_date,
        modified_date,
        status,
        definition,
        valid_constants,
        default_value
    )
    SELECT
        insurer_panel_member_key,
        scheme_number,
        @lookup_key_out,
        @lookup_name,
        getdate(),
        getdate(),
        0,
        definition,
        valid_constants,
        default_value
    FROM gis_lookup_header
    WHERE lookup_key = @lookup_key_in

    INSERT INTO GIS_Lookup_Data
    (
        lookup_key,
        line_key,
        key_level,
        value,
        type
    )
    SELECT
        @lookup_key_out,
        line_key,
        key_level,
        value,
        type
    FROM GIS_Lookup_Data
    WHERE lookup_key = @lookup_key_in

    SELECT @lookup_key_out, getdate()
GO


