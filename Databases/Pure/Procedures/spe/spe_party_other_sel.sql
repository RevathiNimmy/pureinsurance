SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_other_sel'
GO

CREATE PROCEDURE spe_party_other_sel
    @party_cnt int
AS

SELECT
	party_cnt,
	license_type_id,
	license_number,
	date_of_birth,
	gender,
	party_status,
	reference_number,
	external_id,
	reg_number,
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
	company_notes
FROM
	party_other
WHERE
	party_cnt = @party_cnt

GO

