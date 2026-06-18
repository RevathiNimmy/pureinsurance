SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TXN_tax_policy_fee_add'
GO

CREATE PROCEDURE spu_TXN_tax_policy_fee_add
	@from_event 		bit,
	@insurance_file_cnt 	int,
	@policy_fee_id 		int,
	@tax_group_id 		int,
	@tax_rate 		numeric(7,4),
	@tax_amount 		numeric(19,4)
AS
BEGIN

DECLARE @tax_band_id int
SELECT TOP 1 @tax_band_id=tax_band_id FROM tax_group_tax_band WHERE tax_group_id=@tax_group_id

DECLARE @calc_basis int
SELECT TOP 1 @calc_basis=calc_basis FROM tax_band_rate WHERE tax_band_id=@tax_band_id


	IF @from_event=0
		INSERT INTO tax_calculation (insurance_file_cnt, percentage, value, tax_group_id, policy_fee_id, transtype, tax_band_id, is_value, is_manually_changed, allow_tax_credit,calc_basis)
		VALUES (@insurance_file_cnt, @tax_rate, @tax_amount, @tax_group_id, @policy_fee_id, 'TTF', @tax_band_id, 1, 0, 0,@calc_basis)
	ELSE
		INSERT INTO event_tax_calculation (insurance_file_cnt, percentage, value, tax_group_id, policy_fee_id, transtype, tax_band_id, is_value, is_manually_changed, allow_tax_credit,calc_basis)
		VALUES (@insurance_file_cnt, @tax_rate, @tax_amount, @tax_group_id, @policy_fee_id, 'TTF', @tax_band_id, 1, 0, 0,@calc_basis)

END
GO 
