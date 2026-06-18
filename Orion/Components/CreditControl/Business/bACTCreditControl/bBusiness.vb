Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
Imports SharedFiles
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports SharedQuoteEngine

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' *****************************************************************
    ' Class Name: Business
    '
    ' Date: 03/10/2002
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              Credit Control Rules and Steps.
    '
    ' Edit History:
    '   09-02-2005 : MEvans : Merge bug fixes from 1.9 back into 1.8 version (including comments)
    ' RAW 24/02/2004 : CQ4106 : Prevent an error when policy is fully paid and no outstanding transactions for account
    '                           ADO converts bit columns from 1,0 to True,False - always convert True/False to 1,0
    ' RAW 06/05/2004 : CQ4811 : added extra validation to AutoCancel to prevent type mismatch errors
    ' RKS 04/07/2006 : added UseEffectiveDate to Credit Control Rule
    ' *****************************************************************


    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private m_sIsUnderwritingOrAgency As String = ""

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' SP to Select a list of Credit Control Rules
    Private Const ACSelAllRulesName As String = "SelAllCreditControlRule"
    Private Const ACSelAllRulesSQL As String = "spu_ACT_SelAll_Credit_Control_Rule"

    ' SP to Select a list of Credit Control Steps
    Private Const ACSelAllStepsName As String = "SelAllCreditControlStep"
    Private Const ACSelAllStepsSQL As String = "spu_ACT_SelAll_Credit_Control_Step"

    ' SP to Select a single Credit Control Rule
    Private Const ACSelRuleName As String = "SelectCreditControlRule"
    Private Const ACSelRuleSQL As String = "spu_ACT_Select_Credit_Control_Rule"

    ' SP to Select a single Credit Control Step
    Private Const ACSelStepName As String = "SelectCreditControlStep"
    Private Const ACSelStepSQL As String = "spu_ACT_Select_Credit_Control_Step"

    ' SP to Add a Credit Control Rule
    Private Const ACDirectAddRuleName As String = "AddCreditControlRule"
    Private Const ACDirectAddRuleSQL As String = "spu_ACT_Add_Credit_Control_Rule"

    ' SP to Add a Credit Control Step
    Private Const ACDirectAddStepName As String = "AddCreditControlStep"
    Private Const ACDirectAddStepSQL As String = "spu_ACT_Add_Credit_Control_Step"

    ' SP to Edit a Credit Control Rule
    Private Const ACDirectEditRuleName As String = "UpdateCreditControlRule"
    Private Const ACDirectEditRuleSQL As String = "spu_ACT_Update_Credit_Control_Rule"

    ' SP to Edit a Credit Control Step
    Private Const ACDirectEditStepName As String = "UpdateCreditControlRule"
    Private Const ACDirectEditStepSQL As String = "spu_ACT_Update_Credit_Control_Step"

    ' SP to Delete a Credit Control Rule
    Private Const ACDirectDeleteRuleName As String = "DeleteCreditControlRule"
    Private Const ACDirectDeleteRuleSQL As String = "spu_ACT_Delete_Credit_Control_Rule"

    ' SP to Delete a Credit Control Step
    Private Const ACDirectDeleteStepName As String = "DeleteCreditControlStep"
    Private Const ACDirectDeleteStepSQL As String = "spu_ACT_Delete_Credit_Control_Step"

    ' SP to Find out if a policy is paid
    Private Const ACIsPolicyPaidName As String = "IsPolicyPaid"
    Private Const ACIsPolicyPaidSQL As String = "spu_ACT_Is_Policy_Paid"

    ' SP to Find out if a policy is paid - Broking version
    Private Const ACIsPolicyPaidBrokingName As String = "IsPolicyPaidBroking"
    Private Const ACIsPolicyPaidBrokingSQL As String = "spu_ACT_Is_Policy_Paid_B"


    ' MEVANS ADDED ***********
    ' SP to Find out if a policy is paid
    Private Const ACHasPolicyLiveMTAName As String = "HasPolicyLiveMTA"
    Private Const ACHasPolicyLiveMTASQL As String = "spu_ACT_Has_Policy_LiveMTA"
    ' MEVANS ADDED ***********

    ' SP to Take Credit Control Records off hold
    Private Const ACTakeOffHoldName As String = "TakeOffHold"
    Private Const ACTakeOffHoldSQL As String = "spu_ACT_Credit_Control_Take_Off_Hold"

    ' SP to Get details for use when running the auto-cancel rules script
    Private Const ACGetItemDetailsForScriptName As String = "GetItemDetailsForScript"
    Private Const ACGetItemDetailsForScriptSQL As String = "spu_ACT_Get_CC_Item_Insurance_File_Dets"

    ' SP to Get party name from account
    Private Const ACGetPartyNameFromAccountName As String = "GetPartyNameFromAccount"
    Private Const ACGetPartyNameFromAccountSQL As String = "spu_ACT_Get_Party_Name_From_Account"

    ' SP to find out how much of the policy has been paid
    Private Const ACGetPolicyPaidToDateName As String = "GetPolicyPaidToDate"
    Private Const ACGetPolicyPaidToDateSQL As String = "spu_ACT_Get_Instalments_Paid_To_Date"

    ' SP to get the Credit Control letters to print for client
    Private Const ACGetCreditControlDocIDsName As String = "GetCreditControlDocIDs"
    Private Const ACGetCreditControlDocIDsSQL As String = "spu_ACT_Get_Credit_Control_Doc_IDs"

    ' SP to get the Other Interested Parties
    Private Const ACGetOtherInterestedPartiesName As String = "GetOtherInterestedParties"
    Private Const ACGetOtherInterestedPartiesSQL As String = "spu_ACT_Get_Other_Interested_Parties"

    ' Existing SPs (i.e. not created specifically for this component)...

    ' SP to get a unique session id
    Private Const ACGetSessionIDName As String = "GetSessionID"
    Private Const ACGetSessionIDSQL As String = "spu_pm_session_id_alloc"

    ' SP to add a record to the Temp ID List table
    Private Const ACTempIDListAddName As String = "TempIDListAdd"
    Private Const ACTempIDListAddSQL As String = "spu_TempIDList_add"

    ' SP to clear the Temp ID List table
    Private Const ACTempIDListClearName As String = "TempIDListClear"
    Private Const ACTempIDListClearSQL As String = "spu_TempIDList_clear"

    ' SP to release the unique session id
    Private Const ACReleaseSessionIDName As String = "ReleaseSessionID"
    Private Const ACReleaseSessionIDSQL As String = "spu_pm_session_id_free"

    ' SP to get the Account details
    Private Const ACGetAccountDetailsName As String = "SelectAccount"
    Private Const ACGetAccountDetailsSQL As String = "spu_ACT_select_Account"

    'SP to check weather the document has been generated or not
    Private Const kCheckAutoCancellationDocumentName As String = "CheckAutoCancellationDocument"
    Private Const kCheckAutoCancellationDocumentSQL As String = "spu_Act_Check_AutoCancellation_Document"

    Public Const ACSelDoctTemplateTypeIDStored As Boolean = False
    Public Const ACSelDoctTemplateTypeIDName As String = "GetDocumentTemplateTypeID"
    Public Const ACSelDoctTemplateTypeIDSQL As String = "SELECT document_template_id, document_type_id FROM document_template" & Strings.Chr(13) & Strings.Chr(10) &
                                               "WHERE code = {code}" & Strings.Chr(13) & Strings.Chr(10)
    Public Const ACSelEmailContactStored As Boolean = True
    Public Const ACSelEmailContactName As String = "Get Main Email ContactAddress"
    Public Const ACSelEmailContactSQL As String = "spu_email_contact_select"

    Public Const ACSelPreferredCorrespondenceStored As Boolean = True
    Public Const ACSelPreferredCorrespondenceName As String = "Get Preferred Correspondence Address"
    Public Const ACSelPreferredCorrespondenceSQL As String = "spu_Get_Preferred_Correspondence_Address"

    Public Const ACSelBranchEmailContactStored As Boolean = False
    Public Const ACSelBranchEmailContactName As String = "Get Branch Email Address"
    Public Const ACSelBranchEmailContactSQL As String = "select email  from Source where source_id = {source_id}"

    'Result Array columns for GetDetails for Credit Control Rule
    Private Const ACRCreditControlRuleID As Integer = 0
    Private Const ACRDescription As Integer = 1
    Private Const ACRSourceID As Integer = 2
    Private Const ACRBusinessType As Integer = 3
    Private Const ACRPFFrequencyID As Integer = 4
    Private Const ACRPFFrequencyDescription As Integer = 5
    Private Const ACRIsActive As Integer = 6
    Private Const ACRProcssingDays As Integer = 7
    Private Const ACRUseEffectiveDate As Integer = 8
    Private Const ACRUseGreaterTransEffDate As Integer = 9
    Private Const ACRPFInstalmentsResultId As Integer = 10
    Private Const ACRIsPolicyPaid As Integer = 11
    Private Const ACRProductID As Integer = 12
    Private Const ACRUseDueDate As Integer = 13
    Private Const kUseInceptionDateForAutoCancel As Integer = 14

    'Result Array columns for GetDetails for Credit Control Step
    Private Const ACSCreditControlStepID As Integer = 0
    Private Const ACSCreditControlRuleID As Integer = 1
    Private Const ACSStepNumber As Integer = 2
    Private Const ACSNumberOfDays As Integer = 3
    Private Const ACSBrokerDays As Integer = 4
    Private Const ACSClientDocumentTemplateID As Integer = 5
    Private Const ACSOIPDocumentTemplateID As Integer = 6
    Private Const ACSBrokerReportID As Integer = 7
    Private Const ACSPolicyToleranceAmount As Integer = 8
    Private Const ACSAccountToleranceAmount As Integer = 9
    Private Const ACSPMWrkTaskID As Integer = 10
    Private Const ACSPMUserGroupID As Integer = 11
    Private Const ACSCheckAutoCancel As Integer = 12
    Private Const ACSAutoCancelPolicy As Integer = 13
    Private Const ACSNextStep As Integer = 14
    Private Const ACSPreviousStep As Integer = 15
    Private Const ACSOffHoldStep As Integer = 16
    Private Const ACSRecurringDays As Integer = 17
    Private Const ACSRecurringLetters As Integer = 18
    Private Const ACSJumpToNextStep As Integer = 19
    Private Const ACSClientDocumentTemplateID2 As Integer = 20
    Private Const ACSOIPDocumentTemplateID2 As Integer = 21
    Private Const ACSPMWrkTaskID2 As Integer = 22
    Private Const ACSPMUserGroupID2 As Integer = 23
    Private Const ACSPMActionType As Integer = 24
    Private Const ACSPMActionType2 As Integer = 25
    Private Const ACSTolerancePercentage1 As Integer = 26
    Private Const ACSTolerancePercentage2 As Integer = 27
    Private Const ACSStepDescription As Integer = 28
    Private Const ACSPMWrkTaskGroupId As Integer = 29
    Private Const ACSBrokerLetterId As Integer = 30
    Private Const ACSStopAccount As Integer = 31
    Private Const ACSAutoLapseRenewal As Integer = 32
    Private Const ACSInstalmentFailureCount As Integer = 33

    Private Const ACSAutoCancelDoc1Trigger As Integer = 34
    Private Const ACSAutoCancelDoc2Trigger As Integer = 35
    Private Const ACSAutoCancelDoc1 As Integer = 36
    Private Const ACSAutoCancelDoc2 As Integer = 37
    Private Const ACSWriteOffTolerance As Integer = 38
    Private Const ACSWriteOffReasonId As Integer = 39
    Private Const kJumpToNextStepBroker As Integer = 40
    Private Const kJumpToNextStepBrokerSingleInst As Integer = 41
    Private Const kAccountNoofDaysSingleInst As Integer = 42
    Private Const kAccountToleranceAmountSingleInst As Integer = 43
    Private Const kBrokerLetterIDSingleInst As Integer = 44

    ' Constants for add or edit
    Private Const ACUpdateAdd As Byte = 1
    Private Const ACUpdateEdit As Byte = 2

    ' Constants for transaction array
    Private Const ACTransDetailID As Integer = 0
    Private Const ACAmount As Integer = 1
    Private Const ACCurrencyAmount As Integer = 2

    ' Constants for item details array
    Private Const ACBusinessType As Integer = 0
    Private Const ACServiceLevel As Integer = 1
    Private Const ACPolicyVersion As Integer = 2
    Private Const ACClaimRegistered As Integer = 3
    Private Const ACCoverStartDate As Integer = 4
    Private Const ACCoverEndDate As Integer = 5
    Private Const kInceptionDateTPI As Integer = 6

    ' Constants match amount array
    Private Const ACMAAmount As Integer = 0
    Private Const ACMAFullyMatched As Integer = 1
    Private Const ACMAMatchAmount As Integer = 2
    Private Const ACMAHasLiveMTA As Integer = 3

    ' Constants for Credit Control Doc IDs array
    Private Const ACDIClientDocTemplateID As Integer = 0
    Private Const ACDIOIPDocTemplateID As Integer = 1
    Private Const ACDIPartyCnt As Integer = 2
    Private Const ACDIInsuranceFileCnt As Integer = 3
    Private Const ACDIBusinessType As Integer = 4
    Private Const ACDIBrokerDocTemplateId As Integer = 5
    Private Const ACDICreditControlItemId As Integer = 6
    Private Const ACDIInsuranceFolderCnt As Integer = 7
    Private Const ACDIDocumentTypeId As Integer = 8
    Private Const ACDIClientDocTemplateDesc As Integer = 9
    Private Const ACDIBrokerDocTemplateDesc As Integer = 9

    ' Constatnt for document printing mode
    Private Const ACPrintSilentMode As Integer = 3
    Private Const ACSpoolDocMode As Integer = 4

    Public Property IsCalledFromBatchProcessing() As Boolean = False

    Public Const ACLeadAgentCnt As Integer = 15
    Public Const ACEmailDocType As Integer = 8
    Public Const ACEMailSubTemplateCode As Integer = 11
    Public Const ACEMailAttachementTemplateCodes As Integer = 12
    Public Const ACCorrespondenceType As Integer = 13
    Public Const ACDefaultPreferredCorrespondenceType As Integer = 14
    Public Const ACIsAgentReceiveCorrespondence As Integer = 15

    Public Const ACAutoCancelLeadAgentCnt As Integer = 5
    Public Const ACAutoCancelEmailDocType As Integer = 8
    Public Const ACAutoCancelEMailSubTemplateCode As Integer = 1
    Public Const ACAutoCancelEMailAttachementTemplateCodes As Integer = 2
    Public Const ACAutoCancelCorrespondenceType As Integer = 3
    Public Const ACAutoCancelDefaultPreferredCorrespondenceType As Integer = 4
    Public Const ACAutoCancelIsAgentReceiveCorrespondence As Integer = 5

    ''' <summary>
    ''' Run the auto cancel rules VBScript.
    ''' </summary>
    ''' <param name="v_lCreditControlItemId"></param>
    ''' <param name="v_bCheckRulesOnly"></param>
    ''' <param name="r_bAutoCancelResult"></param>
    ''' <param name="v_bArchiveDoc"></param>
    ''' <param name="v_bSpoolDoc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AutoCancel(ByVal v_lCreditControlItemId As Integer, ByVal v_bCheckRulesOnly As Boolean, ByRef r_bAutoCancelResult As Boolean, Optional ByVal v_bArchiveDoc As Boolean = False, Optional ByVal v_bSpoolDoc As Boolean = False) As Integer

        Dim result As Integer = 0

        Dim vCreditControlRuleID As Object
        Dim oCreditControlItem As bACTCreditControlItem.Business
        Dim oAccount As bACTAccount.Form
        Dim oEventLog As bSIREvent.Business
        Dim vCreditControlStepID As Object
        Dim vDocumentID As Object
        Dim vInsuranceFileCnt As Object
        Dim vAccountID As Object
        Dim vCanAutoCancel As Object
        Dim vPMUserGroupID As Object
        Dim vPMUserGroupID2, vActionType, vActionType2, vTolerancePercentage1, vTolerancePercentage2 As Object
        Dim oPremiumFinance As bSIRPremiumFinance.Business
        Dim vPMWrkTaskID As Object
        Dim vPMWrkTaskID2 As Object
        Dim vBusinessType As String = ""
        Dim vPFPremiumFinanceCnt As Object
        Dim vPFPremiumFinanceVersion As Object
        Dim bFullyPaid, bPartiallyPaid As Boolean
        Dim cAmountOwing, cCredit As Decimal
        Dim vResultArray(,) As Object
        Dim sCustomer As String
        Dim dCoverStartDate, dCoverExpiryDate As Date
        Dim cTotalPaidToDate, cTotalPremium As Decimal
        Dim dLapsedDate As Date
        Dim lPartyCnt As Integer
        Dim vAccountCredit As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oTransactions As Object = Nothing
        Dim dOutstandingBalance As Decimal
        Dim bSingleInstalment As Boolean
        Dim oResultArrayForSIP As Object = Nothing

        Dim dtFirstInstallmentFailureDate As Date
        Dim bUsedInstalmentDueDate As Boolean
        Dim bIsRenewedPolicy As Boolean
        Dim dtInception_date_tpi As Date

        Dim lRelaxationPeriod As Integer = 0
        Dim ENPolicyCancellationCriteria As SharedStorage.PolicyCancellationDateCriteria = SharedStorage.PolicyCancellationDateCriteria.PremiumReceivedDate
        Dim sOptionValue As String = String.Empty
        Dim bUseInceptionDateForAutoCancel As Boolean
        Dim dtInceptionDateTPI As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bAutoCancelResult = False ' RAW 24/02/2004 : CQ4106 : added just in case we exit before setting it

            ' Create an instance of the CreditControlItem business object

            oCreditControlItem = New bACTCreditControlItem.Business
            m_lReturn = oCreditControlItem.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the Account business object

            oAccount = New bACTAccount.Form
            m_lReturn = oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Credit Control Item details for the passed ID

            m_lReturn = oCreditControlItem.GetDetails(v_lCreditControlItemId:=v_lCreditControlItemId, r_vAccountID:=vAccountID, r_vDocumentID:=vDocumentID, r_vInsuranceFileCnt:=vInsuranceFileCnt, r_vPFPremFinanceCnt:=vPFPremiumFinanceCnt, r_vPFPremFinanceVersion:=vPFPremiumFinanceVersion, r_vCanAutoCancel:=vCanAutoCancel, r_vCreditControlStepID:=vCreditControlStepID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 06/05/2004 : CQ4811 : added
            ' validate CCI details
            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And vInsuranceFileCnt > 0 Then
                ' this is ok
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="InsuranceFileCnt missing from credit control item", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancel")
                Return result
            End If

            Dim dbNumericTemp2 As Double
            If Double.TryParse(CStr(vAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) And vAccountID > 0 Then
                ' this is ok
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="AccountID missing from credit control item", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancel")
                Return result
            End If
            ' RAW 06/05/2004 : CQ4811 : end

            ' Get the credit control step details for this item
            m_lReturn = GetStepDetails(v_lCreditControlStepId:=vCreditControlStepID, r_vCreditControlRuleID:=vCreditControlRuleID, r_vPMWrkTaskID:=vPMWrkTaskID, r_vPMWrkTaskID2:=vPMWrkTaskID2, r_vPMUserGroupID:=vPMUserGroupID, r_vPMUserGroupID2:=vPMUserGroupID2, r_vActionType:=vActionType, r_vActionType2:=vActionType2, r_vTolerancePercentage1:=vTolerancePercentage1, r_vTolerancePercentage2:=vTolerancePercentage2)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the credit control rule details for this item

            m_lReturn = CType(GetRuleDetails(v_lCreditControlRuleId:=CInt(vCreditControlRuleID), r_vBusinessType:=vBusinessType), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 06/05/2004 : CQ4811 : added
            ' validate CCI details
            If vBusinessType = "INS" Then

                Dim dbNumericTemp4 As Double
                Dim dbNumericTemp3 As Double
                If Double.TryParse(CStr(vPFPremiumFinanceCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) And vPFPremiumFinanceCnt > 0 And Double.TryParse(CStr(vPFPremiumFinanceVersion), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) And vPFPremiumFinanceVersion > 0 Then
                    ' this is ok
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Premium Finance details missing from credit control item linked to an instalment rule", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancel")
                    Return result
                End If

            End If
            ' RAW 06/05/2004 : CQ4811 : end

            ' Get the party count...

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the account id parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(vAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement to get the account details
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAccountDetailsSQL, sSQLName:=ACGetAccountDetailsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' store the party count
            If Information.IsArray(vResultArray) Then

                lPartyCnt = CInt(Conversion.Val(CStr(vResultArray(48, 0))))
            End If

            ' Find out if item is partially paid
            m_lReturn = CType(IsPolicyPaid(v_lInsuranceFileCnt:=vInsuranceFileCnt, r_bIsPaidInFull:=bFullyPaid, r_bIsPartiallyPaid:=bPartiallyPaid, r_cAmountOwing:=cAmountOwing), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bFullyPaid Then
                ' nothing to cancel
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' get the total amount of any credits currently against
            ' this account that have not been allocated.
            lReturn = CType(GetAccountCreditAmounts(v_lAccountId:=vAccountID, r_vResults:=vAccountCredit), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAccountCreditAmounts failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancel")
                Return result
            End If

            ' store the accounts credit balance
            If Information.IsArray(vAccountCredit) Then

                cCredit = CDec(vAccountCredit(0, 0))
                cCredit = Math.Abs(cCredit)
            Else
                cCredit = 0
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Source ID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement to get the other details via the insurance file
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetItemDetailsForScriptSQL, sSQLName:=ACGetItemDetailsForScriptName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Store the cover start date
            If Information.IsDate(vResultArray(ACCoverStartDate, 0)) Then

                dCoverStartDate = CDate(vResultArray(ACCoverStartDate, 0))
            End If

            ' Store the cover end date
            If Information.IsDate(vResultArray(ACCoverEndDate, 0)) Then

                dCoverExpiryDate = CDate(vResultArray(ACCoverEndDate, 0))
            End If

            If Information.IsDate(vResultArray.GetValue(kInceptionDateTPI, 0)) Then
                dtInceptionDateTPI = ToSafeDate(vResultArray(kInceptionDateTPI, 0))
            End If
            m_lReturn = CheckSingleInstalmentPlan(v_lCreditControlItemId, bSingleInstalment)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("AutoCancel", "CheckSingleInstalmentPlan Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If bSingleInstalment Then

                m_lReturn = GetDetailsForAutoAllocate(v_lCreditControlItemId, oResultArrayForSIP)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("AutoCancel", "GetDetailsForAutoAllocate Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = GetOutstandingBalanceAndTransactionsForSingleInstalmentPlan(
                    ToSafeInteger(oResultArrayForSIP(0, 0)), ToSafeInteger(oResultArrayForSIP(1, 0)), dOutstandingBalance, oTransactions)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("AutoCancel", "GetOutstandingBalanceAndTransactionsForInsuranceFolder Failed",
                               gPMConstants.PMELogLevel.PMLogError)
                End If
            Else
                m_lReturn = GetOutstandingBalanceAndTransactionsForInsuranceFolder(vInsuranceFileCnt, dOutstandingBalance, oTransactions)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("AutoCancel", "GetOutstandingBalanceAndTransactionsForInsuranceFolder Failed",
                               gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                            v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=GeneralConst.kSystemOptionRuleTypeCreditControl,
                                            r_sOptionValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sOptionValue = "1" Then
                m_lReturn = ExecuteRuleFile(vResultArray:=vResultArray, vBusinessType:=vBusinessType, cCredit:=cCredit, vCanAutoCancel:=vCanAutoCancel, bPartiallyPaid:=bPartiallyPaid,
                            r_bAutoCancelResult:=r_bAutoCancelResult, lRelaxationPeriod:=lRelaxationPeriod, bUsedInstalmentDueDate:=bUsedInstalmentDueDate, ENPolicyCancellationCriteria:=ENPolicyCancellationCriteria)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                m_lReturn = ExecuteRulesCompiled(vResultArray:=vResultArray, vBusinessType:=vBusinessType, cCredit:=cCredit, vCanAutoCancel:=vCanAutoCancel, bPartiallyPaid:=bPartiallyPaid,
                            r_bAutoCancelResult:=r_bAutoCancelResult, lRelaxationPeriod:=lRelaxationPeriod, ENPolicyCancellationCriteria:=ENPolicyCancellationCriteria,
                            bUsedInstalmentDueDate:=bUsedInstalmentDueDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            ' If not only checking rules then check status of autocancel flag
            If Not v_bCheckRulesOnly Then

                ' If not autocancel then create a work manager task
                If Not r_bAutoCancelResult Then

                    ' Get the customer name
                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    ' Add the account id parameter
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(vAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement to get the customer name
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyNameFromAccountSQL, sSQLName:=ACGetPartyNameFromAccountName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If Information.IsArray(vResultArray) Then

                        sCustomer = CStr(vResultArray(0, 0))
                    Else
                        sCustomer = "Customer not found."
                    End If

                    ' Create the work mgr task
                    ' PW151203 - CQ3541 - if no group or task specified, then
                    ' do not create a task
                    If vPMUserGroupID <> "" And vPMWrkTaskID <> "" Then
                        m_lReturn = CType(CreateWorkManagerTask(v_lPMUserGroupID:=CInt(vPMUserGroupID), v_lPMWrkTaskID:=vPMWrkTaskID, v_sCustomer:=sCustomer, v_sDescription:="Manual Debt Review", v_dtTaskDueDate:=DateTime.Now), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    ' If autocancel, then cancel the policy...
                Else
                    ' Calculate lapsed date, if on instalments
                    If vBusinessType = "INSTALMENTS" Then

                        ' Get the premium finance details - how much paid

                        ' Clear the Database Parameters Collection
                        m_oDatabase.Parameters.Clear()

                        ' Add the Premium Finance key parameters
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="pfpremium_finance_cnt", vValue:=CStr(vPFPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="pfpremium_finance_version", vValue:=CStr(vPFPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="FirstInstallmentFailureDate", vValue:=DateTimeHelper.ToString(dtFirstInstallmentFailureDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDate)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        ' Execute SQL Statement to get the payment status
                        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyPaidToDateSQL, sSQLName:=ACGetPolicyPaidToDateName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        dtFirstInstallmentFailureDate = gPMFunctions.ToSafeDate(m_oDatabase.Parameters.Item("FirstInstallmentFailureDate").Value)

                        If Information.IsArray(vResultArray) Then

                            cTotalPremium = Conversion.Val(CStr(vResultArray(0, 0)))

                            cTotalPaidToDate = Conversion.Val(CStr(vResultArray(1, 0)))

                            dtInception_date_tpi = gPMFunctions.ToSafeDate(vResultArray(2, 0))

                            bIsRenewedPolicy = False
                            If bUsedInstalmentDueDate And cTotalPaidToDate = 0 Then
                                m_lReturn = CType(CheckForRenewedPolicy(v_lInsuranceFileCnt:=vInsuranceFileCnt, r_bIsRenewedPolicy:=bIsRenewedPolicy), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    AutoCancel = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If
                            End If

                            If bIsRenewedPolicy Then

                                dLapsedDate = dCoverStartDate

                            Else

                                If ENPolicyCancellationCriteria = SharedStorage.PolicyCancellationDateCriteria.PremiumReceivedDate Then
                                    ' Work out lapsed date:
                                    'Add [(total paid/total prem) * policy days] to start date
                                    If cTotalPremium <> 0 Then
                                        dLapsedDate = dtInception_date_tpi.AddDays(((cTotalPaidToDate / cTotalPremium) * DateAndTime.DateDiff("d", dtInception_date_tpi, dCoverExpiryDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)) + lRelaxationPeriod)
                                    Else
                                        dLapsedDate = dtInception_date_tpi.AddDays((DateAndTime.DateDiff("d", dtInception_date_tpi, dCoverExpiryDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1)) + lRelaxationPeriod)
                                    End If
                                ElseIf ENPolicyCancellationCriteria = SharedStorage.PolicyCancellationDateCriteria.InstallmentFailureDate Then
                                    dLapsedDate = dtFirstInstallmentFailureDate.AddDays(lRelaxationPeriod)
                                ElseIf ENPolicyCancellationCriteria = SharedStorage.PolicyCancellationDateCriteria.SystemDate Then
                                    dLapsedDate = DateTime.Today
                                ElseIf ENPolicyCancellationCriteria = SharedStorage.PolicyCancellationDateCriteria.PolicyStartDate Then
                                    dLapsedDate = dtInception_date_tpi
                                End If

                            End If

                        Else

                            ' If nothing returned from previous SQL, then no instalments
                            ' have been paid so set lapsed date to start date
                            dLapsedDate = dCoverStartDate

                        End If

                    Else
                        ' Not on instalments so just use the start date as the
                        ' lapsed date
                        dLapsedDate = dCoverStartDate

                    End If

                    If bUseInceptionDateForAutoCancel OrElse dLapsedDate < dtInceptionDateTPI Then
                        dLapsedDate = dtInceptionDateTPI
                    End If
                    dLapsedDate = dLapsedDate.Date
                    ' Cancel the policy
                    ' PW161203 - Pass lapsed date
                    m_lReturn = CType(CancelPolicy(lCreditControlItemID:=v_lCreditControlItemId, dLapsedDate:=dLapsedDate), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Set the status of installment to Failed as its cancelled
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If IsNumeric(vPFPremiumFinanceCnt) = True And
                           gPMFunctions.ToSafeInteger(vPFPremiumFinanceCnt) > 0 And
                           IsNumeric(vPFPremiumFinanceVersion) = True And
                           gPMFunctions.ToSafeInteger(vPFPremiumFinanceVersion) > 0 Then

                            oPremiumFinance = New bSIRPremiumFinance.Business
                            m_lReturn = oPremiumFinance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError("AutoCancel", "CreateBusinessObject to bSIRPremiumFinance Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            m_lReturn = oPremiumFinance.UpdateInstalmentStatusForPlan(v_lPfPremFinanceCnt:=vPFPremiumFinanceCnt,
                                                  v_lPfPremFinanceVersion:=vPFPremiumFinanceVersion,
                                                  v_sPFInstalmentsStatusCode:="F",
                                                  v_bDuringCCCancellation:=True)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError("AutoCancel", "UpdateInstalmentStatusForPlan Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            ' Kill the event object
                            oPremiumFinance.Dispose()
                        End If
                    End If
                    oPremiumFinance = Nothing
                    ' Generate event log entry

                    ' Create an instance of the EventLog business object

                    oEventLog = New bSIREvent.Business
                    m_lReturn = oEventLog.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add an event

                    m_lReturn = oEventLog.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vEventType:=5, vUserID:=m_iUserID, vEventDate:=DateTime.Now, vDescription:="Auto Cancelled")

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Kill the event object

                    oEventLog.Dispose()
                    oEventLog = Nothing

                End If
            End If

            ' Kill the account object

            oAccount.Dispose()

            ' Kill the credit control item object

            oCreditControlItem.Dispose()

            oAccount = Nothing
            oCreditControlItem = Nothing

            lReturn = CType(ProcessPostPlanCancellationActions(v_lCreditControlItemId, lPartyCnt, vInsuranceFileCnt, vCreditControlStepID, v_bArchiveDoc, v_bSpoolDoc, dOutstandingBalance), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to run AutoCancel method successfully", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ''' <summary>
    ''' execute VBscript .rul file base on Rule type
    ''' </summary>
    ''' <param name="vResultArray"></param>
    ''' <param name="vBusinessType"></param>
    ''' <param name="cCredit"></param>
    ''' <param name="vCanAutoCancel"></param>
    ''' <param name="bPartiallyPaid"></param>
    ''' <param name="r_bAutoCancelResult"></param>
    ''' <param name="lRelaxationPeriod"></param>
    ''' <param name="ENPolicyCancellationCriteria"></param>
    ''' <param name="bUsedInstalmentDueDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteRuleFile(ByVal vResultArray(,) As Object, ByRef vBusinessType As String, ByVal cCredit As Decimal, ByVal vCanAutoCancel As Object, ByVal bPartiallyPaid As Boolean,
                                    ByRef r_bAutoCancelResult As Boolean, ByRef lRelaxationPeriod As Integer, ByRef ENPolicyCancellationCriteria As SharedStorage.PolicyCancellationDateCriteria,
                                    ByRef bUsedInstalmentDueDate As Boolean) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oSharedStorage As SharedStorage
        Dim sMethodName As String = "start"
        Dim oVBQuoteEngine As SharedQuoteEngine.VBQuoteEngine

        Dim sScript As String = String.Empty

        ENPolicyCancellationCriteria = SharedStorage.PolicyCancellationDateCriteria.PremiumReceivedDate

        ' Create script control object
        oVBQuoteEngine = New VBQuoteEngine()
        ' Create shared storage object, used to hold values that are
        ' read/writable from the VB script file
        oSharedStorage = New SharedStorage()

        ' Set the properties of the Shared Storage object
        If vBusinessType = "INS" Or vBusinessType = "INSH" Or vBusinessType = "INSC" Then
            vBusinessType = "INSTALMENTS"
        Else
            ' PW181103 - CQ3099 - trim strings from result array

            If CStr(vResultArray(ACBusinessType, 0)).Trim() = "DIRECT" Then
                vBusinessType = "DIRECT"
            Else
                vBusinessType = "BROKER"
            End If
        End If

        oSharedStorage.BusinessType = vBusinessType
        ' PW181103 - CQ3099 - trim strings from result array

        oSharedStorage.ServiceLevel = CStr(vResultArray(ACServiceLevel, 0)).Trim()
        oSharedStorage.Credit = cCredit
        oSharedStorage.CanAutoCancel = (Conversion.Val(vCanAutoCancel) = 1)

        oSharedStorage.PolicyVersion = CInt(Conversion.Val(CStr(vResultArray(ACPolicyVersion, 0))))

        oSharedStorage.ClaimRegistered = (CStr(vResultArray(ACClaimRegistered, 0)).Trim() <> "")
        oSharedStorage.PartiallyPaid = bPartiallyPaid

        ' Read in the script and run it
        nResult = CType(GetScriptFile(v_sScript:=sScript), gPMConstants.PMEReturnCode)

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sScript = Replace(sScript, "oSharedStorage.Credit", "Round(oSharedStorage.Credit)")

        oVBQuoteEngine.RunMediaTypeValidation(sScript, sMethodName, oSharedStorage)

        ' Retrieve autocancel flag
        r_bAutoCancelResult = oSharedStorage.AutoCancel

        lRelaxationPeriod = oSharedStorage.RelaxationPeriod
        ENPolicyCancellationCriteria = oSharedStorage.ENPolicyCancellationCriteria
        bUsedInstalmentDueDate = oSharedStorage.Used_Instalment_Due_Date
        ' Kill the script and shared storage objects
        oSharedStorage = Nothing
        oVBQuoteEngine = Nothing

        Return nResult
    End Function

    ''' <summary>
    ''' execute compiled rules base on Rule type
    ''' </summary>
    ''' <param name="vResultArray"></param>
    ''' <param name="vBusinessType"></param>
    ''' <param name="cCredit"></param>
    ''' <param name="vCanAutoCancel"></param>
    ''' <param name="bPartiallyPaid"></param>
    ''' <param name="r_bAutoCancelResult"></param>
    ''' <param name="lRelaxationPeriod"></param>
    ''' <param name="ENPolicyCancellationCriteria"></param>
    ''' <param name="bUsedInstalmentDueDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteRulesCompiled(ByVal vResultArray(,) As Object, ByVal vBusinessType As String, ByVal cCredit As Decimal,
                                          ByVal vCanAutoCancel As Object, ByVal bPartiallyPaid As Boolean, ByRef r_bAutoCancelResult As Boolean, ByRef lRelaxationPeriod As Integer,
                                          ByRef ENPolicyCancellationCriteria As SharedStorage.PolicyCancellationDateCriteria, ByRef bUsedInstalmentDueDate As Boolean) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oSharedStorage As SharedStorage
        Dim oRules As Object
        Dim sAssemblyClassName As String = String.Empty

        ENPolicyCancellationCriteria = SharedStorage.PolicyCancellationDateCriteria.PremiumReceivedDate

        ' Create shared storage object, used to hold values that are
        ' read/writable from the VB script file
        oSharedStorage = New SharedStorage()

        ' Set the properties of the Shared Storage object
        If vBusinessType = "INS" Or vBusinessType = "INSH" Or vBusinessType = "INSC" Then
            vBusinessType = "INSTALMENTS"
        Else
            ' PW181103 - CQ3099 - trim strings from result array
            If CStr(vResultArray(ACBusinessType, 0)).Trim() = "DIRECT" Then
                vBusinessType = "DIRECT"
            Else
                vBusinessType = "BROKER"
            End If
        End If

        oSharedStorage.BusinessType = vBusinessType
        ' PW181103 - CQ3099 - trim strings from result array

        oSharedStorage.ServiceLevel = CStr(vResultArray(ACServiceLevel, 0)).Trim()
        oSharedStorage.Credit = cCredit
        oSharedStorage.CanAutoCancel = (Conversion.Val(vCanAutoCancel) = 1)
        oSharedStorage.PolicyVersion = CInt(Conversion.Val(CStr(vResultArray(ACPolicyVersion, 0))))
        oSharedStorage.ClaimRegistered = (CStr(vResultArray(ACClaimRegistered, 0)).Trim() <> "")
        oSharedStorage.PartiallyPaid = bPartiallyPaid

        ' Read in the script and run it
        nResult = CType(GetCompiledRuleFile(v_sScript:=sAssemblyClassName), gPMConstants.PMEReturnCode)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sAssemblyClassName = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)
        If Not (oRules Is Nothing) Then
            oRules.oSharedStorage = oSharedStorage
            oRules.Start()

            ' Retrieve autocancel flag
            r_bAutoCancelResult = oRules.oSharedStorage.AutoCancel
            lRelaxationPeriod = oRules.oSharedStorage.RelaxationPeriod
            ENPolicyCancellationCriteria = oRules.oSharedStorage.ENPolicyCancellationCriteria
            bUsedInstalmentDueDate = oRules.oSharedStorage.Used_Instalment_Due_Date
        End If

        ' Kill the script and shared storage objects
        oSharedStorage = Nothing

        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: ProcessPostPlanCancellationActions
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-09-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Private Function ProcessPostPlanCancellationActions(ByVal v_lCreditControlItemId As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lCreditControlStepId As Integer,
                                                        ByVal v_bArchiveDoc As Boolean, ByVal v_bSpoolDoc As Boolean, ByVal crOutstandingBalance As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessPostPlanCancellationActions"

        Const kInsuranceFileStatusCodeCancelled As String = "CAN"


        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sInsuranceFileStatus As String = ""

        Dim vAutoCancelDoc1Id, vAutoCancelDoc2Id As Object
        Dim vAutoCancelDoc1TriggerAmount As String = ""
        Dim vAutoCancelDoc2TriggerAmount As String = ""
        Dim vWriteOffToleranceAmount, vWriteOffReasonId, vTransactions As Object

        Dim crWriteOffAmount, crOutstandingBalanceAmount As Decimal
        Dim nFound As Integer
        Dim bSingleInstalment As Boolean
        Dim iCount As Integer
        Dim oResultArray As Object
        Dim oResultArrayForSIP As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ensure the policy was actually cancelled
            ' prior to performing any function actions
            lReturn = CType(GetInsuranceFileStatus(v_lInsuranceFileCnt, sInsuranceFileStatus), gPMConstants.PMEReturnCode)

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                If sInsuranceFileStatus = kInsuranceFileStatusCodeCancelled Then

                    'get credit control step details that apply on auto cancellation
                    lReturn = GetStepDetails(v_lCreditControlStepId:=v_lCreditControlStepId, r_vAutoCancelDoc1Id:=vAutoCancelDoc1Id, r_vAutoCancelDoc2Id:=vAutoCancelDoc2Id, r_vAutoCancelDoc1TriggerAmount:=vAutoCancelDoc1TriggerAmount, r_vAutoCancelDoc2TriggerAmount:=vAutoCancelDoc2TriggerAmount, r_vWriteOffToleranceAmount:=vWriteOffToleranceAmount, r_vWriteOffReasonId:=vWriteOffReasonId)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetStepDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = CheckSingleInstalmentPlan(v_lCreditControlItemId, bSingleInstalment)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "CheckSingleInstalmentPlan Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If bSingleInstalment Then
                        m_lReturn = GetDetailsForAutoAllocate(v_lCreditControlItemId, oResultArrayForSIP)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "GetDetailsForAutoAllocate Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        m_lReturn = GetOutstandingBalanceAndTransactionsForSingleInstalmentPlan(ToSafeInteger(oResultArrayForSIP(0, 0)),
                                                                                              ToSafeInteger(oResultArrayForSIP(1, 0)),
                                                                                              crOutstandingBalanceAmount, vTransactions)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "GetOutstandingBalanceAndTransactionsForInsuranceFolder Failed",
                                       gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else

                        lReturn = CType(GetOutstandingBalanceAndTransactionsForInsuranceFolder(v_lInsuranceFileCnt, crOutstandingBalanceAmount, vTransactions), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "GetOutstandingBalanceAndTransactionsForInsuranceFolder Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    m_lReturn = AutoAllocate(oTransactions:=vTransactions,
                                             crOutstandingBalance:=crOutstandingBalanceAmount,
                                             oWriteOffToleranceAmount:=CStr(vWriteOffToleranceAmount),
                                             oWriteOffReasonId:=vWriteOffReasonId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "AutoAllocate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If bSingleInstalment Then

                        m_lReturn = GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration(v_lCreditControlItemId, oResultArray)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If IsArray(oResultArray) Then

                            nFound = 0
                            m_lReturn = CheckAutoCancellationDocument(nInsuranceFileCnt:=oResultArray(1, iCount), nFound:=nFound)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "CheckAutoCancellationDocument Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            If nFound = 0 Then
                                m_lReturn = GenerateAutoCancellationDocuments(oResultArray(0, iCount),
                                                                            v_lInsuranceFileCnt,
                                                                            v_lCreditControlItemId,
                                                                            crOutstandingBalance,
                                                                            vAutoCancelDoc1Id,
                                                                            vAutoCancelDoc1TriggerAmount,
                                                                            vAutoCancelDoc2Id,
                                                                            vAutoCancelDoc2TriggerAmount,
                                                                            v_bArchiveDoc,
                                                                            v_bSpoolDoc,
                                                                            bSingleInstalment)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    RaiseError(kMethodName, "GenerateAutoCancelDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If
                            End If
                        End If
                    Else
                        nFound = 0
                        m_lReturn = CheckAutoCancellationDocument(nInsuranceFileCnt:=v_lInsuranceFileCnt, nFound:=nFound)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "CheckAutoCancellationDocument Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        If nFound = 0 Then
                            m_lReturn = CType(GenerateAutoCancellationDocuments(v_lPartyCnt, v_lInsuranceFileCnt,
                                                                                v_lCreditControlItemId,
                                                                                crOutstandingBalance,
                                                                                vAutoCancelDoc1Id,
                                                                                vAutoCancelDoc1TriggerAmount,
                                                                                vAutoCancelDoc2Id,
                                                                                vAutoCancelDoc2TriggerAmount,
                                                                                v_bArchiveDoc,
                                                                                v_bSpoolDoc), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "GenerateAutoCancelDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    End If
                End If
            End If
        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetOriginalInsuranceFileCnt " & "Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GenerateAutoCancellationDocuments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-09-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Private Function GenerateAutoCancellationDocuments(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lCreditControlItemId As Integer, ByVal v_crOutstandingBalance As Decimal, ByVal v_vAutoCancelDoc1Id As Object, ByVal v_vAutoCancelDoc1TriggerAmount As String, ByVal v_vAutoCancelDoc2Id As Object, ByVal v_vAutoCancelDoc2TriggerAmount As String, ByVal v_bArchiveDocuments As Boolean, ByVal v_bSpoolDocuments As Boolean,
                                                       Optional ByVal bSingleInstalment As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GenerateAutoCancellationDocuments"

        Dim lReturn As gPMConstants.PMEReturnCode


        Dim crTriggerAmount As Decimal
        Dim lDocumentTemplateId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vCreditControlItems(0) As Object

            vCreditControlItems(0) = v_lCreditControlItemId

            'if outstanding amount >= trigger 1 produce document 1
            If v_vAutoCancelDoc1TriggerAmount <> "" Then

                crTriggerAmount = gPMFunctions.ToSafeCurrency(v_vAutoCancelDoc1TriggerAmount, 0)

                If v_crOutstandingBalance >= crTriggerAmount Then

                    lDocumentTemplateId = gPMFunctions.ToSafeLong(v_vAutoCancelDoc1Id, 0)

                    If lDocumentTemplateId <> 0 Then

                        lReturn = CType(ProduceAutoCancellationLetter(lDocumentTemplateId, "Auto Cancellation Document Printed", v_lCreditControlItemId, v_lPartyCnt, v_lInsuranceFileCnt, vCreditControlItems, v_bSpoolDocuments, v_bArchiveDocuments, bSingleInstalment), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "ProduceAutoCancellationLetter Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                End If

            End If

            'if outstanding amount <= trigger 2 produce document 2

            If v_vAutoCancelDoc2TriggerAmount <> "" Then

                crTriggerAmount = gPMFunctions.ToSafeCurrency(v_vAutoCancelDoc2TriggerAmount, 0)

                If v_crOutstandingBalance <= crTriggerAmount Then

                    lDocumentTemplateId = gPMFunctions.ToSafeLong(v_vAutoCancelDoc2Id, 0)

                    If lDocumentTemplateId <> 0 Then

                        lReturn = CType(ProduceAutoCancellationLetter(lDocumentTemplateId, "Auto Cancellation Document Printed", v_lCreditControlItemId, v_lPartyCnt, v_lInsuranceFileCnt, vCreditControlItems, v_bSpoolDocuments, v_bArchiveDocuments, bSingleInstalment), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "ProduceAutoCancellationLetter Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                End If

            End If

        Catch excep As Exception
            ' DO Not Call any functions before here or the error will be lost

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetOriginalInsuranceFileCnt " & "Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message,
                               excep:=excep)
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CalculateWriteOffAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-09-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Private Function CalculateWriteOffAmount(ByVal v_crOutstandingBalance As Decimal, ByVal vWriteOffToleranceAmount As String, ByRef r_crWriteOffAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim crWriteOffTolerance As Decimal




        result = gPMConstants.PMEReturnCode.PMTrue

        ' if a write off amount has been supplied
        If vWriteOffToleranceAmount <> "" Then

            crWriteOffTolerance = gPMFunctions.ToSafeCurrency(vWriteOffToleranceAmount)

            If crWriteOffTolerance >= Math.Abs(v_crOutstandingBalance) Then
                r_crWriteOffAmount = v_crOutstandingBalance
            End If

        Else
            r_crWriteOffAmount = 0
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ProcessAutoAllocationAndWriteOff
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-09-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Private Function ProcessAutoAllocationAndWriteOff(ByVal v_vTransactions(,) As Object, ByVal v_crWriteOffAmount As Decimal, ByVal v_lWriteOffReasonId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessAutoAllocationAndWriteOff"
        Const kAllowPartialAllocation As Boolean = True
        Const kThisIsNotACurrencyWriteOff As Boolean = False
        Const kNoCashListItemSpecified As Integer = 0
        Const kAmountToBeAllocated As Integer = 2
        Const kTransDetailId As Integer = 1
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oAutoAllocation As bACTImportSiriusTrans.Business
        Dim nWriteOffTransDetailId As Integer = 0
        Dim oTransactionArray(,) As Object
        Dim nWriteOffTransDetailIndex As Integer = 0
        Dim crOutstanding As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the business object to perform the auto allocation

            oAutoAllocation = New bACTImportSiriusTrans.Business
            lReturn = oAutoAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If Information.IsArray(v_vTransactions) AndAlso v_crWriteOffAmount <> 0 Then

                For iCount As Integer = 0 To v_vTransactions.GetUpperBound(1)
                    If CDbl(v_vTransactions(kAmountToBeAllocated, iCount)) > 0 Then
                        crOutstanding += CDec(v_vTransactions(kAmountToBeAllocated, iCount))
                    End If
                Next

                For lCount As Integer = 0 To v_vTransactions.GetUpperBound(1)

                    If Math.Sign(CDbl(v_vTransactions(kAmountToBeAllocated, lCount))) <> Math.Sign(crOutstanding) Then

                        If Math.Abs(crOutstanding) <> 0 AndAlso Math.Abs(crOutstanding) < Math.Abs(CDbl(v_vTransactions(kAmountToBeAllocated, lCount))) Then

                            v_vTransactions(kAmountToBeAllocated, lCount) = -crOutstanding
                            crOutstanding = 0
                            If nWriteOffTransDetailId = 0 Then
                                nWriteOffTransDetailId = v_vTransactions(kTransDetailId, lCount)
                                nWriteOffTransDetailIndex = lCount
                            End If
                        ElseIf Math.Abs(crOutstanding) >= Math.Abs(CDbl(v_vTransactions(kAmountToBeAllocated, lCount))) Then

                            crOutstanding -= Math.Abs(CDbl(v_vTransactions(kAmountToBeAllocated, lCount)))
                        End If
                    End If
                Next
                If crOutstanding > 0 Then


                    v_vTransactions(kAmountToBeAllocated, 0) = CDbl(v_vTransactions(kAmountToBeAllocated, 0)) - crOutstanding
                    If nWriteOffTransDetailId = 0 Then
                        nWriteOffTransDetailId = v_vTransactions(kTransDetailId, 0)
                    End If
                End If
            End If
            If nWriteOffTransDetailId <> 0 AndAlso v_crWriteOffAmount <> 0 Then
                ReDim oTransactionArray(v_vTransactions.GetUpperBound(0), v_vTransactions.GetUpperBound(1))
                For iCount As Integer = 0 To v_vTransactions.GetUpperBound(0)
                    oTransactionArray(iCount, 0) = v_vTransactions(iCount, nWriteOffTransDetailIndex)
                Next
                Dim iCounter As Integer = 1
                For iCount As Integer = 0 To v_vTransactions.GetUpperBound(1)
                    If v_vTransactions(1, iCount) <> nWriteOffTransDetailId Then
                        For iCnt As Integer = 0 To v_vTransactions.GetUpperBound(0)
                            oTransactionArray(iCnt, iCounter) = v_vTransactions(iCnt, iCount)
                        Next
                        iCounter += 1
                    End If
                Next
                v_vTransactions = oTransactionArray
            End If
            ' perform allocation and write off

            lReturn = oAutoAllocation.AutoAllocateCancellingTransactions(v_vTransactions, kAllowPartialAllocation, v_crWriteOffAmount, v_lWriteOffReasonId, kThisIsNotACurrencyWriteOff, kNoCashListItemSpecified)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bACTImportSiriusTrans.Business.AutoAllocateCancellingTransactions Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            oAutoAllocation = Nothing

        Catch excep As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                sMsg:="GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration " & "Failed", vApp:=ACApp, vClass:=ACClass,
                                vMethod:="GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration", vErrNo:=Information.Err().Number,
                                vErrDesc:=excep.Message, excep:=excep)
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetOutstandingBalanceAndTransactionsForInsuranceFolder
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 25-09-2007: ADDACS Phase II
    ' ***************************************************************** '
    Private Function GetOutstandingBalanceAndTransactionsForInsuranceFolder(ByVal v_lInsuranceFileCnt As Integer, ByRef r_crOutstandingBalance As Decimal, ByRef r_vResults(,) As Object,
                                                                            Optional ByVal v_iViaCreditControl As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetOutstandingBalanceAndTransactionsForInsuranceFolder"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddOutputParameter(v_sName:="outstandingbalance", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            If v_iViaCreditControl <> 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="nViaCreditControl", v_vValue:=v_iViaCreditControl, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            End If

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetOutstandingBalanceAndTransactionsForInsuranceFolderSQL, sSQLName:=kGetOutstandingBalanceAndTransactionsForInsuranceFolderSQL, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetOutstandingBalanceAndTransactionsForInsuranceFolderSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' return the outstanding balance
            r_crOutstandingBalance = gPMFunctions.ToSafeCurrency(m_oDatabase.Parameters.Item("outstandingbalance").Value, 0)

        Catch excep As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration " & "Failed",
                               vApp:=ACApp,
                               vClass:=ACClass,
                               vMethod:="GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration",
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceFileStatus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-09-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Private Function GetInsuranceFileStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sInsuranceFileStatusCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInsuranceFileStatus"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute selection Query
        lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInsuranceFileStatusSQL, sSQLName:=kGetInsuranceFileStatusSQL, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kGetInsuranceFileStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Information.IsArray(vResults) Then

            r_sInsuranceFileStatusCode = CStr(vResults(0, 0)).Trim().ToUpper()
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result
    End Function

    '*****************************************************************
    ' Name: AutoCancelReport (Public)
    '
    ' Description: This function runs the Auto Cancellation report.
    '
    ' History: PW060103 - created
    '          PW151003 - Change to create the report via a document
    '*****************************************************************
    Public Function AutoCancelReport(ByVal v_vCreditControlItems As Object) As Integer

        Dim result As Integer = 0
        Dim lSessionID As Integer
        Dim sFileName As String = ""
        ' PW151003 - CQ2732
        'Developer Guide No 108
        Dim oDocMgrWrapper As bSIRDocManagerWrapper.Interface_Renamed

        ' PW151003 - CQ2732
        ' Const kAutoCancelDocTemplateCode As String = "AUTOCANCEL"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' Create a Session and store the passed Credit Control Item IDs in it
            '
            m_lReturn = CType(CreateTempSession(v_vCreditControlItems:=v_vCreditControlItems, r_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            ' Run the Auto Cancellation Report
            '
            ' PW151003 - CQ2732 - Create a document with an embedded report
            ' rather than creating the report directly: Start

            ' Create an instance of the Document Manager Wrapper business object
            'Developer Guide No 108
            oDocMgrWrapper = New bSIRDocManagerWrapper.Interface_Renamed()

            'Pass the BatchProcessingFlag to document wrapper class
            oDocMgrWrapper.IsCalledFromBatchProcessing = IsCalledFromBatchProcessing
            m_lReturn = CType(oDocMgrWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

            ' Check for error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oDocMgrWrapper.InitialiseBusiness failed.", vApp:=ACApp, vClass:="AutoCancelReport", vMethod:="ProduceReportDocuments")
                oDocMgrWrapper.Dispose()
                oDocMgrWrapper = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' MEVANS : IMPORTANT NB: NEED TO UNCOMMENT THIS CODE
            ' AFTER RAMS CHANGES TO DOC PRODUCTION ARE CHECKED IN

            ' Pass the details to the DocMgrWrapper object
            '    oDocMgrWrapper.DocumentTemplateCode = kAutoCancelDocTemplateCode
            '    oDocMgrWrapper.SessionID = lSessionID
            '    oDocMgrWrapper.Mode = ACSpoolDocMode
            '    oDocMgrWrapper.SpoolDesc = kAutoCancelDocTemplateCode & _
            ''            " - automatically spooled"
            '
            '    ' Start the DocMgrWrapper component
            '    m_lReturn = oDocMgrWrapper.Start()
            '
            '    If m_lReturn <> PMTrue Then
            '        AutoCancelReport = PMFalse
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to Start the DocManagerWrapper component.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="AutoCancelReport", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            ' Terminate the DocMgrWrapper component
            oDocMgrWrapper.Dispose()
            oDocMgrWrapper = Nothing

            ' PW151003 - CQ2732: End
            '
            ' Clear and close the Session
            '
            m_lReturn = CType(ReleaseTempSession(v_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to run AutoCancelReport method successfully", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function


    '************************************************************************
    ' Name: CreateTempSession (private)
    '
    ' Description: Gets the next unique Session ID and adds the passed Credit
    '              Control Item ID's to the TempLinkID table.
    '
    ' Created: PW090103
    '************************************************************************
    Private Function CreateTempSession(ByVal v_vCreditControlItems As Object, ByRef r_lSessionID As Integer) As Integer

        Dim result As Integer = 0
        ' RAW 12/10/2004 : CQ4811 : changed from integer to long
        Dim lLBound As Integer ' RAM20041012 : CQ4811 : Declared this variable
        Dim lUBound As Integer ' RAM20041012 : CQ4811 : Declared this variable

        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
        '
        ' Get a unique session ID
        '
        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Session ID output parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="r_lSessionID", vValue:=CStr(r_lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement to get a unique session ID
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetSessionIDSQL, sSQLName:=ACGetSessionIDName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Retrieve the session ID
        If lRecordsAffected > 0 Then
            r_lSessionID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("r_lSessionID").Value)
        Else
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get a unique Session ID", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTempSession")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '
        ' Insert the passed array details into the Standard ID table
        '
        ' Loop through all elements of the array


        lLBound = v_vCreditControlItems.GetLowerBound(0) ' RAM20041012 : CQ4811 : Performance Enhancement

        lUBound = v_vCreditControlItems.GetUpperBound(0) ' RAM20041012 : CQ4811 : Pefrormance Enhancement

        For i As Integer = lLBound To lUBound

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Session ID input parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lSessionID", vValue:=CStr(r_lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Link ID (Credit Control Item) input parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lLinkID", vValue:=CStr(v_vCreditControlItems(i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement to add the item
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACTempIDListAddSQL, sSQLName:=ACTempIDListAddName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next

        Return result

    End Function

    '************************************************************************
    ' Name: CreateWorkManagerTask
    '
    ' Description: Creates a work manager task to remind the user to review
    ' the debt. Copied from bPMUFollowUpTasks and amended.
    '
    ' Created: PW030103
    '************************************************************************
    Public Function CreateWorkManagerTask(ByVal v_lPMUserGroupID As Integer, ByVal v_lPMWrkTaskID As String, ByVal v_sCustomer As String, ByVal v_sDescription As String, Optional ByVal v_dtTaskDueDate As Date = #12/30/1899#, Optional ByVal v_iTaskStatus As Integer = 0, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_dtDateCreated As Date = #12/30/1899#, Optional ByVal v_iCreatedByID As Integer = 1, Optional ByRef r_lPMWrkTaskInstanceCnt As Integer = 0, Optional ByRef v_iIsVisible As Integer = 1, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_lPMWrkTaskActionTypeId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim oWrkMgrTaskControl As bPMWrkTaskInstance.TaskControl
            Dim iUserID As Integer
            Dim lTaskInstanceCnt As Integer

            ' Initialise Required Variables
            result = gPMConstants.PMEReturnCode.PMTrue

            'Create the business Object
            oWrkMgrTaskControl = New bPMWrkTaskInstance.TaskControl()

            'Initialise with the Sirius user and password
            m_lReturn = CType(oWrkMgrTaskControl, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise WorkManager Task Business Object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set the value for the task
            ' Or get it from the Registry
            iUserID = m_iUserID

            'Create the WorkManager Task
            'Create the WorkManager Task
            'TODO: to be checked at runtime
            'm_lReturn = oWrkMgrTaskControl.CreateNew(v_lPMWrkTaskGroupID:=1, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iUserID:=iUserID, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=v_iIsVisible, v_lPMWrkTaskActionTypeId:=v_lPMWrkTaskActionTypeId) 
            m_lReturn = oWrkMgrTaskControl.CreateNew(v_lPMWrkTaskGroupID:=1, v_lPMWrkTaskID:=v_lPMWrkTaskID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=lTaskInstanceCnt, v_iUserID:=iUserID, v_vKeyArray:=v_vKeyArray, v_iIsVisible:=v_iIsVisible)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create WorkManager Task.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Work Manager task create successfully
            ' Assign the return value
            r_lPMWrkTaskInstanceCnt = lTaskInstanceCnt

            oWrkMgrTaskControl = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create WorkManager Task", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '************************************************************************
    ' Name: ReleaseTempSession (private)
    '
    ' Description: Releases the Session ID and removes the passed Credit
    '              Control Item ID's from the TempLinkID table.
    '
    ' Created: PW090103
    '************************************************************************
    Public Function ReleaseTempSession(ByVal v_lSessionID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' Clear the Standard ID table
            '
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Session ID input parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lSessionID", vValue:=CStr(v_lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement to clear the table
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACTempIDListClearSQL, sSQLName:=ACTempIDListClearName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '
            ' Release the unique Session ID
            '
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Session ID input parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lSessionID", vValue:=CStr(v_lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement to clear the table
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACReleaseSessionIDSQL, sSQLName:=ACReleaseSessionIDName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to process ReleaseTempSession method", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseTempSession", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property


    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property
    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    ' *****************************************************************
    ' Name: ProduceClientLetters (Public)
    '
    ' Description: Print letters to the client for any outstanding amounts.
    '              Use the document template specified in the Credit
    '              Control Step record
    '
    ' Created: PW080103
    '
    'AAB-18-Feb-2004 12:04 - Added those 2 variables they were created
    '                        before but got lost during the split
    ' MEvans : 22-02-2005 : Updated to produce broker letters as well
    '                        as client letters depending on ther business type
    '                        of the credit control item
    ' *****************************************************************
    Public Function ProduceClientLetters(ByVal v_vCreditControlItems As Object, Optional ByVal v_bSpoolDocuments As Boolean = False, Optional ByVal v_bArchiveDocuments As Boolean = False) As Integer

        Dim result As Integer = 0
        'Developer Guide No 108
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Dim oEventLog As bSIREvent.Business
        Dim lSessionID As Integer
        Dim vResultArray(,) As Object
        Dim lLBound, lUBound As Integer
        Dim bProduceLetter As Boolean
        Dim sEventDescription, sBusinessType As String
        Dim vFieldParams As Object
        ' RDC 17102005
        Dim lDocumasterID As Integer
        Dim lPartyCnt As Integer
        Dim m_oBusiness As New bSIRDocTemplate.Business
        Dim bSingleInstallmentPlan As Boolean
        Dim oResultArrayForSIP As Object
        Dim nInsuranceFolderCnt As Integer
        Dim sOptionValue As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocTemplate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return result
            End If
            ' Create a Session and store the passed Credit Control Item IDs in it
            m_lReturn = CType(CreateTempSession(v_vCreditControlItems:=v_vCreditControlItems, r_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Session ID parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="session_id", vValue:=CStr(lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Template ID etc. for each required letter
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCreditControlDocIDsSQL, sSQLName:=ACGetCreditControlDocIDsName, bStoredProcedure:=True, lNumberRecords:=SharedFiles.gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the Event Log business object

            oEventLog = New bSIREvent.Business
            m_lReturn = oEventLog.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an instance of the Document Manager Wrapper business object
            'Developer Guide No 108
            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
            oDocManagerWrapper.IsCalledFromBatchProcessing = IsCalledFromBatchProcessing
            m_lReturn = CType(oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocManagerWrapper.InitialiseBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Loop through the array
            If Information.IsArray(vResultArray) Then


                lLBound = vResultArray.GetLowerBound(1)

                lUBound = vResultArray.GetUpperBound(1)

                ' for each credit control item
                For i As Integer = lLBound To lUBound


                    lPartyCnt = CInt(Conversion.Val(CStr(vResultArray(ACDIPartyCnt, i))))
                    ' Set the appropriate properties
                    oDocManagerWrapper.DocumentTypeId = CInt(Conversion.Val(CStr(vResultArray(ACDIDocumentTypeId, i))))  ' standard letter
                    result = CheckSingleInstalmentPlan(Val(vResultArray(ACDICreditControlItemId, i)), bSingleInstallmentPlan)
                    result = GetInsuranceFolderCnt(ToSafeInteger(vResultArray(ACDIInsuranceFileCnt, i)), nInsuranceFolderCnt)
                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("ProduceClientLetters", "GetInsuranceFolderCnt Failed.", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If bSingleInstallmentPlan Then

                        m_lReturn = GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration(Val(vResultArray(ACDICreditControlItemId, i)), oResultArrayForSIP)

                        oDocManagerWrapper.PartyCnt = ToSafeInteger(oResultArrayForSIP(0, 0))
                        oDocManagerWrapper.InsuranceFileCnt = 0
                    Else

                        oDocManagerWrapper.PartyCnt = lPartyCnt
                        oDocManagerWrapper.InsuranceFileCnt = CInt(Conversion.Val(CStr(vResultArray.GetValue(ACDIInsuranceFileCnt, i))))
                        oDocManagerWrapper.InsuranceFolderCnt = nInsuranceFolderCnt
                    End If
                    oDocManagerWrapper.ArchiveDoc = v_bArchiveDocuments
                    'Documents generated from credit control will be visible from web only if CalledFromSAM is True.So setting it to true
                    'oDocManagerWrapper.CalledFromSAM = True 'TFS 6032


                    ' reset produce letter flag
                    bProduceLetter = False

                    ' get the business type

                    sBusinessType = CStr(vResultArray(ACDIBusinessType, i)).Trim().ToUpper()

                    ' if this is "direct business" the process should produce the client letter
                    If sBusinessType = "DIRECT" Then
                        ' if there is a valid client letter template id

                        If Conversion.Val(CStr(vResultArray(ACDIClientDocTemplateID, i))) > 0 Then
                            bProduceLetter = True
                        End If
                        ' this is broker business and the process should produce the broker letter
                    Else
                        ' if there is a valid broker letter template id

                        If Conversion.Val(CStr(vResultArray(ACDIBrokerDocTemplateId, i))) > 0 Then
                            bProduceLetter = True
                        End If
                    End If

                    Dim bSendEMailToClient As Boolean = False
                    Dim bSendEMailToAgent As Boolean = False

                    ' set default LETTER - maintain backward compatibility

                    Dim sCorrespondenceType As String = "LETTER"
                    Dim sDefaultPreferredCorrespondenceType As String = "LETTER"
                    Dim bIsAgentCorrespondence As Boolean = False

                    If ToSafeInteger(vResultArray(8, i)) = ACEmailDocType Then
                        If ToSafeString(vResultArray(ACCorrespondenceType, i)).Trim <> "" Then
                            sCorrespondenceType = ToSafeString(vResultArray(ACCorrespondenceType, i)).Trim
                        End If

                        If ToSafeString(vResultArray(ACDefaultPreferredCorrespondenceType, i)).Trim <> "" Then
                            sDefaultPreferredCorrespondenceType = ToSafeString(vResultArray(ACDefaultPreferredCorrespondenceType, i)).Trim
                        End If

                        bIsAgentCorrespondence = ToSafeBoolean(vResultArray(ACIsAgentReceiveCorrespondence, i))

                        bSendEMailToClient = True
                        bSendEMailToAgent = True


                        'No Agent business
                        If ToSafeInteger(vResultArray(ACLeadAgentCnt, i)) = 0 Then
                            bSendEMailToAgent = False
                            bIsAgentCorrespondence = False 'no correspondence to agent
                        End If

                        If bSendEMailToAgent Then
                            If Not bIsAgentCorrespondence OrElse (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType <> "MEMAIL") _
                                OrElse sCorrespondenceType = "LETTER" Then
                                bSendEMailToAgent = False
                            End If
                        End If

                        If bSendEMailToClient Then
                            If bIsAgentCorrespondence OrElse (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType <> "MEMAIL") _
                                OrElse sCorrespondenceType = "LETTER" Then
                                bSendEMailToClient = False
                            End If
                        End If

                        Dim vEmailDocsPathArray As Object

                        If bSendEMailToClient Or bSendEMailToAgent Then
                            Dim sDocMergedPath As String = ""

                            Dim sDocCode As String = ""
                            Dim iDocTemplateID As Integer = 0
                            Dim iDocTypeId As Integer = 0
                            Dim vEMailDocs As New List(Of String)
                            Dim vEMailDocPath As New List(Of String)
                            Dim bOutputAsPDF As Boolean = False
                            Dim bOutPutHTM As Boolean = False
                            Dim bOutPutTXT As Boolean = False

                            Dim sClientEmailAddress As String = String.Empty
                            Dim sAgentEmailAddress As String = String.Empty

                            Dim sAttachmentSTR() As String = Split(ToSafeString(vResultArray(ACEMailAttachementTemplateCodes, i)), ",")

                            ReDim vEmailDocsPathArray(sAttachmentSTR.GetUpperBound(0) + 2)

                            vEMailDocs.Add(ToSafeString(vResultArray(0, i)))
                            vEMailDocs.Add(ToSafeString(vResultArray(ACEMailSubTemplateCode, i)))

                            For iAttachmentCnt As Integer = 0 To sAttachmentSTR.GetUpperBound(0)
                                vEMailDocs.Add(ToSafeString(sAttachmentSTR(iAttachmentCnt).Trim))
                            Next

                            Dim sSubjectContent As String = ""

                            For iDocCnt As Integer = 0 To vEMailDocs.Count - 1
                                sDocCode = vEMailDocs.Item(iDocCnt)
                                iDocTemplateID = 0
                                iDocTypeId = 0
                                bOutputAsPDF = False
                                bOutPutHTM = False
                                bOutPutTXT = False
                                sDocMergedPath = String.Empty

                                If iDocCnt = 0 Then
                                    iDocTemplateID = ToSafeInteger(sDocCode)
                                    iDocTypeId = ToSafeInteger(vResultArray(8, i))
                                    bOutPutHTM = True
                                Else
                                    m_lReturn = GetDocTemplateTypeID(v_sDocCode:=sDocCode,
                                                              r_lDocTemplateTypeId:=iDocTemplateID,
                                                              r_lDocTypeID:=iDocTypeId)

                                    If iDocCnt = 1 Then
                                        bOutPutTXT = True
                                    Else
                                        bOutputAsPDF = True
                                    End If
                                End If
                                If iDocTemplateID <> 0 And iDocTypeId <> 0 Then
                                    m_lReturn = PrintDocument(v_lPartyCnt:=lPartyCnt,
                                                              v_lDocTemplateID:=iDocTemplateID,
                                                              v_sType:="",
                                                              r_sMergedFilePath:=sDocMergedPath,
                                                              v_lInsuranceFileCnt:=vResultArray(ACDIInsuranceFileCnt, i),
                                                              r_sDocDesc:="",
                                                              v_lDocumentTypeId:=iDocTypeId,
                                                              v_iProductionOrder:=0,
                                                              v_iMode:=gSIRLibrary.ACEmailMode, v_bCalledFromSAM:=False,
                                                              v_lInsuranceFolderCnt:=vResultArray(ACDIInsuranceFolderCnt, i),
                                                              v_sInsuranceFileRef:=vResultArray(ACInsuranceRef, i), v_bOutputAsHTM:=bOutPutHTM,
                                                              v_bOutPutAsTXT:=bOutPutTXT, r_bRetainTempFiles:=True,
                                                              v_bOutputAsPDF:=bOutputAsPDF)

                                    If iDocCnt = 1 AndAlso Not String.IsNullOrEmpty(sDocMergedPath) Then
                                        Dim srSubjectTmp As New StreamReader(File.OpenRead(sDocMergedPath))
                                        Dim sbsbTemplate As New StringBuilder(srSubjectTmp.ReadToEnd())
                                        sbsbTemplate.Replace(vbCrLf, " ")
                                        sSubjectContent = sbsbTemplate.ToString
                                    End If
                                    vEMailDocPath.Add(sDocMergedPath)
                                End If
                            Next

                            Dim v_Attachment As New List(Of String)

                            For iDocCnt As Integer = 2 To vEMailDocPath.Count - 1
                                v_Attachment.Add(vEMailDocPath(iDocCnt))
                            Next

                            m_lReturn = m_oBusiness.GetPolicyLevelEmailAddress(ToSafeInteger(Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, i)))), sClientEmailAddress)
                            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                                Return result
                            End If

                            If bSendEMailToClient AndAlso sBusinessType = "DIRECT" Then
                                sClientEmailAddress = String.Empty
                                If (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType = "MEMAIL") OrElse sCorrespondenceType = "EMAIL" Then
                                    If String.IsNullOrEmpty(sClientEmailAddress) Then
                                        m_lReturn = GetEmailAddress(lPartyCnt, "MEMAIL", sClientEmailAddress)
                                    End If
                                Else
                                    m_lReturn = GetBranchEmailAddress(m_iSourceID, sClientEmailAddress)
                                End If
                                If Not String.IsNullOrEmpty(sClientEmailAddress) Then
                                    m_lReturn = m_oBusiness.SendEMail(v_sTo:=sClientEmailAddress,
                                                                      v_sSubject:=sSubjectContent,
                                                                      v_sMessagePath:=vEMailDocPath(0),
                                                                      v_sAttachment:=v_Attachment.ToArray)

                                    bProduceLetter = False

                                    sEventDescription = "Credit Control Client Email Sent."
                                End If
                            End If

                            If bSendEMailToAgent AndAlso sBusinessType <> "DIRECT" Then
                                sClientEmailAddress = String.Empty
                                If (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType = "MEMAIL") OrElse sCorrespondenceType = "EMAIL" Then
                                    m_lReturn = GetEmailAddress(ToSafeInteger(vResultArray(ACLeadAgentCnt, i)), "MEMAIL", sClientEmailAddress)
                                Else
                                    m_lReturn = GetBranchEmailAddress(m_iSourceID, sClientEmailAddress)
                                End If
                                If Not String.IsNullOrEmpty(sClientEmailAddress) Then
                                    m_lReturn = m_oBusiness.SendEMail(v_sTo:=sClientEmailAddress,
                                                                     v_sSubject:=sSubjectContent,
                                                                     v_sMessagePath:=vEMailDocPath(0),
                                                                     v_sAttachment:=v_Attachment.ToArray)

                                    bProduceLetter = False

                                    sEventDescription = "Credit Control Broker Email Sent."
                                End If
                            End If

                        End If
                    End If

                    ' if there is enough information to produce the letter
                    ' ( a valid document template id)
                    If bProduceLetter Then
                        'print doc in loop fashion from mail attachments
                        Dim iUbound As Integer = 0
                        Dim bAttachmentFound As Boolean = False

                        Dim sAttachmentSTR() As String

                        If ToSafeInteger(vResultArray(8, i)) = ACEmailDocType Then
                            sAttachmentSTR = Split(ToSafeString(vResultArray(ACEMailAttachementTemplateCodes, i)), ",")
                            iUbound = sAttachmentSTR.GetUpperBound(0)
                            If ToSafeString(vResultArray(ACEMailAttachementTemplateCodes, i)).Trim <> "" Then
                                bAttachmentFound = True
                            End If
                        End If

                        Dim bDocSpooled As Boolean = False

                        For iTmpCnt As Integer = 0 To iUbound
                            bDocSpooled = False
                            Dim iDocTemplateID As Integer = 0
                            Dim iDocTypeId As Integer = 0

                            If bAttachmentFound Then
                                Dim sDocCode As String = sAttachmentSTR(iTmpCnt).Trim


                                m_lReturn = GetDocTemplateTypeID(v_sDocCode:=sDocCode,
                                                             r_lDocTemplateTypeId:=iDocTemplateID,
                                                             r_lDocTypeID:=iDocTypeId)
                            Else
                                iDocTemplateID = ToSafeInteger(vResultArray(0, i))
                                iDocTypeId = ToSafeInteger(vResultArray(8, i))
                            End If


                            If sBusinessType = "DIRECT" Then

                                oDocManagerWrapper.DocumentTemplateId = CInt(Conversion.Val(CStr(vResultArray(ACDIClientDocTemplateID, i))))
                                oDocManagerWrapper.SpoolDesc = ToSafeString(vResultArray(ACDIClientDocTemplateDesc, i))
                                If v_bSpoolDocuments Then
                                    sEventDescription = "Credit Control Client Letter generated."
                                Else
                                    sEventDescription = "Credit Control Client Letter printed."
                                End If
                            Else

                                oDocManagerWrapper.DocumentTemplateId = CInt(Conversion.Val(CStr(vResultArray(ACDIBrokerDocTemplateId, i))))
                                oDocManagerWrapper.SpoolDesc = ToSafeString(vResultArray(ACDIBrokerDocTemplateDesc, i))
                                If v_bSpoolDocuments Then
                                    sEventDescription = "Credit Control Broker Letter generated."
                                Else
                                    sEventDescription = "Credit Control Broker Letter printed."
                                End If
                            End If


                            ReDim vFieldParams(1, 0)

                            vFieldParams(0, 0) = "CreditControlItem"


                            vFieldParams(1, 0) = Conversion.Val(CStr(vResultArray(ACDICreditControlItemId, i)))


                            oDocManagerWrapper.FieldParameters = vFieldParams

                            If v_bSpoolDocuments Then
                                oDocManagerWrapper.Mode = ACSpoolDocMode
                            Else
                                oDocManagerWrapper.Mode = ACPrintSilentMode
                            End If

                            ' Produce the Client Letters
                            m_lReturn = oDocManagerWrapper.Start()

                            ' RDC 17102005
                            lDocumasterID = oDocManagerWrapper.DocumasterID

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = m_lReturn
                                ' Log Error Message
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to print the document", vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceClientLetters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                Return result
                            End If

                            ' Generate an event log entry

                            ' Add an event
                            ' RDC 17102005 add vDocumentCnt
                        Next
                    End If
                    If bProduceLetter Or bSendEMailToClient Or bSendEMailToAgent Then
                        m_lReturn = oEventLog.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFileCnt:=Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, i))), vEventType:=10, vUserId:=m_iUserID, vEventDate:=DateTime.Now, vDescription:=sEventDescription, vDocumentCnt:=lDocumasterID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If
                Next
            End If

            ' Kill the event object

            oEventLog.Dispose()
            oEventLog = Nothing

            ' Clear iPMBDocTemplate.Interface
            oDocManagerWrapper.Dispose()
            oDocManagerWrapper = Nothing
            '
            ' Clear and close the Session
            '
            m_lReturn = CType(ReleaseTempSession(v_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to run ProduceClientLetters method successfully", vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceClientLetters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Private Function GetEmailAddress(ByRef v_Partycnt As Integer, ByVal v_ContactType As String, ByRef v_EmailAddress As String) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_Partycnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_type", vValue:=v_ContactType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelEmailContactSQL, sSQLName:=ACSelEmailContactName, bStoredProcedure:=ACSelEmailContactStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim nlength As Integer = vResultArray.GetUpperBound(1)
            For nTemp As Integer = 0 To nlength
                If (nTemp = nlength) Then
                    v_EmailAddress += ToSafeString(vResultArray(2, nTemp))
                Else
                    v_EmailAddress += ToSafeString(vResultArray(2, nTemp)) + ";"
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEmailAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEmailAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Get the branch Email Address
    ''' </summary>
    ''' <param name="v_Source_Id"></param>
    ''' <param name="v_EmailAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBranchEmailAddress(ByRef v_Source_Id As Integer, ByRef v_EmailAddress As String) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_Source_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelBranchEmailContactSQL, sSQLName:=ACSelBranchEmailContactName, bStoredProcedure:=ACSelBranchEmailContactStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            v_EmailAddress = ToSafeString(vResultArray(0, 0))

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplateTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateTypeID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' Get the merged path of the document.
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_lDocTemplateID"></param>
    ''' <param name="v_sType"></param>
    ''' <param name="r_sMergedFilePath"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="r_sDocDesc"></param>
    ''' <param name="v_lDocumentTypeId"></param>
    ''' <param name="v_iIsClient"></param>
    ''' <param name="v_iIsAgent"></param>
    ''' <param name="v_iIsOffice"></param>
    ''' <param name="v_iProductionOrder"></param>
    ''' <param name="v_sSpoolDesc"></param>
    ''' <param name="v_bCalledFromSAM"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_sInsuranceFileRef"></param>
    ''' <param name="v_bOutputAsHTM"></param>
    ''' <param name="v_bOutPutAsTXT"></param>
    ''' <param name="r_bRetainTempFiles"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PrintDocument(ByVal v_lPartyCnt As Integer, ByVal v_lDocTemplateID As Integer, ByVal v_sType As String,
                                  ByRef r_sMergedFilePath As String, ByVal v_iMode As Integer,
                                  Optional ByVal v_lInsuranceFileCnt As Integer = 0,
                                  Optional ByRef r_sDocDesc As String = "",
                                  Optional ByVal v_lDocumentTypeId As Integer = 0,
                                  Optional ByVal v_iIsClient As Integer = 0, Optional ByVal v_iIsAgent As Integer = 0,
                                  Optional ByVal v_iIsOffice As Integer = 0,
                                  Optional ByVal v_iProductionOrder As Integer = 1,
                                  Optional ByVal v_sSpoolDesc As String = "",
                                  Optional ByVal v_bCalledFromSAM As Boolean = False,
                                  Optional ByVal v_lInsuranceFolderCnt As Integer = 0,
                                  Optional ByVal v_sInsuranceFileRef As String = "",
                                  Optional ByVal v_bOutputAsHTM As Boolean = True,
                                  Optional ByVal v_bOutPutAsTXT As Boolean = False,
                                  Optional ByRef r_bRetainTempFiles As Boolean = False,
                                  Optional ByVal v_bOutputAsPDF As Boolean = False) As Integer

        ''PN62462
        Dim result As Integer = 0
        Dim sTempFilePath As String = ""
        Dim sTempFileName As String = ""
        Dim sMergedFilePath As String = ""
        'developer guide no. 108
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 108
            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()

            m_lReturn = oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            With oDocManagerWrapper

                .InsuranceFolderCnt = v_lInsuranceFolderCnt
                .InsuranceFileRef = v_sInsuranceFileRef
                .PartyCnt = v_lPartyCnt
                .InsuranceFileCnt = v_lInsuranceFileCnt
                .OutputAsHTML = v_bOutputAsHTM
                .OutputAsTXT = v_bOutPutAsTXT
                .Mode = v_iMode
                .DocumentTemplateId = v_lDocTemplateID
                .DocumentTypeId = v_lDocumentTypeId
                .SpoolDesc = v_sSpoolDesc
                .IsClient = v_iIsClient
                .IsAgent = v_iIsAgent
                .IsOffice = v_iIsOffice
                .ProductionOrder = v_iProductionOrder
                .CalledFromSAM = v_bCalledFromSAM
                .RetainTempFiles = r_bRetainTempFiles
                m_lReturn = .Start()
                r_sMergedFilePath = .MergedFilePath
                r_sDocDesc = .DocumentTemplateDescription

            End With
            If v_bOutputAsPDF Then
                sTempFileName = String.Empty
                sTempFilePath = String.Empty

                If r_sMergedFilePath.ToUpper.EndsWith("XML") Then
                    sTempFileName = Path.GetFileNameWithoutExtension(r_sMergedFilePath)
                    sTempFilePath = r_sMergedFilePath.Remove(r_sMergedFilePath.Length - 3) & "PDF"
                    sTempFilePath = sTempFilePath.Replace(sTempFileName, r_sDocDesc)
                    ConvertDocumentUsingSiriusDocumentUtility(r_sMergedFilePath, sTempFilePath)
                End If
                r_sMergedFilePath = sTempFilePath
            End If
            oDocManagerWrapper.Dispose()

            oDocManagerWrapper = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Get document template id from code
    ''' </summary>
    ''' <param name="v_sDocCode"></param>
    ''' <param name="r_lDocTemplateTypeId"></param>
    ''' <param name="r_lDocTypeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDocTemplateTypeID(ByVal v_sDocCode As String, ByRef r_lDocTemplateTypeId As Integer, ByRef r_lDocTypeID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sDocCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'execute SQL statement
            result = m_oDatabase.SQLSelect(sSQL:=ACSelDoctTemplateTypeIDSQL, sSQLName:=ACSelDoctTemplateTypeIDName, bStoredProcedure:=ACSelDoctTemplateTypeIDStored, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lDocTemplateTypeId = CInt(vResultArray(0, 0))
            r_lDocTypeID = CInt(vResultArray(1, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplateTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateTypeID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: ProduceOIPLetters (Public)
    '
    ' Description: Print letters to the other interested parties for any
    '              outstanding amounts.
    '              Use the document template specified in the Credit
    '              Control Step record
    '
    ' Created: PW080103
    'AAB-18-Feb-2004 12:04 - Added those 2 variables they were created
    '                        before but got lost during the split
    ' *****************************************************************
    Public Function ProduceOIPLetters(ByVal v_vCreditControlItems As Object, Optional ByVal v_bSpoolDocuments As Boolean = False, Optional ByVal v_bArchiveDocuments As Boolean = False) As Integer

        Dim result As Integer = 0
        'Developer Guide No 108
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Dim oEventLog As bSIREvent.Business
        Dim lSessionID As Integer
        Dim vResultArray(,) As Object
        Dim vOIPArray(,) As Object
        ' RAW 12/10/2004 : CQ4811 : changed from integer to long
        ' RAW 12/10/2004 : CQ4811 : changed from integer to long
        ' RAW 12/10/2004 : CQ4811 : changed from integer to long
        Dim lLBound As Integer ' RAM20041012 : CQ4811 : Declared this variable
        Dim lUBound As Integer ' RAM20041012 : CQ4811 : Declared this variable
        Dim colOtherInterestedParties As Collection
        Dim lOtherInterestedParty As Integer
        Dim vFieldParams As Object




        result = gPMConstants.PMEReturnCode.PMTrue
        '
        ' Create a Session and store the passed Credit Control Item IDs in it
        '
        m_lReturn = CType(CreateTempSession(v_vCreditControlItems:=v_vCreditControlItems, r_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '
        ' Get the Template ID etc. for each required letter
        '
        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Session ID parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="session_id", vValue:=CStr(lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCreditControlDocIDsSQL, sSQLName:=ACGetCreditControlDocIDsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '
        ' Produce the OIP Letters
        '
        ' Create an instance of the Event Log business object

        oEventLog = New bSIREvent.Business
        m_lReturn = oEventLog.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Create an instance of the Document Manager Wrapper business object
        'Developer Guide No108
        oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
        m_lReturn = CType(oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocManagerWrapper.InitialiseBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Loop through the array
        If Information.IsArray(vResultArray) Then


            lLBound = vResultArray.GetLowerBound(1) ' RAM20041012 : CQ4811 : Declared this variable

            lUBound = vResultArray.GetUpperBound(1) ' RAM20041012 : CQ4811 : Declared this variable

            For i As Integer = lLBound To lUBound

                ' Only attempt to print if there is a document template ID

                If Conversion.Val(CStr(vResultArray(ACDIOIPDocTemplateID, 0))) > 0 Then

                    ' Get all the Other Interested Parties for this policy...

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    ' Add the Insurance File Cnt parameter

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, 0)))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement to get OIPs
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOtherInterestedPartiesSQL, sSQLName:=ACGetOtherInterestedPartiesName, bStoredProcedure:=True, vResultArray:=vOIPArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Information.IsArray(vOIPArray) Then

                        ' Go through the returned OIPs and add to a collection
                        ' This will get rid of any duplicates.
                        ' Don't add OIP's with a value of 0
                        colOtherInterestedParties = New Collection()

                        For iRow As Integer = vOIPArray.GetLowerBound(1) To vOIPArray.GetUpperBound(1)
                            For iCol As Integer = 0 To 4

                                lOtherInterestedParty = CInt(Conversion.Val(CStr(vOIPArray(iCol, iRow))))
                                If lOtherInterestedParty <> 0 Then
                                    Try
                                        colOtherInterestedParties.Add(lOtherInterestedParty, CStr(lOtherInterestedParty))

                                    Catch
                                    End Try


                                End If
                            Next
                        Next

                        ' Now create this letter for each Interested Party
                        For iRow As Integer = 1 To colOtherInterestedParties.Count

                            ' Set the appropriate properties

                            oDocManagerWrapper.DocumentTemplateId = CInt(Conversion.Val(CStr(vResultArray(ACDIOIPDocTemplateID, 0))))
                            oDocManagerWrapper.DocumentTypeId = CInt(Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, i)))) ' standard letter

                            oDocManagerWrapper.PartyCnt = CInt(colOtherInterestedParties(iRow))

                            oDocManagerWrapper.InsuranceFileCnt = CInt(Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, 0))))
                            oDocManagerWrapper.SpoolDesc = "Credit Control OIP Letter"

                            ReDim vFieldParams(1, 0)

                            vFieldParams(0, 0) = "CreditControlItem"


                            vFieldParams(1, 0) = Conversion.Val(CStr(vResultArray(ACDICreditControlItemId, i)))


                            oDocManagerWrapper.FieldParameters = vFieldParams

#If CODEBASE = 18 Then
                            ' START CHANGES - Changed By: AAB  - Changed On: 18-Feb-2004 12:10
                            '1.8 Platform prints directly
                            If v_bSpoolDocuments Then
                                oDocManagerWrapper.Mode = ACSpoolDocMode
                            Else
                                oDocManagerWrapper.Mode = ACPrintSilentMode
                            End If
                            'Set the value to archivedoc then start will archive it.
                            'oDocManagerWrapper.ArchiveDoc = v_bArchiveDocuments
                            ' END CHANGES - Changed By: AAB  - Changed On: 18-Feb-2004 12:10
#Else

                            '1.9 Platform prints to Document Spooler
                            oDocManagerWrapper.Mode = ACSpoolDocMode
#End If

                            ' Call the Start Method of iPMBDocTemplate.Interface
                            m_lReturn = oDocManagerWrapper.Start()

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = m_lReturn
                                ' Log Error Message
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to print the document", vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceOIPLetters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                Return result
                            End If

                            ' Generate an event log entry

                            ' Add an event


                            m_lReturn = oEventLog.DirectAdd(vPartyCnt:=colOtherInterestedParties(iRow), vInsuranceFileCnt:=Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, 0))), vEventType:=10, vUserID:=m_iUserID, vEventDate:=DateTime.Now, vDescription:="Credit Control OIP Letter printed")

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        Next
                    End If

                    ' Drop the collection of OIPs
                    colOtherInterestedParties = Nothing

                End If
            Next
        End If

        ' Kill the event object

        oEventLog.Dispose()
        oEventLog = Nothing

        ' Clear iPMBDocTemplate.Interface
        oDocManagerWrapper.Dispose()
        oDocManagerWrapper = Nothing
        '
        ' Clear and close the Session
        '
        m_lReturn = CType(ReleaseTempSession(v_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

Err_ProduceOIPLetters:

        ' Error.
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to run ProduceOIPLetters method successfully", vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceOIPLetters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function


    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' *****************************************************************
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' *****************************************************************
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sIsUnderwritingOrAgency), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("getUnderwritingOrAgency", "Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: IsPolicyPaid (Public)
    '
    ' Description: Check if a debt has been fully paid, partially paid,
    ' or not paid at all.
    '
    ' Created: PW030103
    '
    ' *****************************************************************
    Public Function IsPolicyPaid(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsPaidInFull As Boolean, ByRef r_bIsPartiallyPaid As Boolean, ByRef r_cAmountOwing As Decimal, Optional ByRef r_bHasLiveMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim cAmountDue As Decimal

        Dim cOutstandingAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Insurance File Cnt parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsPolicyPaidSQL, sSQLName:=ACIsPolicyPaidName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '**********************************************
            '**********************************************

            ' Go through the result array and tot up
            If Information.IsArray(vResultArray) Then

                ' get the accounts details

                cAmountDue = Conversion.Val(CStr(vResultArray(0, 0)))

                cOutstandingAmount = Conversion.Val(CStr(vResultArray(1, 0)))

                If cOutstandingAmount = 0 Then
                    r_bIsPaidInFull = True
                Else
                    If cOutstandingAmount <> cAmountDue Then
                        r_bIsPartiallyPaid = True
                    End If
                End If

                r_cAmountOwing = cOutstandingAmount

            End If


            '**********************************************
            '**********************************************
            ' this should only perform the bHasliveMTA
            ' checks if the parameter has been specified
            ' in the call....

            If Not Information.IsNothing(r_bHasLiveMTA) Then

                'DD 09/01/2004: Now determine if the Policy has a live MTA

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the Insurance File Cnt parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the Insurance File Cnt parameter

                'developer guide no 85. 
                m_lReturn = m_oDatabase.Parameters.Add(sName:="HasLiveMTA", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACHasPolicyLiveMTASQL, sSQLName:=ACHasPolicyLiveMTAName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Determine whether true or not
                r_bHasLiveMTA = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("HasLiveMTA").Value) = 1

            End If

            Return result

        Catch excep As System.Exception
            ' MEVANS ADDED



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to find out if the policy is paid", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPolicyPaid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' *****************************************************************
    ' Name: IsPolicyPaid (Public)
    '
    ' Description: Broking version of IsPolicyPaid
    '
    ' *****************************************************************
    Public Function IsPolicyPaidBroking(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lDocumentId As Integer, ByRef r_bIsPaidInFull As Boolean, ByRef r_bIsPartiallyPaid As Boolean, ByRef r_cAmountOwing As Decimal, Optional ByRef r_bHasLiveMTA As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim cAmountDue As Decimal

        Dim cOutstandingAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Insurance File Cnt parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Document ID parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", vValue:=CStr(v_lDocumentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsPolicyPaidBrokingSQL, sSQLName:=ACIsPolicyPaidBrokingName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '**********************************************
            '**********************************************

            ' Go through the result array and tot up
            If Information.IsArray(vResultArray) Then

                ' get the accounts details

                cAmountDue = Conversion.Val(CStr(vResultArray(0, 0)))

                cOutstandingAmount = Conversion.Val(CStr(vResultArray(1, 0)))

                If cOutstandingAmount = 0 Then
                    r_bIsPaidInFull = True
                Else
                    If cOutstandingAmount <> cAmountDue Then
                        r_bIsPartiallyPaid = True
                    End If
                End If

                r_cAmountOwing = cOutstandingAmount

            End If


            '**********************************************
            '**********************************************
            ' this should only perform the bHasliveMTA
            ' checks if the parameter has been specified
            ' in the call....

            If Not Information.IsNothing(r_bHasLiveMTA) Then

                'DD 09/01/2004: Now determine if the Policy has a live MTA

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                ' Add the Insurance File Cnt parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the Insurance File Cnt parameter

                'developer guide no. 
                m_lReturn = m_oDatabase.Parameters.Add(sName:="HasLiveMTA", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACHasPolicyLiveMTASQL, sSQLName:=ACHasPolicyLiveMTAName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Determine whether true or not
                r_bHasLiveMTA = gPMFunctions.NullToInteger(m_oDatabase.Parameters.Item("HasLiveMTA").Value) = 1

            End If

            Return result

        Catch excep As System.Exception
            ' MEVANS ADDED



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to find out if the policy is paid", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPolicyPaidBroking", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' *****************************************************************
    ' Name: TakeOffHold (Public)
    '
    ' Description: Update all Credit Control Item records for the passed
    ' account id, by setting the step_id to the off_hold_step.
    '
    ' *****************************************************************
    Public Function TakeOffHold(ByVal v_lAccountId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Account ID parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACTakeOffHoldSQL, sSQLName:=ACTakeOffHoldName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to process TakeOffHold method", vApp:=ACApp, vClass:=ACClass, vMethod:="TakeOffHold", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' *****************************************************************
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' *****************************************************************
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' *****************************************************************
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' *****************************************************************
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: GetRuleList (Public)
    '
    ' Description: Select multiple Credit Control Rule records from
    ' the database.
    '
    ' *****************************************************************
    Public Function GetRuleList(ByVal v_lSourceID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Source ID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllRulesSQL, sSQLName:=ACSelAllRulesName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Credit Control Rule records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' *****************************************************************
    ' Name: GetStepList (Public)
    '
    ' Description: Select multiple Credit Control Step records from
    ' the database.
    '
    ' *****************************************************************
    Public Function GetStepList(ByVal v_lCreditControlRuleId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Credit Control Rule ID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_rule_id", vValue:=CStr(v_lCreditControlRuleId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelAllStepsSQL, sSQLName:=ACSelAllStepsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Credit Control Step records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: GetStepDetails (Public)
    '
    ' Description: Select a single Credit Control Step record from the
    ' database.
    '
    ' *****************************************************************
    'Developer Guide No. 101
    Public Function GetStepDetails(ByVal v_lCreditControlStepId As Object, Optional ByRef r_vCreditControlRuleID As Object = Nothing, Optional ByRef r_vStepNumber As Object = Nothing, Optional ByRef r_vNumberOfDays As Object = Nothing, Optional ByRef r_vBrokerDays As Object = Nothing, Optional ByRef r_vClientDocumentTemplateID As Object = Nothing, Optional ByRef r_vClientDocumentTemplateID2 As Object = Nothing, Optional ByRef r_vOIPDocumentTemplateID As Object = Nothing, Optional ByRef r_vOIPDocumentTemplateID2 As Object = Nothing, Optional ByRef r_vBrokerReportID As Object = Nothing, Optional ByRef r_vPolicyToleranceAmount As Object = Nothing, Optional ByRef r_vAccountToleranceAmount As Object = Nothing, Optional ByRef r_vPMWrkTaskID As Object = Nothing, Optional ByRef r_vPMWrkTaskID2 As Object = Nothing, Optional ByRef r_vPMUserGroupID As Object = Nothing, Optional ByRef r_vPMUserGroupID2 As Object = Nothing, Optional ByRef r_vActionType As Object = Nothing, Optional ByRef r_vActionType2 As Object = Nothing, Optional ByRef r_vTolerancePercentage1 As Object = Nothing, Optional ByRef r_vTolerancePercentage2 As Object = Nothing, Optional ByRef r_vCheckAutoCancel As Object = Nothing, Optional ByRef r_vAutoCancelPolicy As Object = Nothing, Optional ByRef r_vNextStep As Object = Nothing, Optional ByRef r_vPreviousStep As Object = Nothing, Optional ByRef r_vOffHoldStep As Object = Nothing, Optional ByRef r_vRecurringDays As Object = Nothing, Optional ByRef r_vRecurringLetters As Object = Nothing, Optional ByRef r_vJumpToNextStep As Object = Nothing, Optional ByRef r_vStepDescription As Object = Nothing, Optional ByRef r_vPMWrkTaskGroupId As Object = Nothing, Optional ByRef r_vBrokerLetterId As Object = Nothing, Optional ByRef r_vStopAccount As Object = Nothing, Optional ByRef r_vAutoLapseRenewal As Object = Nothing, Optional ByRef r_vInstalmentFailureCount As Object = Nothing, Optional ByRef r_vAutoCancelDoc1Id As Object = Nothing, Optional ByRef r_vAutoCancelDoc2Id As Object = Nothing, Optional ByRef r_vAutoCancelDoc1TriggerAmount As Object = Nothing, Optional ByRef r_vAutoCancelDoc2TriggerAmount As Object = Nothing, Optional ByRef r_vWriteOffToleranceAmount As Object = Nothing, Optional ByRef r_vWriteOffReasonId As Object = Nothing,
                                   Optional ByRef o_nJumpToNextStepBroker As Integer = 0,
                                   Optional ByRef o_nJumpToNextStepBrokerSingleInst As Integer = 0,
                                   Optional ByRef o_nAccountNoofDaysSingleInst As Object = 0,
                                   Optional ByRef o_nAccountToleranceAmountSingleInst As Integer = 0,
                                   Optional ByRef o_nBrokerLetterIDSingleInst As Integer = 0) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vResultArray(,) As Object = Nothing
        Const klFirstRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Credit Control step id INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_step_id", vValue:=v_lCreditControlStepId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelStepSQL, sSQLName:=ACSelStepName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'PvResultArraypulate the params

            r_vCreditControlRuleID = vResultArray.GetValue(ACSCreditControlRuleID, klFirstRow)

            r_vStepNumber = vResultArray.GetValue(ACSStepNumber, klFirstRow)

            r_vNumberOfDays = vResultArray.GetValue(ACSNumberOfDays, klFirstRow)

            r_vBrokerDays = vResultArray.GetValue(ACSBrokerDays, klFirstRow)

            r_vClientDocumentTemplateID = vResultArray.GetValue(ACSClientDocumentTemplateID, klFirstRow)

            r_vOIPDocumentTemplateID = vResultArray.GetValue(ACSOIPDocumentTemplateID, klFirstRow)

            r_vBrokerReportID = vResultArray.GetValue(ACSBrokerReportID, klFirstRow)

            r_vPolicyToleranceAmount = vResultArray.GetValue(ACSPolicyToleranceAmount, klFirstRow)

            r_vAccountToleranceAmount = vResultArray.GetValue(ACSAccountToleranceAmount, klFirstRow)

            r_vPMWrkTaskID = vResultArray.GetValue(ACSPMWrkTaskID, klFirstRow)

            r_vPMUserGroupID = vResultArray.GetValue(ACSPMUserGroupID, klFirstRow)

            r_vCheckAutoCancel = vResultArray.GetValue(ACSCheckAutoCancel, klFirstRow)

            r_vAutoCancelPolicy = vResultArray.GetValue(ACSAutoCancelPolicy, klFirstRow)

            r_vNextStep = vResultArray.GetValue(ACSNextStep, klFirstRow)

            r_vPreviousStep = vResultArray.GetValue(ACSPreviousStep, klFirstRow)

            r_vOffHoldStep = vResultArray.GetValue(ACSOffHoldStep, klFirstRow)

            r_vRecurringDays = vResultArray.GetValue(ACSRecurringDays, klFirstRow)

            r_vRecurringLetters = vResultArray.GetValue(ACSRecurringLetters, klFirstRow)

            r_vJumpToNextStep = vResultArray.GetValue(ACSJumpToNextStep, klFirstRow)

            r_vClientDocumentTemplateID2 = vResultArray.GetValue(ACSClientDocumentTemplateID2, klFirstRow)

            r_vOIPDocumentTemplateID2 = vResultArray.GetValue(ACSOIPDocumentTemplateID2, klFirstRow)

            r_vPMWrkTaskID2 = vResultArray.GetValue(ACSPMWrkTaskID2, klFirstRow)

            r_vPMUserGroupID2 = vResultArray.GetValue(ACSPMUserGroupID2, klFirstRow)

            r_vActionType = vResultArray.GetValue(ACSPMActionType, klFirstRow)

            r_vActionType2 = vResultArray.GetValue(ACSPMActionType2, klFirstRow)

            r_vTolerancePercentage1 = vResultArray.GetValue(ACSTolerancePercentage1, klFirstRow)

            r_vTolerancePercentage2 = vResultArray.GetValue(ACSTolerancePercentage2, klFirstRow)

            r_vStepDescription = vResultArray.GetValue(ACSStepDescription, klFirstRow)

            r_vPMWrkTaskGroupId = vResultArray.GetValue(ACSPMWrkTaskGroupId, klFirstRow)

            r_vBrokerLetterId = vResultArray.GetValue(ACSBrokerLetterId, klFirstRow)

            r_vStopAccount = vResultArray.GetValue(ACSStopAccount, klFirstRow)

            r_vAutoLapseRenewal = vResultArray.GetValue(ACSAutoLapseRenewal, klFirstRow)

            r_vInstalmentFailureCount = vResultArray.GetValue(ACSInstalmentFailureCount, klFirstRow)

            r_vAutoCancelDoc1Id = vResultArray.GetValue(ACSAutoCancelDoc1, klFirstRow)

            r_vAutoCancelDoc2Id = vResultArray.GetValue(ACSAutoCancelDoc2, klFirstRow)

            r_vAutoCancelDoc1TriggerAmount = vResultArray.GetValue(ACSAutoCancelDoc1Trigger, klFirstRow)

            r_vAutoCancelDoc2TriggerAmount = vResultArray.GetValue(ACSAutoCancelDoc2Trigger, klFirstRow)

            r_vWriteOffToleranceAmount = vResultArray.GetValue(ACSWriteOffTolerance, klFirstRow)

            r_vWriteOffReasonId = vResultArray.GetValue(ACSWriteOffReasonId, klFirstRow)

            o_nJumpToNextStepBroker = ToSafeInteger(vResultArray(kJumpToNextStepBroker, klFirstRow), 0)

            o_nJumpToNextStepBrokerSingleInst = ToSafeInteger(vResultArray(kJumpToNextStepBrokerSingleInst, klFirstRow), 0)

            o_nAccountNoofDaysSingleInst = ToSafeInteger(vResultArray(kAccountNoofDaysSingleInst, klFirstRow), 0)

            o_nAccountToleranceAmountSingleInst = ToSafeInteger(vResultArray(kAccountToleranceAmountSingleInst, klFirstRow), 0)

            o_nBrokerLetterIDSingleInst = ToSafeInteger(vResultArray(kBrokerLetterIDSingleInst, klFirstRow), 0)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Credit Control Step", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: GetDocTemplateList (Public)
    '
    ' Description: Return data from the document_template table
    '
    ' Comments: This function is a temporary fix - so the sql is hard coded.
    '           This needs to be resolved in future !
    ' *****************************************************************
    Public Function GetDocTemplateList(ByRef r_vDocumentList(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQLString As String = ""

        'Const klFirstRow As Long = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Define the sql
            sSQLString = "select document_template_id, document_template.description "
            sSQLString = sSQLString & "from document_template "

            sSQLString = sSQLString & "where document_type_id in (4,5,8) and document_template.is_deleted = 0 "


            sSQLString = sSQLString & "ORDER BY document_template.description"


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQLString, sSQLName:="", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vDocumentList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Document List", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplateList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' *****************************************************************
    ' Name: GetRuleDetails (Public)
    '
    ' Description: Select a single Credit Control Rule record from the
    ' database.
    '
    ' *****************************************************************
    Public Function GetRuleDetails(ByVal v_lCreditControlRuleId As Integer,
                                   Optional ByRef r_vDescription As String = "",
                                   Optional ByRef r_vSourceID As String = "",
                                   Optional ByRef r_vBusinessType As String = "",
                                   Optional ByRef r_vPFFrequencyID As Object = Nothing,
                                   Optional ByRef r_vPFFrequencyDescription As Object = Nothing,
                                   Optional ByRef r_vIsActive As Object = Nothing,
                                   Optional ByRef r_vProcessingDays As Object = Nothing,
                                   Optional ByRef r_vUseEffectiveDate As Object = Nothing,
                                   Optional ByRef r_vUseGreaterTransEffDate As Object = Nothing,
                                   Optional ByRef r_vPFInstalmentsResultId As Integer = 0,
                                   Optional ByRef r_vPolicyIsPaid As Integer = 0,
                                   Optional ByRef r_vProductId As Integer = 0,
                                   Optional ByRef r_vUseDueDate As Object = Nothing,
                                   Optional ByRef r_oUseInceptionDateForAutoCancel As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Const klFirstRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Credit Control rule id INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_rule_id", vValue:=CStr(v_lCreditControlRuleId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRuleSQL, sSQLName:=ACSelRuleName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Populate the params


            If Not Information.IsNothing(r_vDescription) Then
                r_vDescription = vResultArray(ACRDescription, klFirstRow)
            End If

            If Not Information.IsNothing(r_vSourceID) Then
                r_vSourceID = vResultArray(ACRSourceID, klFirstRow)
            End If

            If Not Information.IsNothing(r_vBusinessType) Then
                r_vBusinessType = vResultArray(ACRBusinessType, klFirstRow)
            End If

            If Not Information.IsNothing(r_vPFFrequencyID) Then
                r_vPFFrequencyID = vResultArray(ACRPFFrequencyID, klFirstRow)
            End If

            If Not Information.IsNothing(r_vPFFrequencyID) Then
                r_vPFFrequencyDescription = vResultArray(ACRPFFrequencyDescription, klFirstRow)
            End If


            If Not Information.IsNothing(r_vIsActive) Then
                r_vIsActive = vResultArray(ACRIsActive, klFirstRow)
            End If


            If Information.IsNothing(r_vProcessingDays) Then
                r_vProcessingDays = vResultArray(ACRProcssingDays, klFirstRow)
            End If


            If Not Information.IsNothing(r_vUseEffectiveDate) Then
                r_vUseEffectiveDate = vResultArray(ACRUseEffectiveDate, klFirstRow)
            End If


            If Not Information.IsNothing(r_vUseGreaterTransEffDate) Then
                r_vUseGreaterTransEffDate = vResultArray(ACRUseGreaterTransEffDate, klFirstRow)
            End If

            If Not False Then
                r_vPFInstalmentsResultId = gPMFunctions.ToSafeInteger(vResultArray(ACRPFInstalmentsResultId, klFirstRow), 0)
            End If


            If Not Information.IsNothing(r_vPolicyIsPaid) Then
                r_vPolicyIsPaid = gPMFunctions.ToSafeInteger(vResultArray(ACRIsPolicyPaid, klFirstRow), -1)
            End If


            If Not Information.IsNothing(r_vProductId) Then
                r_vProductId = gPMFunctions.ToSafeLong(vResultArray(ACRProductID, klFirstRow), -1)
            End If

            If Not Information.IsNothing(r_vUseDueDate) Then
                r_vUseDueDate = gPMFunctions.ToSafeInteger(vResultArray(ACRUseDueDate, klFirstRow), -1)
            End If

            If Not Information.IsNothing(r_oUseInceptionDateForAutoCancel) Then
                r_oUseInceptionDateForAutoCancel = ToSafeInteger(vResultArray.GetValue(kUseInceptionDateForAutoCancel, 0))
            End If

            If Not Information.IsNothing(r_oUseInceptionDateForAutoCancel) Then
                r_oUseInceptionDateForAutoCancel = ToSafeInteger(vResultArray.GetValue(kUseInceptionDateForAutoCancel, 0))
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Credit Control Rule", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '*********************************************************************
    ' Name: GetScriptFile
    '
    ' Description : Find and read the VBScript file
    '
    ' Created: PW030103
    '*********************************************************************
    Private Function GetScriptFile(ByRef v_sScript As String) As Integer

        Dim result As Integer = 0
        Dim sFullPath As String = ""
        Dim iFile As Integer
        Dim lFileLength As Integer
        Dim sPathName As String = ""
        Dim lFileNumber As gPMConstants.PMEReturnCode
        Dim sStr, sStr2 As String



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the path to the validation script from the registry
        lFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

        ' Build the path to the script file
        sPathName = sPathName.Trim()
        If Not sPathName.EndsWith("\") And Not sPathName.EndsWith(":") Then
            sPathName = sPathName & "\"
        End If
        sFullPath = sPathName & "AUTO_CANCELLATION.rul"

        ' Ensure the file exists
        If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AUTO_CANCELLATION.rul file not found", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScriptFile")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Open the VBscript file
        iFile = FileSystem.FreeFile()
        FileSystem.FileOpen(iFile, sFullPath, OpenMode.Input)
        lFileLength = FileSystem.LOF(iFile)

        ' Read the script into the string variable
        sStr2 = FileSystem.InputString(iFile, lFileLength)

        FileSystem.FileClose(iFile)

        ' Add the option explicit in case it's missing
        sStr = "Option Explicit" & Strings.Chr(13) & Strings.Chr(10)

        sStr = sStr & sStr2 & Strings.Chr(13) & Strings.Chr(10)

        ' Return the script
        v_sScript = sStr.Trim()

        Return result

    End Function

    ''' <summary>
    ''' get filename from system option
    ''' </summary>
    ''' <param name="v_sScript"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCompiledRuleFile(ByRef v_sScript As String) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        nResult = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID,
                                            v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=GeneralConst.kSystemOptionCompiledRuleCreditControl,
                                            r_sOptionValue:=v_sScript)
        Return nResult

    End Function

    ' *****************************************************************
    ' Name: DirectAddRule (Public)
    '
    ' Description: Adds a Credit Control Rule record to the database
    '
    ' *****************************************************************
    'Developer Guide No. 101
    Public Function DirectAddRule(ByRef r_vCreditControlRuleID As Object, ByVal v_vDescription As Object, ByVal v_vSourceID As Object,
                                  ByVal v_vBusinessType As Object, ByVal v_vPFFrequencyID As Object, ByVal v_vIsActive As Object,
                                  Optional ByVal v_vProcessingDays As Object = 0, Optional ByVal v_vUseEffectiveDate As Object = 0,
                                  Optional ByVal v_vUseGreaterTranEffDate As Object = 0, Optional ByVal v_vPFInstalmentsResultId As Object = 0,
                                  Optional ByVal v_vPolicyIsPaid As Object = 0, Optional ByVal v_vProductId As Object = 0,
                                  Optional ByVal v_vUseDueDate As Object = 0, Optional ByVal oUseInceptionDate As Object = Nothing, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "DirectAddRule"
        Dim lRecordsAffected As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            AddOutputParameter("credit_control_rule_id", r_vCreditControlRuleID, gPMConstants.PMEDataType.PMLong)

            AddInputParameter("description", v_vDescription, gPMConstants.PMEDataType.PMString)
            AddInputParameter("source_id", v_vSourceID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("business_type", v_vBusinessType, gPMConstants.PMEDataType.PMString)
            AddInputParameter("pffrequency_id", v_vPFFrequencyID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("is_active", v_vIsActive, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_effective_date", v_vUseEffectiveDate, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_Greater_TransEff_date", v_vUseGreaterTranEffDate, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_due_date", v_vUseDueDate, gPMConstants.PMEDataType.PMInteger)

            If v_vProcessingDays <> StringsHelper.ToDoubleSafe("") Then
                AddInputParameter("Processing_Days", v_vProcessingDays, gPMConstants.PMEDataType.PMLong)
            Else
                AddInputParameter("Processing_Days", 0, gPMConstants.PMEDataType.PMLong)
            End If

            If v_vPFInstalmentsResultId <> 0 Then
                AddInputParameter("pfInstalments_result_id", v_vPFInstalmentsResultId, gPMConstants.PMEDataType.PMLong)
            Else

                'developer guide no 85. 
                AddInputParameter("pfInstalments_result_id", DBNull.Value, gPMConstants.PMEDataType.PMLong)
            End If

            AddInputParameter("policy_is_paid", v_vPolicyIsPaid, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("product_id", v_vProductId, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("use_inception_Date", oUseInceptionDate, gPMConstants.PMEDataType.PMBoolean)
            AddInputParameter("user_id", m_iUserID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("unique_id", v_sUniqueId, gPMConstants.PMEDataType.PMString)
            AddInputParameter("screen_hierarchy", v_sScreenHierarchy, gPMConstants.PMEDataType.PMString)

            lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddRuleSQL, sSQLName:=ACDirectAddRuleName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDirectAddRuleSQL & " Failed")
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected > 0 Then
                r_vCreditControlRuleID = m_oDatabase.Parameters.Item("credit_control_rule_id").Value
            Else
                ' Nothing affected, so set to error
                gPMFunctions.RaiseError(kMethodName, "No Record affected")
            End If

            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMTrue
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try

    End Function


    ' *****************************************************************
    ' Name: DirectAddStep (Public)
    '
    ' Description: Adds a Credit Control Step record to the database
    '
    ' *****************************************************************
    'Developer Guide No. 101
    Public Function DirectAddStep(ByRef r_vCreditControlStepID As Object, ByVal v_vCreditControlRuleID As Object, ByVal v_vStepNumber As Object, ByVal v_vNumberOfDays As Object, ByVal v_vBrokerDays As Object, ByVal v_vClientDocumentTemplateID As Object, ByVal v_vClientDocumentTemplateID2 As Object, ByVal v_vOIPDocumentTemplateID As Object, ByVal v_vOIPDocumentTemplateID2 As Object, ByVal v_vBrokerReportID As Object, ByVal v_vPolicyToleranceAmount As Object, ByVal v_vAccountToleranceAmount As Object, ByVal v_vPMWrkTaskID As Object, ByVal v_vPMWrkTaskID2 As Object, ByVal v_vPMUserGroupID As Object, ByVal v_vPMUserGroupID2 As Object, ByVal v_vActionType As Object, ByVal v_vActionType2 As Object, ByVal v_vTolerancePercentage1 As Object, ByVal v_vTolerancePercentage2 As Object, ByVal v_vCheckAutoCancel As Object, ByVal v_vAutoCancelPolicy As Object, ByVal v_vNextStep As Object, ByVal v_vPreviousStep As Object, ByVal v_vOffHoldStep As Object, ByVal v_vRecurringDays As Object, ByVal v_vRecurringLetters As Object, ByVal v_vJumpToNextStep As Object, ByVal v_vStepDescription As Object, ByVal v_vPMWrkTaskGroupId As Object, ByVal v_vBrokerLetterId As Object, ByVal v_vStopAccount As Object, ByVal v_vAutoLapseRenewal As Object, ByVal v_vInstalmentFailureCount As Object, ByVal v_vAutoCancelDocument1TriggerAmount As Object, ByVal v_vAutoCancelDocument2TriggerAmount As Object, ByVal v_vAutoCancelDocument1TemplateId As Object, ByVal v_vAutoCancelDocument2TemplateId As Object, ByVal v_vWriteOffToleranceAmount As Object, ByVal v_vWriteOffReasonId As Object,
                                  ByVal cJumpToNextStepBroker As Integer,
                                  ByVal cJumpToNextStepBrokerSingleInst As Integer,
                                  ByVal cAccountNoofDaysSingleInst As Integer,
                                  ByVal cAccountToleranceAmountSingleInst As Integer,
                                  ByVal cBrokerLetterIDSingleInst As Integer,
                                  Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Const kMethodName As String = "DirectAddStep"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Credit Control Step id

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_step_id", vValue:=r_vCreditControlStepID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add credit control rule id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_rule_id", vValue:=v_vCreditControlRuleID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add step number as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="step_number", vValue:=v_vStepNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add number of days as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="number_of_days", vValue:=v_vNumberOfDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add broker days as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="broker_days", vValue:=v_vBrokerDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add client document template id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_document_template_id", vValue:=v_vClientDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add client document template id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_document_template_id2", vValue:=v_vClientDocumentTemplateID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add OIP document template id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="oip_document_template_id", vValue:=v_vOIPDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add OIP document template id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="oip_document_template_id2", vValue:=v_vOIPDocumentTemplateID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add broker report id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="broker_report_id", vValue:=v_vBrokerReportID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add policy tolerance amount as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_tolerance_amount", vValue:=v_vPolicyToleranceAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add account tolerance amount as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_tolerance_amount", vValue:=v_vAccountToleranceAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add work task id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=v_vPMWrkTaskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add work task id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id2", vValue:=v_vPMWrkTaskID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=v_vPMUserGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id2", vValue:=v_vPMUserGroupID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="action_type", vValue:=v_vActionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="action_type2", vValue:=v_vActionType2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add check auto cancel as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="check_auto_cancel", vValue:=v_vCheckAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add auto cancel policy as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_cancel_policy", vValue:=v_vAutoCancelPolicy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add next step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="next_step", vValue:=v_vNextStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add previous step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="previous_step", vValue:=v_vPreviousStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add off_hold_step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="off_hold_step", vValue:=v_vOffHoldStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add recurring days as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="recurring_days", vValue:=v_vRecurringDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add recurring letters as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="recurring_letters", vValue:=v_vRecurringLetters, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add jump to next step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="jump_to_next_step", vValue:=v_vJumpToNextStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="tolerance_percentage_1", vValue:=v_vTolerancePercentage1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="tolerance_percentage_2", vValue:=v_vTolerancePercentage2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If


            ' Add pmwrk_task_group_id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=v_vPMWrkTaskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add credit control step description as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="step_description", vValue:=v_vStepDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add broker letter as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="broker_letter_id", vValue:=v_vBrokerLetterId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add stop account as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stop_account", vValue:=v_vStopAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If


            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_lapse_renewal", vValue:=v_vAutoLapseRenewal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            m_lReturn = CType(AddInputParameter("Instalment_Failure_Count", v_vInstalmentFailureCount, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_1_trigger_amount", v_vAutoCancelDocument1TriggerAmount, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_2_trigger_amount", v_vAutoCancelDocument2TriggerAmount, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_1_template_id", gPMFunctions.ZeroToNull(v_vAutoCancelDocument1TemplateId), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_2_template_id", gPMFunctions.ZeroToNull(v_vAutoCancelDocument2TemplateId), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("write_off_tolerance", v_vWriteOffToleranceAmount, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("write_off_reason_id", gPMFunctions.ZeroToNull(v_vWriteOffReasonId), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter("nJump_to_next_step_broker", ZeroToNull(cJumpToNextStepBroker), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("nSingle_instalment_jump_to_next_step_broker", ZeroToNull(cJumpToNextStepBrokerSingleInst), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("nSingle_instalment_account_number_of_days", ZeroToNull(cAccountNoofDaysSingleInst), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("dSingle_instalment_account_tollerance_amount", ZeroToNull(cAccountToleranceAmountSingleInst), gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("nSingle_instalment_broker_letter_id", ZeroToNull(cBrokerLetterIDSingleInst), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = AddInputParameter("user_id", m_iUserID, gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter("unique_id", v_sUniqueId, gPMConstants.PMEDataType.PMString)
            m_lReturn = AddInputParameter("screen_hierarchy", v_sScreenHierarchy, gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddStepSQL, sSQLName:=ACDirectAddStepName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected > 0 Then

                r_vCreditControlStepID = m_oDatabase.Parameters.Item("credit_control_step_id").Value
            Else
                ' Nothing affected, so set to error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function



    ' *****************************************************************
    ' Name: DirectEditRule (Public)
    '
    ' Description: Edits a Credit Control Rule record in the database
    '
    ' *****************************************************************
    'Developer Guide No. 101
    'Public Function DirectEditRule(ByVal v_vCreditControlRuleID As Byte, ByVal v_vDescription As Byte, ByVal v_vSourceID As Byte, ByVal v_vBusinessType As Byte, ByVal v_vPFFrequencyID As Byte, ByVal v_vIsActive As Byte, Optional ByVal v_vProcessingDays As Byte = 0, Optional ByVal v_vUseEffectiveDate As Byte = 0, Optional ByVal v_vUseGreaterTranEffDate As Byte = 0, Optional ByVal v_vPFInstalmentsResultId As Byte = 0, Optional ByVal v_vPolicyIsPaid As Byte = 0, Optional ByVal v_vProductId As Byte = 0) As Integer
    Public Function DirectEditRule(ByVal v_vCreditControlRuleID As Object, ByVal v_vDescription As Object, ByVal v_vSourceID As Object,
                                   ByVal v_vBusinessType As Object, ByVal v_vPFFrequencyID As Object, ByVal v_vIsActive As Object,
                                   Optional ByVal v_vProcessingDays As Object = Nothing, Optional ByVal v_vUseEffectiveDate As Object = Nothing,
                                   Optional ByVal v_vUseGreaterTranEffDate As Object = Nothing, Optional ByVal v_vPFInstalmentsResultId As Object = Nothing,
                                   Optional ByVal v_vPolicyIsPaid As Object = Nothing, Optional ByVal v_vProductId As Object = Nothing,
                                   Optional ByVal v_vUseDueDate As Object = 0, Optional ByVal oUseInceptionDate As Object = Nothing, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DirectEditRule"
        Dim lRecordsAffected As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Credit Control Rule id
            AddInputParameter("credit_control_rule_id", v_vCreditControlRuleID, gPMConstants.PMEDataType.PMLong)

            AddInputParameter("description", v_vDescription, gPMConstants.PMEDataType.PMString)
            AddInputParameter("source_id", v_vSourceID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("business_type", v_vBusinessType, gPMConstants.PMEDataType.PMString)
            AddInputParameter("pffrequency_id", v_vPFFrequencyID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("is_active", v_vIsActive, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_effective_date", v_vUseEffectiveDate, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_Greater_TransEff_date", v_vUseGreaterTranEffDate, gPMConstants.PMEDataType.PMInteger)

            AddInputParameter("use_due_date", v_vUseDueDate, gPMConstants.PMEDataType.PMInteger)

            If v_vProcessingDays <> StringsHelper.ToDoubleSafe("") Then
                AddInputParameter("Processing_Days", v_vProcessingDays, gPMConstants.PMEDataType.PMLong)
            Else
                AddInputParameter("Processing_Days", 0, gPMConstants.PMEDataType.PMLong)
            End If

            If v_vPFInstalmentsResultId <> 0 Then
                AddInputParameter("pfInstalments_result_id", v_vPFInstalmentsResultId, gPMConstants.PMEDataType.PMLong)
            Else
                AddInputParameter("pfInstalments_result_id", DBNull.Value, gPMConstants.PMEDataType.PMLong)
            End If

            AddInputParameter("policy_is_paid", v_vPolicyIsPaid, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("product_id", v_vProductId, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("use_inception_Date", oUseInceptionDate, gPMConstants.PMEDataType.PMBoolean)
            AddInputParameter("user_id", m_iUserID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("unique_id", v_sUniqueId, gPMConstants.PMEDataType.PMString)
            AddInputParameter("screen_hierarchy", v_sScreenHierarchy, gPMConstants.PMEDataType.PMString)
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectEditRuleSQL, sSQLName:=ACDirectEditRuleName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDirectEditRuleSQL & " failed.")
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected = 0 Then
                ' Nothing affected, so set to error
                gPMFunctions.RaiseError(kMethodName, " No Record updated")
            End If

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result

        End Try

    End Function

    ' *****************************************************************
    ' Name: DirectEditStep (Public)
    '
    ' Description: Edits a Credit Control Step record in the database
    '
    ' *****************************************************************
    Public Function DirectEditStep(ByVal v_vCreditControlStepID As Object, ByVal v_vCreditControlRuleID As Object, ByVal v_vStepNumber As Object, ByVal v_vNumberOfDays As Object, ByVal v_vBrokerDays As Object, ByVal v_vClientDocumentTemplateID As Object, ByVal v_vClientDocumentTemplateID2 As Object, ByVal v_vOIPDocumentTemplateID As Object, ByVal v_vOIPDocumentTemplateID2 As Object, ByVal v_vBrokerReportID As Object, ByVal v_vPolicyToleranceAmount As Object, ByVal v_vAccountToleranceAmount As Object, ByVal v_vPMWrkTaskID As Object, ByVal v_vPMWrkTaskID2 As Object, ByVal v_vPMUserGroupID As Object, ByVal v_vPMUserGroupID2 As Object, ByVal v_vActionType As Object, ByVal v_vActionType2 As Object, ByVal v_vTolerancePercentage1 As Object, ByVal v_vTolerancePercentage2 As Object, ByVal v_vCheckAutoCancel As Object, ByVal v_vAutoCancelPolicy As Object, ByVal v_vNextStep As Object, ByVal v_vPreviousStep As Object, ByVal v_vOffHoldStep As Object, ByVal v_vRecurringDays As Object, ByVal v_vRecurringLetters As Object, ByVal v_vJumpToNextStep As Object, ByVal v_vStepDescription As Object, ByVal v_vPMWrkTaskGroupId As Object, ByVal v_vBrokerLetterId As Object, ByVal v_vStopAccount As Object, ByVal v_vAutoLapseRenewal As Object, ByVal v_vInstalmentFailureCount As Object, ByVal v_vAutoCancelDocument1TriggerAmount As Object, ByVal v_vAutoCancelDocument2TriggerAmount As Object, ByVal v_vAutoCancelDocument1TemplateId As Object, ByVal v_vAutoCancelDocument2TemplateId As Object, ByVal v_vWriteOffToleranceAmount As Object, ByVal v_vWriteOffReasonId As Object,
                                   ByVal cJumpToNextStepBroker As Integer,
                                   ByVal cJumpToNextStepBrokerSingleInst As Integer,
                                   ByVal cAccountNoofDaysSingleInst As Integer,
                                   ByVal cAccountToleranceAmountSingleInst As Integer,
                                   ByVal cBrokerLetterIDSingleInst As Integer,
                                   Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Const kMethodName As String = "DirectAddStep"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Credit Control Step id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_step_id", vValue:=v_vCreditControlStepID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add credit control rule id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_rule_id", vValue:=v_vCreditControlRuleID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add step number as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="step_number", vValue:=v_vStepNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add number of days as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="number_of_days", vValue:=v_vNumberOfDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add broker days as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="broker_days", vValue:=v_vBrokerDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add client document template id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_document_template_id", vValue:=v_vClientDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add client document template id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_document_template_id2", vValue:=v_vClientDocumentTemplateID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add OIP document template id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="oip_document_template_id", vValue:=v_vOIPDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add OIP document template id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="oip_document_template_id2", vValue:=v_vOIPDocumentTemplateID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add broker report id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="broker_report_id", vValue:=v_vBrokerReportID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add policy tolerance amount as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_tolerance_amount", vValue:=v_vPolicyToleranceAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add account tolerance amount as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_tolerance_amount", vValue:=v_vAccountToleranceAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add work task id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=v_vPMWrkTaskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add work task id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id2", vValue:=v_vPMWrkTaskID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=v_vPMUserGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id2", vValue:=v_vPMUserGroupID2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="action_type", vValue:=v_vActionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="action_type2", vValue:=v_vActionType2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add check auto cancel as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="check_auto_cancel", vValue:=v_vCheckAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add auto cancel policy as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_cancel_policy", vValue:=v_vAutoCancelPolicy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add next step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="next_step", vValue:=v_vNextStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add previous step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="previous_step", vValue:=v_vPreviousStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add off_hold_step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="off_hold_step", vValue:=v_vOffHoldStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add recurring days as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="recurring_days", vValue:=v_vRecurringDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add recurring letters as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="recurring_letters", vValue:=v_vRecurringLetters, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add jump to next step as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="jump_to_next_step", vValue:=v_vJumpToNextStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="tolerance_percentage_1", vValue:=v_vTolerancePercentage1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="tolerance_percentage_2", vValue:=v_vTolerancePercentage2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If


            ' Add pmwrk_task_group_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=v_vPMWrkTaskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add credit control step description as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="step_description", vValue:=v_vStepDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add broker letter as an input param for an insert

            'Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="broker_letter_id", vValue:=v_vBrokerLetterId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add stop account as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stop_account", vValue:=v_vStopAccount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_lapse_renewal", vValue:=v_vAutoLapseRenewal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            m_lReturn = CType(AddInputParameter("Instalment_Failure_Count", v_vInstalmentFailureCount, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_1_trigger_amount", v_vAutoCancelDocument1TriggerAmount, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_2_trigger_amount", v_vAutoCancelDocument2TriggerAmount, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_1_template_id", gPMFunctions.ZeroToNull(v_vAutoCancelDocument1TemplateId), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("auto_cancel_document_2_template_id", gPMFunctions.ZeroToNull(v_vAutoCancelDocument2TemplateId), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("write_off_tolerance", v_vWriteOffToleranceAmount, gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("write_off_reason_id", gPMFunctions.ZeroToNull(v_vWriteOffReasonId), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter("nJump_to_next_step_broker", ZeroToNull(cJumpToNextStepBroker), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("nsingle_instalment_jump_to_next_step_broker", ZeroToNull(cJumpToNextStepBrokerSingleInst), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("nsingle_instalment_account_number_of_days", ZeroToNull(cAccountNoofDaysSingleInst), gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("dsingle_instalment_account_tollerance_amount", ZeroToNull(cAccountToleranceAmountSingleInst), gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter("nsingle_instalment_broker_letter_id", ZeroToNull(cBrokerLetterIDSingleInst), gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = AddInputParameter("user_id", m_iUserID, gPMConstants.PMEDataType.PMLong)
            m_lReturn = AddInputParameter("unique_id", v_sUniqueId, gPMConstants.PMEDataType.PMString)
            m_lReturn = AddInputParameter("screen_hierarchy", v_sScreenHierarchy, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectEditStepSQL, sSQLName:=ACDirectEditStepName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected = 0 Then
                ' Nothing affected, so set to error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
        End Try
        Return result

    End Function

    ' *****************************************************************
    ' Name: DirectDeleteStep (Public)
    '
    ' Description: Deletes a Credit Control Step record
    '
    ' *****************************************************************
    Public Function DirectDeleteStep(ByVal v_lCreditControlStepId As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Credit Control step id as an input param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_step_id", vValue:=CStr(v_lCreditControlStepId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nResult", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)
            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectDeleteStepSQL, sSQLName:=ACDirectDeleteStepName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                result = m_lReturn
            End If

            result = gPMFunctions.ToSafeInteger(m_oDatabase.Parameters.Item("nResult").Value)

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDeleteStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDeleteStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: DirectDeleteRule (Public)
    '
    ' Description: Deletes a Credit Control Rule record
    '
    ' *****************************************************************
    Public Function DirectDeleteRule(ByVal v_lCreditControlRuleId As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add CreditControl_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="credit_control_rule_id", vValue:=CStr(v_lCreditControlRuleId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="unique_id", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="screen_hierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectDeleteRuleSQL, sSQLName:=ACDirectDeleteRuleName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDeleteRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDeleteRule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    ' *****************************************************************
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' *****************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' *****************************************************************
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' *****************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' *****************************************************************
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' *****************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function CancelPolicy(ByVal lCreditControlItemID As Object, ByVal dLapsedDate As Date) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CancelPolicy
        ' PURPOSE: Auto-Cancels a policy
        ' AUTHOR: Danny Davis
        ' DATE: 20 November 2003, 04:04 PM
        ' RETURNS: PMTrue for success
        ' CHANGES: PW161203 - CQ3535 - accept lapsed date as parameter and use
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim bSingleInstalment As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object
            Dim oAutoMTA As bSIRAutoMTA.Business


            oAutoMTA = New bSIRAutoMTA.Business
            m_lReturn = oAutoMTA.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CheckSingleInstalmentPlan(lCreditControlItemID, bSingleInstalment)

            If bSingleInstalment Then
                m_lReturn = GetAutoCancelDetailForSingleInstalmentPlan(lCreditControlItemID, vResultArray)

                Dim nLBound As Integer
                Dim nUBound As Integer
                If IsArray(vResultArray) Then
                    nLBound = LBound(vResultArray, 2)
                    nUBound = UBound(vResultArray, 2)

                    For iCount As Integer = nLBound To nUBound
                        m_lReturn = oAutoMTA.AutoCancelMTA(v_lPartyCnt:=vResultArray(0, iCount),
                                                           v_lInsuranceFolderCnt:=vResultArray(1, iCount),
                                                           v_dtEffectiveDate:=dLapsedDate,
                                                           v_bIsSingleInstalmentForCCProcess:=bSingleInstalment)
                    Next
                Else
                    CancelPolicy = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            Else

                'Get the Information for Cancellation
                With m_oDatabase
                    .Parameters.Clear()

                    .Parameters.Add("credit_control_item_id", CStr(lCreditControlItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLSelect("spu_ACT_Select_Credit_Control_AutoCancel", "Get Credit Control Auto Cancel details", True, , vResultArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                'Fire it off
                If Information.IsArray(vResultArray) Then

                    m_lReturn = oAutoMTA.AutoCancelMTA(v_lPartyCnt:=vResultArray(0, 0), v_lInsuranceFolderCnt:=vResultArray(1, 0), v_dtEffectiveDate:=dLapsedDate)
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            oAutoMTA.Dispose()
            oAutoMTA = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally
        End Try
        Return result


    End Function


    ' ***************************************************************** '
    ' Name: GetALLPMWrkTaskGroupTasks
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function GetALLPMWrkTaskGroupTasks(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetALLPMWrkTaskGroupTasks"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetALLPMWrkTaskGroupTasksSQL, sSQLName:=kGetALLPMWrkTaskGroupTasksName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kGetALLPMWrkTaskGroupTasksSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try


        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetALLPMWrkTaskGroupPMUserGroups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetALLPMWrkTaskGroupPMUserGroups(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetALLPMWrkTaskGroupPMUserGroups"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetALLPMWrkTaskGroupPMUserGroupsSQL, sSQLName:=kGetALLPMWrkTaskGroupPMUserGroupsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kGetALLPMWrkTaskGroupPMUserGroupsSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result


    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues
    '
    ' Parameters: n/a
    '
    ' Description: Returns Lookup Details for specified lookup tables
    '
    ' History:
    '           Created : MEvans : 14-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef v_vLookupTables As Object, ByRef r_vLookupDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupValues"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oLookup As bPMLookup.Business

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' create instance of lookup object

            oLookup = New bPMLookup.Business
            m_lReturn = oLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            ' get the lookup details for the specified

            lReturn = oLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=v_vLookupTables, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Today, vResultArray:=r_vLookupDetails)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bPMLookup.Business.GetLookupValues Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy object instance
            oLookup = Nothing
        End Try
        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CreateTask
    '
    ' Parameters: n/a
    '
    ' Description: Create a work manager task
    '
    ' History:
    '           Created : MEvans : 21-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function CreateTask(ByVal v_lPMWrkTaskID As Integer, ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByVal v_sWorkflowInformation As String = "", Optional ByVal v_dtDateCreated As Date = #12/30/1899#, Optional ByVal v_iCreatedByID As Integer = 0, Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateTask"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oTaskInstance As bPMWrkTaskInstance.TaskControl


        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            ' Create an instance of the bPMWrkTaskInstance Component

            oTaskInstance = New bPMWrkTaskInstance.TaskControl
            lReturn = oTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "gPMComponentServices.CreateBusinessObject Failed to create instance of bPMWrkTaskInstance.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Create a new task with the specified parameters

            lReturn = oTaskInstance.CreateNew(v_lPMWrkTaskID:=v_lPMWrkTaskID, v_lPMWrkTaskGroupID:=v_lPMWrkTaskGroupID, v_sCustomer:=v_sCustomer, v_dtTaskDueDate:=v_dtTaskDueDate, v_lPMUserGroupID:=v_lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=v_iTaskStatus, v_iIsUrgent:=v_iIsUrgent, r_lPMWrkTaskInstanceCnt:=r_lPMWrkTaskInstanceCnt)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bPMWrkTaskInstance.Business.CreateNew Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' destroy instance of object
            oTaskInstance = Nothing
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAccountCreditAmounts
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 22-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Private Function GetAccountCreditAmounts(ByVal v_lAccountId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountCreditAmounts"

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="account_id", v_vValue:=v_lAccountId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute selection Query
        If m_oDatabase.SQLSelect(sSQL:=kGetAccountCreditAmountsSQL, sSQLName:=kGetAccountCreditAmountsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetAccountCreditAmountsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    'developer guide no. 101(Guide)
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object
        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: AddOutputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an Output parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddOutputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddOutputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description))

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetInstalmentImportInsuranceFileStatuses
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function GetInstalmentImportInsuranceFileStatuses(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInstalmentImportInsuranceFileStatuses"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInstalmentImportInsuranceFileStatuses, sSQLName:=kGetInstalmentImportInsuranceFileStatuses, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetInstalmentImportInsuranceFileStatuses & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddCreditControlInsuranceFileStatus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Private Function AddCreditControlInsuranceFileStatus(ByVal v_lCreditControlRuleId As Integer, ByVal v_lInsuranceFileStatusId As Integer, ByVal v_sUniqueId As String, ByVal v_sScreenHierarchy As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddCreditControlInsuranceFileStatus"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="credit_control_rule_id", v_vValue:=v_lCreditControlRuleId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="insurance_file_status_id", v_vValue:=v_lInsuranceFileStatusId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="UserId", v_vValue:=m_iUserID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="UniqueId", v_vValue:=v_sUniqueId, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="ScreenHierarchy", v_vValue:=v_sScreenHierarchy, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kAddCreditControlInsuranceFileStatus, sSQLName:=kAddCreditControlInsuranceFileStatus, bStoredProcedure:=True)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kAddCreditControlInsuranceFileStatus & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If



        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddCreditControlInsuranceFileStatuses
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 24-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function AddCreditControlInsuranceFileStatuses(ByVal v_lCreditControlRuleId As Integer, ByVal v_vInsuranceFileStatuses As Object, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddCreditControlInsuranceFileStatuses"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lUBound, lLBound, lInsuranceFileStatusId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' delete existing items
            lReturn = CType(DeleteCreditControlRuleInsuranceFileStatus(v_lCreditControlRuleId, v_sUniqueId, v_sScreenHierarchy), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DeleteCreditControlRuleInsuranceFileStatus Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(v_vInsuranceFileStatuses) Then

                lLBound = v_vInsuranceFileStatuses.GetLowerBound(0)
                lUBound = v_vInsuranceFileStatuses.GetUpperBound(0)

                For lItem As Integer = lLBound To lUBound


                    lInsuranceFileStatusId = CInt(v_vInsuranceFileStatuses(lItem))

                    lReturn = CType(AddCreditControlInsuranceFileStatus(v_lCreditControlRuleId, lInsuranceFileStatusId, v_sUniqueId, v_sScreenHierarchy), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "AddCreditControlInsuranceFileStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteCreditControlRuleInsuranceFileStatus
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function DeleteCreditControlRuleInsuranceFileStatus(ByVal v_lCreditControlRuleId As Integer, ByVal v_sUniqueId As String, ByVal v_sScreenHierarchy As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteCreditControlRuleInsuranceFileStatus"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()


            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="credit_control_rule_id", v_vValue:=v_lCreditControlRuleId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="UserId", v_vValue:=m_iUserID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="UniqueId", v_vValue:=v_sUniqueId, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="ScreenHierarchy", v_vValue:=v_sScreenHierarchy, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kDeleteCreditControlRuleInsuranceFileStatus, sSQLName:=kDeleteCreditControlRuleInsuranceFileStatus, bStoredProcedure:=True)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kDeleteCreditControlRuleInsuranceFileStatus & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetCreditControlRuleInsuranceFileStatuses
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 28-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function GetCreditControlRuleInsuranceFileStatuses(ByVal v_lCreditControlRuleId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCreditControlRuleInsuranceFileStatuses"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="credit_control_rule_id", v_vValue:=v_lCreditControlRuleId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetCreditControlRuleInsuranceFileStatuses, sSQLName:=kGetCreditControlRuleInsuranceFileStatuses, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetCreditControlRuleInsuranceFileStatuses & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetNextAvailableInstalmentFailureCount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Function GetNextAvailableInstalmentFailureCount(ByVal v_lCreditControlRuleId As Integer, ByRef r_lNextInstalmentFailureCount As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetNextAvailableInstalmentFailureCount"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object
        Dim lLBound, lUBound, lNextInstalmentFailureCount As Integer


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="credit_control_rule_id", v_vValue:=v_lCreditControlRuleId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetNextAvailableInstalmentFailureCount, sSQLName:=kGetNextAvailableInstalmentFailureCount, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetNextAvailableInstalmentFailureCount & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lNextInstalmentFailureCount = 1

            If Information.IsArray(vResults) Then


                lLBound = vResults.GetLowerBound(1)

                lUBound = vResults.GetUpperBound(1)

                For lItem As Integer = lLBound To lUBound


                    'developer guide no.248
                    If gPMFunctions.ToSafeDouble(vResults(0, lItem)) = lNextInstalmentFailureCount Then
                        lNextInstalmentFailureCount += 1
                    End If

                Next

            End If

            ' return the next available failure count
            r_lNextInstalmentFailureCount = lNextInstalmentFailureCount

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function


    ' *****************************************************************
    ' Name: ProduceAutoCancellationLetter (Public)
    '
    ' Description: Print letters to the client for any outstanding amounts.
    '              Use the document template specified in the Credit
    '              Control Step record
    '
    ' Created: PW080103
    '
    'AAB-18-Feb-2004 12:04 - Added those 2 variables they were created
    '                        before but got lost during the split
    ' MEvans : 22-02-2005 : Updated to produce broker letters as well
    '                        as client letters depending on ther business type
    '                        of the credit control item
    ' *****************************************************************

    Private Function ProduceAutoCancellationLetter(ByVal v_lDocumentTemplateId As Integer, ByVal v_sDocumentTemplatePrintedDescription As String, ByVal v_lCreditControlItemId As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vCreditControlItems As Object, ByVal v_bSpoolDocuments As Boolean, ByVal v_bArchiveDocuments As Boolean,
                                                   Optional ByVal bSingleInstalment As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProduceAutoCancellationLetter"

        Const kEventTypeDocument As Integer = 10
        Const kDocumentTypeStandardLetter As Integer = 5

        Dim lReturn As gPMConstants.PMEReturnCode

        'developer guide no. 108
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Dim oEventLog As bSIREvent.Business
        Dim lSessionID As Integer
        Dim vFieldParams As Object
        Dim lDocumasterID As Integer
        Dim sEventDescription As String = ""
        Dim nInsuranceFolderCnt As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' CREATES DOCUMENT PRODUCTION LINKS TO CREDIT CONTROL
            lReturn = CType(CreateTempSession(v_vCreditControlItems:=v_vCreditControlItems, r_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "CreateTempSession Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oEventLog = New bSIREvent.Business
            lReturn = oEventLog.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create instance of bSIREvent.Business", gPMConstants.PMELogLevel.PMLogError)
            End If
            'developer guide no. 108
            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
            lReturn = CType(oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create instance of bSIRDocManagerWrapper.Interface", gPMConstants.PMELogLevel.PMLogError)
            End If
            lReturn = GetInsuranceFolderCnt(v_lInsuranceFileCnt, nInsuranceFolderCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInsuranceFolderCnt Failed.", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim bProduceLetter As Boolean = True
            Dim strEmailAttachment As String = String.Empty
            Dim iDocType As Integer = 5
            lReturn = SendAutoCancelationMailToClientOrBroker(v_iInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                              v_iDocumentTemplateID:=v_lDocumentTemplateId,
                                                               v_iPartycnt:=v_lPartyCnt,
                                                              r_bProduceLetter:=bProduceLetter,
                                                          v_strEmailAttachment:=strEmailAttachment,
                                                          v_iDocTypeId:=iDocType,
                                                          v_sEventDescription:=v_sDocumentTemplatePrintedDescription)

            If bProduceLetter Then
                'print doc in loop fashion from mail attachments
                Dim iUbound As Integer = 0
                Dim bAttachmentFound As Boolean = False

                Dim sAttachmentSTR() As String

                If ToSafeInteger(iDocType) = ACEmailDocType Then
                    sAttachmentSTR = Split(ToSafeString(strEmailAttachment), ",")
                    iUbound = sAttachmentSTR.GetUpperBound(0)
                    If ToSafeString(strEmailAttachment).Trim <> "" Then
                        bAttachmentFound = True
                    Else
                        bAttachmentFound = False
                    End If
                End If

                Dim bDocSpooled As Boolean = False

                For iTmpCnt As Integer = 0 To iUbound
                    bDocSpooled = False
                    Dim iDocTemplateID As Integer = 0
                    Dim iDocTypeId As Integer = 0

                    If bAttachmentFound Then
                        Dim sDocCode As String = sAttachmentSTR(iTmpCnt).Trim


                        m_lReturn = GetDocTemplateTypeID(v_sDocCode:=sDocCode,
                                                     r_lDocTemplateTypeId:=iDocTemplateID,
                                                 r_lDocTypeID:=iDocTypeId)
                    Else
                        iDocTemplateID = ToSafeInteger(v_lDocumentTemplateId)
                        iDocTypeId = ToSafeInteger(kDocumentTypeStandardLetter)
                    End If


                    ' SET UP DOCUMENT MANAGER
                    oDocManagerWrapper.DocumentTemplateId = iDocTemplateID
                    oDocManagerWrapper.SpoolDesc = v_sDocumentTemplatePrintedDescription
                    oDocManagerWrapper.DocumentTypeId = iDocTypeId
                    oDocManagerWrapper.PartyCnt = v_lPartyCnt
                    If bSingleInstalment Then
                        oDocManagerWrapper.InsuranceFileCnt = 0
                    Else
                        oDocManagerWrapper.InsuranceFileCnt = v_lInsuranceFileCnt
                        oDocManagerWrapper.InsuranceFolderCnt = nInsuranceFolderCnt
                    End If
                    oDocManagerWrapper.ArchiveDoc = v_bArchiveDocuments

                    ReDim vFieldParams(1, 0)

                    vFieldParams(0, 0) = "CreditControlItem"

                    vFieldParams(1, 0) = v_lCreditControlItemId


                    oDocManagerWrapper.FieldParameters = vFieldParams

                    If v_bSpoolDocuments Then
                        oDocManagerWrapper.Mode = ACSpoolDocMode
                    Else
                        oDocManagerWrapper.Mode = ACPrintSilentMode
                    End If

                    ' PRODUCE THE DOCUMENT
                    lReturn = oDocManagerWrapper.Start()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "bSIRDocManagerWrapper.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                Next
            End If
            ' GENERATE AN EVENT TIEING THE PARTY TO THE DOCUMENT
            lDocumasterID = oDocManagerWrapper.DocumasterID
            sEventDescription = v_sDocumentTemplatePrintedDescription


            lReturn = oEventLog.DirectAdd(vPartyCnt:=v_lPartyCnt, vInsuranceFileCnt:=v_lInsuranceFileCnt, vEventType:=kEventTypeDocument, vUserId:=m_iUserID, vEventDate:=DateTime.Now, vDescription:=sEventDescription, vDocumentCnt:=lDocumasterID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bSIREvent.Business.DirectAdd Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' RELEASE TEMP SESSION
            lReturn = CType(ReleaseTempSession(v_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "ReleaseTempSession Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally
            If (oEventLog Is Nothing = False) Then
                oEventLog.Dispose()
                oEventLog = Nothing
            End If

            If (oDocManagerWrapper Is Nothing = False) Then
                oDocManagerWrapper.Dispose()
                oDocManagerWrapper = Nothing
            End If
        End Try

        Return result

    End Function
    ''' <summary>
    ''' Get preferred correspondence contact type for client and agent
    ''' </summary>
    ''' <param name="v_iInsuranceFileCnt"></param>
    ''' <param name="v_iDocumentTemplateID"></param>
    ''' <param name="vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks>BFM WPR 3</remarks>
    Private Function GetPreferredCorrespondenceDetails(ByVal v_iInsuranceFileCnt As Integer,
                                                       ByVal v_iDocumentTemplateID As Integer,
                                                       ByRef vResultArray As Object) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_iInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If
            If m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=v_iDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'execute SQL statement
            result = m_oDatabase.SQLSelect(sSQL:=ACSelPreferredCorrespondenceSQL, sSQLName:=ACSelPreferredCorrespondenceName, bStoredProcedure:=ACSelPreferredCorrespondenceStored, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreferredCorrespondenceDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreferredCorrespondenceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Send an auto cancellation email to client or agent, if preferred correspondence contact is "MEMAIL" 
    ''' </summary>
    ''' <param name="v_iInsuranceFileCnt"></param>
    ''' <param name="v_iDocumentTemplateID"></param>
    ''' <param name="v_iPartycnt"></param>
    ''' <param name="r_bProduceLetter"></param>
    ''' <param name="v_strEmailAttachment"></param>
    ''' <param name="v_iDocTypeId"></param>
    ''' <param name="v_sEventDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SendAutoCancelationMailToClientOrBroker(ByVal v_iInsuranceFileCnt As Integer, ByVal v_iDocumentTemplateID As Integer,
                                                        ByVal v_iPartycnt As Integer, ByRef r_bProduceLetter As Boolean,
                                                        ByRef v_strEmailAttachment As String,
                                                        ByRef v_iDocTypeId As Integer,
                                                        ByRef v_sEventDescription As String) As Integer
        Dim bSendEMailToClient As Boolean = False
        Dim bSendEMailToAgent As Boolean = False
        Dim vResultArray As Object(,)
        ' set default LETTER - maintain backward compatibility
        Dim sOptionValue As String = String.Empty
        Dim m_oBusiness As New bSIRDocTemplate.Business

        Try
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRDocTemplate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseBusiness")
                Return m_lReturn
            End If

            m_lReturn = GetPreferredCorrespondenceDetails(v_iInsuranceFileCnt:=v_iInsuranceFileCnt,
                                                          v_iDocumentTemplateID:=v_iDocumentTemplateID,
                                                          vResultArray:=vResultArray)

            For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                Dim sCorrespondenceType As String = "LETTER"
                Dim sDefaultPreferredCorrespondenceType As String = "LETTER"
                Dim bIsAgentCorrespondence As Boolean = False

                v_iDocTypeId = ToSafeInteger(vResultArray(0, i))
                v_strEmailAttachment = ToSafeString(vResultArray(ACAutoCancelEMailAttachementTemplateCodes, i))

                If ToSafeInteger(vResultArray(0, i)) = ACAutoCancelEmailDocType Then

                    If ToSafeString(vResultArray(ACAutoCancelCorrespondenceType, i)).Trim <> "" Then
                        sCorrespondenceType = ToSafeString(vResultArray(ACAutoCancelCorrespondenceType, i)).Trim
                    End If

                    If ToSafeString(vResultArray(ACAutoCancelDefaultPreferredCorrespondenceType, i)).Trim <> "" Then
                        sDefaultPreferredCorrespondenceType = ToSafeString(vResultArray(ACAutoCancelDefaultPreferredCorrespondenceType, i)).Trim
                    End If

                    bIsAgentCorrespondence = ToSafeBoolean(vResultArray(ACAutoCancelIsAgentReceiveCorrespondence, i))
                    bSendEMailToClient = True
                    bSendEMailToAgent = True



                    'No Agent business
                    If ToSafeInteger(vResultArray(ACAutoCancelLeadAgentCnt, i)) = 0 Then
                        bSendEMailToAgent = False
                        bIsAgentCorrespondence = False 'no correspondence to agent
                    End If

                    If bSendEMailToAgent Then
                        If Not bIsAgentCorrespondence OrElse (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType <> "MEMAIL") _
                            OrElse sCorrespondenceType = "LETTER" Then
                            bSendEMailToAgent = False
                        End If
                    End If

                    If bSendEMailToClient Then
                        If bIsAgentCorrespondence OrElse (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType <> "MEMAIL") _
                            OrElse sCorrespondenceType = "LETTER" Then
                            bSendEMailToClient = False
                        End If
                    End If

                    Dim vEmailDocsPathArray As Object

                    If bSendEMailToClient Or bSendEMailToAgent Then
                        Dim sDocMergedPath As String = ""

                        Dim sDocCode As String = ""
                        Dim iDocTemplateID As Integer = 0
                        Dim iDocTypeId As Integer = 0
                        Dim vEMailDocs As New List(Of String)
                        Dim vEMailDocPath As New List(Of String)
                        Dim bOutputAsPDF As Boolean = False
                        Dim bOutPutHTM As Boolean = False
                        Dim bOutPutTXT As Boolean = False

                        Dim sClientEmailAddress As String = String.Empty
                        Dim sAgentEmailAddress As String = String.Empty

                        Dim sAttachmentSTR() As String = Split(ToSafeString(vResultArray(ACAutoCancelEMailAttachementTemplateCodes, i)), ",")

                        ReDim vEmailDocsPathArray(sAttachmentSTR.GetUpperBound(0) + 2)

                        vEMailDocs.Add(ToSafeString(v_iDocumentTemplateID))
                        vEMailDocs.Add(ToSafeString(vResultArray(ACAutoCancelEMailSubTemplateCode, i)))

                        For iAttachmentCnt As Integer = 0 To sAttachmentSTR.GetUpperBound(0)
                            vEMailDocs.Add(ToSafeString(sAttachmentSTR(iAttachmentCnt).Trim))
                        Next

                        Dim sSubjectContent As String = ""

                        For iDocCnt As Integer = 0 To vEMailDocs.Count - 1
                            sDocCode = vEMailDocs.Item(iDocCnt)
                            iDocTemplateID = 0
                            iDocTypeId = 0
                            bOutputAsPDF = False
                            bOutPutHTM = False
                            bOutPutTXT = False
                            sDocMergedPath = String.Empty

                            If iDocCnt = 0 Then
                                iDocTemplateID = ToSafeInteger(sDocCode)
                                iDocTypeId = ToSafeInteger(vResultArray(0, i))
                                bOutPutHTM = True
                            Else
                                m_lReturn = GetDocTemplateTypeID(v_sDocCode:=sDocCode,
                                                          r_lDocTemplateTypeId:=iDocTemplateID,
                                                          r_lDocTypeID:=iDocTypeId)

                                If iDocCnt = 1 Then
                                    bOutPutTXT = True
                                Else
                                    bOutputAsPDF = True
                                End If
                            End If
                            If iDocTemplateID <> 0 And iDocTypeId <> 0 Then
                                m_lReturn = PrintDocument(v_lPartyCnt:=v_iPartycnt,
                                                          v_lDocTemplateID:=iDocTemplateID,
                                                          v_sType:="",
                                                          r_sMergedFilePath:=sDocMergedPath,
                                                          v_lInsuranceFileCnt:=v_iInsuranceFileCnt,
                                                          r_sDocDesc:="",
                                                          v_lDocumentTypeId:=iDocTypeId,
                                                          v_iProductionOrder:=0,
                                                          v_iMode:=gSIRLibrary.ACEmailMode, v_bCalledFromSAM:=False,
                                                          v_bOutputAsHTM:=bOutPutHTM,
                                                          v_bOutPutAsTXT:=bOutPutTXT, r_bRetainTempFiles:=True,
                                                          v_bOutputAsPDF:=bOutputAsPDF)
                                If iDocCnt = 1 AndAlso Not String.IsNullOrEmpty(sDocMergedPath) Then
                                    Dim srSubjectTmp As New StreamReader(File.OpenRead(sDocMergedPath))
                                    Dim sbsbTemplate As New StringBuilder(srSubjectTmp.ReadToEnd())
                                    sbsbTemplate.Replace(vbCrLf, " ")
                                    sSubjectContent = sbsbTemplate.ToString
                                End If
                                vEMailDocPath.Add(sDocMergedPath)
                            End If
                        Next

                        Dim v_Attachment As New List(Of String)

                        For iDocCnt As Integer = 2 To vEMailDocPath.Count - 1
                            v_Attachment.Add(vEMailDocPath(iDocCnt))
                        Next
                        v_strEmailAttachment = vResultArray(ACAutoCancelEMailAttachementTemplateCodes, i)
                        v_iDocTypeId = vResultArray(0, i)

                        m_lReturn = m_oBusiness.GetPolicyLevelEmailAddress(ToSafeInteger(Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, i)))), sClientEmailAddress)
                        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                            Return m_lReturn
                        End If

                        If bSendEMailToClient Then
                            sClientEmailAddress = String.Empty
                            If (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType = "MEMAIL") OrElse sCorrespondenceType = "EMAIL" Then
                                If String.IsNullOrEmpty(sClientEmailAddress) Then
                                    m_lReturn = GetEmailAddress(v_iPartycnt, "MEMAIL", sClientEmailAddress)
                                End If
                            Else
                                m_lReturn = GetBranchEmailAddress(m_iSourceID, sClientEmailAddress)
                            End If
                            If Not String.IsNullOrEmpty(sClientEmailAddress) Then
                                m_lReturn = m_oBusiness.SendEMail(v_sTo:=sClientEmailAddress,
                                                                  v_sSubject:=sSubjectContent,
                                                                  v_sMessagePath:=vEMailDocPath(0),
                                                                  v_sAttachment:=v_Attachment.ToArray)
                                r_bProduceLetter = False
                                v_sEventDescription = "Auto Cancellation Email Sent"
                            End If
                        End If

                        If bSendEMailToAgent Then
                            If (sCorrespondenceType = "DEFAULT" AndAlso sDefaultPreferredCorrespondenceType = "MEMAIL") OrElse sCorrespondenceType = "EMAIL" Then
                                If String.IsNullOrEmpty(sClientEmailAddress) Then
                                    m_lReturn = GetEmailAddress(ToSafeInteger(vResultArray(ACLeadAgentCnt, i)), "MEMAIL", sClientEmailAddress)
                                End If
                            Else
                                m_lReturn = GetBranchEmailAddress(m_iSourceID, sClientEmailAddress)
                            End If
                            If Not String.IsNullOrEmpty(sClientEmailAddress) Then
                                m_lReturn = m_oBusiness.SendEMail(v_sTo:=sClientEmailAddress,
                                                                 v_sSubject:=sSubjectContent,
                                                                 v_sMessagePath:=vEMailDocPath(0),
                                                                 v_sAttachment:=v_Attachment.ToArray)
                                r_bProduceLetter = False
                                v_sEventDescription = "Auto Cancellation Email Sent"
                            End If
                        End If

                    End If
                End If
            Next
        Catch ex As Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendAutoCancelationMailToClientOrBroker Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendAutoCancelationMailToClientOrBroker", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message)

            Return m_lReturn

        End Try
        Return m_lReturn
    End Function


    Private Function CheckForRenewedPolicy(
                    ByVal v_lInsuranceFileCnt As Long,
                         ByRef r_bIsRenewedPolicy As Boolean) As Long


        Const kMethodName As String = "CheckForRenewedPolicy"

        Dim lReturn As Long

        CheckForRenewedPolicy = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)
        m_lReturn = AddOutputParameter(v_sName:="IsRenewedPolicy", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMBoolean)
        ' Execute selection Query
        lReturn = m_oDatabase.SQLAction(
                                sSQL:=kCheckForRenewedPolicySQL,
                                sSQLName:=kCheckForRenewedPolicySQL,
                                bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, kCheckForRenewedPolicySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        r_bIsRenewedPolicy = ToSafeBoolean(m_oDatabase.Parameters.Item("IsRenewedPolicy").Value, 0)


        Exit Function


    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nFound"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckAutoCancellationDocument(ByVal nInsuranceFileCnt As Integer,
                                                   ByRef nFound As Integer) As Integer

        Const kMethodName As String = "CheckAutoCancellationDocument"

        Dim oResults As Object = Nothing
        Dim nReturn As Integer = PMEReturnCode.PMTrue

        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=nInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = AddOutputParameter(v_sName:="flag", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=kCheckAutoCancellationDocumentSQL,
                                            sSQLName:=kCheckAutoCancellationDocumentName,
                                            bStoredProcedure:=True,
                                            vResultArray:=oResults,
                                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Selection Query Failed", gPMConstants.PMELogLevel.PMLogError)
                CheckAutoCancellationDocument = PMEReturnCode.PMFalse
            End If

            ' return value from the output patameter
            nFound = ToSafeCurrency(m_oDatabase.Parameters.Item("flag").Value, 0)
            CheckAutoCancellationDocument = PMEReturnCode.PMTrue

        Catch excep As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetOriginalInsuranceFileCnt " & "Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            CheckAutoCancellationDocument = PMEReturnCode.PMFalse
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nCreditControlItemID"></param>
    ''' <param name="bSingleInstalmentPlan"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckSingleInstalmentPlan(ByVal nCreditControlItemID As Integer,
                                              ByRef bSingleInstalmentPlan As Boolean) As Integer

        Try
            CheckSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMTrue

            Dim oResultArray As Object = Nothing

            'Get the Information for Cancellation
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("nCredit_control_item_id", nCreditControlItemID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .SQLSelect("spu_Is_SingleInstallment_For_Credit_Control",
                                       "Is SingleInstallment For Credit Control", True, , oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CheckSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End With

            If IsArray(oResultArray) Then
                bSingleInstalmentPlan = ToSafeBoolean(oResultArray(0, 0))
            End If

        Catch excep As Exception
            CheckSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Check Single InstalmentPlan Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="CheckSingleInstalmentPlan", vErrNo:=Information.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="creditControlItemID"></param>
    ''' <param name="oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAutoCancelDetailForSingleInstalmentPlan(ByVal creditControlItemID As Integer,
                                                               ByRef oResultArray As Object) As Integer

        Try

            GetAutoCancelDetailForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMTrue

            'Get the Information for Cancellation
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("ncredit_control_item_id", creditControlItemID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .SQLSelect("spu_ACT_Select_Credit_Control_AutoCancel_For_SingleInstalment",
                                       "Get Auto Cancel Detail For Credit Control", True, , oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetAutoCancelDetailForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End With

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetAutoCancelDetailForSingleInstalmentPlan " & "Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetAutoCancelDetailForSingleInstalmentPlan", vErrNo:=Information.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)
            GetAutoCancelDetailForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nStepID"></param>
    ''' <param name="nCreditControlItemID"></param>
    ''' <param name="nInsutanceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateLetterForBrokarDaysAndOutstandingAmount(ByVal nStepID As Integer,
                                                                    ByVal nCreditControlItemID As Integer,
                                                                    ByVal nInsutanceFileCnt As Integer) As Integer
        Dim bSpoolDocuments As Boolean
        Dim bArchiveDocuments As Boolean
        Dim nBrokarLetterId As Integer
        Dim nPartyCnt As Integer

        Dim oResult As Object = Nothing
        Dim oResultClientCnt As Object = Nothing

        Try

            GenerateLetterForBrokarDaysAndOutstandingAmount = gPMConstants.PMEReturnCode.PMTrue

            Dim vCreditControlItems(0) As Object
            vCreditControlItems(0) = nCreditControlItemID

            m_lReturn = GetClientIDForSingleInstalmentPlan(nCreditControlItemID, oResultClientCnt)

            If IsArray(oResultClientCnt) Then
                nPartyCnt = ToSafeInteger(oResultClientCnt(0, 0))
            End If

            m_lReturn = GetStepDetailsForSingleInstalmentPlan(nStepID, oResult)

            If IsArray(oResult) Then
                nBrokarLetterId = ToSafeInteger(oResult(30, 0))
            End If

            If nBrokarLetterId > 0 Then

                m_lReturn = ProduceBrokarDaysAndOSAmountLetter(nBrokarLetterId,
                                                               "Letter For Brokar Days And OS Printed",
                                                               nCreditControlItemID,
                                                               0,
                                                               vCreditControlItems,
                                                               nPartyCnt,
                                                               bSpoolDocuments,
                                                               bArchiveDocuments)
            End If

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GenerateLetterForBrokarDaysAndOutstandingAmount Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GenerateLetterForBrokarDaysAndOutstandingAmount", vErrNo:=Information.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)
            GenerateLetterForBrokarDaysAndOutstandingAmount = gPMConstants.PMEReturnCode.PMFalse

        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nDocumentTemplateId"></param>
    ''' <param name="sDocumentTemplatePrintedDescription"></param>
    ''' <param name="nCreditControlItemID"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="oCreditControlItems"></param>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="bSpoolDocuments"></param>
    ''' <param name="bArchiveDocuments"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProduceBrokarDaysAndOSAmountLetter(ByVal nDocumentTemplateId As Integer,
                                                        ByVal sDocumentTemplatePrintedDescription As String,
                                                        ByVal nCreditControlItemID As Integer,
                                                        ByVal nInsuranceFileCnt As Integer,
                                                        ByVal oCreditControlItems As Object,
                                                        ByVal nPartyCnt As Integer,
                                                        ByVal bSpoolDocuments As Boolean,
                                                        ByVal bArchiveDocuments As Boolean) As Integer

        Const methodName As String = "ProduceBrokarDaysAndOSAmountLetter"

        Const kDocumentTypeStandardLetter As Integer = 5
        Dim nReturn As Integer

        Dim oDocManagerWrapper As Object
        Dim nSessionId As Integer
        Dim oFieldParams As Object

        Try

            ProduceBrokarDaysAndOSAmountLetter = gPMConstants.PMEReturnCode.PMTrue
            bSpoolDocuments = True

            ' CREATES DOCUMENT PRODUCTION LINKS TO CREDIT CONTROL
            nReturn = CreateTempSession(v_vCreditControlItems:=oCreditControlItems,
                                        r_lSessionID:=nSessionId)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "CreateTempSession Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            oDocManagerWrapper = CreateObject("bSIRDocManagerWrapper.Interface")
            nReturn = oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername,
                                                            sPassword:=m_sPassword,
                                                            iUserID:=m_iUserID,
                                                            iSourceID:=m_iSourceID,
                                                            iLanguageID:=m_iLanguageID,
                                                            iCurrencyID:=m_iCurrencyID,
                                                            iLogLevel:=m_iLogLevel,
                                                            sCallingAppName:=ACApp,
                                                            vDatabase:=m_oDatabase)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to create instance of bSIRDocManagerWrapper.Interface", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' SET UP DOCUMENT MANAGER
            oDocManagerWrapper.DocumentTemplateId = nDocumentTemplateId
            oDocManagerWrapper.SpoolDesc = sDocumentTemplatePrintedDescription
            oDocManagerWrapper.DocumentTypeId = kDocumentTypeStandardLetter
            oDocManagerWrapper.PartyCnt = nPartyCnt
            oDocManagerWrapper.InsuranceFileCnt = nInsuranceFileCnt
            oDocManagerWrapper.ArchiveDoc = bArchiveDocuments

            ReDim oFieldParams(0 To 1, 0 To 0)
            oFieldParams(0, 0) = "CreditControlItem"
            oFieldParams(1, 0) = nCreditControlItemID
            oDocManagerWrapper.FieldParameters = oFieldParams

            If bSpoolDocuments = True Then
                oDocManagerWrapper.Mode = ACSpoolDocMode
            Else
                oDocManagerWrapper.Mode = ACPrintSilentMode
            End If

            ' PRODUCE THE DOCUMENT
            nReturn = oDocManagerWrapper.Start
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "bSIRDocManagerWrapper.Interface.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = ReleaseTempSession(v_lSessionID:=nSessionId)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "ReleaseTempSession Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=methodName & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=methodName,
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            ProduceBrokarDaysAndOSAmountLetter = gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nStepID"></param>
    ''' <param name="o_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStepDetailsForSingleInstalmentPlan(ByVal nStepID As Integer,
                                                          ByRef o_oResultArray As Object) As Integer

        Try

            GetStepDetailsForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("credit_control_step_id", nStepID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .SQLSelect("spu_ACT_Select_Credit_Control_Step", "Get Step Details For SingleI nstalment Plan", True, , o_oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetStepDetailsForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End With

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetStepDetailsForSingleInstalmentPlan " & "Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetStepDetailsForSingleInstalmentPlan", vErrNo:=Information.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)
            GetStepDetailsForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ''' <summary>
    ''' GetClientIDForSingleInstalmentPlan
    ''' </summary>
    ''' <param name="nCreditControlItemID"></param>
    ''' <param name="o_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClientIDForSingleInstalmentPlan(ByVal nCreditControlItemID As Integer,
                                                       ByRef o_oResultArray As Object) As Integer
        Try

            GetClientIDForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("credit_control_Item_id", nCreditControlItemID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .SQLSelect("spu_ACT_Select_ClientID_For_Control_Control_Item ",
                                       "Get ClientID For Single Instalment Plan", True, , o_oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetClientIDForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End With

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetClientIDForSingleInstalmentPlan Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetClientIDForSingleInstalmentPlan", vErrNo:=Information.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)
            GetClientIDForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ''' <summary>
    ''' GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration
    ''' </summary>
    ''' <param name="nCreditControlItemID"></param>
    ''' <param name="o_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration(ByVal nCreditControlItemID As Integer,
                                                                                ByRef o_oResultArray As Object) As Integer

        Try
            GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("ncredit_control_item_id", nCreditControlItemID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .SQLSelect("spu_ACT_Select_Credit_Control_Details_For_DocsGeneration_For_SingleInstalment",
                                       "Get AutoCancel Detail For SingleInstalmentPlan For DocsGeneration", True, , o_oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End With

        Catch excep As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass,
                             v_sMethod:="GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration",
                             r_lFunctionReturn:=GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration)
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration", vErrNo:=Information.Err().Number,
                               vErrDesc:=excep.Message, excep:=excep)
            GetAutoCancelDetailForSingleInstalmentPlanForDocsGeneration = gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function

    ''' <summary>
    ''' GetDetailsForAutoAllocate
    ''' </summary>
    ''' <param name="nCreditControlItemID"></param>
    ''' <param name="o_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDetailsForAutoAllocate(ByVal nCreditControlItemID As Integer,
                                              ByRef o_oResultArray As Object) As Integer

        Try
            GetDetailsForAutoAllocate = gPMConstants.PMEReturnCode.PMTrue

            'Get the Information for Cancellation
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("ncredit_control_item_id", nCreditControlItemID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                m_lReturn = .SQLSelect("spu_ACT_GetDetailsForAutoAllocate_ForSIP", "Get Details For Auto Allocate For Single installemtn plan", True, , o_oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetDetailsForAutoAllocate = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End With

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetDetailsForAutoAllocate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsForAutoAllocate",
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
        Return m_lReturn

    End Function

    ''' <summary>
    ''' GetOutstandingBalanceAndTransactionsForSingleInstalmentPlan
    ''' </summary>
    ''' <param name="nAccountId"></param>
    ''' <param name="nPfPremiumFinanceCnt"></param>
    ''' <param name="r_crOutstandingBalance"></param>
    ''' <param name="o_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetOutstandingBalanceAndTransactionsForSingleInstalmentPlan(ByVal nAccountId As Integer,
                                                                                 ByVal nPfPremiumFinanceCnt As Integer,
                                                                                 ByRef r_crOutstandingBalance As Decimal,
                                                                                 ByRef o_oResults As Object) As Integer
        Const methodName As String = "GetOutstandingBalanceAndTransactionsForSingleInstalmentPlan"
        Dim nReturn As Integer
        Dim nCount As Integer

        Try

            GetOutstandingBalanceAndTransactionsForSingleInstalmentPlan = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = AddInputParameter(v_sName:="naccount_id", v_vValue:=nAccountId, v_iType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = AddInputParameter(v_sName:="npfprem_finance_cnt", v_vValue:=nPfPremiumFinanceCnt, v_iType:=gPMConstants.PMEDataType.PMInteger)

            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetOutstandingBalanceAndTransactionsForSIP,
                                            sSQLName:=kGetOutstandingBalanceAndTransactionsForSIP,
                                            bStoredProcedure:=True,
                                            vResultArray:=o_oResults,
                                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, kGetOutstandingBalanceAndTransactionsForInsuranceFolderSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If IsArray(o_oResults) Then
                For nCount = LBound(o_oResults, 2) To UBound(o_oResults, 2)
                    r_crOutstandingBalance = r_crOutstandingBalance + ToSafeCurrency(o_oResults(2, nCount))
                Next
            Else
                r_crOutstandingBalance = 0
            End If

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=methodName & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=methodName,
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try
        Return nReturn

    End Function

    ''' <summary>
    ''' CreditcontrolStepInUse
    ''' </summary>
    ''' <param name="nCreditControlStepID"></param>
    ''' <param name="o_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreditcontrolStepInUse(ByVal nCreditControlStepID As Integer,
                                           ByRef o_oResults As Object) As Integer

        Const kMethodName As String = "CreditcontrolStepInUse"
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        Try

            CreditcontrolStepInUse = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="CreditcontrolStepID", v_vValue:=nCreditControlStepID, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_CreditControlStepInUse",
                                            sSQLName:="spu_ACT_CreditControlStepInUse",
                                            bStoredProcedure:=True,
                                            vResultArray:=o_oResults,
                                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "spu_ACT_CreditControlStepInUse" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetOriginalInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return nReturn
    End Function

    ''' <summary>
    ''' GetInsuranceFolderCnt
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="o_nInsuranceFolderCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInsuranceFolderCnt(ByVal nInsuranceFileCnt As Integer,
                                           ByRef o_nInsuranceFolderCnt As Integer) As Integer
        Const kMethodName As String = "GetInsuranceFolderCnt"

        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Dim oResult As Object = Nothing

        Try

            GetInsuranceFolderCnt = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=nInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_insurance_folder",
                                            sSQLName:="spu_get_insurance_folder",
                                            bStoredProcedure:=True,
                                            vResultArray:=oResult,
                                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "spu_get_insurance_folder" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If IsArray(oResult) Then
                o_nInsuranceFolderCnt = ToSafeInteger(oResult(0, 0))
            End If

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="GetOriginalInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message,
                               excep:=excep)
        End Try
        Return nReturn
    End Function

    ''' <summary>
    ''' AutoAllocate
    ''' </summary>
    ''' <param name="oTransactions"></param>
    ''' <param name="crOutstandingBalance"></param>
    ''' <param name="oWriteOffToleranceAmount"></param>
    ''' <param name="oWriteOffReasonId"></param>
    ''' <param name="v_bIsAgentCommission"></param>
    ''' <returns>Success Flag as Integer</returns>
    ''' <remarks></remarks>
    Private Function AutoAllocate(ByVal oTransactions As Object,
                                  ByVal crOutstandingBalance As Decimal,
                                  ByVal oWriteOffToleranceAmount As Object,
                                  ByVal oWriteOffReasonId As Object,
                                  Optional ByVal v_bIsAgentCommission As Boolean = False) As Integer

        Const kMethodName As String = "AutoAllocate"
        Dim nReturn As Integer
        Dim crWriteOffAmount As Decimal
        Dim nRow As Integer
        Dim bTransHasSEC As Boolean
        Dim bTransHasSND As Boolean
        Dim bTransHasSRD As Boolean
        Dim crOutstandingAmount As Decimal
        Dim crCreditTotal As Decimal
        Dim crDebitTotal As Decimal

        Try

            nReturn = gPMConstants.PMEReturnCode.PMTrue

            nReturn = CalculateWriteOffAmount(v_crOutstandingBalance:=crOutstandingBalance,
                                              vWriteOffToleranceAmount:=oWriteOffToleranceAmount,
                                              r_crWriteOffAmount:=crWriteOffAmount)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "CalculateWriteOffAmount Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Allocations of SEC vs SND
            bTransHasSND = False
            bTransHasSEC = False
            bTransHasSRD = False
            crOutstandingAmount = ToSafeCurrency(crOutstandingBalance, 0)
            m_lReturn = ShellSort2DArray(oTransactions, 0)

            ' SRD to add besides SND
            For nRow = 0 To UBound(oTransactions, 2)
                If ToSafeString(oTransactions(3, nRow)) = "SEC" Then
                    bTransHasSEC = True
                End If
                If ToSafeString(oTransactions(3, nRow)) = "SND" Then
                    bTransHasSND = True
                End If
                If ToSafeString(oTransactions(3, nRow)) = "SRD" Then
                    bTransHasSRD = True
                End If

                If gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) < 0 Then
                    crCreditTotal += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                Else
                    crDebitTotal += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                End If
            Next

            'Settle the exact amount which satify for both debit and credit transaction
            'Taking the Decision to be allocated the amount by doing the CR and DR Same figure.
            Dim bHasAllocated As Boolean = False
            Dim crAmountToBeAllocated As Double
            Dim crAmountSumUp As Double

            If crWriteOffAmount = 0 Then ' it should work when writeoff is zero

                'There is two case ,one is  when Outstanding >0 ans second when Outstanding<0
                'This case wil run when outstanding amount>0
                If crOutstandingAmount > 0 Then
                    crAmountToBeAllocated = crCreditTotal
                    crCreditTotal = 0
                    crDebitTotal = 0
                    crAmountSumUp = 0

                    For nRow = 0 To UBound(oTransactions, 2)
                        'If bHasAllocated flag is true it's means that required Debit amount has settled down for the allocation
                        'So needs to set nothing if there are any extra Debit records
                        'Also ignore setting it to Nothing for the credit transaction. Credit Transactions are going to settle in full as toal credit amount  < total debit amount
                        If bHasAllocated AndAlso gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) > 0 Then
                            oTransactions(0, nRow) = Nothing
                            oTransactions(1, nRow) = Nothing
                            oTransactions(2, nRow) = Nothing
                            oTransactions(3, nRow) = Nothing
                        Else
                            'Do the Process untill the debit and credit is not equal
                            If gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) > 0 Then
                                crAmountSumUp += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))

                                If crAmountToBeAllocated + crAmountSumUp <= 0 Then
                                    oTransactions(2, nRow) = gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                                Else
                                    'Here we have to workout and assign back to the array with the required amount of credit only
                                    'This case will only happen when Credit amount exceeds the value as Debit amount
                                    oTransactions(2, nRow) = gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) - (crAmountToBeAllocated + crAmountSumUp)
                                    bHasAllocated = True
                                End If
                            End If
                        End If

                        If gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) < 0 Then
                            crCreditTotal += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                        Else
                            crDebitTotal += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                        End If
                    Next

                ElseIf crOutstandingAmount < 0 Then ' Case when Credit Amount would be Greater than Debit
                    crAmountToBeAllocated = crDebitTotal
                    crCreditTotal = 0
                    crDebitTotal = 0
                    crAmountSumUp = 0
                    For nRow = 0 To UBound(oTransactions, 2)

                        If bHasAllocated Then
                            oTransactions(0, nRow) = Nothing
                            oTransactions(1, nRow) = Nothing
                            oTransactions(2, nRow) = Nothing
                            oTransactions(3, nRow) = Nothing
                        Else
                            If gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) < 0 Then
                                crAmountSumUp += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))

                                If crAmountToBeAllocated + crAmountSumUp >= 0 Then
                                    oTransactions(2, nRow) = gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                                Else
                                    oTransactions(2, nRow) = gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) - (crAmountToBeAllocated + crAmountSumUp)
                                    bHasAllocated = True
                                End If

                            End If
                        End If
                        If gPMFunctions.ToSafeDouble(oTransactions(2, nRow)) < 0 Then
                            crCreditTotal += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                        Else
                            crDebitTotal += gPMFunctions.ToSafeDouble(oTransactions(2, nRow))
                        End If
                    Next
                End If

                If crCreditTotal + crDebitTotal <> 0 Then
                    RaiseError(kMethodName, "Debit and Credit mismatched", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            If crOutstandingBalance <> crWriteOffAmount Then
                m_lReturn = ShellSort2DArray(oTransactions, 0)
                If bTransHasSEC And (bTransHasSND Or bTransHasSRD) Then
                    For nRow = 0 To UBound(oTransactions, 2)

                        If (ToSafeString(oTransactions(3, nRow)) = "SND" OrElse ToSafeString(oTransactions(3, nRow)) = "SRD") Then
                            If crOutstandingAmount <= 0 Then
                                Exit For
                            End If
                            If ToSafeCurrency(oTransactions(2, nRow), 0) < crOutstandingAmount AndAlso v_bIsAgentCommission = False Then

                                crOutstandingAmount = crOutstandingAmount - ToSafeCurrency(oTransactions(2, nRow), 0)
                                oTransactions(2, nRow) = 0
                            Else
                                oTransactions(2, nRow) = oTransactions(2, nRow) - crOutstandingAmount
                                crOutstandingAmount = 0
                            End If
                        End If
                    Next
                End If
            End If

            nReturn = ProcessAutoAllocationAndWriteOff(v_vTransactions:=oTransactions,
                                                       v_lWriteOffReasonId:=ToSafeLong(oWriteOffReasonId, 0),
                                                       v_crWriteOffAmount:=crWriteOffAmount)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "ProcessAutoAllocationAndWriteOff Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="AutoAllocate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName,
                               vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
        End Try

        Return nReturn
    End Function

End Class
