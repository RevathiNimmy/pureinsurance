SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Group_upd'
GO


CREATE PROCEDURE spu_GIS_Scheme_Group_upd
    @gis_scheme_group_id int,
    @code varchar(10),
    @caption_id int,
    @description varchar(255),
    @gis_business_type_id int
AS


UPDATE gis_scheme_group
    SET code = @code,
        caption_id = @caption_id,
        description = @description,
        gis_business_type_id = @gis_business_type_id
    WHERE gis_scheme_group_id = @gis_scheme_group_id
GO


