SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_Folder_upd'
GO
CREATE PROCEDURE spu_FSA_Complaint_Folder_upd
	@FSA_complaint_folder_cnt int,
	@complaint_type_Id tinyint,
	@reference varchar(100),
	@date_opened datetime,
	@insurance_file_cnt int,
	@claim_id int,
	@date_settled datetime,
	@description text,
	@fsa_complaint_method_id int,
	@long_complaint tinyint,
	@fsa_complaint_category_id int,
	@fsa_class_of_business_id int,
	@risk_group_id int,
	@party_cnt int,
	@party_handler_cnt int,
	@handler_id int,
	@contact varchar(255),
	@complaint_owner_id int,
        @complaint_upheld tinyint,
        @complaint_referred_to_fos tinyint,
        @compensation_paid money
AS
BEGIN
UPDATE FSA_Complaint_Folder
SET 
	complaint_type_Id=@complaint_type_Id,
	reference=@reference,
	date_opened=@date_opened,
	insurance_file_cnt=@insurance_file_cnt,
	claim_id=@claim_id,
	date_settled=@date_settled,
	description=@description,
	fsa_complaint_method_id=@fsa_complaint_method_id,
	long_complaint=@long_complaint,
	fsa_complaint_category_id=@fsa_complaint_category_id,
	fsa_class_of_business_id=@fsa_class_of_business_id,
	risk_group_id=@risk_group_id,
	party_cnt=@party_cnt,
	party_handler_cnt=@party_handler_cnt,
	handler_id=@handler_id,
	contact=@contact,
        complaint_owner_id=@complaint_owner_id,
        complaint_upheld=@complaint_upheld, 
        complaint_referred_to_fos=@complaint_referred_to_fos,
        compensation_paid=@compensation_paid 
WHERE FSA_complaint_folder_cnt=@FSA_complaint_folder_cnt
END
GO
