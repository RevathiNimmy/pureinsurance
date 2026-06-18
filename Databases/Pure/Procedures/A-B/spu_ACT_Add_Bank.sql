SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Add_Bank'
GO

CREATE PROCEDURE spu_ACT_Add_Bank
    @bank_id smallint OUTPUT,
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
    @bank_account_type_id int = NULL,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS
SET IDENTITY_INSERT BANK ON
IF (@bank_id = 0) OR (@bank_id IS NULL)
BEGIN
    SELECT @bank_id = MAX(bank_id) + 1 FROM Bank
    IF (@bank_id IS NULL) SELECT @bank_id = 1
    IF (@head_office = 0) SELECT @head_office = @bank_id
END

	IF @head_office=0
BEGIN
	    set @head_office=null
END

BEGIN

INSERT INTO Bank (
    code ,
    branch_code ,
    bank_name ,
    head_office ,
    bank_address1 ,
    bank_address2 ,
    bank_address3 ,
    bank_address4 ,
    bank_postal_code ,
    bank_country ,
    bank_phone_area_code ,
    bank_phone_number ,
    bank_phone_extension ,
    bank_fax_area_code ,
    bank_fax_number ,
    bank_fax_extension ,
    comments,
	bank_id,
    bank_account_type_id,
	UserId,
	UniqueId,
	ScreenHierarchy)
VALUES (
    @code,
    @branch_code,
    @bank_name,
    @head_office,
    @bank_address1,
    @bank_address2,
    @bank_address3,
    @bank_address4,
    @bank_postal_code,
    @bank_country,
    @bank_phone_area_code,
    @bank_phone_number,
    @bank_phone_extension,
    @bank_fax_area_code,
    @bank_fax_number,
    @bank_fax_extension,
    @comments,
	@bank_id,
    @bank_account_type_id,
	@user_id,
	@unique_id,
	@screen_hierarchy)
END

				select @bank_id=SCOPE_IDENTITY()
			
				if @head_office is null
				  begin
					update bank set head_office=@bank_id
					where bank_id=@bank_id
				  end
SET IDENTITY_INSERT BANK OFF
SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS OFF
