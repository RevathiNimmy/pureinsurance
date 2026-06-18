EXECUTE DDLEnableIntegrity 0
GO

--***********************************
--*                                 *
--* DELETE ALL ENTRIES              *
--*                                 *
--***********************************

PRINT 'DELETE ALL ENTRIES FROM FOLLOWING TABLES.'
GO
PRINT 'abandoned_numbers'
DELETE abandoned_numbers
GO
PRINT 'accumulation_class'
DELETE accumulation_class
GO
PRINT 'accumulation_limit'
DELETE accumulation_limit
GO
PRINT 'accumulation_values'
DELETE accumulation_values
GO
PRINT 'Address'
DELETE Address
GO
PRINT 'agent_commission'
DELETE agent_commission
GO
PRINT 'Agent_group_rate'
DELETE agent_group_rate
GO
PRINT 'Agent_rate'
DELETE agent_rate
GO
PRINT 'Arc_Archive_Folder'
DELETE arc_archive_folder
GO
PRINT 'aviation'
DELETE aviation
GO
PRINT 'Block_Record_Usage'
DELETE block_record_usage
GO
PRINT 'Broking_companies'
DELETE broking_companies
GO
PRINT 'Broking_user'
DELETE broking_user
GO
PRINT 'campaign'
DELETE campaign
GO
PRINT 'Catastrophe_Code'
DELETE catastrophe_code
GO
PRINT 'Claim'
DELETE claim
GO
PRINT 'Claim_Address'
DELETE claim_address
GO
PRINT 'claim_conviction'
DELETE claim_conviction
GO
PRINT 'Claim_Expert_Service'
DELETE claim_expert_service
GO
PRINT 'Claim_Party'
DELETE claim_party
GO
PRINT 'Claim_Party_Claim'
DELETE claim_party_claim
GO
PRINT 'claim_party_link'
DELETE claim_party_link
GO
PRINT 'Claim_Party_type'
DELETE claim_party_type
GO
PRINT 'claim_Peril'
DELETE claim_peril
GO
PRINT 'Claim_Private_Text'
DELETE claim_private_text
GO
PRINT 'Claim_Public_Text'
DELETE claim_public_text
GO
PRINT 'Claim_Risk'
DELETE claim_risk
GO
PRINT 'Claim_Structure'
DELETE claim_structure
GO
PRINT 'Claim_user_defined_risk_data'
DELETE claim_user_defined_risk_data
GO
PRINT 'Class_Of_Business_Analysis'
DELETE Class_Of_Business_Analysis
GO
PRINT 'Client_product_link'
DELETE Client_product_link
GO
PRINT 'cobol_notes'
DELETE cobol_notes
GO
PRINT 'Coi_Arrangement'
DELETE Coi_Arrangement
GO
PRINT 'Coi_Compulsory_Value'
DELETE Coi_Compulsory_Value
GO
PRINT 'Collect_Type'
DELETE Collect_Type
GO
PRINT 'combined_liability'
DELETE combined_liability
GO
PRINT 'combined_motor'
DELETE combined_motor
GO
PRINT 'commercial_combined'
DELETE commercial_combined
GO
PRINT 'Contact'
DELETE Contact
GO
PRINT 'Contact_Address_Usage'
DELETE Contact_Address_Usage
GO
PRINT 'default_policy'
DELETE default_policy
GO
PRINT 'defined_risk_data'
DELETE defined_risk_data
GO
PRINT 'diary_action_code'
DELETE diary_action_code
GO
PRINT 'diary_life_cycle'
DELETE diary_life_cycle
GO
PRINT 'diary_life_cycle_docs'
DELETE diary_life_cycle_docs
GO
PRINT 'diary_task'
DELETE diary_task
GO
PRINT 'documents'
DELETE documents
GO
PRINT 'document_spooler'
DELETE document_spooler
GO
PRINT 'drivers'
DELETE drivers
GO
PRINT 'event_aviation'
DELETE event_aviation
GO
PRINT 'Event_Claim'
DELETE dtproperties
GO
PRINT 'event_combined_liability'
DELETE event_combined_liability
GO
PRINT 'event_combined_motor'
DELETE event_combined_motor
GO
PRINT 'event_commercial_combined'
DELETE event_commercial_combined
GO
PRINT 'event_drivers'
DELETE event_drivers
GO
PRINT 'event_Farm'
DELETE event_Farm
GO
PRINT 'event_household_buildings'
DELETE event_household_buildings
GO
PRINT 'event_household_contents'
DELETE event_household_contents
GO
PRINT 'Event_Insurance_File'
DELETE Event_Insurance_File
GO
PRINT 'Event_Insurance_File_System'
DELETE Event_Insurance_File_System
GO
PRINT 'Event_Insurance_Folder'
DELETE Event_Insurance_Folder
GO
PRINT 'event_log'
DELETE event_log
GO
PRINT 'event_marine'
DELETE event_marine
GO
PRINT 'Event_Media'
DELETE Event_Media
GO
PRINT 'Event_Media_Type'
DELETE Event_Media_Type
GO
PRINT 'event_Offices'
DELETE event_Offices
GO
PRINT 'event_Party'
DELETE event_Party
GO
PRINT 'event_Party_Corporate_Client'
DELETE event_Party_Corporate_Client
GO
PRINT 'event_Party_Group_Client'
DELETE event_Party_Group_Client
GO
PRINT 'event_Party_Personal_Client'
DELETE event_Party_Personal_Client
GO
PRINT 'event_personal_accident'
DELETE event_personal_accident
GO
PRINT 'event_Policy_coinsurers'
DELETE event_Policy_coinsurers
GO
PRINT 'event_Policy_fee'
DELETE event_Policy_fee
GO
PRINT 'event_policy_narrative'
DELETE event_policy_narrative
GO
PRINT 'event_Policy_relationship'
DELETE event_Policy_relationship
GO
PRINT 'event_policy_shared_premiums'
DELETE event_policy_shared_premiums
GO
PRINT 'event_private_motor'
DELETE event_private_motor
GO
PRINT 'event_private_public_hire'
DELETE event_private_public_hire
GO
PRINT 'event_property_owners'
DELETE event_property_owners
GO
PRINT 'event_risk_claim'
DELETE event_risk_claim
GO
PRINT 'event_shop'
DELETE event_shop
GO
PRINT 'event_travel'
DELETE event_travel
GO
PRINT 'event_user_defined_risk_data'
DELETE event_user_defined_risk_data
GO
PRINT 'event_vehicles'
DELETE event_vehicles
GO
PRINT 'Fac_Arrangement'
DELETE Fac_Arrangement
GO
PRINT 'Fac_Arrangement_Summary'
DELETE Fac_Arrangement_Summary
GO
PRINT 'Farm'
DELETE Farm
GO
PRINT 'Fee_amounts'
DELETE Fee_amounts
GO
PRINT 'Field'
DELETE Field
GO
PRINT 'Field_Usage'
DELETE Field_Usage
GO
PRINT 'Fields'
DELETE Fields
GO
PRINT 'GIS_Cobol_Linkage'
DELETE GIS_Cobol_Linkage
GO
PRINT 'household_buildings'
DELETE household_buildings
GO
PRINT 'household_contents'
DELETE household_contents
GO
PRINT 'Ins_File_Block_Usage'
DELETE Ins_File_Block_Usage
GO
PRINT 'Ins_File_Extra_Value'
DELETE Ins_File_Extra_Value
GO
PRINT 'Ins_File_List_Usage'
DELETE Ins_File_List_Usage
GO
PRINT 'Ins_File_Private_Text'
DELETE Ins_File_Private_Text
GO
PRINT 'Ins_File_Public_Text'
DELETE Ins_File_Public_Text
GO
PRINT 'Ins_File_RI_Arrangement'
DELETE Ins_File_RI_Arrangement
GO
PRINT 'Ins_File_Tax_Band'
DELETE Ins_File_Tax_Band
GO
PRINT 'Ins_File_Tax_Value'
DELETE Ins_File_Tax_Value
GO
PRINT 'Insurance_File'
DELETE Insurance_File
GO
PRINT 'insurance_file_agent'
DELETE insurance_file_agent
GO
PRINT 'insurance_file_risk_link'
DELETE insurance_file_risk_link
GO
PRINT 'Insurance_File_System'
DELETE Insurance_File_System
GO
PRINT 'insurance_file_tax'
DELETE insurance_file_tax
GO
PRINT 'Insurance_Folder'
DELETE Insurance_Folder
GO
PRINT 'Insurer_group_rate'
DELETE Insurer_group_rate
GO
PRINT 'Insurer_rate'
DELETE Insurer_rate
GO
PRINT 'JW1_Fire'
DELETE JW1_Fire
GO
PRINT 'JW1_Household'
DELETE JW1_Household
GO
PRINT 'JW1_Motor'
DELETE JW1_Motor
GO
PRINT 'JW1_Policy_binder'
DELETE JW1_Policy_binder
GO
PRINT 'JW1_sum_insured'
DELETE JW1_sum_insured
GO
PRINT 'Last_Print_Run'
DELETE Last_Print_Run
GO
PRINT 'Lead_Commission'
DELETE Lead_Commission
GO
PRINT 'Lists'
DELETE Lists
GO
PRINT 'List_custom'
DELETE List_custom
GO
PRINT 'List_user'
DELETE List_user
GO
PRINT 'List_Record_Usage'
DELETE List_Record_Usage
GO
PRINT 'Locator_Type'
DELETE Locator_Type
GO
PRINT 'Main_Event'
DELETE Main_Event
GO
PRINT 'Main_Event_Type'
DELETE Main_Event_Type
GO
PRINT 'marine'
DELETE marine
GO
PRINT 'MTA'
DELETE MTA
GO
PRINT 'MTA_Text'
DELETE MTA_Text
GO
PRINT 'MTA_Type'
DELETE MTA_Type
GO
PRINT 'MTA_Type_Usage'
DELETE MTA_Type_Usage
GO
PRINT 'Offices'
DELETE Offices
GO
PRINT 'Party'
DELETE party
GO
PRINT 'party_account_handler'
DELETE party_account_handler
GO
PRINT 'Party_Addr_Dtls_Temp'
DELETE Party_Addr_Dtls_Temp
GO
PRINT 'Party_Address_Usage'
DELETE Party_Address_Usage
GO
PRINT 'Party_Agent'
DELETE Party_Agent
GO
PRINT 'Party_Block_Usage'
DELETE Party_Block_Usage
GO
PRINT 'party_business'
DELETE party_business
GO
PRINT 'party_category'
DELETE party_category
GO
PRINT 'Party_Claim'
DELETE Party_Claim
GO
PRINT 'party_consultant'
DELETE party_consultant
GO
PRINT 'Party_Contact_Usage'
DELETE Party_Contact_Usage
GO
PRINT 'party_conviction'
DELETE party_conviction
GO
PRINT 'Party_Corporate_Client'
DELETE Party_Corporate_Client
GO
PRINT 'Party_Group_Client'
DELETE Party_Group_Client
GO
PRINT 'Party_Group_Type'
DELETE Party_Group_Type
GO
PRINT 'party_insurer'
DELETE party_insurer
GO
PRINT 'party_lifestyle'
DELETE party_lifestyle
GO
PRINT 'Party_List_Usage'
DELETE Party_List_Usage
GO
PRINT 'Party_Locator'
DELETE Party_Locator
GO
PRINT 'Party_Marketing_Data'
DELETE Party_Marketing_Data
GO
PRINT 'Party_Net_Data'
DELETE Party_Net_Data
GO
PRINT 'party_other'
DELETE party_other
GO
PRINT 'Party_Personal_Client'
DELETE Party_Personal_Client
GO
PRINT 'Party_Private_Text'
DELETE Party_Private_Text
GO
PRINT 'party_prospect'
DELETE party_prospect
GO
PRINT 'Party_Public_Text'
DELETE Party_Public_Text
GO
PRINT 'Party_Relationship'
DELETE Party_Relationship
GO
PRINT 'Party_Supplier_Business'
DELETE Party_Supplier_Business
GO
PRINT 'Payment'
DELETE Payment
GO
PRINT 'pay_screen_type'
DELETE pay_screen_type
GO
PRINT 'Peril'
DELETE Peril
GO
PRINT 'Peril_Party'
DELETE Peril_Party
GO
PRINT 'personal_accident'
DELETE personal_accident
GO
PRINT 'PMLock'
DELETE PMLock
GO
PRINT 'PMManual_Rate'
DELETE PMManual_Rate
GO
PRINT 'PMManual_Rate_Line'
DELETE PMManual_Rate_Line
GO
PRINT 'PMManual_Rate_Type'
DELETE PMManual_Rate_Type
GO
PRINT 'PMNumber'
DELETE PMNumber
GO
PRINT 'PMNumber_Group'
DELETE PMNumber_Group
GO
PRINT 'PMNumber_Range'
DELETE PMNumber_Range
GO
PRINT 'PMProc_Lock_Group'
DELETE PMProc_Lock_Group
GO
PRINT 'PMReference_Field'
DELETE PMReference_Field
GO
PRINT 'PMReference_Field_Order'
DELETE PMReference_Field_Order
GO
PRINT 'PMReference_Type'
DELETE PMReference_Type
GO
PRINT 'PMUser_Authority_Level'
DELETE PMUser_Authority_Level
GO
PRINT 'PMUser_Authority_Rule_Set_Link'
DELETE PMUser_Authority_Rule_Set_Link
GO
PRINT 'Policy_agents'
DELETE Policy_agents
GO
PRINT 'Policy_coinsurers'
DELETE Policy_coinsurers
GO
PRINT 'Policy_fee'
DELETE Policy_fee
GO
PRINT 'policy_life_cycle'
DELETE policy_life_cycle
GO
PRINT 'policy_life_cycle_actions'
DELETE policy_life_cycle_actions
GO
PRINT 'policy_life_cycle_docs'
DELETE policy_life_cycle_docs
GO
PRINT 'policy_narrative'
DELETE policy_narrative
GO
PRINT 'Policy_relationship'
DELETE Policy_relationship
GO
PRINT 'policy_sections'
DELETE policy_sections
GO
PRINT 'policy_shared_premiums'
DELETE policy_shared_premiums
GO
PRINT 'policy_standard_wording'
DELETE policy_standard_wording
GO
PRINT 'previous_accidents'
DELETE previous_accidents
GO
PRINT 'private_motor'
DELETE private_motor
GO
PRINT 'private_public_hire'
DELETE private_public_hire
GO
PRINT 'property_owners'
DELETE property_owners
GO
PRINT 'prospect_campaign'
DELETE prospect_campaign
GO
PRINT 'prospect_policy'
DELETE prospect_policy
GO
PRINT 'Queries'
DELETE Queries
GO
PRINT 'Rating_Section'
DELETE Rating_Section
GO
PRINT 'Receipt'
DELETE Receipt
GO
PRINT 'Recovery'
DELETE Recovery
GO
PRINT 'Renewal_report'
DELETE Renewal_report
GO
PRINT 'Renewal_Status'
DELETE Renewal_Status
GO
PRINT 'report_audit_debit_table1'
DELETE report_audit_debit_table1
GO
PRINT 'report_audit_debit_table2'
DELETE report_audit_debit_table2
GO
PRINT 'report_audit_debit_table3'
DELETE report_audit_debit_table3
GO
PRINT 'report_audit_debit_table6'
DELETE report_audit_debit_table6
GO
PRINT 'ReportPartyList'
DELETE ReportPartyList
GO
PRINT 'ReportPolicyList'
DELETE ReportPolicyList
GO
PRINT 'Reserve'
DELETE Reserve
GO
PRINT 'RI_Arrangement_Line'
DELETE RI_Arrangement_Line
GO
PRINT 'Risk'
DELETE Risk
GO
PRINT 'Risk_Block_Usage'
DELETE Risk_Block_Usage
GO
PRINT 'risk_claim'
DELETE risk_claim
GO
PRINT 'Risk_Extra_Value'
DELETE Risk_Extra_Value
GO
PRINT 'Risk_Folder'
DELETE Risk_Folder
GO
PRINT 'Risk_History'
DELETE Risk_History
GO
PRINT 'Risk_List_Usage'
DELETE Risk_List_Usage
GO
PRINT 'Risk_Locator'
DELETE Risk_Locator
GO
PRINT 'Risk_Private_Text'
DELETE Risk_Private_Text
GO
PRINT 'Risk_Public_Text'
DELETE Risk_Public_Text
GO
PRINT 'Risk_RI_Arrangement'
DELETE Risk_RI_Arrangement
GO
PRINT 'Risk_RI_Band'
DELETE Risk_RI_Band
GO
PRINT 'risk_tax'
DELETE risk_tax
GO
PRINT 'Risk_Tax_Band'
DELETE Risk_Tax_Band
GO
PRINT 'Risk_Tax_Value'
DELETE Risk_Tax_Value
GO
PRINT 'RSA_Accident'
DELETE RSA_Accident
GO
PRINT 'RSA_convictions'
DELETE RSA_convictions
GO
PRINT 'RSA_Core'
DELETE RSA_Core
GO
PRINT 'RSA_driver'
DELETE RSA_driver
GO
PRINT 'RSA_EMPLOYEE_LIST'
DELETE RSA_EMPLOYEE_LIST
GO
PRINT 'RSA_Engineering'
DELETE RSA_Engineering
GO
PRINT 'RSA_Financial'
DELETE RSA_Financial
GO
PRINT 'RSA_Fire'
DELETE RSA_Fire
GO
PRINT 'RSA_Glass_List'
DELETE RSA_Glass_List
GO
PRINT 'RSA_Glass_Pieces'
DELETE RSA_Glass_Pieces
GO
PRINT 'RSA_Household'
DELETE RSA_Household
GO
PRINT 'RSA_Household_buildings'
DELETE RSA_Household_buildings
GO
PRINT 'RSA_Household_contents'
DELETE RSA_Household_contents
GO
PRINT 'RSA_Household_High_Risk'
DELETE RSA_Household_High_Risk
GO
PRINT 'RSA_Household_Personal'
DELETE RSA_Household_Personal
GO
PRINT 'RSA_Marine'
DELETE RSA_Marine
GO
PRINT 'RSA_Motor'
DELETE RSA_Motor
GO
PRINT 'RSA_motor_deductibles'
DELETE RSA_motor_deductibles
GO
PRINT 'RSA_Output'
DELETE RSA_Output
GO
PRINT 'RSA_Policy_binder'
DELETE RSA_Policy_binder
GO
PRINT 'RSA_RSA_Employee_list'
DELETE RSA_RSA_Employee_list
GO
PRINT 'RSA_RSA_Theft_List'
DELETE RSA_RSA_Theft_List
GO
PRINT 'RSA_Specials'
DELETE RSA_Specials
GO
PRINT 'RSA_standard_wording'
DELETE RSA_standard_wording
GO
PRINT 'RSA_sum_insured'
DELETE RSA_sum_insured
GO
PRINT 'Save_RI_Arrangement_Line'
DELETE Save_RI_Arrangement_Line
GO
PRINT 'Save_Risk_RI_Arrangement'
DELETE Save_Risk_RI_Arrangement
GO
PRINT 'shop'
DELETE shop
GO
PRINT 'Stats_Detail'
DELETE Stats_Detail
GO
PRINT 'Stats_Folder'
DELETE Stats_Folder
GO
PRINT 'Sub_Commission_Band'
DELETE Sub_Commission_Band
GO
PRINT 'Sub_Commission_Party'
DELETE Sub_Commission_Party
GO
PRINT 'Sub_Commission_Value'
DELETE Sub_Commission_Value
GO
PRINT 'Sub_Event'
DELETE Sub_Event
GO
PRINT 'Sub_Event_Type'
DELETE Sub_Event_Type
GO
PRINT 'Summary_Stats_Agent'
DELETE Summary_Stats_Agent
GO
PRINT 'Summary_Stats_Day'
DELETE Summary_Stats_Day
GO
PRINT 'Summary_Stats_Holder'
DELETE Summary_Stats_Holder
GO
PRINT 'Summary_Stats_Month'
DELETE Summary_Stats_Month
GO
PRINT 'Summary_Stats_Premium'
DELETE Summary_Stats_Premium
GO
PRINT 'Tax_Rates'
DELETE Tax_Rates
GO
PRINT 'Temp_C_Aged_Balance'
DELETE Temp_C_Aged_Balance
GO
PRINT 'Temp_C_Statement'
DELETE Temp_C_Statement
GO
PRINT 'test_linkage'
DELETE test_linkage
GO
PRINT 'Transaction_Export_Detail'
DELETE Transaction_Export_Detail
GO
PRINT 'Transaction_Export_Folder'
DELETE Transaction_Export_Folder
GO
PRINT 'Transaction_options'
DELETE Transaction_options
GO
PRINT 'travel'
DELETE travel
GO
PRINT 'Treaty_Party'
DELETE Treaty_Party
GO
PRINT 'User_Defined_Data_Link'
DELETE User_Defined_Data_Link
GO
PRINT 'User_defined_peril_data'
DELETE User_defined_peril_data
GO
PRINT 'user_defined_risk_data'
DELETE user_defined_risk_data
GO
PRINT 'Valid_Value'
DELETE Valid_Value
GO
PRINT 'Validation'
DELETE Validation
GO
PRINT 'Value_Caption'
DELETE Value_Caption
GO
PRINT 'Var_Data_Structure'
DELETE Var_Data_Structure
GO
PRINT 'Var_Data_Usage'
DELETE Var_Data_Usage
GO
PRINT 'vehicles'
DELETE vehicles
GO
PRINT 'work_Claim'
DELETE work_Claim
GO
PRINT 'work_Claim_Expert_Service'
DELETE work_Claim_Expert_Service
GO
PRINT 'work_Claim_Party'
DELETE work_Claim_Party
GO
PRINT 'work_Claim_Party_Claim'
DELETE work_Claim_Party_Claim
GO
PRINT 'work_claim_party_link'
DELETE work_claim_party_link
GO
PRINT 'work_claim_Peril'
DELETE work_claim_Peril
GO
PRINT 'work_Claim_Risk'
DELETE work_Claim_Risk
GO
PRINT 'work_Claim_user_defined_risk_data'
DELETE work_Claim_user_defined_risk_data
GO
PRINT 'work_Payment'
DELETE work_Payment
GO
PRINT 'work_Peril_Party'
DELETE work_Peril_Party
GO
PRINT 'work_Receipt'
DELETE work_Receipt
GO
PRINT 'work_Recovery'
DELETE work_Recovery
GO
PRINT 'work_Reserve'
DELETE work_Reserve
GO
PRINT 'Work_Stats_Detail'
DELETE Work_Stats_Detail
GO
PRINT 'Work_Stats_Folder'
DELETE Work_Stats_Folder
GO
PRINT 'Work_Transaction_Export_Detail'
DELETE Work_Transaction_Export_Detail
GO
PRINT 'Work_Transaction_Export_Folder'
DELETE Work_Transaction_Export_Folder
GO
PRINT 'work_User_defined_peril_data'
DELETE work_User_defined_peril_data
GO
PRINT 'wrk_scheme_properties'
DELETE wrk_scheme_properties
GO

