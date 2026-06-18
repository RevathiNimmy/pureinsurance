SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_sel'
GO


CREATE PROCEDURE spu_Risk_Type_sel

	@risk_type_id int

AS



	SELECT  rt.risk_type_id,

		rt.risk_folder_type_id,

		rt.caption_id,

		rt.code,

		rt.[description],

		rt.effective_date,

		rt.is_deleted,

		rt.var_data_structure_id,

		rt.interface_object_name,

		rt.interface_class_name,

		rt.override_peril_ri_band,

		rt.override_peril_xl_band,

		rt.nb_premium_pro_rata_type_id,

		rt.mta_premium_pro_rata_type_id,

		rt.rn_premium_pro_rata_type_id,

		rt.is_share_with_co_insurers,

		rt.is_share_with_re_insurers,

		rt.is_suppress_public_text,

		rt.is_suppress_private_text,

		rt.is_suppress_taxes,

		rt.report_pointer,

		rt.section_mask,

		rt.stamp_duty_rate1,

		rt.stamp_duty_rate2,

		rt.primary_sort,

		rt.secondary_sort,

		rt.header_clause,

		rt.trailer_clause,

		rt.is_ri_at_risk_level,

		rt.is_auto_reinsured,

		rt.header_clause_id,

		rt.trailer_clause_id,

		rt.accumulation_level,

		rt.gis_screen_id,

		dt1.[description],

		dt2.[description],

		rt.is_deferred_RI_permitted,

		rt.claims_is_post_taxes,

		rt.display_reinsurance_screen,

		rt.allow_add_ratingsection,

		rt.allow_edit_ratingsection,

		rt.allow_delete_ratingsection,

		rt.allow_edit_ratingsection_ratetype,

		rt.allow_edit_ratingsection_rate,

		rt.allow_edit_ratingsection_suminsured,

		rt.allow_edit_ratingsection_thispremium,

        	rt.display_claims_reinsurance_screen,
			rt.Claims_type_basis_ID,
			rt.Claims_Cover_basis_ID,
			rt.Attach_Claim_Outside_Of_Policy_Period

	FROM    Risk_Type rt

	LEFT JOIN

		document_template dt1 ON dt1.document_template_id = rt.header_clause_id

	LEFT JOIN

		document_template dt2 ON dt2.document_template_id = rt.trailer_clause_id

	WHERE   rt.risk_type_id = @risk_type_id


GO
