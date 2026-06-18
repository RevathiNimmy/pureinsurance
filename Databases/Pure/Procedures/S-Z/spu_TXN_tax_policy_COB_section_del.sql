SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_TXN_tax_policy_COB_section_del'
GO

CREATE PROCEDURE spu_TXN_tax_policy_COB_section_del
	@from_event 		bit,
	@insurance_file_cnt 	int,
	@insurance_COB_section_id int

AS
BEGIN
	IF @from_event=0
		DELETE 
		FROM tax_calculation 
		WHERE insurance_file_cnt=@insurance_file_cnt AND insurance_section_id=@insurance_COB_section_id
	ELSE
		DELETE 
		FROM event_tax_calculation 
		WHERE insurance_file_cnt=@insurance_file_cnt AND insurance_section_id=@insurance_COB_section_id

END
GO 
