Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.IO

Partial Friend Class frmPartyGCView
    Inherits System.Windows.Forms.Form
    '
    ' History:
    ' CJB080805 - PN23013 - Changed LoadInterface to prevent access to view claims if no access given.
    '
    ' ***************************************************************** '
    ' Form Name: frmPartyGCView
    '
    ' Date: 15/03/2001
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmPartyGCView"
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

    'DD 13/10/2003
    Private m_bFilterOutOtherBranches As Boolean
    Private m_bIsIncludeClosedBranchChecked As Boolean

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
    ' PRIVATE Property Procedures (End)
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

    ' PUBLIC Methods (Begin)

    Public Function LoadInterface() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sOptionValue As String = ""
        Dim vMultiCompany, vBranchLogon As Object
        'AR20050824 - PN24332
        Dim bPartyHasDataModel, bPartyHasCustomData As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DD 13/10/2003
            'Check these product options in Broking. If both set
            'then disable editing for Partys that don't belong to the
            'current Branch

            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vMultiCompany)


            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, v_vBranch:=1, r_vUnderwriting:=vBranchLogon)

            m_bFilterOutOtherBranches = (gPMFunctions.NullToString(vBranchLogon) = "1" And gPMFunctions.NullToString(vMultiCompany) = "1")

            If m_bFilterOutOtherBranches And g_iSourceID <> uctPartyGCControl1.PartySourceID Then
                cmdEdit.Enabled = False
                mnuTransaction.Available = False
            Else
                ' Alix - 30/05/2003 - Only show edit button if system option says so.

                m_lReturn = iPMFunc.GetSystemOption(5000, sOptionValue, g_iSourceID)

                cmdEdit.Enabled = sOptionValue = "1"

                'Party View
                If objCM.g_bIsViewOnlyClientManager Then
                    cmdEdit.Enabled = False
                End If
            End If

            'AR20050824 - PN24332
            If PartyBuilderHandler.g_oObjectManager Is Nothing Then
                PartyBuilderHandler.g_oObjectManager = objCM.g_oObjectManager
            End If
            m_lReturn = PartyBuilderHandler.GetPartyBuilderFlags(m_lPartyCnt, bPartyHasDataModel, bPartyHasCustomData)

            If uctPartyGCControl1.ShortName.Trim() = String.Empty Then
                m_sShortName = objCM.m_sShortName
            Else
                m_sShortName = uctPartyGCControl1.ShortName.Trim()
            End If

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

            ' /Alix
            '2005 Client Manager Security
            If Not objCM.g_bEditClientAuthority Then
                cmdEdit.Enabled = False
            End If
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



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    Private Sub cmdCustom_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCustom.Click

        'AR20050824 - PN24332
        PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)
        'The Party-PartyBuilder link will now exist, so set the picture to a tick
        'Set Me.picIndicator.Picture = Me.imgCustomData.ListImages("TICK").ExtractIcon
        MyBase.ParentForm.Focus()
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        'open another document

        m_lReturn = uctPartyGCControl1.EditClick()

        ' Check the return value.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lReturn = objCM.OpenFile(vPartyCnt:=m_lPartyCnt, vPartyShortName:=m_sShortName, vPartyType:=m_sPartyType, vPartyResolvedName:=m_sResolved, bIsIncludeClosedBranchchecked:=m_bIsIncludeClosedBranchChecked)

        m_bEditing = True

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Me.Hide()
        End If

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        '        m_lReturn& = ShowHelp(dlgHelp,objCM. ScreenHelpID)
        ' Click event of the Cancel button.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Process the next set of actions depending
        ' upon the interface task etc.
        'm_lReturn& = uctPartyGCControl1.ShowHelpScreen
        '
        ' Check the return value.
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error.
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim iLen As Integer

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Everything OK, so we can hide the interface.
            Me.Close()


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
            m_lReturn = uctPartyGCControl1.CancelClick()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Developer Guide No. 231
                Me.Close()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)

    Private Sub frmPartyGCView_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            m_parentMdiForm.StatusBar1.Items.Item(0).Text = Me.Footer
            m_parentMdiForm.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            m_parentMdiForm.StatusBar1.Items.Item(2).Text = Me.ShortName
            m_parentMdiForm.ShortName = Me.ShortName 'ADDED MK 991014
            m_parentMdiForm.ResolvedName = Me.ResolvedName 'ADDED MK 991014
            m_parentMdiForm.PartyType = Me.PartyType 'ADDED MKR PN 17193
            m_parentMdiForm.Text = "[" & Me.ResolvedName.Trim() & "] Sirius Client Manager"
            Me.Text = "View : [" & Me.ResolvedName.Trim() & "]"



            'sj 03/07/2002 - start
            If objCM.g_bRestrictInsurerAccess Then
                m_lReturn = objCM.SetRestrictedToolbar(v_sFormName:=Me.Name)
            Else
                m_lReturn = objCM.SetToolbar(v_sFormName:=Me.Name)
            End If
            'sj 03/07/2002 - end
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


    Private Sub frmPartyGCView_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lStatus As Integer
        Dim sTemp As String = ""
        Dim iLoop1 As Integer
        Dim sOption As String
        '        ' Forms load event.

        Try

            m_bEditing = False

            m_iTask = gPMConstants.PMEComponentAction.PMView

            VB6.SetDefault(cmdOK, True)

            'eck19052005
            '    Me.Height = 6000
            '    Me.Width = 9435
            'Me.Height = VB6.TwipsToPixelsY(8055)
            'Me.Width = VB6.TwipsToPixelsX(10950)

            With uctPartyGCControl1
                'Developer Guide No.  24(Guide)
                .Task = m_iTask
                'Developer Guide No.  24(Guide)
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                .PartyCnt = m_lPartyCnt
                .IsIncludeClosedBranchChecked = m_bIsIncludeClosedBranchChecked
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
            '    For iLoop1% = 2 To g_iMaxRecent%
            '        Load mnuRecentFile(iLoop1%)
            '        With mnuRecentFile(iLoop1%)
            '            .Caption = "RecentFile" & CStr(iLoop1%)
            '            .Visible = False
            '        End With
            '    Next iLoop1%
            '
            ' PW170602 - set up menus depending on app type
            '

            mnuReportsPolicyList.Available = False
            mnuReportsStatement.Available = False
            'Kevin Renshaw (CMG) 24/03/2003 - remove Broking functionality
            mnuDocumentRiskRegister.Available = False
            mnuDocumentsMarket.Available = False

            'Thinh Nguyen 27/06/2003 (start) - PN5049 do not show Transaction menu for underwriting
            mnuTransaction.Available = False
            'Thinh Nguyen 27/06/2003 (start) - PN5049 do not show Transaction menu for underwriting

            ' Alix - 20/01/2003 - PN9811
            ' Hide broking functionnality
            mnuGotoiMarket.Available = False
            mnuGoToStickyNotes.Available = False

            mnuReportsClientSummary.Available = False

            m_lReturn = objCM.LoadRecentFilesFromReg()
            m_lReturn = objCM.LoadRecentFiles(r_oForm:=Me)

            'sj 03/07/2002 - start
            'Restricted access for users who are tagged as insurers
            If objCM.g_bRestrictInsurerAccess Then
                cmdEdit.Visible = False
                '        mnuClientCopy.Visible = False
                '        mnuClientMove.Visible = False
                mnuGoToAccounts.Available = False
                mnuTransaction.Available = False
                mnuGoToClaim.Available = False
                mnuGoToDocumaster.Available = False
                mnuGoToFinancePlan.Available = False
                mnuGoToStickyNotes.Available = False
                mnuGoToSwift.Available = False
                mnuGoToTextFiles.Available = False
                mnuGotoNotes.Available = False
                mnuDocuments.Available = False
                mnuReportS.Available = False
                mnuTasks.Available = False
            End If
            'sj 03/07/2002 - end

            'sj 04/10/2002 - start
            If objCM.g_bHidePublicPrivateNotes Then
                mnuGotoNotes.Available = False
            End If
            'sj 04/10/2002 - end
            'eck Datasure only show Imarket link for UK
            If objCM.g_iCountryID <> 1 Then
                mnuGotoiMarket.Available = False
            End If
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

    Private Sub frmPartyGCView_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint


        'MKR 29/09/2004 PN 6021 'We are now using '|' as a delimiter rather than ','
        Dim vFileName As String = Me.ShortName & "|" & Me.ResolvedName & "|" & CStr(Me.PartyCnt) & Me.PartyType

        ' Update the recent files menu
        m_lReturn = objCM.UpdateFileMenu(vFileName:=vFileName)

    End Sub

    Private Sub frmPartyGCView_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
                m_lReturn = uctPartyGCControl1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If
            'sj 19/06/2002 - start
            If Not m_bEditing Then
                ' Terminate the control
                uctPartyGCControl1.Dispose()
            End If
            'sj 19/06/2002 - end
            '    ' Terminate the control
            '    m_lReturn& = uctPartyGCControl1.Terminate()
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to terminate the uctPartyGCControl.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_QueryUnload", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '    End If

            'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True

            'Don't reset these if editing, as the other form is already activated and
            'so won't be updating them...
            If Not m_bEditing Then

                m_parentMdiForm.StatusBar1.Items.Item(0).Text = ""
                m_parentMdiForm.StatusBar1.Items.Item(1).Text = ""
                m_parentMdiForm.StatusBar1.Items.Item(2).Text = ""
                m_parentMdiForm.Text = "Sirius Client Manager"

            End If

            '2005 Close sticky notes
            '2005 Close sticky notes
            If Not m_bEditing Then
                If Information.IsArray(objCM.g_vWarnings) Then

                    For iLoop1 As Integer = 0 To objCM.g_vWarnings.GetUpperBound(1)
                        'ContainerHelper.UnloadControl(Me, "Document", objCM.g_vWarnings(0, iLoop1))
                        If objCM.Document(objCM.g_vWarnings(0, iLoop1)).Name <> Me.Name Then
                            objCM.Document(objCM.g_vWarnings(0, iLoop1)).Close()
                        End If
                    Next iLoop1
                    'Developer Guide No. 146(Guide)
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

            'For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
            '    If Application.OpenForms.Item(iLoop1).IsMdiChild Then
            '        If Application.OpenForms.Item(iLoop1).GetType.ToString <> Me.GetType.ToString Then
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
                'sj 03/07/2002 - start
                If objCM.g_bRestrictInsurerAccess Then
                    m_lReturn = objCM.SetRestrictedToolbar(v_sFormName:=m_parentMdiForm.Name)
                Else
                    m_lReturn = objCM.SetToolbar(v_sFormName:=m_parentMdiForm.Name)
                End If
                'sj 03/07/2002 - end
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
    Private Sub frmPartyGCView_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            'eck19052005
            '       'sj 19/06/2002 - start
            '       ' Me.Height = 6000
            '        Me.Height = 6705
            '       'sj 19/06/2002 - end
            '        Me.Width = 9435
            Me.Height = 529 'VB6.TwipsToPixelsY(8055)
            Me.Width = 738 'VB6.TwipsToPixelsX(10950)
        End If

    End Sub

    Private Sub frmPartyGCView_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
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

    'Start - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    'UPGRADE_NOTE: (7001) The following declaration (mnuCashDeposit_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub mnuCashDeposit_Click()
    'Dim iSIRCashDeposit As Object
    'Const kMethodName As String = "CallCashDeposit"

    'Dim oCashDeposit As iSIRCashDeposit.Interface_Renamed
    'Dim temp_oCashDeposit As Object
    'm_lReturn = objCM.g_oObjectManager.GetInstance(temp_oCashDeposit, sClassName:="iSIRCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'oCashDeposit = temp_oCashDeposit
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("mnuGotoCashDeposit_Click", "Failed to create a Instance for iSIRCashDeposit.Interface", gPMConstants.PMELogLevel.PMLogError)
    'End If

    'CType(oCashDeposit, SSP.S4I.Interfaces.ILocalInterface).Initialise()

    'oCashDeposit.PartyCode = m_sShortName

    'oCashDeposit.PartyCnt = m_lPartyCnt

    'oCashDeposit.PartyName = m_sResolved

    'oCashDeposit.Task = gPMConstants.PMEComponentAction.PMView

    'oCashDeposit.FromAgentOrClientMaintenance = True

    'oCashDeposit.Start()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Start Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '

    'm_lReturn = oCashDeposit.Terminate()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, "Terminate Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'End Sub
    'End - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

    Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
        'Developer Guide No. 231
        Me.Close() 'CT 31/08/00
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

    Public Sub mnuFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFind.Click

        m_lReturn = objCM.ShowTaskList(v_lPartyCnt:=PartyCnt)

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

    'Start - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    Public Sub mnuGotoCashDeposit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoCashDeposit.Click
        Dim iSIRCashDeposit As Object

        Const kMethodName As String = "CallCashDeposit"

        Dim oCashDeposit As iSIRCashDeposit.Interface_Renamed
        Dim temp_oCashDeposit As Object
        m_lReturn = objCM.g_oObjectManager.GetInstance(temp_oCashDeposit, sClassName:="iSIRCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oCashDeposit = temp_oCashDeposit
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("mnuGotoCashDeposit_Click", "Failed to create a Instance for iSIRCashDeposit.Interface", gPMConstants.PMELogLevel.PMLogError)
        End If

        CType(oCashDeposit, SSP.S4I.Interfaces.ILocalInterface).Initialise()

        oCashDeposit.PartyCode = m_sShortName

        oCashDeposit.PartyCnt = m_lPartyCnt

        oCashDeposit.PartyName = m_sResolved

        oCashDeposit.FromAgentOrClientMaintenance = True

        oCashDeposit.Task = gPMConstants.PMEComponentAction.PMView


        oCashDeposit.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Start Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        oCashDeposit.Dispose()

        oCashDeposit = Nothing

    End Sub
    'End - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

    Public Sub mnuGoToClaim_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToClaim.Click

        ' CTAF 030300
        'DC 101100 now implemented
        'MsgBox "This functionality is yet to be implemented.", vbInformation, "Claims"
        'Exit Sub

        'DC 101100 process 'goto' claim
        Try

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

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.7)
    Public Sub mnuGotoInsuredAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoInsuredAccounts.Click

        Const kMethodName As String = "mnuGotoInsuredAccounts_Click"

        m_lReturn = objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sShortName:=m_sShortName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessOrionFunc Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.7)

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

    Public Sub mnuNewDiary_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNewDiary.Click

        m_lReturn = objCM.ProcessTasks(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:=m_sResolved, v_lInsuranceFolderCnt:=0, v_lInsuranceFileCnt:=0, v_sPolicyDesc:="")

    End Sub

    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click, _mnuRecentFile_5.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)

        m_lReturn = objCM.ShowRecentFile(iIndex:=Index, r_oForm:=Me)

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


    'UPGRADE_NOTE: (7001) The following declaration (mnuReportStatements_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub mnuReportStatements_Click()
    '
    'End Sub


    Public Sub mnuReportsStatements_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReportsStatements.Click
        '
        ' PW170602 - add underwriting version of report
        '
        m_lReturn = objCM.RunReport(v_lPartyCnt:=PartyCnt, v_sReportName:="Client_Statement_By_PartyCnt_U")

    End Sub

    Private Sub frmPartyGCView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 3
        End If
        If e.Alt And e.KeyCode = Keys.D5 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 4
        End If
        If e.Alt And e.KeyCode = Keys.D6 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 5
        End If
        If e.Alt And e.KeyCode = Keys.D7 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 6
        End If
        If e.Alt And e.KeyCode = Keys.D8 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 7
        End If
        If e.Alt And e.KeyCode = Keys.D9 Then
            DirectCast(uctPartyGCControl1.Controls("tabMainTab"), TabControl).SelectedIndex = 8
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

    Private Sub cmdExtractClientData_Click(sender As Object, e As EventArgs) Handles cmdExtractClientData.Click

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nPasswordAction As PMEReturnCode = PMEReturnCode.PMCancel
        Dim sPassword As String = String.Empty
        Dim sFilePath As String = String.Empty
        Dim sFileName As String = String.Empty
        Dim oPasswordForm As frmPassword
        Dim oGis As bGIS.Application = Nothing
        Try
            'Get the password
            oPasswordForm = New frmPassword()
            iPMFunc.CenterForm(oPasswordForm)

            oPasswordForm.ShowDialog()
            sPassword = oPasswordForm.txtPassword.Text.Trim()
            nPasswordAction = oPasswordForm.Status

            oPasswordForm.Close()
            oPasswordForm = Nothing

            If nPasswordAction <> gPMConstants.PMEReturnCode.PMOK Then
                nResult = gPMConstants.PMEReturnCode.PMCancel
                Exit Sub
            End If

            oGis = New bGIS.Application
            nResult = objCM.g_oObjectManager.GetInstance(oObject:=oGis, sClassName:="bGIS.Application", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            ' Check for errors.
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGIS Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExtractClientData_Click")
                MessageOnExtractFailure()
            End If

            Using sfdExtractClientData As New SaveFileDialog()
                sfdExtractClientData.Title = "Save As"
                sfdExtractClientData.AddExtension = True
                sfdExtractClientData.OverwritePrompt = True
                sfdExtractClientData.CheckPathExists = True
                sfdExtractClientData.ValidateNames = True
                sfdExtractClientData.FileName = "ClientDataExtract"
                sfdExtractClientData.Filter = "Zip Files|*.zip"
                sfdExtractClientData.DefaultExt = ".zip"
                If sfdExtractClientData.ShowDialog() = DialogResult.OK Then
                    sFilePath = sfdExtractClientData.FileName
                Else
                    Exit Sub
                End If

                Dim abExtractedClientData As Byte() = Nothing
                nResult = oGis.ExtractData(PartyCnt(), sPassword, abExtractedClientData)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGIS ExtractData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdExtractClientData_Click")
                    MessageOnExtractFailure()
                End If
                If abExtractedClientData Is Nothing OrElse abExtractedClientData.Length = 0 Then
                    MessageOnExtractFailure()
                Else
                    File.WriteAllBytes(sFilePath, abExtractedClientData)
                End If

            End Using
        Catch ex As Exception
            MessageOnExtractFailure(ex.Message)
        Finally
            If oGis IsNot Nothing Then
                oGis.Dispose()
                oGis = Nothing
            End If
        End Try
    End Sub

    Private Sub MessageOnExtractFailure(Optional ByVal sAdditionalMsg As String = "")
        MessageBox.Show(String.Format("There was an error generating data file. Please try again.{0}", Environment.NewLine & sAdditionalMsg),
                        "Extract File", MessageBoxButtons.OK)
    End Sub
End Class