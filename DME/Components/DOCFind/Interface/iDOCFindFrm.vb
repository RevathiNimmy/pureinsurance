Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Imports System.IO

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: {17/2/98}
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    '***Insert Form Constants***

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_finddocExists As Boolean = False

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}


    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer


    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            '    m_lreturn& = m_oBusiness.GetDetails()

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            '    If (m_lreturn& <> PMTrue) Then
            '        ' Failed to get details.
            '        GetBusiness = PMFalse
            '
            '        ' Log Error.
            '        iPMFunc.LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetBusiness"
            '
            '        Exit Function
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim sDMEDir As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            txtDocName.MaxLength = DOCDoc_Name_Max
            cmdFindNow.Enabled = False
            txtStartFolder.Text = g_sStartFoldName

            'set default max returned values
            cmbMax.Items.Add(CStr(5))
            cmbMax.Items.Add(CStr(10))
            cmbMax.Items.Add(CStr(15))
            cmbMax.Items.Add(CStr(20))
            cmbMax.Items.Add(CStr(30))
            cmbMax.Items.Add(CStr(40))
            cmbMax.Items.Add(CStr(50))
            cmbMax.Text = "5"


            'get DME install dir and load avi
            m_lReturn = GetDMEDIR(sDMEDir)
            Dim file As FileInfo = New FileInfo(sDMEDir & DOCAVIDir & "\finddoc.avi")
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If file.Exists Then
                    '  aniFind.Open(sDMEDir & DOCAVIDir & "\finddoc.avi")
                    m_finddocExists = True
                End If

            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim lMaxExceeded As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'so what did the user do.

            Select Case Me.Status
                Case gPMConstants.PMEReturnCode.PMCancel

                    'user cancelled
                    g_vDocsFound = Nothing

                Case gPMConstants.PMEReturnCode.PMOK

                    'right, lets go to work.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    If m_finddocExists Then
                        ' aniFind.Play()
                    End If


                    'find doc using the business object.
                    'UPGRADE_TODO: (1067) Member FindDocument is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    m_lReturn = m_oBusiness.FindDocument(sDocName:=txtDocName.Text.Trim(), lMaxReturned:=CInt(cmbMax.Text), vDocArray:=g_vDocsFound, lStartFoldNum:=g_lStartFoldNum, iAccessLevel:=g_iDOCaccesslevel, lMaxExceeded:=lMaxExceeded)

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to add the details
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed in Find Document", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")

                    End If

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

                    ' aniFind.Stop()

                    If lMaxExceeded > 0 Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("You cannot find more than " & lMaxExceeded & _
                                        " documents.", "Find Document")
                        cmbMax.Focus()
                        Return result
                    End If

                    If Not Information.IsArray(g_vDocsFound) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("No Documents Found.", "Find Document")
                        txtDocName.Focus()
                        Return result
                    End If

            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try


            ' Display all language specific captions.

            '    Me.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACInterfaceTitle, _
            ''        iDataType:=PMResString)
            '
            '    ' Check for an error.
            '    If (Me.Caption = "") Then
            '        ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse
            '
            '        ' Log Error.
            '        iPMFunc.LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            ''            "Please check the file exists and the correct captions are available", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="DisplayCaptions"
            '
            '        Exit Function
            '    End If

            '    cmdFindNow.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACOKButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdCancel.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACCancelButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdHelp.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACHelpButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdNavigate.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACNavigateButton, _
            ''        iDataType:=PMResString)
            '
            '    tabMainTab.TabCaption(0) = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACTabTitle1, _
            ''        iDataType:=PMResString)

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            '***Insert GetRes Calls***

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_WARNING: (2074) ComboBox event cmbMax.Change was upgraded to cmbMax.TextChanged which has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2074.aspx
    Private isInitializingComponent As Boolean
    Private Sub cmbMax_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbMax.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            'dont allow non numerics
            Dim dbNumericTemp As Double
            If Not Double.TryParse(cmbMax.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                If Strings.Len(cmbMax.Text) > 0 Then
                    cmbMax.Text = cmbMax.Text.Substring(0, Strings.Len(cmbMax.Text) - 1)
                Else
                    cmbMax.Text = ""
                End If
            End If

            cmbMax.SelectionLength = Strings.Len(cmbMax.Text)
            cmbMax.SelectionStart = Strings.Len(cmbMax.Text)

            'enable find now button
            If (txtDocName.Text.Trim().Length > 0) And (cmbMax.Text <> "") Then
                cmdFindNow.Enabled = True
                VB6.SetDefault(cmdFindNow, True)
            Else
                cmdFindNow.Enabled = False
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmbMax_Change", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Exit Sub
        End Try

    End Sub

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bDOCFind.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                'sTitle$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFailTitle, _
                'iDataType:=PMResString)

                'sMessage$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFail, _
                'iDataType:=PMResString)

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        txtDocName.Focus()
    End Sub

    'UPGRADE_WARNING: (2080) Form_Load event was upgraded to Form_Load event and has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2080.aspx
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try
            txtDocName.Focus()
            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.
            'UPGRADE_ISSUE: (2070) Constant vbFormCode was not upgraded. More Information: http://www.vbtonet.com/ewis/ewi2070.aspx
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7 
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If


            ' Terminate the business object
            'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_oBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub txtDocName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'enable find now button
        If (txtDocName.Text.Trim().Length > 0) And (cmbMax.Text <> "") Then
            cmdFindNow.Enabled = True
            VB6.SetDefault(cmdFindNow, True)
        Else
            cmdFindNow.Enabled = False
        End If

    End Sub
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub
End Class