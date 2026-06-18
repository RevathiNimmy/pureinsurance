SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_SchemeGrpMember_add'
GO


CREATE PROCEDURE spu_GIS_SchemeGrpMember_add
    @gis_scheme_group_id int,
    @gis_scheme_id int
AS

IF @gis_scheme_id<>0
BEGIN
INSERT INTO GIS_Scheme_Group_Member
    ( gis_scheme_group_id,
      gis_scheme_id )
    VALUES ( @gis_scheme_group_id, @gis_scheme_id )
END    
GO


