Option Strict On
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 14/09/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRInsuranceFile.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select All SIRInsuranceFile SQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRInsuranceFile"
    'Developer Guide No 39.
    Public Const ACGetAllDetailsSQL As String = "spe_Insurance_File_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRInsuranceFileID"
    'Developer Guide No 39.
    Public Const ACCheckIDSQL As String = "spe_SIRInsuranceFile_check_id"

    ' Get Party details SQL
    Public Const ACGetPartyDetailsStored As Boolean = False
    Public Const ACGetPartyDetailsName As String = "GETPartyDETAILS"

    Public Const kCancelMTAStored As Boolean = True
    Public Const kCancelMTAName As String = "CancelMTA"
    Public Const kCancelMTASQL As String = "spu_SIR_Update_Policy_Details"


    'TN20001027 (Start) - resolved_name sometime empty

    'Modifying the inline query to make it compatible with SQL server 2005

    Public Const ACGetPartyDetailsSQL As String = "SELECT resolved_name = " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "CASE resolved_name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "WHEN NULL THEN name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "WHEN '' THEN name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "ELSE resolved_name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "END" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "FROM Party where party_cnt ="
    ''TN20000809 - request by Cathy
    'TN20002710 (End)

    'Don't forget that this should ideally use the caption and language, not the description
    ' Get Risk details SQL
    Public Const ACGetRiskDetailsStored As Boolean = False
    Public Const ACGetRiskDetailsName As String = "GETRiskDETAILS"
    Public Const ACGetRiskDetailsSQL As String = "SELECT description, risk_group_id FROM risk_code WHERE risk_code_id = "

    ' Get Analysis details SQL
    Public Const ACGetAnalysisDetailsStored As Boolean = False
    Public Const ACGetAnalysisDetailsName As String = "GETAnalysisDETAILS"
    Public Const ACGetAnalysisDetailsSQL As String = "SELECT description FROM analysis_code WHERE analysis_code_id = "

    ' Get Policy Dedectible details SQL
    Public Const ACGetPolicyDeductibleDescStored As Boolean = True
    Public Const ACGetPolicyDeductibleDescName As String = "GETPolicyDeductibleDETAILS"
    Public Const ACGetPolicyDeductibleDescSQL As String = "spu_Get_Policy_Deductible_Desc"

    ' Get Policy Dedectible Limits SQL
    Public Const ACGetPolicyLimitsDescStored As Boolean = True
    Public Const ACGetPolicyLimitsDescName As String = "GetPolicyLimitsDesc"
    Public Const ACGetPolicyLimitsDescSQL As String = "spu_Get_Policy_Limits_Desc"

    ' Get Policy Type details SQL
    Public Const ACGetPolicyTypeDetailsStored As Boolean = False
    Public Const ACGetPolicyTypeDetailsName As String = "GETPolicyTypeDETAILS"
    Public Const ACGetPolicyTypeDetailsSQL As String = "SELECT description FROM policy_type WHERE policy_type_id = "

    ' Get Product details SQL
    Public Const ACGetProductDetailsStored As Boolean = False
    Public Const ACGetProductDetailsName As String = "GETProductDETAILS"

    Public Const ACGetProductDetailsBrokingSQL As String = "SELECT S.scheme_desc,S.GIS_scheme_id  " &
                                                           " FROM GIS_scheme S,GIS_policy_link P  " &
                                                           " Where S.GIS_scheme_id = P.GIS_scheme_id " &
                                                           "AND P.insurance_file_cnt = "

    Public Const ACGetProductDetailsSQL As String = "SELECT p.description,i.product_id  " &
                                                    " FROM Product P INNER JOIN Insurance_File I " &
                                                    " ON I.product_id = P.product_id " &
                                                    "WHERE I.insurance_file_cnt = "

    ' Get Policy Relationship details SQL
    Public Const ACGetRelationshipDetailsStored As Boolean = False
    Public Const ACGetRelationshipDetailsName As String = "GETRelationshipDETAILS"
    Public Const ACGetRelationshipDetailsSQL As String = "SELECT relation_cnt, policy_relationship_type_id FROM policy_relationship WHERE insurance_file_cnt = "

    ' Get Related Policy details SQL
    Public Const ACGetPolicyDetailsStored As Boolean = False
    Public Const ACGetPolicyDetailsName As String = "GETPolicyDETAILS"
    Public Const ACGetPolicyDetailsSQL As String = "SELECT insurance_ref FROM insurance_file WHERE insurance_file_cnt = "
    'ECK 14/07/99
    ' Get Commission Account details SQL
    Public Const ACGetCommissionAccountStored As Boolean = True
    Public Const ACGetCommissionAccountName As String = "GetCommissionAccount"
    'eck200400
    ' SET 30/04/2003 PS235
    'Developer Guide No 39.
    Public Const ACGetCommissionAccountSQL As String = "spu_commission_account_sel"

    'Datasure - return new fields
    'Developer Guide No 39.
    Public Const ACGetAllFeeSQL As String = "spu_policy_fee_sel"
    Public Const ACGetAllFeeStored As Boolean = True

    Public Const ACGetAllFeeUWStored As Boolean = False
    Public Const ACGetAllFeeUWName As String = "SelectAllFee"
    Public Const ACGetAllFeeUWSQL As String = "SELECT pt.description, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "p.shortname, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "pf.fee_rate_percentage, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "pf.currency_amount Amount," & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "pf.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "FROM Policy_Fee_U pf JOIN Party p ON pf.party_cnt = p.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "JOIN Party_Type pt ON p.party_type_id = pt.party_type_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "Where pf.insurance_file_cnt = {insurance_file_cnt} " & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "ORDER BY pf.policy_fee_u_id"


    ' Select All Policy Fees from Event
    'Datasure - return new fields
    Public Const ACGetAllEventFeeStored As Boolean = True
    Public Const ACGetAllEventFeeName As String = "SelectAllEventFee"
    'Developer Guide No 39.
    Public Const ACGetAllEventFeeSQL As String = "spu_event_policy_fee_sel"

    ' Delete All Policy Fees
    Public Const ACDeleteFeeStored As Boolean = False
    Public Const ACDeleteFeeName As String = "DeleteFee"
    Public Const ACDeleteFeeSQL As String = "Delete from policy_fee where insurance_file_cnt = {insurance_file_cnt}"

    Public Const ACDeleteEventFeeSQL As String = "Delete from event_policy_fee where insurance_file_cnt = {insurance_file_cnt}"

    'Datasure Delete All Policy Fee Taxes
    Public Const ACDeleteFeeTaxesStored As Boolean = False
    Public Const ACDeleteFeeTaxesName As String = "DeleteFee"
    Public Const ACDeleteFeeTaxesSQL As String = "Delete from tax_calculation where insurance_file_cnt = {insurance_file_cnt} AND transtype in ('TTF','TTFC')"

    Public Const ACDeleteEventFeeTaxesStored As Boolean = False
    Public Const ACDeleteEventFeeTaxesName As String = "DeleteFee"
    Public Const ACDeleteEventFeeTaxesSQL As String = "Delete from event_tax_calculation where insurance_file_cnt = {insurance_file_cnt} AND transtype in ('TTF','TTFC')"

    'Datasure
    'Select PolicyFee Tax SQL
    Public Const ACSelectFeeTaxStored As Boolean = True
    Public Const ACSelectFeeTaxName As String = "SelectFeeTax"
    'Developer Guide No 39.
    Public Const ACSelectFeeTaxSQL As String = "spu_SIR_Calculate_Fee_Tax_Amounts_SFB"

    'Select EventPolicyFee Tax SQL
    Public Const ACSelectEventFeeTaxStored As Boolean = True
    Public Const ACSelectEventFeeTaxName As String = "SelectFeeTax"
    'Developer Guide No 39.
    Public Const ACSelectEventFeeTaxSQL As String = "spu_Event_SIR_Calculate_Fee_Tax_Amounts_SFB"

    'Select PolicyFeeCommission Tax SQL
    Public Const ACSelectFeeCommissionTaxStored As Boolean = True
    Public Const ACSelectFeeCommissionTaxName As String = "SelectFeeCommissionTax"
    'Developer Guide No 39.
    Public Const ACSelectFeeCommissionTaxSQL As String = "spu_SIR_Calculate_Fee_Commission_Tax_Amounts_SFB"

    'Select PolicyEventFeeCommission Tax SQL
    Public Const ACSelectEventFeeCommissionTaxStored As Boolean = True
    Public Const ACSelectEventFeeCommissionTaxName As String = "SelectEventFeeCommissionTax"
    'Developer Guide No 39.
    Public Const ACSelectEventFeeCommissionTaxSQL As String = "spu_Event_SIR_Calculate_Fee_Commission_Tax_Amounts_SFB"


    ' Insert Policy Fees
    Public Const ACInsertFeeStored As Boolean = True
    Public Const ACInsertFeeName As String = "InsertFee"
    'EK 14/09/99 New fields for Extras

    'Datasure 4 new parameters
    'Developer Guide No 39.
    Public Const ACInsertFeeSQL As String = "spe_policy_fee_add"
    Public Const ACInsertEventFeeSQL As String = "spe_event_policy_fee_add"

    ' Select All Policy Narratives
    Public Const ACGetAllNarrativeStored As Boolean = False
    Public Const ACGetAllNarrativeName As String = "SelectAllNarrative"
    Public Const ACGetAllNarrativeSQL As String = "SELECT nc.code, c.caption, pn.narrative_code_id " &
                                                  "FROM policy_narrative pn, narrative_code nc, PMcaption c " &
                                                  "WHERE pn.insurance_file_cnt = {insurance_file_cnt} " &
                                                  "AND nc.narrative_code_id = pn.narrative_code_id " &
                                                  "AND nc.caption_id = c.caption_id " &
                                                  "AND c.language_id = {language_id} " &
                                                  "ORDER BY pn.policy_narrative_id"

    ' Select All Policy Narratives from Event
    Public Const ACGetAllEventNarrativeStored As Boolean = False
    Public Const ACGetAllEventNarrativeName As String = "SelectAllNarrative"
    Public Const ACGetAllEventNarrativeSQL As String = "SELECT nc.code, c.caption, pn.narrative_code_id " &
                                                       "FROM event_policy_narrative pn, narrative_code nc, PMcaption c " &
                                                       "WHERE pn.insurance_file_cnt = {insurance_file_cnt} " &
                                                       "AND nc.narrative_code_id = pn.narrative_code_id " &
                                                       "AND nc.caption_id = c.caption_id " &
                                                       "AND c.language_id = {language_id} " &
                                                       "ORDER BY pn.policy_narrative_id"

    ' Delete All Policy Narratives
    Public Const ACDeleteNarrativeStored As Boolean = False
    Public Const ACDeleteNarrativeName As String = "DeleteNarrative"
    Public Const ACDeleteNarrativeSQL As String = "Delete from policy_narrative where insurance_file_cnt = {insurance_file_cnt}"
    'EK 05/09/99 Delete editable event
    Public Const ACDeleteEventNarrativeSQL As String = "Delete from event_policy_narrative where insurance_file_cnt = {insurance_file_cnt}"

    ' Insert Policy Narratives
    Public Const ACInsertNarrativeStored As Boolean = True
    Public Const ACInsertNarrativeName As String = "InsertFee"
    'Developer Guide No 39.
    Public Const ACInsertNarrativeSQL As String = "spe_policy_narrative_add"
    'EK 05/09/99 Insert editable event
    'Developer Guide No 39.
    Public Const ACInsertEventNarrativeSQL As String = "spe_event_policy_narrative_add"

    'EK 20/10/99
    ' Delete All CoInsurers
    Public Const ACDeleteCoInsurerStored As Boolean = False
    Public Const ACDeleteCoInsurerName As String = "DeleteCoInsurers"
    Public Const ACDeleteCoInsurerSQL As String = "Delete from policy_coinsurers where insurance_file_cnt = {insurance_file_cnt}"
    Public Const ACDeleteEventCoInsurerSQL As String = "Delete from event_policy_coinsurers where insurance_file_cnt = {insurance_file_cnt}"
    '

    ' Get Live Transaction Details SQL
    Public Const ACGetLiveTransactionDetailsStored As Boolean = False
    Public Const ACGetLiveTransactionDetailsName As String = "GetLiveTransactionDetails"
    Public Const ACGetLiveTransactionDetailsSQL As String = "SELECT distinct td.insurance_ref " &
                                                            "FROM insurance_file ifi, transdetail td " &
                                                            "WHERE td.insurance_ref = ifi.insurance_ref " &
                                                            "AND ifi.insurance_file_cnt = {insurance_file_cnt} " &
                                                            "AND td.fully_matched = 0"

    'DC050606 PN28736 check for claims attached to policy being deleted
    ' Get Claims For Policy SQL
    Public Const ACGetClaimsForPolicyStored As Boolean = False
    Public Const ACGetClaimsForPolicyName As String = "GetClaimsForPolicy"
    Public Const ACGetClaimsForPolicySQL As String = "SELECT distinct c.claim_number " &
                                                     "FROM claim c " &
                                                     "WHERE c.policy_id = {insurance_file_cnt} "

    Public Const ACIsLivePolicyStored As Boolean = False
    Public Const ACIsLivePolicyName As String = "IsLivePolicy"
    Public Const ACIsLivePolicySQL As String = "SELECT" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "    NULL" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "FROM insurance_file" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "WHERE insurance_file_cnt = {insurance_file_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AND insurance_file_status_id IS NULL" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AND insurance_file_type_id <> 1" & Strings.ChrW(13) & Strings.ChrW(10)

    ' 'Delete' Policy SQL
    Public Const ACDeletePolicyStored As Boolean = False
    Public Const ACDeletePolicyName As String = "DeletePolicy"
    Public Const ACDeletePolicySQL As String = "UPDATE insurance_file " &
                                               "SET policy_ignore = 1 WHERE insurance_file_cnt = {insurance_file_cnt}"


    ' 'Undelete' Policy SQL
    Public Const ACUndeletePolicyStored As Boolean = False
    Public Const ACUndeletePolicyName As String = "UndeletePolicy"
    Public Const ACUndeletePolicySQL As String = "UPDATE insurance_file " &
                                                 "SET policy_ignore = Null WHERE insurance_file_cnt = {insurance_file_cnt}"


    ' get Valid Risk Codes for this branch
    'Datasure further checks against country
    Public Const ACSelectRiskCodesForBranchStored As Boolean = True
    Public Const ACSelectRiskCodesForBranchName As String = "SelectRiskCodesForBranch"
    'Developer Guide No 39.
    Public Const ACSelectRiskCodesForBranchSQL As String = "spu_select_risks_for_branch"

    'TN20000816 - Doc Ref 10 (Start)
    Public Const ACSelectLeadAgentUsingAgentCntStored As Boolean = False
    Public Const ACSelectLeadAgentUsingAgentCntName As String = "SelectLeadAgentUsingAgentCnt"
    'Moh : 15-05-2003 - Retrieve the date cancelled field too

    'Modifying the inline query to make it compatible with SQL server 2005

    Public Const ACSelectLeadAgentUsingAgentCntSQL As String = "SELECT resolved_name = " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "CASE A.resolved_name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "WHEN NULL THEN A.name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "WHEN '' THEN A.name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "ELSE A.resolved_name" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "END," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "B.agent_cnt, PA.date_cancelled FROM " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "Party A INNER JOIN Party B ON A.party_cnt = B.agent_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "INNER JOIN Party_Agent PA ON PA.party_cnt = A.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                               "WHERE B.party_cnt ={party_cnt} "
    'TMP - True Monthly Policy
    Public Const ACSelectAgentAllowCommissionusingAgentCntStored As Boolean = True
    Public Const ACSelectAgentAllowCommissionusingAgentCntName As String = "SelectAgentAllowCommissionusingAgentCnt"
    Public Const ACSelectAgentAllowCommissionusingAgentCntSQL As String = "spu_Allow_Consolidated_Commission"



    'Public Const ACSelectLeadAgentUsingAgentCntSQL = "SELECT resolved_name = " & vbCrLf & _
    '"Case A.resolved_name" & vbCrLf & _
    '"WHEN null THEN A.name" & vbCrLf & _
    '"WHEN '' THEN A.name" & vbCrLf & _
    '"Else A.resolved_name" & vbCrLf & _
    '"END," & vbCrLf & _
    '"B.agent_cnt FROM " & vbCrLf & _
    '"Party A , Party B " & vbCrLf & _
    '"WHERE A.party_cnt = B.agent_cnt " & vbCrLf & _
    '"AND B.party_cnt = "

    Public Const ACSelectSubAgentStored As Boolean = True
    Public Const ACSelectSubAgentName As String = "SelectSubAgents"
    'Developer Guide No 39.
    Public Const ACSelectSubAgentSQL As String = "spu_Select_SubAgents"


    Public Const ACDeleteSubAgentStored As Boolean = True
    Public Const ACDeleteSubAgentName As String = "DeleteSubAgents"
    'Developer Guide No 39.
    Public Const ACDeleteSubAgentSQL As String = "spu_Del_SubAgents"

    Public Const ACAddSubAgentStored As Boolean = True
    Public Const ACAddSubAgentName As String = "AddSubAgents"
    'Developer Guide No 39.
    Public Const ACAddSubAgentSQL As String = "spu_Add_SubAgents"
    'TN20000816 - Doc Ref 10 (End)

    'Standard wordings
    ' Select All Policy Standard Wordings
    Public Const ACGetAllStandardWordingstored As Boolean = False
    Public Const ACGetAllStandardWordingName As String = "SelectAllStandardWording"

    ' Start - Sankar - PN 61172
    'Public Const ACGetAllStandardWordingSQL As String = "SELECT  dt.code, dt.description, dt.document_template_id " & _
    '                                                    "FROM policy_standard_wording psw, document_template dt, wording_product_link WPL  " & _
    '                                                    "WHERE psw.insurance_file_cnt = {insurance_file_cnt} " & _
    '                                                    "AND psw.document_template_id = dt.document_template_id " & _
    '                                                    "AND dt.document_template_id = WPL.document_template_id " & _
    '                                                    "AND WPL.Product_Id = {product_id} " & _
    '                                                    "AND WPL.Branch_Id = {source_id} " & _
    '                                                    "ORDER BY psw.policy_standard_wording_id"
    ' End - Sankar - PN 61172

    Public Const ACGetAllStandardWordingSQL As String = "SELECT  dt.code, dt.description, dt.document_template_id,psw.do_not_merge " &
                                                       "FROM policy_standard_wording psw, document_template dt " &
                                                       "WHERE psw.insurance_file_cnt = {insurance_file_cnt} " &
                                                       "AND psw.document_template_id = dt.document_template_id " &
                                                       "ORDER BY psw.policy_standard_wording_id"
    ' Select All Policy Standard Wordings from Event
    Public Const ACGetAllEventStandardWordingStored As Boolean = False
    Public Const ACGetAllEventStandardWordingName As String = "SelectAllStandardWording"
    Public Const ACGetAllEventStandardWordingSQL As String = "SELECT  dt.code, dt.description, dt.document_template_id,psw.do_not_merge " &
                                                             "FROM policy_standard_wording psw, document_template dt " &
                                                             "WHERE psw.insurance_file_cnt = {insurance_file_cnt} " &
                                                             "AND psw.document_template_id = dt.document_template_id " &
                                                             "ORDER BY psw.policy_standard_wording_id"

    ' Delete All Policy Standard Wordings
    Public Const ACDeleteStandardWordingStored As Boolean = False
    Public Const ACDeleteStandardWordingName As String = "DeleteStandardWording"
    Public Const ACDeleteStandardWordingSQL As String = "Delete from policy_standard_wording where insurance_file_cnt = {insurance_file_cnt}"
    'EK 05/09/99 Delete editable event
    Public Const ACDeleteEventStandardWordingSQL As String = "Delete from event_policy_standard_wording where insurance_file_cnt = {insurance_file_cnt}"

    ' Insert Policy Standard Wordings
    Public Const ACInsertStandardWordingStored As Boolean = True
    Public Const ACInsertStandardWordingName As String = "InsertStandardWording"

    'Tomo290900
    'Limitation on stored procedure name length - missed off the g
    'Developer Guide No 39.
    Public Const ACInsertStandardWordingSQL As String = "spe_policy_standard_wordin_add"
    'EK 05/09/99 Insert editable event
    'Developer Guide No 39.
    Public Const ACInsertEventStandardWordingSQL As String = "spe_event_policy_StandardWording_add"

    'TN20001030 (Start)
    Public Const ACDelCoiArrangementStored As Boolean = True
    Public Const ACDelCoiArrangementName As String = "DelCoiArrangement"
    'Developer Guide No 39.
    Public Const ACDelCoiArrangementSQL As String = "spe_Coi_Arrangement_del"

    Public Const ACDelCoiValueStored As Boolean = True
    Public Const ACDelCoiValueName As String = "DelCoiValue"
    'Developer Guide No 39.
    Public Const ACDelCoiValueSQL As String = "spe_Coi_Value_del"

    Public Const ACDelCoiCompulsoryStored As Boolean = True
    Public Const ACDelCoiCompulsoryName As String = "DelCoiCompulsory"
    'Developer Guide No 39.
    Public Const ACDelCoiCompulsorySQL As String = "spe_Coi_Compulsory_Value_del"
    'TN20001030 (End)
    'eck070301
    ' Get Risk Cnt Details SQL
    Public Const ACGetPolicyRiskCntStored As Boolean = False
    Public Const ACGetPolicyRiskCntName As String = "GetLiveTransactionDetails"
    Public Const ACGetPolicyRiskCntSQL As String = "SELECT l.risk_cnt " &
                                                   "FROM  insurance_file_risk_link l " &
                                                   "WHERE l.insurance_file_cnt = "

    'MSS210901 - Added for merge
    'TN20010419 Start
    Public Const ACCancelMTAStored As Boolean = True
    Public Const ACCancelMTAName As String = "Cancel MTA"
    ' Public Const ACCancelMTASQL As String = "spu_SIR_Update_Policy_Details"
    Public Const ACCancelMTASQL As String = "UPDATE insurance_file" & Strings.ChrW(13) & Strings.ChrW(10) &
                                            "SET insurance_file_status_id = 1" & Strings.ChrW(13) & Strings.ChrW(10) &
                                            "WHERE base_insurance_file_cnt = {insurance_file_cnt} OR insurance_file_cnt = {insurance_file_cnt}"
    '"WHERE insurance_file_cnt = {insurance_file_cnt}"
    'TN20010419 End
    'MSS210901 - Merge end

    Public Const ACSelectIsMidnightRenewalStored As Boolean = False
    Public Const ACSelectIsMidnightRenewalName As String = "GetIsMidnightRenewal"
    Public Const ACSelectIsMidnightRenewalSQL As String = "SELECT is_midnight_renewal FROM Product" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                          "WHERE product_id = {product_id}"
    '2005Roadmap
    Public Const ACSelectIsMidnightRenewalSFBStored As Boolean = False
    Public Const ACSelectIsMidnightRenewalSFBName As String = "GetIsMidnightRenewalSFB"
    Public Const ACSelectIsMidnightRenewalSFBSQL As String = "SELECT RG.midnight_renewal FROM Risk_Group RG " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "JOIN Risk_Code RC ON RC.risk_group_id = RG.risk_group_id  " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "WHERE RC.risk_code_id = {risk_code_id}"


    Public Const ACSelectInsuredNameStored As Boolean = True
    Public Const ACSelectInsuredNameName As String = "ACSelectInsuredName"
    'Developer Guide No 39.
    Public Const ACSelectInsuredNameSQL As String = "spu_Get_Insured_Name"


    ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (START)
    Public Const ACAddClientPolicyStored As Boolean = True
    Public Const ACAddClientPolicyName As String = "AddClientPolicy"
    'Developer Guide No 39.
    Public Const ACAddClientPolicySQL As String = "spu_Policy_Client_add"

    Public Const ACDeleteClientPolicyStored As Boolean = True
    Public Const ACDeleteClientPolicyName As String = "DeleteClientPolicy"
    'Developer Guide No 39.
    Public Const ACDeleteClientPolicySQL As String = "spu_Policy_Client_delall"

    Public Const ACSelectClientPolicyStored As Boolean = True
    Public Const ACSelectClientPolicyName As String = "SelectClientPolicy"
    'Developer Guide No 39.
    Public Const ACSelectClientPolicySQL As String = "spu_Policy_Client_selall"

    Public Const ACSetClientPolicyLeadStored As Boolean = True
    Public Const ACSetClientPolicyLeadName As String = "SetClientPolicyLead"
    'Developer Guide No 39.
    Public Const ACSetClientPolicyLeadSQL As String = "spu_Policy_Client_setlead"
    ' PWF20020624 - Associated Clients SQL - SBO 1.9 - 094 Customer Centricity (END)

    Public Const ACGetBranchesStored As Boolean = True
    Public Const ACGetBranchesName As String = "GetBranches"
    'Developer Guide No 39.
    Public Const ACGetBranchesSQL As String = "spu_pm_get_user_sources2"

    'Just for LIVE(040) policies
    ' RDT20070425 - Moved to a stored Proc and extended to retrieve the latest live, completed or superseeded version.
    Public Const ACHasInstalmentStored As Boolean = True
    Public Const ACHasInstalmentName As String = "HasInstalment"
    'Developer Guide No 39.
    Public Const ACHasInstalmentSQL As String = "spu_get_current_PF_for_insurance_file"

    'Datasure new procedures to check for coinsurers
    Public Const ACCheckCoInsurersStored As String = "True"
    Public Const ACCheckCoInsurersName As String = "CheckCoInsurers"
    'Developer Guide No 39.
    Public Const ACCheckCoInsurersSQL As String = "spu_SIR_check_coinsurers"

    Public Const ACCheckEventCoInsurersStored As String = "True"
    Public Const ACCheckEventCoInsurersName As String = "CheckEventCoInsurers"
    'Developer Guide No 39.
    Public Const ACCheckEventCoInsurersSQL As String = "spu_Event_SIR_check_coinsurers"

    'Policy Discount work
    Public Const ACCalculateDiscountStored As Boolean = True
    Public Const ACCalculateDiscountName As String = "CalculateDiscount"
    'Developer Guide No 39. 
    Public Const ACCalculateDiscountSQL As String = "spu_calculate_discount"

    Public Const ACApplyDiscountStored As Boolean = True
    Public Const ACApplyDiscountName As String = "ApplyDiscount"
    'Developer Guide No 39. 
    Public Const ACApplyDiscountSQL As String = "spu_apply_discount"

    Public Const ACAdjustDiscountStored As Boolean = True
    Public Const ACAdjustDiscountName As String = "AdjustDiscount"
    'Developer Guide No 39. 
    Public Const ACAdjustDiscountSQL As String = "spu_adjust_discount"

    Public Const kGetPolicyDiscountTotalPremiumName As String = "recalculates the fees for the risks associated with this policy"
    Public Const kGetPolicyDiscountTotalPremiumSQL As String = "spu_SIR_Policy_Discount_Get_Total_Premium"

    Public Const kGetSelectedRiskCountName As String = "returns the total number of selected risks for the specified insurance file"
    Public Const kGetSelectedRiskCountSQL As String = "spu_SIR_Policy_Discount_Get_Selected_Risk_Count"
    '31563
    Public Const ACGetPartySubBranchStored As Boolean = False
    Public Const ACGetPartySubBranchName As String = "GetPartySubBranch"
    Public Const ACGetPartySubBranchSQL As String = "SELECT source_id,sub_branch_id FROM party" &
                                                    " WHERE party_cnt = {party_cnt}"

    Public Const ACGetFeeSaTypeOfSaleForPartyAndInsuranceFileStored As Boolean = True
    Public Const ACGetFeeSaTypeOfSaleForPartyAndInsuranceFileName As String = "GetFeeFsaTypeOfSaleForPartyAndInsuranceFile"
    Public Const ACGetFeeSaTypeOfSaleForPartyAndInsuranceFileSQL As String = "spu_GetFeeFsaTypeOfSaleForPartyAndInsuranceFile"

    Public Const ACGetAdditionalFeeDetailsForPartyAndInsuranceFileStored As Boolean = True
    Public Const ACGetAdditionalFeeDetailsForPartyAndInsuranceFileName As String = "getadditionalfeedetailsforPartyAndInsuranceFile"
    Public Const ACGetAdditionalFeeDetailsForPartyAndInsuranceFileSQL As String = "spu_getadditionalfeedetailsforPartyAndInsuranceFile"

    Public Const AC_SQL_AddPolicyAddOnTaxStored As Boolean = True
    Public Const AC_SQL_AddPolicyAddOnTaxName As String = "AddPolicyAddOnTax"
    Public Const AC_SQL_AddPolicyAddOnTaxSQL As String = "spu_TXN_tax_policy_fee_add"

    'Deletes all corresponding Risk Data for a Policy
    Public Const ACDeleteRiskDataStored As Boolean = True
    Public Const ACDeleteRiskDataName As String = "spu_SIR_Delete_Risk_Data"
    'Developer Guide No 39. 
    Public Const ACDeleteRiskDataSQL As String = "spu_SIR_Delete_Risk_Data"

    ' Get Policy RiskId SQL
    Public Const ACGetPolicyRiskStored As Boolean = True
    Public Const ACGetPolicyRiskName As String = "spu_Get_Risk_Code_Id"
    'Developer Guide No 39.
    Public Const ACGetPolicyRiskSQL As String = "spu_Get_Risk_Code_Id"

    Public Const ACGetAgentDetailStored As Boolean = True
    Public Const ACGetAgentDetailName As String = "Get Agent Detail"
    Public Const ACGetAgentDetailSQL As String = "spu_Get_Agent_Detail"

    Public Const ACGetClausesSetToDefaultInProductStored As Boolean = True
    Public Const ACGetClausesSetToDefaultInProductName As String = "Get Default Clauses attached with product"
    Public Const ACGetClausesSetToDefaultInProductSQL As String = "spu_Get_Clauses_Set_To_default_In_Product"

    Public Const ACGetDefaultPreferredCorrespondenceStored As Boolean = False
    Public Const ACGetDefaultPreferredCorrespondenceName As String = "Get Default Preferred Correspondence"
    Public Const ACGetDefaultPreferredCorrespondenceSQL As String = "select correspondence_type_id ,c.code, c.description from Party P, Contact_Type C where c.contact_type_id = P.correspondence_type_id and  party_cnt = {party_cnt}"

    Public Const ACExistingPreferredCorrespondenceStored As Boolean = False
    Public Const ACExistingPreferredCorrespondenceName As String = "Get Default Preferred Correspondence"
    Public Const ACExistingPreferredCorrespondenceSQL As String = "select  ifile.Default_Preferred_Correspondence ,c.code, c.description" &
                                                                    " from Insurance_File  ifile, Contact_Type C " &
                                                                    " where c.contact_type_id = ifile.Default_Preferred_Correspondence  and  insurance_file_cnt = {insurance_file_cnt}"

    Public Const ACGetRenewalFrequencyMonthsSQL As String = "select number_of_months from Renewal_Frequency where renewal_frequency_id= {RenewalFrequencyId}"

    Public Const ACUpdateLapseVersionsStatusSQL As String = "spu_Update_lapse_policyversions_Status"
    Public Const ACUpdateLapseVersionsStatusStored As Boolean = True
    Public Const ACUpdateLapseVersionsStatusName As String = "UpdateLapseVersionsStatus"
End Module