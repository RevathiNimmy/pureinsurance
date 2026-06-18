Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmGIIMRisk
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmGIIMRisk
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
	Private Const ACClass As String = "frmGIIMRisk"
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
	
	Private m_bEvent As Boolean
	
	Private m_bEventRaised As Boolean
	
	'TN20000809
	Private m_bPMRaiseEvent As Boolean 'set to true to create event
	Private m_lPMRaiseEventState As Integer 'create event now or latter in parent object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
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
	Public Property InsReference() As String
		Get
			
			Return m_sInsReference
			
		End Get
		Set(ByVal Value As String)
			
			m_sInsReference = Value
			
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
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		Try 
			
			' Set the interface status.
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			
			' Process the next set of actions depending
			' upon the interface task etc.
			m_lReturn = uctGIIMRiskDataControl1.CancelClick()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			' Everything OK, so we can hide the interface.
			Me.Close()
		
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
			uctGIIMRiskDataControl1.ShowHelpScreen()
			
			' Check the return value.
		
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
			
			m_lReturn = uctGIIMRiskDataControl1.OKClick()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			' Check the return value.
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				' Everything OK, so we can hide the interface.
				'Me.Hide
				Me.Close()
				
			End If
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmGIIMRisk_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = Me.Footer
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = Me.InsReference
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = Me.ShortName
            '    objCM.m_ofrmMDI.Caption = "[" & Trim$(Me.ResolvedName) & "] Policy Master Client Manager"
            '    Me.Caption = "Policy : [" & Trim$(Me.ResolvedName) & "]"
            '    objCM.m_ofrmMDI.Caption = "[" & Trim$(Me.ShortName) & "] Policy Master Client Manager"
            '    Me.Caption = "Policy : [" & Trim$(Me.ResolvedName) & "]"
            objCM.m_ofrmMDI.Text = "[" & Me.ShortName.Trim() & " " & Me.InsReference.Trim() & "] Sirius Client Manager"
            Me.Text = "Risk for Policy : [" & Me.ShortName.Trim() & " " & Me.InsReference.Trim() & "]"


            m_lReturn = CType(objCM.SetToolbar(v_sFormName:=Me.Name), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Problem Setting Toolbar keys", Application.ProductName)
            End If

            objCM.m_ofrmMDI.InsFileCnt = Me.InsFileCnt
            objCM.m_ofrmMDI.InsuranceFolderCnt = Me.InsuranceFolderCnt
            objCM.m_ofrmMDI.InsReference = Me.InsReference
            objCM.m_ofrmMDI.PolicyTypeId = PMBConst.PMBPolicyTypeGeneral
            objCM.m_ofrmMDI.GeminiPolicyStatus = 0

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


    Private Sub frmGIIMRisk_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim i As Integer
        Dim lReturn, lStatus As Integer

        ' Forms load event.
        Try

            '    If (Me.InsFileCnt <> 0) Then
            '        m_iTask = PMEdit
            '    Else
            '        m_iTask = PMAdd
            '    End If

            If FromEvent Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If

            Me.Height = VB6.TwipsToPixelsY(6480)
            Me.Width = VB6.TwipsToPixelsX(9345)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            With uctGIIMRiskDataControl1
                .Task = m_iTask
                .Status = gPMConstants.PMEReturnCode.PMTrue
                .TransactionType = ""
                .EffectiveDate = DateTime.Today
                .ProcessMode = 0

                .Visible = True
                .Top = 0

                .PartyCnt = m_lPartyCnt
                .InsuranceFolderCnt = m_lInsuranceFolderCnt
                .InsuranceFileCnt = m_lInsFileCnt
                .RiskGroupId = m_lRiskGroupId
                '.FromEvent = m_bEvent
                'TN20000809        .EventRaised = m_bEventRaised
                '.PMRaiseEvent = PMRaiseEvent
                '.PMRaiseEventState = PMRaiseEventState

                m_lReturn = .Initialise()

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Exit Sub
                End If

                m_lReturn = .LoadControl()
                m_lReturn = .GetRiskData()
                lStatus = .Status

            End With

            If FromEvent Then
                mnuPolicy.Text = "Event"
                mnuPolicyCopy.Available = False
                mnuPolicyMove.Available = False
                mnuGoTo.Available = False
                mnuDocumentation.Available = False
                mnuReports.Available = False
                mnuTasks.Available = False
            End If

            ' CTAF 170801 - Use objCM.LoadRecentFiles
            m_lReturn = CType(objCM.LoadRecentFiles(r_oForm:=Me), gPMConstants.PMEReturnCode)

            'sj 04/10/2002 - start
            If objCM.g_bHidePublicPrivateNotes Then
                mnuGoToNotes.Available = False
            End If

            'Thinh Nguyen 27/06/2003 (start) - PN5049 do not show Transaction menu for underwriting
            mnuGotoTransaction.Available = False
            'Thinh Nguyen 27/06/2003 (start) - PN5049 do not show Transaction menu for underwriting

            'sj 04/10/2002 - end
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmGIIMRisk_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
                m_lReturn = uctGIIMRiskDataControl1.CancelClick()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    eventArgs.Cancel = True
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

            End If

            ' Terminate the control
            uctGIIMRiskDataControl1.Dispose()

            'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True

            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = ""
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
            objCM.m_ofrmMDI.Text = "Sirius Client Manager"

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
	Private Sub frmGIIMRisk_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Me.WindowState <> FormWindowState.Minimized Then
			Me.Height = VB6.TwipsToPixelsY(6240)
			Me.Width = VB6.TwipsToPixelsX(9435)
		End If
		
	End Sub
	
	Private Sub frmGIIMRisk_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
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
		
        m_lReturn = CType(objCM.ShowTaskList(v_lPartyCnt:=PartyCnt), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Public Sub mnuDiaryNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDiaryNew.Click
		
        m_lReturn = CType(objCM.ProcessTasks(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:=m_sResolvedName, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=InsFileCnt, v_sPolicyDesc:=m_sInsReference), gPMConstants.PMEReturnCode)
	End Sub
	
	' PRIVATE Property Procedures (End)
	
	Public Sub mnuDocumentationLetterWriting_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentationLetterWriting.Click
		
		Try 
			
			' Call Toolbar Control function
            m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=objCM.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt, v_lInsuranceFileCnt:=m_lInsFileCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuGotoAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoAccounts.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGotoAccounts, v_sShortName:=m_sShortName), gPMConstants.PMEReturnCode)
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

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.ProcessToolbar(v_iButton:=ACIButtonClaim), gPMConstants.PMEReturnCode)

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
        m_lReturn = CType(objCM.ShowDocumaster(v_sLinkCode:=m_sInsReference & objCM.DME_POLICY), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Continue as not serious
            Exit Sub
        End If


    End Sub

    Public Sub mnuGoToEvents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToEvents.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.ShowEvents(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:=m_sPartyType, v_sResolvedName:=m_sResolvedName, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.12)
    Public Sub mnuGotoInsuredAccounts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoInsuredAccounts.Click

        Const kMethodName As String = "mnuGotoInsuredAccounts_Click"

        m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=objCM.ACIGotoInsuredAccounts, v_sShortName:=m_sShortName), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "ProcessOrionFunc Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
    End Sub
    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.12)

    Public Sub mnuGoToNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToNotes.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.CallNotes(v_sEntityType:=gSIRLibrary.SIREntityNamePolicy, v_lEntityCnt:=m_lInsFileCnt, v_sTextType:="Public"), gPMConstants.PMEReturnCode)

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

        Dim vRiskCodeId As Integer
        Dim vRiskGroupId, lRiskCodeId, lRiskGroupId As Integer

        Try

            vRiskCodeId = m_lRiskCodeId
            vRiskGroupId = m_lRiskGroupId


            If Convert.IsDBNull(vRiskCodeId) Or IsNothing(vRiskCodeId) Then
                lRiskCodeId = 0
            Else
                lRiskCodeId = vRiskCodeId
            End If


            If Convert.IsDBNull(vRiskGroupId) Or IsNothing(vRiskGroupId) Then
                lRiskGroupId = 0
            Else
                lRiskGroupId = vRiskGroupId
            End If

            ' Call Toolbar Control function
            m_lReturn = CType(objCM.ShowTextFiles(v_lPartyCnt:=m_lPartyCnt, v_sShortName:=m_sShortName, v_sPartyType:="X", v_sResolvedName:="Resolved", v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsFileCnt, v_sPolicyDesc:=m_sInsReference, v_lRiskCodeId:=lRiskCodeId, v_lRiskGroupID:=lRiskGroupId), gPMConstants.PMEReturnCode)

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionCash menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionCash_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    'EK 22/9/99
    Public Sub mnuGotoTransactionDebit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionDebit.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionDebit, v_lInsuranceFileCnt:=m_lInsFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionDebit menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionDebit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Public Sub mnuGotoTransactionCredit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGotoTransactionCredit.Click

        Try

            ' Call OrionLinkFunc function
            m_lReturn = CType(objCM.ProcessOrionFunc(v_iButton:=ACIGoToTransactionCredit, v_lInsuranceFileCnt:=m_lInsFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the mnuGotoTransactionCredit menu item.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuGotoTransactionCredit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	
	Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click
		
        m_lReturn = CType(objCM.ShowSBOAbout(), gPMConstants.PMEReturnCode)
		
	End Sub
End Class
