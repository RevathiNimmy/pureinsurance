SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_Type_sel'
GO

CREATE PROCEDURE spe_Risk_Type_sel
    @risk_type_id int
AS

SELECT
    risk_type_id,
    risk_folder_type_id,
    caption_id,
    code,
    description,
    effective_date,
    is_deleted,
    var_data_structure_id,
    interface_object_name,
    interface_class_name,
    override_peril_ri_band,
    override_peril_xl_band,
    nb_premium_pro_rata_type_id,
    mta_premium_pro_rata_type_id,
    rn_premium_pro_rata_type_id,
    is_share_with_co_insurers,
    is_share_with_re_insurers,
    is_suppress_public_text,
    is_suppress_private_text,
    is_suppress_taxes,
    report_pointer,
    section_mask,
    stamp_duty_rate1,
    stamp_duty_rate2,
    primary_sort,
    secondary_sort,
    header_clause,
    trailer_clause,
    is_ri_at_risk_level,
    is_auto_reinsured,
    header_clause_id,
    trailer_clause_id,
    accumulation_level,
    gis_screen_id,
    display_reinsurance_screen,
	Claims_type_basis_ID,
	Claims_Cover_basis_ID,
	Attach_Claim_Outside_Of_Policy_Period

FROM Risk_Type

WHERE risk_type_id = @risk_type_id

GO