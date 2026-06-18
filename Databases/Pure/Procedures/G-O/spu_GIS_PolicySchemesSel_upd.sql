SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GIS_PolicySchemesSel_upd'
GO


CREATE PROCEDURE spu_GIS_PolicySchemesSel_upd
    @gis_policy_link_id int,
    @gis_scheme_id int
AS

/* History: IJM 240801 Created */
UPDATE GIS_Policy_Schemes_Sel
    SET gis_scheme_id = @gis_scheme_id
    WHERE gis_policy_link_id = @gis_policy_link_id
GO


