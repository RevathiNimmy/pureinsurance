SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_wp_branch'
GO


CREATE PROCEDURE spu_wp_branch
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
DECLARE
	@branch_address1 varchar(40),
	@branch_address2 varchar(40),
        @branch_address3 varchar(40),
        @branch_address4 varchar(40),
        @branch_postal_code varchar(20),
        @branch_name varchar(255),
        @branch_phone_area_code varchar(10),
        @branch_phone_number varchar(15),
        @branch_phone_extension varchar(6),
        @branch_fax_area_code varchar(10),
        @branch_fax_number varchar(15),
        @branch_fax_extension varchar(6),
        @branch_registration_number_1 varchar(30),
        @branch_registration_number_2 varchar(30),
        @branch_email varchar(50),
        @branch_vat_number varchar(20),
        @branch_abi_code varchar(20),
	@source_id integer

SELECT   @branch_address1 = s.address1 ,
         @branch_address2 = s.address2 ,
         @branch_address3 = s.address3 ,
         @branch_address4 = s.address4 ,
         @branch_postal_code = s.postal_code ,
         @branch_name = s.description ,
         @branch_phone_area_code = s.phone_area_code ,
         @branch_phone_number = s.phone_number ,
         @branch_phone_extension = s.phone_extension ,
         @branch_fax_area_code = s.fax_area_code ,
         @branch_fax_number = s.fax_number ,
         @branch_fax_extension = s.fax_extension ,
 	 @branch_registration_number_1 = s.reg_no_1 ,
 	 @branch_registration_number_2 = s.reg_no_2 ,
 	 @branch_email = s.email,
         @branch_vat_number = s.vat_no, 
	 @branch_abi_code = s.broker_abi_id,
	 @source_id = s.source_id
    FROM     party p,
         source s
    WHERE    p.party_cnt = @PartyCnt
         AND p.source_id = s.source_id


if @InsuranceFileCnt > 0
BEGIN
SELECT   @branch_address1 = s.address1 ,
         @branch_address2 = s.address2 ,
         @branch_address3 = s.address3 ,
         @branch_address4 = s.address4 ,
         @branch_postal_code = s.postal_code ,
         @branch_name = s.description ,
         @branch_phone_area_code = s.phone_area_code ,
         @branch_phone_number = s.phone_number ,
         @branch_phone_extension = s.phone_extension ,
         @branch_fax_area_code = s.fax_area_code ,
         @branch_fax_number = s.fax_number ,
         @branch_fax_extension = s.fax_extension ,
 	 @branch_registration_number_1 = s.reg_no_1 ,
 	 @branch_registration_number_2 = s.reg_no_2 ,
 	 @branch_email = s.email,
         @branch_vat_number = s.vat_no, 
	 @branch_abi_code = s.broker_abi_id,
	 @source_id = s.source_id
    FROM     insurance_file i,
         source s
    WHERE    i.insurance_file_cnt = @InsuranceFileCnt
         AND i.source_id = s.source_id
END


select 	'branch_address1' = @branch_address1,
	'branch_address2' = @branch_address2,
        'branch_address3' = @branch_address3,
        'branch_address4' = @branch_address4,
        'branch_postal_code'= @branch_postal_code,
        'branch_name' = @branch_name,
        'branch_phone_area_code' = @branch_phone_area_code,
        'branch_phone_number'= @branch_phone_number,
        'branch_phone_extension' = @branch_phone_extension,
        'branch_fax_area_code' = @branch_fax_area_code,
        'branch_fax_number' = @branch_fax_number,
        'branch_fax_extension' = @branch_fax_extension,
        'branch_registration_number_1' = @branch_registration_number_1,
        'branch_registration_number_2' = @branch_registration_number_2,
        'branch_email' = @branch_email,
        'branch_vat_number' = @branch_vat_number,
        'branch_abi_code' = @branch_abi_code,
        'branch_staffwording' = s.fsa_staffwording
from source s
where s.source_id = @source_id

GO
