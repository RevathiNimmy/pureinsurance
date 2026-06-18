SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Sub_Branch'
GO


CREATE PROCEDURE spu_ACT_SelAll_Sub_Branch
AS


SELECT
    sub_branch_id,
	source_id,
	caption_id,
	code,
	description,
	is_deleted,
	effective_date,
	reg_no_1,
	reg_no_2,
	address1,
	address2,
	address3,
	address4,
	postal_code,
	country_id,
	phone_area_code,
	phone_number,
	phone_extension,
	fax_area_code,
	fax_number,
    fax_extension,
    email,
    vat_no
FROM sub_branch
GO
