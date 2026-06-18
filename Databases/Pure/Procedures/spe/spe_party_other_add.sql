SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_other_add'
GO

CREATE PROCEDURE spe_party_other_add
    @party_cnt int,
    @license_type_id int,
    @license_number varchar(20),
    @date_of_birth datetime,
    @gender varchar(70),
    @party_status int,
    @reference_number varchar(20),
    @external_id int,
    @reg_number varchar(20),
    @date_passed_test datetime,
    @contact_name varchar(255),
    @contact_telephone_number varchar(255),
    @insurer_name varchar(255),
    @insurer_address1 varchar(60),
    @insurer_address2 varchar(60),
    @insurer_address3 varchar(60),
    @insurer_address4 varchar(60),
    @insurer_postcode varchar(20),
    @insurer_telephone_number varchar(255),
    @insurer_fax_number varchar(255),
    @insurer_contact_name varchar(255),
    @insurer_email varchar(255),
    @insurer_notes varchar(2000),
    @company_notes varchar(2000),
    @active_indicator bit=null,
    @after_hours_indicator bit=null,
    @priority_indicator tinyint=null,
	@UserId int = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL

AS

BEGIN
INSERT INTO party_other
	(
	party_cnt ,
	license_type_id ,
	license_number ,
	date_of_birth ,
	gender ,
	party_status ,
	reference_number ,
	external_id ,
	reg_number ,
	date_passed_test,
	contact_name,
	contact_telephone_number,
	insurer_name,
	insurer_address1,
	insurer_address2,
	insurer_address3,
	insurer_address4,
	insurer_postcode,
	insurer_telephone_number,
	insurer_fax_number,
	insurer_contact_name,
	insurer_email,
	insurer_notes,
	company_notes,
	active_indicator,
	after_hours_indicator,
	priority_indicator,
	UserId,
	UniqueId,
	ScreenHierarchy
	)
VALUES  (
	@party_cnt,
	@license_type_id,
	@license_number,
	@date_of_birth,
	@gender,
	@party_status,
	@reference_number,
	@external_id,
	@reg_number,
	@date_passed_test,
	@contact_name,
	@contact_telephone_number,
	@insurer_name,
	@insurer_address1,
	@insurer_address2,
	@insurer_address3,
	@insurer_address4,
	@insurer_postcode,
	@insurer_telephone_number,
	@insurer_fax_number,
	@insurer_contact_name,
	@insurer_email,
	@insurer_notes,
	@company_notes,
	@active_indicator,
	@after_hours_indicator,
	@priority_indicator,
	@UserId,
	@UniqueId,
	@ScreenHierarchy
	)
END

GO

