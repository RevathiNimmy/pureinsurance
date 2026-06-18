SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_FSA_Complaint_Folder_add'
GO
CREATE PROCEDURE spu_FSA_Complaint_Folder_add
	@FSA_complaint_folder_cnt int OUTPUT ,
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
INSERT INTO FSA_Complaint_Folder(
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
	compensation_paid)
VALUES (
	@complaint_type_Id,
	@reference,
	@date_opened,
	@insurance_file_cnt,
	@claim_id,
	@date_settled,
	@description,
	@fsa_complaint_method_id,
	@long_complaint,
	@fsa_complaint_category_id,
	@fsa_class_of_business_id,
	@risk_group_id,
	@party_cnt,
	@party_handler_cnt,
	@handler_id,
	@contact,
	@complaint_owner_id,
        @complaint_upheld,
        @complaint_referred_to_fos,
        @compensation_paid)
END
BEGIN
SELECT @FSA_complaint_folder_cnt = @@IDENTITY
END
GO 
