SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Policy_Link_sel'
GO

CREATE PROCEDURE spe_GIS_Policy_Link_sel
    @gis_policy_link_id int
AS
SELECT
    gis_policy_link_id,
    gis_data_model_id,
    insurance_file_cnt
 FROM GIS_Policy_Link
WHERE gis_policy_link_id = @gis_policy_link_id

GO

