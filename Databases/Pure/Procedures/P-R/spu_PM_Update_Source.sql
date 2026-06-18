SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PM_Update_Source'
GO


CREATE PROCEDURE spu_PM_Update_Source
    @source_id integer,
    @code char(10),
    @description varchar(255),
    @caption_id int,
    @parent_id smallint,
    @base_currency smallint,
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
    @default_indicator char(1),
    @is_deleted int,
    @effective_date datetime,
    @FSA_CompanyCategory_id integer,
    @FSA_StaffWording text,
    @underwriting_branch_ind tinyint,
    @closed_allow_temp_mta tinyint,
    @closed_allow_perm_mta tinyint,
    @closed_allow_reports tinyint,
    @closed_allow_claims tinyint,
    @closed_allow_accounts tinyint,
    @FSA_banktype_id smallint,
	@user_id int,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 New procedure 09/05/2000 DAK */
/********************************************************************************************************/
UPDATE Source
    SET code=@code,
        description=@description,
        caption_id=@caption_id,
        parent_id=@parent_id,
        is_deleted=@is_deleted,
        effective_date=@effective_date,
        base_currency_id=@base_currency,
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
        default_indicator=@default_indicator,
        FSA_CompanyCategory_id=@FSA_CompanyCategory_id,
        FSA_StaffWording=@FSA_StaffWording,
        underwriting_branch_ind=@underwriting_branch_ind,
	closed_allow_temp_mta=@closed_allow_temp_mta,
	closed_allow_perm_mta=@closed_allow_perm_mta,
	closed_allow_reports=@closed_allow_reports,
	closed_allow_claims=@closed_allow_claims,
	closed_allow_accounts=@closed_allow_accounts,
        FSA_banktype_id=@FSA_banktype_id,
		UserId = @user_id,
		UniqueId = @unique_id,
		ScreenHierarchy = @screen_hierarchy
    WHERE source_id = @source_id
GO
