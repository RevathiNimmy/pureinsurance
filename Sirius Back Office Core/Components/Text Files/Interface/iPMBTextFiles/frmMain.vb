Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmMain"
	
	Private m_lPartyCnt As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_lInsuranceFolderCnt As Integer
	Private m_iTask As Integer
	Private m_lReturn As Integer
	Private m_lStatus As Integer
	'DC250101
	Private m_lRiskCodeId As Integer
	Private m_lRiskGroupId As Integer
	'DC290702 -start
	Private m_lClaimCnt As Integer
	Private m_sClaimRef As String = ""
	'DC290702 -end
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
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
	
	Public Property InsuranceFolderCnt() As Integer
		Get
			Return m_lInsuranceFolderCnt
		End Get
		Set(ByVal Value As Integer)
			m_lInsuranceFolderCnt = Value
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
	
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	'DC250101
	Public Property RiskCodeId() As Integer
		Get
			Return m_lRiskCodeId
		End Get
		Set(ByVal Value As Integer)
			m_lRiskCodeId = Value
		End Set
	End Property
	'DC250101
	Public Property RiskGroupId() As Integer
		Get
			Return m_lRiskGroupId
		End Get
		Set(ByVal Value As Integer)
			m_lRiskGroupId = Value
		End Set
	End Property
	
	'DC290702 -start
	Public Property ClaimCnt() As Integer
		Get
			Return m_lClaimCnt
		End Get
		Set(ByVal Value As Integer)
			m_lClaimCnt = Value
		End Set
	End Property
	
	Public Property ClaimRef() As String
		Get
			Return m_sClaimRef
		End Get
		Set(ByVal Value As String)
			m_sClaimRef = Value
		End Set
	End Property
	'DC290702 -end
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Set the status to Cancelled
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		m_lReturn = uctTextFiles.CancelClick()
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		m_lReturn = uctTextFiles.DeleteClick()
		
		'Refresh the list after editting
		m_lReturn = uctTextFiles.GetTextFiles()
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		cmdEdit.Enabled = False
		m_lReturn = uctTextFiles.EditClick()
		cmdEdit.Enabled = True
		'DJM 02/05/2002 : Refresh the list after editting
		m_lReturn = uctTextFiles.GetTextFiles()
		
	End Sub
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		
		m_lReturn = uctTextFiles.ShowHelpScreen()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Set the status to OK
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		m_lReturn = uctTextFiles.OKClick()
		
		Me.Hide()
		
	End Sub
	

	Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Try 
			
			' Set the controls properties
			With uctTextFiles
				.Task = m_iTask
				.PartyCnt = m_lPartyCnt
				.InsuranceFileCnt = m_lInsuranceFileCnt
				.InsuranceFolderCnt = m_lInsuranceFolderCnt
				'DC250101
				.RiskCodeId = m_lRiskCodeId
				.RiskGroupId = m_lRiskGroupId
				'DC290702
				.ClaimCnt = m_lClaimCnt
				.ClaimDesc = m_sClaimRef
			End With
			
			' Initialise it
			m_lReturn = CType(uctTextFiles, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			' Load it
			m_lReturn = uctTextFiles.LoadControl()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			' Let it go off and get the details
			m_lReturn = uctTextFiles.GetTextFiles()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		Try 
			
			m_lReturn = uctTextFiles.UnLoadControl(Cancel:=Cancel, UnloadMode:=UnloadMode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
            uctTextFiles.Dispose()
        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_QueryUnload Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	
    Private Sub uctTextFiles_lvwSearchDetailsClick(ByVal Sender As Object, ByVal e As uctTextFilesControl.uctTextFiles.lvwSearchDetailsClickEventArgs) Handles uctTextFiles.lvwSearchDetailsClick


        ' Check if we should enable the edit button
        If e.lFileNumber > 0 Then
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        ElseIf (e.lFileNumber = 0) Then
            cmdEdit.Enabled = True
            cmdDelete.Enabled = False
        Else
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
        End If

    End Sub
End Class
