Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable

    Dim frmInterface As frmInterface

    Private Const ACClass As String = "Interface"

    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sNavigatorTitle As String = ""

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

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

    Public ReadOnly Property Task() As Integer
        Get

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate

        End Get
    End Property

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            'DAK031299
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMCancel
                'g_oObjectManager = Nothing
                Return result

            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager to the public variables, 
            ' to enable us to use them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMEdit
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'Check if we can continue
            m_lReturn = CType(GetSysAdminStatus(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function GetPrivileges() As Integer
        Dim result As Integer = 0
        Dim oBusiness As bPMCurrency.Form
        Dim iPrivilegeLevel As gPMConstants.PMELookupEditPrivlegeLevel
        Dim bIsAdministrator As Boolean
        Dim vSupervisedGroups As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the Business object
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMCurrency.Form", vInstanceManager:="ClientManager")
            oBusiness = temp_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oBusiness.Dispose()
                oBusiness = Nothing

                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMCurrency.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges")

                Return result
            End If


            m_lReturn = oBusiness.GetPrivilegeLevel(r_iPrivilegeLevel:=iPrivilegeLevel)
            'DAK011299
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or (iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges And iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions And iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView And iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserNone) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                oBusiness.Dispose()
                oBusiness = Nothing

                MessageBox.Show("You do not have permission to access " &
                                "PM Currency Maintenance." &
                                Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() &
                                "Please contact your System Administrator.", Application.ProductName)

                '        LogMessagePopup _
                'iType:=PMLogInfo, _
                'sMsg:="No Access to PM Currency Maintenance", _
                'vApp:=ACApp, _
                'vClass:=ACClass, _
                'vMethod:="GetPrivileges"

                Return result
            End If

            If iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then

                oBusiness.Dispose()
                oBusiness = Nothing
                Return result
            End If


            m_lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)

            oBusiness.Dispose()
            oBusiness = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not bIsAdministrator Then
                result = gPMConstants.PMEReturnCode.PMFalse

                MessageBox.Show("You do not have permission to access " &
                                "PM Currency Maintenance." &
                                Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & Strings.Chr(13).ToString() &
                                "Please contact your System Administrator.", Application.ProductName)

                '        LogMessagePopup _
                'iType:=PMLogInfo, _
                'sMsg:="No Access to PM Currency Maintenance", _
                'vApp:=ACApp, _
                'vClass:=ACClass, _
                'vMethod:="GetPrivileges"

                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivileges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Stores all of the parameter members with the key array.
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Stores all of the key array with the parameter members.
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 0)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Stores all of the summary array with the parameter members
    ''' </summary>
    ''' <param name="vSummaryArray"></param>
    ''' <returns></returns>
    'developer guide no. 17
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vKeyArray(1, 0) As Object
            vSummaryArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Entry point for the object to start its processing.
    ''' </summary>
    ''' <returns></returns>
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Calls the appropriate methods to process interface.
    ''' </summary>
    ''' <returns></returns>
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ''' <summary>
    ''' Loads the instance of the interface into memory and passes the parameters in.
    ''' </summary>
    ''' <returns></returns>
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.50
        frmInterface = New frmInterface
        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

        End With

        ' Load the instance of the interface into memory.
        frmInterface.frmInterfaceLoad()

        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            result = frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ''' <summary>
    ''' Unloads the instance of the interface from memory.
    ''' </summary>
    ''' <returns></returns>
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With frmInterface
            m_lStatus = .Status

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing

        Return result

    End Function

    ''' <summary>
    ''' Displays the instance of the interface using the display state.
    ''' </summary>
    ''' <param name="lDisplayState"></param>
    ''' <returns></returns>
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function

    Public Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ''' <summary>
    ''' check if user is member of a SysAdmin user group.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetSysAdminStatus() As Integer
        Dim result As Integer = 0
        Dim lStatus As Integer

        Dim oBusiness As bPMCurrency.Form

        result = gPMConstants.PMEReturnCode.PMFalse

        'Get the Business object
        Dim temp_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMCurrency.Form", vInstanceManager:="ClientManager")
        oBusiness = temp_oBusiness

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oBusiness = Nothing

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMCurrency.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSysAdminStatus")

            Return result
        End If

        m_lReturn = oBusiness.GetSysAdminStatus(lStatus)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lStatus = 0 Then

            oBusiness = Nothing

            MessageBox.Show("You do not have permission to access " & _
                            "Currency Maintenance." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                            "Please contact your System Administrator.", Application.ProductName)

            Return result
        End If

        oBusiness = Nothing

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
End Class

