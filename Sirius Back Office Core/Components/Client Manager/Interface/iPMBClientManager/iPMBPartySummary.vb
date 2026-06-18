Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmPartySummary
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmPartySummary
	'
	' Date: 23/06/1998
	'
	' Description: Main interface.
	'
	' Edit History: TF031298 - Menu & Toolbar activity
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
    Private Const ACClass As String = "frmPartySummary"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_iIndex As Integer
    ' {* USER DEFINED CODE (End) *}
    Private m_oInterface As Object

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private Const vbScrollBars As String = "0x80000000"
    Private Const vbDesktop As String = "0x80000001"
    Private Const vbActiveTitleBar As String = "0x80000002"
    Private Const vbInactiveTitleBar As String = "0x80000003"
    Private Const vbMenuBar As String = "0x80000004"
    Private Const vbWindowBackground As String = "H00FFFFC0"
    Private Const vbWindowFrame As String = "0x80000006"
    Private Const vbMenuText As String = "0x80000007"
    Private Const vbWindowText As String = "0x80000008"
    Private Const vbTitleBarText As String = "0x80000009"
    Private Const vbActiveBorder As String = "0x8000000A"
    Private Const vbInactiveBorder As String = "0x8000000B"
    Private Const vbApplicationWorkspace As String = "0x8000000C"
    Private Const vbHighlight As String = "0x8000000D"
    Private Const vbHighlightText As String = "0x8000000E"
    Private Const vbButtonFace As String = "0x8000000F"
    Private Const vbButtonShadow As String = "0x80000010"
    Private Const vbGrayText As String = "0x80000011"
    Private Const vbButtonText As String = "0x80000012"
    Private Const vbInactiveCaptionText As String = "0x80000013"
    Private Const vb3DHighlight As String = "0x80000014"
    Private Const vb3DDKShadow As String = "0x80000015"
    Private Const vb3DLight As String = "0x80000016"
    Private Const vbInfoText As String = "0x80000017"
    Private Const vbInfoBackground As String = "0x80000018"

    Private m_sFooter As String = ""
    Private m_sPartyType As String = ""
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sMainPostCode As String = ""
    Private m_sSurName As String = ""
    Private m_iPartyTitleID As Integer
    Private m_sForeName As String = ""
    Private m_sInitials As String = ""
    Private m_sResolved As String = ""
    Private m_iOccupationID As Integer
    Private m_dtDOB As Date
    Private m_lAgentCnt As Object
    Private m_sAgentRef As String = ""
    Private m_sAgentName As String = ""
    Private m_lEmployerCnt As Object
    Private m_sEmployerRef As String = ""
    Private m_vAddresses As Object
    Private m_vAddressTypes As Object
    Private m_vContacts As Object
    Private m_sAddressLine1 As String = ""
    'Extras for PMB
    Private m_vPersons As Object
    Private m_vPersonTypes As Object
    Private m_vPersonSex As Object
    Private m_sAssociateRef As String = ""
    Private m_sConsultantRef As String = ""
    Private m_sConsultantName As String = ""

    'Flags to indicate whether we need to check the employer/agent ids match
    'the employer/agent ref as user may change the reference directly
    Private m_bVerifyAgentCnt As Boolean
    Private m_bVerifyEmployerCnt As Boolean

    'Are we closing this to edit, or just closing
    Private m_bEditing As Boolean

    'Note the index in the lookup array of the main address
    Private m_iMainAddressIndex As Integer

    ' Declare an instance of the address interface.
    Private m_oAddress As Object

    ' Declare an instance of the contact interface.
    Private m_oContact As Object

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
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

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
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
    Public Property Index() As Integer
        Get

            ' Return the objects task.
            Return m_iIndex

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iIndex = Value

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
    ' {* USER DEFINED CODE (Begin) *}

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property
    Public Property Footer() As String
        Get

            Return m_sFooter

        End Get
        Set(ByVal Value As String)

            m_sFooter = Value

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
    Public Property ResolvedName() As String
        Get

            Return m_sResolved

        End Get
        Set(ByVal Value As String)

            m_sResolved = Value

        End Set
    End Property
    Public Property PartyType() As String
        Get

            Return m_sPartyType

        End Get
        Set(ByVal Value As String)

            m_sPartyType = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    Public Function LoadInterface() As gPMConstants.PMEReturnCode


        Return gPMConstants.PMEReturnCode.PMTrue



        ' Error Section

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisableForm) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisableForm(ByRef lDisabled As Integer) As Integer
    '
    'Dim result As Integer = 0
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Set all of the forms controls to the disable state.

    'For	Each ctlFormControl As Control In ContainerHelper.Controls(frmPartyPC)
    ' Check the type of the control.
    'If TypeOf ctlFormControl Is TextBox Then
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
    'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
    'ElseIf (TypeOf ctlFormControl Is SSOption) Then
    '    ctlFormControl.Enabled = Not lDisabled&
    'End If
    'Next ctlFormControl
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        'open another document

        m_lReturn = uctPartySummControl1.EditClick()

        ' Check the return value.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        'SD 12/07/2002 Open screen in edit detail
        objCM.gPartySummaryScreenShown = True

        m_lReturn = objCM.OpenSummaryFile(vPartyCnt:=m_lPartyCnt, vPartyShortName:=m_sShortName, vPartyType:=m_sPartyType, vPartyResolvedName:=m_sResolved)

        m_bEditing = True


        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Me.Hide()
        End If

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        '        m_lReturn& = ShowHelp(dlgHelp,objCM. ScreenHelpID)
        ' Click event of the Cancel button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctPartySummControl1.ShowHelpScreen(cmdHelp, ScreenHelpID:=MainModule.ScreenHelpID)

            ' Check the return value.

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim iLen As Integer

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = uctPartySummControl1.OKClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_lReturn = uctPartySummControl1.CancelClick()

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

    ' PRIVATE Events (End)

    Private Sub frmPartySummary_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            objCM.m_ofrmMDI.ShortName = Me.ShortName 'ADDED MK 991014
            objCM.m_ofrmMDI.ResolvedName = Me.ResolvedName 'ADDED MK 991014
            objCM.m_ofrmMDI.PartyType = Me.PartyType 'ADDED MKR PN 17193
            objCM.m_ofrmMDI.Text = "[" & Me.ResolvedName.Trim() & "] Sirius Client Manager"
            Me.Text = "Summary : [" & Me.ResolvedName.Trim() & "]"

            m_lReturn = objCM.SetToolbar(v_sFormName:=Me.Name)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If

            objCM.m_ofrmMDI.InsuranceFolderCnt = 0
            objCM.m_ofrmMDI.InsFileCnt = 0
            objCM.m_ofrmMDI.InsReference = ""
            objCM.m_ofrmMDI.PolicyTypeId = PMBConst.PMBPolicyTypeGeneral
            objCM.m_ofrmMDI.GeminiPolicyStatus = 0

        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section
        '
        'm_lErrorNumber = gPMConstants.PMEReturnCode.PMError
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub


    Private Sub frmPartySummary_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lStatus As Integer
        Dim sTemp As String = ""

        '        ' Forms load event.

        Try

            m_bEditing = False

            If Me.PartyCnt <> 0 Then
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
            Else
                m_iTask = gPMConstants.PMEComponentAction.PMAdd
            End If

            VB6.SetDefault(cmdOK, True)

            Me.Height = VB6.TwipsToPixelsY(6400)
            Me.Width = VB6.TwipsToPixelsX(9435)

            With uctPartySummControl1
                'Developer Guide No. 24(Guide)
                '.set_Task(m_iTask)
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                .PartyCnt = m_lPartyCnt
                m_lReturn = .Initialise()
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Exit Sub
                End If
                m_lReturn = .LoadControl()
                m_lReturn = .GetParty()
                lStatus = .Status
            End With

            ' Get the number of recent files
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="MaxMRU", r_sSettingValue:=sTemp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read registry settings.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                ' Default to 4
                sTemp = "4"
            End If

            ' Convert the result back to an integer
            objCM.g_iMaxRecent = CInt(sTemp)

            ' Load the menus
            For iLoop1 As Integer = 2 To objCM.g_iMaxRecent
                ContainerHelper.LoadControl(Me, "mnuRecentFile", iLoop1)
                With mnuRecentFile(iLoop1)
                    .Text = "RecentFile" & iLoop1
                    .Available = False
                End With
            Next iLoop1

            'sj 04/10/2002 - start
            If objCM.g_bHidePublicPrivateNotes Then
                mnuGotoNotes.Available = False
            End If
            'sj 04/10/2002 - end

            'Kevin Renshaw (CMG) 24/03/2003 - remove Broking functionality

            mnuDocumentRiskRegister.Available = False
            mnuDocumentsMarket.Available = False

            'Thinh Nguyen 27/06/2003 (start) - PN5049 do not show Transaction menu for underwriting
            mnuTransaction.Available = False
            'Thinh Nguyen 27/06/2003 (start) - PN5049 do not show Transaction menu for underwriting

            ' Alix - 20/01/2003 - PN9811
            ' Hide broking functionnality
            mnuGotoiMarket.Available = False
            ' /Alix

            'eck Datasure only show Imarket link for UK
            If objCM.g_iCountryID <> 1 Then
                mnuGotoiMarket.Available = False
            End If

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmPartySummary_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint


        'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
        Dim vFileName As String = Me.ShortName & "|" & Me.ResolvedName & "|" & CStr(Me.PartyCnt) & Me.PartyType

        ' Update the recent files menu
        m_lReturn = objCM.UpdateFileMenu(vFileName:=vFileName)

    End Sub

    Private Sub frmPartySummary_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim iCount As Integer

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctPartySummControl1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If

            'SD 12/07/2002 Reinstate the terminate method
            ' Terminate the control
            If Not m_bEditing Then
                uctPartySummControl1.Dispose()
            End If

            'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True

            'Don't reset these if editing, as the other form is already activated and
            'so won't be updating them...
            If Not m_bEditing Then

                objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = ""
                objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
                objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
                objCM.m_ofrmMDI.Text = "Sirius Client Manager"

            End If

            iCount = 0

            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name <> Me.Name Then
                    If Application.OpenForms.Item(iLoop1).Name <> objCM.m_ofrmMDI.Name Then
                        iCount += 1
                    End If
                End If
            Next iLoop1

            If iCount = 0 Then
                ' Update

                m_lReturn = objCM.g_oCMManager.ImEmpty(v_lPartyCnt:=m_lPartyCnt)
                m_lReturn = objCM.SetToolbar(v_sFormName:=objCM.m_ofrmMDI.Name)
            End If

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

    Private isInitializingComponent As Boolean
    Private Sub frmPartySummary_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            Me.Height = VB6.TwipsToPixelsY(6400)
            Me.Width = VB6.TwipsToPixelsX(9435)
        End If

    End Sub

    Private Sub frmPartySummary_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        '         Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        '         Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            '             Call the recent file list procedure
            objCM.GetRecentFiles()
        End If
    End Sub

    Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
        Me.Hide() 'CT 31/08/00
    End Sub

    Public Sub mnuClientOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientOpen.Click

        m_lReturn = objCM.OpenClient()

    End Sub

    Public Sub mnuDocumentRiskRegister_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentRiskRegister.Click

        Try

            ' Call ProcessRiskRegister function
            m_lReturn = objCM.ProcessRiskRegister(v_lPartyCnt:=m_lPartyCnt, v_lMode:=objCM.ACRiskMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuDocumentRiskRegister menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuDocumentRiskRegister_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuDocumentsLetterWriting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentsLetterWriting.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuDocumentsMarket_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentsMarket.Click

        Try

            ' Call ProcessRiskRegister function
            m_lReturn = objCM.ProcessRiskRegister(v_lPartyCnt:=m_lPartyCnt, v_lMode:=objCM.ACMarketMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuDocumentsMarket menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuDocumentsMarket_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuGoToAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToAccounts.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=ACIGotoAccounts, v_sShortName:=m_sShortName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoAccounts menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoAccounts_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuGoToClaim_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToClaim.Click

        ' CTAF 030300
        MessageBox.Show("This functionality is yet to be implemented.", "Claims", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Exit Sub

    End Sub

    Public Sub mnuGoToDocumaster_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToDocumaster.Click

        ' CTAF 030300
        'MsgBox "This functionality is yet to be implemented.", vbInformation, "Documaster"
        'Exit Sub

        ' ND 181000
        ' Call Documaster link to open Documaster at client level (1) for this client
        m_lReturn = objCM.ShowDocumaster(v_sLinkCode:=m_sShortName & objCM.DME_CLIENT)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Continue as not serious
            Exit Sub
        End If


    End Sub

    Public Sub mnuGoToEvents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToEvents.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:=m_sResolved)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.10)
    Public Sub mnuGotoInsuredAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoInsuredAccounts.Click

        Const kMethodName As String = "mnuGotoInsuredAccounts_Click"

        m_lReturn = objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sShortName:=m_sShortName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessOrionFunc Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.10)

    Public Sub mnuGotoNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoNotes.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNameParty, v_lEntityCnt:=m_lPartyCnt, v_sTextType:="Public")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    'DC041203
    Public Sub mnuGotoiMarket_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoiMarket.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.CalliMarket()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try



    End Sub

    Public Sub mnuGoToPolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToPolicy.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowPolicy(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGoToTextFiles_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToTextFiles.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowTextFiles(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="X", v_sResolvedName:=m_sResolved)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGotoTransactionCash_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionCash.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=ACIGotoTransactionCash, v_sShortName:=m_sShortName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionCash menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionCash_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuGotoTransactionFee_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionFee.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = objCM.ProcessOrionFunc(v_iButton:=ACIGotoTransactionFee, v_lPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionFee menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionFee_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        m_lReturn = objCM.ShowSBOAbout()

    End Sub

    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)

        m_lReturn = objcm.ShowRecentFile(iIndex:=Index, r_oForm:=Me)

    End Sub

    Public Sub mnuReportsClientSummary_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportsClientSummary.Click

        m_lReturn = objCM.RunReport(v_lPartyCnt:=PartyCnt, v_sReportName:="ClientReportSummary")

    End Sub

    Public Sub mnuReportsStatements_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportsStatements.Click

        m_lReturn = objCM.RunReport(v_lPartyCnt:=PartyCnt, v_sReportName:="clients\Client_statement")

    End Sub

    Private Sub frmPartySummary_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctPartySummControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub
End Class
