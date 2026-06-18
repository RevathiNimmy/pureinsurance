SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_event_policy_coinsurers_section_upd'
GO


CREATE PROCEDURE spu_event_policy_coinsurers_section_upd(
	@policy_coinsurers_section_id int,
	@insurance_file_cnt int,
	@party_cnt int,
	@COB_rating_section_id int,
	@share_percent numeric(19,4),
	@premium_exc_tax numeric(19,4),
	@premium_inc_tax numeric(19,4),
	@tax_group_id int = NULL,
	@commission_percent numeric(19,4) = NULL,
	@commission_charge numeric(19,4) = NULL,
	@commission_exc_tax numeric(19,4) = NULL,
	@commission_inc_tax numeric(19,4) = NULL,
	@commission_tax_group_id int = NULL,
	@base_premium_exc_tax numeric(19,4),
	@base_premium_inc_tax numeric(19,4),
	@base_commission_charge numeric(19,4) = NULL,
	@base_commission_exc_tax numeric(19,4) = NULL,
	@base_commission_inc_tax numeric(19,4) = NULL,
	@override_rate_table tinyint,
	@is_applied Bit
)
	
AS

BEGIN
UPDATE event_policy_coinsurers_section
SET
	insurance_file_cnt=@insurance_file_cnt,
	party_cnt=@party_cnt,
	COB_rating_section_id=@COB_rating_section_id,
	share_percent=@share_percent,
	premium_exc_tax=@premium_exc_tax,
	premium_inc_tax=@premium_inc_tax,
	tax_group_id=@tax_group_id,
	commission_percent=@commission_percent,
	commission_charge=@commission_charge,
	commission_exc_tax=@commission_exc_tax,
	commission_inc_tax=@commission_inc_tax,
	commission_tax_group_id=@commission_tax_group_id,
	base_premium_exc_tax=@base_premium_exc_tax,
	base_premium_inc_tax=@base_premium_inc_tax,
	base_commission_charge=@base_commission_charge,
	base_commission_exc_tax=@base_commission_exc_tax,
	base_commission_inc_tax=@base_commission_inc_tax,
	override_rate_table=@override_rate_table,
	is_applied=@is_applied
WHERE
	policy_coinsurers_section_id=@policy_coinsurers_section_id
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

