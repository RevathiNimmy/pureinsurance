SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_risk_type_rule_set_sel'
GO

CREATE PROCEDURE spe_risk_type_rule_set_sel
    @risk_type_rule_set_id int,
    @risk_type_id int
AS

SELECT
	risk_type_rule_set_id,
	rtrs.caption_id,
	rtrs.code,
	rtrs.description,
	rtrs.is_deleted,
	rtrs.effective_date,
	rtrs.risk_type_id,
	file_name,
	live,
	rtrs.risk_type_rule_set_type_id,
	dre_executor_url,
	dre_default_token,
	dre_default,
	dre_quote,
	dre_validation,
	post_dre_script,	
	gdm.code,
	pre_pre_rule,
	pre_version,
	pre_ruleset_effective_date,
	pre_child_ruleset_effectivedate
 FROM risk_type_rule_set rtrs with (nolock)
 INNER JOIN risk_type RT with (nolock) ON RT.risk_type_id = rtrs.risk_type_id
	INNER JOIN gis_screen gs with (nolock) ON RT.gis_screen_id = gs.gis_screen_id 
 INNER JOIN gis_data_model gdm with (nolock) ON GS.gis_data_Model_id = gdm.gis_data_model_id
WHERE   risk_type_rule_set_id = @risk_type_rule_set_id
AND rtrs.risk_type_id = @risk_type_id
AND rtrs.is_deleted = 0

GO

