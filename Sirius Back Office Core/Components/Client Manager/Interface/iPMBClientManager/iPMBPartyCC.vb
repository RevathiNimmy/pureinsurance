Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmPartyCC
	Inherits System.Windows.Forms.Form
	'
	' History:
	' CJB080805 - PN23013 - Changed LoadInterface to prevent access to view claims if no access given.
	'
	' ***************************************************************** '
	' Form Name: frmPartyCC
	'
	' Date: 08/06/1998
	'
	' Description: Party Corporate Interface.
	'
	' Edit History:
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 3
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

    Private m_iIndex As Integer
    Private m_lPartyCnt As Integer
    Private m_sPartyType As String = ""
    Private m_sFooter As String = ""
    Private m_sShortName As String = ""
    Private m_sResolved As String = ""

    Private m_bEvent As Boolean

    'Are we closing this to edit, or just closing
    Private m_bEditing As Boolean

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_bIsIncludeClosedBranchChecked As Boolean

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property


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
    Public Property PartyType() As String
        Get

            Return m_sPartyType

        End Get
        Set(ByVal Value As String)

            m_sPartyType = Value

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

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property
    'JT PN-13238 To hold that whether the CheckBox of Include Closed Branch
    'was checked or not in FindParty
    Public Property IsIncludeClosedBranchChecked() As Boolean
        Get
            Return m_bIsIncludeClosedBranchChecked
        End Get
        Set(ByVal Value As Boolean)
            m_bIsIncludeClosedBranchChecked = Value
        End Set
    End Property
    'Private objFState() As FormState = Nothing
    'Public WriteOnly Property FState() As FormState()
    '    Set(ByVal value As FormState())
    '        Me.objFState = value
    '    End Set
    'End Property

    'Private objDocument() As Object
    'Public WriteOnly Property Document() As Object()
    '    Set(ByVal value As Object())
    '        Me.objDocument = value
    '    End Set
    'End Property
    Private m_parentMdiForm As frmMDI
    Public WriteOnly Property ParentMdiForm() As Form
        Set(ByVal value As Form)
            m_parentMdiForm = value
        End Set
    End Property
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctPartyCCControl1.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Close()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'm_lReturn& = ShowHelp(dlgHelp,objCM. ScreenHelpID)
        ' Click event of the Cancel button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            uctPartyCCControl1.ShowHelpScreen(cmdHelp, ScreenHelpID:=MainModule.ScreenHelpID)

            ' Check the return value.

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCustom_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCustom.Click

        'AR20050824 - PN24332
        m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)
        MyBase.ParentForm.Focus()

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'The Party-PartyBuilder link will now exist, so set the picture to a tick

            'Developer Guide No. 171(guide)
            Me.picIndicator.Image = Me.imgCustomData.Images.Item("TICK")
        End If

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim vFileName As String = ""
        Dim nReturn As Integer
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            uctPartyCCControl1.CurrentResolvedName = objCM.g_sResolvedName
            m_lReturn = uctPartyCCControl1.OKClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'IJB 04/09/2002 Save new ClientCode to recent files list

                'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
                vFileName = uctPartyCCControl1.ShortName & "|" & uctPartyCCControl1.LongName & "|" & CStr(m_lPartyCnt) & "C"
                objCM.AddRecentFile(m_parentMdiForm, vFileName)
                objCM.SaveRecentFiles(m_parentMdiForm)

                nReturn = uctPartyCCControl1.AddPartyHistory()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create party history.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                ' Everything OK, so we can hide the interface.
                Me.Close()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmPartyCC_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            m_parentMdiForm.StatusBar1.Items.Item(0).Text = Me.Footer
            m_parentMdiForm.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            m_parentMdiForm.StatusBar1.Items.Item(2).Text = Me.ShortName
            m_parentMdiForm.ShortName = Me.ShortName 'ADDED MK 991014
            m_parentMdiForm.ResolvedName = Me.ResolvedName 'ADDED MK 991014
            m_parentMdiForm.PartyType = Me.PartyType 'ADDED MKR PN 17193
            m_parentMdiForm.Text = "[" & Me.ResolvedName.Trim() & "] Sirius Client Manager"
            Me.Text = "Corporate Client : [" & Me.ResolvedName.Trim() & "]"


            m_lReturn = objCM.SetToolbar(v_sFormName:=Me.Name)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If

            m_parentMdiForm.InsuranceFolderCnt = 0
            m_parentMdiForm.InsFileCnt = 0
            m_parentMdiForm.InsReference = ""
            m_parentMdiForm.PolicyTypeId = PMBConst.PMBPolicyTypeGeneral
            m_parentMdiForm.GeminiPolicyStatus = 0

        End If
    End Sub

    ' ***************************************************************** '
    ' Name: LoadInterface
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function LoadInterface() As Integer

        Dim result As Integer = 0
        Try

            'AR20050824 - PN24332
            Dim bPartyHasDataModel, bPartyHasCustomData As Boolean

            result = gPMConstants.PMEReturnCode.PMTrue
            'AR20050824 - PN24332
            If PartyBuilderHandler.g_oObjectManager Is Nothing Then
                PartyBuilderHandler.g_oObjectManager = objCM.g_oObjectManager
            End If

            m_lReturn = PartyBuilderHandler.GetPartyBuilderFlags(m_lPartyCnt, bPartyHasDataModel, bPartyHasCustomData)

            If bPartyHasDataModel Then
                Me.cmdCustom.Visible = True
                If bPartyHasCustomData Then

                    'Developer Guide No. 171(guide)
                    Me.picIndicator.Image = Me.imgCustomData.Images.Item("TICK")
                Else

                    'Developer Guide No. 171(guide)
                    Me.picIndicator.Image = Me.imgCustomData.Images.Item("CROSS")
                End If
            Else
                Me.cmdCustom.Visible = False
                Me.picIndicator.Image = Nothing
            End If

            '2005 Client Manager Security
            If Not objCM.g_bRaiseCashAuthority Then
                mnuGotoTransactionCash.Enabled = False
            End If
            If Not objCM.g_bRaiseFeeAuthority Then
                mnuGotoTransactionFee.Enabled = False
            End If
            If Not objCM.g_bEditClaimAuthority Then 'PN23013
                mnuGoToClaim.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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
        'm_lErrorNumber = gPMConstants.PMEReturnCode.PMError
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub


    Private Sub frmPartyCC_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim i As Integer
        Dim lReturn, lStatus As Integer
        Dim sOption As String
        ' Forms load event.
        Try

            If Me.PartyCnt <> 0 Then
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
            Else
                m_iTask = gPMConstants.PMEComponentAction.PMAdd
            End If

            If FromEvent Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If

            Me.Height = VB6.TwipsToPixelsY(8330)
            Me.Width = VB6.TwipsToPixelsX(11195)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            With uctPartyCCControl1
                'Developer Guide No.  24(Guide)
                .Task = m_iTask
                'Developer Guide No.  24(Guide)
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                .PartyCnt = m_lPartyCnt
                .IsIncludeClosedBranchChecked = m_bIsIncludeClosedBranchChecked
                'Don't forget to put this back when Chris has finished with PartyCC
                'MSS280901 - Put it back when doing merge. Hope it works...
                .FromEvent = FromEvent
            End With

            'Developer Guide No. 9(guide)
            m_lReturn = uctPartyCCControl1.Initialise()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctPartyCCControl1.LoadControl()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctPartyCCControl1.GetParty()
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the business details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            lStatus = uctPartyCCControl1.Status

            'MSS280901 - Added for merge

            mnuTasks.Available = False
            mnuGotoTransaction.Available = False
            mnuDocumentationRiskRegister.Available = False
            mnuDocumentationMarkPresentation.Available = False
            'PW170602
            mnuReportsPolicyList.Available = False
            mnuReportsStatement.Available = False
            ' Alix - 20/01/2003 - PN9811
            ' Hide broking functionnality
            mnuGotoiMarket.Available = False
            mnuGoToStickyNotes.Available = False
            'MSS280901 - Merge end
            'eck Datasure only show Imarket link for UK
            If objCM.g_iCountryID <> 1 Then
                mnuGotoiMarket.Available = False
            End If

            'PW170602
            mnuReportsClientSummary.Available = False

            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = objCM.LoadRecentFiles(r_oForm:=Me)

            'sj 04/10/2002 - start
            If objCM.g_bHidePublicPrivateNotes Then
                mnuGoToNotes.Available = False
            End If
            'sj 04/10/2002 - end

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'Sharepoint
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            If sOption = "1" Then
                mnuGoToDocumaster.Visible = True
                mnuGoToSharePoint.Visible = False
            ElseIf sOption = "2" Then
                mnuGoToDocumaster.Visible = False
                mnuGoToSharePoint.Visible = True
            End If
            'Sharepoint
        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmPartyCC_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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

                cmdCancel.Focus() 'ct 31/08/00 This stops VB error 91 when closing not closing form via cancel button

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = uctPartyCCControl1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            End If

            ' Terminate the control
            uctPartyCCControl1.Dispose()

            'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True

            'DC150301 -start
            If Not m_bEditing Then
                'DC150301 -end

                m_parentMdiForm.StatusBar1.Items.Item(0).Text = ""
                m_parentMdiForm.StatusBar1.Items.Item(1).Text = ""
                m_parentMdiForm.StatusBar1.Items.Item(2).Text = ""
                m_parentMdiForm.Text = "Sirius Client Manager"

                'DC150301 -start
            End If
            'DC150301 -end

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            '2005 Close sticky notes
            'unload when not closed through MDI
            If UnloadMode <> 4 Then
                If Information.IsArray(objCM.g_vWarnings) Then

                    For iLoop1 As Integer = 0 To objCM.g_vWarnings.GetUpperBound(1)
                        'ContainerHelper.UnloadControl(Me, "Document", objCM.g_vWarnings(0, iLoop1))
                        If objCM.Document(objCM.g_vWarnings(0, iLoop1)).Name <> Me.Name Then
                            objCM.Document(objCM.g_vWarnings(0, iLoop1)).Close()
                        End If
                    Next iLoop1

                    objCM.g_vWarnings = Nothing
                End If
            End If


            iCount = 0

            'For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
            '    If Application.OpenForms.Item(iLoop1).Name <> Me.Name Then
            '        If Application.OpenForms.Item(iLoop1).Name <> m_parentMdiForm.Name Then
            '            iCount += 1
            '        End If
            '    End If
            'Next iLoop1
            For Each frmChild As Form In m_parentMdiForm.MdiChildren
                If Not frmChild Is Me And Not frmChild.Name = "frmWarning" Then
                    If frmChild.Visible Then
                        iCount += 1
                    End If
                End If
            Next

            If iCount = 0 Then
                ' Update

                m_lReturn = objCM.g_oCMManager.ImEmpty(v_lPartyCnt:=m_lPartyCnt)
                m_lReturn = objCM.SetToolbar(v_sFormName:=m_parentMdiForm.Name)
            End If

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
    Private Sub frmPartyCC_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            'sj 18/06/2002 - start
            'Me.Height = 6360 - 255
            'Me.Height = 7035
            'sj 18/06/2002 - end
            'sj 02/07/2002 - start
            Me.Height = VB6.TwipsToPixelsY(8330)
            'sj 02/07/2002 - end
            Me.Width = VB6.TwipsToPixelsX(11195)
        End If

    End Sub

    Private Sub frmPartyCC_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        '         Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        'gp20020415 - Ensure the RiskCnt is cleared
        m_parentMdiForm.RiskCnt = 0

        '         Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            '             Call the recent file list procedure
            'GetRecentFiles
            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = objCM.LoadRecentFiles(r_oForm:=Me)

        End If

    End Sub

    Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
        'CT 31/08/00 bugfix 379
        cmdCancel.Focus()
        cmdCancel_Click(cmdCancel, New EventArgs())
    End Sub

    Public Sub mnuClientOpen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientOpen.Click

        m_lReturn = objCM.OpenClient()

    End Sub

    Public Sub mnuDiaryFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDiaryFind.Click

        m_lReturn = objCM.ShowTaskList(v_lPartyCnt:=PartyCnt)

    End Sub

    Public Sub mnuDiaryNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDiaryNew.Click

        m_lReturn = objCM.ProcessTasks(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="", v_sResolvedName:=m_sResolved, v_lInsuranceFolderCnt:=0, v_lInsuranceFileCnt:=0, v_sPolicyDesc:="")

    End Sub

    ' PRIVATE Property Procedures (End)

    Public Sub mnuDocumentationLetterWriting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentationLetterWriting.Click

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

    Public Sub mnuDocumentationMarkPresentation_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentationMarkPresentation.Click

        Try

            ' Call ProcessRiskRegister function
            m_lReturn = objCM.ProcessRiskRegister(v_lPartyCnt:=m_lPartyCnt, v_lMode:=objCM.ACMarketMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuDocumentationMarkPresentation menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuDocumentationMarkPresentation_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuDocumentationRiskRegister_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentationRiskRegister.Click

        Try

            ' Call ProcessRiskRegister function
            m_lReturn = objCM.ProcessRiskRegister(v_lPartyCnt:=m_lPartyCnt, v_lMode:=objCM.ACRiskMode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuDocumentationRiskRegister menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuDocumentationRiskRegister_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuGotoAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoAccounts.Click

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

        Try

            ' CTAF 030300
            'DC071100 Now implemented
            'MsgBox "This functionality is yet to be implemented.", vbInformation, "Claims"
            'Exit Sub

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowClaimList(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch


            'Continue as not serious
            Exit Sub
        End Try


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
            m_lReturn = objCM.ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="C", v_sResolvedName:=m_sResolved)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.4)
    Public Sub mnuGotoInsuredAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoInsuredAccounts.Click

        Const kMethodName As String = "mnuGotoInsuredAccounts_Click"

        m_lReturn = objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sShortName:=m_sShortName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessOrionFunc Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.4)

    Public Sub mnuGoToNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToNotes.Click

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

    Public Sub mnuGoToStickyNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToStickyNotes.Click

        m_lReturn = objCM.CallAddStickyNote(v_lPartyCnt:=m_lPartyCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallAddStickyNote().", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGoToStickyNotes_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Exit Sub
        End If

    End Sub

    'eck131100
    Public Sub mnuGoToFinancePlan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToFinancePlan.Click

        m_lReturn = objCM.ProcessFinancePlanFunction(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sLongname:=m_sResolved)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Continue as not serious
            Exit Sub
        End If

    End Sub

    Public Sub mnuGoToTextFiles_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToTextFiles.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ShowTextFiles(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="C", v_sResolvedName:=m_sResolved)

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

    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click, _mnuRecentFile_5.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)

        m_lReturn = objcm.ShowRecentFile(iIndex:=Index, r_oForm:=Me)

    End Sub

    Public Sub mnuReportsClientSummary_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportsClientSummary.Click

        m_lReturn = objCM.RunReport(v_lPartyCnt:=PartyCnt, v_sReportName:="ClientReportSummary")

    End Sub

    Public Sub mnuReportsStatement_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportsStatement.Click

        m_lReturn = objCM.RunReport(v_lPartyCnt:=PartyCnt, v_sReportName:="Client_Statement_By_PartyCnt")

    End Sub

    Public Sub mnuReportsPolicyList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportsPolicyList.Click

        m_lReturn = objCM.RunReport(v_lPartyCnt:=PartyCnt, v_sReportName:="Policy_List_By_PartyCnt")

    End Sub

    Public Sub mnuReportsStatements_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportsStatements.Click
        '
        ' PW170602 - add underwriting version of report
        '
        m_lReturn = objCM.RunReport(v_lPartyCnt:=PartyCnt, v_sReportName:="Client_Statement_By_PartyCnt_U")

    End Sub

    Private Sub frmPartyCC_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 3
        End If
        If e.Alt And e.KeyCode = Keys.D5 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 4
        End If
        If e.Alt And e.KeyCode = Keys.D6 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 5
        End If
        If e.Alt And e.KeyCode = Keys.D7 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 6
        End If
        If e.Alt And e.KeyCode = Keys.D8 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 7
        End If
        If e.Alt And e.KeyCode = Keys.D9 Then
            DirectCast(uctPartyCCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 8
        End If
    End Sub

    Private Sub mnuGoToSharePoint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuGoToSharePoint.Click
        Dim sOption, sSPUrl, sDOCLIB As String
        m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, v_iSourceID:=g_iSourceID)
        Dim sDocumentLibrary As String = ""

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return
        End If

        If sOption = "2" Then
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5085, r_sOptionValue:=sSPUrl, v_iSourceID:=g_iSourceID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            m_lReturn = objCM.GetDocumentLibrary(v_lPartyCnt:=m_lPartyCnt, r_lDocumentLibrary:=sDocumentLibrary)
            sDOCLIB = sDocumentLibrary
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            If String.IsNullOrEmpty(sDOCLIB) Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5086, r_sOptionValue:=sDOCLIB, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return
                End If
            End If
        End If
        System.Diagnostics.Process.Start(sSPUrl & "\" & sDOCLIB & "\" & m_sShortName.Trim())
    End Sub
End Class
