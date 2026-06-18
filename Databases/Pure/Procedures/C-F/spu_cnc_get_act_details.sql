EXECUTE DDLDropProcedure 'spu_cnc_get_act_details'
GO

CREATE PROCEDURE spu_cnc_get_act_details
	@insurance_file_cnt int
AS
BEGIN

	/*
		Get the details for the client transaction
		in the document ref.
		This is ok so long as the document_sequence stays
		as 1 for the client, which it should do.
	*/
	SELECT		t.transdetail_id, t.insurance_ref, t.amount, t.account_id, d.insurance_file_cnt
	FROM		transdetail t
	INNER JOIN	document d
	ON		t.document_id = d.document_id
	WHERE		d.insurance_file_cnt = @insurance_file_cnt
	AND		t.document_sequence = 1

END
GO