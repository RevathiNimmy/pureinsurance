SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_SchemeGrpMember_sel'
GO

CREATE PROCEDURE spu_GIS_SchemeGrpMember_sel
    @gis_scheme_group_id int
AS
SELECT gis_scheme_group_id, s.gis_scheme_id
      FROM gis_scheme_group_member gm
     INNER JOIN gis_scheme  s
     ON gm.gis_scheme_id=s.gis_scheme_id
     WHERE gis_scheme_group_id = @gis_scheme_group_id 
     AND s.expiry_date > getdate()
GO