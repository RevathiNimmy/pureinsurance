Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed
    Implements IDisposable

    Public frmFrequencyScheduler As frmFrequencyScheduler

    ' Constant for the functions to identify
    ' which class this is.

    Private m_sCallingAppName As String = ""
    Private Const ACClass As String = "frmFrequencyScheduler"

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_lReturn As Integer
    Private m_lPMAuthorityLevel As Integer
    ' Stores the exit status of the interface.
    Private m_lStatus As Integer
    Private m_dtStartDate As Date
    Private m_iOccurances As Integer
    Private m_iBatchProcessId As Integer
    Private m_jobCode As String
    Private m_jobDescription As String
    Private m_jobType As String
    Private m_iStatus As Integer
    Private m_userName As String
    Private m_sbatchStatus As String
    Private m_oTaskScheduler As Object
    Private m_sProcess As String
    Private m_sProcessDescription As String
    Dim m_dtParameters As DataTable = Nothing
    Private m_ibatchSchedulerId As Integer

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property
    Public Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
        Set(value As Integer)
            m_iTask = value

        End Set
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property
    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property
    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property
    Public Property StartDate() As Date
        Get
            Return m_dtStartDate

        End Get
        Set(ByVal Value As Date)

            m_dtStartDate = Value

        End Set
    End Property

    Public Property Occurances() As Integer
        Get
            Return m_iOccurances

        End Get
        Set(ByVal Value As Integer)

            m_iOccurances = Value

        End Set
    End Property
    Public Property BatchProcessId() As Integer
        Get
            Return m_iBatchProcessId
        End Get
        Set(ByVal Value As Integer)
            m_iBatchProcessId = Value
        End Set
    End Property

    Public Property JobCode() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobCode
        End Get
        Set(value As String)
            m_jobCode = value
        End Set
    End Property
    Public Property JobDescription() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobDescription
        End Get
        Set(value As String)
            m_jobDescription = value
        End Set
    End Property
    Public Property JobType() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobType
        End Get
        Set(value As String)
            m_jobType = value
        End Set
    End Property
    Public Property JobStatus() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchStatus
        End Get
        Set(value As String)
            m_sbatchStatus = value
        End Set
    End Property
    Public Property UserName() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_userName
        End Get
        Set(value As String)
            m_userName = value
        End Set
    End Property
    Public Property Process() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sProcess
        End Get
        Set(value As String)
            m_sProcess = value
        End Set
    End Property
    Public Property ProcessDescription() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sProcessDescription
        End Get
        Set(value As String)
            m_sProcessDescription = value
        End Set
    End Property

    Public Property BatchFileName() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchFileName
        End Get
        Set(value As String)
            m_sbatchFileName = value
        End Set
    End Property
    Public Property BatchFileContentDetails() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchContentDetails
        End Get
        Set(value As String)
            m_sbatchContentDetails = value
        End Set
    End Property
    Public Property BatchProcessName() As String
        Get
            Return m_sBatchProcessName
        End Get
        Set(ByVal Value As String)
            m_sBatchProcessName = Value
        End Set
    End Property
    Public Property ProcessParameters() As DataTable
        Get
            Return m_dtParameters
        End Get
        Set(ByVal Value As DataTable)
            m_dtParameters = Value
        End Set
    End Property
    Public Property BatchSchedulerId() As Integer
        Get
            Return m_ibatchSchedulerId
        End Get
        Set(ByVal Value As Integer)
            m_ibatchSchedulerId = Value
        End Set
    End Property
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private m_sbatchFileName As String
    Private m_sbatchContentDetails As String
    Private m_sBatchProcessName As String
    Dim dtParameters As DataTable = Nothing
    Public g_oObjectManager As bObjectManager.ObjectManager
    Public g_oBusiness As Object
    Public g_oTaskSchedulerBusiness As Object
    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sError As String = ""
        Const kMethodName As String = "Initialise"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sError = "Failed to initialise object manager"
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID

            End With

            ' Initialise the process modes.
            'm_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            BatchProcessId = m_iBatchProcessId
            Process = m_sProcess
            ProcessDescription = m_sProcessDescription
            BatchProcessName = m_sBatchProcessName
            ProcessParameters = m_dtParameters
            BatchSchedulerId = m_ibatchSchedulerId
            m_jobCode = JobCode
            m_sbatchStatus = JobStatus
            m_jobDescription = JobDescription
            m_jobType = JobType
            m_userName = UserName
            Task = m_iTask
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(
                oObject:=m_oTaskScheduler,
                sClassName:="bSIRTaskScheduler.Business",
                vInstanceManager:=PMGetViaClientManager)
            '    m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTaskScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)


            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=ex)

        Finally

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                If result = gPMConstants.PMEReturnCode.PMFalse Then
                    gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sError, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                End If

                If Not (g_oTaskSchedulerBusiness Is Nothing) Then

                    'm_lReturn = g_oBusiness.Terminate()
                    g_oTaskSchedulerBusiness.Dispose()
                    g_oTaskSchedulerBusiness = Nothing
                End If

                If Not (g_oObjectManager Is Nothing) Then
                    'm_lReturn = g_oObjectManager.Terminate()
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End Try
        Return result
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oTaskSchedulerBusiness IsNot Nothing Then
                    g_oTaskSchedulerBusiness.Dispose()
                    g_oTaskSchedulerBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = LoadInterface()
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'Developer Guide No. 50
        frmFrequencyScheduler = New frmFrequencyScheduler
        ' Assign the parameters to the interface properties.
        With frmFrequencyScheduler
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .Business = m_oTaskScheduler
            .UserName = m_userName
            'Developer Guide No. 24
            '.set_Parameters(m_vParameters)
            '.Parameters = m_vParameters

            ''31/10/2002 - PWC - Added
            '.FilterName = m_sFilterName
            '.FilterValue = m_sFilterValue
            '.SaveParams = m_bSaveParams
            ''Developer Guide No.
            ''.set_KeyPrompts(m_vKeyPrompts)
            '.KeyPrompts = VB6.CopyArray(m_vKeyPrompts)
            '8.5

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.
        ' Dim tempLoadForm As frmInterface = frmInterface
        ' tempLoadForm.Show()
        ' Check if we have had an error so far.
        ' If frmFrequencyScheduler.ErrorNumber <> 0 Then
        ' We have already encountered an error,
        ' so we MUST return the error.
        'result = frmFrequencyScheduler.ErrorNumber
        'End If

        Return result

    End Function

    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue
        frmFrequencyScheduler = New frmFrequencyScheduler
        'assign renewal mode and filters values to form
        ' m_ofrmRenewalProcess.RenewalMode = m_lRenewalMode
        '  m_lReturn = frmFrequencyScheduler.Initialise()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmFrequencyScheduler = Nothing
            Return result
        End If

        With frmFrequencyScheduler
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            .BatchProcessId = m_iBatchProcessId

            .JobCode = m_jobCode
            .JobStatus = m_sbatchStatus
            .JobDescription = m_jobDescription
            .JobType = m_jobType
            .UserName = m_userName
            .Process = m_sProcess
            .ProcessDescription = m_sProcessDescription
            .ProcessParameters = m_dtParameters
            .BatchProcessName = m_sBatchProcessName
            .BatchFileName = m_sbatchFileName
            .BatchFileContentDetails = m_sbatchContentDetails
            .Business = m_oTaskScheduler
            .BatchSchedulerId = m_ibatchSchedulerId
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        'load renewal main form


        ' Call the ShowForm method to show the form, allow user input etc.

        ' If m_ofrmFrequencyScheduler.Status = gPMConstants.PMEReturnCode.PMTrue Then
        frmFrequencyScheduler.ShowDialog()
        'Else
        'result = gPMConstants.PMEReturnCode.PMFalse
        'End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmFrequencyScheduler = Nothing
            Return result
        End If

        Return result

    End Function
    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
End Class
