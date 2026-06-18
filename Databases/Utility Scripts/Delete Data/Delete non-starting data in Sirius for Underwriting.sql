USE Sirius_For_Broking

-- DISABLE ALL CONSTRAINTS AND TRIGGERS
EXECUTE DDLEnableIntegrity 0
GO

DECLARE @Table_name varchar(255)

BEGIN TRAN

--***********************************
--*                                 *
--* DELETE ALL ENTRIES              *
--*                                 *
--***********************************
PRINT 'DELETE ALL ENTRIES FROM FOLLOWING TABLES.'

SELECT @Table_name = 'abandoned_numbers'
PRINT @Table_name
DELETE abandoned_numbers
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'accumulation_class'
PRINT @Table_name
DELETE accumulation_class
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'accumulation_limit'
PRINT @Table_name
DELETE accumulation_limit
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'accumulation_values'
PRINT @Table_name
DELETE accumulation_values
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Address'
PRINT @Table_name
DELETE Address
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'agent_commission'
PRINT @Table_name
DELETE agent_commission
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Agent_group_rate'
PRINT @Table_name
DELETE agent_group_rate
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Agent_rate'
PRINT @Table_name
DELETE agent_rate
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Arc_Archive_Folder'
PRINT @Table_name
DELETE arc_archive_folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'aviation'
PRINT @Table_name
DELETE aviation
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Block_Record_Usage'
PRINT @Table_name
DELETE block_record_usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'branch'
PRINT @Table_name
DELETE branch
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Broking_companies'
PRINT @Table_name
DELETE broking_companies
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Broking_user'
PRINT @Table_name
DELETE broking_user
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'campaign'
PRINT @Table_name
DELETE campaign
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Catastrophe_Code'
PRINT @Table_name
DELETE catastrophe_code
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim'
PRINT @Table_name
DELETE claim
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Address'
PRINT @Table_name
DELETE claim_address
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'claim_conviction'
PRINT @Table_name
DELETE claim_conviction
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Expert_Service'
PRINT @Table_name
DELETE claim_expert_service
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Party'
PRINT @Table_name
DELETE claim_party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Party_Claim'
PRINT @Table_name
DELETE claim_party_claim
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'claim_party_link'
PRINT @Table_name
DELETE claim_party_link
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Party_type'
PRINT @Table_name
DELETE claim_party_type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'claim_Peril'
PRINT @Table_name
DELETE claim_peril
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Private_Text'
PRINT @Table_name
DELETE claim_private_text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Public_Text'
PRINT @Table_name
DELETE claim_public_text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Risk'
PRINT @Table_name
DELETE claim_risk
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Structure'
PRINT @Table_name
DELETE claim_structure
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_user_defined_risk_data'
PRINT @Table_name
DELETE claim_user_defined_risk_data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Class_Of_Business_Analysis'
PRINT @Table_name
DELETE Class_Of_Business_Analysis
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Client_product_link'
PRINT @Table_name
DELETE Client_product_link
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Coi_Arrangement'
PRINT @Table_name
DELETE Coi_Arrangement
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Coi_Compulsory_Value'
PRINT @Table_name
DELETE Coi_Compulsory_Value
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Collect_Type'
PRINT @Table_name
DELETE Collect_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'combined_liability'
PRINT @Table_name
DELETE combined_liability
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'combined_motor'
PRINT @Table_name
DELETE combined_motor
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'commercial_combined'
PRINT @Table_name
DELETE commercial_combined
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Contact'
PRINT @Table_name
DELETE Contact
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'defined_risk_data'
PRINT @Table_name
DELETE defined_risk_data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'document_spooler'
PRINT @Table_name
DELETE document_spooler
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'drivers'
PRINT @Table_name
DELETE drivers
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'dtproperties'
PRINT @Table_name
DELETE dtproperties
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_aviation'
PRINT @Table_name
DELETE event_aviation
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Event_Claim'
PRINT @Table_name
DELETE dtproperties
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_combined_liability'
PRINT @Table_name
DELETE event_combined_liability
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_combined_motor'
PRINT @Table_name
DELETE event_combined_motor
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_commercial_combined'
PRINT @Table_name
DELETE event_commercial_combined
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_drivers'
PRINT @Table_name
DELETE event_drivers
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Farm'
PRINT @Table_name
DELETE event_Farm
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_household_buildings'
PRINT @Table_name
DELETE event_household_buildings
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_household_contents'
PRINT @Table_name
DELETE event_household_contents
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Event_Insurance_File'
PRINT @Table_name
DELETE Event_Insurance_File
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Event_Insurance_File_System'
PRINT @Table_name
DELETE Event_Insurance_File_System
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Event_Insurance_Folder'
PRINT @Table_name
DELETE Event_Insurance_Folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_log'
PRINT @Table_name
DELETE event_log
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_marine'
PRINT @Table_name
DELETE event_marine
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Event_Media'
PRINT @Table_name
DELETE Event_Media
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Event_Media_Type'
PRINT @Table_name
DELETE Event_Media_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Offices'
PRINT @Table_name
DELETE event_Offices
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Party'
PRINT @Table_name
DELETE event_Party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Party_Corporate_Client'
PRINT @Table_name
DELETE event_Party_Corporate_Client
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Party_Group_Client'
PRINT @Table_name
DELETE event_Party_Group_Client
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Party_Personal_Client'
PRINT @Table_name
DELETE event_Party_Personal_Client
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_personal_accident'
PRINT @Table_name
DELETE event_personal_accident
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Policy_coinsurers'
PRINT @Table_name
DELETE event_Policy_coinsurers
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Policy_fee'
PRINT @Table_name
DELETE event_Policy_fee
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_policy_narrative'
PRINT @Table_name
DELETE event_policy_narrative
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_Policy_relationship'
PRINT @Table_name
DELETE event_Policy_relationship
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_policy_shared_premiums'
PRINT @Table_name
DELETE event_policy_shared_premiums
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_private_motor'
PRINT @Table_name
DELETE event_private_motor
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_private_public_hire'
PRINT @Table_name
DELETE event_private_public_hire
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_property_owners'
PRINT @Table_name
DELETE event_property_owners
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_risk_claim'
PRINT @Table_name
DELETE event_risk_claim
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_shop'
PRINT @Table_name
DELETE event_shop
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_travel'
PRINT @Table_name
DELETE event_travel
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_user_defined_risk_data'
PRINT @Table_name
DELETE event_user_defined_risk_data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'event_vehicles'
PRINT @Table_name
DELETE event_vehicles
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Fac_Arrangement'
PRINT @Table_name
DELETE Fac_Arrangement
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Fac_Arrangement_Summary'
PRINT @Table_name
DELETE Fac_Arrangement_Summary
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Farm'
PRINT @Table_name
DELETE Farm
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Fee_amounts'
PRINT @Table_name
DELETE Fee_amounts
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Field'
PRINT @Table_name
DELETE Field
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Field_Usage'
PRINT @Table_name
DELETE Field_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Fields'
PRINT @Table_name
DELETE Fields
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Policy_Link'
PRINT @Table_name
DELETE GIS_Policy_Link
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Policy_Schemes_Sel'
PRINT @Table_name
DELETE GIS_Policy_Schemes_Sel
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Scheme_Audit'
PRINT @Table_name
DELETE GIS_Scheme_Audit
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'household_buildings'
PRINT @Table_name
DELETE household_buildings
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'household_contents'
PRINT @Table_name
DELETE household_contents
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_Block_Usage'
PRINT @Table_name
DELETE Ins_File_Block_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_Extra_Value'
PRINT @Table_name
DELETE Ins_File_Extra_Value
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_List_Usage'
PRINT @Table_name
DELETE Ins_File_List_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_Private_Text'
PRINT @Table_name
DELETE Ins_File_Private_Text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_Public_Text'
PRINT @Table_name
DELETE Ins_File_Public_Text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_RI_Arrangement'
PRINT @Table_name
DELETE Ins_File_RI_Arrangement
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_Tax_Band'
PRINT @Table_name
DELETE Ins_File_Tax_Band
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Ins_File_Tax_Value'
PRINT @Table_name
DELETE Ins_File_Tax_Value
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Insurance_File'
PRINT @Table_name
DELETE Insurance_File
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'insurance_file_agent'
PRINT @Table_name
DELETE insurance_file_agent
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'insurance_file_risk_link'
PRINT @Table_name
DELETE insurance_file_risk_link
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Insurance_File_System'
PRINT @Table_name
DELETE Insurance_File_System
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'insurance_file_tax'
PRINT @Table_name
DELETE insurance_file_tax
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Insurance_Folder'
PRINT @Table_name
DELETE Insurance_Folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Insurer_group_rate'
PRINT @Table_name
DELETE Insurer_group_rate
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Insurer_rate'
PRINT @Table_name
DELETE Insurer_rate
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'IPT'
PRINT @Table_name
DELETE IPT
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'IPT_Extras'
PRINT @Table_name
DELETE IPT_Extras
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'JW1_Fire'
PRINT @Table_name
DELETE JW1_Fire
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'JW1_Household'
PRINT @Table_name
DELETE JW1_Household
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'JW1_Motor'
PRINT @Table_name
DELETE JW1_Motor
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'JW1_Policy_binder'
PRINT @Table_name
DELETE JW1_Policy_binder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'JW1_sum_insured'
PRINT @Table_name
DELETE JW1_sum_insured
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Last_Print_Run'
PRINT @Table_name
DELETE Last_Print_Run
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Lead_Commission'
PRINT @Table_name
DELETE Lead_Commission
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'lifestyle_category'
PRINT @Table_name
DELETE lifestyle_category
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'List_Record_Usage'
PRINT @Table_name
DELETE List_Record_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Locator_Type'
PRINT @Table_name
DELETE Locator_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Main_Event'
PRINT @Table_name
DELETE Main_Event
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Main_Event_Type'
PRINT @Table_name
DELETE Main_Event_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'marine'
PRINT @Table_name
DELETE marine
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Marketing_Code'
PRINT @Table_name
DELETE Marketing_Code
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'MTA'
PRINT @Table_name
DELETE MTA
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'MTA_Text'
PRINT @Table_name
DELETE MTA_Text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'MTA_Type'
PRINT @Table_name
DELETE MTA_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'MTA_Type_Usage'
PRINT @Table_name
DELETE MTA_Type_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Offices'
PRINT @Table_name
DELETE Offices
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party'
PRINT @Table_name
DELETE party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_account_handler'
PRINT @Table_name
DELETE party_account_handler
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Addr_Dtls_Temp'
PRINT @Table_name
DELETE Party_Addr_Dtls_Temp
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Address_Usage'
PRINT @Table_name
DELETE Party_Address_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Agent'
PRINT @Table_name
DELETE Party_Agent
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Block_Usage'
PRINT @Table_name
DELETE Party_Block_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_business'
PRINT @Table_name
DELETE party_business
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_category'
PRINT @Table_name
DELETE party_category
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Claim'
PRINT @Table_name
DELETE Party_Claim
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_consultant'
PRINT @Table_name
DELETE party_consultant
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Contact_Usage'
PRINT @Table_name
DELETE Party_Contact_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_conviction'
PRINT @Table_name
DELETE party_conviction
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Corporate_Client'
PRINT @Table_name
DELETE Party_Corporate_Client
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Group_Client'
PRINT @Table_name
DELETE Party_Group_Client
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Group_Type'
PRINT @Table_name
DELETE Party_Group_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_insurer'
PRINT @Table_name
DELETE party_insurer
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_lifestyle'
PRINT @Table_name
DELETE party_lifestyle
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_List_Usage'
PRINT @Table_name
DELETE Party_List_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Locator'
PRINT @Table_name
DELETE Party_Locator
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Marketing_Data'
PRINT @Table_name
DELETE Party_Marketing_Data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Net_Data'
PRINT @Table_name
DELETE Party_Net_Data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_other'
PRINT @Table_name
DELETE party_other
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Personal_Client'
PRINT @Table_name
DELETE Party_Personal_Client
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Private_Text'
PRINT @Table_name
DELETE Party_Private_Text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_prospect'
PRINT @Table_name
DELETE party_prospect
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Public_Text'
PRINT @Table_name
DELETE Party_Public_Text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Relationship'
PRINT @Table_name
DELETE Party_Relationship
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

