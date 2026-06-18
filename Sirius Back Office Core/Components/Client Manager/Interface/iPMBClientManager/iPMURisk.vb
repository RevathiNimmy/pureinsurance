Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Friend Partial Class frmRiskUnderwriting
	Inherits System.Windows.Forms.Form
	Private Sub frmRiskUnderwriting_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

		End If
	End Sub
	
	
	' ***************************************************************** '
	' Form Name: frmRisk
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
	Private Const ACClass As String = "frmRisk"
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

    Private m_iIndex As Integer
    Private m_sShortName As String = ""
    Private m_sInsReference As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsFileCnt As Integer
    Private m_lPartyCnt As Integer
    Private m_sFooter As String = ""
    Private m_sResolvedName As String = ""
    Private m_sPartyType As String = ""
    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer
    Private m_lRiskGisScreenId As Integer
    Private m_lRiskTypeId As Integer

    Private m_bEvent As Boolean

    Private m_bEventRaised As Boolean

    'TN20000809
    Private m_bPMRaiseEvent As Boolean 'set to true to create event
    Private m_lPMRaiseEventState As Integer 'create event now or latter in parent object

    'MSS280901 - Added for merge
    'TN20000111
    Private m_lIsReInsuranceAtRiskLevel As gPMConstants.PMEReturnCode
    'MSS280901 - Merge end

    'DN 17/10/02

    Private m_oRiskTax As bSIRRITax.Business
    Private m_lRiskCnt As Integer
    Private m_vRITax As Object
    Private m_sDesc As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    'sj 19/08/2002 - start
    Private uctControl As AxHost

    'Public Property Get RiskScreen1() As Object
    '    Set RiskScreen1 = uctControl.object
    'End Property
    'sj 19/08/2002 - end

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

    'TN20010111 End
    'MSS280901 - Added for merge

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
    Public Property InsReference() As String
        Get

            Return m_sInsReference

        End Get
        Set(ByVal Value As String)

            m_sInsReference = Value

        End Set
    End Property
    Public Property ScreenId() As Integer
        Get

            Return m_lRiskGisScreenId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGisScreenId = Value

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

    Public Property InsFileCnt() As Integer
        Get

            Return m_lInsFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsFileCnt = Value

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

    Public Property Footer() As String
        Get

            Return m_sFooter

        End Get
        Set(ByVal Value As String)

            m_sFooter = Value

        End Set
    End Property

    Public Property RiskCodeId() As Integer
        Get

            Return m_lRiskCodeId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskCodeId = Value

        End Set
    End Property

    Public Property RiskGroupId() As Integer
        Get

            Return m_lRiskGroupId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGroupId = Value

        End Set
    End Property
    Public Property RiskTypeId() As Integer
        Get

            Return m_lRiskTypeId

        End Get
        Set(ByVal Value As Integer)

            m_lRiskTypeId = Value

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

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    Public Property EventRaised() As Boolean
        Get

            Return m_bEventRaised

        End Get
        Set(ByVal Value As Boolean)

            m_bEventRaised = Value

        End Set
    End Property

    Public Property PMRaiseEvent() As Boolean
        Get
            Return m_bPMRaiseEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bPMRaiseEvent = Value
        End Set
    End Property

    Public Property PMRaiseEventState() As Integer
        Get
            Return m_lPMRaiseEventState
        End Get
        Set(ByVal Value As Integer)
            m_lPMRaiseEventState = Value
        End Set
    End Property

    'MSS280901 - Added for merge
    'TN20010111 Start
    Public WriteOnly Property IsReInsuranceAtRiskLevel() As Integer
        Set(ByVal Value As Integer)
            m_lIsReInsuranceAtRiskLevel = Value
        End Set
    End Property
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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Set the focus
            Me.Activate()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SwitchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SwitchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            'm_lReturn& = RiskScreen1.CancelClick

            m_lReturn = uctRiskScreenControl.CancelClick()


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        '        m_lReturn& = ShowHelp(dlgHelp,objCM. ScreenHelpID)
        ' Click event of the Cancel button.

        Try

            ' Process the next set of actions depending
            ' upon the interface task etc.
            'RiskScreen1.ShowHelpScreen
            uctRiskScreenControl.ShowHelpScreen(cmdHelp,objCM. ScreenHelpID)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Help command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHelp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'm_lReturn& = RiskScreen1.OKClick
            m_lReturn = uctRiskScreenControl.OKClick()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'RiskScreen1.UnLoadControl


            'RiskScreen1.Terminate


            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'MSS280901 - Added for merge
    Private Sub cmdPremium_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPremium.Click
        m_lReturn = GetRiskRating()
    End Sub

    Private Sub cmdReInsurance_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReInsurance.Click
        m_lReturn = GetRiskReinsurance()
    End Sub

    Private Sub cmdRiskTax_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRiskTax.Click
        m_lReturn = GetRiskTax()
    End Sub
    'MSS280901 - Merge end

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmRiskUnderwriting_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim n As Integer = 0
        Dim nReturn As Integer = 0
        Dim nStatus As Integer = 0
        Dim sOption As String = ""
        ' Forms load event.
        Try
            If FromEvent Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            'With RiskScreen1
            With uctRiskScreenControl
                'Developer Guide No 24.  
                .Task = gPMConstants.PMEComponentAction.PMView
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0
                .ScreenId = m_lRiskGisScreenId
                .InsuranceFileCnt = m_lInsFileCnt
                'CT 29/11/00 new versions of screen control requires folder count
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .RiskTypeId = m_lRiskTypeId
                .RiskId = m_lRiskCodeId
                .FromEvent = m_bEvent
                .PartyCnt = m_lPartyCnt
                m_lReturn = .Initialise()

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Exit Sub
                End If
                m_lReturn = .LoadControl()
                m_lReturn = .GetRisk()
                nStatus = .Status
            End With

            Dim lToolBarOffset As Integer
            lToolBarOffset = 420
            Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(uctRiskScreenControl.Height) _
                                           + 1050 + lToolBarOffset) ' RAM20021023 : Added an Offset
            Me.Width = uctRiskScreenControl.Width + VB6.TwipsToPixelsX(315)

            cmdHelp.Top = Me.ClientRectangle.Height - (cmdHelp.Height + VB6.TwipsToPixelsY(100))
            cmdHelp.Left = Me.Width - VB6.TwipsToPixelsX(210) - cmdHelp.Width
            cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdHelp.Left) - _
                                                VB6.PixelsToTwipsX(cmdCancel.Width) - 80)
            cmdOK.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdCancel.Left) - _
                                            VB6.PixelsToTwipsX(cmdOK.Width) - 80)

            cmdCancel.Top = cmdHelp.Top
            cmdOK.Top = cmdHelp.Top
            cmdPremium.Top = cmdHelp.Top
            cmdRiskTax.Top = cmdHelp.Top
            cmdReInsurance.Top = cmdHelp.Top

            If FromEvent Then
                mnuPolicy.Text = "Event"
                mnuPolicyCopy.Available = False
                mnuPolicyMove.Available = False
                mnuGoTo.Available = False
                mnuDocumentation.Available = False
                mnuReports.Available = False
                mnuTasks.Available = False
            End If

            'show re-insurance button only if re-insurance is set at risk level
            cmdReInsurance.Visible = m_lIsReInsuranceAtRiskLevel = gPMConstants.PMEReturnCode.PMTrue

            If objCM.g_bHidePublicPrivateNotes Then
                mnuGoToNotes.Available = False
            End If

            'Alternative is to add a message box to iPMURITax
            Dim temp_m_oRiskTax As Object
            m_lReturn = objCM.g_oObjectManager.GetInstance(temp_m_oRiskTax, "bSIRRITax.Business", _
                                                           vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRiskTax = temp_m_oRiskTax
            m_oRiskTax.RiskCnt = m_lRiskCodeId
            m_oRiskTax.InsuranceFileCnt = m_lInsFileCnt
            m_lReturn = m_oRiskTax.GetRiskTax(r_vRiskTax:=m_vRITax, r_sDescription:=m_sDesc, iTask:=m_iTask)
            cmdRiskTax.Enabled = Information.IsArray(m_vRITax)
            mnuGotoTransaction.Available = False 'PN5049 do not show Transaction menu for underwriting
            mnuPolicyCopy.Available = False
            mnuPolicyMove.Available = False

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            'Sharepoint
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOption, _
                                                 v_iSourceID:=g_iSourceID)

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
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass,vMethod:="Form_Load", vErrNo:=Information.Err().Number,vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub


    Private Sub frmRiskUnderwriting_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
                ' m_lReturn& = RiskScreen1.CancelClick
                m_lReturn = uctRiskScreenControl.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            End If

            ' Terminate the control
            'm_lReturn& = RiskScreen1.Terminate()
            uctRiskScreenControl.Dispose()

            'Flag the document as closed with the MDI Form
            objcm.FState(Index).Deleted = True

            objcm.m_ofrmMDI.StatusBar1.Items.Item(0).Text = ""
            objcm.m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
            objcm.m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
            objcm.m_ofrmMDI.Text = "Sirius Client Manager"

            iCount = 0

            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If Application.OpenForms.Item(iLoop1).Name <> Me.Name Then
                    If Application.OpenForms.Item(iLoop1).Name <> objcm.m_ofrmMDI.Name Then
                        iCount += 1
                    End If
                End If
            Next iLoop1

            If iCount = 0 Then
                ' Update

                m_lReturn = objCM.g_oCMManager.ImEmpty(v_lPartyCnt:=m_lPartyCnt)
                m_lReturn = objCM.SetToolbar(v_sFormName:=objcm.m_ofrmMDI.Name)
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
    Private Sub frmRiskUnderwriting_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        If Me.WindowState <> FormWindowState.Minimized Then
            'Me.Height = 7000
            'Me.Width = 9435
        End If

    End Sub

    Private Sub frmRiskUnderwriting_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        '         Show the current form instance as deleted
        objCM.FState(CInt(Convert.ToString(Me.Tag))).Deleted = True

        '         Hide the toolbar edit buttons if no notepad windows exist.
        If Not objCM.AnyPadsLeft() Then

            objCM.gToolsHidden = True
            '             Call the recent file list procedure
            '        GetRecentFiles
        End If

    End Sub

    Public Sub mnuDiaryFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDiaryFind.Click

        m_lReturn = objcm.ShowTaskList(v_lPartyCnt:=PartyCnt, v_lInsuranceFileCnt:=InsFileCnt)

    End Sub

    Public Sub mnuDiaryNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDiaryNew.Click

        m_lReturn = objCM.ProcessTasks(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:=m_sResolvedName, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=InsFileCnt, v_sPolicyDesc:=m_sInsReference)
    End Sub

    Public Sub mnuDocumentationLetterWriting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentationLetterWriting.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_lRiskCnt:=m_lRiskCodeId)

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

        ' ND 181000
        ' Call Documaster link to open Documaster at policy level (2) for this client
        m_lReturn = objCM.ShowDocumaster(v_sLinkCode:=m_sInsReference & objCM.DME_POLICY)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Continue as not serious
            Exit Sub
        End If

    End Sub

    'MSS280901 - Added functions for merge
    ' ***************************************************************** '
    ' Name: GetRiskTax
    '
    ' Desc: get tax if required
    '
    ' Hist: 18/01/2001 Tinny - Created
    '
    ' ***************************************************************** '
    Private Function GetRiskTax() As Integer

        Dim result As Integer = 0

        Dim oObject As iPMUPerilAllocation.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oObject = New iPMUPerilAllocation.Interface_Renamed()
            'Developer Guide No.9
            m_lReturn = oObject.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            oObject.InsuranceFileCnt = m_lInsFileCnt
            oObject.RiskId = m_lRiskCodeId

            ' for showing tax tab by default
            oObject.DefaultTab = 2

            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = oObject.Status

            oObject.Dispose()


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTax", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRiskRating
    '
    ' Description:
    '
    ' History: 11/09/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GetRiskRating() As Integer

        Dim result As Integer = 0
        Dim oObject As iPMUPerilAllocation.Interface_Renamed


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'What happens here is that the Rating script is run, which populates the RSA_OUTPUT table.
            'We then call the Peril Allocation program, which takes that output and applies it

            'But until that's in place, we just do it manually...
            oObject = New iPMUPerilAllocation.Interface_Renamed()
            'Developer Guide No.9
            m_lReturn = oObject.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            oObject.InsuranceFileCnt = m_lInsFileCnt
            oObject.RiskId = m_lRiskCodeId

            m_lReturn = oObject.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lStatus = oObject.Status

            oObject.Dispose()

            oObject = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskRating Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskRating", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	'
	' Name: GetRiskReinsurance
	'
	' Description:
	'
	' History: 11/09/2000 Tomo - Created.
	'
	' ***************************************************************** '
	Private Function GetRiskReinsurance() As Integer
		Dim result As Integer = 0
		Dim iPMUReinsurance As Object
		

        Dim oObject As iPMUReinsurance.Interface_Renamed

		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'    If (m_iIsRiAtRiskLevel = 0) Then
			'        Exit Function
			'    End If
			
			Dim temp_oObject As Object
			m_lReturn = objCM.g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMUReinsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
			oObject = temp_oObject
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)
			

			oObject.InsuranceFileCnt = m_lInsFileCnt

			oObject.RiskID = m_lRiskCodeId
			

			m_lReturn = oObject.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			

			m_lStatus = oObject.Status
			

            oObject.Dispose()

			oObject = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskReinsurance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	Public Sub mnuClientExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuClientExit.Click
		
		cmdCancel.Focus()
		cmdCancel_Click(cmdCancel, New EventArgs())
		
	End Sub
	
	Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click
		
		m_lReturn = objCM.ShowSBOAbout()
		
	End Sub
	
	Public Sub mnuRecentFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRecentFile_0.Click, _mnuRecentFile_1.Click
		Dim Index As Integer = Array.IndexOf(mnuRecentFile, eventSender)
		
		m_lReturn = objcm.ShowRecentFile(iIndex:=Index, r_oForm:=Me)
		
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
