SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_Pol_Sch_Sel_Select'
GO


CREATE PROCEDURE spu_GIS_Pol_Sch_Sel_Select
    @gis_policy_link_id int
AS


SELECT
 gis_policy_link_id,
 gis_scheme_id
 FROM GIS_Policy_Schemes_Sel
WHERE
 gis_policy_link_id = @gis_policy_link_id
GO