--DELETING THIS TABLE GIVES AN ERROR WHEN TRYING TO ADD A CLIENT
--SELECT @Table_name = 'Party_Structure'
--DELETE Party_Structure
--IF @@ERROR <> 0
-- GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Supplier_Business'
PRINT @Table_name
DELETE Party_Supplier_Business
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Payment'
PRINT @Table_name
DELETE Payment
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Peril'
PRINT @Table_name
DELETE Peril
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Peril_Party'
PRINT @Table_name
DELETE Peril_Party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'personal_accident'
PRINT @Table_name
DELETE personal_accident
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMLock'
PRINT @Table_name
DELETE PMLock
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMManual_Rate'
PRINT @Table_name
DELETE PMManual_Rate
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMManual_Rate_Line'
PRINT @Table_name
DELETE PMManual_Rate_Line
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMManual_Rate_Type'
PRINT @Table_name
DELETE PMManual_Rate_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMNumber'
PRINT @Table_name
DELETE PMNumber
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMNumber_Group'
PRINT @Table_name
DELETE PMNumber_Group
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMNumber_Range'
PRINT @Table_name
DELETE PMNumber_Range
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMProc_Lock_Group'
PRINT @Table_name
DELETE PMProc_Lock_Group
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMReference_Field'
PRINT @Table_name
DELETE PMReference_Field
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMReference_Field_Order'
PRINT @Table_name
DELETE PMReference_Field_Order
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMReference_Type'
PRINT @Table_name
DELETE PMReference_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMUser_Authority_Level'
PRINT @Table_name
DELETE PMUser_Authority_Level
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'PMUser_Authority_Rule_Set_Link'
PRINT @Table_name
DELETE PMUser_Authority_Rule_Set_Link
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Policy_coinsurers'
PRINT @Table_name
DELETE Policy_coinsurers
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Policy_fee'
PRINT @Table_name
DELETE Policy_fee
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'policy_narrative'
PRINT @Table_name
DELETE policy_narrative
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Policy_relationship'
PRINT @Table_name
DELETE Policy_relationship
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'policy_sections'
PRINT @Table_name
DELETE policy_sections
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'policy_shared_premiums'
PRINT @Table_name
DELETE policy_shared_premiums
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'policy_standard_wording'
PRINT @Table_name
DELETE policy_standard_wording
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'previous_accidents'
PRINT @Table_name
DELETE previous_accidents
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'private_motor'
PRINT @Table_name
DELETE private_motor
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'private_public_hire'
PRINT @Table_name
DELETE private_public_hire
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'property_owners'
PRINT @Table_name
DELETE property_owners
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'prospect_campaign'
PRINT @Table_name
DELETE prospect_campaign
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'prospect_policy'
PRINT @Table_name
DELETE prospect_policy
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Queries'
PRINT @Table_name
DELETE Queries
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Rating_Section'
PRINT @Table_name
DELETE Rating_Section
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Receipt'
PRINT @Table_name
DELETE Receipt
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Recovery'
PRINT @Table_name
DELETE Recovery
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Renewal_report'
PRINT @Table_name
DELETE Renewal_report
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Renewal_Status'
PRINT @Table_name
DELETE Renewal_Status
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'report_audit_debit_table1'
PRINT @Table_name
DELETE report_audit_debit_table1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'report_audit_debit_table2'
PRINT @Table_name
DELETE report_audit_debit_table2
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'report_audit_debit_table3'
PRINT @Table_name
DELETE report_audit_debit_table3
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'report_audit_debit_table6'
PRINT @Table_name
DELETE report_audit_debit_table6
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'ReportPartyList'
PRINT @Table_name
DELETE ReportPartyList
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'ReportPolicyList'
PRINT @Table_name
DELETE ReportPolicyList
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Reserve'
PRINT @Table_name
DELETE Reserve
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RI_Arrangement_Line'
PRINT @Table_name
DELETE RI_Arrangement_Line
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk'
PRINT @Table_name
DELETE Risk
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Block_Usage'
PRINT @Table_name
DELETE Risk_Block_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_By_Source'
PRINT @Table_name
DELETE Risk_By_Source
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'risk_claim'
PRINT @Table_name
DELETE risk_claim
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_code'
PRINT @Table_name
DELETE Risk_code
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Extra_Value'
PRINT @Table_name
DELETE Risk_Extra_Value
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Folder'
PRINT @Table_name
DELETE Risk_Folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_group'
PRINT @Table_name
DELETE Risk_group
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_History'
PRINT @Table_name
DELETE Risk_History
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_List_Usage'
PRINT @Table_name
DELETE Risk_List_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Locator'
PRINT @Table_name
DELETE Risk_Locator
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Private_Text'
PRINT @Table_name
DELETE Risk_Private_Text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Public_Text'
PRINT @Table_name
DELETE Risk_Public_Text
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_RI_Arrangement'
PRINT @Table_name
DELETE Risk_RI_Arrangement
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_RI_Band'
PRINT @Table_name
DELETE Risk_RI_Band
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'risk_tax'
PRINT @Table_name
DELETE risk_tax
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Tax_Band'
PRINT @Table_name
DELETE Risk_Tax_Band
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Tax_Value'
PRINT @Table_name
DELETE Risk_Tax_Value
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Accident'
PRINT @Table_name
DELETE RSA_Accident
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_convictions'
PRINT @Table_name
DELETE RSA_convictions
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Core'
PRINT @Table_name
DELETE RSA_Core
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_driver'
PRINT @Table_name
DELETE RSA_driver
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_EMPLOYEE_LIST'
PRINT @Table_name
DELETE RSA_EMPLOYEE_LIST
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Engineering'
PRINT @Table_name
DELETE RSA_Engineering
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Financial'
PRINT @Table_name
DELETE RSA_Financial
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Fire'
PRINT @Table_name
DELETE RSA_Fire
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Glass_List'
PRINT @Table_name
DELETE RSA_Glass_List
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Glass_Pieces'
PRINT @Table_name
DELETE RSA_Glass_Pieces
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Household'
PRINT @Table_name
DELETE RSA_Household
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Household_buildings'
PRINT @Table_name
DELETE RSA_Household_buildings
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Household_contents'
PRINT @Table_name
DELETE RSA_Household_contents
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Household_High_Risk'
PRINT @Table_name
DELETE RSA_Household_High_Risk
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Household_Personal'
PRINT @Table_name
DELETE RSA_Household_Personal
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Marine'
PRINT @Table_name
DELETE RSA_Marine
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Motor'
PRINT @Table_name
DELETE RSA_Motor
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_motor_deductibles'
PRINT @Table_name
DELETE RSA_motor_deductibles
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Output'
PRINT @Table_name
DELETE RSA_Output
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Policy_binder'
PRINT @Table_name
DELETE RSA_Policy_binder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_RSA_Employee_list'
PRINT @Table_name
DELETE RSA_RSA_Employee_list
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_RSA_Theft_List'
PRINT @Table_name
DELETE RSA_RSA_Theft_List
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_Specials'
PRINT @Table_name
DELETE RSA_Specials
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_standard_wording'
PRINT @Table_name
DELETE RSA_standard_wording
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RSA_sum_insured'
PRINT @Table_name
DELETE RSA_sum_insured
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Save_RI_Arrangement_Line'
PRINT @Table_name
DELETE Save_RI_Arrangement_Line
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Save_Risk_RI_Arrangement'
PRINT @Table_name
DELETE Save_Risk_RI_Arrangement
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'shop'
PRINT @Table_name
DELETE shop
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'SIC_code'
PRINT @Table_name
DELETE SIC_code
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Stats_Detail'
PRINT @Table_name
DELETE Stats_Detail
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Stats_Folder'
PRINT @Table_name
DELETE Stats_Folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Sub_Commission_Band'
PRINT @Table_name
DELETE Sub_Commission_Band
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Sub_Commission_Party'
PRINT @Table_name
DELETE Sub_Commission_Party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Sub_Commission_Value'
PRINT @Table_name
DELETE Sub_Commission_Value
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Sub_Event'
PRINT @Table_name
DELETE Sub_Event
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Sub_Event_Type'
PRINT @Table_name
DELETE Sub_Event_Type
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Summary_Stats_Agent'
PRINT @Table_name
DELETE Summary_Stats_Agent
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Summary_Stats_Day'
PRINT @Table_name
DELETE Summary_Stats_Day
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Summary_Stats_Holder'
PRINT @Table_name
DELETE Summary_Stats_Holder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Summary_Stats_Month'
PRINT @Table_name
DELETE Summary_Stats_Month
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Summary_Stats_Premium'
PRINT @Table_name
DELETE Summary_Stats_Premium
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Tax_Rates'
PRINT @Table_name
DELETE Tax_Rates
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Tax_Type_Band'
PRINT @Table_name
DELETE Tax_Type_Band
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Temp_C_Aged_Balance'
PRINT @Table_name
DELETE Temp_C_Aged_Balance
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Temp_C_Statement'
PRINT @Table_name
DELETE Temp_C_Statement
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Transaction_Export_Detail'
PRINT @Table_name
DELETE Transaction_Export_Detail
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Transaction_Export_Folder'
PRINT @Table_name
DELETE Transaction_Export_Folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Transaction_options'
PRINT @Table_name
DELETE Transaction_options
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'travel'
PRINT @Table_name
DELETE travel
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Treaty_Party'
PRINT @Table_name
DELETE Treaty_Party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'User_Defined_Data_Link'
PRINT @Table_name
DELETE User_Defined_Data_Link
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'User_defined_peril_data'
PRINT @Table_name
DELETE User_defined_peril_data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'user_defined_risk_data'
PRINT @Table_name
DELETE user_defined_risk_data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Valid_Value'
PRINT @Table_name
DELETE Valid_Value
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Validation'
PRINT @Table_name
DELETE Validation
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Value_Caption'
PRINT @Table_name
DELETE Value_Caption
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Var_Data_Structure'
PRINT @Table_name
DELETE Var_Data_Structure
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Var_Data_Usage'
PRINT @Table_name
DELETE Var_Data_Usage
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'vehicles'
PRINT @Table_name
DELETE vehicles
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Claim'
PRINT @Table_name
DELETE work_Claim
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Claim_Expert_Service'
PRINT @Table_name
DELETE work_Claim_Expert_Service
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Claim_Party'
PRINT @Table_name
DELETE work_Claim_Party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Claim_Party_Claim'
PRINT @Table_name
DELETE work_Claim_Party_Claim
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_claim_party_link'
PRINT @Table_name
DELETE work_claim_party_link
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_claim_Peril'
PRINT @Table_name
DELETE work_claim_Peril
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Claim_Risk'
PRINT @Table_name
DELETE work_Claim_Risk
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Claim_user_defined_risk_data'
PRINT @Table_name
DELETE work_Claim_user_defined_risk_data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Payment'
PRINT @Table_name
DELETE work_Payment
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Peril_Party'
PRINT @Table_name
DELETE work_Peril_Party
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Receipt'
PRINT @Table_name
DELETE work_Receipt
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Recovery'
PRINT @Table_name
DELETE work_Recovery
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_Reserve'
PRINT @Table_name
DELETE work_Reserve
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Work_Stats_Detail'
PRINT @Table_name
DELETE Work_Stats_Detail
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Work_Stats_Folder'
PRINT @Table_name
DELETE Work_Stats_Folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Work_Transaction_Export_Detail'
PRINT @Table_name
DELETE Work_Transaction_Export_Detail
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Work_Transaction_Export_Folder'
PRINT @Table_name
DELETE Work_Transaction_Export_Folder
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'work_User_defined_peril_data'
PRINT @Table_name
DELETE work_User_defined_peril_data
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

