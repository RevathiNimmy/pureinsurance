Option Strict Off
Option Explicit On
Module FindRiskSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: FindInsuranceSQL
    '
    ' Date: 23rd October 1996
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate an FindInsurance
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectEvent"
    ' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"

    ' Select FindInsurance SQL
    Public Const ACInsLikeRefStored As Boolean = True
    Public Const ACInsLikeRefName As String = "FindInsuranceLikeRef"
    'Developer guide no 39. 
    Public Const ACInsLikeRefSQL As String = "spu_findins_like_ref"

    ' Select FindInsurance SQL
    Public Const ACInsLikeIndexStored As Boolean = True
    Public Const ACInsLikeIndexName As String = "FindInsuranceLikeIndex"
    Public Const ACInsLikeIndexSQL As String = "spu_findins_like_index"

    ' Select FindInsurance SQL
    Public Const ACInsLikeRefHolderStored As Boolean = True
    Public Const ACInsLikeRefHolderName As String = "FindInsuranceLikeRefHolder"
    Public Const ACInsLikeRefHolderSQL As String = "spu_findins_like_ref_and_holder"

    ' Find Insurance from built query.
    Public Const ACInsFileFromQueryStored As Boolean = False
    Public Const ACInsFileFromQueryName As String = "FindInsuranceQuery"
    Public Const ACInsFileFromQuerySQL As String = ""

    ' Select Insurance From Vehicle Reg. SQL
    Public Const ACInsLikeRegistrationStored As Boolean = False
    Public Const ACInsLikeRegistrationName As String = "FindInsuranceLikeRegistration"
    Public Const ACInsLikeRegistrationSQL As String = "exec spu_findins_like_vehicle {source}, {list}"

    ' Select FindInsurance SQL
    Public Const ACInsRiskDetailsStored As Boolean = True
    Public Const ACInsRiskDetailsName As String = "FindInsuranceRiskDetails"
    Public Const ACInsRiskDetailsSQL As String = "spu_gii_findins_risk_details"

    ' Select GetVehicle SQL
    Public Const ACGetVehicleDetailsStored As Boolean = True
    Public Const ACGetVehicleDetailsName As String = "GetVehicleDetails"
    Public Const ACGetVehicleDetailsSQL As String = "spu_gii_get_vehicle_details"

    ' Select Insurance File Risk Link Details SQL
    Public Const ACSelectInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACSelectInsuranceFileRiskLinkDetailsName As String = "SelectInsuranceFileRiskLinkDetails"
    Public Const ACSelectInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_sel"

    ' Update Insurance File Risk Link Details SQL
    Public Const ACUpdateInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACUpdateInsuranceFileRiskLinkDetailsName As String = "UpdateInsuranceFileRiskLinkDetails"
    Public Const ACUpdateInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_upd" 'TN20010127 added extra parameter

    ' Update Insurance File Risk Link Details status flag SQL
    Public Const ACUpdateInsuranceFileRiskLinkDetailsStatusStored As Boolean = True
    Public Const ACUpdateInsuranceFileRiskLinkDetailsStatusName As String = "UpdateInsuranceFileRiskLinkDetailsStatus"
    Public Const ACUpdateInsuranceFileRiskLinkDetailsStatusSQL As String = "spu_delete_insurance_file_risk_link"

    Public Const ACIsReInsuranceAtRiskLevelStored As Boolean = False
    Public Const ACIsReInsuranceAtRiskLevelName As String = "IsReInsuranceAtRiskLevel"
    Public Const ACIsReInsuranceAtRiskLevelSQL As String = "SELECT is_ri_at_risk_level FROM Risk_Type" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                           " WHERE risk_type_id = {risk_type_id}"

    'TN20010420 Start
    Public Const ACGetPolicyRiskStored As Boolean = True
    Public Const ACGetPolicyRiskName As String = "Get all risks for this policy"
    Public Const ACGetPolicyRiskSQL As String = "spu_GetPolicyRisks"
    'TN20010420 End

    ' PW051102
    Public Const ACGetRiskDesc As Boolean = True
    Public Const ACGetRiskDescription As String = "Get risk descriptions"
    Public Const ACGetRiskDescriptionSQL As String = "spu_GetRiskDescription"

    ' RDC 14042004
    Public Const ACGetTranCurrencyStored As Boolean = True
    Public Const ACGetTranCurrencyDescription As String = "Get risk descriptions"
    Public Const ACGetTranCurrencySQL As String = "spu_GetTransactionCurrency"

    Public Const ACGetHasCurrencyChangedStored As Boolean = True
    Public Const ACGetHasCurrencyChangedName As String = "GetHasCurrencyChanged"
    Public Const ACGetHasCurrencyChangedSQL As String = "spu_Get_Has_Currency_Changed"


    'Start (Sriram P) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.1.1)

    Public Const ACAttachCoverNoteDetailsStored As Boolean = True
    Public Const ACAttachCoverNoteDetailsName As String = "Attach Cover Notes"
    Public Const ACAttachCoverNoteDetailsSQL As String = "spu_Attach_Cover_Notes"

    Public Const ACDetachCoverNoteDetailsStored As Boolean = True
    Public Const ACDetachCoverNoteDetailsName As String = "Detach Cover Notes"
    Public Const ACDetachCoverNoteDetailsSQL As String = "spu_Detach_Cover_Notes"

    'End (Sriram P) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.1.1)
End Module
