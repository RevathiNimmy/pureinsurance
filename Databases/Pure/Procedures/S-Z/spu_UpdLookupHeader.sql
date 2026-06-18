SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_UpdLookupHeader'
GO


CREATE PROCEDURE spu_UpdLookupHeader
    @insurer_panel_member_key int,
    @scheme_number int,
    @lookup_key int,
    @lookup_name varchar(50),
    @effective_date datetime,
    @modified_date datetime,
    @status int,
    @definition varchar(250),
    @valid_constants varchar(250),
    @default_value varchar(200),
    @gis_data_model_id int
AS


UPDATE Gis_Lookup_Header
SET insurer_panel_member_key=@insurer_panel_member_key,
    scheme_number=@scheme_number,
    lookup_key=@lookup_key,
    lookup_name=@lookup_name,
    effective_date=@effective_date,
    modified_date=@modified_date,
    status=@status,
    definition=@definition,
    valid_constants=@valid_constants,
    default_value=@default_value,
    gis_data_model_id=@gis_data_model_id

WHERE insurer_panel_member_key=@insurer_panel_member_key
AND scheme_number=@scheme_number
AND lookup_key=@lookup_key
AND gis_data_model_id=@gis_data_model_id
GO


