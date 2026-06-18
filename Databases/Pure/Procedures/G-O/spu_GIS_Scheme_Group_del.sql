SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Group_del'
GO


CREATE PROCEDURE spu_GIS_Scheme_Group_del
    @gis_scheme_group_id int
AS


UPDATE gis_scheme_group
    SET is_deleted = 1
    WHERE gis_scheme_group_id = @gis_scheme_group_id
GO


