Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmClaimVersions
	Inherits System.Windows.Forms.Form
	Private Sub frmClaimVersions_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	' ***************************************************************** '
	' Form Name: frmClaimsVersions
	'
	' Date:
	'
	' Description: Main interface form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "frmClaimVersions"
	
	
	' General Property variables
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lPMAuthorityLevel As Integer
	Private m_bError As Integer
	Private m_lReturn As Integer
	Private m_bInterfaceError As Boolean
	
	' process modes
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_iTask As gPMConstants.PMEComponentAction
	
	' return parameters
	Private m_lRiskCnt As Integer
	Private m_lClaimId As Integer
	Private m_sClaimNumber As String = ""
	Private m_lInsuranceFilecnt As Integer
	Private m_sInsuranceRef As String = ""
	Private m_sInsuranceHolder As String = ""
	Private m_lSelectedClaimId As Integer
	'PN 58569
	Private m_bRecovery As Boolean
	Public ReadOnly Property Recovery() As Boolean
		Get
			Return m_bRecovery
		End Get
	End Property
	
	
	Public Property ClaimId() As Integer
		Get
			Return m_lClaimId
		End Get
		Set(ByVal Value As Integer)
			m_lClaimId = Value
		End Set
	End Property
	Public Property ClaimNumber() As String
		Get
			Return m_sClaimNumber
		End Get
		Set(ByVal Value As String)
			m_sClaimNumber = Value
		End Set
	End Property
	
	Public ReadOnly Property RiskCnt() As Integer
		Get
			Return m_lRiskCnt
		End Get
	End Property
	Public ReadOnly Property InsuranceFileCnt() As Integer
		Get
			Return m_lInsuranceFilecnt
		End Get
	End Property
	Public ReadOnly Property InsuranceRef() As String
		Get
			Return m_sInsuranceRef
		End Get
	End Property
	Public ReadOnly Property InsuranceHolder() As String
		Get
			Return m_sInsuranceHolder
		End Get
	End Property
	
	Public Property SelectedClaimId() As Integer
		Get
			Return m_lSelectedClaimId
		End Get
		Set(ByVal Value As Integer)
			m_lSelectedClaimId = Value
		End Set
	End Property
	
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			m_lNavigate = Value
		End Set
	End Property
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			m_lProcessMode = Value
		End Set
	End Property
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	Public WriteOnly Property Task() As Integer
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public WriteOnly Property Claim() As Integer
		Set(ByVal Value As Integer)
			m_lClaimId = Value
		End Set
	End Property
	
	'********************************
	' General Interface Properties
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			m_sCallingAppName = Value
		End Set
	End Property
	Public WriteOnly Property PMAuthorityLevel() As Integer
		Set(ByVal Value As Integer)
			m_lPMAuthorityLevel = Value
		End Set
	End Property
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	Public ReadOnly Property Error_Renamed() As Integer
		Get
			Return m_bError
		End Get
	End Property
	'********************************
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		Me.Hide()
	End Sub
	
	Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
		ActionOk()
	End Sub
	
	' ***************************************************************** '
	' Name: Form_Initialize
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Sub Form_Initialize_Renamed()
		
		Const kMethodName As String = "Form_Initialize"
		
		Dim lReturn, lSubValue As Integer
		
		Try
		
		
		
		' Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		' initialise form error indicator
		m_bError = False
		
		' Set the interface status to cancelled. This is done
		' so that any interface termination will be noted
		' as cancelled except in the event of accepting
		' the interface.
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		End Try
	End Sub
	
	' ***************************************************************** '
	' Name: Form_QueryUnload
	'
	' Description: Determines whether any actions need to take place
	'               before unload.
	' History:
	'           Created : MEvans : Date : Process Id
	' ***************************************************************** '
	Private Sub frmClaimVersions_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
	
	' ***************************************************************** '
	' Name: Form_Unload
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Sub frmClaimVersions_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		Const kMethodName As String = "Form_Unload"
		
		Dim lReturn, lSubValue As Integer
		
		Try
		
		
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
	
		End Try
	End Sub
	
	' ***************************************************************** '
	' Name: Form_Load
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 14-03-2006 : Claims Versioning Changes
	' ***************************************************************** '

	Public Sub frmClaimVersions_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Const kMethodName As String = "Form_Load"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim lSubValue As Integer
		
		Try
		
		
		
		' set up interface
		lReturn = SetupForm()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "SetupForm Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
	
		End Try
	End Sub
	
	' ***************************************************************** '
	' Name: SetupForm
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 14-03-2006 : Claims Versioning Changes
	' ***************************************************************** '
	Public Function SetupForm() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SetupForm"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
        ' initialise claims versions user control
        'Developer Guide no. 9
        lReturn = uctCLMVersions.Initialise()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMVersions.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' set process modes
		lReturn = uctCLMVersions.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMVersions.SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' set base claim id
		uctCLMVersions.ClaimId = m_lClaimId
		uctCLMVersions.SelectedClaimId = m_lSelectedClaimId
		uctCLMVersions.ClaimNumber = m_sClaimNumber

        lReturn = uctCLMVersions.Load_Renamed()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "uctCLMVersions.Load", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: ActionOk
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 14-03-2006 : Claims Versioning Changes
	' ***************************************************************** '
	Public Function ActionOk() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ActionOk"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' get the selected claims details
		lReturn = uctCLMVersions.GetSelectedClaimsDetails(r_lClaimId:=m_lClaimId, r_lInsuranceFileCnt:=m_lInsuranceFilecnt, r_sClaimNumber:=m_sClaimNumber, r_sInsuranceRef:=m_sInsuranceRef, r_lRiskCnt:=m_lRiskCnt, r_sInsuranceHolderShortname:=m_sInsuranceHolder, r_bRecovery:=m_bRecovery)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetSelectedClaimsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' if a claim version was selected
		If m_lClaimId <> 0 And m_lInsuranceFilecnt <> 0 Then
			' return ok and hide the form
			m_lStatus = gPMConstants.PMEReturnCode.PMOK
			Me.Hide()
		Else
			' otherwise indicate to the user he must select a claim version
			MessageBox.Show("You must select a claim", "Claim Version Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	Private Sub uctCLMVersions_DblClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctCLMVersions.DblClick
		cmdOk_Click(cmdOk, New EventArgs())
	End Sub
End Class