--***********************************
--*                                 *
--* DELETE 'IS_DELETED' ENTRIES     *
--*                                 *
--***********************************

PRINT 'DELETE ALL ENTRIES FLAGGED AS IS_DELETED FROM FOLLOWING TABLES.'

SELECT @Table_name = 'Accum_Treatment_Type'
PRINT @Table_name
DELETE FROM Accum_Treatment_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Accumulation'
PRINT @Table_name
DELETE FROM Accumulation
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Address_Usage_Type'
PRINT @Table_name
DELETE FROM Address_usage_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Analysis_code'
PRINT @Table_name
DELETE FROM analysis_code
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Area'
PRINT @Table_name
DELETE FROM area
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Authority_Level_Type'
PRINT @Table_name
DELETE FROM authority_level_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Business_Type'
PRINT @Table_name
DELETE FROM business_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Claim_Cause'
PRINT @Table_name
DELETE FROM claim_cause
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Class_Of_Business'
PRINT @Table_name
DELETE FROM class_of_business
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Coi_Default'
PRINT @Table_name
DELETE FROM coi_default
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'coinsurance_treatment'
PRINT @Table_name
DELETE FROM coinsurance_treatment
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'commission_arrangement'
PRINT @Table_name
DELETE FROM commission_arrangement
WHERE is_deleted = 1
OR party_cnt = 0
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Contact_Type'
PRINT @Table_name
DELETE FROM Contact_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'department'
PRINT @Table_name
DELETE department
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'document_template'
PRINT @Table_name
DELETE FROM document_template
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'document_type'
PRINT @Table_name
DELETE FROM document_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Driver_Status'
PRINT @Table_name
DELETE driver_status
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'entity_type'
PRINT @Table_name
DELETE FROM entity_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Expert_Service'
PRINT @Table_name
DELETE FROM expert_service
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Extra_Type'
PRINT @Table_name
DELETE FROM extra_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Business_Type'
PRINT @Table_name
DELETE FROM GIS_Business_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Data_Model'
PRINT @Table_name
DELETE FROM GIS_Data_Model
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Insurer'
PRINT @Table_name
DELETE FROM GIS_Insurer
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Property'
PRINT @Table_name
DELETE FROM GIS_Property
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_QEM'
PRINT @Table_name
DELETE FROM GIS_QEM
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_Screen'
PRINT @Table_name
DELETE FROM GIS_Screen
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_user_def_detail'
PRINT @Table_name
DELETE FROM GIS_user_def_detail
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_user_def_header'
PRINT @Table_name
DELETE FROM GIS_user_def_header
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_user_def_header_inds'
PRINT @Table_name
DELETE FROM GIS_user_def_header_inds
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'GIS_user_def_header_rates'
PRINT @Table_name
DELETE FROM GIS_user_def_header_rates
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'index_linking'
PRINT @Table_name
DELETE FROM index_linking
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'index_linking_detail'
PRINT @Table_name
DELETE FROM index_linking_detail
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'License_type'
PRINT @Table_name
DELETE License_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Narrative_code'
PRINT @Table_name
DELETE FROM Narrative_code
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Nationality'
PRINT @Table_name
DELETE FROM Nationality
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'numbering_scheme'
PRINT @Table_name
DELETE FROM numbering_scheme
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'numbering_scheme_type'
PRINT @Table_name
DELETE FROM numbering_scheme_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Agent_Origin'
PRINT @Table_name
DELETE Party_Agent_Origin
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'party_agent_type'
PRINT @Table_name
DELETE party_agent_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Other_Posting_Type'
PRINT @Table_name
DELETE FROM Party_Other_Posting_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Party_Type'
PRINT @Table_name
DELETE FROM Party_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Peril_Group'
PRINT @Table_name
DELETE FROM Peril_Group
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Peril_Type'
PRINT @Table_name
DELETE FROM Peril_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Peril_Type_Analysis'
PRINT @Table_name
DELETE FROM Peril_Type_Analysis
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Policy_relationship_type'
PRINT @Table_name
DELETE FROM Policy_relationship_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Policy_Section_Type'
PRINT @Table_name
DELETE FROM Policy_Section_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'policy_type'
PRINT @Table_name
DELETE FROM policy_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Posting_Type'
PRINT @Table_name
DELETE FROM Posting_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Premium_Pro_Rata_Type'
PRINT @Table_name
DELETE FROM Premium_Pro_Rata_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'primary_cause'
PRINT @Table_name
DELETE FROM primary_cause
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Product'
PRINT @Table_name
DELETE FROM Product
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Product_Analysis'
PRINT @Table_name
DELETE FROM Product_Analysis
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'progress_status'
PRINT @Table_name
DELETE FROM progress_status
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'prospect_status'
PRINT @Table_name
DELETE FROM prospect_status
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

