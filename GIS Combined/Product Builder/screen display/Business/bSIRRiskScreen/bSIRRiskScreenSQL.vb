Option Strict Off
Option Explicit On
'developer guide no. 129
Module BusinessSQL
    '******************************************************************************
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    '******************************************************************************
    '

    '******************************************************************************
    ' Class Name: BusinessSQL
    '
    ' Date: 07/05/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRRiskScreen.Business class.
    '
    ' Edit History:
    '******************************************************************************

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select Data Dictionary SQL
    Public Const ACGetDataDictionaryStored As Boolean = True
    Public Const ACGetDataDictionaryName As String = "SelectDataDictionary"
    Public Const ACGetDataDictionarySQL As String = "spu_full_data_dictionary_sel"

    ' Select Screen Details SQL
    Public Const ACGetAllScreenDetailsStored As Boolean = True
    Public Const ACGetAllScreenDetailsName As String = "SelectAllScreenDetails"
    Public Const ACGetAllScreenDetailsSQL As String = "spu_GIS_screen_detail_extra_saa"

    ' Select Child Screen Details SQL
    Public Const ACGetAllChildScreenDetailsStored As Boolean = True
    Public Const ACGetAllChildScreenDetailsName As String = "SelectAllChildScreenDetails"
    Public Const ACGetAllChildScreenDetailsSQL As String = "spu_GIS_child_screen_detail_saa"


    ' Select Party SQL
    Public Const ACGetPartyStored As Boolean = False
    Public Const ACGetPartyName As String = "SelectParty"
    Public Const ACGetPartySQL As String = ""

    ' Select Policy SQL
    Public Const ACGetPolicyStored As Boolean = False
    Public Const ACGetPolicyName As String = "SelectPolicy"
    Public Const ACGetPolicySQL As String = ""

    ' Select Lookup SQL
    Public Const ACGetLookupStored As Boolean = False
    Public Const ACGetLookupName As String = "SelectLookup"
    Public Const ACGetLookupSQL As String = ""

    ' Select UserDef SQL
    Public Const ACGetUserDefStored As Boolean = False
    Public Const ACGetUserDefName As String = "SelectUserDef"
    Public Const ACGetUserDefSQL As String = ""

    ' Get The Binder SQL
    Public Const ACGetBinderStored As Boolean = False
    Public Const ACGetBinderName As String = "SelectBinder"
    Public Const ACGetBinderSQL As String = "SELECT {data_model}_policy_binder_id " &
                                            "FROM {data_model}_policy_binder " &
                                            "WHERE gis_policy_link_id = {policy_link_id}"

    ' Get The Object Keys SQL
    Public Const ACGetObjectKeysStored As Boolean = False
    Public Const ACGetObjectKeysName As String = "SelectObjectKeys"
    Public Const ACGetObjectKeysSQL As String = "SELECT column_name " &
                                                "FROM GIS_property p, GIS_object o " &
                                                "WHERE o.object_name = {object_name} " &
                                                "AND o.gis_object_id = p.gis_object_id " &
                                                "AND p.is_primary_key = 1" &
                                                "AND o.gis_data_model_id= {datamodel_id}" &
                                                "ORDER BY p.gis_property_id"


    ' Get The Data Model SQL
    'Public Const ACGetGISDataModelStored = False
    Public Const ACGetGISDataModelStored As Boolean = True
    Public Const ACGetGISDataModelName As String = "SelectGISDataModel"
    Public Const ACGetGISDataModelSQL As String = "spu_data_model_from_screen"

    ' Select Standard Wording SQL
    Public Const ACGetStandardWordingStored As Boolean = False
    Public Const ACGetStandardWordingName As String = "SelectStandardWording"
    Public Const ACGetStandardWordingSQL As String = "SELECT s.document_template_id, d.code, d.description, d.copy_of_original " &
                                                     "FROM {data_model}_standard_wording s, document_template d " &
                                                     "WHERE s.{data_model}_policy_binder_id = {policy_binder_id} " &
                                                     "AND s.gis_property_id= {gis_property_id} " &
                                                     "AND s.gis_object_id= {gis_object_id} " &
                                                     "AND s.document_template_id = d.document_template_id " &
                                                     "AND (s.child IS NULL or s.child = 0) " &
                                                     "ORDER BY s.sequence_id"
    Public Const ACGetStandardWordingChildSQL As String = "SELECT s.document_template_id, d.code, d.description, d.copy_of_original " &
                                                          "FROM {data_model}_standard_wording s, document_template d " &
                                                          "WHERE s.{data_model}_policy_binder_id = {policy_binder_id} " &
                                                          "AND s.gis_property_id= {gis_property_id} " &
                                                          "AND s.gis_object_id= {gis_object_id} " &
                                                          "AND s.document_template_id = d.document_template_id " &
                                                          "AND s.child = 1 " &
                                                          "AND ( s.{parent_key_name} IS NULL OR s.{parent_key_name} = {parent_key_value} )" &
                                                          "ORDER BY s.sequence_id"

    ' Select Sum Insured SQL
    Public Const ACGetSumInsuredStored As Boolean = False
    Public Const ACGetSumInsuredName As String = "SelectSumInsured"
    Public Const ACGetSumInsuredSQL As String = "SELECT s.description, s.reference, s.sum_insured, s.date_added, " &
                                                "s.date_deleted, s.is_valuation_required, s.valuation_date, si.rate, si.premium " &
                                                "FROM {data_model}_sum_insured s " &
                                                "JOIN (SELECT {data_model}_policy_binder_id, rate, premium FROM {data_model}_sum_insured " &
                                                "WHERE sequence_id=1 AND sum_insured_type_id = {sum_insured_type}) si ON si.{data_model}_policy_binder_id=s.{data_model}_policy_binder_id " &
                                                "WHERE s.{data_model}_policy_binder_id = {policy_binder_id} " &
                                                "AND s.sum_insured_type_id = {sum_insured_type} " &
                                                "AND s.sum_insured IS NOT NULL " &
                                                "ORDER BY s.sequence_id"

    ' Delete StandardWordingSQL
    Public Const ACDeleteStandardWordingStored As Boolean = False
    Public Const ACDeleteStandardWordingName As String = "DeleteStandardWording"
    Public Const ACDeleteStandardWordingSQL As String = "DELETE FROM {data_model}_standard_wording " &
                                                        "WHERE {data_model}_policy_binder_id = {policy_binder_id} " &
                                                        "AND gis_property_id = {gis_property_id} AND gis_object_id = {gis_object_id} " &
                                                        "AND (child IS NULL OR child = 0)"
    Public Const ACDeleteStandardWordingChildSQL As String = "DELETE FROM {data_model}_standard_wording " &
                                                             "WHERE {data_model}_policy_binder_id = {policy_binder_id} " &
                                                             "AND gis_property_id = {gis_property_id} AND gis_object_id = {gis_object_id} " &
                                                             "AND ({parent_key_name} = {parent_key_value} OR {parent_key_name} IS NULL ) AND child = 1"

    ' Delete Sum Insured SQL
    Public Const ACDeleteSumInsuredStored As Boolean = False
    Public Const ACDeleteSumInsuredName As String = "DeleteSumInsured"
    Public Const ACDeleteSumInsuredSQL As String = "DELETE FROM {data_model}_sum_insured " &
                                                   "WHERE {data_model}_policy_binder_id = {policy_binder_id} " &
                                                   "AND sum_insured_type_id = {sum_insured_type}"

    ' Insert StandardWordingSQL
    Public Const ACInsertStandardWordingStored As Boolean = False
    Public Const ACInsertStandardWordingName As String = "InsertStandardWording"
    Public Const ACInsertStandardWordingSQL As String = "INSERT INTO {data_model}_standard_wording " &
                                                        "({data_model}_Policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id) " &
                                                        "VALUES ({policy_binder_id}, {sequence_id}, {document_template_id}, {gis_property_id}, {gis_object_id})"
    Public Const ACInsertStandardWordingChildSQL As String = "INSERT INTO {data_model}_standard_wording " &
                                                             "({data_model}_Policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id, child, {parent_key_name}) " &
                                                             "VALUES ({policy_binder_id}, {sequence_id}, {document_template_id}, {gis_property_id}, {gis_object_id}, 1, {parent_key_value})"

    ' Insert Sum Insured SQL
    Public Const ACInsertSumInsuredStored As Boolean = False
    Public Const ACInsertSumInsuredName As String = "InsertSumInsured"
    Public Const ACInsertSumInsuredSQL As String = "INSERT INTO {data_model}_sum_insured " &
                                                   "({data_model}_Policy_binder_id, sum_insured_type_id, sequence_id, description, reference, " &
                                                   "sum_insured, date_added, date_deleted, is_valuation_required, valuation_date, rate, premium) " &
                                                   "VALUES ({policy_binder_id}, {sum_insured_type_id}, {sequence_id}, {description}, {reference}, " &
                                                   "{sum_insured}, {date_added}, {date_deleted}, {is_valuation_required}, {valuation_date}, {rate}, {premium}) "

    ' Copy StandardWording SQL
    Public Const ACCopyStandardWordingStored As Boolean = True
    Public Const ACCopyStandardWordingName As String = "CopyStandardWording"
    Public Const ACCopyStandardWordingSQL As String = "spu_Copy_Standard_Wording"
    'Replaced with stored procedure generating dynamic sql
    'Public Const ACCopyStandardWordingSQL = "INSERT INTO {data_model}_standard_wording " & _
    '"({data_model}_Policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id) " & _
    '"SELECT p1.{data_model}_policy_binder_id, s.sequence_id, s.document_template_id, s.gis_property_id, s.gis_object_id " & _
    '"FROM {data_model}_standard_wording s, {data_model}_policy_binder p1, {data_model}_policy_binder p2 " & _
    '"WHERE p1.gis_policy_link_id = {new_policy_link} " & _
    '"AND s.{data_model}_policy_binder_id = p2.{data_model}_policy_binder_id " & _
    '"AND p2.gis_policy_link_id = {old_policy_link} "

    ' Copy Sum Insured SQL
    Public Const ACCopySumInsuredStored As Boolean = False
    Public Const ACCopySumInsuredName As String = "CopySumInsured"
    Public Const ACCopySumInsuredSQL As String = "INSERT INTO {data_model}_sum_insured " &
                                                 "({data_model}_Policy_binder_id, sum_insured_type_id, sequence_id, description, reference, " &
                                                 "sum_insured, date_added, date_deleted, is_valuation_required, valuation_date, rate, premium) " &
                                                 "SELECT p1.{data_model}_policy_binder_id, s.sum_insured_type_id, s.sequence_id, s.description, s.reference, " &
                                                 "s.sum_insured, s.date_added, s.date_deleted, s.is_valuation_required, s.valuation_date, s.rate, s.premium " &
                                                 "FROM {data_model}_sum_insured s, {data_model}_policy_binder p1, {data_model}_policy_binder p2 " &
                                                 "WHERE p1.gis_policy_link_id = {new_policy_link} " &
                                                 "AND s.{data_model}_policy_binder_id = p2.{data_model}_policy_binder_id " &
                                                 "AND p2.gis_policy_link_id = {old_policy_link} "

    ' Copy Rating Section SQL
    Public Const ACCopyRatingSectionStored As Boolean = False
    Public Const ACCopyRatingSectionName As String = "CopyRatingSection"
    Public Const ACCopyRatingSectionSQL As String = "INSERT INTO rating_section " &
                                                    "(risk_cnt, rating_section_id, rating_section_type_id, policy_section_type_id, sequence_number, " &
                                                    "description, rate_type_id, annual_rate, is_standard, sum_insured, annual_premium, this_premium) " &
                                                    "SELECT {new_risk_cnt}, rating_section_id, rating_section_type_id, policy_section_type_id, sequence_number, " &
                                                    "description, rate_type_id, annual_rate, is_standard, sum_insured, annual_premium, this_premium " &
                                                    "FROM rating_section " &
                                                    "WHERE risk_cnt = {old_risk_cnt}"

    ' Copy Risk Extras SQL
    Public Const ACCopyRiskExtrasStored As Boolean = True
    Public Const ACCopyRiskExtrasName As String = "CopyRiskExtras"
    Public Const ACCopyRiskExtrasSQL As String = "spu_copy_risk_extras"

    ' Select Risk Details SQL
    Public Const ACGetRiskDetailsStored As Boolean = True
    Public Const ACGetRiskDetailsName As String = "SelectRiskDetails"
    Public Const ACGetRiskDetailsSQL As String = "spe_Risk_sel"

    ' Select Detail for insurance_file_cnt
    Public Const ACGetInsuranceFileStored As Boolean = True
    Public Const ACGetInsuranceFileName As String = "SelectInsuranceFileDetail"
    Public Const ACGetInsuranceFileSQL As String = "spe_Insurance_File_sel"

    ' Insert Risk Details SQL
    ' PW311002 - add coverage, insured item and extensions fields
    ' PW181102 - add risk no, variation no, and is risk selected flag
    Public Const ACInsertRiskDetailsStored As Boolean = True
    Public Const ACInsertRiskDetailsName As String = "InsertRiskDetails"
    Public Const ACInsertRiskDetailsSQL As String = "spe_Risk_add"


    ' Update Risk Details SQL
    Public Const ACUpdateRiskDetailsStored As Boolean = True
    Public Const ACUpdateRiskDetailsName As String = "UpdateRiskDetails"
    Public Const ACUpdateRiskDetailsSQL As String = "spe_Risk_upd"

    ' Select Risk Type Details SQL
    Public Const ACGetRiskTypeDetailsStored As Boolean = True
    Public Const ACGetRiskTypeDetailsName As String = "SelectRiskTypeDetails"
    Public Const ACGetRiskTypeDetailsSQL As String = "spe_Risk_Type_sel"

    'CT 23/10/00 - cater for SBO risk screens as well as underwriting -start
    ' Select Risk Group Details SQL
    Public Const ACGetRiskGroupDetailsStored As Boolean = True
    Public Const ACGetRiskGroupDetailsName As String = "SelectRiskGroupDetails"
    Public Const ACGetRiskGroupDetailsSQL As String = "spe_Risk_Group_sel"

    ' Select Product Details SQL
    Public Const ACGetProductDetailsStored As Boolean = True
    Public Const ACGetProductDetailsName As String = "SelectProductDetails"
    Public Const ACGetProductDetailsSQL As String = "spe_Product_sel"

    ' Select Risk Folder Details SQL
    Public Const ACGetRiskFolderDetailsStored As Boolean = True
    Public Const ACGetRiskFolderDetailsName As String = "SelectRiskFolderDetails"
    Public Const ACGetRiskFolderDetailsSQL As String = "spe_Risk_Folder_sel"

    ' Insert Risk Folder Details SQL
    Public Const ACInsertRiskFolderDetailsStored As Boolean = True
    Public Const ACInsertRiskFolderDetailsName As String = "InsertRiskFolderDetails"
    Public Const ACInsertRiskFolderDetailsSQL As String = "spe_Risk_Folder_add"

    ' Update Risk Folder Details SQL
    Public Const ACUpdateRiskFolderDetailsStored As Boolean = True
    Public Const ACUpdateRiskFolderDetailsName As String = "UpdateRiskFolderDetails"
    Public Const ACUpdateRiskFolderDetailsSQL As String = "spe_Risk_Folder_upd"

    ' Select Insurance File Risk Link Details SQL
    Public Const ACGetInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACGetInsuranceFileRiskLinkDetailsName As String = "SelectInsuranceFileRiskLinkDetails"
    Public Const ACGetInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_sel"

    ' Insert Insurance File Risk Link Details SQL
    Public Const ACInsertInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACInsertInsuranceFileRiskLinkDetailsName As String = "InsertInsuranceFileRiskLinkDetails"
    Public Const ACInsertInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_add"

    ' Update Insurance File Risk Link Details SQL
    Public Const ACUpdateInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACUpdateInsuranceFileRiskLinkDetailsName As String = "UpdateInsuranceFileRiskLinkDetails"
    Public Const ACUpdateInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_upd"

    ' Delete Insurance File Risk Link Details SQL
    Public Const ACDeleteInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACDeleteInsuranceFileRiskLinkDetailsName As String = "DeleteInsuranceFileRiskLinkDetails"
    Public Const ACDeleteInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_del"

    ' Select Transaction Type SQL
    Public Const ACGetTransactionTypeStored As Boolean = False
    Public Const ACGetTransactionTypeName As String = "GetTransactionType"
    Public Const ACGetTransactionTypeSQL As String = "select transaction_type_id from transaction_type where code = {code} and is_deleted = 0"

    ' Delete Risk Cancelled On Add SQL
    Public Const ACDeleteRiskCancelledOnAddStored As Boolean = True
    Public Const ACDeleteRiskCancelledOnAddSQL As String = "spu_delete_risk_cancelled_on_add"
    Public Const ACDeleteRiskCancelledOnAddName As String = "DeleteRiskCancelledOnAdd"

    ' CQ3303 Combo Id is shown in child
    Public Const ACGetGISUserDefHeaderDetailStored As Boolean = False
    Public Const ACGetGISUserDefHeaderDetailName As String = "CopySumInsured"
    Public Const ACGetGISUserDefHeaderDetailSQL As String = "select caption from pmcaption pmc inner join gis_user_def_detail gudd on  pmc.caption_id = gudd.caption_id where gudd.gis_user_def_header_id={gis_user_def_header_id} and gis_user_def_detail_id={gis_user_def_detail_id}"

    ' Select GIS_Policy_Link details for a claim SQL
    Public Const ACGetGISPolicyLinkDetailsStored As Boolean = True
    Public Const ACGetGISPolicyLinkDetailsName As String = "SelectGISPolicyLinkDetails"
    Public Const ACGetGISPolicyLinkDetailsSQL As String = "spu_CLM_Get_GIS_Policy_Link_Details"

    Public Const kGetPolicyLinkIdFromRiskName As String = "Returns the gis policy link id for the specified risk_id"
    Public Const kGetPolicyLinkIdFromRiskSQL As String = "spu_SIR_Get_GIS_Policy_Link_Details_By_RiskId"


    ' RAW 16/09/2003 : CQ809 : copied from bSIRListRisks
    'Revert original risk value into risk_cnt in Insurance_file_Risk_link
    Public Const ACRevertRiskStored As Boolean = True
    Public Const ACRevertRiskName As String = "RevertRisk"
    Public Const ACRevertRiskSQL As String = "spu_insurance_file_risk_li_revert"

    ' Select UnusedEditedStandardWording
    Public Const ACGetUnusedEditedStandardWordingStored As Boolean = True
    Public Const ACGetUnusedEditedStandardWordingName As String = "GetUnusedEditedStandardWording"
    Public Const ACGetUnusedEditedStandardWordingSQL As String = "spu_get_UnusedEditedStandardWording"


    Public Const kCopyOriginalRiskPremiumThisYearToNewRiskName As String = "Copy Original Risk Premium This Year To New Risk"
    Public Const kCopyOriginalRiskPremiumThisYearToNewRiskSQL As String = "spu_SIR_Copy_Risk_Billed_Premium"

    Public Const kUpdateCaseGISPolicyLinkName As String = "Update CaseID in gis policy link for new case"
    Public Const kUpdateCaseGISPolicyLinkSQL As String = "spu_Update_Gis_Policy_Link_CaseID"

    Public Const ACGetDefaultClausesForRiskStored As Boolean = True
    Public Const ACGetDefaultClausesForRiskName As String = "GetDefaultClausesForRisk"
    Public Const ACGetDefaultClausesForRiskSQL As String = "spu_Get_Clauses_Set_To_default_In_Risk"

    ' Delete Tax details and Policy Fees Details Batch Renewals SQL
    Public Const ACDeleteTaxDetailsBatchRenewalsStored = True
    Public Const ACDeleteTaxDetailsBatchRenewalsName = "DeleteTaxDetailsBatchRenewals"
    Public Const ACDeleteTaxDetailsBatchRenewalsSQL = "spu_DeleteTaxDetailsForBatchRenewals"


End Module