SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Risk_Group_sel'
GO

CREATE PROCEDURE spe_Risk_Group_sel
    @Risk_Group_id int
AS

SELECT
    risk_group_id 'risk_type_id',
    null 'risk_folder_type_id',
    caption_id,
    code,
    description,
    effective_date,
    is_deleted,
    null 'var_data_structure_id',
    null 'interface_object_name',
    null 'interface_class_name',
    null 'override_peril_ri_band',
    null 'override_peril_xl_band',
    null 'nb_premium_pro_rata_type_id',
    null 'mta_premium_pro_rata_type_id',
    null 'rn_premium_pro_rata_type_id',
    null 'is_share_with_co_insurers',
    null 'is_share_with_re_insurers',
    null 'is_suppress_public_text',
    null 'is_suppress_private_text',
    null 'is_suppress_taxes',
    null 'report_pointer',
    null 'section_mask',
    null 'stamp_duty_rate1',
    null 'stamp_duty_rate2',
    null 'primary_sort',
    null 'secondary_sort',
    null 'header_clause',
    null 'trailer_clause',
    null 'is_ri_at_risk_level',
    null 'is_auto_reinsured',
    null 'header_clause_id',
    null 'trailer_clause_id',
    null 'accumulation_level',
    gis_screen_id 'gis_screen_id'
 FROM Risk_Group

WHERE risk_group_id = @Risk_Group_id

GO

