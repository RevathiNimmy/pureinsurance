SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_Folder_selall'
GO
CREATE PROCEDURE spu_FSA_Complaint_Folder_selall
	@party_cnt int
AS
BEGIN
SELECT 
	fo.FSA_complaint_folder_cnt,
	fo.complaint_type_Id,
	fo.reference,
	fo.date_opened,
	fo.insurance_file_cnt,
	fo.claim_id,
	fo.date_settled,
	fi.comment,
	fo.fsa_complaint_method_id,
	fo.long_complaint,
	fo.fsa_complaint_category_id,
	fo.fsa_class_of_business_id,
	fo.risk_group_id,
	fo.party_cnt,
	fo.party_handler_cnt,
	fo.handler_id,
	fo.contact,
	fo.complaint_owner_id,
        fo.complaint_upheld,
       	fo.complaint_referred_to_fos,
       	fo.compensation_paid,
	''
FROM FSA_Complaint_Folder fo
JOIN FSA_Complaint_file fi on fo.FSA_complaint_folder_Cnt = fi.FSA_complaint_folder_cnt
JOIN FSA_Complaint_actiontype fa on fi.FSA_complaint_actiontype_id = fa.FSA_complaint_actiontype_id
WHERE fo.party_cnt = @party_cnt
AND fa.code = 'OPEN_C'
END
GO
 