PRINT 'Resetting rate type on rating section'
UPDATE rating_section_type
SET rate_type_id = 12
WHERE rate_type_id = 8

SELECT @Table_name = 'rate_type'
PRINT @Table_name
DELETE FROM rate_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Rating_Section_Type'
PRINT @Table_name
DELETE FROM Rating_Section_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Record'
PRINT @Table_name
DELETE FROM Record
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Recovery_type'
PRINT @Table_name
DELETE FROM Recovery_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Reinsurance_type'
PRINT @Table_name
DELETE FROM Reinsurance_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Relationship_Type'
PRINT @Table_name
DELETE FROM Relationship_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Reminder_Type'
PRINT @Table_name
DELETE FROM Reminder_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Renewal_Frequency'
PRINT @Table_name
DELETE FROM Renewal_Frequency
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Renewal_Method'
PRINT @Table_name
DELETE FROM Renewal_Method
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Renewal_Process'
PRINT @Table_name
DELETE FROM Renewal_Process
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Renewal_Status_Type'
PRINT @Table_name
DELETE FROM Renewal_Status_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Renewal_stop_code'
PRINT @Table_name
DELETE FROM Renewal_stop_code
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Report'
PRINT @Table_name
DELETE FROM Report
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Report_Group'
PRINT @Table_name
DELETE FROM Report_Group
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'RI_Model'
PRINT @Table_name
DELETE FROM RI_Model
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Folder_Type'
PRINT @Table_name
DELETE FROM Risk_Folder_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Status'
PRINT @Table_name
DELETE FROM Risk_Status
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Type'
PRINT @Table_name
DELETE FROM Risk_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Risk_Type_Group'
PRINT @Table_name
DELETE FROM Risk_Type_Group
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'commission_band'
PRINT @Table_name
DELETE FROM commission_band
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'handler'
PRINT @Table_name
DELETE FROM handler
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Language'
PRINT @Table_name
DELETE FROM Language
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Lapsed_Reason'
PRINT @Table_name
DELETE FROM Lapsed_Reason
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'risk_type_rule_set'
PRINT @Table_name
DELETE FROM risk_type_rule_set
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Rule_Set'
PRINT @Table_name
DELETE FROM Rule_Set
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Seasonal_gift'
PRINT @Table_name
DELETE FROM Seasonal_gift
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'secondary_cause'
PRINT @Table_name
DELETE FROM secondary_cause
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Service_level'
PRINT @Table_name
DELETE FROM Service_level
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Service_type'
PRINT @Table_name
DELETE FROM Service_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'short_period_rates'
PRINT @Table_name
DELETE FROM short_period_rates
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Stats_Treatment_Type'
PRINT @Table_name
DELETE FROM Stats_Treatment_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Strength_code'
PRINT @Table_name
DELETE FROM Strength_code
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'sum_insured_type'
PRINT @Table_name
DELETE FROM sum_insured_type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Supplier_Business'
PRINT @Table_name
DELETE FROM Supplier_Business
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Supplier_Speciality'
PRINT @Table_name
DELETE FROM Supplier_Speciality
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Tax_Band'
PRINT @Table_name
DELETE FROM Tax_Band
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'tax_band_rate'
PRINT @Table_name
DELETE FROM tax_band_rate
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Tax_Type'
PRINT @Table_name
DELETE FROM Tax_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'town'
PRINT @Table_name
DELETE FROM town
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Transaction_Type'
PRINT @Table_name
DELETE FROM Transaction_Type
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

