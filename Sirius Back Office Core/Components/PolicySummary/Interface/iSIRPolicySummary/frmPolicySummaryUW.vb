Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmPolicySummaryUW
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmMainSFU
	' File Name     : frmMainSFU.frm
	' Date          : 17-10-2002
	' Author        : Ram Chandrabose
	' Description   : Interface used to display the Policy Summary Details
	' Note          : This interface is Underwriting specific
	'
	' Edit History  :
	' RAM20021018   : Created
	' ***************************************************************** '
	
	Private Const ACClass As String = "frmPolicySummaryUW"
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_lPartyCnt As Integer
	Private m_sShortName As String = ""
	Private m_lInsuranceFolderCnt As Integer
	Private m_lInsuranceFileCnt As Integer
	Private m_sInsReference As String = ""
	
	
	Public WriteOnly Property InsuranceFolderCnt() As Integer
		Set(ByVal Value As Integer)
			
			m_lInsuranceFolderCnt = Value
			
		End Set
	End Property
	
	Public WriteOnly Property InsuranceFileCnt() As Integer
		Set(ByVal Value As Integer)
			
			m_lInsuranceFileCnt = Value
			
		End Set
	End Property
	
	Public WriteOnly Property InsReference() As String
		Set(ByVal Value As String)
			
			m_sInsReference = Value
			
		End Set
	End Property
	
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			
			m_lPartyCnt = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ShortName() As String
		Set(ByVal Value As String)
			
			m_sShortName = Value
			
		End Set
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		' Since we don't need to do anything other than hide it
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		' Since we don't need to do anything other than hide it
		Me.Hide()
	End Sub
	

	Private Sub frmPolicySummaryUW_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Me.Text = "Summary : [" & m_sInsReference.Trim() & "]"
		
		With uctPMUPolicySummary1

            'developer guide no. 24
            .Task = gPMConstants.PMEComponentAction.PMView
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
		End With
		
	End Sub

    Private Sub frmPolicySummaryUW_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctPMUPolicySummary1.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub
End Class