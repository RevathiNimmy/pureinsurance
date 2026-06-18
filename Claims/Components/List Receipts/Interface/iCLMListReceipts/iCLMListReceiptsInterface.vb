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
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Description: Main public class to accompany the interface form.
    ' ***************************************************************** '
    'developer guide no.50
    'Dim frmInterface As frmInterface

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)



    Private m_lClaimId As Integer
    Private m_lRecoveryID As Integer
    Private m_sRecoveryText As String = ""
    Private m_vRecoveryType As Integer
    Private m_lClaimPerilId As Integer

    Private m_lWindowHWND As Integer


    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    'Added to the Existing Address-Pandu
    Public Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the task.
            m_iTask = Value

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

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property ClaimId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property
    Public WriteOnly Property RecoveryID() As Integer
        Set(ByVal Value As Integer)
            m_lRecoveryID = Value
        End Set
    End Property

    Public WriteOnly Property RecoveryText() As String
        Set(ByVal Value As String)
            m_sRecoveryText = Value
        End Set
    End Property
    Public WriteOnly Property RecoveryType() As Integer
        Set(ByVal Value As Integer)
            m_vRecoveryType = Value
        End Set
    End Property
    Public WriteOnly Property ClaimPerilId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimPerilId = Value
        End Set
    End Property


    Public Property WindowHWND() As Integer
        Get
            Return m_lWindowHWND
        End Get
        Set(ByVal Value As Integer)
            m_lWindowHWND = Value
        End Set
    End Property



    ' {* USER DEFINED CODE (End) *}
    ' ***************************************************************** '
    ' Name: InitilizeReceiptItem
    '
    ' Parameters: n/a
    '
    ' Description:To call public function ActionViewReceipt
    ' Receipt button on uctCLMListReceipts will call it to execute
    ' ActionViewReceipt in frmInteface.


    ' ***************************************************************** '

    Public Function InitilizeReceiptItem() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "InitilizeReceiptItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_ofrmInterface.ActionViewReceipt()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "frmInterface.ActionviewReceipt failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    ' PUBLIC Property Procedures (End)
    ' PUBLIC Methods (Begin)


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sHelpFile As String = ""
        Dim m_lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sOptionValue As String = ""
        Const kMethodName As String = "Initialise"

        Try
            result = gPMConstants.PMEReturnCode.PMTrue



            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise object manager", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetProcessModes"
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


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetStatus"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Start"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed To ProcessInterface", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterface"
        result = gPMConstants.PMEReturnCode.PMTrue


        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to LoadInterface", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to ShowInterface", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to UnLoadInterface", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadInterface"

        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.50
        m_ofrmInterface = New frmInterface

        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            .ClaimId = m_lClaimId
            .RecoveryID = m_lRecoveryID
            .RecoveryText = m_sRecoveryText

            'developer guide no.24
            .RecoveryType = m_vRecoveryType
            .ClaimPerilId = m_lClaimPerilId
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = m_ofrmInterface

        ' Check if we have had an error so far.
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_ofrmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        m_lReturn = CType(m_ofrmInterface.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the status.
            gPMFunctions.RaiseError(kMethodName, "Method frmInterface.SetStatus Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result


    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnLoadInterface"
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmInterface
            m_lStatus = .Status
            m_sStepStatus.Value = .StepStatus

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_ofrmInterface.Close()
        m_ofrmInterface = Nothing


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ShowInterface"
        result = gPMConstants.PMEReturnCode.PMTrue


        WindowHWND = m_ofrmInterface.Handle.ToInt32()

        'Since this control can also be invoked via claim builder
        'so to keep this form in front of all other claim builder
        'screens we are calling the below function

        KeepWindowOnTop(True)


        ' Display the interface.
        VB6.ShowForm(m_ofrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_ofrmInterface.ErrorNumber <> 0 Then
                result = m_ofrmInterface.ErrorNumber
            End If
        End If


        Return result

    End Function
    'PRIVATE Methods (End)


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function KeepWindowOnTop(ByVal bKeepOnTop As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "KeepWindowOnTop"
        Try


            m_lReturn = CType(iPMFunc.SetWindowPlacement(WindowHWND, bKeepOnTop), gPMConstants.PMEReturnCode)


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return m_lReturn
    End Function
End Class