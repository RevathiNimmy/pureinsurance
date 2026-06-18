SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Gis_Scheme_Data_sel'
GO

CREATE PROCEDURE spe_Gis_Scheme_Data_sel
    @gis_scheme_id int
AS

SELECT
    gis_scheme_id,
    admin_charge,
    min_perm_charge,
    min_temp_charge,
    reinst_days_with_no_rp,
    min_reinst_premium,
    override_scr,
    gis_business_type_id,
    mta_perm_adm_charge,
    mta_perm_min_value,
    mta_perm_round_type,
    mta_temp_adm_charge,
    mta_temp_min_value,
    mta_temp_round_type,
    mta_can_adm_charge,
    mta_can_min_value,
    mta_can_round_type,
    mta_rei_adm_charge,
    mta_rei_min_value,
    mta_rei_round_type,
    mta_cpd_adm_charge,
    mta_cpd_min_value,
    mta_cpd_round_type
FROM Gis_Scheme_Data
WHERE gis_scheme_id = @gis_scheme_id

GO

