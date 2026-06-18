Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmSBORiskScreen
	Inherits System.Windows.Forms.Form
	Private Sub frmSBORiskScreen_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	
	' ***************************************************************** '
	' Form Name: frmSBORiskScreen
	'
	' Date:
	'
	' Description: Copied from frmRiskUnderwriting and then modified
	'
	' Edit History:
	' CJB 210905 PN24176 Changed Form_Load to prepare KeyArray to set in
	'            uctRiskScreen (which will get passed to Dynamic Logic scripts)
	'            We need this (in particular m_lRiskCodeId) in order to show
	'            Stargate risk screens via BackOffice and have logic work correctly.
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmSBORiskScreen"
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
	'eck070301
	Private m_lRiskCnt As Integer
	
	Private m_bEvent As Boolean
	
	Private m_bEventRaised As Boolean
	
	'TN20000809
	Private m_bPMRaiseEvent As Boolean 'set to true to create event
	Private m_lPMRaiseEventState As Integer 'create event now or latter in parent object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	'ISS1498 JAS 05/12/02 PolicyTypeId to determine whether OK button should be disabled
	Private m_lPolicyTypeID As Integer

    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property
	
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
	
	
	Public WriteOnly Property PolicyTypeId() As Integer
		Set(ByVal Value As Integer)
			'ISS1498   JAS 05/12/02 - to disable OK button if policy is scheme
			m_lPolicyTypeID = Value
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
	'eck070301
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
			m_lReturn = RiskScreen1.CancelClick()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
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
			RiskScreen1.ShowHelpScreen()
		
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
			
			m_lReturn = RiskScreen1.OKClick()
			'eck140301
			RiskCnt = RiskScreen1.RiskId
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmSBORiskScreen_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim i As Integer
		Dim lReturn, lStatus As Integer
		Dim vValue As String = ""
        Dim vKeyArray(,) As Object
		
		' Forms load event.
		Try 
			
			'ISS1498 JAS 11/12/02 Enable Editing of Risk Screen
			m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableRiskScreenEditing, gPMConstants.SIRBCHHeadOffice, vValue)
			
			
			If FromEvent Then
				m_iTask = gPMConstants.PMEComponentAction.PMView
			End If
			
			Me.Height = VB6.TwipsToPixelsY(7000)
			Me.Width = VB6.TwipsToPixelsX(9345)
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Prepare KeyArray to set in uctRiskScreen (which will get passed to Dynamic Logic scripts) PN24176
			' We need this (in particular m_lRiskCodeId) in order to show Stargate risk screens via BackOffice and
			' have logic work correctly.
			ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4)
			

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNamePolicyKey

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lInsFileCnt
			

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameRiskCodeID

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lRiskCodeId
			

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNamePartyCnt

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lPartyCnt
			

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameInsuranceFileCnt

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_lInsFileCnt
			

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.PMKeyNameInsuranceFolderCnt

			vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lInsuranceFolderCnt
			
			With RiskScreen1


                .KeyArray = vKeyArray 'PN24176
                .Task = m_iTask
                .Status = gPMConstants.PMEReturnCode.PMTrue
				.TransactionType = ""
				.EffectiveDate = DateTime.Today
				.ProcessMode = 0
				
				'sj 19/08/2002 - start
				'.Visible = True
				'.Top = 0
				'sj 19/08/2002 - end
				
				.ScreenId = m_lRiskGisScreenId
				.InsuranceFileCnt = m_lInsFileCnt
				'CT 29/11/00 new versions of screen control requires folder count
				.InsuranceFolderCnt = m_lInsuranceFolderCnt
				
				'.RiskTypeId = m_lRiskTypeId
				.RiskTypeId = m_lRiskTypeId
				'eck070301
				'        .RiskId = 1
				.RiskId = m_lRiskCnt
				'
				.FromEvent = m_bEvent
				
				m_lReturn = .Initialise()
				
				If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
					Exit Sub
				End If
				
				m_lReturn = .LoadControl()
				m_lReturn = .GetRisk()
				lStatus = .Status
				
			End With
			
			cmdOK.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(RiskScreen1.Top) + VB6.PixelsToTwipsY(RiskScreen1.Height) + 135)
			cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(RiskScreen1.Top) + VB6.PixelsToTwipsY(RiskScreen1.Height) + 135)
			cmdHelp.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(RiskScreen1.Top) + VB6.PixelsToTwipsY(RiskScreen1.Height) + 135)
			
			cmdHelp.Left = RiskScreen1.Left + RiskScreen1.Width - cmdHelp.Width
			cmdCancel.Left = cmdHelp.Left - VB6.TwipsToPixelsX(1200)
			cmdOK.Left = cmdCancel.Left - VB6.TwipsToPixelsX(1200)
			
			Me.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(RiskScreen1.Top) + VB6.PixelsToTwipsY(RiskScreen1.Height) + 885)
			Me.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(RiskScreen1.Left) + VB6.PixelsToTwipsX(RiskScreen1.Width) + 210)
			
			If FromEvent Then
				mnuPolicy.Text = "Event"
				mnuPolicyCopy.Available = False
				mnuPolicyMove.Available = False
				mnuGoTo.Available = False
				mnuDocumentation.Available = False
				mnuReports.Available = False
				mnuTasks.Available = False
			End If
			
            If objCM.g_bRestrictInsurerAccess Then
                mnuPolicyCopy.Available = False
                mnuPolicyMove.Available = False
                mnuPolicyDelete.Available = False
                mnuGotoAccounts.Available = False
                mnuGotoTransaction.Available = False
                mnuGoToClaim.Available = False
                mnuGoToDocumaster.Available = False
                mnuGoToSwift.Available = False
                mnuGoToTextFiles.Available = False
                mnuReports.Available = False
                mnuTasks.Available = False
                mnuDocumentation.Available = False
            End If
			
			'ISS1498 - JAS 05/12/02 if policy type is schemes then disable OK button
			'ISS1498 - JAS 11/12/02 added product option vValue
			If m_lPolicyTypeID = PMBConst.PMBPolicyTypeSchemes And vValue <> "1" Then
				cmdOK.Enabled = False
			End If
			'ISS1498 - end
			
			mnuGotoTransaction.Available = False 'PN5049 do not show Transaction menu for underwriting
			mnuPolicyCopy.Available = False
			mnuPolicyMove.Available = False
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub


		End Try
		
	End Sub
	
	
	Private Sub frmSBORiskScreen_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		Dim iCount As Integer
		Dim lPolicyCount As Integer
		
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
				m_lReturn = RiskScreen1.CancelClick()
				
				' Check the return value.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Cancel = 1
                    eventArgs.cancel = True
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					Exit Sub
				End If
				
			End If
			
			'gp20020215 - ensure the risk id is passed back
			m_lRiskCnt = RiskScreen1.RiskId
			
			' Terminate the control
            RiskScreen1.Dispose()
			
			'Flag the document as closed with the MDI Form
            objCM.FState(Index).Deleted = True
			
            objCM.m_ofrmMDI.StatusBar1.Items.Item(0).Text = ""
            objCM.m_ofrmMDI.StatusBar1.Items.Item(1).Text = ""
            objCM.m_ofrmMDI.StatusBar1.Items.Item(2).Text = ""
            objCM.m_ofrmMDI.Text = "Sirius Client Manager"
            'eck140301
            objCM.m_ofrmMDI.RiskCnt = RiskCnt

            lPolicyCount = 0

            'gp20020215 - ensure the risk id is passed back
            For iLoop1 As Integer = 0 To Application.OpenForms.Count - 1
                If (Application.OpenForms.Item(iLoop1).Name = "frmPolicy") Or (Application.OpenForms.Item(iLoop1).Name = "frmPolicySummary") Then
                    'gp20020311 - check against the InsuranceFolderCnt
                    'vRiskCodeId = ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "InsuranceFolderCnt")
                    If ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "InsuranceFolderCnt") = InsuranceFolderCnt Then
                        ReflectionHelper.SetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicyControl1"), "RiskCnt", RiskCnt)
                        lPolicyCount += 1
                    End If
                    'gp20020228 - extra checks with the risk id
                ElseIf (Application.OpenForms.Item(iLoop1).Name = "frmPolicySummary") Then
                    'gp20020311 - check against the InsuranceFolderCnt
                    If ReflectionHelper.GetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "InsuranceFolderCnt") = InsuranceFolderCnt Then
                        ReflectionHelper.SetMember(ReflectionHelper.GetMember(Application.OpenForms.Item(iLoop1), "uctPolicySummControl1"), "RiskCnt", RiskCnt)
                    End If
                End If
            Next iLoop1

            'DJM 02/12/2003 : If no frmpolicy screens open then ensure that policy is unlocked.
            If lPolicyCount = 0 Then
                'We need to unlock the policy
                m_lReturn = objCM.UnlockThePolicy(v_lInsuranceFileCnt:=m_lInsFileCnt)
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
                'sj 03/07/2002 - start
                If objCM.g_bRestrictInsurerAccess Then
                    m_lReturn = objCM.SetRestrictedToolbar(v_sFormName:=objCM.m_ofrmMDI.Name)
                Else
                    m_lReturn = objCM.SetToolbar(v_sFormName:=objCM.m_ofrmMDI.Name)
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
	Private Sub frmSBORiskScreen_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		'MKW080503 PN3967 START - Removed line to resize to default
		'If (Me.WindowState <> vbMinimized) Then
		'    Me.Height = 7000
		'    Me.Width = 9435
		'End If
		'MKW080503 PN3967 END
	End Sub
	
	Private Sub frmSBORiskScreen_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
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
	
	Public Sub mnuGoToDocumaster_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuGoToDocumaster.Click
		' CTAF 030300
		'MsgBox "This functionality is yet to be implemented.", vbInformation, "Documaster"
		
		' ND 181000
		' Call Documaster link to open Documaster at policy level (2) for this client
        m_lReturn = objCM.ShowDocumaster(v_sLinkCode:=m_sInsReference & objcm.DME_POLICY)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			'Continue as not serious
			Exit Sub
		End If
		
	End Sub
End Class
