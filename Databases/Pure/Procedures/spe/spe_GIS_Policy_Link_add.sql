SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_Policy_Link_add'
GO

CREATE PROCEDURE spe_GIS_Policy_Link_add
    @gis_policy_link_id int OUTPUT ,
    @gis_data_model_id int ,
    @insurance_file_cnt int
AS
BEGIN
INSERT INTO GIS_Policy_Link (
    gis_data_model_id,
    insurance_file_cnt)
VALUES (
    @gis_data_model_id,
    @insurance_file_cnt)
END
BEGIN
SELECT @gis_policy_link_id = @@IDENTITY
END

GO

