SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TXN_tax_policy_fee_del'
GO

CREATE PROCEDURE spu_TXN_tax_policy_fee_del
	@from_event 		bit,
	@insurance_file_cnt 	int

AS
BEGIN
	IF @from_event=0
		DELETE 
		FROM tax_calculation 
		WHERE insurance_file_cnt=@insurance_file_cnt AND transtype='TTF' AND policy_fee_id IS NOT NULL
	ELSE
		DELETE 
		FROM event_tax_calculation 
		WHERE insurance_file_cnt=@insurance_file_cnt AND transtype='TTF' AND policy_fee_id IS NOT NULL

END
GO 
