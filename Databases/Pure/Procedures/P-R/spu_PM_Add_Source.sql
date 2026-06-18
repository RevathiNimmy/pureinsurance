SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PM_Add_Source'
GO

CREATE PROCEDURE spu_PM_Add_Source  
    @source_id integer OUTPUT,  
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
    @FSA_CompanyCategory_id int,  
    @FSA_StaffWording text,  
    @underwriting_branch_ind tinyint,  
    @closed_allow_temp_mta tinyint,  
    @closed_allow_perm_mta tinyint,  
    @closed_allow_reports tinyint,  
    @closed_allow_claims tinyint,  
    @closed_allow_accounts tinyint,  
    @FSA_banktype_id smallint,
	@user_id int = null,
	@unique_id varchar(50) = null,
	@screen_hierarchy varchar(500) = null
AS  
  
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.0 New procedure 09/05/2000 DAK */  
/* 1.1 AG - 21/10/2004 - PN 15790 - Add system currency as default currency to the branch */  
/********************************************************************************************************/  
  
/* AG - 21/10/2004 - PN 15790 */  
DECLARE @system_currency_id smallint  
  
IF (@source_id = 0) OR (@source_id IS NULL)  
BEGIN  
  
 SELECT @source_id = MAX(source_id) + 1 FROM Source  
  
 IF (@source_id IS NULL)  
 BEGIN  
        SELECT @source_id = 1  
 END  
  
END  
  
INSERT INTO Source  
(  
 source_id,  
 code,  
 description,  
 caption_id,  
 parent_id,  
 is_deleted,  
 effective_date,  
 base_currency_id,  
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
)  
VALUES  
(  
 @source_id,  
 @code,  
 @description,  
 @caption_id,  
 @parent_id,  
 @is_deleted,  
 @effective_date,  
 @base_currency,  
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
 @default_indicator,  
 @FSA_CompanyCategory_id,  
 @FSA_StaffWording,  
 @underwriting_branch_ind,  
 @closed_allow_temp_mta,  
 @closed_allow_perm_mta,  
 @closed_allow_reports,  
 @closed_allow_claims,  
 @closed_allow_accounts,  
 @FSA_banktype_id,
 @user_id,
 @unique_id,
 @screen_hierarchy
)
  
-- NB: Multi Account should only be created when the system option is set and it is broking  
IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number = 1 AND branch_id = 1 AND value = 'A')  
BEGIN
	IF EXISTS(SELECT NULL FROM sysobjects WHERE name = 'spu_Create_MULTI_Account' AND xtype = 'P')  
	BEGIN  
		EXEC spu_Create_MULTI_Account @source_id  
	END  
END 
   
IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number = 1 AND branch_id = 1 AND value = 'U')  
BEGIN  
 INSERT INTO pmuser_source  (user_id,source_id)
 SELECT  
  user_id,  
  @source_id  
 FROM pmuser  
END  
  
/* Add a default currency rate for the base currency */  
INSERT INTO CurrencyRate (rate_against_base, currency_id, company_id,  
 effective_from)  
VALUES (1, @base_currency, @source_id, CONVERT(VARCHAR(20),GETDATE(),112))  
  
/* AG - 21/10/2004 - PN 15790 */  
SELECT @system_currency_id =  currency_id  
FROM PMSystem  
WHERE system_id = 1  
INSERT INTO CompanyCurrency (currency_id, company_id)  
VALUES(@system_currency_id, @source_id)  

IF EXISTS(SELECT NULL FROM hidden_options WHERE option_number = 91 AND branch_id = 1 AND value = '1')  
BEGIN  
	IF EXISTS(SELECT NULL FROM sysobjects WHERE name = 'spu_Create_MULTI_Account' AND xtype = 'P')  
	BEGIN  
    	EXEC spu_SQ_Update_Data_in_Policy_Numbers
	END
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