SELECT @Table_name = 'Treaty'
PRINT @Table_name
DELETE FROM Treaty
WHERE is_deleted = 1
IF @@ERROR <> 0
    GOTO Err_Tidy_Database

--***********************************
--* *
--* UPDATE DATA IN TABLES ONLY *
--* *
--***********************************

PRINT 'UPDATE ALL ENTRIES IN FOLLOWING TABLES.'

SELECT @Table_name = 'Next_Orion_Doc_Ref'
PRINT @Table_name
UPDATE next_orion_doc_ref
SET next_number = 10000001
WHERE next_number > 10000001

SELECT @Table_name = 'Update - numbering_scheme'
PRINT @Table_name
UPDATE numbering_scheme
SET next_number = 100001
WHERE next_number > 100001

--***********************************
--*                                 *
--* DELETE NOTHING FROM TABLE       *
--* OR NO IS_DELETED FIELD          *
--*                                 *
--***********************************

--GIS_LOOKUP_DATA

--GIS_LOOKUP_HEADER

--GIS_object

--allowed_risk_values

--Claim_Lookup

--Coi_Value

--Contact_Address_Usage

--Country

--Currency

--Event_Type

--Export_Map_Detail

--Export_Map_Format

--Export_Map_Model

--GIS_QEM_Usage

