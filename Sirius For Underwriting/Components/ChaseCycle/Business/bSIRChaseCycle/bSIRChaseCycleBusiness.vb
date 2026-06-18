Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
Imports SharedFiles
Imports SharedQuoteEngine

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' *****************************************************************
    ' Class Name: Business
    '
    ' Date: 01/03/2013
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              Chase Cycle Rules and Steps.
    '
    ' *****************************************************************


    ' ************************************************
    ' Added to replace global variables 01/03/2013
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

    ' SP to Select a list of Chase Cycle Rules
    Private Const ACSelAllRulesName As String = "SelAllChaseCycleRule"
    Private Const ACSelAllRulesSQL As String = "spu_SIR_SelAll_Chase_Cycle_Rule"

    ' SP to Select a list of Chase Cycle Steps
    Private Const ACSelAllStepsName As String = "SelAllChaseCycleStep"
    Private Const ACSelAllStepsSQL As String = "spu_SIR_SelAll_Chase_Cycle_Step"

    ' SP to Select a single Chase Cycle Rule
    Private Const ACSelRuleName As String = "SelectChaseCycleRule"
    Private Const ACSelRuleSQL As String = "spu_SIR_Select_Chase_Cycle_Rule"

    ' SP to Select a single Chase Cycle Step
    Private Const ACSelStepName As String = "SelectChaseCycleStep"
    Private Const ACSelStepSQL As String = "spu_SIR_Select_Chase_Cycle_Step"

    ' SP to Add a Chase Cycle Rule
    Private Const ACDirectAddRuleName As String = "AddChaseCycleRule"
    Private Const ACDirectAddRuleSQL As String = "spu_SIR_Add_Chase_Cycle_Rule"

    ' SP to Add a Chase Cycle Step
    Private Const ACDirectAddStepName As String = "AddChaseCycleStep"
    Private Const ACDirectAddStepSQL As String = "spu_SIR_Add_Chase_Cycle_Step"

    ' SP to Edit a Chase Cycle Rule
    Private Const ACDirectEditRuleName As String = "UpdateChaseCycleRule"
    Private Const ACDirectEditRuleSQL As String = "spu_SIR_Update_Chase_Cycle_Rule"

    ' SP to Edit a Chase Cycle Step
    Private Const ACDirectEditStepName As String = "UpdateChaseCycleRule"
    Private Const ACDirectEditStepSQL As String = "spu_SIR_Update_Chase_Cycle_Step"

    ' SP to Delete a Chase Cycle Rule
    Private Const ACDirectDeleteRuleName As String = "DeleteChaseCycleRule"
    Private Const ACDirectDeleteRuleSQL As String = "spu_SIR_Delete_Chase_Cycle_Rule"

    ' SP to Delete a Chase Cycle Step
    Private Const ACDirectDeleteStepName As String = "DeleteChaseCycleStep"
    Private Const ACDirectDeleteStepSQL As String = "spu_SIR_Delete_Chase_Cycle_Step"


    ' SP to Get details for use when running the auto-cancel rules script
    Private Const ACGetItemDetailsForScriptName As String = "GetItemDetailsForScript"
    Private Const ACGetItemDetailsForScriptSQL As String = "spu_ACT_Get_CC_Item_Insurance_File_Dets"

    ' SP to Get party name from account
    Private Const ACGetPartyNameFromAccountName As String = "GetPartyNameFromAccount"
    Private Const ACGetPartyNameFromAccountSQL As String = "spu_SIR_Get_Party_Name_From_Account"

    ' SP to get the Chase Cycle letters to print for client
    Private Const ACGetChaseCycleDocIDsName As String = "GetChaseCycleDocIDs"
    Private Const ACGetChaseCycleDocIDsSQL As String = "spu_SIR_Get_Chase_Cycle_Doc_IDs"

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
    Private Const ACGetAccountDetailsSQL As String = "spu_SIR_select_Account"

    Private Const ACGetChaseCyclePropertiesName As String = "SelectProperties"
    Private Const ACGetChaseCyclePropertiesSQL As String = "spu_get_chase_cycle_properties"

    Private Const ACGetChaseCycleUDLSQL As String = "spu_get_chase_cycle_udl"
    Private Const ACGetChaseCycleUDLName As String = "GetChaseCycleUDL"

    'Result Array columns for GetDetails for Chase Cycle Rule
    Private Const ACRChaseCycleRuleID As Integer = 0
    Private Const ACRDescription As Integer = 1
    Private Const ACRSourceID As Integer = 2
    Private Const ACRGISDataModel As Integer = 3
    Private Const ACRGISProperty As Integer = 4
    Private Const ACRChaseCycleStatusID As Integer = 5
    Private Const ACRIsActive As Integer = 6
    Private Const ACRProcssingDays As Integer = 7
    Private Const ACRUseEffectiveDate As Integer = 8
    Private Const ACRUseGreaterTransEffDate As Integer = 9
    Private Const ACRProductID As Integer = 10
    Private Const ACRIncludeCancelled As Integer = 11
    Private Const ACRCancelledOnly As Integer = 12
    Private Const ACGISDataModelCode As Integer = 13

    'Result Array columns for GetDetails for Chase Cycle Step
    Private Const ACSChaseCycleStepID As Integer = 0
    Private Const ACSChaseCycleRuleID As Integer = 1
    Private Const ACSStepNumber As Integer = 2
    Private Const ACSNumberOfDays As Integer = 3
    Private Const ACSClientDocumentTemplateID As Integer = 4
    Private Const ACSPMWrkTaskID As Integer = 5
    Private Const ACSPMUserGroupID As Integer = 6
    Private Const ACSCheckAutoCancel As Integer = 7
    Private Const ACSAutoCancelPolicy As Integer = 8
    Private Const ACSNextStep As Integer = 9
    Private Const ACSPreviousStep As Integer = 10
    Private Const ACSStepDescription As Integer = 11
    Private Const ACSPMWrkTaskGroupId As Integer = 12

    ' Constants for add or edit
    Private Const ACUpdateAdd As Byte = 1
    Private Const ACUpdateEdit As Byte = 2
    Private Const ACInsCoverStartDate As Integer = 4
    Private Const ACInsPolicyVersion As Integer = 2

    ' Constants for Chase Cycle Doc IDs array
    Private Const ACDIClientDocTemplateID As Integer = 0
    Private Const ACDIOIPDocTemplateID As Integer = 1
    Private Const ACDIClientDocTemplateCode As Integer = 4

    Private Const ACDIBusinessType As Integer = 4
    Private Const ACDIBrokerDocTemplateId As Integer = 5

    ' Constatnt for document printing mode
    Private Const ACPrintSilentMode As Integer = 3
    Private Const ACSpoolDocMode As Integer = 4

    Private Const ACDIInsuranceFileCnt As Integer = 1
    Private Const ACDIChaseCycleItemId As Integer = 2
    Private Const ACDIInsuranceFolderCnt As Integer = 3
    Private Const ACDIClientDocTemplateDesc = 4
    Private Const ACDIPartyCnt As Integer = 5

    ' *****************************************************************
    ' Name: AutoCancel (Public)
    '
    ' Description: Run the auto cancel rules VBScript.
    '
    '
    ' *****************************************************************
    Public Function AutoCancel(ByVal v_lChaseCycleItemId As Integer, ByVal v_bCheckRulesOnly As Boolean, ByRef r_bAutoCancelResult As Boolean, Optional ByVal v_bArchiveDoc As Boolean = False, Optional ByVal v_bSpoolDoc As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim vChaseCycleRuleID, vPMUserId, vIsDeleted As Object
        Dim oChaseCycleItem As bSIRChaseCycle.ChaseCycleItem
        Dim oEventLog As bSIREvent.Business
        Dim vChaseCycleStepID As Object
        Dim vInsuranceFileCnt As Object
        Dim vCanAutoCancel As Object
        Dim vPMUserGroupID As Object
        Dim vChaseCycleReason, vInsuranceFolderCnt, vWillAutoCancel, vCreatedDate, vDueDate, vLetterSent As Object
        Dim vPMWrkTaskID As Object
        Dim vBusinessType As String = ""
        Dim vResultArray(,) As Object
        Dim sCustomer As String
        Dim dCoverStartDate As Date
        Dim lPartyCnt As Integer
        Dim sOptionValue As String = String.Empty


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bAutoCancelResult = False ' added just in case we exit before setting it

            ' Create an instance of the ChaseCycleItem business object

            oChaseCycleItem = New bSIRChaseCycle.ChaseCycleItem
            m_lReturn = oChaseCycleItem.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Chase Cycle Item details for the passed ID

            m_lReturn = oChaseCycleItem.GetDetails(v_lChaseCycleItemId:=v_lChaseCycleItemId, r_vChaseCycleReason:=vChaseCycleReason, r_vInsuranceFileCnt:=vInsuranceFileCnt, r_vInsuranceFolderCnt:=vInsuranceFolderCnt, r_vCanAutoCancel:=vCanAutoCancel, r_vWillAutoCancel:=vWillAutoCancel, r_vChaseCycleStepID:=vChaseCycleStepID, r_vCreatedDate:=vCreatedDate, r_vDueDate:=vDueDate, r_vLetterSent:=vLetterSent, v_vPMUserGroupId:=vPMUserGroupID, v_vPMUserId:=vPMUserId, v_vIsDeleted:=vIsDeleted)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' validate CCI details
            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And vInsuranceFileCnt > 0 Then
                ' this is ok
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="InsuranceFileCnt missing from Chase Cycle item", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancel")
                Return result
            End If

            ' Get the Chase Cycle step details for this item
            m_lReturn = GetStepDetails(v_lChaseCycleStepId:=vChaseCycleStepID, r_vChaseCycleRuleID:=vChaseCycleRuleID, r_vPMWrkTaskID:=vPMWrkTaskID, r_vPMUserGroupID:=vPMUserGroupID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Chase Cycle rule details for this item

            m_lReturn = CType(GetRuleDetails(v_lChaseCycleRuleId:=CInt(vChaseCycleRuleID)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
            If Information.IsDate(vResultArray(ACINSCoverStartDate, 0)) Then

                dCoverStartDate = CDate(vResultArray(ACINSCoverStartDate, 0))
            End If

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=GeneralConst.kSystemOptionRuleTypeChaseCycle, r_sOptionValue:=sOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sOptionValue = "1" Then
                'if .Rul script is selected
                m_lReturn = ExecuteRuleScripting(vBusinessType:=vBusinessType, vCanAutoCancel:=vCanAutoCancel, vResultArray:=vResultArray, r_bAutoCancelResult:=r_bAutoCancelResult)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                m_lReturn = ExecuteRulesCompiled(vBusinessType:=vBusinessType, vCanAutoCancel:=vCanAutoCancel, vResultArray:=vResultArray, r_bAutoCancelResult:=r_bAutoCancelResult)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' If not only checking rules then check status of autocancel flag
            If Not v_bCheckRulesOnly Then

                ' If not autocancel then create a work manager task
                If Not r_bAutoCancelResult Then

                    ' Create the work mgr task
                    '  if no group or task specified, then
                    ' do not create a task
                    If vPMUserGroupID <> "" And vPMWrkTaskID <> "" Then
                        m_lReturn = CType(CreateWorkManagerTask(v_lPMUserGroupID:=CInt(vPMUserGroupID), v_lPMWrkTaskID:=vPMWrkTaskID, v_sCustomer:=sCustomer, v_sDescription:="Manual Debt Review", v_dtTaskDueDate:=DateTime.Now), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    ' If autocancel, then cancel the policy...
                Else
                    ' Not on instalments so just use the start date as the
                    ' lapsed date


                End If

                ' Cancel the policy
                ' Pass lapsed date
                m_lReturn = CType(CancelPolicy(lChaseCycleItemID:=v_lChaseCycleItemId, dLapsedDate:=m_dtEffectiveDate), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

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

            ' Kill the Chase Cycle item object

            oChaseCycleItem.Dispose()

            oChaseCycleItem = Nothing

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
    ''' execute compiled rules base on Rule type
    ''' </summary>
    ''' <param name="vBusinessType"></param>
    ''' <param name="vCanAutoCancel"></param>
    ''' <param name="vResultArray"></param>
    ''' <param name="r_bAutoCancelResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteRulesCompiled(ByVal vBusinessType As String, ByVal vCanAutoCancel As Boolean, ByVal vResultArray(,) As Object, ByRef r_bAutoCancelResult As Boolean) As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim oSharedStorage As SharedStorage
        Dim oRules As Object
        Dim sAssemblyClassName As String = String.Empty

        ' Create shared storage object, used to hold values that are
        ' read/writable from the VB script file
        oSharedStorage = New SharedStorage()

        oSharedStorage.BusinessType = vBusinessType
        '  trim strings from result array
        oSharedStorage.CanAutoCancel = (Conversion.Val(vCanAutoCancel) = 1)
        oSharedStorage.PolicyVersion = CInt(Conversion.Val(CStr(vResultArray(ACInsPolicyVersion, 0))))

        ' Read in the script and run it
        result = CType(GetCompiledRuleFile(v_sScript:=sAssemblyClassName), gPMConstants.PMEReturnCode)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
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
        End If

        ' Kill the script and shared storage objects
        oSharedStorage = Nothing
        Return result
    End Function

    ''' <summary>
    ''' get compiled rule file name from system option
    ''' </summary>
    ''' <param name="v_sScript"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCompiledRuleFile(ByRef v_sScript As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        nResult = iPMFunc.GetSystemOption(v_iOptionNumber:=GeneralConst.kSystemOptionCompiledRuleChaseCycle, r_sOptionValue:=v_sScript)

        Return nResult

    End Function

    ''' <summary>
    ''' execute VBscript .rul file base on Rule type
    ''' </summary>
    ''' <param name="vBusinessType"></param>
    ''' <param name="vCanAutoCancel"></param>
    ''' <param name="vResultArray"></param>
    ''' <param name="r_bAutoCancelResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteRuleScripting(ByVal vBusinessType As String, ByVal vCanAutoCancel As Boolean, ByVal vResultArray(,) As Object, ByRef r_bAutoCancelResult As Boolean) As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim oSharedStorage As SharedStorage
        Dim sScript As String = String.Empty
        Dim sMethodName As String = "start"
        Dim oVBQuoteEngine As SharedQuoteEngine.VBQuoteEngine

        ' Create script control object
        oVBQuoteEngine = New VBQuoteEngine()

        ' Create shared storage object, used to hold values that are
        ' read/writable from the VB script file
        oSharedStorage = New SharedStorage()


        oSharedStorage.BusinessType = vBusinessType
        '  trim strings from result array

        oSharedStorage.CanAutoCancel = (Conversion.Val(vCanAutoCancel) = 1)

        oSharedStorage.PolicyVersion = CInt(Conversion.Val(CStr(vResultArray(ACInsPolicyVersion, 0))))

        ' Read in the script and run it
        result = CType(GetScriptFile(v_sScript:=sScript), gPMConstants.PMEReturnCode)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sScript = Replace(sScript, "oSharedStorage.Credit", "CLng(oSharedStorage.Credit)")

        oVBQuoteEngine.RunMediaTypeValidation(sScript, sMethodName, oSharedStorage)

        ' Retrieve autocancel flag
        r_bAutoCancelResult = oSharedStorage.AutoCancel

        ' Kill the script and shared storage objects
        oSharedStorage = Nothing
        oVBQuoteEngine = Nothing
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
    '           Created :  01/03/2013
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

    '************************************************************************
    ' Name: CreateTempSession (private)
    '
    ' Description: Gets the next unique Session ID and adds the passed Credit
    '              Control Item ID's to the TempLinkID table.
    '
    ' Created: 01/03/2013
    '************************************************************************
    Private Function CreateTempSession(ByVal v_vChaseCycleItems As Object, ByRef r_lSessionID As Integer) As Integer

        Dim result As Integer = 0

        Dim lLBound As Integer
        Dim lUBound As Integer

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


        lLBound = v_vChaseCycleItems.GetLowerBound(0) ' Performance Enhancement

        lUBound = v_vChaseCycleItems.GetUpperBound(0) '  Pefrormance Enhancement

        For i As Integer = lLBound To lUBound

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Session ID input parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lSessionID", vValue:=CStr(r_lSessionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Link ID (Chase Cycle Item) input parameter

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lLinkID", vValue:=CStr(v_vChaseCycleItems(i)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
    ' Created: 01/03/2013
    '************************************************************************
    Public Function CreateWorkManagerTask(ByVal v_lPMUserGroupID As Integer, ByVal v_lPMWrkTaskID As String, ByVal v_sCustomer As String, ByVal v_sDescription As String, Optional ByVal v_dtTaskDueDate As Date = #12/30/1899#, Optional ByVal v_iTaskStatus As Integer = 0, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_dtDateCreated As Date = #12/30/1899#, Optional ByVal v_iCreatedByID As Integer = 1, Optional ByRef r_lPMWrkTaskInstanceCnt As Integer = 0, Optional ByRef v_iIsVisible As Integer = 1, Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_lPMWrkTaskActionTypeId As Object = Nothing) As Integer

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
    ' Created: 01/03/2013
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
    '              Use the document template specified in the Chase
    '              Cycle Step record
    '
    ' Created: 01/03/2013
    '  
    ' *****************************************************************
    Public Function ProduceClientLetters(ByVal v_vChaseCycleItems As Object, Optional ByVal v_bSpoolDocuments As Boolean = False, Optional ByVal v_bArchiveDocuments As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
        Dim oEventLog As bSIREvent.Business
        Dim lSessionID As Integer
        Dim vResultArray(,) As Object
        Dim lLBound, lUBound As Integer
        Dim bProduceLetter As Boolean
        Dim sEventDescription As String
        Dim vFieldParams As Object
        Dim lDocumasterID As Integer
        Dim lPartyCnt As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a Session and store the passed Chase Cycle Item IDs in it
            m_lReturn = CType(CreateTempSession(v_vChaseCycleItems:=v_vChaseCycleItems, r_lSessionID:=lSessionID), gPMConstants.PMEReturnCode)

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
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetChaseCycleDocIDsSQL, sSQLName:=ACGetChaseCycleDocIDsName, bStoredProcedure:=True, lNumberRecords:=SharedFiles.gPMConstants.PMAllRecords, vResultArray:=vResultArray)

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
            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
            m_lReturn = CType(oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocManagerWrapper.InitialiseBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Loop through the array
            If Information.IsArray(vResultArray) Then


                lLBound = vResultArray.GetLowerBound(1)

                lUBound = vResultArray.GetUpperBound(1)

                ' for each Chase Cycle item
                For i As Integer = lLBound To lUBound


                    lPartyCnt = CInt(Conversion.Val(CStr(vResultArray(ACDIPartyCnt, i))))

                    ' reset produce letter flag
                    bProduceLetter = False

                    ' get the business type

                    If Conversion.Val(CStr(vResultArray(ACDIClientDocTemplateID, i))) > 0 Then
                        bProduceLetter = True

                    End If
                    If bProduceLetter Then

                        oDocManagerWrapper.DocumentTemplateId = CInt(Conversion.Val(CStr(vResultArray(ACDIClientDocTemplateID, i))))
                        oDocManagerWrapper.DocumentTemplateCode = CStr(vResultArray(ACDIClientDocTemplateCode, i))
                        oDocManagerWrapper.SpoolDesc = "Chase Cycle Letter - " & ToSafeString(vResultArray(ACDIClientDocTemplateDesc, i))
                        If v_bSpoolDocuments Then
                            sEventDescription = "Chase Cycle Letter generated"
                        Else
                            sEventDescription = "Chase Cycle Letter printed"
                        End If

                        ' Set the appropriate properties
                        oDocManagerWrapper.DocumentTypeId = 5 ' standard letter

                        oDocManagerWrapper.PartyCnt = lPartyCnt

                        oDocManagerWrapper.InsuranceFileCnt = CInt(Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, i))))

                        oDocManagerWrapper.ArchiveDoc = v_bArchiveDocuments

                        oDocManagerWrapper.InsuranceFolderCnt = CInt(Conversion.Val(CStr(vResultArray(ACDIInsuranceFolderCnt, i))))

                        'Documents generated from Chase Cycle will be visible from web only if CalledFromSAM is True.So setting it to true
                        oDocManagerWrapper.CalledFromSAM = True

                        ReDim vFieldParams(1, 0)

                        vFieldParams(0, 0) = "ChaseCycleItem"

                        vFieldParams(1, 0) = Conversion.Val(CStr(vResultArray(ACDIChaseCycleItemId, i)))

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

                        m_lReturn = oEventLog.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFileCnt:=Conversion.Val(CStr(vResultArray(ACDIInsuranceFileCnt, i))), vEventType:=10, vUserID:=m_iUserID, vEventDate:=DateTime.Now, vDescription:=sEventDescription, vDocumentCnt:=lDocumasterID)

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
                    m_oDatabase = Nothing
                End If
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
    ' Description: Select multiple Chase Cycle Rule records from
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Chase Cycle Rule records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' *****************************************************************
    ' Name: GetStepList (Public)
    '
    ' Description: Select multiple Chase Cycle Step records from
    ' the database.
    '
    ' *****************************************************************
    Public Function GetStepList(ByVal v_lChaseCycleRuleId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Chase Cycle Rule ID INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_rule_id", vValue:=CStr(v_lChaseCycleRuleId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Chase Cycle Step records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' *****************************************************************
    ' Name: GetStepDetails (Public)
    '
    ' Description: Select a single Chase Cycle Step record from the
    ' database.
    '
    ' *****************************************************************

    Public Function GetStepDetails(ByVal v_lChaseCycleStepId As Object, Optional ByRef r_vChaseCycleRuleID As Object = Nothing, Optional ByRef r_vStepNumber As Object = Nothing, Optional ByRef r_vNumberOfDays As Object = Nothing, Optional ByRef r_vClientDocumentTemplateID As Object = Nothing, Optional ByRef r_vPMWrkTaskID As Object = Nothing, Optional ByRef r_vPMUserGroupID As Object = Nothing, Optional ByRef r_vCheckAutoCancel As Object = Nothing, Optional ByRef r_vAutoCancelPolicy As Object = Nothing, Optional ByRef r_vNextStep As Object = Nothing, Optional ByRef r_vPreviousStep As Object = Nothing, Optional ByRef r_vStepDescription As Object = Nothing, Optional ByRef r_vPMWrkTaskGroupId As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vResultArray(,) As Object
        Const klFirstRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Chase Cycle step id INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_step_id", vValue:=v_lChaseCycleStepId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelStepSQL, sSQLName:=ACSelStepName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Populate the params

            r_vChaseCycleRuleID = vResultArray(ACSChaseCycleRuleID, klFirstRow)

            r_vStepNumber = vResultArray(ACSStepNumber, klFirstRow)

            r_vNumberOfDays = vResultArray(ACSNumberOfDays, klFirstRow)

            r_vClientDocumentTemplateID = vResultArray(ACSClientDocumentTemplateID, klFirstRow)

            r_vPMWrkTaskID = vResultArray(ACSPMWrkTaskID, klFirstRow)

            r_vPMUserGroupID = vResultArray(ACSPMUserGroupID, klFirstRow)

            r_vCheckAutoCancel = vResultArray(ACSCheckAutoCancel, klFirstRow)

            r_vAutoCancelPolicy = vResultArray(ACSAutoCancelPolicy, klFirstRow)

            r_vNextStep = vResultArray(ACSNextStep, klFirstRow)

            r_vPreviousStep = vResultArray(ACSPreviousStep, klFirstRow)

            r_vStepDescription = vResultArray(ACSStepDescription, klFirstRow)

            r_vPMWrkTaskGroupId = vResultArray(ACSPMWrkTaskGroupId, klFirstRow)

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Chase Cycle Step", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStepDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            sSQLString = sSQLString & "where document_type_id in (4,5) and document_template.is_deleted = 0 "


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
    ' Description: Select a single Chase Cycle Rule record from the
    ' database.
    '
    ' *****************************************************************
    Public Function GetRuleDetails(ByVal v_lChaseCycleRuleId As Integer, _
                                   Optional ByRef r_vDescription As String = "", _
                                   Optional ByRef r_vSourceID As String = "", _
                                   Optional ByRef r_vGISDataModel As String = "", Optional ByRef r_vGISPropertyId As String = "", _
                                   Optional ByRef r_vChaseCycleUDLID As Object = Nothing, _
                                   Optional ByRef r_vIsActive As Object = Nothing, _
                                   Optional ByRef r_vProcessingDays As Object = Nothing, _
                                   Optional ByRef r_vUseEffectiveDate As Object = Nothing, _
                                   Optional ByRef r_vUseGreaterTransEffDate As Object = Nothing, _
                                   Optional ByRef r_vProductId As Integer = 0, _
                                   Optional ByRef r_vIncludeCancelled As Object = Nothing, _
                                   Optional ByRef r_vCancelledOnly As Object = Nothing, Optional ByVal v_vGISDataModelCode As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Const klFirstRow As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Chase Cycle rule id INPUT parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_rule_id", vValue:=CStr(v_lChaseCycleRuleId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRuleSQL, sSQLName:=ACSelRuleName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Populate the params

            r_vDescription = vResultArray(ACRDescription, klFirstRow)
            r_vSourceID = vResultArray(ACRSourceID, klFirstRow)
            r_vGISDataModel = vResultArray(ACRGISDataModel, klFirstRow)
            r_vGISPropertyId = vResultArray(ACRGISProperty, klFirstRow)
            r_vChaseCycleUDLID = vResultArray(ACRChaseCycleStatusID, klFirstRow)
            r_vIsActive = vResultArray(ACRIsActive, klFirstRow)
            r_vProcessingDays = vResultArray(ACRProcssingDays, klFirstRow)
            r_vUseEffectiveDate = vResultArray(ACRUseEffectiveDate, klFirstRow)
            r_vUseGreaterTransEffDate = vResultArray(ACRUseGreaterTransEffDate, klFirstRow)
            r_vProductId = gPMFunctions.ToSafeLong(vResultArray(ACRProductID, klFirstRow), -1)
            r_vIncludeCancelled = gPMFunctions.ToSafeInteger(vResultArray(ACRIncludeCancelled, klFirstRow), -1)
            r_vCancelledOnly = gPMFunctions.ToSafeInteger(vResultArray(ACRCancelledOnly, klFirstRow), -1)
            v_vGISDataModelCode = gPMFunctions.ToSafeInteger(vResultArray(ACGISDataModelCode, klFirstRow), -1)

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get the Chase Cycle Rule", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '*********************************************************************
    ' Name: GetScriptFile
    '
    ' Description : Find and read the VBScript file
    '
    ' Created: 01/03/2013
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
        sFullPath = sPathName & "CHASE_CYCLE_AUTO_CANCELLATION.rul"

        ' Ensure the file exists
        If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CHASE_CYCLE_AUTO_CANCELLATION.rul file not found", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScriptFile")

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

    ' *****************************************************************
    ' Name: DirectAddRule (Public)
    '
    ' Description: Adds a Chase Cycle Rule record to the database
    '
    ' *****************************************************************
    Public Function DirectAddRule(ByRef r_vChaseCycleRuleID As Object, ByVal v_vDescription As Object, ByVal v_vSourceID As Object, _
                                  ByVal v_vGISDataModel As Object, ByVal v_vGISProperty As Object, ByVal v_vChaseCycleStatusID As Object, ByVal v_vIsActive As Object, _
                                  Optional ByVal v_vProcessingDays As Object = 0, Optional ByVal v_vUseEffectiveDate As Object = 0, _
                                  Optional ByVal v_vUseGreaterTranEffDate As Object = 0, Optional ByVal v_vProductId As Object = 0, _
                                  Optional ByVal v_vIncludeCancelled As Object = 0, Optional ByVal v_vCancelledOnly As Object = 0) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "DirectAddRule"
        Dim lRecordsAffected As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            AddOutputParameter("Chase_Cycle_rule_id", r_vChaseCycleRuleID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("description", v_vDescription, gPMConstants.PMEDataType.PMString)
            AddInputParameter("source_id", v_vSourceID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("GIS_data_model_id", v_vGISDataModel, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("GIS_property_id", v_vGISProperty, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("chase_cycle_status_udl_value_id", v_vChaseCycleStatusID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("is_active", v_vIsActive, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_effective_date", v_vUseEffectiveDate, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_Greater_TransEff_date", v_vUseGreaterTranEffDate, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("include_cancelled", v_vIncludeCancelled, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("cancelled_only", v_vCancelledOnly, gPMConstants.PMEDataType.PMInteger)

            If v_vProcessingDays <> StringsHelper.ToDoubleSafe("") Then
                AddInputParameter("Processing_Days", v_vProcessingDays, gPMConstants.PMEDataType.PMLong)
            Else
                AddInputParameter("Processing_Days", 0, gPMConstants.PMEDataType.PMLong)
            End If

            AddInputParameter("product_id", v_vProductId, gPMConstants.PMEDataType.PMLong)

            lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddRuleSQL, sSQLName:=ACDirectAddRuleName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDirectAddRuleSQL & " Failed")
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected > 0 Then
                r_vChaseCycleRuleID = m_oDatabase.Parameters.Item("Chase_Cycle_rule_id").Value
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
    ' Description: Adds a Chase Cycle Step record to the database
    '
    ' *****************************************************************

    Public Function DirectAddStep(ByRef r_vChaseCycleStepID As Object, ByVal v_vChaseCycleRuleID As Object, ByVal v_vStepNumber As Object, ByVal v_vNumberOfDays As Object, ByVal v_vClientDocumentTemplateID As Object, ByVal v_vPMWrkTaskID As Object, ByVal v_vPMUserGroupID As Object, ByVal v_vCheckAutoCancel As Object, ByVal v_vAutoCancelPolicy As Object, ByVal v_vNextStep As Object, ByVal v_vPreviousStep As Object, ByVal v_vStepDescription As Object, ByVal v_vPMWrkTaskGroupId As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Const kMethodName As String = "DirectAddStep"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Chase Cycle Step id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_step_id", vValue:=r_vChaseCycleStepID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add Chase Cycle rule id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_rule_id", vValue:=v_vChaseCycleRuleID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            ' Add client document template id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="client_document_template_id", vValue:=v_vClientDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=v_vPMWrkTaskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If


            ' Add user group id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=v_vPMUserGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If


            ' Add check auto cancel as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="check_auto_cancel", vValue:=v_vCheckAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add auto cancel policy as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_cancel_policy", vValue:=v_vAutoCancelPolicy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add next step as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="next_step", vValue:=v_vNextStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add previous step as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="previous_step", vValue:=v_vPreviousStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add pmwrk_task_group_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=v_vPMWrkTaskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add Chase Cycle step description as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="step_description", vValue:=v_vStepDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectAddStepSQL, sSQLName:=ACDirectAddStepName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected > 0 Then

                r_vChaseCycleStepID = m_oDatabase.Parameters.Item("Chase_Cycle_step_id").Value
            Else
                ' Nothing affected, so set to error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
        End Try

        Return result

    End Function



    ' *****************************************************************
    ' Name: DirectEditRule (Public)
    '
    ' Description: Edits a Chase Cycle Rule record in the database
    '
    ' *****************************************************************
    Public Function DirectEditRule(ByVal v_vChaseCycleRuleID As Object, ByVal v_vDescription As Object, ByVal v_vSourceID As Object, _
                                  ByVal v_vGISDataModel As Object, ByVal v_lGISPropertyID As Object, ByVal v_vChaseCycleStatusID As Object, ByVal v_vIsActive As Object, _
                                  Optional ByVal v_vProcessingDays As Object = 0, Optional ByVal v_vUseEffectiveDate As Object = 0, _
                                  Optional ByVal v_vUseGreaterTranEffDate As Object = 0, Optional ByVal v_vProductId As Object = 0, _
                                  Optional ByVal v_vIncludeCancelled As Object = 0, Optional ByVal v_vCancelledOnly As Object = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DirectEditRule"
        Dim lRecordsAffected As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Chase Cycle Rule id
            AddInputParameter("Chase_Cycle_rule_id", v_vChaseCycleRuleID, gPMConstants.PMEDataType.PMLong)

            AddInputParameter("description", v_vDescription, gPMConstants.PMEDataType.PMString)
            AddInputParameter("source_id", v_vSourceID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("GIS_data_model_id", v_vGISDataModel, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("GIS_property_id", v_lGISPropertyID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("chase_cycle_status_udl_value_id", v_vChaseCycleStatusID, gPMConstants.PMEDataType.PMLong)
            AddInputParameter("is_active", v_vIsActive, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_effective_date", v_vUseEffectiveDate, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("use_Greater_TransEff_date", v_vUseGreaterTranEffDate, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("include_cancelled_policies", v_vIncludeCancelled, gPMConstants.PMEDataType.PMInteger)
            AddInputParameter("cancelled_only", v_vCancelledOnly, gPMConstants.PMEDataType.PMInteger)

            If v_vProcessingDays <> StringsHelper.ToDoubleSafe("") Then
                AddInputParameter("Processing_Days", v_vProcessingDays, gPMConstants.PMEDataType.PMLong)
            Else
                AddInputParameter("Processing_Days", 0, gPMConstants.PMEDataType.PMLong)
            End If

            AddInputParameter("product_id", v_vProductId, gPMConstants.PMEDataType.PMLong)

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
    ' Description: Edits a Chase Cycle Step record in the database
    '
    ' *****************************************************************
    Public Function DirectEditStep(ByVal v_vChaseCycleStepID As Object, ByVal v_vChaseCycleRuleID As Object, ByVal v_vStepNumber As Object, ByVal v_vNumberOfDays As Object, ByVal v_vClientDocumentTemplateID As Object, ByVal v_vPMWrkTaskID As Object, ByVal v_vPMUserGroupID As Object, ByVal v_vCheckAutoCancel As Object, ByVal v_vAutoCancelPolicy As Object, ByVal v_vNextStep As Object, ByVal v_vPreviousStep As Object, ByVal v_vStepDescription As Object, ByVal v_vPMWrkTaskGroupId As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Const kMethodName As String = "DirectAddStep"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Chase Cycle Step id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_step_id", vValue:=v_vChaseCycleStepID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_rule_id", vValue:=v_vChaseCycleRuleID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            ' Add client document template id as an input param for an insert


            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=v_vClientDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=v_vPMWrkTaskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add user group id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_group_id", vValue:=v_vPMUserGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If


            ' Add check auto cancel as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="check_auto_cancel", vValue:=v_vCheckAutoCancel, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add auto cancel policy as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="auto_cancel_policy", vValue:=v_vAutoCancelPolicy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add next step as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="next_step", vValue:=v_vNextStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add previous step as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="previous_step", vValue:=v_vPreviousStep, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add pmwrk_task_group_id as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_group_id", vValue:=v_vPMWrkTaskGroupId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Add Chase Cycle step description as an input param for an insert

            m_lReturn = m_oDatabase.Parameters.Add(sName:="step_description", vValue:=v_vStepDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Parameters.Add Failed")
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectEditStepSQL, sSQLName:=ACDirectEditStepName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record was added, retrieve the ID
            If lRecordsAffected = 0 Then
                ' Nothing affected, so set to error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)
            Return result
        End Try

        Return result

    End Function

    ' *****************************************************************
    ' Name: DirectDeleteStep (Public)
    '
    ' Description: Deletes a Chase Cycle Step record
    '
    ' *****************************************************************
    Public Function DirectDeleteStep(ByVal v_lChaseCycleStepId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add Chase Cycle step id as an input param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_step_id", vValue:=CStr(v_lChaseCycleStepId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDirectDeleteStepSQL, sSQLName:=ACDirectDeleteStepName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Description: Deletes a Chase Cycle Rule record
    '
    ' *****************************************************************
    Public Function DirectDeleteRule(ByVal v_lChaseCycleRuleId As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add ChaseCycle_id as an input param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_Cycle_rule_id", vValue:=CStr(v_lChaseCycleRuleId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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



    Public Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CancelPolicy
    ' PURPOSE: Auto-Cancels a policy
    ' DATE: 01/03/2013
    ' RETURNS: PMTrue for success
    ' CHANGES: accept lapsed date as parameter and use
    ' ---------------------------------------------------------------------------
    Public Function CancelPolicy(ByVal lChaseCycleItemID As Object, ByVal dLapsedDate As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object
            Dim oAutoMTA As bSIRAutoMTA.Business


            oAutoMTA = New bSIRAutoMTA.Business
            m_lReturn = oAutoMTA.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Information for Cancellation
            With m_oDatabase
                .Parameters.Clear()

                .Parameters.Add("Chase_Cycle_item_id", CStr(lChaseCycleItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect("spu_SIR_Select_Chase_Cycle_AutoCancel", "Get Chase Cycle Auto Cancel details", True, , vResultArray)

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

            oAutoMTA.Dispose()
            oAutoMTA = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

            Return result
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
    '           Created :  01/03/2013 : Chase Cycle RetroFit
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

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            ' If you want to rollback a transaction or something, do it here

            Return result
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
    '           Created :  Date 01/03/2013
    ' ***************************************************************** '
    Public Function GetALLPMWrkTaskGroupPMUserGroups(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetALLPMWrkTaskGroupPMUserGroups"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetALLPMWrkTaskGroupPMUserGroupsSQL, sSQLName:=kGetALLPMWrkTaskGroupPMUserGroupsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kGetALLPMWrkTaskGroupPMUserGroupsSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            ' If you want to rollback a transaction or something, do it here
            Return result
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
    '           Created :  01/03/2013: Chase Cycle RetroFit
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

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            ' If you want to rollback a transaction or something, do it here

            Return result
        End Try

        ' destroy object instance
        oLookup = Nothing

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
    '           Created :  01/03/2013 Chase Cycle RetroFit
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

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            ' If you want to rollback a transaction or something, do it here
            Return result

        Finally
            ' destroy instance of object
            oTaskInstance = Nothing
        End Try

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
    '           Created: 01/03/2013
    ' ***************************************************************** '

    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object
        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName & _
                                       ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)


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
    '         Created :  01/03/2013
    ' ***************************************************************** '
    Private Function AddOutputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddOutputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName & _
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

        End If

        Return result


    End Function

    ' ***************************************************************** '
    ' Name: GetChaseCycleUDL
    '
    ' Parameters: n/a
    '
    ' Description: Returns Lookup Details for specified lookup tables
    '
    ' History:
    '           Created: 01/03/2013 : Chase Cycle RetroFit
    ' ***************************************************************** '
    Public Function GetChaseCycleUDL_old(ByVal v_lGISDataModel As Integer, ByRef r_sChaseCycleUDL As String, ByRef r_vUDLArray(,) As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="v_lGISDataModel", vValue:=CInt(v_lGISDataModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="r_sChaseCycleUDL", vValue:=CStr(r_sChaseCycleUDL), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAllUDLItemsSQL, sSQLName:=kGetAllUDLItemsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vUDLArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Chase Cycle Item records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetChaseCycleUDL
    '
    ' Parameters: v_lGISProperty,r_vPropertyArray
    '
    ' Description:It will get all Chase Cycle Property for selected  Gis property
    ' 
    '  Created :  01/03/2013
    ' ***************************************************************** '
    Public Function GetChaseCycleUDL(ByVal v_lGISProperty As Integer, ByRef r_vPropertyArray(,) As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_property_id", vValue:=CInt(v_lGISProperty), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetChaseCycleUDLSQL, sSQLName:=ACGetChaseCycleUDLName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vPropertyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Chase Cycle Property records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetChaseCycleUDLDesc
    '
    ' Parameters: v_chaseCycleRuleID,r_vUDLArrayDescription
    '
    ' Description:It will get all Chase Cycle Status description for selected  chase cycle Rule
    ' 
    '  Created :  01/03/2013
    ' ***************************************************************** '

    Public Function GetChaseCycleUDLDesc(ByVal v_chaseCycleRuleID As Integer, ByRef r_vUDLArrayDescription(,) As Object) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Chase_cycle_rule_id", vValue:=CInt(v_chaseCycleRuleID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetChaseCycleUDLStatusDesc, sSQLName:=kGetChaseCycleUDLStatusDescName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vUDLArrayDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Chase Cycle Rule records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetChaseCycleProperties
    '
    ' Parameters: v_lGISDataModel,r_vUDLArray
    '
    ' Description:It will get all Chase Cycle Properties for selected data model
    ' 
    '  Created :  01/03/2013
    ' ***************************************************************** '
    Public Function GetChaseCycleProperties(ByVal v_lGISDataModel As Integer, ByRef r_vUDLArray(,) As Object) As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CInt(v_lGISDataModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetChaseCyclePropertiesSQL, sSQLName:=kGetChaseCycleUDLStatusDescName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vUDLArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all Chase Cycle Item records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function
End Class
