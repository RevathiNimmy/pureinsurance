SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Gis_Scheme_Data_upd'
GO

CREATE PROCEDURE spe_Gis_Scheme_Data_upd
    @gis_scheme_id int,
    @admin_charge decimal(4,2),
    @min_perm_charge decimal(4,2),
    @min_temp_charge decimal(4,2),
    @reinst_days_with_no_rp smallint,
    @min_reinst_premium decimal(4,2),
    @override_scr bit,
    @gis_business_type_id smallint,
    @mta_perm_adm_charge int,
    @mta_perm_min_value int,
    @mta_perm_round_type char(1),
    @mta_temp_adm_charge int,
    @mta_temp_min_value int,
    @mta_temp_round_type char(1),
    @mta_can_adm_charge int,
    @mta_can_min_value int,
    @mta_can_round_type char(1),
    @mta_rei_adm_charge int,
    @mta_rei_min_value int,
    @mta_rei_round_type char(1),
    @mta_cpd_adm_charge int,
    @mta_cpd_min_value int,
    @mta_cpd_round_type char(1)
AS
BEGIN

UPDATE Gis_Scheme_Data
    SET
    admin_charge=@admin_charge,
    min_perm_charge=@min_perm_charge,
    min_temp_charge=@min_temp_charge,
    reinst_days_with_no_rp=@reinst_days_with_no_rp,
    min_reinst_premium=@min_reinst_premium,
    override_scr=@override_scr,
    gis_business_type_id=@gis_business_type_id,
    mta_perm_adm_charge=@mta_perm_adm_charge,
    mta_perm_min_value=@mta_perm_min_value,
    mta_perm_round_type=@mta_perm_round_type,
    mta_temp_adm_charge=@mta_temp_adm_charge,
    mta_temp_min_value=@mta_temp_min_value,
    mta_temp_round_type=@mta_temp_round_type,
    mta_can_adm_charge=@mta_can_adm_charge,
    mta_can_min_value=@mta_can_min_value,
    mta_can_round_type=@mta_can_round_type,
    mta_rei_adm_charge=@mta_rei_adm_charge,
    mta_rei_min_value=@mta_rei_min_value,
    mta_rei_round_type=@mta_rei_round_type,
    mta_cpd_adm_charge=@mta_cpd_adm_charge,
    mta_cpd_min_value=@mta_cpd_min_value,
    mta_cpd_round_type=@mta_cpd_round_type

WHERE gis_scheme_id = @gis_scheme_id

END
GO

