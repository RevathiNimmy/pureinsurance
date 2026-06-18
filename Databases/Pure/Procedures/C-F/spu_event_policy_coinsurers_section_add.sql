SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_event_policy_coinsurers_section_add'
GO

CREATE PROCEDURE spu_event_policy_coinsurers_section_add(
	@insurance_file_cnt int,
	@party_cnt int,
	@COB_rating_section_id int,	
	@sequence int=NULL,
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
 

if @Sequence is null
Begin
	select @Sequence = isnull(max(sequence),1) from event_policy_coinsurers_section where insurance_file_cnt=@insurance_file_cnt
End


INSERT INTO event_policy_coinsurers_section
( 
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
)
VALUES( 
	@insurance_file_cnt,
	@party_cnt,
	@COB_rating_section_id,
	@sequence,
	@share_percent,
	@premium_exc_tax,
	@premium_inc_tax,
	@tax_group_id,
	@commission_percent,
	@commission_charge,
	@commission_exc_tax,
	@commission_inc_tax,
	@commission_tax_group_id,
	@base_premium_exc_tax,
	@base_premium_inc_tax,
	@base_commission_charge,
	@base_commission_exc_tax,
	@base_commission_inc_tax,
	@override_rate_table,
	@is_applied
)

SELECT @@IDENTITY AS id
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO