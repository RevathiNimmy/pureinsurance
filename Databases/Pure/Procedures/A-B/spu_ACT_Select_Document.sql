SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Document'
GO


CREATE PROCEDURE spu_ACT_Select_Document
    @document_id int
AS


SELECT	 d.document_id,
		 d.company_id,
		 d.postingstatus_id,
		 d.documenttype_id,
	     d.auditset_id,
		 d.batch_id,
		 d.document_ref,
		 d.document_date,
		 d.created_date,
		 d.authorised_date,
		 d.comment,
		 d.write_off_reason_id,
		 d.sub_branch_id,
		 d.insurance_file_cnt,
		 d.reason,
		 d.claim_id,
		 d.terms_of_payment_id,
		 d.payment_due_date,
		 dt.code
FROM Document d
	 LEFT JOIN documenttype dt ON d.documenttype_id=dt.documenttype_id
WHERE document_id = @document_id
GO

