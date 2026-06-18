SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_AddLookupHeader'
GO


CREATE PROCEDURE spu_AddLookupHeader
    @insurer_panel_member_key int,
    @scheme_number int,
    @lookup_key int OUTPUT,
    @lookup_name varchar(50),
    @effective_date datetime,
    @modified_date datetime,
    @status int,
    @definition varchar(250),
    @valid_constants varchar(250),
    @default_value varchar(200),
    @gis_data_model_id int
AS


-- GET max lookup key
SELECT @lookup_key = max(lookup_key) FROM Gis_Lookup_Header

-- if no record then default it to 0
IF @lookup_key is null
    SELECT @lookup_key = 0

--increment lookup key by 1
SELECT @lookup_key = @lookup_key + 1

INSERT INTO Gis_Lookup_Header
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
    default_value   ,
    gis_data_model_id
)
VALUES
(
    @insurer_panel_member_key,
    @scheme_number,
    @lookup_key,
    @lookup_name,
    @effective_date,
    @modified_date,
    @status,
    @definition,
    @valid_constants,
    @default_value,
    @gis_data_model_id
)

SELECT @gis_data_model_id
GO


