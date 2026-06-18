SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_PolicySchemesSel_add'
GO


CREATE PROCEDURE spu_GIS_PolicySchemesSel_add
    @gis_policy_link_id int,
    @gis_scheme_id int
AS


INSERT INTO GIS_Policy_Schemes_Sel
          ( gis_policy_link_id , gis_scheme_id )
   VALUES ( @gis_policy_link_id , @gis_scheme_id )
GO


