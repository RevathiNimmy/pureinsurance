Option Strict Off
Option Explicit On
Imports SharedFiles
'UPGRADE_NOTE: Interface was upgraded to Interface_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    Private Const ACClass As String = "Interface"

    Private m_iTask As Short
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date
    Private m_sCallingAppName As String
    Private m_lStatus As Integer

    Private m_bAddTOC As Boolean

    Private m_sClient As String
    Private m_sServer As String

    Private m_oWord As Microsoft.Office.Interop.Word.Application

    Private m_bCreatedWord As Boolean

    Private m_lReturn As Integer

    Private m_vRiskArray As Object

    Private m_vMailshotArray As Object

    Private m_lMailshotDocumentTemplateId As Integer
    Private m_lMailshotDocumentTypeId As Integer

    Private m_proProgress As AxComctlLib.AxProgressBar
    Private frmInterface As New iPMUDocConvert_frm

    ' Public Methods
    Public Function Initialise() As Integer

        Dim sMessage As String
        Dim sTitle As String

        Try

            Initialise = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager

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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Exit Function
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
            m_sTransactionType = PMTransactionTypeGeneric
            m_dtEffectiveDate = Now

            ' Get an instance of the business object via
            ' the public object manager.
            m_lReturn = g_oObjectManager.GetInstance(oObject:=g_oBusiness, sClassName:="bSIRFieldManager.Business", vInstanceManager:="ClientManager")

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get an instance of the business object.
                Initialise = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                'UPGRADE_WARNING: Couldn't resolve default property of object GetResData(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sTitle = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                'UPGRADE_WARNING: Couldn't resolve default property of object GetResData(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

                ' Display message.
                MsgBox(sMessage, MsgBoxStyle.Critical, sTitle)

                Exit Function
            End If

            g_oLog = New CreateLog


            Exit Function


        Catch ex As Exception

            Initialise = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
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

        Try

            Start = gPMConstants.PMEReturnCode.PMTrue

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

            ' Assign the details from the search data storage
            ' to the interface.
            m_lReturn = frmInterface.DataToInterface()

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

        ' Load the instance of the interface into memory.
        'UPGRADE_ISSUE: Load statement is not supported. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="B530EFF2-3132-48F8-B8BC-D88AF543D321"'
        frmInterface.Show()

        ' Check if we have had an error so far.
        If (frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse) Then
            ' We have already encountered an error,
            ' so we MUST return the error.
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