--GIS_Scheme

--GIS_Screen_detail

--GIS_user_def_detail_inds

--GIS_user_def_detail_rates

--Hidden_options

--insurance_file_status

--insurance_file_structure

--insurance_file_type

--Peril_data_Definition

--peril_type_reserve_type

--Peril_Type_Usage

--PMCaption

--PMUser

--Product_Allowed_Causation

--Product_RI_Model_Usage

--Product_Risk_Type_Group

--Renewal_Process_Status

--Renewal_Stats

--Report_Group_Contents

--Report_Group_User_Groups

--Reserve_type

--RI_Model_Line

--Risk_data_definition

--Risk_type_Expert_service

--Risk_Type_GIS_Screen

--Risk_type_Peril_type

--Risk_Type_RI_Model_Usage

--System_options

--Unique_Number

--wording_wording_link

--wording_risk_type_link

--Tmp_Risk

--Third_Party_Interest

--text_file_number

--text_file_description

--text_file

--Risk_Type_Usage

--Risk_Type_RI_Values

--risk_type_ri_properties

--source

--wp_fields

--***********************************
--*                                 *
--* END OF SCRIPT AND ERROR ROUTINE *
--*                                 *
--***********************************

IF @@ERROR = 0 BEGIN
    COMMIT TRAN
    EXECUTE DDLEnableIntegrity 1
    IF @@ERROR <> 0 BEGIN
        PRINT 'Failed to reactivate constraints'
        RETURN
    END
    PRINT 'Database clean up completed OK.'
END
RETURN

Err_Tidy_Database:
BEGIN
    PRINT 'Database Clearup Script Failed - Table name ' + @Table_name
    ROLLBACK
    EXECUTE DDLEnableIntegrity 1
    PRINT 'Database constraints reactivated'
END
GO
