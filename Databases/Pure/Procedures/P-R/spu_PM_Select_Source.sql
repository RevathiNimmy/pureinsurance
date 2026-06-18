SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PM_Select_Source'
GO


CREATE PROCEDURE spu_PM_Select_Source
    @source_id integer
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 New procedure 09/05/2000 DAK */
/********************************************************************************************************/
SELECT source_id,
        code,
        description,
        caption_id,
        parent_id,
        is_deleted,
        effective_date,
        base_currency_id base_currency,
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
        vat_no,
        sender_mailbox_id,
        broker_abi_id,
        user_licence_id,
        pm_company_number,
        default_indicator,
        FSA_CompanyCategory_id,
        FSA_StaffWording,
        underwriting_branch_ind,
	closed_allow_temp_mta,
	closed_allow_perm_mta,
	closed_allow_reports,
	closed_allow_claims,
	closed_allow_accounts,
	FSA_banktype_id,
	UserId,
	UniqueId,
	ScreenHierarchy
    FROM Source
    WHERE source_id = @source_id
GO


