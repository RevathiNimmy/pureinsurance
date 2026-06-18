SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_get_party_dataset'
GO


CREATE PROCEDURE spu_get_party_dataset
    @nPartyCnt INTEGER
AS
BEGIN

SELECT
P.shortname AS 'ClientCode',
P.name AS 'Last Name',
PPC.party_title_code AS 'Title',
PPC.initials AS 'Initials',
P.trading_name AS 'Trading Name',
P.alternative_identifier  AS 'Alternative Identifier',
SL.description AS 'Service Level',
S.description AS 'Branch',
SB.description AS 'Sub Branch',
A.description AS 'Area',
P.file_code AS 'File Code',
PAgent.Name 'Lead Agent Name',
BA.description AS 'Blacklisting Reason',
PConsultant.resolved_name AS 'Account Executive',
P.Is_Prospect AS 'Is Prospect',
P.Is_also_agent AS 'Is Agent',
PPC.Salutation AS 'Salutation',
CT.description AS 'Preferred Correspondence',
PPC.tpsind AS 'TPS',
PPC.mailshot AS 'MPS',
PPC.empsind AS 'eMPS',
C.description AS 'Currency',
P.payment_method_code AS 'Payment Method',
RSC.description AS 'Renewal Stop Code',
RT.description AS 'Reminder Type',
PPC.Source AS 'Source',
P.Credit_card_Code 'Card Type',
PL.Occupation_code AS 'Occupation',
PL.secondary_occupation_code AS 'Secondary Occupation',
PPC.employer_business AS 'Employer Business',
PPC.secondary_employer_business AS 'Secondary Employer Business',
PPC.employment_status_code AS 'Status',
PPC.secondary_employment_status_co 'Secondary Status',
PL.date_of_birth AS 'DOB',
PL.gender_code AS 'Gender',
PPC.marital_status_code AS 'Marital Status',
N.description AS 'Nationality',
SG.description AS 'Seasonal Gift',
PPC.accommodation_type_code AS 'Accommodation',
P.loyalty_number AS 'Loyalty Number',
PPC.is_pet_owner AS 'Is Pet Owner',
PL.is_smoker  AS 'Is Smoker',
P.tax_number  AS 'Tax Number',
P.domiciled_for_tax AS 'Is Domiciled For Tax',
P.tax_exempt AS 'Tax Exempt',
P.tax_percentage AS 'Tax Percentage'
FROM Party P 
LEFT JOIN Party_Personal_Client PPC ON P.party_cnt = PPC.party_cnt
LEFT JOIN Service_level SL ON SL.service_level_id = P.service_level_id
LEFT JOIN Source S ON S.source_id = P.source_id
LEFT JOIN Sub_Branch SB ON SB.Sub_branch_id = P.sub_branch_id
LEFT JOIN Area A ON A.area_id = P.area_id
LEFT JOIN Party PAgent ON PAgent.party_cnt = P.agent_cnt
LEFT JOIN Blacklist_reason BA ON BA.blacklist_reason_id = P.blacklist_reason_id
LEFT JOIN Party PConsultant ON PConsultant.party_cnt = P.consultant_cnt
LEFT JOIN Contact_Type CT ON CT.contact_type_id = P.Correspondence_Type_id
LEFT JOIN Currency C ON C.currency_id = P.currency_id
LEFT JOIN Renewal_Stop_code RSC ON RSC.Renewal_Stop_code_id = P.Renewal_Stop_code_id
LEFT JOIN reminder_type RT ON RT.reminder_type_id = P.reminder_type_id
LEFT JOIN party_lifestyle PL ON PL.party_cnt = P.Party_cnt
LEFT JOIN Nationality N ON N.nationality_id = PPC.nationality_id
LEFT JOIN seasonal_gift SG ON SG.seasonal_gift_id = P.seasonal_gift_id
where P.party_cnt = @nPartyCnt
AND (PL.party_lifestyle_id = 1 OR PL.party_lifestyle_id IS NULL)

exec spu_ACT_Select_Client_Account_Details @account_key=@nPartyCnt,@company_id=1,@Include_Tax_On_YTD_Turnover=0,@Include_Tax_On_YTD_Income=1

exec spu_get_addresses_for_party @nPartyCnt

exec spu_get_contacts_for_party @nPartyCnt

exec spu_SAM_Get_PartyAssociates @nPartyCnt

exec spe_party_conviction_saa @nPartyCnt

exec spe_party_lifestyle_saa @nPartyCnt

exec spu_SAM_GET_PartyLoyaltyScheme @nPartyCnt

exec spu_PartyBank_Details_Sel @Party_Cnt=@nPartyCnt

 END