--***********************************
--*                                 *
--* DELETE 'IS_DELETED' ENTRIES     *
--*                                 *
--***********************************

PRINT 'DELETE ALL ENTRIES FLAGGED AS IS_DELETED FROM FOLLOWING TABLES.'
GO
PRINT 'Accum_Treatment_Type'
DELETE Accum_Treatment_Type
WHERE is_deleted = 1
GO
PRINT 'Address_Usage_Type'
DELETE Address_usage_type
WHERE is_deleted = 1
GO
PRINT 'Analysis_code'
DELETE analysis_code
WHERE is_deleted = 1
GO
PRINT 'Area'
DELETE area
WHERE is_deleted = 1
GO
PRINT 'Authority_Level_Type'
DELETE authority_level_type
WHERE is_deleted = 1
GO
PRINT 'branch'
DELETE branch
WHERE is_deleted = 1
GO
PRINT 'Business_Type'
DELETE business_type
WHERE is_deleted = 1
GO
PRINT 'Claim_Cause'
DELETE claim_cause
WHERE is_deleted = 1
GO
PRINT 'Class_Of_Business'
DELETE class_of_business
WHERE is_deleted = 1
GO
PRINT 'Coi_Default'
DELETE coi_default
WHERE is_deleted = 1
GO
PRINT 'coinsurance_treatment'
DELETE coinsurance_treatment
WHERE is_deleted = 1
GO
PRINT 'commission_arrangement'
DELETE commission_arrangement
WHERE is_deleted = 1
OR party_cnt = 0
GO
PRINT 'commission_band'
DELETE commission_band
WHERE is_deleted = 1
GO
PRINT 'Contact_Type'
DELETE Contact_Type
WHERE is_deleted = 1
GO
PRINT 'department'
DELETE department
WHERE is_deleted = 1
GO
PRINT 'document_template'
DELETE document_template
WHERE is_deleted = 1
GO
PRINT 'document_type'
DELETE document_type
WHERE is_deleted = 1
GO
PRINT 'Driver_Status'
DELETE driver_status
WHERE is_deleted = 1
GO
PRINT 'entity_type'
DELETE entity_type
WHERE is_deleted = 1
GO
PRINT 'Expert_Service'
DELETE expert_service
WHERE is_deleted = 1
GO
PRINT 'Extra_Type'
DELETE extra_type
WHERE is_deleted = 1
GO
PRINT 'GIS_Business_Type'
DELETE GIS_Business_Type
WHERE is_deleted = 1
GO
PRINT 'GIS_Insurer'
DELETE GIS_Insurer
WHERE is_deleted = 1
GO
PRINT 'GIS_QEM'
DELETE GIS_QEM
WHERE is_deleted = 1
GO
PRINT 'GIS_user_def_detail'
DELETE GIS_user_def_detail
WHERE is_deleted = 1
GO
PRINT 'GIS_user_def_header'
DELETE GIS_user_def_header
WHERE is_deleted = 1
GO
PRINT 'GIS_user_def_header_inds'
DELETE GIS_user_def_header_inds
WHERE is_deleted = 1
GO
PRINT 'GIS_user_def_header_rates'
DELETE GIS_user_def_header_rates
WHERE is_deleted = 1
GO
PRINT 'handler'
DELETE handler
WHERE is_deleted = 1
GO
PRINT 'index_linking'
DELETE index_linking
WHERE is_deleted = 1
GO
PRINT 'index_linking_detail'
DELETE index_linking_detail
WHERE is_deleted = 1
GO
PRINT 'Lapsed_Reason'
DELETE Lapsed_Reason
WHERE is_deleted = 1
GO
PRINT 'License_type'
DELETE License_type
WHERE is_deleted = 1
GO
PRINT 'lifestyle_category'
DELETE lifestyle_category
WHERE is_deleted = 1
GO
PRINT 'Marketing_Code'
DELETE Marketing_Code
WHERE is_deleted = 1
GO
PRINT 'Narrative_code'
DELETE Narrative_code
WHERE is_deleted = 1
GO
PRINT 'Nationality'
DELETE Nationality
WHERE is_deleted = 1
GO
PRINT 'numbering_scheme'
DELETE numbering_scheme
WHERE is_deleted = 1
GO
PRINT 'numbering_scheme_type'
DELETE numbering_scheme_type
WHERE is_deleted = 1
GO
PRINT 'Party_Agent_Origin'
DELETE Party_Agent_Origin
WHERE is_deleted = 1
GO
PRINT 'party_agent_type'
DELETE party_agent_type
WHERE is_deleted = 1
GO
PRINT 'Party_Other_Posting_Type'
DELETE Party_Other_Posting_Type
WHERE is_deleted = 1
GO
PRINT 'Party_Type'
DELETE Party_Type
WHERE is_deleted = 1
GO
PRINT 'Peril_Group'
DELETE Peril_Group
WHERE is_deleted = 1
GO
PRINT 'Peril_Type'
DELETE Peril_Type
WHERE is_deleted = 1
GO
PRINT 'Peril_Type_Analysis'
DELETE Peril_Type_Analysis
WHERE is_deleted = 1
GO
PRINT 'Policy_relationship_type'
DELETE Policy_relationship_type
WHERE is_deleted = 1
GO
PRINT 'Policy_Section_Type'
DELETE Policy_Section_Type
WHERE is_deleted = 1
GO
PRINT 'policy_type'
DELETE policy_type
WHERE is_deleted = 1
GO
PRINT 'Posting_Type'
DELETE Posting_Type
WHERE is_deleted = 1
GO
PRINT 'Premium_Pro_Rata_Type'
DELETE Premium_Pro_Rata_Type
WHERE is_deleted = 1
GO
PRINT 'primary_cause'
DELETE primary_cause
WHERE is_deleted = 1
GO
PRINT 'Product'
DELETE Product
WHERE is_deleted = 1
GO
PRINT 'Product_Analysis'
DELETE Product_Analysis
WHERE is_deleted = 1
GO
PRINT 'progress_status'
DELETE progress_status
WHERE is_deleted = 1
GO
PRINT 'prospect_status'
DELETE prospect_status
WHERE is_deleted = 1
GO
PRINT 'Resetting rate type on rating section'
UPDATE rating_section_type
SET rate_type_id = 12
WHERE rate_type_id = 8
GO
PRINT 'rate_type'
DELETE rate_type
WHERE is_deleted = 1
GO
PRINT 'Rating_Section_Type'
DELETE Rating_Section_Type
WHERE is_deleted = 1
GO
PRINT 'Record'
DELETE Record
WHERE is_deleted = 1
GO
PRINT 'Recovery_type'
DELETE Recovery_type
WHERE is_deleted = 1
GO
PRINT 'Reinsurance_type'
DELETE Reinsurance_type
WHERE is_deleted = 1
GO
PRINT 'Relationship_Type'
DELETE Relationship_Type
WHERE is_deleted = 1
GO
PRINT 'Reminder_Type'
DELETE Reminder_Type
WHERE is_deleted = 1
GO
PRINT 'Renewal_Frequency'
DELETE Renewal_Frequency
WHERE is_deleted = 1
GO
PRINT 'Renewal_Method'
DELETE Renewal_Method
WHERE is_deleted = 1
GO
PRINT 'Renewal_Process'
DELETE Renewal_Process
WHERE is_deleted = 1
GO
PRINT 'Renewal_Status_Type'
DELETE Renewal_Status_Type
WHERE is_deleted = 1
GO
PRINT 'Renewal_stop_code'
DELETE Renewal_stop_code
WHERE is_deleted = 1
GO
PRINT 'Report'
DELETE Report
WHERE is_deleted = 1
GO
PRINT 'Report_Group'
DELETE Report_Group
WHERE is_deleted = 1
GO
PRINT 'RI_Model'
DELETE RI_Model
WHERE is_deleted = 1
GO
PRINT 'Risk_code'
DELETE Risk_code
WHERE is_deleted = 1
GO
PRINT 'Risk_Folder_Type'
DELETE Risk_Folder_Type
WHERE is_deleted = 1
GO
PRINT 'Risk_group'
DELETE Risk_group
WHERE is_deleted = 1
GO
PRINT 'Risk_Status'
DELETE Risk_Status
WHERE is_deleted = 1
GO
PRINT 'Risk_Type'
DELETE Risk_Type
WHERE is_deleted = 1
GO
PRINT 'Risk_Type_Group'
DELETE Risk_Type_Group
WHERE is_deleted = 1
GO
PRINT 'risk_type_rule_set'
DELETE risk_type_rule_set
WHERE is_deleted = 1
GO
PRINT 'Rule_Set'
DELETE Rule_Set
WHERE is_deleted = 1
GO
PRINT 'Seasonal_gift'
DELETE Seasonal_gift
WHERE is_deleted = 1
GO
PRINT 'secondary_cause'
DELETE secondary_cause
WHERE is_deleted = 1
GO
PRINT 'Service_level'
DELETE Service_level
WHERE is_deleted = 1
GO
PRINT 'Service_type'
DELETE Service_type
WHERE is_deleted = 1
GO
PRINT 'SIC_code'
DELETE SIC_code
WHERE is_deleted = 1
GO
PRINT 'short_period_rates'
DELETE short_period_rates
WHERE is_deleted = 1
GO
PRINT 'Stats_Treatment_Type'
DELETE Stats_Treatment_Type
WHERE is_deleted = 1
GO
PRINT 'Strength_code'
DELETE Strength_code
WHERE is_deleted = 1
GO
PRINT 'sum_insured_type'
DELETE sum_insured_type
WHERE is_deleted = 1
GO
PRINT 'Supplier_Business'
DELETE Supplier_Business
WHERE is_deleted = 1
GO
PRINT 'Supplier_Speciality'
DELETE Supplier_Speciality
WHERE is_deleted = 1
GO
PRINT 'Tax_Band'
DELETE Tax_Band
WHERE is_deleted = 1
GO
PRINT 'tax_band_rate'
DELETE tax_band_rate
WHERE is_deleted = 1
GO
PRINT 'Tax_Type'
DELETE Tax_Type
WHERE is_deleted = 1
GO
PRINT 'town'
DELETE town
WHERE is_deleted = 1
GO
PRINT 'Treaty'
DELETE Treaty
WHERE is_deleted = 1
GO

--***********************************
--*                                 *
--* UPDATE DATA IN TABLES ONLY      *
--*                                 *
--***********************************

PRINT 'UPDATE ALL ENTRIES IN FOLLOWING TABLES.'
GO
PRINT 'Next_Orion_Doc_Ref'
UPDATE next_orion_doc_ref
SET next_number = 10000001
WHERE next_number > 10000001
GO
PRINT 'numbering_scheme'
UPDATE numbering_scheme
SET next_number = 100001
WHERE next_number > 100001
GO

--***********************************
--*                                 *
--* END OF SCRIPT AND ERROR ROUTINE *
--*                                 *
--***********************************

EXECUTE DDLEnableIntegrity 1
GO
