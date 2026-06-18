SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_policy_coinsurers_section_sel'
GO

CREATE PROCEDURE spu_policy_coinsurers_section_sel(@policy_coinsurers_section_id int)

AS

BEGIN
SELECT	policy_coinsurers_section_id,
	insurance_file_cnt,
	party_cnt,
	COB_rating_section_id,
	[sequence],
	share_percent,
	premium_exc_tax,
	premium_inc_tax,
	tax_group_id,
	commission_percent,
	commission_charge,
	commission_exc_tax,
	commission_inc_tax,
	commission_tax_group_id,
	base_premium_exc_tax,
	base_premium_inc_tax,
	base_commission_charge,
	base_commission_exc_tax,
	base_commission_inc_tax,
	override_rate_table,
	is_applied
FROM
	policy_coinsurers_section
WHERE
	policy_coinsurers_section_id = @policy_coinsurers_section_id


END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO