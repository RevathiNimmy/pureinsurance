
Create PROCEDURE [dbo].[GetPolicyBranchDetails]
	@insurance_file_cnt INT
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT  RTRIM(s.address1) as 'branch_address1',
			RTRIM(s.address2) as 'branch_address2',
        RTRIM(s.address3) as 'branch_address3',
        RTRIM(s.address4) as 'branch_address4',
        RTRIM(s.postal_code) as 'branch_postal_code',
        RTRIM(s.description) as 'branch_name',
        RTRIM(s.phone_area_code) as 'branch_phone_area_code',
        RTRIM(s.phone_number) as 'branch_phone_number',
        RTRIM(s.phone_extension) as 'branch_phone_extension',
        RTRIM(s.fax_area_code) as 'branch_fax_area_code',
        RTRIM(s.fax_number) as 'branch_fax_number',
        RTRIM(s.fax_extension) as 'branch_fax_extension',
        s.reg_no_1 as 'branch_registration_number_1',
        s.reg_no_2 as 'branch_registration_number_2',
        s.email as 'branch_email',
        s.vat_no as 'branch_vat_number',
        s.broker_abi_id as 'branch_abi_code',
        s.code as 'branch_code'
FROM	dbo.insurance_file ifi
INNER JOIN dbo.source s ON ifi.source_id = s.source_id
WHERE	ifi.insurance_file_cnt= @insurance_file_cnt
       
END




GO


