SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_SchemeGrpMember_All_del'
GO


CREATE PROCEDURE spu_GIS_SchemeGrpMember_All_del
    @gis_scheme_group_id int
AS


DELETE FROM GIS_Scheme_Group_Member
      WHERE gis_scheme_group_id = @gis_scheme_group_id
GO


