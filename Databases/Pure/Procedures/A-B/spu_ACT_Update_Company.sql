SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Company'
GO


CREATE PROCEDURE spu_ACT_Update_Company
    @company_id smallint,
    @base_currency smallint,
    @code char(10),
    @description varchar(255),
    @caption_id int,
    @parent_id smallint,
    @reg_no_1 varchar(30),
    @reg_no_2 varchar(30),
    @address1 varchar(40),
    @address2 varchar(40),
    @address3 varchar(40),
    @address4 varchar(40),
    @postal_code varchar(20),
    @country_id smallint,
    @phone_area_code varchar(10),
    @phone_number varchar(15),
    @phone_extension varchar(6),
    @fax_area_code varchar(10),
    @fax_number varchar(15),
    @fax_extension varchar(6),
    @email varchar(50),
    @vat_no varchar(20),
    @sender_mailbox_id varchar(14),
    @broker_abi_id varchar(6),
    @user_licence_id int,
    @pm_company_number smallint,
    @default_indicator varchar(1)
AS


BEGIN
UPDATE Company
    SET
    base_currency=@base_currency,
    code=@code,
    description=@description,
    caption_id=@caption_id,
    parent_id=@parent_id,
    reg_no_1=@reg_no_1,
    reg_no_2=@reg_no_2,
    address1=@address1,
    address2=@address2,
    address3=@address3,
    address4=@address4,
    postal_code=@postal_code,
    country_id=@country_id,
    phone_area_code=@phone_area_code,
    phone_number=@phone_number,
    phone_extension=@phone_extension,
    fax_area_code=@fax_area_code,
    fax_number=@fax_number,
    fax_extension=@fax_extension,
    email=@email,
    vat_no=@vat_no,
    sender_mailbox_id=@sender_mailbox_id,
    broker_abi_id=@broker_abi_id,
    user_licence_id=@user_licence_id,
    pm_company_number=@pm_company_number,
    default_indicator=@default_indicator
WHERE company_id = @company_id
END
GO


