SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_PolicySchemesSelAll_del'
GO


CREATE PROCEDURE spu_GIS_PolicySchemesSelAll_del
    @gis_policy_link_id int
AS


DELETE FROM GIS_Policy_Schemes_Sel
      WHERE gis_policy_link_id = @gis_policy_link_id
GO


