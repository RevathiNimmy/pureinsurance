Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Text
Imports System.Data
Imports System.Linq
Imports PMLookupControl
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
   
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmList"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Instance of Navigator
    Private WithEvents m_oNav As iPMNavigator.NavigateControl

    Private Const vbFormCode As Integer = 0

    ' Declare an instance of the Business object.

    Private m_oBusiness As bACTFinanceSpoke.Business
    'Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer
    Private m_sAgencyOrUnderwriting As String = ""
    Private m_bAttachToScheduler As Boolean
    Private m_iBatchProcessId As Integer
    Private m_dtParameters As DataTable = Nothing
    Private m_oBatchParameters(,) As Object
    Private m_ibatchSchedulerId As Integer
    Private m_sbatchContentDetails As String = ""

    ' START CHANGES - Changed By: AAB  - Changed On: 18-Feb-2004 11:22
    ' To match bACTFinanceSpoke.ExportChaseCycle class
    ' Constants for the HeaderData array
    Private Const kbHDBranch As Byte = 9
    Private Const kbHDAsOfDate As Byte = 10
    Private Const kbHDSpoolDoc As Byte = 11
    Private Const kbHDArchiveDoc As Byte = 12
    ' END CHANGES - Changed By: AAB  - Changed On: 18-Feb-2004 11:22

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
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public Property AttachToScheduler() As Boolean
        Get
            Return m_bAttachToScheduler
        End Get
        Set(ByVal Value As Boolean)
            m_bAttachToScheduler = Value
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

    Public Property BatchParameters() As Object(,)
        Get
            Return m_oBatchParameters
        End Get
        Set(ByVal Value As Object(,))
            m_oBatchParameters = Value
        End Set
    End Property

    Public Property BatchFileContentDetails() As String
        Get
            Return m_sbatchContentDetails
        End Get
        Set(value As String)
            m_sbatchContentDetails = value
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
    ' PUBLIC Methods (Begin)

    Public Function Initialise() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sMessage, sTitle As String

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTFinanceSpoke.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "bACTFinanceSpoke.Business" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sAgencyOrUnderwriting)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Load_Renamed() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults for the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Gets the interface details to be displayed.

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface defaults for the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception

            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0

        Dim sAutoArchive As String = ""

        Const AUTO_ARCHIVE_ENABLED As Integer = 5008

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            dtpASofDate.Value = DateTime.Now

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get system option auto-archive
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=AUTO_ARCHIVE_ENABLED, r_sOptionValue:=sAutoArchive, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sAutoArchive = "1" Then
                chkArchive.CheckState = CheckState.Checked
                chkArchive.Visible = False
            End If

            'Filter out branches user is not authorised to access.
            cboSource.WhereClause = "source_id NOT IN (SELECT source_id FROM pmuser_source WHERE user_id =" & g_oObjectManager.UserID & " AND is_deleted = 0)"
            cboSource.RefreshList()

            cboSource.ListIndex = -1

            If AttachToScheduler Then
                cmdOK.Visible = False
                cmdCancel.Visible = False
                cmdHelp.Visible = False
                dtpASofDate.Visible = False
                lblAsOFDate.Visible = False
                If cboSource.ListIndex = -1 Then
                    cmdSchedule.Enabled = False
                End If
                Select Case m_iTask
                    Case gPMConstants.PMEComponentAction.PMEdit
                        m_lReturn = LoadBatchParameters()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("LoadBatchParameters", "Unable to load parameters for batch schedular")
                        End If
                    Case gPMConstants.PMEComponentAction.PMView
                        cmdSchedule.Text = "View &Schedule"

                        m_lReturn = LoadBatchParameters()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("LoadBatchParameters", "Unable to load parameters for batch schedular")
                        End If

                        EnableDisableControls(False)

                End Select
            Else
                cmdSchedule.Visible = False
            End If

            Return result

        Catch excep As System.Exception

            ' {* USER DEFINED CODE (End) *}

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from tI hehe resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            frmMainFrame.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranchLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAsOFDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAsOfLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            chkSpool.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSpoolCheckBox, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            chkArchive.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACArchiveCheckBox, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Return result

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function ValidateForm() As Boolean
        Dim result As Boolean = False
        Try

            result = True

            If cboSource.ListIndex = -1 Then
                MessageBox.Show("Please select Branch" & Strings.Chr(9), "Invalid branch", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If cboSource.Enabled Then
                    cboSource.Focus()
                End If
                Return False
            End If

            Return result

        Catch excep As System.Exception
            result = False

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateFormFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        ' Process the next set of actions.
        m_lReturn = ProcessCommand()

        ' Check the return value.
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            ' Everything OK, so we can hide the interface.
            Me.Hide()
        End If

        Exit Sub
        ' Error Section.

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub
    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then


                    Else

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If

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
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim sTitle, sMessage, sStatusCode As String
        Dim vHeaderData(1) As Object
        Dim vHeaderDetail(12) As Object

        Try

            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            If ValidateForm() Then
                ' Set the vHeaderData Elements

                vHeaderData(0) = "SIRIUS"

                vHeaderData(1) = vHeaderDetail
                ' Set the 9th element in the detail to = branch
                
                vHeaderData(1)(kbHDBranch) = cboSource.ItemCode.Trim()

                vHeaderData(1)(kbHDAsOfDate) = DateTime.Parse(dtpASofDate.Value).ToString("d")

                vHeaderData(1)(kbHDSpoolDoc) = chkSpool.CheckState

                vHeaderData(1)(kbHDArchiveDoc) = chkArchive.CheckState
                ' Set the status and message variables
                sStatusCode = ACSpokeStatusCode
                sMessage = ACSpokeMessage

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                ' Status text

                _stbStatus_Panel1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusProcessing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                m_lReturn = DisableForm(lDisabled:=True)

                'Process using m_obusiness object

                m_lReturn = m_oBusiness.Export(v_sInterfaceCode:=ACSpokeInterfaceCode, r_sBatchRef:=ACSpokeBatch, r_sStatusCode:=sStatusCode, r_sMessage:=sMessage, r_sHeaderXML:=ACSpokeHeaderXML, r_vHeaderData:=vHeaderData, r_vDetailData:=ACSpokeDetailData)

                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    'show Message Box

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedSpokeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedNoRecords, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Display message.
                    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    'show Message Box

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedSpokeTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedSpokeError, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    ' Display message.
                    MessageBox.Show(sMessage & Strings.Chr(13) & Strings.Chr(10) & "iPMUChaseCycleProcessing.frmInterface" & Strings.Chr(13) & Strings.Chr(10) & "(" & ACApp & "." & ACClass & ")", sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bACTFinanceSpoke.Export method failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Else
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Status text

            _stbStatus_Panel1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusReady, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            m_lReturn = DisableForm(lDisabled:=False)

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdOK_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()
        iPMFunc.ShowFormInTaskBar_Attach()
    End Sub

    Private Sub frmInterface_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        iPMFunc.ShowFormInTaskBar_Detach()
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

             If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.

                    'Cancel = 1
                    eventArgs.Cancel = True

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all of the forms controls to the disable state.

            For Each ctlFormControl As Control In ContainerHelper.Controls(Me)
                ' Check the type of the control.
                If TypeOf ctlFormControl Is TextBox Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is ComboBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                ElseIf (TypeOf ctlFormControl Is GroupBox) Then
                    ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
                End If
            Next ctlFormControl

            Return result

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub



    Private Sub cmdSchedule_Click(sender As Object, e As EventArgs) Handles cmdSchedule.Click

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Const kMethodName As String = "cmdSchedule_Click"
        Const kProcessName As String = "Chase Cycle"
        Dim batchFileContentDetails As String = ""
        Dim batchFileName As String = ""
        Dim vParamArray(,) As Object = Nothing
        Dim processDescription As String = kProcessName
        Try

            If Task <> gPMConstants.PMEComponentAction.PMView Then

                result = SetBatchParameters(vParamArray:=vParamArray, processDescription:=processDescription)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("iACTChaseCycleProcessing.cmdSchedule_Click", "Unable to set batch parameters")
                End If

                Dim lowerBound, upperBound As Integer
                Dim commandLine As New StringBuilder("")

                batchFileContentDetails = Application.StartupPath() & "\ChaseCycleCLI.EXE "

                If Information.IsArray(vParamArray) Then
                    lowerBound = vParamArray.GetLowerBound(1)
                    upperBound = vParamArray.GetUpperBound(1)

                    For count As Integer = lowerBound To upperBound
                        If Not Object.Equals(vParamArray(0, count), Nothing) And Not Object.Equals(vParamArray(1, count), Nothing) Then
                            commandLine.Append(CStr(vParamArray(0, count)).TrimEnd() & "=" & CStr(vParamArray(1, count)).TrimEnd() & " ")
                        End If
                    Next count
                End If

                result = CreateSchedularProcessParameter(vParamArray:=vParamArray)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("iACTChaseCycleProcessing.cmdSchedule_Click", "Unable to create batch parameters")
                End If

                batchFileName = "ChaseCycle_" & Now.ToString("yyyyMMddhhmm")
                batchFileContentDetails &= commandLine.ToString()

            End If

            Dim frequencySchedular As Object = Nothing
            Dim iSIRFrequencyScheduler As iSIRFrequencyScheduler.Interface_Renamed = New iSIRFrequencyScheduler.Interface_Renamed
            m_lReturn = g_oObjectManager.GetInstance(frequencySchedular, sClassName:="iSIRFrequencyScheduler.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            iSIRFrequencyScheduler = frequencySchedular
            iSIRFrequencyScheduler.BatchProcessId = m_iBatchProcessId
            iSIRFrequencyScheduler.BatchFileName = batchFileName
            iSIRFrequencyScheduler.BatchFileContentDetails = batchFileContentDetails
            iSIRFrequencyScheduler.Process = kProcessName
            iSIRFrequencyScheduler.ProcessDescription = processDescription
            iSIRFrequencyScheduler.ProcessParameters = m_dtParameters
            iSIRFrequencyScheduler.BatchSchedulerId = m_ibatchSchedulerId
            iSIRFrequencyScheduler.Task = m_iTask
            iSIRFrequencyScheduler.UserName = g_sUserName
            frequencySchedular.Start()
            cmdCancel_Click(sender, e)
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try

    End Sub

    Private Function SetBatchParameters(ByRef vParamArray(,) As Object, ByRef processDescription As String) As Integer

        Dim upperBound As Integer

        vParamArray = Nothing
        SetBatchParameters = gPMConstants.PMEReturnCode.PMTrue
        Try

            upperBound = 0
            ReDim vParamArray(2, upperBound)
            vParamArray(0, upperBound) = "PROCESSINGDATE"
            vParamArray(1, upperBound) = "CURRENTDATE"

            upperBound += 1
            ReDim Preserve vParamArray(2, upperBound)
            vParamArray(0, upperBound) = "BRANCH"
            vParamArray(1, upperBound) = cboSource.ItemCode.Trim()
            vParamArray(2, upperBound) = "cboSource" & "_" & cboSource.ItemId
            processDescription &= "_" & cboSource.ItemCaption.Trim()

            upperBound += 1
            ReDim Preserve vParamArray(2, upperBound)
            vParamArray(0, upperBound) = "SPOOLDOCUMENTS"
            vParamArray(1, upperBound) = IIf(chkSpool.Checked, "TRUE", "FALSE")
            vParamArray(2, upperBound) = "chkSpool" & "_" & chkSpool.CheckState

            upperBound += 1
            ReDim Preserve vParamArray(2, upperBound)
            vParamArray(0, upperBound) = "ARCHIVEDOCUMENTS"
            vParamArray(1, upperBound) = IIf(chkArchive.Checked, "TRUE", "FALSE")
            vParamArray(2, upperBound) = "chkArchive" & "_" & chkArchive.CheckState
            processDescription &= "_" & Now.ToString("yyyyMMddhhmm")

        Catch
            SetBatchParameters = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function
    Private Sub cboSource_ItemIdChange() Handles cboSource.ItemIdChange
        If AttachToScheduler Then
            cmdSchedule.Enabled = IIf(cboSource.ListIndex = -1, False, True)
        End If
    End Sub

    Private Function CreateSchedularProcessParameter(ByVal vParamArray(,) As Object) As Integer

        CreateSchedularProcessParameter = gPMConstants.PMEReturnCode.PMTrue
        Try
            m_dtParameters = New DataTable("Scheduler")

            m_dtParameters.Columns.AddRange(New DataColumn(4) {
                                                        New DataColumn("Id", System.Type.GetType("System.String")),
                                                       New DataColumn("ParameterName", System.Type.GetType("System.String")),
                                                       New DataColumn("DefaultValue", System.Type.GetType("System.String")),
                                                       New DataColumn("DataType", System.Type.GetType("System.String")),
                                                       New DataColumn("CurrentValue", System.Type.GetType("System.String"))})


            Dim lowerBound, upperBound As Integer
            Dim lookupArray() As String
            Dim parameterName As String = ""
            Dim currentValue As String = ""

            If Information.IsArray(vParamArray) Then

                lowerBound = vParamArray.GetLowerBound(1)
                upperBound = vParamArray.GetUpperBound(1)

                For count As Integer = lowerBound To upperBound
                    If Not Object.Equals(vParamArray(1, count), Nothing) AndAlso Not Object.Equals(vParamArray(2, count), Nothing) Then
                        parameterName = vParamArray(2, count)
                        currentValue = vParamArray(1, count)
                        If parameterName.IndexOf("_") > 0 Then
                            lookupArray = Strings.Split(parameterName, "_")
                            parameterName = lookupArray(0)
                            currentValue = lookupArray(1)
                        End If
                        m_dtParameters.LoadDataRow(New String(4) {String.Empty, parameterName, "", "", currentValue}, True)
                    End If
                Next count
            End If
        Catch
            CreateSchedularProcessParameter = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function
    Private Function LoadBatchParameters() As Integer

        Dim ctlFormControl As Object
        Dim controlValue As String = ""
        LoadBatchParameters = gPMConstants.PMEReturnCode.PMTrue
        Try
            If Information.IsArray(m_oBatchParameters) Then
                For lCount As Integer = m_oBatchParameters.GetLowerBound(1) To m_oBatchParameters.GetUpperBound(1)
                    controlValue = (m_oBatchParameters(1, lCount))
                    ctlFormControl = Me.Controls.Find(m_oBatchParameters(0, lCount), True).FirstOrDefault()
                    If Not ctlFormControl Is Nothing Then
                        If (TypeOf ctlFormControl Is TextBox) Then
                            ctlFormControl.Text = controlValue
                        ElseIf (TypeOf ctlFormControl Is CheckBox) Then
                            ctlFormControl.Checked = IIf(controlValue = "1", CheckState.Checked, CheckState.Unchecked)
                        ElseIf (TypeOf ctlFormControl Is cboPMLookup) Then
                            ctlFormControl.ItemId = CInt(controlValue)
                        End If
                    End If
                Next
            End If
        Catch
            LoadBatchParameters = gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function
    Private Sub EnableDisableControls(ByVal enabled As Boolean)

        lblBranch.Enabled = enabled
        cboSource.Enabled = enabled
        chkSpool.Enabled = enabled
        chkArchive.Enabled = enabled
    End Sub
End Class
