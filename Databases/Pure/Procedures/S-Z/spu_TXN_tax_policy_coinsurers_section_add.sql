SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TXN_tax_policy_coinsurers_section_add'
GO

CREATE PROCEDURE spu_TXN_tax_policy_coinsurers_section_add
	@from_event 		bit,
	@insurance_file_cnt 	int,
	@policy_coinsurers_section_id 		int,
	@is_commission bit,
	@transtype varchar(10),
	@tax_group_id 		int,
	@tax_rate 		numeric(7,4),
	@tax_amount 		numeric(19,4),
	@calc_basis int,
	@insurer_party_cnt int
AS
BEGIN

DECLARE @tax_band_id int

IF @tax_group_id > 0
BEGIN
SELECT TOP 1 @tax_band_id=tax_band_id FROM tax_group_tax_band WHERE tax_group_id=@tax_group_id
END
ELSE
SET @tax_band_id=0

	IF @tax_band_id>0
	BEGIN
		IF @from_event=0
			INSERT INTO tax_calculation (insurance_file_cnt, percentage, value, tax_group_id, policy_coinsurers_section_id, transtype, is_commission_tax, tax_band_id, is_value, is_manually_changed, allow_tax_credit,calc_basis, insurer_party_cnt)
			VALUES (@insurance_file_cnt, @tax_rate, @tax_amount, @tax_group_id, @policy_coinsurers_section_id, @transtype, @is_commission, @tax_band_id, 1, 0, 0,@calc_basis, @insurer_party_cnt)
		ELSE
			INSERT INTO event_tax_calculation (insurance_file_cnt, percentage, value, tax_group_id, policy_coinsurers_section_id, transtype, is_commission_tax, tax_band_id, is_value, is_manually_changed, allow_tax_credit,calc_basis, insurer_party_cnt)
			VALUES (@insurance_file_cnt, @tax_rate, @tax_amount, @tax_group_id, @policy_coinsurers_section_id, @transtype, @is_commission, @tax_band_id, 1, 0, 0,@calc_basis, @insurer_party_cnt)
	END
END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

