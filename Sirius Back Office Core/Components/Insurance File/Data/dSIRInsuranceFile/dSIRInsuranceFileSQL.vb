Option Strict Off
Option Explicit On
Module SQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: SQL
    '
    ' Date: 02/09/1998
    '
    ' Description: Contains the SQL Statements required by the
    '              SIRInsuranceFile class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Select SIRInsuranceFile SQL
    Public Const ACSelectSingleStored As Boolean = True
    Public Const ACSelectSingleName As String = "SelectSingleSIRInsuranceFile"
    Public Const ACSelectSingleSQL As String = "spe_Insurance_File_sel"

    ' Select SIRInsuranceFile from Event SQL
    Public Const ACSelectSingleEventStored As Boolean = True
    Public Const ACSelectSingleEventName As String = "SelectSingleSIREventInsuranceFile"
    Public Const ACSelectSingleEventSQL As String = "spe_Event_Insurance_File_sel"

    ' Add SIRInsuranceFile SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddSIRInsuranceFile"
    'Datasure extra parameter for Country Id
    Public Const ACAddSQL As String = "spe_Insurance_File_add"

    ' Delete SIRInsuranceFile SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteSIRInsuranceFile"
    Public Const ACDeleteSQL As String = "spe_Insurance_File_del"

    ' Update SIRInsuranceFile SQL
    'Datasure extra parameter for Country Id
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateSIRInsuranceFile"
    Public Const ACUpdateSQL As String = "spe_Insurance_File_upd"

    'Datasure extra parameter for Country Id
    Public Const ACUpdateEventStored As Boolean = True
    Public Const ACUpdateEventName As String = "UpdateSIREventInsuranceFile"
    Public Const ACUpdateEventSQL As String = "spe_Event_Insurance_File_upd"

    ' Copy Folder to event
    Public Const ACCopyFolderToEventStored As Boolean = True
    Public Const ACCopyFolderToEventName As String = "CopyFolderToEvent"
    Public Const ACCopyFolderToEventSQL As String = "spu_copy_folder_to_event"

    'EK 05/09/99
    ' Copy Folder from event
    Public Const ACCopyEventToFolderStored As Boolean = True
    Public Const ACCopyEventToFolderName As String = "CopyEventToFolder"
    Public Const ACCopyEventToFolderSQL As String = "spu_copy_event_to_folder"

    ' Copy File to event
    Public Const ACCopyFileToEventStored As Boolean = True
    Public Const ACCopyFileToEventName As String = "CopyFileToEvent"
    Public Const ACCopyFileToEventSQL As String = "spu_copy_file_to_event"

    'EK 05/09/99
    ' Copy File from event
    Public Const ACCopyEventToFileStored As Boolean = True
    Public Const ACCopyEventToFileName As String = "CopyEventToFile"
    Public Const ACCopyEventToFileSQL As String = "spu_copy_event_to_file"


    'EK 05/09/99
    ' Copy System from event
    Public Const ACCopyEventToSystemStored As Boolean = True
    Public Const ACCopyEventToSystemName As String = "CopyEventToSystem"
    Public Const ACCopyEventToSystemSQL As String = "spu_copy_event_to_system"

    'Datasure 30062005
    ' Copy Policy Class of Business sections from event
    Public Const ACCopyEventToPolicySectionsStored As Boolean = True
    Public Const ACCopyEventToPolicySectionsName As String = "CopyPolicySectionsToEvent"
    Public Const ACCopyEventToPolicySectionsSQL As String = "spu_copy_event_to_policy_section"

    'Datasure 15092006
    Public Const ACCopyCoinsurerSectionToEventStored As Boolean = True
    Public Const ACCopyCoinsurerSectionToEventName As String = "CopyCoinsurerSectionToEvent"
    Public Const ACCopyCoinsurerSectionToEventSQL As String = "spu_copy_coinsurers_section_to_event"

    Public Const ACCopyEventToCoinsurerSectionStored As Boolean = True
    Public Const ACCopyEventToCoinsurerSectionName As String = "CopyEventToCoinsurerSection"
    Public Const ACCopyEventToCoinsurerSectionSQL As String = "spu_copy_event_to_coinsurers_section"

    'Datasure 06082005
    ' Copy Policy Class of Business sections to event
    Public Const ACCopyTaxCalculationToEventStored As Boolean = True
    Public Const ACCopyTaxCalculationToEventName As String = "CopyTaxBreakdownToEvent"
    Public Const ACCopyTaxCalculationToEventSQL As String = "spu_copy_tax_calculation_to_event"

    ' Recreate the tax breakdown
    Public Const ACReCalculateTaxCalculationStored As Boolean = True
    Public Const ACReCalculateTaxCalculationName As String = "RecreateTaxBreakdown"
    Public Const ACReCalculateTaxCalculationSQL As String = "spu_SIR_recalculate_policy_tax_amounts_SFB"
    'Datasure 22082005
    'Add event & policy sections together for MTA Debit
    Public Const ACAddFiletoEventStored As Boolean = True
    Public Const ACAddFiletoEventName As String = "AddFileToEvent"
    Public Const ACAddFiletoEventSQL As String = "spu_add_file_to_event"

    'Subtract event from policy sections for MTA Credit
    Public Const ACTakeFileFromEventStored As Boolean = True
    Public Const ACTakeFileFromEventName As String = "TakeFileFromEvent"
    Public Const ACTakeFileFromEventSQL As String = "spu_take_file_from_event"

    Public Const ACUpdateCoinsurerToEventStored As Boolean = True
    Public Const ACUpdateCoinsurerToEventName As String = "UpdateCoinsurerToEvent"
    Public Const ACUpdateCoinsurerToEventSQL As String = "spu_update_coinsurer_to_event"

    Public Const ACUpdateCoinsurerFromEventStored As Boolean = True
    Public Const ACUpdateCoinsurerFromEventName As String = "UpdateCoinsurerFromEvent"
    Public Const ACUpdateCoinsurerFromEventSQL As String = "spu_update_coinsurer_from_event"


    'EK 05/09/99
    ' Copy Coinsurers from event
    Public Const ACCopyEventToCoinsurersStored As Boolean = True
    Public Const ACCopyEventToCoinsurersName As String = "CopyEventToCoinsurers"
    Public Const ACCopyEventToCoinsurersSQL As String = "spu_copy_event_to_coinsurers"

    'EK 05/09/99
    ' Copy Fees from event
    Public Const ACCopyEventToFeesStored As Boolean = True
    Public Const ACCopyEventToFeesName As String = "CopyEventToFees"
    Public Const ACCopyEventToFeesSQL As String = "spu_copy_event_to_policy_fee"




    ' Copy Shared Premiums from event
    Public Const ACCopyEventToPremiumsStored As Boolean = True
    Public Const ACCopyEventToPremiumsName As String = "CopyEventToPremiums"
    Public Const ACCopyEventToPremiumsSQL As String = "spu_copy_event_to_premiums"

    'eck210601

    ' Copy Policy Agents from event
    Public Const ACCopyEventToPolicyAgentsStored As Boolean = True
    Public Const ACCopyEventToPolicyAgentsName As String = "CopyEventToPremiums"
    Public Const ACCopyEventToPolicyAgentsSQL As String = "spu_copy_event_to_policy_agents"


    'RJG 14/07/00 - SP details for Lapse Insurance File Update
    Public Const ACInsuranceFileLapseStored As Boolean = True
    Public Const ACInsuranceFileLapseName As String = "UpdateInsurancFileLapse"
    Public Const ACInsuranceFileLapseSQL As String = "spu_Ins_File_Lapse"

    'RJG 17/07/00 - SP details for Cancel Insurance File Update
    Public Const ACInsuranceFileCancelStored As Boolean = True
    Public Const ACInsuranceFileCancelName As String = "UpdateInsurancFileCancel"
    Public Const ACInsuranceFileCancelSQL As String = "spu_Ins_File_Cancel"

    'CT /12/00 Select party shortname for use in documaster
    Public Const ACGetPartyShortNameStored As Boolean = False
    Public Const ACGetPartyShortNameName As String = "GetPartyShortName"
    Public Const ACGetPartyShortNameSQL As String = "select shortname from party " &
                                                    "where party_cnt = {party_cnt}"

    'DN 16/08/01 - Select policy Description for use in documaster from insurance_file_system
    Public Const ACGetPolicyDescStored As Boolean = False
    Public Const ACGetPolicyDescName As String = "GetPolicyDesc"
    Public Const ACGetPolicyDescSQL As String = "select s.last_trans_description from insurance_file_system s, insurance_file i " &
                                                "where i.insurance_folder_cnt = {insurance_folder_cnt} " &
                                                "and i.insurance_file_cnt = s.insurance_file_cnt"

    'eck030801
    ' Delete Transaction Event
    Public Const ACDeleteTransactionEventStored As Boolean = True
    Public Const ACDeleteTransactionEventName As String = "DeleteTransactionEvent"
    Public Const ACDeleteTransactionEventSQL As String = "spu_delete_transaction_event"

    'Select contract certainty details
    Public Const ACGetContractCertaintyStored As Boolean = True
    Public Const ACGetContractCertaintyName As String = "GetContractCertainty"
    'developer guide no. 39
    Public Const ACGetContractCertaintySQL As String = "spu_contract_certainty_select"

    'Update contract certainty Details
    Public Const ACUpdateContractCertaintyStored As Boolean = True
    Public Const ACUpdateContractCertaintyName As String = "UpdateContractCertainty"
    'developer guide no. 39
    Public Const ACUpdateContractCertaintySQL As String = "spu_contract_certainty_update"

    Public Const ACRecalculatePolicyFeeStored As Boolean = True
    Public Const ACRecalculatePolicyFeeName As String = "RecalculatePolicyFee"
    Public Const ACRecalculatePolicyFeeSQL As String = "spu_SIR_ReCalculate_Policy_Fee"

    Public Const ACRecalculatePolicyAgentsStored As Boolean = True
    Public Const ACRecalculatePolicyAgentsName As String = "RecalculatePolicyAgents"
    Public Const ACRecalculatePolicyAgentsSQL As String = "spu_SIR_ReCalculate_Policy_Agents"

    Public Const ACGetExpiryDateStored As Boolean = True
    Public Const ACGetExpiryDatesName As String = "GetExpiryDate"
    Public Const ACGetExpiryDateSQL As String = "spu_Sir_GetExpiryDate"


    Public Const ACCheckDMEFolderStored As Boolean = True
    Public Const ACCheckDMEFolderName As String = "CheckDMEFolder"
    Public Const ACCheckDMEFolderSQL As String = "spu_sir_check_DME_folder"
End Module
