SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

GO

EXECUTE DDLDropProcedure 'spe_risk_type_rule_set_upd'

GO

CREATE PROCEDURE spe_risk_type_rule_set_upd 
	@risk_type_rule_set_id				INT,
	@caption_id							INT,
	@code								CHAR(10),
	@description						VARCHAR(255),
	@is_deleted							TINYINT,
	@effective_date						DATETIME,
	@risk_type_id						INT,
	@file_name							VARCHAR(255),
	@live								TINYINT,
	@type								VARCHAR(10),
	@risk_type_rule_set_type_id			INT = NULL,
	@dre_executor_url					VARCHAR(255) = NULL,
	@dre_default_token					VARCHAR(255) = NULL,
	@dre_default						TINYINT = NULL,
	@dre_quote							TINYINT = NULL,
	@dre_validate						TINYINT = NULL,
	@post_dre_vb						TINYINT = NULL, 
	@pre_pre_rule						TINYINT = NULL,
	@pre_version						VARCHAR(50) = NULL,
	@pre_ruleset_effective_date			VARCHAR(50) = NULL, 
	@pre_child_ruleset_effectivedate	TINYINT = NULL,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS
BEGIN
	UPDATE risk_type_rule_set
	SET    	caption_id = @caption_id,
			code = @code,
			DESCRIPTION = @description,
			is_deleted = @is_deleted,
			effective_date = @effective_date,
			risk_type_id = @risk_type_id,
			file_name = @file_name,
			live = @live,
			TYPE = @type,
			risk_type_rule_set_type_id = @risk_type_rule_set_type_id,
			dre_executor_url = @dre_executor_url,
			dre_default_token = @dre_default_token,
			dre_default = @dre_default,
			dre_quote = @dre_quote,
			dre_validation = @dre_validate,
			post_dre_script=@post_dre_vb,
			pre_pre_rule=@pre_pre_rule,
			pre_version = @pre_version,
			pre_ruleset_effective_date = @pre_ruleset_effective_date,
			pre_child_ruleset_effectivedate = @pre_child_ruleset_effectivedate,
			UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy = @ScreenHierarchy
	WHERE  risk_type_rule_set_id = @risk_type_rule_set_id
	AND risk_type_id = @risk_type_id
END

GO 
