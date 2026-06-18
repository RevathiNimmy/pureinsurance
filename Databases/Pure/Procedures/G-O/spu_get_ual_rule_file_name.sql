SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_ual_rule_file_name'
GO


CREATE PROCEDURE spu_get_ual_rule_file_name
    @gis_policy_link_id INT,
    @user_id INT,
    @transaction_type_id INT,
    @effective_date DATETIME
AS


DECLARE @party_cnt INT,
    @is_underwriter TINYINT,
    @product_id INT,
    @file_name VARCHAR(20)

    SELECT  @party_cnt = party_cnt
    FROM    pmuser
    WHERE   user_id = @user_id

    IF @party_cnt IS NULL
        SELECT @is_underwriter = 1
    ELSE
        SELECT @is_underwriter = 0

    -- There'll only be one, but as far as SQL is concerned there could be a shitload
  IF @transaction_type_id in (1,2,3,5,6)
		BEGIN
			SELECT  @product_id = (SELECT top 1 product_id
			FROM    insurance_file ifi, claim c,
				gis_policy_link gpl
			WHERE   gpl.gis_policy_link_id = @gis_policy_link_id
			AND gpl.claim_id = c.claim_id AND c.policy_id=ifi.insurance_file_cnt)
		END
	ELSE
		BEGIN
			SELECT  @product_id = (SELECT top 1 product_id
			FROM    insurance_file ifi,
				gis_policy_link gpl
			WHERE   gpl.gis_policy_link_id = @gis_policy_link_id
			AND gpl.insurance_file_cnt = ifi.insurance_folder_cnt)
		END

    SELECT  RS.file_name
    FROM    Rule_Set RS,
        PMUser_Authority_Rule_Set_Link UARSL,
        PMUser_Authority_Level AL
    WHERE   RS.rule_set_id = UARSL.rule_set_id
    AND UARSL.is_underwriter = @is_underwriter
    AND UARSL.product_id = @product_id
    AND UARSL.transaction_type_id = @transaction_type_id
    AND UARSL.authority_level_type_id = AL.authority_level_type_id
    AND UARSL.product_id = AL.product_id
    AND AL.user_id = @user_id
GO


