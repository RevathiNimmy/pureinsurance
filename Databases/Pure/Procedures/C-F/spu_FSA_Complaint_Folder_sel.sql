SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_Folder_sel'
GO
CREATE PROCEDURE spu_FSA_Complaint_Folder_sel
	@FSA_complaint_folder_cnt int,
	@FSA_complaint_file_cnt int
AS
BEGIN
IF @FSA_complaint_folder_cnt IS NULL
	BEGIN
        SELECT 
			fo.FSA_complaint_folder_cnt,
			fo.complaint_type_Id,
			fo.reference,
			fo.date_opened,
			fo.insurance_file_cnt,
			fo.claim_id,
			fo.date_settled,
			fo.description,
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
		FROM FSA_Complaint_Folder fo,
		     FSA_Complaint_File fi
		WHERE fo.FSA_complaint_folder_cnt=fi.FSA_complaint_folder_cnt
		AND fi.FSA_complaint_file_cnt = @FSA_complaint_file_cnt
	END
ELSE
	BEGIN
		SELECT 
			FSA_complaint_folder_cnt,
			complaint_type_Id,
			reference,
			date_opened,
			insurance_file_cnt,
			claim_id,
			date_settled,
			description,
			fsa_complaint_method_id,
			long_complaint,
			fsa_complaint_category_id,
			fsa_class_of_business_id,
			risk_group_id,
			party_cnt,
			party_handler_cnt,
			handler_id,
			contact,
			complaint_owner_id,
       			complaint_upheld,
       			complaint_referred_to_fos,
       			compensation_paid,
			''
		FROM FSA_Complaint_Folder
		WHERE FSA_complaint_folder_cnt=@FSA_complaint_folder_cnt
	END
END
GO
--{call spu_FSA_Complaint_Folder_sel (?)}
