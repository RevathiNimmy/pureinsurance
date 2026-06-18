Option Strict Off
Option Explicit On
Imports SharedFiles
'UPGRADE_NOTE: Interface was upgraded to Interface_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
Friend NotInheritable Class Interface_Renamed
    Implements IDisposable

    Private Const ACClass As String = "Interface"

    Private m_iTask As Short
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date
    Private m_sCallingAppName As String
    Private m_lStatus As Integer

    Private m_lReturn As Integer


    ' Public Methods
    Public Function Initialise() As Integer

        Dim sMessage As String
        Dim sTitle As String
        Dim result As Integer
        result = gPMConstants.PMEReturnCode.PMTrue
        'On Error GoTo Err_Initialise
        Try
            Initialise = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to call the initialise method.
                Initialise = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                'UPGRADE_NOTE: Object g_oObjectManager may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                g_oObjectManager = Nothing

                ' Log Error.
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Exit Function
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUserName = .UserName
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = PMTransactionTypeGeneric
            m_dtEffectiveDate = Now

            '    Set g_oDataBase = CreateObject("dPMDAO.Database")
            '
            '    m_lReturn = g_oDataBase.OpenDatabase(g_sUserName, g_iSourceID, g_iLanguageID, ACApp)
            '
            '    If m_lReturn <> PMTrue Then
            '        GoTo Err_Initialise
            '    End If

            ' Get an instance of the business object via
            ' the public object manager.
            m_lReturn = g_oObjectManager.GetInstance(oObject:=g_oBusiness, sClassName:="bSIRFieldManager.Business", vInstanceManager:="ClientManager")
            Exit Function
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
            'Err_Initialise:

            '            Initialise = gPMConstants.PMEReturnCode.PMError

            '            ' Log Error.
            '            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description)

            '            Exit Function
        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If g_oDataBase IsNot Nothing Then
                    g_oDataBase.Dispose()
                    g_oDataBase = Nothing
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

        Dim sClient As String
        Dim sClientFile As String
        Dim sValue As String

        Try

            Start = gPMConstants.PMEReturnCode.PMTrue

            sValue = ReadRegistry(gpmConstants.HKEY_LOCAL_MACHINE, "Software\PM\SiriusSolutions\Setup\", "DocumentsConvertedToXML")

            If UCase(sValue) = "NOT FOUND" Then
                WriteRegistry(gpmConstants.HKEY_LOCAL_MACHINE, "Software\PM\SiriusSolutions\Setup\", "DocumentsConvertedToXML", Registry.InTypes.ValString, "0")
            End If
            'Needs to be deleted
            sValue = 0
            If sValue = "1" Then
                Exit Function
            End If

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to process the interface.
                Start = gPMConstants.PMEReturnCode.PMFalse
            End If

            Exit Function

        Catch ex As Exception

            Start = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' End of Public Methods
    ' Private Methods


    ' End of Private Methods

    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Initialize_Renamed()

        ' Class Initialise Event.


        Exit Sub


    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Try

            GetInterfaceDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Get the interface details from the business object.
            m_lReturn = frmInterface.GetBusiness()

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to assign the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            ' Error Section.

            GetInterfaceDetails = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer


        ProcessInterface = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            ' Failed to load the interface.
            ProcessInterface = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = frmInterface.GetInterfaceDetails()
        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            ' Failed to load the interface.
            ProcessInterface = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        frmInterface.Visible = False
        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=VB6.FormShowConstants.Modal)

        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            ' Failed to display the inteface.
            ProcessInterface = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

        ' Check for errors.
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            ' Failed to unload the interface.
            ProcessInterface = gPMConstants.PMEReturnCode.PMFalse
        End If

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer


        LoadInterface = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With
        frmInterface.Show()
        ' Check if we have had an error so far.
        If (frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse) Then

            LoadInterface = frmInterface.ErrorNumber
        End If
        Exit Function
    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer


        UnLoadInterface = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With frmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}


            ' {* USER DEFINED CODE (End) *}

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        'UPGRADE_NOTE: Object frmInterface may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        frmInterface = Nothing

        Exit Function


    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer


        ShowInterface = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(frmInterface, lDisplayState)

        If (lDisplayState = VB6.FormShowConstants.Modal) Then
            ' Check for any form errors.
            If (frmInterface.ErrorNumber <> 0) Then
                ShowInterface = frmInterface.ErrorNumber
            End If
        End If

        Exit Function


    End Function
End Class
