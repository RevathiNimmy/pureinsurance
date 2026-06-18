SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_GIS_data_model_from_risk'
GO


CREATE PROCEDURE spu_get_GIS_data_model_from_risk
    @insurance_file_cnt INT,
    @risk_id INT
AS


SELECT  gdm.code,
    gpl.gis_policy_link_id
FROM    gis_data_model gdm,
    gis_policy_link gpl
WHERE   gdm.gis_data_model_id = gpl.gis_data_model_id
AND gpl.insurance_file_cnt = @insurance_file_cnt
AND gpl.risk_id = @risk_id
GO


