Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No.129
Imports SharedFiles
Imports uctListEventsControl

Partial Friend Class frmListEvents
    Inherits System.Windows.Forms.Form
    '*******************************************************************************
    ' Form Name: frmListEvents
    '
    ' Date: 20/07/1999
    '
    ' Description: Events list interface.
    '
    ' History: TF031298 - Menu & Toolbar activity
    '
    '          PW081204 - PN17216 - deal with the event type of FSA
    '                     Product disclosure.
    '
    '          PW091204 - PN17202 - change "recent file" processing to use the
    '                     standard modular function. This is so the DPA questions
    '                     will only be asked when required.
    '
    '          CJB180705  PN22457 Changed vvwSearchDetailsDblClick to do nothing if
    '                     'Renewal' event type and no ins file cnt
    '
    '          CJB020805- PN22835 - changed uctListEvents1_lvwSearchDetailsClick to cater
    '                     for all complaint type events and enable the view button for them.
    '
    '*******************************************************************************


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmListEvents"
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

    Private m_sFooter As String = ""
    Private m_sPartyType As String = ""
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sResolvedName As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sPolicyDesc As String = ""
    Private m_lClaimCnt As Integer
    Private m_sClaimDesc As String = ""
    Private m_lEventCnt As Integer
    Private m_sEventType As String = ""
    Private m_lPolicyTypeID As Integer
    ''sj 03/10/2002 - start
    'Private uctControl As VBControlExtender
    ''sj 03/10/2002 - end
    '
    ''sj 03/10/2002 - start
    'Public Property Get uctPartyCCControl1() As Object
    '    Set uctListEvents1 = uctControl.object
    'End Property
    ''sj 03/10/2002 - end
    ''Extras for PMB

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

    Public Property PartyType() As String
        Get

            Return m_sPartyType

        End Get
        Set(ByVal Value As String)

            m_sPartyType = Value

        End Set
    End Property

    Public Property ResolvedName() As String
        Get

            Return m_sResolvedName

        End Get
        Set(ByVal Value As String)

            m_sResolvedName = Value

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

    Public Property PolicyDesc() As String
        Get

            Return m_sPolicyDesc

        End Get
        Set(ByVal Value As String)

            m_sPolicyDesc = Value

        End Set
    End Property

    Public Property ClaimCnt() As Integer
        Get

            Return m_lClaimCnt

        End Get
        Set(ByVal Value As Integer)

            m_lClaimCnt = Value

        End Set
    End Property

    Public Property ClaimDesc() As String
        Get

            Return m_sClaimDesc

        End Get
        Set(ByVal Value As String)

            m_sClaimDesc = Value

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

    'Private Sub cmdEdit_Click()
    '
    '    'open another document
    '
    '    m_lReturn = OpenFile(vPartyCnt:=m_lPartyCnt, _
    ''                         vPartyShortName:=m_sShortName, _
    ''                         vPartyType:=m_sPartyType, _
    ''                         vPartyResolvedName:=m_sResolved)
    '
    '    m_bEditing = True
    '
    '    If (m_lReturn = PMTrue) Then
    '        Unload Me
    '    End If
    '
    'End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        '        m_lReturn& = ShowHelp(dlgHelp,objCM. ScreenHelpID)
        ' Click event of the Cancel button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = uctListEvents1.ShowHelpScreen(cmdHelp, lContextID:=MainModule.ScreenHelpID)

            ' Check the return value.

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'sj 30/09/2002 - start
    'Private Sub cmdNotes_Click()
    '
    '    On Error GoTo Err_cmdNotes_Click
    '
    '    If uctListEvents1.EventCnt > 0 Then
    '        m_lReturn& = objcm.CallNotes("Event", uctListEvents1.EventCnt, "Public")
    '
    '        If m_lReturn& = PMTrue Then
    '            m_lReturn& = uctListEvents1.GetBusiness
    '        End If
    '
    '    End If
    '
    'Exit Sub
    '
    'Err_cmdNotes_Click:
    '
    '' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to process the notes command button", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="cmdNotes_Click", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub
    'sj 30/09/2002 - end

    Private Sub cmdNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNotes.Click

        Try

            If uctListEvents1.EventCnt > 0 Then
                If uctListEvents1.EventType <> "N_WARN" Then
                    m_lReturn = CType(objCM.CallNotes("Event", uctListEvents1.EventCnt, "Public"), gPMConstants.PMEReturnCode)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = uctListEvents1.GetBusiness()
                    End If
                End If
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the notes command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNotes_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'sj 01/10/2002 - start
    Private Sub cmdNotesAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNotesAdd.Click
        m_lReturn = uctListEvents1.AddClick()
    End Sub

    Private Sub cmdNotesView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNotesView.Click
        m_lReturn = uctListEvents1.ViewClick()
    End Sub
    'sj 01/10/2002 - end

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim iLen As Integer

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'SJ says OK exits - just like everything else
            '    m_lReturn& = uctListEvents1.OKClick
            '
            '    ' Check the return value.
            '    If (m_lReturn& = PMTrue) Then
            '        ' Everything OK, so we can hide the interface.
            '
            '        With uctListEvents1
            '        'Don't forget to do this right
            '            m_lReturn& = objCM.ShowEventDetail(v_lEventCnt:=.EventCnt, _
            ''                                         v_sPartyType:=m_sPartyType, _
            ''                                         v_lPartyCnt:=m_lPartyCnt, _
            ''                                         v_sShortName:=m_sShortName, _
            ''                                         v_sResolvedName:=m_sResolvedName, _
            ''                                         v_lInsuranceFileCnt:=.InsuranceFileCnt, _
            ''                                         v_sInsReference:=.PolicyDesc, _
            ''                                         v_lInsuranceFileStructureId:=.InsuranceFileCnt, _
            ''                                         v_lClaimCnt:=.ClaimCnt, _
            ''                                         v_sClaimDesc:=.ClaimDesc, _
            ''                                         v_lOldAddressCnt:=.OldAddressCnt, _
            ''                                         v_lNewAddressCnt:=.NewAddressCnt, _
            ''                                         v_lDocumentCnt:=.DocumentCnt, _
            ''                                         v_sEventType:=.EventType, _
            ''                                         v_lPolicyTypeId:=m_lPolicyTypeId)
            '
            '        End With
            '
            '        'Me.Hide
            '        'Unload Me
            '
            '    End If

            'Me.Close()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Close()

                If IsArray(objCM.g_vWarnings) = True Then
                    For m_iIndex = 0 To UBound(objCM.g_vWarnings, 2)
                        objCM.Document(objCM.g_vWarnings(0, m_iIndex)).Close()
                    Next m_iIndex
                    objCM.g_vWarnings = ""
                End If

                objCM.OpenWarnings(m_lPartyCnt)

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
            m_lReturn = uctListEvents1.CancelClick()

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


    ' PRIVATE Events (End)

    Private Sub frmListEvents_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            Me.Text = "Event List : ["
            If m_sClaimDesc.Length <> 0 Then
                Me.Text = Me.Text & Me.ClaimDesc.Trim() & "]"
            ElseIf (Strings.Len(Me.PolicyDesc) <> 0) Then
                Me.Text = Me.Text & Me.PolicyDesc.Trim() & "]"
            Else
                Me.Text = Me.Text & Me.ResolvedName.Trim() & "]"
            End If
            objCM.m_ofrmMDI.Text = Me.Text & " Sirius Client Manager"

            m_lReturn = CType(objCM.SetToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If

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


    Private Sub frmListEvents_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lStatus As Integer
        Dim sTemp As String = ""
        Dim iLoop1 As Integer

        '
        '
        '        ' Forms load event.
        '
        Try

            Me.Height = VB6.TwipsToPixelsY(6360)
            Me.Width = VB6.TwipsToPixelsX(9360)

            m_iTask = gPMConstants.PMEComponentAction.PMEdit

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            '    ' Add the PartyPC user control
            '    m_lReturn& = CreateUserControl( _
            ''                    v_sProgID:="uctListEventsControl.uctListEvents", _
            ''                    v_sObjectName:="uctListEvents1", _
            ''                    v_oForm:=Me, _
            ''                    r_oContainer:=picHolder, _
            ''                    r_oControl:=uctControl)
            '    If (m_lReturn& <> PMTrue) Then
            '        SetMousePointer PMMouseNormal
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to create user control - uctPartyCCControl.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_Load", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '    End If

            With uctListEvents1
                .Task = m_iTask
                'Developer Guide No.24
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
            End With
            'Developer Guide No.9
            m_lReturn = uctListEvents1.Initialise()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'Activate is not always called to start with, so let's put this here
            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = CStr(Me.PartyCnt)
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            Me.Text = "Event List : ["
            If m_sClaimDesc.Trim() <> "" Then
                Me.Text = Me.Text & Me.ClaimDesc.Trim() & "]"
            ElseIf (Me.PolicyDesc.Trim() <> "") Then
                Me.Text = Me.Text & Me.PolicyDesc.Trim() & "]"
            Else
                Me.Text = Me.Text & Me.ResolvedName.Trim() & "]"
            End If

            uctListEvents1.PartyCnt = m_lPartyCnt
            uctListEvents1.InsuranceFolderCnt = m_lInsuranceFolderCnt
            uctListEvents1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctListEvents1.ClaimCnt = m_lClaimCnt
            uctListEvents1.PolicyDesc = m_sPolicyDesc
            uctListEvents1.ClaimDesc = m_sClaimDesc
            'SJ 24/02/2004 - start
            '    uctListEvents1.ShortName = m_sShortName
            'SJ 24/02/2004 - end
            m_lReturn = uctListEvents1.LoadControl()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the user control.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = uctListEvents1.GetEvents()

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get business details.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            lStatus = uctListEvents1.Status

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Get the number of recent files
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="MaxMRU", r_sSettingValue:=sTemp), gPMConstants.PMEReturnCode)
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

            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = CType(objCM.LoadRecentFiles(r_oForm:=Me), gPMConstants.PMEReturnCode)


            'sj 04/10/2002 - start
            If objCM.g_bHidePublicPrivateNotes Then
                mnuGoto.Available = False
            End If
            'sj 04/10/2002 - end

        Catch excep As System.Exception



            ' Error Section.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmListEvents_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
                m_lReturn = uctListEvents1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    'Developer Guide No. 7
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If

            End If

            'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True

            'Don't reset these if editing, as the other form is already activated and
            'so won't be updating them...
            '    If Not m_bEditing Then
            '
            '        objCM.m_ofrmMDI.StatusBar1.Panels(1).Text = ""
            '        objCM.m_ofrmMDI.StatusBar1.Panels(2).Text = ""
            '        objCM.m_ofrmMDI.StatusBar1.Panels(3).Text = ""
            '        objCM.m_ofrmMDI.Caption = "Policy Master Client Manager"
            '
            '    End If

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmListEvents_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        'Code to resize uctListEvents and other controls
        uctListEvents1.Width = Me.Width - VB6.TwipsToPixelsX(475)
        If VB6.PixelsToTwipsY(Me.Height) >= 1185 Then
            uctListEvents1.Height = Me.Height - VB6.TwipsToPixelsY(1300)
        End If
        cmdNotesView.Top = uctListEvents1.Top + uctListEvents1.Height
        cmdNotesAdd.Top = uctListEvents1.Top + uctListEvents1.Height
        cmdNotes.Top = uctListEvents1.Top + uctListEvents1.Height
        cmdOK.Top = uctListEvents1.Top + uctListEvents1.Height
        cmdCancel.Top = uctListEvents1.Top + uctListEvents1.Height
        cmdHelp.Top = uctListEvents1.Top + uctListEvents1.Height

    End Sub

    Private Sub frmListEvents_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        ' Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            ' Call the recent file list procedure
            'GetRecentFiles
            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = CType(objCM.LoadRecentFiles(r_oForm:=Me), gPMConstants.PMEReturnCode)

        End If

    End Sub

    Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
        'CT 31/08/00 bugfix 379
        cmdCancel.Focus()
        cmdCancel_Click(cmdCancel, New EventArgs())
    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        m_lReturn = CType(objCM.ShowSBOAbout(), gPMConstants.PMEReturnCode)

    End Sub

    Public Sub mnuNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNotes.Click

        Try
            'PN 15512 called the correct function to display Policy Note
            '         or Client Notes based on the m_lInsuranceFileCnt
            ' Call Toolbar Control function
            If m_lInsuranceFileCnt = 0 Then
                m_lReturn = CType(objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNameParty, v_lEntityCnt:=m_lPartyCnt, v_sTextType:="Public"), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNamePolicy, v_lEntityCnt:=m_lInsuranceFileCnt, v_sTextType:="Public", v_lPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)
            End If
            m_lReturn = RefreshList()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click, _mnuRecentFile_2.Click, _mnuRecentFile_3.Click, _mnuRecentFile_4.Click, _mnuRecentFile_5.Click
        Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)

        ' Use the standard modular function to show a recent file. PN17202
        m_lReturn = CType(objCM.ShowRecentFile(iIndex:=Index, r_oForm:=Me), gPMConstants.PMEReturnCode)

    End Sub



    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Description: Refreshes the data on the form
    '
    ' ***************************************************************** '
    Public Function RefreshList() As Integer

        Dim result As Integer = 0
        Try

            ' Call GetEvents on the user control

            Return uctListEvents1.GetEvents()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

            If Me.Visible Then

                ' Set the focus
                Me.Activate()

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub uctListEvents1_lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As uctListEvents.lvwSearchDetailsClickEventArgs) Handles uctListEvents1.lvwSearchDetailsClick

        Select Case e.sEventType
            Case gSIRLibrary.ACNotesCustomer, gSIRLibrary.ACNotesAccount, gSIRLibrary.ACNotesClaims, gSIRLibrary.ACNotesPolicy, gSIRLibrary.ACNotesFSA, gSIRLibrary.ACNotesWarning, gSIRLibrary.ACEventFSAProductDisclosure, gSIRLibrary.ACNotesFSAGIIDisclosure, gSIRLibrary.ACNotesFSAGIIDN, gSIRLibrary.ACGeneralComplaint, gSIRLibrary.ACClaimComplaint, gSIRLibrary.ACPolicyComplaint 'PN22835
                cmdNotesView.Enabled = True
            Case Else
                cmdNotesView.Enabled = False
        End Select

    End Sub
    Private Sub uctListEvents1_lvwSearchDetailsKeyUp(ByVal Sender As Object, ByVal e As uctListEvents.lvwSearchDetailsKeyUpEventArgs) Handles uctListEvents1.lvwSearchDetailsKeyUp

        Select Case e.sEventType
            Case gSIRLibrary.ACNotesCustomer, gSIRLibrary.ACNotesAccount, gSIRLibrary.ACNotesClaims, gSIRLibrary.ACNotesPolicy, gSIRLibrary.ACNotesFSA, gSIRLibrary.ACNotesWarning, gSIRLibrary.ACEventFSAProductDisclosure, gSIRLibrary.ACNotesFSAGIIDisclosure, gSIRLibrary.ACNotesFSAGIIDN, gSIRLibrary.ACGeneralComplaint, gSIRLibrary.ACClaimComplaint, gSIRLibrary.ACPolicyComplaint 'PN22835
                cmdNotesView.Enabled = True
            Case Else
                cmdNotesView.Enabled = False
        End Select

    End Sub

    'JAS 10012005 PN17985 now using Folder not File cnt
    Private Sub uctListEvents1_lvwSearchDetailsDblClick(ByVal Sender As Object, ByVal e As uctListEvents.lvwSearchDetailsDblClickEventArgs) Handles uctListEvents1.lvwSearchDetailsDblClick

        Try

            Select Case e.sEventType
                ' Add Event for FSA Product Disclosure to list of those to
                ' view. PN17216
                'DC200405 PN20287 added GII Event Types
                Case gSIRLibrary.ACNotesCustomer, gSIRLibrary.ACNotesAccount, gSIRLibrary.ACClaimComplaint, gSIRLibrary.ACNotesClaims, gSIRLibrary.ACNotesPolicy, gSIRLibrary.ACNotesFSA, gSIRLibrary.ACNotesWarning, gSIRLibrary.ACEventFSAProductDisclosure, gSIRLibrary.ACNotesFSAGIIDisclosure, gSIRLibrary.ACNotesFSAGIIDN
                    m_lReturn = uctListEvents1.ViewClick()
                Case Else
                    ' Call Toolbar Control function
                    'JAS 10012005 PN17985 now using Folder not File cnt

                    ' If Renewals and no ins file cnt do nothing  PN22457
                    If Not (e.lInsuranceFileCnt = 0 And CBool(CStr(e.sEventType.ToUpper() = "RENEWAL").Trim())) Then


                        m_lReturn = objCM.ShowEventDetail(v_lEventCnt:=e.lEventCnt, v_sPartyType:=m_sPartyType, v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sResolvedName:=m_sResolvedName, v_lInsuranceFileCnt:=e.lInsuranceFileCnt, v_sInsReference:=e.sPolicyDesc, v_lInsuranceFileStructureId:=e.lInsuranceFileStructureId, v_lClaimCnt:=e.lClaimCnt, v_sClaimDesc:=e.sClaimDesc, v_lOldAddressCnt:=e.lOldAddressCnt, v_lNewAddressCnt:=e.lNewAddressCnt, v_lDocumentCnt:=e.lDocumentCnt, v_sEventType:=e.sEventType, v_lPolicyTypeId:=m_lPolicyTypeID, v_lOldPartyTypeID:=e.lOldPartyTypeID, v_sDocumentRef:=e.sDocumentRef, v_dtNoteDate:=e.dtNoteDate, v_lFsaComplaintFolderCnt:=e.lFSAComplaintFolderCnt)
                        'sj 15/09/2003 - Added v_lFsaComplaintFileCnt

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'Continue as not serious
                            Exit Sub
                        End If
                    End If
            End Select

        Catch


            'Continue as not serious
            Exit Sub
        End Try
    End Sub

    Private Sub frmListEvents_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctListEvents1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub
End Class
