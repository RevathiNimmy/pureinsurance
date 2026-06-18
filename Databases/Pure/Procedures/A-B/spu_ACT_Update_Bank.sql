SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Bank'
GO


CREATE PROCEDURE spu_ACT_Update_Bank
    @bank_id smallint,
    @code char(10),
    @branch_code varchar(30),
    @bank_name varchar(60),
    @head_office smallint,
    @bank_address1 varchar(40),
    @bank_address2 varchar(40),
    @bank_address3 varchar(40),
    @bank_address4 varchar(40),
    @bank_postal_code varchar(20),
    @bank_country smallint,
    @bank_phone_area_code varchar(10),
    @bank_phone_number varchar(15),
    @bank_phone_extension varchar(6),
    @bank_fax_area_code varchar(10),
    @bank_fax_number varchar(15),
    @bank_fax_extension varchar(6),
    @comments varchar(255),
    @bank_account_type_id int,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS


BEGIN
UPDATE Bank
    SET
    code=@code,
    branch_code=@branch_code,
    bank_name=@bank_name,
    head_office=@head_office,
    bank_address1=@bank_address1,
    bank_address2=@bank_address2,
    bank_address3=@bank_address3,
    bank_address4=@bank_address4,
    bank_postal_code=@bank_postal_code,
    bank_country=@bank_country,
    bank_phone_area_code=@bank_phone_area_code,
    bank_phone_number=@bank_phone_number,
    bank_phone_extension=@bank_phone_extension,
    bank_fax_area_code=@bank_fax_area_code,
    bank_fax_number=@bank_fax_number,
    bank_fax_extension=@bank_fax_extension,
    comments=@comments,
    bank_account_type_id = @bank_account_type_id,
	UserId = @user_id,
	UniqueId = @unique_id,
	ScreenHierarchy = @screen_hierarchy
WHERE bank_id = @bank_id
END
GO


