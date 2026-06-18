SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON

GO

EXECUTE DDLDropProcedure 'spe_risk_type_rule_set_add'

GO

CREATE PROCEDURE spe_risk_type_rule_set_add 
	@risk_type_rule_set_id				INT OUTPUT,
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
	IF @risk_type_rule_set_id = 0
		SELECT @risk_type_rule_set_id = NULL

	IF @risk_type_rule_set_id IS NULL
		SELECT @risk_type_rule_set_id = MAX(risk_type_rule_set_id) + 1
		FROM   risk_type_rule_set

	IF @risk_type_rule_set_id IS NULL
		SELECT @risk_type_rule_set_id = 1

	INSERT INTO risk_type_rule_set
				(risk_type_rule_set_id,
				 caption_id,
				 code,
				 DESCRIPTION,
				 is_deleted,
				 effective_date,
				 risk_type_id,
				 file_name,
				 live,
				 type,
				 risk_type_rule_set_type_id,
				 dre_executor_url,
				 dre_default_token,
				 dre_default,
				 dre_quote,
				 dre_validation,
				 post_dre_script, 
				 pre_pre_rule,
				 pre_version,
				 pre_ruleset_effective_date,
				 pre_child_ruleset_effectivedate,
				 UserId,
				 UniqueId,
				 ScreenHierarchy)
	VALUES       (@risk_type_rule_set_id,
				  @caption_id,
				  @code,
				  @description,
				  @is_deleted,
				  @effective_date,
				  @risk_type_id,
				  @file_name,
				  @live,
				  @type,
				  @risk_type_rule_set_type_id,
				  @dre_executor_url,
				  @dre_default_token,
				  @dre_default,
				  @dre_quote,
				  @dre_validate,
				  @post_dre_vb, 
				  @pre_pre_rule,
				  @pre_version,
				  @pre_ruleset_effective_date,
				  @pre_child_ruleset_effectivedate,
				  @UserId,
				  @UniqueId,
				  @ScreenHierarchy)
END

BEGIN
	SELECT risk_type_rule_set_id = @risk_type_rule_set_id
END

GO 
