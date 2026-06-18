SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Company'
GO


CREATE PROCEDURE spu_ACT_Add_Company
    @company_id smallint OUTPUT,
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


IF (@company_id = 0) OR (@company_id IS NULL)
BEGIN
    SELECT @company_id = MAX(company_id) + 1 FROM Company
    IF (@company_id IS NULL) SELECT @company_id = 1
END
BEGIN
INSERT INTO Company (
    company_id ,
    base_currency ,
    code ,
    description ,
    caption_id ,
    parent_id ,
    reg_no_1 ,
    reg_no_2 ,
    address1 ,
    address2 ,
    address3 ,
    address4 ,
    postal_code ,
    country_id ,
    phone_area_code ,
    phone_number ,
    phone_extension ,
    fax_area_code ,
    fax_number ,
    fax_extension ,
    email ,
    vat_no ,
    sender_mailbox_id ,
    broker_abi_id ,
    user_licence_id ,
    pm_company_number ,
    default_indicator )
VALUES (
    @company_id,
    @base_currency,
    @code,
    @description,
    @caption_id,
    @parent_id,
    @reg_no_1,
    @reg_no_2,
    @address1,
    @address2,
    @address3,
    @address4,
    @postal_code,
    @country_id,
    @phone_area_code,
    @phone_number,
    @phone_extension,
    @fax_area_code,
    @fax_number,
    @fax_extension,
    @email,
    @vat_no,
    @sender_mailbox_id,
    @broker_abi_id,
    @user_licence_id,
    @pm_company_number,
    @default_indicator)
END
GO


