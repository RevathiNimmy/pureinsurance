Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmPolicySummaryUnderwriting
	Inherits System.Windows.Forms.Form
	
	
	' ***************************************************************** '
	' Form Name: frmPartySummary
	'
	' Date: 23/06/1998
	'
	' Description: Main interface.
	'
	' Edit History: TF031298 - Menu & Toolbar activity
	'               VB 03/03/2005 PN19085 Documaster implemented in mnuGoToDocumater
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmPolicySummaryUnderwriting"
    'Developer Guide No. 7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
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
    Private m_lReturn As gPMConstants.PMEReturnCode

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

    Private m_lPartyCnt As Integer
    Private m_sPartyType As String = ""
    Private m_sShortName As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sInsReference As String = ""
    Private m_lInsuranceFileStructureId As Integer
    Private m_lPolicyTypeID As Integer
    Private m_vGeminiPolicyStatus As Integer
    'Are we closing this to edit, or just closing
    Private m_bEditing As Boolean
    Private m_sFooter As String = ""
    Private m_lRiskCnt As Integer


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


    Public Property Footer() As String
        Get

            Return m_sFooter

        End Get
        Set(ByVal Value As String)

            m_sFooter = Value

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

    Public Property InsReference() As String
        Get

            Return m_sInsReference

        End Get
        Set(ByVal Value As String)

            m_sInsReference = Value

        End Set
    End Property

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

    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property

    Public Property InsuranceFileStructureId() As Integer
        Get

            Return m_lInsuranceFileStructureId

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileStructureId = Value

        End Set
    End Property

    Public Property PolicyTypeId() As Integer
        Get

            Return m_lPolicyTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lPolicyTypeID = Value

        End Set
    End Property

    Public Property GeminiPolicyStatus() As Integer
        Get

            Return m_vGeminiPolicyStatus

        End Get
        Set(ByVal Value As Integer)


            m_vGeminiPolicyStatus = CInt(Value)

        End Set
    End Property


    Public Property RiskCnt() As Integer
        Get
            Return m_lRiskCnt
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    Public Function LoadInterface() As gPMConstants.PMEReturnCode


        Return gPMConstants.PMEReturnCode.PMTrue



        ' Error Section

        ' Log Error.
        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:= New Exception(Information.Err().Description))

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: SwitchTo
    '
    ' Description: Switches focus to this form.
    '
    ' ***************************************************************** '
    Public Function SwitchTo() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the focus
            Me.Activate()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchTo", excep:=excep)

            Return result

        End Try
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

    ' ***************************************************************** '
    '
    ' Name: LockThePolicy
    '
    ' Description:
    '
    ' History: 06/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function LockThePolicy() As Integer

        Dim result As Integer = 0
        Try


            Return uctPMUPolicySummary1.LockThePolicy()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockThePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockThePolicy", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnlockThePolicy
    '
    ' Description:
    '
    ' History: 06/01/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UnlockThePolicy() As Integer

        Dim result As Integer = 0
        Try


            Return uctPMUPolicySummary1.UnlockThePolicy()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockThePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockThePolicy", excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Dim bThere As Boolean

        'open another document

        'Check that the risk screen isn't open
        m_lReturn = CType(objCM.CheckRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskGroupID:=uctPMUPolicySummary1.RiskGroupId, v_bFromEvent:=False, r_bThere:=bThere), gPMConstants.PMEReturnCode)

        If bThere Then
            MessageBox.Show("You must close risk screen before editing", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        'If we're here then we're showing one of our own policies, but just in case...

        m_lReturn = objCM.ShowPolicyDetail(v_lPartyCnt:=m_lPartyCnt, v_sPartyType:=m_sPartyType, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsFileCnt:=m_lInsuranceFileCnt, v_sShortName:=m_sShortName, v_sInsReference:=m_sInsReference, v_lInsuranceFileStructureId:=m_lInsuranceFileStructureId, v_bFromEvent:=False, v_lPolicyTypeId:=m_lPolicyTypeID, v_vGeminiPolicyStatus:=m_vGeminiPolicyStatus)

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
            m_lReturn = uctPMUPolicySummary1.ShowHelpScreen(cmdHelp, objCM.ScreenHelpID)

            ' Check the return value.

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim iLen As Integer
        Dim bThere As Boolean

        ' Click event of the OK button.

        Try

            'Check that the risk screen isn't open
            m_lReturn = CType(objCM.CheckRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskGroupID:=uctPMUPolicySummary1.RiskGroupId, v_bFromEvent:=False, r_bThere:=bThere), gPMConstants.PMEReturnCode)

            If bThere Then
                MessageBox.Show("You must close risk screen before exiting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_lReturn = uctPMUPolicySummary1.OKClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim bThere As Boolean

        ' Click event of the Cancel button.

        Try

            'Check that the risk screen isn't open
            m_lReturn = CType(objCM.CheckRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskGroupID:=uctPMUPolicySummary1.RiskGroupId, v_bFromEvent:=False, r_bThere:=bThere), gPMConstants.PMEReturnCode)

            If bThere Then
                MessageBox.Show("You must close risk screen before exiting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctPMUPolicySummary1.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)

    Private Sub frmPolicySummaryUnderwriting_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            objCM.m_ofrmMDI.Text = "[" & Me.InsReference.Trim() & "] Sirius Client Manager"
            Me.Text = "Summary : [" & Me.InsReference.Trim() & "]"

            m_lReturn = CType(objCM.SetToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If

            objCM.m_ofrmMDI.InsFileCnt = Me.InsuranceFileCnt
            objCM.m_ofrmMDI.InsuranceFolderCnt = Me.InsuranceFolderCnt
            objCM.m_ofrmMDI.InsReference = Me.InsReference
            objCM.m_ofrmMDI.PolicyTypeId = m_lPolicyTypeID
            objCM.m_ofrmMDI.GeminiPolicyStatus = m_vGeminiPolicyStatus

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


    Private Sub frmPolicySummaryUnderwriting_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lStatus As Integer
        Dim sTemp As String = ""
        Dim sOption As String
        '        ' Forms load event.

        Try

            m_bEditing = False

            'hide unwanted menu
            mnuPolicy.Available = False
            mnuGoTOTRansaction.Available = False
            mnuGoToSwift.Available = False

            m_iTask = gPMConstants.PMEComponentAction.PMView

            Me.Height = VB6.TwipsToPixelsY(6285)
            Me.Width = VB6.TwipsToPixelsX(9045)

            ' PSA 25092000
            'If (m_lPolicyTypeId = PMBPolicyTypeGIIMotor) Then
            '    cmdEdit.Visible = False
            'End If
            ' PSA 25092000

            With uctPMUPolicySummary1
                'Developer Guide No 24. 
                .Task = m_iTask
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                .PartyCnt = m_lPartyCnt
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .InsuranceFileCnt = m_lInsuranceFileCnt
                m_lReturn = .Initialise()
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Exit Sub
                End If
                m_lReturn = .LoadControl()
                m_lReturn = .GetPolicy()
                lStatus = .Status
                m_lInsuranceFileStructureId = .InsuranceFileStructureId
            End With

            ' Get the number of recent files
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="MaxMRU", r_sSettingValue:=sTemp), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read registry settings.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=New Exception(Information.Err().Description))

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

            mnuTasks.Available = False
            mnuGoTOTRansaction.Available = False
            mnuReportsClientSummary.Available = False


            'sj 04/10/2002 - start
            If objCM.g_bHidePublicPrivateNotes Then
                mnuGotoNotes.Available = False
            End If
            'sj 04/10/2002 - end
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
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmPolicySummaryUnderwriting_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
                m_lReturn = uctPMUPolicySummary1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

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
                m_lReturn = CType(objCM.SetToolbar(v_sFormName:=objCM.m_ofrmMDI.Name), gPMConstants.PMEReturnCode)
            End If

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmPolicySummaryUnderwriting_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            Me.Height = 433 ' VB6.TwipsToPixelsY(6285)
            Me.Width = 623 'VB6.TwipsToPixelsX(9045)
        End If

    End Sub

    Private Sub frmPolicySummaryUnderwriting_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        '         Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        '         Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            '             Call the recent file list procedure
            '       GetRecentFiles
        End If
    End Sub

    Public Sub mnuDocumentsLetterWriting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentsLetterWriting.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub


    Public Sub mnuGoToAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToAccounts.Click

        Try

            ' Call OrionLinkFunc function
            'eck220800
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGotoAccounts, v_sShortName:=m_sShortName, v_sInsuranceRef:=m_sInsReference), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoAccounts menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoAccounts_Click", excep:=excep)

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
        'VB 03/03/2005 PN19085 Documaster implemented
        m_lReturn = CType(objCM.ShowDocumaster(v_sLinkCode:=m_sInsReference & objCM.DME_POLICY), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Continue as not serious
            Exit Sub
        End If
    End Sub

    Public Sub mnuGoToEvents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToEvents.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:="", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sPolicyDesc:=m_sInsReference), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.15)
    Public Sub mnuGotoInsuredAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoInsuredAccounts.Click

        Const kMethodName As String = "mnuGotoInsuredAccounts_Click"
        'Sankar - PN 55197 - Added m_sInsReference
        m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sInsuranceRef:=m_sInsReference, v_sShortName:=m_sShortName), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessOrionFunc Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.15)

    Public Sub mnuGotoNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoNotes.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNamePolicy, v_lEntityCnt:=m_lInsuranceFileCnt, v_sTextType:="Public"), gPMConstants.PMEReturnCode)

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
            m_lReturn = CType(objCM.ShowPolicy(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGoToRisk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToRisk.Click

        Dim vRiskCodeId As Object
        Dim lRiskCodeId As Integer
        Dim vRiskGroupId As Object
        Dim lRiskGroupId As Integer

        Try

            'If we're here we must be underwriting...

            m_lReturn = CType(objCM.ShowListofRisks(v_lPartyCnt:=PartyCnt, v_sShortName:=ShortName, v_sPartyType:="X", v_sResolvedName:="Resolved", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId, v_sInsuranceRef:=m_sInsReference, v_bFromEvent:=False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGoToSwift_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToSwift.Click

        ' CTAF 030300
        MessageBox.Show("This functionality is yet to be implemented.", "Swift", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Exit Sub

    End Sub

    Public Sub mnuGoToTextFiles_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToTextFiles.Click

        Dim vRiskCodeId As String = ""
        Dim vRiskGroupId As Object
        Dim lRiskCodeId, lRiskGroupId As Integer

        Try

            vRiskCodeId = uctPMUPolicySummary1.RiskCodeId


            vRiskGroupId = uctPMUPolicySummary1.RiskGroupId


            If Convert.IsDBNull(vRiskCodeId) Or IsNothing(vRiskCodeId) Then
                lRiskCodeId = 0
            Else
                lRiskCodeId = CInt(vRiskCodeId)
            End If


            If Convert.IsDBNull(vRiskGroupId) Or IsNothing(vRiskGroupId) Then
                lRiskGroupId = 0
            Else

                lRiskGroupId = CInt(vRiskGroupId)
            End If

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.ShowTextFiles(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="X", v_sResolvedName:="Resolved", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId), gPMConstants.PMEReturnCode)

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
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGotoTransactionCash, v_sShortName:=m_sShortName), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionCash menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionCash_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub
    'EK 22/9/99
    Public Sub mnuGotoTransactionCredit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionCredit.Click
        Try

            ' Call OrionLinkFunc function
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionCredit, v_lInsuranceFileCnt:=m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionCredit menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionCredit_Click", excep:=excep)

            Exit Sub
        End Try

    End Sub
    'EK 22/9/99
    Public Sub mnuGotoTransactionDebit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionDebit.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionDebit, v_lInsuranceFileCnt:=m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionDebit menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionDebit_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub
    'eck080800
    Public Sub mnuGoTOTRansactionAJ_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoTOTRansactionAJ.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGoToTransactionAJ, v_lInsuranceFileCnt:=m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionAJ menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionAJ_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub
    Public Sub mnuGoToTRansactionAJReversal_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToTRansactionAJReversal.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGoToTransactionAJReversal, v_lInsuranceFileCnt:=m_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionAJReversal menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionAJReversal_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        m_lReturn = CType(objCM.ShowSBOAbout(), gPMConstants.PMEReturnCode)

    End Sub

    Public Sub mnuPolicyDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPolicyDelete.Click

        Dim bThere As Boolean

        Try

            'It may turn out that we don't care about this, and we just close them afterward
            'It's a business flow decision left until this is system tested

            'Check that the risk screen isn't open
            m_lReturn = CType(objCM.CheckRisk(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_lRiskGroupID:=uctPMUPolicySummary1.RiskGroupId, v_bFromEvent:=False, r_bThere:=bThere), gPMConstants.PMEReturnCode)

            If bThere Then
                MessageBox.Show("You must close risk screen before deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'Check that the text screen isn't open
            m_lReturn = CType(objCM.CheckText(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_bFromEvent:=False, r_bThere:=bThere), gPMConstants.PMEReturnCode)

            If bThere Then
                MessageBox.Show("You must close text screen before deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            'Check that the event screen isn't open
            m_lReturn = CType(objCM.CheckEvent(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_bThere:=bThere), gPMConstants.PMEReturnCode)

            If bThere Then
                MessageBox.Show("You must close event screen before deleting", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Call DeleteClick function
            m_lReturn = uctPMUPolicySummary1.DeleteClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Update the policy window
            m_lReturn = CType(objCM.m_ofrmMDI.RefreshPolicies(), gPMConstants.PMEReturnCode)

            Me.Hide()

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuPolicyDelete menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPolicyDelete_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuPolicyExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPolicyExit.Click
        Me.Hide() 'CT 31/08/00
    End Sub

    Private Sub frmPolicySummaryUnderwriting_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctPMUPolicySummary1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub

    Private Sub mnuGoToSharePoint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuGoToSharePoint.Click
        Dim sOption, sSPUrl, sDocLIB As String
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
            sDocLIB = sDocumentLibrary
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return
            End If

            If String.IsNullOrEmpty(sDocLIB) Then
                m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=5086, r_sOptionValue:=sDocLIB, v_iSourceID:=g_iSourceID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return
                End If
            End If
        End If
        System.Diagnostics.Process.Start(sSPUrl & "\" & sDocLIB & "\" & m_sShortName.Trim() & "\Policy\" & m_sInsReference)
    End Sub
End Class
