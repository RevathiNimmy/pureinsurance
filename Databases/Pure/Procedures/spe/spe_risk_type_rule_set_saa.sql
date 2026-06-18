SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_risk_type_rule_set_saa'
GO

CREATE PROCEDURE spe_risk_type_rule_set_saa
    @risk_type_id int,
    @type varchar(10)
AS

SELECT
	rtrs.risk_type_rule_set_id,
	rtrs.caption_id,
	rtrs.code,
	rtrs.description,
	rtrs.is_deleted,
	rtrs.effective_date,
	rtrs.risk_type_id,
	rtrs.file_name,
	rtrs.live,
	rtrst.description,
	rtrs.risk_type_rule_set_type_id,
	rtrs.dre_executor_url,
	rtrs.dre_default_token,
	rtrs.dre_default,
	rtrs.dre_quote,
	rtrs.dre_validation,
	rtrs.post_dre_script,
	rtrs.pre_pre_rule,
	rtrs.pre_version,
	rtrs.pre_ruleset_effective_date,
	rtrs.pre_child_ruleset_effectivedate
	FROM risk_type_rule_set rtrs
	LEFT JOIN risk_type_rule_set_type rtrst ON rtrs.risk_type_rule_set_type_id = rtrst.risk_type_rule_set_type_id
	WHERE risk_type_id = @risk_type_id
	AND       type = @type
	ORDER BY risk_type_rule_set_id ASC

GO

