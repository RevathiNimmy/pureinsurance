Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 02 Sep 2003
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to get a claim
    '              letter
    '
    ' Edit History: AMB - created for ICB Deferred RI development
    '               This is a cloned version of the Get Claim Letter component
    '
    ' SJP14062002 - getUnderWritingOrAgency changed to use product option scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sUnderwritingOrAgency As String = ""
    Private m_oSystemOption As Object

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
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


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Sub New()
        MyBase.New()


        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub




    ' ***************************************************************** '
    '
    ' Name: GetClaimRiskStatus
    '
    ' Description:
    '
    ' History: 17/04/01 DC - Created.
    '
    ' ***************************************************************** '
    Public Function GetClaimRiskStatus(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Number parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimRiskStatus")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimRiskStatusSQL, sSQLName:=ACGetClaimRiskStatus, bStoredProcedure:=ACGetClaimRiskStatusStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimRiskStatus")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name : AddTaskToWorkManager
    '
    ' Desc : Add task to work manager
    '
    ' Hist : AMB 03/09/2003: 1.8.6 Deferred Reinsurance development
    ' ***************************************************************** '
    Public Function AddTaskToWorkManager(ByVal v_sClientName As String, ByVal v_sDescription As String, ByVal v_dtDueDate As Date) As Integer

        Dim result As Integer = 0
        Dim bCloseDatabase As Boolean
        Dim oDatabase As Object = Nothing
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lPMWrkTaskID, lPMWrkTaskGroupID, lPMUserGroupID, lPMWrkTaskInstanceCnt As Integer

        Const PMTaskClaimPayment As String = "PAYCLM"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'open architecture database

            If gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase, v_vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            If oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get claim task group from system option
            m_lReturn = CType(bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, CInt("1019"), r_sOptionValue:=CStr(gPMFunctions.NullToLong(lPMWrkTaskGroupID))), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'get claim user group from system option
            m_lReturn = CType(bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, CInt("1020"), r_sOptionValue:=CStr(gPMFunctions.NullToLong(lPMUserGroupID))), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Get the task_id
            sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = {task_code}"


            If oDatabase.Parameters.Add("task_code", vValue:=PMTaskClaimPayment, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oDatabase.SQLSelect(sSQL:=ToSafeString(sSQL), sSQLName:="GetTaskID", bStoredProcedure:=False, vResultArray:=CType(vResultArray, Object)) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the task_id
            lPMWrkTaskID = gPMFunctions.NullToLong(vResultArray(0, 0))

            'create task

            If oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=lPMWrkTaskGroupID, v_lPMWrkTaskID:=lPMWrkTaskID, v_sCustomer:=v_sClientName, v_dtTaskDueDate:=v_dtDueDate, v_lPMUserGroupID:=lPMUserGroupID, v_sDescription:=v_sDescription, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt, v_iUserID:=m_iUserID) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'close database
            If bCloseDatabase = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oDatabase.CloseDatabase()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProductDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '

    'Private Function GetProductDetails(ByVal v_lClaimId As Integer, Optional ByRef r_lClaimTaskGroup As Integer = 0, Optional ByRef r_lClaimUserGroup As Integer = 0) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "GetProductDetails"
    '
    'Const kIClaimTaskGroup As Integer = 134
    'Const kIClaimUserGroup As Integer = 135
    '
    'Dim lReturn As Integer
    'Dim oBusiness As Object
    'Dim bCloseDatabase As Boolean
    'Dim oDatabase As Object
    'Dim lProductID As Integer
    'Dim iClaim_Task_Group, iClaim_User_Group As Integer
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'open architecture database

    'If gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=oDatabase, v_vDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'gPMFunctions.RaiseError(kMethodName, "CheckDatabase Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '

    'If gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness, v_sClassName:="bSIRProduct.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bSIRProduct.Business", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'get product details

    'm_lReturn = oBusiness.GetProductLevelOptionsForClaim(v_lClaimId:=v_lClaimId, r_iClaim_Task_Group:=iClaim_Task_Group, r_iClaim_User_Group:=iClaim_User_Group)
    '
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    'r_lClaimTaskGroup = iClaim_Task_Group
    'r_lClaimUserGroup = iClaim_User_Group
    '

    'lReturn = oBusiness.Terminate()
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    'Return result
    '
    'End Function
End Class
