SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

ddldropprocedure 'spu_wp_InsuranceFileBranch'
go

CREATE PROCEDURE spu_wp_InsuranceFileBranch
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
SELECT  s.address1 as 'branch_address1',
        s.address2 as 'branch_address2',
        s.address3 as 'branch_address3',
        s.address4 as 'branch_address4',
        s.postal_code as 'branch_postal_code',
        s.description as 'branch_name',
        s.phone_area_code as 'branch_phone_area_code',
        s.phone_number as 'branch_phone_number',
        s.phone_extension as 'branch_phone_extension',
        s.fax_area_code as 'branch_fax_area_code',
        s.fax_number as 'branch_fax_number',
        s.fax_extension as 'branch_fax_extension',
        s.reg_no_1 as 'branch_registration_number_1',
        s.reg_no_2 as 'branch_registration_number_2',
        s.email as 'branch_email',
        s.vat_no as 'branch_vat_number',
        s.broker_abi_id as 'branch_abi_code',  
        s.code as 'branch_code'
FROM	insurance_file ifi,
	source s
WHERE	ifi.insurance_file_cnt=@InsuranceFileCnt
        AND ifi.source_id = s.source_id
go