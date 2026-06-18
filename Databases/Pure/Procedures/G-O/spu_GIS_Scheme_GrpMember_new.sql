SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_Scheme_GrpMember_new'
GO


CREATE PROCEDURE spu_GIS_Scheme_GrpMember_new
    @gis_scheme_id int,
    @gis_previous_scheme_id int
AS


DECLARE @gis_scheme_group_id int

-- get gis_scheme_group_id for previous scheme version
SET @gis_scheme_group_id =
    (SELECT gis_scheme_group_id FROM GIS_Scheme_Group_Member
    WHERE gis_scheme_id = @gis_previous_scheme_id)

-- if found, add new row to GIS_Scheme_Group_Member
IF NOT @gis_scheme_group_id IS NULL
    INSERT INTO GIS_Scheme_Group_Member
    ( gis_scheme_group_id, gis_scheme_id )
    VALUES ( @gis_scheme_group_id, @gis_scheme_id )
GO


