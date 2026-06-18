Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class MIDInterface
    Implements IDisposable

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MIDInterface"

    Public nStartMode As Integer

#Region "Private Variables"

    Private m_sCallingAppName As String = ""
    Private m_nStatus As Integer
    Private m_nTask As Integer
    Private m_nNavigate As Integer
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_nPMAuthorityLevel As Integer
    Private m_oFrmInterface As Object
    Private m_nStartupMode As Integer
    Private m_bDisposedValue As Boolean
    Private m_sProcessStatus As String

#End Region

#Region "Public Properties"

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_nPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_nPMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_nStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            ' Standard Property.
            ' Return the task.
            Return m_nTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.
            ' Return the navigate flag.
            Return m_nNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.
            ' Return the process mode.
            Return m_nProcessMode
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

    Public Property StartupMode() As Integer
        Get
            Return m_nStartupMode
        End Get
        Set(ByVal Value As Integer)
            m_nStartupMode = Value
        End Set
    End Property

#End Region

#Region "Public Functions"

    ''' <summary>
    ''' Creats class object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Entry point for any initialisation code for this object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise() As Integer

        Dim nResult As PMEReturnCode
        Dim sHelpFile As String = String.Empty
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            nResult = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            ' Store the data from the object manager
            ' for us to use them throughout the object.
            With g_oObjectManager
                g_nLanguageID = .LanguageID
                g_nSourceID = .SourceID
                g_sUserName = .UserName
            End With

            ' Initialise the process modes.
            m_nTask = gPMConstants.PMEComponentAction.PMEdit
            m_nNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_nProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            nResult = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return nResult
            End If
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read system option for Cheque Production assuming Not Installed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=New Exception(Information.Err().Description))
            End If

        Catch Excep As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", Excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Stores all of the parameter members with the key array
    ''' </summary>
    ''' <param name="r_aoKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetKeys(ByRef r_aoKeyArray(,) As Object) As Integer

        Dim nResult As PMEReturnCode
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' Check we have a vaild array.
            If Not Information.IsArray(r_aoKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, Excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Stores all of the key array with the parameter members
    ''' </summary>
    ''' <param name="r_aoKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetKeys(ByRef r_aoKeyArray(,) As Object) As Integer

        Dim nResult As PMEReturnCode
        Try
            nResult = PMEReturnCode.PMTrue

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, Excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Stores all of the summary array with the parameter members
    ''' </summary>
    ''' <param name="r_oSummaryArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSummary(ByRef r_oSummaryArray As Object) As Integer

        Dim nResult As PMEReturnCode
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

        Catch Excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Set the optional process modes.
    ''' </summary>
    ''' <param name="r_oTask"></param>
    ''' <param name="r_oNavigate"></param>
    ''' <param name="r_oProcessMode"></param>
    ''' <param name="r_oTransactionType"></param>
    ''' <param name="r_oEffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProcessModes(Optional ByRef r_oTask As Object = Nothing, Optional ByRef r_oNavigate As Object = Nothing, _
                                    Optional ByRef r_oProcessMode As Object = Nothing, Optional ByRef r_oTransactionType As Object = Nothing, _
                                    Optional ByRef r_oEffectiveDate As Object = Nothing) As Integer
        Dim nResult As PMEReturnCode
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.
            If Not Information.IsNothing(r_oTask) Then
                m_nTask = CInt(r_oTask)
            End If

            If Not Information.IsNothing(r_oNavigate) Then
                m_nNavigate = CInt(r_oNavigate)
            End If

            If Not Information.IsNothing(r_oProcessMode) Then
                m_nProcessMode = CInt(r_oProcessMode)
            End If

            If Not Information.IsNothing(r_oTransactionType) Then
                m_sTransactionType = CStr(r_oTransactionType)
            End If

            If Not Information.IsNothing(r_oEffectiveDate) Then
                m_dtEffectiveDate = CDate(r_oEffectiveDate)
            End If

        Catch Excep As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Entry point for the object to start its processing.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Start() As Integer

        Dim nResult As PMEReturnCode
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            nResult = CType(ProcessInterface(), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch Excep As Exception
            ' Error Section.
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, Excep:=Excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Entry point for any termination code for this object
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "Private/ Protected Functions"

    ''' <summary>
    ''' Calls the appropriate methods to process the interface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessInterface() As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        nResult = CType(LoadInterface(), gPMConstants.PMEReturnCode)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        nResult = ShowInterface()
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        nResult = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function

    ''' <summary>
    ''' Loads the instance of the interface into memory and 
    ''' passes the parameters in.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function LoadInterface() As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        'Some kind of test required to determine which form to start with
        If Me.nStartMode = 0 Then
            m_oFrmInterface = New frmMIDRules
        Else
            m_oFrmInterface = New frmMIDRuleConfiguration
        End If

        ' Assign the parameters to the interface properties.
        With m_oFrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_nTask
            .Navigate = m_nNavigate
            .ProcessMode = m_nProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
        End With

        ' Load the instance of the interface into memory.
        ' Check if we have had an error so far.
        If m_oFrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            nResult = m_oFrmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        nResult = CType(m_oFrmInterface.SetStatus(sProcessStatus:=m_sProcessStatus), gPMConstants.PMEReturnCode)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the status.
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function

    ''' <summary>
    ''' Displays the instance of the interface using the display state.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ShowInterface() As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        'Display the interface.
        Dim frm As Form = New Form()
        frm = CType(m_oFrmInterface, Form)
        frm.WindowState = FormWindowState.Normal
        frm.ShowDialog()

        If frm.Modal Then
            ' Check for any form errors.
            If m_oFrmInterface.ErrorNumber <> 0 Then
                nResult = m_oFrmInterface.ErrorNumber
            End If
        End If

        Return nResult

    End Function

    ''' <summary>
    ''' Unloads the instance of the interface from memory.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UnLoadInterface() As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_oFrmInterface
            m_nStatus = .Status
        End With

        ' Unload and destroy the instance of the interface from memory.
        m_oFrmInterface.Close()
        m_oFrmInterface = Nothing

        Return nResult
    End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' Terminates the object references
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Sub Dispose(disposing As Boolean)
        If Not Me.m_bDisposedValue Then
            Me.m_bDisposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()

                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.m_bDisposedValue = True
    End Sub

#End Region

End Class

