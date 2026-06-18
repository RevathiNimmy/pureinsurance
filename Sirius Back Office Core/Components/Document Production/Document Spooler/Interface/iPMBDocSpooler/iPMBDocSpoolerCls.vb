Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    'developer guide no.50
    Public m_ofrminterface As iPMBDocSpooler.frmInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 21/01/1998
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_lPMAuthorityLevel As Integer

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}
    ' Debug flag
    Private m_bDebugOn As Boolean
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sInsuranceRef As String = ""
    Private m_lClaimCnt As Integer
    'DC240603 -ISS4097 -added new parameter
    Private m_lSourceId As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
        End Set
    End Property

    ' The Calling application or component name.
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    'End Property

    ' ***************************************************************** '
    ' The following properties are set by Navigator via the             '
    ' SetProcessModes method and tell the component what                '
    ' mode of operation it is in.                                       '
    ' ***************************************************************** '

    ' The Task that the form is to perform
    ' i.e. Add, Edit, View, Delete
    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    ' The status of the Navigator button on the form.
    ' i.e. Not Required, Enabled, Disabled
    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    ' The type of process that is being performed
    ' i.e. Generic, Enquiry, Quotation, Make Live
    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    ' The type of transaction that is being performed
    ' i.e. Quotation, New Business, Renewal, MTA
    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property

    ' The effective date that we are working to
    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    ' ***************************************************************** '
    ' The following properties need to be set by the                    '
    ' component so that Navigator can tell what happened.               '
    ' ***************************************************************** '

    ' The user status of the form on exit
    ' i.e. PMOK, PMCancel or PMNavigate.
    ' Business objects will normally just set this to PMOK,
    ' unless they want the ability to adjust the route through a process.
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    ' The Completion Status of the Step
    ' i.e.Complete , Incomplete, Inactive
    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus.Value
        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property DebugOn() As Boolean
        Set(ByVal Value As Boolean)
            m_bDebugOn = Value
        End Set
    End Property
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer


        '

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)

            ' Check Return Value. We're probably logging on so take this into account
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Continue
                Case gPMConstants.PMEReturnCode.PMCancel
                    ' User has cancelled so exit without message
                    result = gPMConstants.PMEReturnCode.PMCancel
                    MainModule.g_oObjectManager = Nothing
                    Return result
                Case Else
                    ' Some sort of error so display message
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MainModule.g_oObjectManager = Nothing
                    ' Log Error.
                    gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogError, "Failed to initialise the object manager", MainModule.ACApp, ACClass, "Initialise")
                    Return result
            End Select

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With MainModule.g_oObjectManager
                MainModule.g_iLanguageID = .LanguageID
                MainModule.g_iSourceID = .SourceID
                MainModule.g_sUserName = .UserName
                MainModule.g_iUserId = .UserID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            'SD 01/08/2002 Scalability changes
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise the object", MainModule.ACApp, ACClass, "Initialise", excep:=excep)

            Return result

        End Try
    End Function

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
            Me.disposedValue = True
            If disposing Then
                If MainModule.g_oObjectManager IsNot Nothing Then
                    MainModule.g_oObjectManager.Dispose()
                    MainModule.g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(WrapperProcess(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to start the object", MainModule.ACApp, ACClass, "Start", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' The following methods are called by Navigator BEFORE              '
    ' the component is told to Start its job.                           '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: SetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The array will contain the key values required by the
    '              component to do its job.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameShortName

                        m_sShortName = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsuranceFolderCnt

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyNo

                        m_sInsuranceRef = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetKeys Failed", MainModule.ACApp, ACClass, "SetKeys", Information.Err().Number, excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Navigator Standard Method)
    '
    ' Description: Sets the mode of operation for the Component.
    '              The properties are described individually above.
    '
    ' ***************************************************************** '
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
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetProcessModes Failed", MainModule.ACApp, ACClass, "SetProcessModes", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Navigator Standard Method)
    '
    ' Description: Set the Process, Map and Step Completion Status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "SetStatus Failed", MainModule.ACApp, ACClass, "SetStatus", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' The following methods are called by Navigator AFTER               '
    ' the component has done its job.                                   '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: GetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The component populates the array with
    '              key values. i.e. If the component is
    '              FindParty it will return the PartyCnt of the Party
    '              selected by the user.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 2)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameInsuranceFileCnt
            '    vKeyArray(PMKeyValue, 0) = m_lInsuranceFileCnt
            '    vKeyArray(PMKeyName, 1) = PMKeyNameInsuranceFolderCnt
            '    vKeyArray(PMKeyValue, 1) = m_lInsuranceFolderCnt
            '    vKeyArray(PMKeyName, 2) = PMKeyNamePolicyNo
            '    vKeyArray(PMKeyValue, 2) = m_sInsuranceRef

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "GetKeys Failed", MainModule.ACApp, ACClass, "GetKeys", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: WrapperProcess
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function WrapperProcess() As Integer

        ' RDC 22/09/2005
        Dim result As Integer = 0
        Dim sAutoArchive As String = ""
        Const AUTO_ARCHIVE_ENABLED As Integer = 5008



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' RDC 22/09/2005
        ' Get system option auto-archive
        m_lReturn = CType(iPMFunc.GetSystemOption(AUTO_ARCHIVE_ENABLED, sAutoArchive, MainModule.g_iSourceID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_ofrminterface = New frmInterface

        'developer guide no.50
        With m_ofrminterface
            .PartyCnt = m_lPartyCnt
            .ShortName = m_sShortName
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .InsuranceRef = m_sInsuranceRef
            .ClaimCnt = m_lClaimCnt
            ' RDC 22/09/2005
            .AutoArchiveEnabled = (sAutoArchive = "1")

            m_lReturn = CType(.Initialise(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .LoadInterface()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            .ShowDialog()

            m_lStatus = .Status

            .Dispose()
        End With
        'developer guide no.50
        m_ofrminterface.Close()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format Summary Level, Summary
    '              Heading, Summary Value.
    '
    '              The component populates the array with any
    '              summary information it wants to return to Navigator.
    '
    '              There are three levels of Summary, Process,
    '              Map Instance and Map.
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "GetSummary Failed", MainModule.ACApp, ACClass, "GetSummary", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
